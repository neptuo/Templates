using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Neptuo.Web.Framework.Parser.HtmlContent
{
    public class StateMachine
    {
        public StateMachineResult Process(string content, string tagNamespace, string tagName)
        {
            HtmlTag result = new HtmlTag();
            result.Namespace = tagNamespace;
            result.Name = tagName;

            State state = new StartTagNameState();
            state.HtmlTag = result;

            int lastIndex = 0;
            foreach (char input in content)
            {
                lastIndex++;
                state = state.Process(input);
                if (state.GetType() == typeof(DoneState))
                    break;

                if (state.GetType() == typeof(ErrorState))
                    throw new StateMachineException();

            }

            return new StateMachineResult
            {
                HtmlTag = result,
                LastIndex = lastIndex
            };
        }
    }

    public abstract class State
    {
        public HtmlTag HtmlTag { get; set; }

        public string Stack { get; set; }

        public Dictionary<string, string> Context { get; set; }

        public State()
        {
            Context = new Dictionary<string, string>();
        }

        public abstract State Process(char input);

        #region Factory

        public T Create<T>(string name1 = null, string value1 = null, string name2 = null, string value2 = null)
            where T : State, new()
        {
            T result = new T();
            result.HtmlTag = HtmlTag;
            result.Stack = "";

            if (name1 != null)
                result.Context.Add(name1, value1);

            if (name2 != null)
                result.Context.Add(name2, value2);

            return result;
        }

        #endregion
    }

    public class StartTagNameState : State
    {
        public override State Process(char input)
        {
            if (input == ' ')
                return Create<StartAttributeState>();

            if (input == '/')
                return Create<ClosingState>();

            if (input == '>')
                return Create<TagBodyState>();

            Stack += input;
            return this;
        }
    }

    public class StartAttributeState : State
    {
        public override State Process(char input)
        {
            if (input == '=')
                return Create<AttributeSeparatorState>("attribute", Stack);

            if (input == '/')
                return Create<ClosingState>();

            if (input == '>')
                return Create<TagBodyState>();

            if (input == ' ')
                return this;

            Stack += input;
            return this;
        }
    }

    public class AttributeSeparatorState : State
    {
        public override State Process(char input)
        {
            if (input == '"' || input == '\'')
                return Create<AttributeValueState>("attribute", Context["attribute"], "separator", input.ToString());

            return Create<ErrorState>();
        }
    }

    public class AttributeValueState : State
    {
        public override State Process(char input)
        {
            if (input.ToString() == Context["separator"])
            {
                HtmlTag.Attributes.Add(new HtmlAttribute(Context["attribute"], Stack));
                return Create<AfterAttributeState>();
            }

            Stack += input;
            return this;
        }
    }

    public class AfterAttributeState : State
    {
        public override State Process(char input)
        {
            if (input == '/')
                return Create<ClosingState>();

            if(input == ' ')
                return Create<StartAttributeState>();

            if (input == '>')
                return Create<TagBodyState>();


            return Create<ErrorState>();
        }
    }

    public class ClosingState : State
    {
        public override State Process(char input)
        {
            if (input == '>')
                return Create<DoneState>();

            return Create<ErrorState>();
        }
    }

    public class TagBodyState : State
    {
        private int InnerSameTags = 0;

        public override State Process(char input)
        {
            Stack += input;

            string endTag = "</" + HtmlTag.Fullname.ToLowerInvariant() + ">";
            if (Stack.ToLowerInvariant().EndsWith(endTag))
            {
                if (InnerSameTags == 0)
                {
                    HtmlTag.Content = Stack.Substring(0, Stack.Length - endTag.Length);
                    return Create<DoneState>();
                }

                InnerSameTags--;
                //Stack = "";
            }
            else if (Stack.ToLowerInvariant().EndsWith("<" + HtmlTag.Fullname.ToLowerInvariant() + ">")
                || Stack.ToLowerInvariant().EndsWith("<" + HtmlTag.Fullname.ToLowerInvariant() + " "))
            {
                InnerSameTags++;
                //Stack = "";
            }

            return this;
        }
    }


    public class DoneState : State
    {
        public override State Process(char input)
        {
            Stack += input;
            return this;
        }
    }

    public class ErrorState : State
    {
        public override State Process(char input)
        {
            Stack += input;
            return this;
        }
    }

}
