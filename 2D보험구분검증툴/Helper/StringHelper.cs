using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

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
    }
}
