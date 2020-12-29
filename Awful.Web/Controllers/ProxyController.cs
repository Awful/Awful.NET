// <copyright file="ProxyController.cs" company="Drastic Actions">
// Copyright (c) Drastic Actions. All rights reserved.
// </copyright>

using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace Awful.Web.Controllers
{
    /// <summary>
    /// Awful.Web File Proxy.
    /// Used to get images (and other files) from sites that are secured.
    /// Basically, it's a really stupid controller that only returns file requests.
    /// </summary>
    [ApiController]
    [Route("[controller]")]
    public class ProxyController : ControllerBase
    {
        private HttpClient client = new HttpClient();

        /// <summary>
        /// Get Request.
        /// </summary>
        /// <returns>Action Result.</returns>
        [HttpGet]
        public async Task<ActionResult> Get()
        {
            if (this.Request.Query.ContainsKey("file"))
            {
                var result = await this.client.GetAsync(this.Request.Query["file"]).ConfigureAwait(false);

                // TODO: It should actually return the real content type, not just png.
                // But these old browsers don't seem to care about that, so good enough for now!
                return this.File(await result.Content.ReadAsStreamAsync().ConfigureAwait(false), "image/png");
            }
            else
            {
                return this.NoContent();
            }
        }
    }
}
