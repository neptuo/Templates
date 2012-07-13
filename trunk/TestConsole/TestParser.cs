using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Neptuo.Web.Framework.Parser;
using Neptuo.Web.Framework.Parser.HtmlContent;
using Neptuo.Web.Framework.Parser.ExtensionContent;
using System.Text.RegularExpressions;

namespace TestConsole
{
    class TestParser
    {
        static string basicMarkup = @"<asp:Label x:Name=""head"" text=""hello"" runat=""server"" ui:visible='false' />";

        static string basicMarkup2 = @"<asp:Label x:Name=""head"" runat=""server"" ui:visible='false'>Hello, World!</asp:Label>";

        static string basicMarkup3 = @"
            <div>hello</div>
            <ui:Template Path='~/Templates/Site.html'>
                <asp:Label x:Name=""head"" runat=""server"" ui:visible='false' />

                <ui:Template Path='~/Templates/Site.html'>
                    <asp:Label2 x:Name=""head"" runat=""server"" ui:visible='false' />
                </ui:Template>
            </ui:Template>
        ";

        static string markup = @"
            <!DOCTYPE html PUBLIC ""-//W3C//DTD XHTML 1.0 Transitional//EN"" ""http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd"">
            <!-- Hello -->
            <div>
                <div>hello</div>
                <div>hello < asdasd > asd as d< asd asd asdsdadas
                    <div>hello</div>
                    <div>hello</div>
                    <div>hello</div>
                    <div>hello</div>
                </div>
                <div>hello</div>
                <div>hello</div>
                <div>hello</div>
            </div>
            <div>hello</div>
            <div>hello</div>
            <div>hello</div>
            <hr />
            <asp:Content runat='server'>
                <asp:Label x:Name=""head"" text=""hello"" runat=""server"" ui:visible='false' />
            </asp:Content>
        ";

        static string markup2 = @"<asp:Label x:Name=""head"" text=""hello"" runat=""server"" ui:visible='false' />";

        static string markup3 = @"
<!DOCTYPE HTML PUBLIC ""-//W3C//DTD HTML 4.0 Transitional//EN"">
<html>
    <head>
        <title></title>
    </head>
    <body>
    
    </body>
</html>
        ";

        static string markup4 = @"
<!DOCTYPE html PUBLIC ""-//W3C//DTD XHTML 1.0 Transitional//EN"" ""http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd"">
<html xmlns='http://www.w3.org/1999/xhtml'>
    <head>
        <meta httpe-quiv='content-type' content='text/html; charset=utf-8' />
        <meta name='description' content='Vítejte na stránkách Základní školy J.E.Purkyně a Základní umělecké školy v Libochovicích - skolylibo.webs.neptuo.com' />
        <meta name='keywords' content='wfw,rssmm' />
        <meta http-equiv='Content-language' content='' />
        <meta name='robots' content='all, index, follow' />
        <meta name='author' content='Marek Fišera' />
        <title>Vítejte na stránkách Základní školy J.E.Purkyně a Základní umělecké školy v Libochovicích - skolylibo.webs.neptuo.com</title>
        <link rel='stylesheet' href='/file.php?fid=19' type='text/css' />
    </head>
    <body>
        <img style='margin:0' src='http://skolylibochovice.cz/logo.jpg' />
        <div id='obal'>
            <h1>Vítejte na stránkách Základní školy J.E.Purkyně a Základní umělecké školy v Libochovicích</h1>
            <div id='zs'>
                <a href='http://skolylibo.webs.neptuo.com/zs/'></a>
            </div>
            <div id='zus'>
                <a href='http://www.skolylibochovice.cz/zus'></a>
            </div>
        </div>
    </body>
</html>
";

        static string markup5 = "<div>xx<HtmlContent>AA</HtmlContent>yy</div>";

        static string extension = "{data:Binding ID}";
        static string extension2 = "{data:Binding Expression=ID}";
        static string extension3 = "{data:Binding {StaticResource idConverter}}";
        static string extension4 = "{data:Binding Expression=ID, Converter={StaticResource idConverter}}";

        public static void Test()
        {
            TestHtmlContentParser();
            //TestExtensionContentParser();
        }

        private static void TestHtmlContentParser()
        {
            HtmlContentParser parser = new HtmlContentParser(new HtmlContentParser.Configuration
            {
                StartTagRegex = new Regex("<(?<TagName>HtmlContent)")
            });
            parser.OnParsedItem += (e) =>
            {
                Console.WriteLine(e.ParsedItem.Fullname);

                if (!String.IsNullOrEmpty(e.ParsedItem.Content))
                    parser.Parse(e.ParsedItem.Content);
            };
            parser.Parse(markup5);
        }

        private static void TestExtensionContentParser()
        {
            ExtensionContentParser parser = new ExtensionContentParser();
            parser.OnParsedItem += (e) =>
            {
                if (e.ParsedItem != null)
                    Console.WriteLine(e.ParsedItem.Fullname);
                else
                    Console.WriteLine("Not an extension!");
            };
            parser.Parse(extension3);
        }
    }
}
