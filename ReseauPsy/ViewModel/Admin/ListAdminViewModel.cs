using Business;
using Microsoft.AspNet.Identity;
using ReseauPsy.Models;
using ReseauPsy.Resources;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Web;
using System.Web.Mvc;

namespace ReseauPsy.ViewModel.Admin
{
    public class ListAdminViewModel
    {

        public List<GetListAdmin_Result> Admins { get; set; }
        public List<SelectListItem> Status { get; set; }
        public int NbPage { get; set; }
        public int NbPagerPageShown { get; set; }
        public int AdminId { get; set; }




        public ListAdminViewModel()
        {
            Status = new List<SelectListItem>();
            this.NbPagerPageShown = WebSiteProperties.NbPagerShown;
        }

        /// <summary>
        /// Constructor to return the list of admin
        /// </summary>
        /// <param name="_context">Database Context</param>
        /// <param name="isActive">Determine if we pick deleted or not deleted admin</param>
        public ListAdminViewModel(ReseauPsyEntities _context)
            : this()
        {
            Status.Add(new SelectListItem() { Text = Global.Active, Value = "active"});
            Status.Add(new SelectListItem() { Text = Global.Inactive, Value = "inactive"});

            Admins = _context.GetListAdmin(
                false,
                1,
                WebSiteProperties.NbResultPerPage)
                .ToList();

            var count = _context.GetListAdminCount(false);

            this.NbPage =
                Convert.ToInt32(
                    Math.Ceiling(
                        Convert.ToDecimal(_context.GetListAdminCount(false).First())
                        /
                        Convert.ToDecimal(WebSiteProperties.NbResultPerPage)
                    )
                );

            //this.AdmminId =  HttpContext.Current.User.Identity.GetUserId();
            string aspNetId =  HttpContext.Current.User.Identity.GetUserId();

            AdminId = _context.Admins
                .Where(x => x.AspNetUsersId == aspNetId)
                .Select(x => x.Id)
                .FirstOrDefault();

        }
    }
}