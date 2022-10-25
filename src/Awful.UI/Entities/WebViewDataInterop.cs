// <copyright file="WebViewDataInterop.cs" company="Drastic Actions">
// Copyright (c) Drastic Actions. All rights reserved.
// </copyright>

using System.Text.Json.Serialization;

namespace Awful.UI.Entities
{
    /// <summary>
    /// WebView Data Interop.
    /// </summary>
    public class WebViewDataInterop
    {
        /// <summary>
        /// Gets or sets the type of data being sent.
        /// </summary>
        [JsonPropertyName("type")]
        public string? Type { get; set; }

        /// <summary>
        /// Gets or sets the "Id."
        /// This could depend on the type of data being sent.
        /// </summary>
        [JsonPropertyName("id")]
        public int Id { get; set; }

        /// <summary>
        /// Gets or sets the "Text."
        /// Could be JSON, could be a string. Depends on the type.
        /// </summary>
        [JsonPropertyName("text")]
        public string? Text { get; set; }
    }
}
