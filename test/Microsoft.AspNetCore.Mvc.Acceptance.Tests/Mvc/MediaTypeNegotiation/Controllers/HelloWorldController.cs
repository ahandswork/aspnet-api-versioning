﻿namespace Microsoft.AspNetCore.Mvc.MediaTypeNegotiation.Controllers
{
    using AspNetCore.Routing;
    using Microsoft.AspNetCore.Mvc;
    using Models;

    [ApiController]
    [Route( "api/[controller]" )]
    public class HelloWorldController : ControllerBase
    {
        [HttpGet]
        public IActionResult Get() => Ok( new { Controller = nameof( HelloWorldController ), Version = HttpContext.GetRequestedApiVersion().ToString() } );

        [HttpGet( "{id:int}", Name = "GetMessageById" )]
        public IActionResult Get( int id ) => Ok( new { Controller = GetType().Name, Id = id, Version = HttpContext.GetRequestedApiVersion().ToString() } );

        [HttpPost]
        public IActionResult Post( Message message ) => CreatedAtRoute( "GetMessageById", new { id = 42 }, message );
    }
}