using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Business;
using Library.Helper;

namespace ReseauPsy.ViewModel.Therapist
{
    public class ConfirmPresenceViewModel
    {
        public List<GetListClientToConfirmPresence_Result> Clients { get; set; }



        public ConfirmPresenceViewModel(ReseauPsyEntities _context, int therapistId)
        {
            this.Clients = _context.GetListClientToConfirmPresence(therapistId).ToList();

            foreach (var client in Clients)
            {
                client.ClientName = client.ClientName.Decode();
            }

        }
    }



}