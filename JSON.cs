using System;
using System.Collections.Generic;
using System.Text;
using System.Reflection;
using System.Globalization;
using System.Collections;
using System.Collections.Specialized;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;

using CodeArt.Runtime;

namespace CodeArt.Text
{
    public static class JSON
    {
        public static string WriteValue(object value)
        {
            StringBuilder sb = new StringBuilder();
            WriteValue(sb, value);
            return sb.ToString();
        }

        public static void WriteValue(StringBuilder sb, object value)
        {
            if (value == null || value == System.DBNull.Value)
            {
                sb.Append("null");
            }
            else if (value is string || value is Guid)
            {
                WriteString(sb, value.ToString());
            }
            else if (value is bool)
            {
                sb.Append(value.ToString().ToLower());
            }
            else if(value is double ||
                    value is float ||
                    value is long ||
                    value is int ||
                    value is short ||
                    value is byte ||
                    value is decimal)
            {
                sb.AppendFormat(CultureInfo.InvariantCulture.NumberFormat, "{0}", value);
            }
            else if (value.GetType().IsEnum)
            {
               sb.Append(System.Convert.ToInt32(value));
            }
            else if (value is DateTime)
            {
                sb.Append("new Date(\"");
                sb.Append(((DateTime)value).ToString("MMMM, d yyyy HH:mm:ss", new CultureInfo("en-US", false).DateTimeFormat));
                sb.Append("\")");
            }
            else if (value is Hashtable)
            {
                WriteHashtable(sb, value as Hashtable);
            }
            else if (value is IDictionary)
            {
                WriteDictionary(sb, value as IDictionary);
            }
            else if (value is IEnumerable)
            {
                WriteEnumerable(sb, value as IEnumerable);
            }
            else
            {
                WriteObject(sb, value);
            }
        }

        public static void WriteDictionary(StringBuilder sb, IDictionary value)
        {
            sb.Append("{");
            foreach (DictionaryEntry item in value)
            {
                WriteValue(sb, item.Key);
                sb.Append(":");
                WriteValue(sb, item.Value);
                sb.Append(",");
            }
            if (value.Count > 0) --sb.Length;
            sb.Append("}");
        }

        public static void WriteString(StringBuilder sb, string value)
        {
            sb.Append("\"");
            foreach (char c in value)
            {
                switch (c)
                {
                    case '\"':
                        sb.Append("\\\"");
                        break;
                    case '\\':
                        sb.Append("\\\\");
                        break;
                    case '\b':
                        sb.Append("\\b");
                        break;
                    case '\f':
                        sb.Append("\\f");
                        break;
                    case '\n':
                        sb.Append("\\n");
                        break;
                    case '\r':
                        sb.Append("\\r");
                        break;
                    case '\t':
                        sb.Append("\\t");
                        break;
                    default:
                        //int i = (int)c;
                        //if (i < 32 || i > 127)
                        //{
                        //    sb.AppendFormat("\\u{0:X04}", i);
                        //}
                        //else
                        //{
                        //    sb.Append(c);
                        //}
                        sb.Append(c);
                        break;
                }
            }
            sb.Append("\"");
        }

        public static void WriteObject(StringBuilder sb, object value)
        {
            //���ݿɶ������Ժʹ���JSON��ǩ���ֶδ���json����
            MemberInfo[] members = value.GetType().GetPropertyAndFields();
            sb.Append("{");
            bool hasMembers = false;

            foreach (MemberInfo member in members)
            {
                string jsonName = member.Name;
                bool hasValue = false;
                object val = null;
                if ((member.MemberType & MemberTypes.Field) == MemberTypes.Field)
                {
                    FieldInfo field = (FieldInfo)member;
                    val = field.GetValue(value);
                    hasValue = true;
                }
                else if ((member.MemberType & MemberTypes.Property) == MemberTypes.Property)
                {
                    PropertyInfo property = (PropertyInfo)member;
                    if (property.CanRead && property.GetIndexParameters().Length == 0)
                    {
                        val = property.GetValue(value, null);
                        hasValue = true;
                    }
                }
                if (hasValue)
                {
                    sb.Append(jsonName == string.Empty ? member.Name : jsonName);
                    sb.Append(":");
                    WriteValue(sb, val);
                    sb.Append(",");
                    hasMembers = true;
                }
            }
            if (hasMembers) --sb.Length;
            sb.Append("}");

        }

        public static void WriteHashtable(StringBuilder sb, Hashtable value)
        {
            bool hasItems = false;
            sb.Append("{");
            foreach (string key in value.Keys)
            {
                sb.AppendFormat("\"{0}\":", key.ToLower());
                WriteValue(sb, value[key]);
                sb.Append(",");
                hasItems = true;
            }
            //�Ƴ����Ķ���
            if (hasItems) --sb.Length;
            sb.Append("}");
        }

        public static void WriteEnumerable(StringBuilder sb, IEnumerable value)
        {
            bool hasItems = false;
            sb.Append("[");
            foreach (object val in value)
            {
                WriteValue(sb, val);
                sb.Append(",");
                hasItems = true;
            }

            if (hasItems) --sb.Length;
            sb.Append("]");
        }

        public static string GetCode(object value)
        {
            StringBuilder sb = new StringBuilder();
            try
            {
                WriteValue(sb, value);
            }
            catch (Exception ex)
            {
                throw ex;
            }

            return sb.ToString();
        }

    }
}
