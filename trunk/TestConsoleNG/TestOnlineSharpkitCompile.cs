using Neptuo.Templates.SharpKitGenerator;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TestConsoleNG
{
    public class TestOnlineSharpkitCompile
    {
        public static void Test()
        {
            var cs = @"namespace Neptuo.Templates
{
    using System;
    using Neptuo.Templates;
    using Neptuo.Templates.Controls;
    using SharpKit.JavaScript;
    using TestConsoleNG.Controls;

    [JsType(JsMode.Clr)]
    public sealed class View_7D2EFEF4F3AB0B8A114C713D7C4C8F06E0F3A0F7 : Neptuo.Templates.BaseGeneratedView, System.IDisposable
    {
        protected override void CreateViewPageControls(IViewPage viewPage)
        {
            viewPage.Content.Add(this.field1_Create());
        }
        private PanelControl field1_Create()
        {
            PanelControl field1 = new PanelControl(this.componentManager);
            field1.Content = new System.Collections.Generic.List<object>();
            this.componentManager.AddComponent(field1, this.field1_Bind);
            return field1;
        }
        private void field1_Bind(PanelControl field1)
        {
            field1.SetAttribute(""class"", ""checkin"");
            field1.Content.Add(this.field2_Create());
        }
        private GenericContentControl field2_Create()
        {
            GenericContentControl field2 = new GenericContentControl(this.componentManager);
            componentManager.AddComponent(field2, this.field2_Bind);
            return field2;
        }
        private void field2_Bind(GenericContentControl field2)
        {
            field2.TagName = ""a"";
            field2.SetAttribute(""href"", ""google"");
            field2.Content = new System.Collections.Generic.List<object>();
            field2.Content.Add(""Hello, World!"");
        }
    }
}";
            //var res = new SiteService().CsToJs(cs);
            //Console.WriteLine(res.JsCode);

            StringWriter output = new StringWriter();
            StringReader input = new StringReader(cs);

            SharpKitCodeGenerator generator = new SharpKitCodeGenerator();
            generator.AddReference("mscorlib.dll");
            generator.AddSharpKitReference("SharpKit.JavaScript.dll", "SharpKit.Html.dll", "SharpKit.jQuery.dll");
            generator.AddReferenceFolder(Environment.CurrentDirectory);
            generator.TempDirectory = Path.Combine(Environment.CurrentDirectory, "Temp");
            generator.Generate(new SharpKitCodeGeneratorContext(input, output));

            Console.WriteLine(output.ToString());
        }
    }



    public class SiteService
    {
        static SiteService()
        {
            Initialize();
        }

        public static void Initialize()
        {
            var skDir = @"C:\Program Files (x86)\SharpKit\5\Assemblies\v4.0\";
            if (!Directory.Exists(skDir))
                skDir = @"C:\Program Files\SharpKit\4\Assemblies\v4.0\";
            AssemblyDirs = new List<string> { @"C:\Windows\Microsoft.NET\Framework\v4.0.30319", skDir, Environment.CurrentDirectory };
            SysRefs = new List<string> { "mscorlib.dll", "System.dll", "System.Core.dll" };
            Refs = new List<string> { "SharpKit.JavaScript.dll", "SharpKit.Html.dll", "SharpKit.jQuery.dll", "TestConsoleNG.exe", "TestConsoleNG.Components.dll", 
                "Neptuo.Templates.dll", "Neptuo.Templates.Components.dll" };
            SysRefs = SysRefs.Select(ResolveAssembly).ToList();
            Refs = Refs.Select(ResolveAssembly).ToList();
        }
        static List<string> AssemblyDirs;
        static List<string> SysRefs;
        static List<string> Refs;
        string GetOutput(List<string> output, List<string> blackList)
        {
            var s = String.Join("\n", output.ToArray());
            foreach (var name in blackList)
                s = s.Replace(name, "");
            return s;
        }

        static string ResolveAssembly(string filename)
        {
            if (Path.IsPathRooted(filename))
                return filename;
            var dirs = AssemblyDirs;
            foreach (var dir in dirs)
            {
                var path = Path.Combine(dir, filename);
                if (File.Exists(path))
                    return path;
            }
            throw new Exception("Cannot resolve assembly: " + filename);
        }
        string RefsToArgs(List<string> refs)
        {
            return String.Join(" ", refs.Select(t => "/reference:\"" + t + "\"").ToArray());
        }

        public SkcResult CsToJs(string cs)
        {
            var skcRes = new SkcResult();
            //dir:"C:\Users\dkhen\Documents\Visual Studio 2010\Projects\SharpKitWebApp35\SharpKitWebApp35" /outputgeneratedfile:obj\Release\SharpKitWebApp35.cs /define:TRACE /reference:"C:\Program Files (x86)\Reference Assemblies\Microsoft\Framework\.NETFramework\v4.0\Microsoft.CSharp.dll" /reference:"C:\Program Files (x86)\Reference Assemblies\Microsoft\Framework\.NETFramework\v4.0\mscorlib.dll" /reference:"C:\Program Files (x86)\SharpKit\4\Assemblies\v4.0\SharpKit.Html4.dll" /reference:C:\Projects\SharpKit\googlecode\bin\v4.0\SharpKit.JavaScript.dll /reference:"C:\Program Files (x86)\SharpKit\4\Assemblies\v4.0\SharpKit.jQuery-1.5.2.dll" /reference:C:\Projects\SharpKit\googlecode\bin\v4.0\SharpKit.SenchaTouch-1.1.0.dll /reference:"C:\Program Files (x86)\Reference Assemblies\Microsoft\Framework\.NETFramework\v4.0\System.Configuration.dll" /reference:"C:\Program Files (x86)\Reference Assemblies\Microsoft\Framework\.NETFramework\v4.0\System.Core.dll" /reference:"C:\Program Files (x86)\Reference Assemblies\Microsoft\Framework\.NETFramework\v4.0\System.Data.dll" /reference:"C:\Program Files (x86)\Reference Assemblies\Microsoft\Framework\.NETFramework\v4.0\System.dll" /reference:"C:\Program Files (x86)\Reference Assemblies\Microsoft\Framework\.NETFramework\v4.0\System.Drawing.dll" /reference:"C:\Program Files (x86)\Reference Assemblies\Microsoft\Framework\.NETFramework\v4.0\System.EnterpriseServices.dll" /reference:"C:\Program Files (x86)\Reference Assemblies\Microsoft\Framework\.NETFramework\v4.0\System.Web.ApplicationServices.dll" /reference:"C:\Program Files (x86)\Reference Assemblies\Microsoft\Framework\.NETFramework\v4.0\System.Web.dll" /reference:"C:\Program Files (x86)\Reference Assemblies\Microsoft\Framework\.NETFramework\v4.0\System.Web.Extensions.dll" /reference:"C:\Program Files (x86)\Reference Assemblies\Microsoft\Framework\.NETFramework\v4.0\System.Web.Services.dll" /reference:"C:\Program Files (x86)\Reference Assemblies\Microsoft\Framework\.NETFramework\v4.0\System.Xml.dll" /reference:"C:\Program Files (x86)\Reference Assemblies\Microsoft\Framework\.NETFramework\v4.0\System.Xml.Linq.dll" /out:obj\Release\SharpKitWebApp35.dll /target:library Class1.cs Class2.cs Default.aspx.cs IIndexable.cs Properties\AssemblyInfo.cs "C:\Users\dkhen\AppData\Local\Temp\.NETFramework,Version=v4.0.AssemblyAttributes.cs" /contentfile:Default.aspx /contentfile:HTMLPage1.htm /contentfile:res\jsclr-4.1.0.js /contentfile:res\SharpKitWebApp35.js /contentfile:Web.config
            var dir = "App_Data";
            var tmpDir = dir + "\\tmp";
            if (!Directory.Exists(tmpDir))
                Directory.CreateDirectory(tmpDir);
            var key = Guid.NewGuid().ToString("N");
            var csFile = "tmp\\" + key + ".cs";
            var csFile2 = dir + "\\" + csFile;
            var dllFile = "tmp\\" + key + ".dll";
            var dllFile2 = dir + "\\" + dllFile;
            var jsFile = "tmp\\" + key + ".js";
            var jsFile2 = dir + "\\" + jsFile;
            var manifestFile = Path.ChangeExtension(csFile, ".skccache");
            var manifestFile2 = dir + "\\" + manifestFile;
            if (File.Exists(dllFile2))
                File.Delete(dllFile2);
            if (File.Exists(jsFile2))
                File.Delete(jsFile2);
            if (File.Exists(csFile2))
                File.Delete(csFile2);
            if (File.Exists(manifestFile2))
                File.Delete(manifestFile2);
            File.WriteAllText(csFile2, cs);
            var sysRefsArgs = RefsToArgs(SysRefs);
            var refsArgs = RefsToArgs(Refs);

            var blackList = new List<string> { csFile, jsFile, dllFile, manifestFile };
            var cscArgs = String.Format("/target:library /nologo /out:{1} {2} \"{0}\"", csFile, dllFile, refsArgs);
            var csc = @"C:\Windows\Microsoft.NET\Framework\v4.0.30319\csc.exe";
            var cscRes = Execute(dir, csc, cscArgs);
            if (cscRes.ExitCode != 0)
            {
                skcRes.ErrorMessage = "Compilation error";
                skcRes.Output = GetOutput(cscRes.Output, blackList);
            }
            else
            {
                var args = String.Format("/rebuild /out:{3} {4} {5} /OutputGeneratedJsFile:\"{0}\" \"{1}\" /ManifestFile:\"{2}\"", jsFile, csFile, manifestFile, dllFile, sysRefsArgs, refsArgs);
                var skc4 = @"C:\Windows\Microsoft.NET\Framework\v4.0.30319\SharpKit\5\skc5.exe";
                var res = Execute(dir, skc4, args);
                skcRes.Output = GetOutput(res.Output, blackList);
                skcRes.Success = res.ExitCode == 0 && File.Exists(jsFile2);
                if (res.ExitCode != 0)
                {
                    skcRes.ErrorMessage = "Compilation error";
                }
                else if (!File.Exists(jsFile2))
                {
                    skcRes.ErrorMessage = "No js files were generated";
                }
                else
                {
                    skcRes.JsCode = File.ReadAllText(jsFile2);
                    skcRes.Success = true;
                    File.Delete(jsFile2);
                }
                if (File.Exists(manifestFile2))
                    File.Delete(manifestFile2);
            }
            if (File.Exists(dllFile2))
                File.Delete(dllFile2);
            if (File.Exists(csFile2))
                File.Delete(csFile2);
            return skcRes;
        }

        static ExecuteResult Execute(string dir, string file, string args)
        {
            Console.WriteLine("Executing: {0} {1} {2}", dir, file, args);
            var process = Process.Start(new ProcessStartInfo
            {
                WorkingDirectory = dir,
                FileName = file,
                Arguments = args,
                RedirectStandardError = true,
                RedirectStandardOutput = true,
                UseShellExecute = false,
            });
            var res = new ExecuteResult { Output = new List<string>(), Error = new List<string>() };

            Console.WriteLine("{0}>{1} {2}", process.StartInfo.WorkingDirectory, process.StartInfo.FileName, process.StartInfo.Arguments);
            process.OutputDataReceived += (s, e) => { Console.WriteLine(e.Data); res.Output.Add(e.Data); };
            process.ErrorDataReceived += (s, e) => { Console.WriteLine(e.Data); res.Error.Add(e.Data); };
            process.BeginErrorReadLine();
            process.BeginOutputReadLine();
            process.WaitForExit();
            res.ExitCode = process.ExitCode;
            Console.WriteLine("Finished execution. Exit code: {0}", process.ExitCode);
            return res;
        }

    }

    public class SkcResult
    {
        public bool Success { get; set; }
        public string Output { get; set; }
        public string JsCode { get; set; }
        public string ErrorMessage { get; set; }
        public string JsCodeFormatted { get; set; }
        public string CsCodeFormatted { get; set; }
    }

    class ExecuteResult
    {
        public int ExitCode { get; set; }
        public List<string> Output { get; set; }
        public List<string> Error { get; set; }
    }
}
