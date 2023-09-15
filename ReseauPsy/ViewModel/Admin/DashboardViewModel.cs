using Business;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ReseauPsy.ViewModel.Admin
{
    public class DashboardViewModel
    {
        public int CountRequestToDo { get; set; }
        public int CountCurrentAppointment { get; set; }
        public int CountBillToPay { get; set; }

        public DashboardViewModel(ReseauPsyEntities _context)
        {
            var dashboardInfos = _context.GetInfosAdminDashboard().ToList();

            this.CountRequestToDo = dashboardInfos[0].RequestToDo.Value;
            this.CountCurrentAppointment = dashboardInfos[0].CurrentAppointmentCount.Value;
            this.CountBillToPay = dashboardInfos[0].BillToPayCount.Value;
        }
    }
}