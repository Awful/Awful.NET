// <copyright file="Program.cs" company="Drastic Actions">
// Copyright (c) Drastic Actions. All rights reserved.
// </copyright>

using Awful.ConsoleGUIApp.Views;
using System;
using System.Globalization;
using System.Text;
using Terminal.Gui;

namespace Awful.ConsoleGUIApp
{
    internal class Program
    {
        private static void Main(string[] args)
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
