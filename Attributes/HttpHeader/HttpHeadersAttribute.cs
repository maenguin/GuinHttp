using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GuinHttp.Attributes.HttpHeader
{
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method)]
    public class HttpHeadersAttribute : Attribute
    {
        public string[] Value { get; }
        public HttpHeadersAttribute(params string[] value)
        {
            Value = value;
        }
    }
}
