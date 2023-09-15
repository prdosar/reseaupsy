using Business;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ReseauPsy.Models
{
    public class GenerateClientReceiptExcel
    {
        public string SheetName { get; set; }

        public GenerateClientReceiptExcel(DateTime? startDate, DateTime? endDate, string clientName, ReseauPsyEntities context)
        {

        }
    }
}