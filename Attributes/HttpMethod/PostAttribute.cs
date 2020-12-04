using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GuinHttpWebW.Attributes.HttpMethod
{
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method)]
    public sealed class PostAttribute : HttpMethodAttribute
    {
        public PostAttribute(string apiPath) : base(apiPath) { }
        public override string Value { get => "POST"; }
    }
}
