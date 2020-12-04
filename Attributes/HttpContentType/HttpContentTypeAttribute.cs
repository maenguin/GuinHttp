using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GuinHttpWebW.Attributes.HttpContentType
{
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method)]
    public class HttpContentTypeAttribute : Attribute
    {
        public virtual string Value { get; }
    }
}
