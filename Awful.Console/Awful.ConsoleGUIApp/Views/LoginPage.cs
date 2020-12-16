// <copyright file="LoginPage.cs" company="Drastic Actions">
// Copyright (c) Drastic Actions. All rights reserved.
// </copyright>

using System;
using System.Collections.Generic;
using System.Text;
using Terminal.Gui;

namespace Awful.ConsoleGUIApp.Views
{
    /// <summary>
    /// Login page.
    /// </summary>
    public class LoginPage : BasePage
    {
        /// <inheritdoc/>
        public override void Setup()
        {
            StringBuilder awfulMessage = new StringBuilder();
            awfulMessage.AppendLine(@"╔═══╗       ╔═╗    ╔╗            ╔╗ ");
            awfulMessage.AppendLine(@"║╔═╗║       ║╔╝    ║║           ╔╝╚╗");
            awfulMessage.AppendLine(@"║║ ║║╔╗╔╗╔╗╔╝╚╗╔╗╔╗║║   ╔══╗╔══╗╚╗╔╝");
            awfulMessage.AppendLine(@"║╚═╝║║╚╝╚╝║╚╗╔╝║║║║║║   ║╔╗║║╔╗║ ║║ ");
            awfulMessage.AppendLine(@"║╔═╗║╚╗╔╗╔╝ ║║ ║╚╝║║╚╗╔╗║║║║║║═╣ ║╚╗");
            awfulMessage.AppendLine(@"╚╝ ╚╝ ╚╝╚╝  ╚╝ ╚══╝╚═╝╚╝╚╝╚╝╚══╝ ╚═╝");
            using var awfulLabel = new Label(awfulMessage.ToString()) { X = Pos.Center(), Y = Pos.Center() - 6 };
            using var login = new Label("Login: ") { X = awfulLabel.X - 25, Y = Pos.Bottom(awfulLabel) + 1 };
            using var password = new Label("Password: ")
            {
                X = Pos.Left(login),
                Y = Pos.Bottom(login) + 1,
            };
            using var loginText = new TextField("")
            {
                X = Pos.Right(password),
                Y = Pos.Top(login),
                Width = 40,
            };

            using var passText = new TextField("")
            {
                Secret = true,
                X = Pos.Left(loginText),
                Y = Pos.Top(password),
                Width = Dim.Width(loginText),
            };

            using var button = new Button("Login")
            {
                X = Pos.Center(),
                Y = Pos.Top(passText) + 2,
            };

            button.Clicked += () => MessageBox.Query(20, 7, "Hi", "Neat?", "Yes", "No");
            this.Win.Add(awfulLabel, login, password, loginText, passText, button);
        }
    }
}
