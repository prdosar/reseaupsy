using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace Library.Helper
{
    public static class TextHelper
    {
        public static string DecodeHtmlEntities(string text)
        {
            StringWriter decodedSting = new StringWriter();
            HttpUtility.HtmlDecode(text, decodedSting);

            return decodedSting.ToString();
        }

        public static string EscapeDoubleQuote(string text)
        {
            return text.Replace("\"", "\\\"");
        }
    }
}
