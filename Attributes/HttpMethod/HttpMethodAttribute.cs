using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GuinHttp.Attributes.HttpMethod
{
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method)]
    public class HttpMethodAttribute : Attribute
    {
        public virtual string Value { get; }
        public string ApiPath { get; }
        public HttpMethodAttribute(string apiPath)
        {
            ApiPath = apiPath;
        }
        
    }

   
}
