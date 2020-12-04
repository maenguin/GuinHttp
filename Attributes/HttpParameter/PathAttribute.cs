using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GuinHttpWebW.Attributes.HttpParameter
{
    [System.AttributeUsage(AttributeTargets.Property | AttributeTargets.Parameter)]
    public class PathAttribute : HttpParameterAttribute
    {
    }
}
