// <copyright file="WebViewDataInterop.cs" company="Drastic Actions">
// Copyright (c) Drastic Actions. All rights reserved.
// </copyright>

using System;
using System.Collections.Generic;
using System.Text;
using Newtonsoft.Json;

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
        [JsonProperty("type")]
        public string Type { get; set; }

        /// <summary>
        /// Gets or sets the "Id."
        /// This could depend on the type of data being sent.
        /// </summary>
        [JsonProperty("id")]
        public int Id { get; set; }

        /// <summary>
        /// Gets or sets the "Text."
        /// Could be JSON, could be a string. Depends on the type.
        /// </summary>
        [JsonProperty("text")]
        public string Text { get; set; }
    }
}
