using System;
using System.Collections.Generic;
using System.Text;

namespace PaymentMS
{
    public static class IEnumerableExtensions
    {
        public static string Join(this IEnumerable<string> list, string separator)
        {
            return string.Join(separator, list);
        }
    }
}
