using System;
using System.Collections.Generic;
using System.Linq;

namespace Microsoft.VisualPastie
{
    public static class Extensions
    {
        public static IEnumerable<TExpected> GetAttributeValues<TAttribute, TExpected>(this Enum enumeration, Func<TAttribute, TExpected> expression)
            where TAttribute : Attribute
        {
            if (!Enum.IsDefined(enumeration.GetType(), enumeration))
            {
                throw new InvalidOperationException("Invalid enum value");
            }

            if (expression == null)
            {
                throw new ArgumentException("expression");
            }

            return enumeration.GetType()
                .GetMember(enumeration.ToString())
                .First()
                .GetCustomAttributes(typeof(TAttribute), false)
                .Cast<TAttribute>()
                .Select(expression);
        }
    }
}