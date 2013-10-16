using System;
using System.Linq;
using System.Collections.Generic;
using System.Reflection;
using System.Text;

namespace CodeArt.Text
{
    public static class StringExtensions
    {
        /// <summary>
        /// <para>高效的判定在字符串中{startIndex,length}区域的内容，是否与value匹配</para>
        /// <para>用此方法，不需要再内存中开辟新的字符串</para>
        /// <para>待测试</para>
        /// </summary>
        /// <param name="str"></param>
        /// <param name="startIndex"></param>
        /// <param name="value"></param>
        /// <param name="ignoreCase">true:忽略大小写比对，false:区分大小写</param>
        /// <returns></returns>
        public static bool EqualsWith(this string str, int startIndex, string value, bool ignoreCase)
        {
            if ((startIndex + value.Length) > str.Length) return false;
            if (ignoreCase)
            {
                for (var i = 0; i < value.Length; i++)
                {
                    if (!str[startIndex + i].IgnoreCaseEquals(value[i])) return false;
                }
            }
            else
            {
                for (var i = 0; i < value.Length; i++)
                {
                    if (str[startIndex + i] != value[i]) return false;
                }
            }
            return true;
        }

        public static bool EqualsWith(this string str, int startIndex, string value)
        {
            return EqualsWith(str, startIndex, value, false);
        }

        /// <summary>
        /// 忽略大小写的比较
        /// </summary>
        /// <param name="str"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        public static bool EqualsIgnoreCase(this string str, string value)
        {
            return str.Equals(value, StringComparison.OrdinalIgnoreCase);
        }
        
        public static string EscapeXml(this string str)
        {
            return str.Replace("&", "&amp;").Replace("<", "&lt;").Replace(">", "&gt;").Replace("'", "&apos;").Replace("\"", "&quot;");
        }

        public static string UnescapeXml(this string str)
        {
            return str.Replace("&amp;", "&").Replace("&lt;", "<").Replace("&gt;", ">").Replace("&apos;", "'").Replace("&quot;", "\"");
        }

        /// <summary>
        /// 是否包含xml关键词
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        public static bool ContainsXmlKeyword(this string str)
        {
            return str.IndexOf('&') > -1
                    || str.IndexOf('<') > -1
                    || str.IndexOf('>') > -1
                    || str.IndexOf('\'') > -1
                    || str.IndexOf('\"') > -1;
        }

        #region 大小写转换

        public static string FirstToUpper(this string str)
        {
            return str.ToUpper(0, 1);
        }

        public static string ToUpper(this string str,int startIndex, int length)
        {
            return ChangeLowerOrUpper(str, startIndex, length, (i, len) =>
            {
                return str.Substring(i, len).ToUpper();
            });
        }

        public static string FirstToLower(this string str)
        {
            return str.ToLower(0, 1);
        }

        public static string ToLower(this string str, int startIndex, int length)
        {
            return ChangeLowerOrUpper(str, startIndex, length, (i, len) =>
            {
                return str.Substring(i, len).ToLower();
            });
        }

        private static string ChangeLowerOrUpper(string str, int startIndex, int length, Func<int, int,string> transform)
        {
            if (str.Length == 0) return string.Empty;
            if (startIndex == 0)
            {
                length = length > str.Length ? str.Length : length;
                return string.Format("{0}{1}", transform(0,length),
                                               str.Substring(length));
            }
            else
            {
                int lastIndex = str.Length - 1;
                if (startIndex >= lastIndex) return str;

                int rightLength = str.Length - startIndex - length;
                if (rightLength < 0)
                    length += rightLength;
                return string.Format("{0}{1}{2}", str.Substring(0, startIndex),
                                                transform(startIndex, length),
                                                str.Substring(startIndex + length));
            }
        }

        #endregion

        public static string TrimStart(this string str,string trimValue)
        {
            if (str.IndexOf(trimValue) == 0)
            {
                str = str.Substring(trimValue.Length);
                return str.TrimStart(trimValue);
            }
            return str;
        }

        public static string TrimEnd(this string str, string trimValue)
        {
            int p = str.LastIndexOf(trimValue);
            if (p != -1 && p == (str.Length - trimValue.Length))
            {
                str = str.Substring(0, p);
                return str.TrimEnd(trimValue);
            }
            return str;
        }

        public static string Trim(this string str, string trimValue)
        {
            str = str.TrimStart(trimValue);
            return str.TrimEnd(trimValue);
        }


        #region Base64编码

        public static string ToBase64(this string str)
        {
            byte[] bytes = Encoding.Default.GetBytes(str);
            return Convert.ToBase64String(bytes);
            
        }

        public static string FromBase64(this string str)
        {
             byte[] bytes = Convert.FromBase64String(str);
             return Encoding.Default.GetString(bytes);
        }

        #endregion

    }
}
