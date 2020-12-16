// <copyright file="Program.cs" company="Drastic Actions">
// Copyright (c) Drastic Actions. All rights reserved.
// </copyright>

using System;
using System.Globalization;
using System.Text;
using Awful.ConsoleGUIApp.Views;
using Terminal.Gui;

namespace Awful.ConsoleGUIApp
{
    /// <summary>
    /// Main Program.
    /// </summary>
    internal class Program
    {
        /// <summary>
        /// Main Entrypoint to the app.
        /// </summary>
        private static void Main()
        {
            // SA is a US-ish site. We're going to enforce our app to run in it.
            Console.OutputEncoding = Encoding.Default;
            CultureInfo.DefaultThreadCurrentUICulture = CultureInfo.GetCultureInfo("en-US");
            Application.Init();

            var test = new LoginPage();
            test.Init(Application.Top, Colors.TopLevel);
            test.Setup();
            test.Run();
        }
    }
}
