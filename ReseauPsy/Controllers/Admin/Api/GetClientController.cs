using Business;
using Library.Helper;
using ReseauPsy.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace ReseauPsy.Controllers.Admin.Api
{
    [Authorize(Roles = RoleName.AdminFullAccess)]
    public class GetClientController : ApiController
    {

        #region Database context

        private ReseauPsyEntities _context;

        public GetClientController()
        {
            _context = new ReseauPsyEntities();
        }

        #endregion

        [HttpGet]
        public IEnumerable<GetListClientNameSuggestion_Result> GetClientSuggestion(string textEntered)
        {
            var clients = _context.GetListClientNameSuggestion(
                textEntered.ToLower(),
                null)
                .ToList();

            foreach (var client in clients)
            {
                client.ClientName = client.ClientName.Decode();
            }
            return clients;
        }
    }
}
