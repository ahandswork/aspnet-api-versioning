﻿#if WEBAPI
namespace Microsoft.Web.Http.Versioning.Conventions
#else
namespace Microsoft.AspNetCore.Mvc.Versioning.Conventions
#endif
{
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Diagnostics.CodeAnalysis;
    using System.Linq;
    using System.Reflection;

    /// <summary>
    /// Represents a collection of controller action convention builders.
    /// </summary>
#pragma warning disable SA1619 // Generic type parameters should be documented partial class; false positive
    public partial class ActionApiVersionConventionBuilderCollection<T> : IReadOnlyCollection<ActionApiVersionConventionBuilder<T>>
#pragma warning restore SA1619
    {
        readonly ControllerApiVersionConventionBuilder<T> controllerBuilder;
        readonly IList<ActionBuilderMapping> actionBuilderMappings = new List<ActionBuilderMapping>();

        /// <summary>
        /// Initializes a new instance of the <see cref="ActionApiVersionConventionBuilderCollection{T}"/> class.
        /// </summary>
        /// <param name="controllerBuilder">The associated <see cref="ControllerApiVersionConventionBuilder{T}">controller convention builder</see>.</param>
        public ActionApiVersionConventionBuilderCollection( ControllerApiVersionConventionBuilder<T> controllerBuilder ) =>
            this.controllerBuilder = controllerBuilder;

        /// <summary>
        /// Gets or adds a controller action convention builder for the specified method.
        /// </summary>
        /// <param name="actionMethod">The controller action method to get or add the convention builder for.</param>
        /// <returns>A new or existing <see cref="ActionApiVersionConventionBuilder{T}">controller action convention builder</see>.</returns>
        protected internal virtual ActionApiVersionConventionBuilder<T> GetOrAdd( MethodInfo actionMethod )
        {
            var mapping = actionBuilderMappings.FirstOrDefault( m => m.Method == actionMethod );

            if ( mapping == null )
            {
                mapping = new ActionBuilderMapping( actionMethod, new ActionApiVersionConventionBuilder<T>( controllerBuilder ) );
                actionBuilderMappings.Add( mapping );
            }

            return mapping.Builder;
        }

        /// <summary>
        /// Gets a count of the controller action convention builders in the collection.
        /// </summary>
        /// <value>The total number of controller action convention builders in the collection.</value>
        public virtual int Count => actionBuilderMappings.Count;

        /// <summary>
        /// Attempts to retrieve the controller action convention builder for the specified method.
        /// </summary>
        /// <param name="actionMethod">The controller action method to get the convention builder for.</param>
        /// <param name="actionBuilder">The <see cref="ActionApiVersionConventionBuilder{T}">controller action convention builder</see> or <c>null</c>.</param>
        /// <returns>True if the <paramref name="actionBuilder">action builder</paramref> is successfully retrieved; otherwise, false.</returns>
#if NETCOREAPP3_0
        public virtual bool TryGetValue( MethodInfo? actionMethod, [NotNullWhen( true )] out ActionApiVersionConventionBuilder<T>? actionBuilder )
#else
        public virtual bool TryGetValue( MethodInfo? actionMethod, out ActionApiVersionConventionBuilder<T>? actionBuilder )
#endif
        {
            if ( actionMethod == null )
            {
                actionBuilder = null;
                return false;
            }

            var mapping = actionBuilderMappings.FirstOrDefault( m => m.Method == actionMethod );

            if ( mapping == null )
            {
                actionBuilder = null;
                return false;
            }

            actionBuilder = mapping.Builder;
            return true;
        }

        /// <summary>
        /// Returns an iterator that enumerates the controller action convention builders in the collection.
        /// </summary>
        /// <returns>An <see cref="IEnumerator{T}"/> object.</returns>
        public virtual IEnumerator<ActionApiVersionConventionBuilder<T>> GetEnumerator()
        {
            foreach ( var mapping in actionBuilderMappings )
            {
                yield return mapping.Builder;
            }
        }

        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

        sealed partial class ActionBuilderMapping
        {
            internal ActionBuilderMapping( MethodInfo method, ActionApiVersionConventionBuilder<T> builder )
            {
                Method = method;
                Builder = builder;
            }

            internal MethodInfo Method { get; }

            internal ActionApiVersionConventionBuilder<T> Builder { get; }
        }
    }
}