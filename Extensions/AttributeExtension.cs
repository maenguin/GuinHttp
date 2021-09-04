using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;

namespace GuinHttp.Extensions
{
    public static class AttributeExtension
    {
        public static TValue GetAttributeValue<TAttribute, TValue>(this Type type, Func<TAttribute, TValue> valueSelector)
        where TAttribute : Attribute
        {
            if (type.GetCustomAttributes(typeof(TAttribute), true).FirstOrDefault() is TAttribute att)
            {
                return valueSelector(att);
            }
            return default(TValue);
        }
        public static TValue GetAttributeValue<TAttribute, TValue>(this MethodBase method, Func<TAttribute, TValue> valueSelector)
        where TAttribute : Attribute
        {
            if (method.GetCustomAttributes(typeof(TAttribute), true).FirstOrDefault() is TAttribute att)
            {
                return valueSelector(att);
            }
            return default(TValue);
        }
        public static TValue GetAttributeValue<TAttribute, TValue>(this ParameterInfo parameter, Func<TAttribute, TValue> valueSelector)
        where TAttribute : Attribute
        {
            if (parameter.GetCustomAttributes(typeof(TAttribute), true).FirstOrDefault() is TAttribute att)
            {
                return valueSelector(att);
            }
            return default(TValue);
        }
        public static TValue GetAttributeValue<TAttribute, TValue>(this MemberInfo member, Func<TAttribute, TValue> valueSelector)
        where TAttribute : Attribute
        {
            if (member.GetCustomAttributes(typeof(TAttribute), true).FirstOrDefault() is TAttribute att)
            {
                return valueSelector(att);
            }
            return default(TValue);
        }
    }
}
