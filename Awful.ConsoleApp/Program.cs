// <copyright file="Program.cs" company="Drastic Actions">
// Copyright (c) Drastic Actions. All rights reserved.
// </copyright>

using System;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using System.Threading.Tasks;
using Awful.ConsoleApp.Options;
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
            using var awfulClient = await CreateAwfulClient().ConfigureAwait(false);
            var menu = Prompt.Select<MainMenu>("Main Menu");
            switch (menu)
            {
                case MainMenu.LocalParsing:
                    break;
                case MainMenu.TemplateHandler:
                    TemplateHandler handler = new TemplateHandler();
                    var templateMenu = Prompt.Select<TemplateHandlerOption>("Render Template");
                    var deviceColor = Prompt.Select<DeviceColorTheme>("Device Color Theme");
                    var defaultOptions = new DefaultOptions() { DeviceColorTheme = deviceColor };
                    string result = string.Empty;
                    switch (templateMenu)
                    {
                        case TemplateHandlerOption.ThreadPost:
                            var threadId = Prompt.Input<int>("Enter Thread Id", 3836680);
                            var page = Prompt.Input<int>("Enter Page", 1);
                            var answer = Prompt.Confirm("Go To Last Page?");
                            ThreadPostManager manager = new ThreadPostManager(awfulClient);
                            var entry = await manager.GetThreadPostsAsync(threadId, page, answer).ConfigureAwait(false);
                            result = handler.RenderThreadPostView(entry, defaultOptions);
                            break;
                        case TemplateHandlerOption.Ban:
                            BanManager banManager = new BanManager(awfulClient);
                            var banEntry = await banManager.GetBanPageAsync().ConfigureAwait(false);
                            result = handler.RenderBanView(banEntry, defaultOptions);
                            break;
                        case TemplateHandlerOption.UserProfile:
                            UserManager userManager = new UserManager(awfulClient);
                            var profileId = Prompt.Input<int>("Enter Profile Id", 0);
                            var profileEntry = await userManager.GetUserFromProfilePageAsync(profileId).ConfigureAwait(false);
                            result = handler.RenderProfileView(profileEntry, defaultOptions);
                            break;
                        case TemplateHandlerOption.SAclopedia:
                            var saId = Prompt.Input<int>("Enter SAclopedia Id", 2300);
                            SAclopediaManager saManager = new SAclopediaManager(awfulClient);
                            var saEntry = await saManager.GetEntryAsync(saId).ConfigureAwait(false);
                            result = handler.RenderSAclopediaView(saEntry, defaultOptions);
                            break;
                    }

                    File.WriteAllText("test.html", result);
                    Console.WriteLine("File written to test.html");
                    break;
                default:
                    break;
            }
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