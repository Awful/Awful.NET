// <copyright file="DebugPage.cs" company="Drastic Actions">
// Copyright (c) Drastic Actions. All rights reserved.
// </copyright>

using System;
using System.Collections.Generic;
using System.Text;
using Terminal.Gui;

namespace Awful.ConsoleGUIApp.Views
{
    /// <summary>
    /// Debug page.
    /// </summary>
    public class DebugPage : BasePage
    {
        /// <inheritdoc/>
        public override void Setup()
        {
            // Put your scenario code here, e.g.
            var button = new Button("Press me!")
            {
                X = Pos.Center(),
                Y = Pos.Center(),
            };
            button.Clicked += () => MessageBox.Query(20, 7, "Hi", "Neat?", "Yes", "No");
            this.Win.Add(button);
        }
    }
}
