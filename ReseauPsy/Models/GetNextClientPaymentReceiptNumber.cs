using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Business;

namespace ReseauPsy.Models
{
    public class GetNextClientPaymentReceiptNumber
    {
        public int Number { get; set; }

        public GetNextClientPaymentReceiptNumber(ReseauPsyEntities context)
        {
            int? lastNumber = context.ClientAppointments
                .Where(x => x.ClientPaymentReceiptNumber != null)
                .OrderByDescending(x => x.ClientPaymentReceiptNumber)
                .Select(x => x.ClientPaymentReceiptNumber)
                .FirstOrDefault();

            if (lastNumber == null)
                Number = 1;
            else
                Number = lastNumber.Value + 1;
        }
    }
}