using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GuinHttp.Attributes.HttpMethod
{
    [System.AttributeUsage(AttributeTargets.Class | AttributeTargets.Method)]
    public sealed class DeleteAttribute : HttpMethodAttribute
    {
        public DeleteAttribute(string apiPath) : base(apiPath) { }
        public override string Value { get => "DELETE"; }
    }
}
