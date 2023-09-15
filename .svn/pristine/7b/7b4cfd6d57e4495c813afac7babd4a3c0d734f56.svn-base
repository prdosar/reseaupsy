using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace Library.Helper
{
    public static class StringExtentions
    {
        public static string Decode(this string s)
        {

            StringWriter decodedSting = new StringWriter();
            HttpUtility.HtmlDecode(s, decodedSting);

            return decodedSting.ToString();
        }
    }
}
