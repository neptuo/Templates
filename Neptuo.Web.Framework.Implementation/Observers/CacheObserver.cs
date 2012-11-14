using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace Neptuo.Web.Framework.Observers
{
    public class CacheObserver : IObserver
    {
        private static Dictionary<string, string> cache = new Dictionary<string, string>();

        public string Key { get; set; }

        public TimeSpan? Duration { get; set; }

        public void OnInit(ObserverEventArgs e)
        { }

        public void Render(ObserverEventArgs e, HtmlTextWriter writer)
        {
            if (Key == null || !cache.ContainsKey(Key))
            {
                StringWriter cacheWriter = new StringWriter();
                e.Target.Render(new HtmlTextWriter(cacheWriter));

                if (Key != null)
                    cache[Key] = cacheWriter.ToString();

                writer.Write(cacheWriter.ToString());
            }
            else
            {
                writer.Write(cache[Key]);
            }
        }
    }
}
