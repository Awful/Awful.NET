﻿// <copyright file="Program.cs" company="Drastic Actions">
// Copyright (c) Drastic Actions. All rights reserved.
// </copyright>

using System;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using System.Threading.Tasks;
using Awful.Core.Managers;
using Awful.Core.Utilities;
using Awful.Webview;
using Awful.Webview.Entities.Themes;
using Sharprompt;
using Sharprompt.Validations;

namespace Awful.ConsoleApp
{
    class Program
    {
        static async Task Main(string[] args)
        {
            var awfulClient = await CreateAwfulClient().ConfigureAwait(false);
            SAclopediaManager manager = new SAclopediaManager(awfulClient);
            TemplateHandler handler = new TemplateHandler();
            var entry = await manager.GetEntryAsync(2300).ConfigureAwait(false);
            var defaultOptions = new DefaultOptions() { DeviceColorTheme = DeviceColorTheme.Dark };
            var result = handler.RenderSAclopediaView(entry, defaultOptions);
            File.WriteAllText("test.html", result);
        }

        private static async Task<AwfulClient> CreateAwfulClient()
        {
            var cookiePath = "user.cookie";
            AwfulClient webClient;
            if (!File.Exists(cookiePath))
            {
                webClient = new AwfulClient();
                var username = Prompt.Input<string>("Username", validators: new[] { Validators.Required() });
                var password = Prompt.Password("Password", validators: new[] { Validators.Required() });
                var authManager = new AuthenticationManager(webClient);
                var result = await authManager.AuthenticateAsync(username, password).ConfigureAwait(false);
                if (!result.IsSuccess)
                {
                    throw new Exception("Could not log in!");
                }

                using (FileStream stream = File.Create(cookiePath))
                {
                    var formatter = new BinaryFormatter();
                    System.Console.WriteLine("Serializing cookie container");
                    formatter.Serialize(stream, webClient.CookieContainer);
                }
            }
            else
            {
                System.Net.CookieContainer cookieContainer;
                using (FileStream stream = File.OpenRead(cookiePath))
                {
                    var formatter = new BinaryFormatter();
                    System.Console.WriteLine("Deserializing cookie container");
                    cookieContainer = (System.Net.CookieContainer)formatter.Deserialize(stream);
                    webClient = new AwfulClient(cookieContainer);
                }
            }

            return webClient;
        }
    }
}
