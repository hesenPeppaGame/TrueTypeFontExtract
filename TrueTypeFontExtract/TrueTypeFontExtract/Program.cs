using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Windows.Media;

/// <summary>
/// 将指定的字体文件中的文字全部获取
/// </summary>
namespace TrueTypeFontExtract
{
    class Program
    {
        static void Main(string[] args)
        {
            string str = GetFontChars("D://msjhl.ttc");
            File.WriteAllText("D://output.txt", str);
        }

        /// <summary>
        /// 获取一个字体里面所有的文字
        /// </summary>
        /// <param name="fontPath"></param>
        /// <returns></returns>
        public static string GetFontChars(string fontPath)
        {
            if (!File.Exists(fontPath))
            {
                throw new Exception($"文件不存在===>{fontPath}");
            }

            GlyphTypeface ttf = new GlyphTypeface(new Uri(fontPath));
            /// <summary>
            /// Returns nominal mapping of Unicode codepoint to glyph index as defined by the font 'CMAP' table.
            /// </summary>
            /// <SecurityNote>
            ///   Critical: May potentially leak a writeable cmap.
            ///   Safe: The cmap IDictionary exposure is read only.
            ///  </SecurityNote>
            IDictionary<int, ushort> map = ttf.CharacterToGlyphMap;
            StringBuilder sb = new StringBuilder();
            int count = 0;
            foreach (var item in map)
            {
                var str = DecodeString(item.Value); 
                sb.Append(str);
                if (count % 20 == 0)
                {
                    sb.AppendLine();
                }

                count++;
            }

            return sb.ToString();
        }

        /// <summary>
        /// Ushort值转换成Unicode编码的文字
        /// </summary>
        /// <param name="text">解析文字</param>
        /// <returns>标准unicode编码</returns>
        public static string DecodeString(ushort text)
        {
            StringBuilder builder = new StringBuilder();
            //builder.Append("\\u");
            builder.Append(UShortToHex(text));
            //return DecodeString(builder.ToString());

            int num = Int32.Parse(builder.ToString(), System.Globalization.NumberStyles.HexNumber);
            return Char.ConvertFromUtf32(num);
        }

        /// <summary>
        /// 反解析Unicode编码文字
        /// </summary>
        /// <param name="unicode"></param>
        /// <returns></returns>
        public static string DecodeString(string unicode)
        {
            if (string.IsNullOrEmpty(unicode))
            {
                return string.Empty;
            }

            string[] ls = unicode.Split(new string[] { "\\u" }, StringSplitOptions.RemoveEmptyEntries);
            StringBuilder builder = new StringBuilder();
            int len = ls.Length;
            for (int i = 0; i < len; i++)
            {
                builder.Append(Convert.ToChar(ushort.Parse(ls[i], System.Globalization.NumberStyles.HexNumber)));
            }

            return builder.ToString();
        }

        /// <summary>
        /// Ushort值转换成Hex值
        /// </summary>
        /// <param name="n"></param>
        /// <returns></returns>
        private static char[] UShortToHex(ushort n)
        {
            int num;
            char[] hex = new char[4];
            for (int i = 0; i < 4; i++)
            {
                num = n % 16;

                if (num < 10)
                    hex[3 - i] = (char)('0' + num);
                else
                    hex[3 - i] = (char)('A' + (num - 10));

                n >>= 4;
            }

            return hex;
        }
    }
}
