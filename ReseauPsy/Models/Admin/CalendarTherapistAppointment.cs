using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ReseauPsy.Models.Admin
{
    public class CalendarTherapistAppointment
    {
        public string id { get; set; }
        public string resourceId { get; set; }
        public string title { get; set; }
        public string start { get; set; }
        public string end { get; set; }

    }
}