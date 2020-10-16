// <copyright file="TemplateHandler.cs" company="Drastic Actions">
// Copyright (c) Drastic Actions. All rights reserved.
// </copyright>

using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using System.Text;
using HandlebarsDotNet;

namespace Awful.Webview
{
    public class TemplateHandler
    {
        private string _saclopediaHtml;

        public TemplateHandler()
        {
            this._saclopediaHtml = TemplateHandler.GetResourceFileContentAsString("Templates.SAclopedia.html.hbs");
        }

        public static string GetResourceFileContentAsString(string fileName)
        {
            var assembly = Assembly.GetExecutingAssembly();
            var resourceName = "Awful.Webview." + fileName;

            string resource = null;
            using (Stream stream = assembly.GetManifestResourceStream(resourceName))
            {
                using (StreamReader reader = new StreamReader(stream))
                {
                    resource = reader.ReadToEnd();
                }
            }
            return resource;
        }

        public void RenderSAsclopediaView()
        {
            var template = Handlebars.Compile(this._saclopediaHtml);
        }
    }
}
