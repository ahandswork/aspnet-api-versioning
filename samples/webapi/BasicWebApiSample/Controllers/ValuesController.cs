﻿namespace Microsoft.Examples.Controllers
{
    using Microsoft.Web.Http;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    using System.Web.Http;

    [ApiVersion( "1.0" )]
    [Route( "api/values" )]
    public class ValuesController : ApiController
    {
        // GET api/values?api-version-1.0
        public async Task<IHttpActionResult> Get() => Ok( new { controller = GetType().Name, version = Request.GetRequestedApiVersion().ToString() } );
    }
}