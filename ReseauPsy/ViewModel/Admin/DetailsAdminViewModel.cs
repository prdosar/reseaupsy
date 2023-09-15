using Business;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ReseauPsy.ViewModel.Admin
{
    public class DetailsAdminViewModel
    {
        public int? AdminId { get; set; }
        public string AdminName { get; set; }
        public string AdminEmail { get; set; }

        public DetailsAdminViewModel()
        {

        }

        public DetailsAdminViewModel(ReseauPsyEntities _context, int adminId)
        {
            this.AdminId = adminId;

            var admin = _context.Admins
                .Where(x => x.Id == adminId)
                .First();

            this.AdminName = admin.Name;
            this.AdminEmail = admin.AspNetUser.Email;
        }
    }
}