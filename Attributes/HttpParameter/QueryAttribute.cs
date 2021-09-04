using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GuinHttp.Attributes.HttpParameter
{
    [System.AttributeUsage(AttributeTargets.Property | AttributeTargets.Parameter)]
    public class QueryAttribute : HttpParameterAttribute
    {
    }
}
