using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Business;
using Library.Helper;

namespace ReseauPsy.ViewModel.Therapist
{
    public class AppointmentNotPayedViewModel
    {
        public List<GetListClientAppointmentNotPayed_Result> Clients { get; set; }


        public AppointmentNotPayedViewModel(ReseauPsyEntities _context, int therapistID)
        {
            this.Clients = _context.GetListClientAppointmentNotPayed(therapistID).ToList();

            foreach (var client in Clients)
            {
                client.ClientName = client.ClientName.Decode();
                client.PhoneNumber = client.PhoneNumber.Decode();
                client.Email = client.Email.Decode();
            }
        }
    }
}