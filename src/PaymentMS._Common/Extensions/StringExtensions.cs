using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PaymentMS
{
    public static class StringExtensions
    {
        public static bool IsNullOrEmpty(this string st)
        {
            return String.IsNullOrEmpty(st);
        }

        public static string ToLowerFirstChar(this string st)
        {
            if (st.IsNullOrEmpty())
                return st;

            return st.First().ToString().ToLower() + st.Substring(1);
        }

        public static string TrimByLength(this string st, int maxLength)
        {
            if (st == null)
                return st;

            if (st.Length <= maxLength)
                return st;

            return st.Substring(0, maxLength);
        }

        public static TEnum AsEnum<TEnum>(this string value) where TEnum : struct, IConvertible
        {
            return EnumHelper.Parse<TEnum>(value);
        }
    }
}
