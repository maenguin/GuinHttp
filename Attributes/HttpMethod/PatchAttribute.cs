using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GuinHttp.Attributes.HttpMethod
{
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method)]
    public sealed class PatchAttribute : HttpMethodAttribute
    {
        public PatchAttribute(string apiPath) : base(apiPath) { }
        public override string Value { get => "PATCH"; }
    }
}
