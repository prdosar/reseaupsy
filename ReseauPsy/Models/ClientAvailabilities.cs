using Business;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ReseauPsy.Models
{
    public class ClientAvailabilities
    {
        public int ClientId { get; set; }
        public int DayOfTheWeek_PeriodOfTheDay_Id { get; set; }

        public ClientAvailabilities()
        {

        }

        public ClientAvailabilities(ClientAvailability clientAvailability)
        {
            this.ClientId = clientAvailability.ClientId;
            this.DayOfTheWeek_PeriodOfTheDay_Id = clientAvailability.DayOfTheWeek_PeriodOfTheDay_Id;
        }
    }
}