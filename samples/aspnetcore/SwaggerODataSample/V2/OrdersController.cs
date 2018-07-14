﻿namespace Microsoft.Examples.V2
{
    using Microsoft.AspNet.OData;
    using Microsoft.AspNet.OData.Routing;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.Examples.Models;
    using System;
    using System.Collections.Generic;

    /// <summary>
    /// Represents a RESTful service of orders.
    /// </summary>
    [ApiVersion( "2.0" )]
    [ODataRoutePrefix( "Orders" )]
    public class OrdersController : ODataController
    {
        /// <summary>
        /// Retrieves all orders.
        /// </summary>
        /// <returns>All available orders.</returns>
        /// <response code="200">The successfully retrieved orders.</response>
        [Produces( typeof( ODataValue<IEnumerable<Order>> ) )]
        [ODataRoute]
        public IActionResult Get()
        {
            var orders = new[]
            {
                new Order(){ Id = 1, Customer = "John Doe" },
                new Order(){ Id = 2, Customer = "Bob Smith" },
                new Order(){ Id = 3, Customer = "Jane Doe", EffectiveDate = DateTime.UtcNow.AddDays( 7d ) }
            };

            return Ok( orders );
        }

        /// <summary>
        /// Gets a single order.
        /// </summary>
        /// <param name="key">The requested order identifier.</param>
        /// <returns>The requested order.</returns>
        /// <response code="200">The order was successfully retrieved.</response>
        /// <response code="404">The order does not exist.</response>
        [Produces( typeof( Order ) )]
        [ODataRoute( "({key})" )]
        public IActionResult Get( int key ) => Ok( new Order() { Id = key, Customer = "John Doe" } );

        /// <summary>
        /// Places a new order.
        /// </summary>
        /// <param name="order">The order to place.</param>
        /// <returns>The created order.</returns>
        /// <response code="201">The order was successfully placed.</response>
        /// <response code="400">The order is invalid.</response>
        [Produces( typeof( Order ) )]
        [ODataRoute]
        public IActionResult Post( [FromBody] Order order )
        {
            if ( !ModelState.IsValid )
            {
                return BadRequest( ModelState );
            }

            order.Id = 42;

            return Created( order );
        }

        /// <summary>
        /// Updates an existing order.
        /// </summary>
        /// <param name="key">The requested order identifier.</param>
        /// <param name="delta">The partial order to update.</param>
        /// <returns>The created order.</returns>
        /// <response code="204">The order was successfully updated.</response>
        /// <response code="404">The order does not exist.</response>
        [Produces( typeof( Order ) )]
        [ODataRoute( "({key})" )]
        public IActionResult Patch( int key, Delta<Order> delta )
        {
            if ( !ModelState.IsValid )
            {
                return BadRequest( ModelState );
            }

            var order = new Order() { Id = 42, Customer = "Bill Mei" };

            delta.Patch( order );

            return Updated( order );
        }

        /// <summary>
        /// Gets the most expensive order.
        /// </summary>
        /// <returns>The most expensive order.</returns>
        /// <response code="200">The order was successfully retrieved.</response>
        /// <response code="404">The no orders exist.</response>
        [HttpGet]
        [Produces( typeof( Order ) )]
        [ODataRoute( nameof( MostExpensive ) )]
        public IActionResult MostExpensive() => Ok( new Order() { Id = 42, Customer = "Bill Mei" } );

        /// <summary>
        /// Rates an order.
        /// </summary>
        /// <param name="key">The requested order identifier.</param>
        /// <param name="parameters">The action parameters.</param>
        /// <returns>None</returns>
        /// <response code="204">The order was successfully rated.</response>
        [HttpPost]
        [ODataRoute( "({key})/Rate" )]
        public IActionResult Rate( int key, ODataActionParameters parameters )
        {
            if ( !ModelState.IsValid )
            {
                return BadRequest( ModelState );
            }

            var rating = (int) parameters["rating"];
            return NoContent();
        }
    }
}