using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Web;
using Business;
using Library.Helper;

namespace ReseauPsy.ViewModel.Admin
{
    public class StatisticsViewModel
    {
        public List<GetTherapistClosingRates_Result> TherapistsClosingRate{ get; set; }
        public List<GetCancelationCountByCancelationReason_Result> CancelationReason { get; set; }
        public int TotalCanceledAppointment { get; set; }


        public StatisticsViewModel(ReseauPsyEntities _context)
        {
            var isFrench = CultureInfo.CurrentCulture.Name == "fr-CA" ? true : false;

            this.TherapistsClosingRate = _context.GetTherapistClosingRates(
                null,
                null).ToList();

            foreach (var therapist in TherapistsClosingRate)
            {
                therapist.TherapistName = therapist.TherapistName.Decode();
            }

            this.CancelationReason = _context.GetCancelationCountByCancelationReason(
                isFrench, 
                null,
                null).ToList();


            int totalCanceldAppointment = 0;

            foreach (var cancelation in this.CancelationReason)
            {
                totalCanceldAppointment += cancelation.NbCancelation.Value;
            }

            this.TotalCanceledAppointment = totalCanceldAppointment;

        }
    }
}