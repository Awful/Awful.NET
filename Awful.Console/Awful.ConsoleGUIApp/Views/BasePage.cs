// <copyright file="BasePage.cs" company="Drastic Actions">
// Copyright (c) Drastic Actions. All rights reserved.
// </copyright>

using System;
using System.Collections.Generic;
using System.Text;
using Terminal.Gui;

namespace Awful.ConsoleGUIApp.Views
{
    /// <summary>
    /// Base Page.
    /// A hack on top of the gui.cs example Scenario design.
    /// Since we're going to need a "page" system, this is a way to start that.
    /// </summary>
    public class BasePage : IDisposable
    {
        private bool disposedValue;

        /// <summary>
        /// Gets or sets the Top level for the <see cref="BasePage"/>. This should be set to <see cref="Terminal.Gui.Application.Top"/> in most cases.
        /// </summary>
        public Toplevel Top { get; set; }

        /// <summary>
        /// Gets or sets the Window for the <see cref="BasePage"/>. This should be set within the <see cref="Terminal.Gui.Application.Top"/> in most cases.
        /// </summary>
        public Window Win { get; set; }

        /// <summary>
        /// Override this to implement the <see cref="Scenario"/> setup logic (create controls, etc...). 
        /// </summary>
        /// <remarks>This is typically the best place to put scenario logic code.</remarks>
        public virtual void Setup()
        {
        }

        /// <summary>
        /// Helper that provides the default <see cref="Terminal.Gui.Window"/> implementation with a frame and
        /// label showing the name of the <see cref="Page"/> and logic to exit back to
        /// the Scenario picker UI.
        /// Override <see cref="Init(Toplevel)"/> to provide any <see cref="Terminal.Gui.Toplevel"/> behavior needed.
        /// </summary>
        /// <param name="top">The Toplevel created by the UI Catalog host.</param>
        /// <param name="colorScheme">The colorscheme to use.</param>
        /// <param name="windowName">Name of the Window.</param>
        /// <remarks>
        /// <para>
        /// Thg base implementation calls <see cref="Application.Init"/>, sets <see cref="Top"/> to the passed in <see cref="Toplevel"/>, creates a <see cref="Window"/> for <see cref="Win"/> and adds it to <see cref="Top"/>.
        /// </para>
        /// <para>
        /// Overrides that do not call the base.<see cref="Run"/>, must call <see cref="Application.Init "/> before creating any views or calling other Terminal.Gui APIs.
        /// </para>
        /// </remarks>
        public virtual void Init(Toplevel top, ColorScheme colorScheme, string windowName = "Awful.NET")
        {
            Application.Init();

            this.Top = top;
            if (this.Top == null)
            {
                this.Top = Application.Top;
            }

            int margin = 3;
            this.Win = new Window(windowName)
            {
                X = 1,
                Y = 1,
                Width = Dim.Fill() - margin,
                Height = Dim.Fill() - margin,
                ColorScheme = colorScheme,
            };

            this.Top.Add(this.Win);
        }

        /// <summary>
        /// Runs the <see cref="BasePage"/>. Override to start the <see cref="BasePage"/> using a <see cref="Toplevel"/> different than `Top`.
        /// </summary>
        /// <remarks>
        /// Overrides that do not call the base.<see cref="Run"/>, must call <see cref="Application.Shutdown"/> before returning.
        /// </remarks>
        public virtual void Run()
        {
            // Must explicit call Application.Shutdown method to shutdown.
            Application.Run(this.Top);
        }

        /// <summary>
        /// Stops the scenario. Override to change shutdown behavior for the <see cref="BasePage"/>.
        /// </summary>
        public virtual void RequestStop()
        {
            Application.RequestStop();
        }

        /// <summary>
        /// Dispose.
        /// </summary>
        /// <param name="disposing">Is Disposing.</param>
        protected virtual void Dispose(bool disposing)
        {
            if (!this.disposedValue)
            {
                if (disposing)
                {
                }

                this.disposedValue = true;
            }
        }

        /// <inheritdoc/>
        public void Dispose()
        {
            // Do not change this code. Put cleanup code in 'Dispose(bool disposing)' method
            this.Dispose(disposing: true);
            GC.SuppressFinalize(this);
        }
    }
}
