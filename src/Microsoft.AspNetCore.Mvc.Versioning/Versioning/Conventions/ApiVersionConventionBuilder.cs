﻿namespace Microsoft.AspNetCore.Mvc.Versioning.Conventions
{
    using ApplicationModels;
    using System;
    using System.Collections.Generic;
    using System.Diagnostics.Contracts;
    using System.Reflection;

    /// <summary>
    /// Represents an object used to configure and create API version conventions for a controllers and their actions.
    /// </summary>
    [CLSCompliant( false )]
    public class ApiVersionConventionBuilder
    {
        /// <summary>
        /// Gets a collection of controller conventions.
        /// </summary>
        /// <value>A <see cref="IDictionary{TKey, TValue}">collection</see> of controller <see cref="IApiVersionConvention{T}">API version conventions</see>.</value>
        protected IDictionary<TypeInfo, IApiVersionConvention<ControllerModel>> ControllerConventions { get; } =
            new Dictionary<TypeInfo, IApiVersionConvention<ControllerModel>>();

        /// <summary>
        /// Gets the count of configured conventions.
        /// </summary>
        /// <value>The total count of configured conventions.</value>
        public virtual int Count => ControllerConventions.Count;

        /// <summary>
        /// Gets or creates the convention builder for the specified controller.
        /// </summary>
        /// <typeparam name="TController">The <see cref="Type">type</see> of controller to build conventions for.</typeparam>
        /// <returns>A new or existing <see cref="ControllerApiVersionConventionBuilder{T}"/>.</returns>
        public virtual ControllerApiVersionConventionBuilder<TController> Controller<TController>()
        {
            Contract.Ensures( Contract.Result<ControllerApiVersionConventionBuilder<TController>>() != null );

            var key = typeof( TController ).GetTypeInfo();
            var convention = default( IApiVersionConvention<ControllerModel> );

            if ( !ControllerConventions.TryGetValue( key, out convention ) )
            {
                var typedConvention = new ControllerApiVersionConventionBuilder<TController>();
                ControllerConventions[key] = typedConvention;
                return typedConvention;
            }

            return (ControllerApiVersionConventionBuilder<TController>) convention;
        }

        /// <summary>
        /// Applies the defined API version conventions to the specified controller.
        /// </summary>
        /// <param name="controllerModel">The <see cref="ControllerModel">controller model</see>
        /// to apply configured conventions to.</param>
        public virtual void ApplyTo( ControllerModel controllerModel )
        {
            Arg.NotNull( controllerModel, nameof( controllerModel ) );

            var key = controllerModel.ControllerType;
            var convention = default( IApiVersionConvention<ControllerModel> );

            if ( ControllerConventions.TryGetValue( key, out convention ) )
            {
                convention.ApplyTo( controllerModel );
            }
        }
    }
}