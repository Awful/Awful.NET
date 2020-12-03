// <copyright file="AwfulPopup.cs" company="Drastic Actions">
// Copyright (c) Drastic Actions. All rights reserved.
// </copyright>

using System;
using System.Collections.Generic;
using System.Text;
using Awful.Mobile.Views;
using Forms9Patch;
using Xamarin.Forms;

namespace Awful.Mobile.Controls
{
    /// <summary>
    /// Awful Popup.
    /// Used to render modal content.
    /// </summary>
    public class AwfulPopup : IDisposable
    {
        private ModalPopup popup;
        private bool disposedValue;
        private Action callback;

        /// <summary>
        /// Initializes a new instance of the <see cref="AwfulPopup"/> class.
        /// </summary>
        public AwfulPopup()
        {
            this.popup = new Forms9Patch.ModalPopup()
            {
                Content = new DefaultView(),
            };
        }

        /// <summary>
        /// Set popup to appear.
        /// </summary>
        /// <param name="isVisible">Set is visibile.</param>
        public void SetIsVisible(bool isVisible)
        {
            this.popup.IsVisible = isVisible;
            if (!isVisible && this.callback != null)
            {
                this.callback.Invoke();
            }
        }

        /// <summary>
        /// Set popups internal content.
        /// </summary>
        /// <param name="view"><see cref="Xamarin.Forms.ContentView"/>.</param>
        /// <param name="launchModal">Launch the modal after setting the content.</param>
        /// <param name="callback">Callback after modal is closed.</param>
        public void SetContent(Xamarin.Forms.ContentView view, bool launchModal = false, Action callback = default)
        {
            this.popup.BackgroundColor = App.GetCurrentBackgroundColor();
            this.popup.Content = view;
            this.callback = callback;

            if (launchModal)
            {
                this.SetIsVisible(true);
            }
        }

        /// <inheritdoc/>
        public void Dispose()
        {
            // Do not change this code. Put cleanup code in 'Dispose(bool disposing)' method
            this.Dispose(disposing: true);
            GC.SuppressFinalize(this);
        }

        /// <summary>
        /// Dispose.
        /// </summary>
        /// <param name="disposing">Disposing.</param>
        protected virtual void Dispose(bool disposing)
        {
            if (!this.disposedValue)
            {
                if (disposing)
                {
                    this.popup.Dispose();
                }

                // TODO: free unmanaged resources (unmanaged objects) and override finalizer
                // TODO: set large fields to null
                this.disposedValue = true;
            }
        }
    }
}
