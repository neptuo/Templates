using LiveWebUI.Models;
using Microsoft.AspNet.SignalR;
using Neptuo.Security.Cryptography;
using Neptuo.Templates;
using Neptuo.Templates.Compilation;
using Neptuo.Templates.Compilation.CodeGenerators;
using Neptuo.Templates.Compilation.Parsers;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using TestConsoleNG;
using TestConsoleNG.Controls;
using TestConsoleNG.Data;
using TestConsoleNG.Extensions;
using TestConsoleNG.Observers;
using TestConsoleNG.SimpleContainer;

namespace LiveWebUI.Hubs
{
    public class LiveHub : Hub
    {
        public string DefaultView(string identifier)
        {
            if (!String.IsNullOrEmpty(identifier))
            {
                string viewContent = TemplateHelper.ViewRepository.GetContent(identifier);
                if (!String.IsNullOrEmpty(viewContent))
                    return viewContent;
            }
            return File.ReadAllText(HttpContext.Current.Server.MapPath("~/App_Data/Index.html"));
        }

        public string GetSourceCode(string viewContent)
        {
            try
            {
                return TemplateHelper.ViewService.GenerateSourceCode(
                    viewContent, 
                    new ViewServiceContext(TemplateHelper.Container), 
                    TemplateHelper.ViewService.NamingService.FromContent(viewContent)
                );
            }
            catch (CodeDomViewServiceException e)
            {
                Clients.All.errors(e.Errors);
                return "Error generation source code! See log for more details";
            }
        }

        public string RunServer(string identifier, string viewContent)
        {
            try
            {
                TemplateHelper.ViewService.CompileViewContent(
                    viewContent, 
                    new ViewServiceContext(TemplateHelper.Container), 
                    TemplateHelper.ViewService.NamingService.FromContent(viewContent)
                );
                return Save(identifier, viewContent);
            }
            catch (CodeDomViewServiceException e)
            {
                Clients.All.errors(e.Errors);
                return null;
            }
        }

        public string Save(string identifier, string viewContent)
        {
            if (String.IsNullOrEmpty(identifier))
                identifier = HashHelper.Sha1(Guid.NewGuid().ToString());

            TemplateHelper.ViewRepository.SetContent(identifier, viewContent);
            return identifier;
        }
    }
}