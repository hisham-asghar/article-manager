using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Generics
{
    public static class StringUtils
    {
        public static bool LinkComaprer(this string service, string p)
        {
            p = p.LinkModifier();
            service = service.LinkModifier();
            return p.LinkModifier() == service.LinkModifier();
        }
        public static string RemoveSpecialCharacters(this string str,char seprator = ' ')
        {
            if (str == null) return null;
            var sb = new StringBuilder();
            foreach (var c in str)
            {
                if ((c >= '0' && c <= '9') || (c >= 'A' && c <= 'Z') || (c >= 'a' && c <= 'z'))
                {
                    sb.Append(c);
                }
                else
                {
                    sb.Append(seprator);
                }
            }
            return sb.ToString();
        }
        public static string Guid(this string str)
        {
            if (str == null)
            {
                return "";
            }
            var guid = str.Trim().ToLower();
            if (guid.Contains(' '))
                guid = guid.Replace(' ', '-');
            guid = System.Text.RegularExpressions.Regex.Replace(guid, @"[^0-9a-zA-Z]+", "-");
            return guid;
        }
        public static string LinkModifier(this string p)
        {
            if (p.Contains("%20"))
                p = p.Replace("%20", "-");
            if(p.Contains(' '))
                p = p.Replace(' ', '-');
            
            p = p.LinkTrimmer() ?? "";
            return p.ToLower();
        }

        public static string LinkTrimmer(this string link)
        {
            if (null == link)
                return null;
            char[] charsToTrim = { '/' };
            return link.Trim(charsToTrim).ToLower();
        }

        public static double ParseDouble(this string doublestringValue)
        {
            var amount = 0.0;

            if (null != doublestringValue && 0 < doublestringValue.Trim().Length)
            {
                double.TryParse(doublestringValue.Trim(), out amount);
            }

            return amount;
        }

        public static int ParseInt(this string intstringValue)
        {
            var amount = 0;

            if (null != intstringValue && 0 < intstringValue.Trim().Length)
            {
                int.TryParse(intstringValue.Trim(), out amount);
            }

            return amount;
        }

        public static double ParseAmount(this string str)
        {
            if (string.IsNullOrWhiteSpace(str)) return 0;
            if (str.Contains(",")) str = str.Replace(",", "");
            if (str.Contains("$")) str = str.Replace("$", "");
            return str.ParseDouble();
        }

        public static bool ParseBool(this string boolstringValue)
        {
            var result = false;

            if (null != boolstringValue && 0 < boolstringValue.Trim().Length)
            {
                bool.TryParse(boolstringValue.Trim(), out result);
            }

            return result;
        }
    }
}
