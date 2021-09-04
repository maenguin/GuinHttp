using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GuinHttp.Attributes.HttpContentType
{
    [System.AttributeUsage(AttributeTargets.Class | AttributeTargets.Method)]
    public class FormUrlEncodedAttribute : HttpContentTypeAttribute
    {
        public override string Value { get => "application/x-www-form-urlencoded"; }
    }
}
