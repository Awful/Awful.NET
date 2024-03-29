﻿// <copyright file="JsonResult.cs" company="Drastic Actions">
// Copyright (c) Drastic Actions. All rights reserved.
// </copyright>

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Awful.Entities.Web
{
    public class JsonResult : Result
    {
        public JsonResult(object jsonObject, bool httpRequestIsSuccess, string text, string endpoint, string errorHtml = "", string onProbationText = "")
            : base(httpRequestIsSuccess, text, endpoint, errorHtml, onProbationText)
        {
            this.Json = jsonObject;
        }

        /// <summary>
        /// Gets the JSON object.
        /// </summary>
        public object Json { get; }
    }
}
