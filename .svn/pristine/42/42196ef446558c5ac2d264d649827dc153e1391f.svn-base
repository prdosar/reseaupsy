using Business;
using Library.Helper;
using Microsoft.AspNet.Identity;
using ReseauPsy.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace ReseauPsy.Controllers.Therapist.Api
{
    [Authorize(Roles = RoleName.Therapist)]
    public class GetClientTherapistController : ApiController
    {
        #region Database context

        private ReseauPsyEntities _context;

        public GetClientTherapistController()
        {
            _context = new ReseauPsyEntities();
        }

        #endregion

        [HttpGet]
        public IEnumerable<GetListClientNameSuggestion_Result> GetClientSuggestion(string textEntered)
        {
            string userIdentity = User.Identity.GetUserId();

            var therapist =  _context.Therapists
                .Where(x => x.AspNetUserId == userIdentity)
                .First();

            var clients = _context.GetListClientNameSuggestion(
                textEntered.ToLower(),
                therapist.Id)
                .ToList();

            foreach (var client in clients)
            {
                client.ClientName = client.ClientName.Decode();
            }
            return clients;
        }
    }
}
