using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GuinHttpWebW.Attributes.HttpParameter
{
    [AttributeUsage(AttributeTargets.Property | AttributeTargets.Parameter)]
    public class HttpParameterAttribute : Attribute
    {
    }
}
