﻿namespace Microsoft.AspNet.OData.Builder
{
    using System;
    using System.Web.Http.Controllers;

    sealed class ODataQueryOptionParameterDescriptor : HttpParameterDescriptor
    {
        private string prefix = "$";

        internal ODataQueryOptionParameterDescriptor( string name, Type type, object defaultValue )
        {
            ParameterName = name;
            ParameterType = type;
            DefaultValue = defaultValue;
        }

        public override string ParameterName { get; }

        public override Type ParameterType { get; }

        public override object DefaultValue { get; }

        public override string Prefix => prefix;

        public override bool IsOptional => true;

        internal void SetPrefix( string value ) => prefix = value ?? string.Empty;
    }
}