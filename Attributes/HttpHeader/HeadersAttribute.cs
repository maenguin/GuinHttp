using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GuinHttp.Attributes.HttpHeader
{
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method)]
    public class HeadersAttribute : HttpHeadersAttribute
    {
        public HeadersAttribute(params string[] value) : base(value) { }
    }
    [Headers( "d","" ) ]
    public class dd
    {

    }
}
