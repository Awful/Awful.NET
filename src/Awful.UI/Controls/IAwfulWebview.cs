// <copyright file="IAwfulWebview.cs" company="Drastic Actions">
// Copyright (c) Drastic Actions. All rights reserved.
// </copyright>

namespace Awful.UI.Controls
{
    /// <summary>
    /// IAwfulWebview.
    /// </summary>
    public interface IAwfulWebview
    {
        /// <summary>
        /// Register Javascript Action.
        /// </summary>
        /// <param name="callback">Callback.</param>
        public void RegisterAction(Action<string> callback);

        /// <summary>
        /// Cleanup Javascript Action.
        /// </summary>
        public void Cleanup();

        /// <summary>
        /// Invoke Action.
        /// </summary>
        /// <param name="data">Data used to invoke.</param>
        public void InvokeAction(string data);

        /// <summary>
        /// Invoke Action.
        /// </summary>
        /// <param name="data">Data used to invoke.</param>
        public Task EvaluateJavaScriptAsync(string data);

        /// <summary>
        /// Sets the source of the webview.
        /// </summary>
        /// <param name="html">HTML for the webview.</param>
        public void SetSource(string html);
    }
}
