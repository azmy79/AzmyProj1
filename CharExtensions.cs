using System;
using System.Linq;
using System.Collections.Generic;
using System.Reflection;
using System.Text;

namespace CodeArt.Text
{
    public static class CharExtensions
    {
        public static bool IgnoreCaseEquals(this char c, char value)
        {
            return c.ToLower() == value.ToLower();
        }

        public static char ToUpper(this char c)
        {
            if (_uppers.ContainsKey(c)) return _uppers[c];
            return c;
        }

        public static char ToLower(this char c)
        {
            if (_lowers.ContainsKey(c)) return _lowers[c];
            return c;
        }

        public static char? ToUpper(this char? c)
        {
            if (c.HasValue)
            {
                if (_uppers.ContainsKey(c.Value)) 
                    return _uppers[c.Value];
            }
            return c;
        }

        public static char? ToLower(this char? c)
        {
            if (c.HasValue)
            {
                if (_lowers.ContainsKey(c.Value))
                    return _lowers[c.Value];
            }
            return c;
        }

        public static bool IgnoreCaseEquals(this char? c, char? value)
        {
            return c.ToLower() == value.ToLower();
        }

        public static bool IsLetter(this char c)
        {
            return _uppers.ContainsKey(c) || _lowers.ContainsKey(c);
        }

        public static bool IsLetter(this char? c)
        {
            return c.HasValue && IsLetter((char)c);
        }

        public static bool IsDigital(this char c)
        {
            return _numerics.ContainsKey(c);
        }

        public static bool IsDigital(this char? c)
        {
            if (!c.HasValue) return false;
            return _numerics.ContainsKey(c.Value);
        }

        public static bool IsXmlKeyword(this char c)
        {
            return _xmlKeywords.ContainsKey(c);
        }

        public static string EscapeXml(this char c)
        {
            string str = null;
            if (_xmlKeywords.TryGetValue(c, out str))
                return str;
            return string.Empty;
        }

        public static bool IsXmlKeyword(this char? c)
        {
            return c.HasValue ? IsXmlKeyword(c.Value) : false;
        }

        public static string EscapeXml(this char? c)
        {
            return c.HasValue ? EscapeXml(c.Value) : string.Empty;
        }


        private static Dictionary<char, char> _uppers = new Dictionary<char, char>();
        private static Dictionary<char, char> _lowers = new Dictionary<char, char>();
        private static Dictionary<char, bool> _numerics = new Dictionary<char, bool>();

        private static Dictionary<char, string> _xmlKeywords = new Dictionary<char, string>(5);


        static CharExtensions()
        {
            #region 小写映射大写
            _uppers.Add('a', 'A');
            _uppers.Add('b', 'B');
            _uppers.Add('c', 'C');
            _uppers.Add('d', 'D');
            _uppers.Add('e', 'E');
            _uppers.Add('f', 'F');
            _uppers.Add('g', 'G');
            _uppers.Add('h', 'H');
            _uppers.Add('i', 'I');
            _uppers.Add('j', 'J');
            _uppers.Add('k', 'K');
            _uppers.Add('l', 'L');
            _uppers.Add('m', 'M');
            _uppers.Add('n', 'N');
            _uppers.Add('o', 'O');
            _uppers.Add('p', 'P');
            _uppers.Add('q', 'Q');
            _uppers.Add('r', 'R');
            _uppers.Add('s', 'S');
            _uppers.Add('t', 'T');
            _uppers.Add('u', 'U');
            _uppers.Add('v', 'V');
            _uppers.Add('w', 'W');
            _uppers.Add('x', 'X');
            _uppers.Add('y', 'Y');
            _uppers.Add('z', 'Z');
            #endregion

            #region 大写映射小谢

            _lowers.Add('A', 'a');
            _lowers.Add('B', 'b');
            _lowers.Add('C', 'c');
            _lowers.Add('D', 'd');
            _lowers.Add('E', 'e');
            _lowers.Add('F', 'f');
            _lowers.Add('G', 'g');
            _lowers.Add('H', 'h');
            _lowers.Add('I', 'i');
            _lowers.Add('J', 'j');
            _lowers.Add('K', 'k');
            _lowers.Add('L', 'l');
            _lowers.Add('M', 'm');
            _lowers.Add('N', 'n');
            _lowers.Add('O', 'o');
            _lowers.Add('P', 'p');
            _lowers.Add('Q', 'q');
            _lowers.Add('R', 'r');
            _lowers.Add('S', 's');
            _lowers.Add('T', 't');
            _lowers.Add('U', 'u');
            _lowers.Add('V', 'v');
            _lowers.Add('W', 'w');
            _lowers.Add('X', 'x');
            _lowers.Add('Y', 'y');
            _lowers.Add('Z', 'z');

            #endregion

            #region 数字

            _numerics.Add('0', true);
            _numerics.Add('1', true);
            _numerics.Add('2', true);
            _numerics.Add('3', true);
            _numerics.Add('4', true);
            _numerics.Add('5', true);
            _numerics.Add('6', true);
            _numerics.Add('7', true);
            _numerics.Add('8', true);
            _numerics.Add('9', true);

            #endregion

            #region xml关键字

            _xmlKeywords.Add('&', "&amp;");
            _xmlKeywords.Add('<', "&lt;");
            _xmlKeywords.Add('>', "&gt;");
            _xmlKeywords.Add('\'', "&apos;");
            _xmlKeywords.Add('"', "&quot;");

            #endregion

        }

    }
}
