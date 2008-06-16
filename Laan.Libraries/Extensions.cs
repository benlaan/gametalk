using System;
using System.Threading;
using Laan.Utilities.IO;
using System.Text;

/*******************************************************************************

	NOTE: This code only works in dotNET 3.5 or later
	
	It employs class helpers (extension methods) to allow new methods on 3rd
	party classes (or dotNET Framework classes)

*******************************************************************************/

namespace Laan.Utilities.Extensions
{
	public static class StringHelper
	{
	    public static bool IsNullOrEmpty(this string input)
	    {
			return System.String.IsNullOrEmpty(input);
		}
	
        public static string XOR(this string input, string strKey)
        {
            if (input.IsNullOrEmpty())
                return input;

            string strEncoded = string.Empty;

            int nKeyIndex = 0;
            for (int i = 0; i < input.Length; i++)
            {
                strEncoded += Convert.ToChar(input[i] ^ strKey[nKeyIndex]);
                nKeyIndex++;
                if (nKeyIndex == strKey.Length)
                    nKeyIndex = 0;

            }
            return strEncoded;
        }

        public static string ToTitleCase(this string input)
        {
            StringBuilder titleCase = new StringBuilder(input.Length);
            for (int index = 0; index < input.Length; index++)
            {
                if (index == 0 || index > 0 && input[index - 1] == ' ')
                    titleCase.Append(input[index].ToString().ToUpper());
                else
                    titleCase.Append(input[index].ToString().ToLower());
            }
            return titleCase.ToString();
        }

        public static string ToCamelCase(this string input)
        {
            if (input.Length == 0)
                return "";
            return input.Left(1).ToLower() + input.Remove(0, 1);
        }

        public static string Left(this string input, int length)
        {
            int len = length > input.Length ? input.Length : length;
            return input.Substring(0, len);
        }

        public static string Right(this string input, int length)
        {
            int maxLen = input.Length - length;
            maxLen = maxLen == 0 ? 0 : Math.Min(input.Length, length);

            return input.Substring(input.Length - maxLen, maxLen);
        }
    
        public static string Formatted(this string format, params object[] data)
        {
			return System.String.Format(format, data);
		}

	    public static string Reverse(this string input)
 	    {
	       char[] chars = input.ToCharArray();
	       Array.Reverse(chars);
	       return new string(chars);
	    }
	}

	public static class StringArrayHelper
	{
		public static string AsCommaText(this string[] items)
		{
			string commaText = "";
			foreach (string item in items)
			    commaText += (commaText == "" ? "" : ",") + item;
			    
			return commaText;
		}
	}
}
