using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GuinHttpWebW.Attributes.HttpContentType
{
    [System.AttributeUsage(AttributeTargets.Class | AttributeTargets.Method)]
    public class JsonBodyAttribute : HttpContentTypeAttribute
    {
        public override string Value { get => "application/json"; }
    }
}
