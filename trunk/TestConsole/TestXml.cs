using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;

namespace TestConsole
{
    class TestXml
    {
        public static void Test()
        {
            XmlDocument document = new XmlDocument();
            document.Load("Index.html");

            //Console.WriteLine(document);

            Print(document.DocumentElement, 0);
        }

        static void Print(XmlNode parent, int indent)
        {
            for (int i = 0; i < indent; i++)
                Console.Write("  ");

            PrintElement(parent);

            foreach (XmlNode node in parent.ChildNodes)
                Print(node, indent + 1);
        }

        static void PrintElement(XmlNode node)
        {
            if (node.GetType() == typeof(XmlText))
                Console.WriteLine(((XmlText)node).InnerText.Trim());
            else if(node.GetType() == typeof(XmlElement))
                Console.WriteLine("<{0} {1}", ((XmlElement)node).Name, ((XmlElement)node).IsEmpty);
            else
                Console.WriteLine(node);
        }
    }
}
