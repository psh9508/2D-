using _2D보험구분검증툴.Class.Helper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace _2D보험구분검증툴.Helper
{
    public static class StringHelper
    {
        public static string IfNullOrEmptyString(this string src, string defaultValue)
        {
            return src == null || src == string.Empty ? src : defaultValue;
        }

        public static bool HasValue(this string src)
        {
            return src != null || src != string.Empty;
        }

        public static string IfNotHasValue(this string src, string replaceString)
        {
            return src != null && src != string.Empty ? src : replaceString;
        }

        public static bool IsNullOrWhiteSpace(this string src)
        {
            return string.IsNullOrWhiteSpace(src);
        }

        public static string Replace(this string src, string oldchar, string newchar = "", int count = 1)
        {
            return new Regex(oldchar).Replace(src, newchar, count);
        }

        public static bool IsNumeric(this string src, bool isFloat = false)
        {
            if (src.IsNullOrWhiteSpace()) return false;
            else if (src.StartsWith("-")) src = src.Substring(1);
            src = src.Replace("\\.\\d*", string.Empty, 1);

            return src.None(x => !(char.IsDigit(x) || x.Equals(',')));
        }
    }
}
