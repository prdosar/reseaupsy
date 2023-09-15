using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using TuesPechkin;

namespace ReseauPsy.PDF.Configuration
{
    public class TuesPechkinConverter
    {
        private static IConverter converter;

        public static IConverter Converter
        {
            get
            {
                if (converter == null)
                {
                    converter = new ThreadSafeConverter(new RemotingToolset<PdfToolset>(new WinAnyCPUEmbeddedDeployment(new TempFolderDeployment())));
                }
                return converter;
            }
        }
    }
}