using Business;
using Microsoft.AspNet.Identity;
using ReseauPsy.Models;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.Owin.Host.SystemWeb;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.UI;
using Library.Helper;
using HelpersLibrary;
using ReseauPsy.Controllers.Admin;
using Microsoft.Owin.Security;
using ReseauPsy.Resources;
using ReseauPsy.Models.Admin;
using System.Resources;
using System.Globalization;

namespace ReseauPsy.LeadManagement
{
    public class Admin
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public string AspNetUserId { get; set; }
        public string AspNetRole { get; set; }


        /// <summary>
        /// Création d'un profil admin dans identity et ajout de l'Admin dans la table admins
        /// </summary>
        /// <param name="_context">Database context</param>
        /// <returns>True si fonctionne, false si la création a échoué</returns>
        public bool CreateAdmin(ReseauPsyEntities _context)
        {
            try
            {
                //On crée l'user dans identity
                ApplicationUserManager userManager = HttpContext.Current.GetOwinContext().GetUserManager<ApplicationUserManager>();
                ApplicationUser user = new ApplicationUser { UserName = Email, Email = Email };
                IdentityResult resultCreateUser = userManager.Create(user, Password);

                //Si l'ajout dans identity n'a pas fonctionné
                if (!resultCreateUser.Succeeded)
                    return false;

                //On ajoute le role
                userManager.AddToRole(user.Id, RoleName.AdminFullAccess);
                AspNetUserId = user.Id;


                //On crée l'admin dans la table admin
                var admin = new Business.Admin();
                admin.Name = Name;
                admin.CreatedDate = DateTime.Now;
                admin.AspNetUsersId = user.Id;

                _context.Admins.Add(admin);
                _context.SaveChanges();

                return true;
            }
            catch (Exception)
            {
                return false;
            }

        }

        /// <summary>
        /// Modify the identity profile and the admin profile
        /// </summary>
        /// <param name="_context">Database context</param>
        /// <returns>True if works, false if failed</returns>
        public bool ModifyAdmin(ReseauPsyEntities _context,string name, string email, string password)
        {
            try
            {
                //Modify the identity profile
                ApplicationUserManager userManager = HttpContext.Current.GetOwinContext().GetUserManager<ApplicationUserManager>();
                var identityUser = userManager.FindById(this.AspNetUserId);
                userManager.SetEmail(identityUser.Id, email);
                identityUser.UserName = email;
                userManager.Update(identityUser);

                //Modify the admin profile
                var admin = _context.Admins.Single(x => x.Id == Id);
                admin.Name = name;
                admin.ModifiedDate = DateTime.Now;

                //Change password if have to
                if (!string.IsNullOrWhiteSpace(password))
                {
                    string resetToken = userManager.GeneratePasswordResetToken(identityUser.Id);
                    var result = userManager.ResetPassword(this.AspNetUserId, resetToken, password);
                }

                _context.SaveChanges();

                return true;
            }
            catch (Exception)
            {
                return false;
            }

        }

        /// <summary>
        /// Set the isDeleted field in the table admin in the database to true
        /// </summary>
        /// <param name="_context">Database context</param>
        public void DeleteAdmin(ReseauPsyEntities _context)
        {
            var adminDb = _context.Admins.Single(x => x.Id == this.Id);
            adminDb.IsDeleted = true;
            _context.SaveChanges();
        }

        /// <summary>
        /// Fonction pour mettre le isDeleted d'una admin à false
        /// </summary>
        /// <param name="_context">database context</param>
        public void RestoreAdmin(ReseauPsyEntities _context)
        {
            var adminDb = _context.Admins.Single(x => x.Id == this.Id);
            adminDb.IsDeleted = false;
            _context.SaveChanges();
        }

        /// <summary>
        /// Set the isdeleted of the client to true and send an email to the client
        /// to let him know is reuqest is now deleted
        /// </summary>
        /// <param name="_context">DB context</param>
        /// <param name="clientId">The client Id</param>
        public bool DeleteClientRequest(ReseauPsyEntities _context, int clientId, int deleteReasonId, string notes)
        {
            var client = _context.Clients
                .Where(x => x.Id == clientId)
                .Single();

            client.IsDeleted = true;
            client.ReasonDeleteClientRequestId = deleteReasonId;
            client.DeletedAdditionalNotes = notes;

            var resourceManager = new ResourceManager(typeof(Email));
            var culture = new CultureInfo(client.LanguageId == 1 ? "fr-CA" : "en-CA");

            string emailContent = resourceManager.GetString("AppointmentRequestDeleted_Subject", culture);
            string emailSubject = resourceManager.GetString("AppointmentRequestDeleted_Content", culture);

            var clientInfos = new GetClientInfos(_context, clientId);
            clientInfos.GetMostRecentClientInfo();

            try
            {
                _context.SaveChanges();
                EmailHelper.Send(emailSubject, emailContent, clientInfos.Email, "");
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }


        /// <summary>
        /// Assign the client request to either a therapist or an external association
        /// </summary>
        /// <param name="_context">DB context</param>
        /// <param name="isSendTherapist">True to send to therapist, false to send to external association</param>
        /// <param name="clientId">the client id</param>
        /// <param name="therapistOrAssociationId">Either the therapistId or the externalAssocaitionId</param>
        /// <param name="notes">The extra note wrote in the assignation modal</param>
        /// <returns>true if worked, false if error</returns>
        public bool AssignClientRequestToTherapistOrAssociation(ReseauPsyEntities _context, bool isSendTherapist, int clientId, int therapistOrAssociationId, string notes)
        {
            var client = _context.Clients
                .Where(x => x.Id == clientId)
                .Single();

            if (isSendTherapist)
            {
                #region Assigned to therapist

                var therapistClientRequest = new TherapistClientRequest();

                therapistClientRequest.ClientId = client.Id;
                therapistClientRequest.TherapistId = therapistOrAssociationId;
                therapistClientRequest.AdditionalMessageFromAdmin = string.IsNullOrWhiteSpace(notes) ? null : notes;
                therapistClientRequest.RequestSentDate = DateTime.Now;

                _context.TherapistClientRequest.Add(therapistClientRequest);

                try
                {
                    _context.SaveChanges();
                    return true;
                }
                catch (Exception ex)
                {
                    return false;
                }

                #endregion
            }
            else
            {
                #region Assigned to assocation

                var clientStatus = _context.ExternalAssociationClientSentStatus
                    .Where(x => x.Id == 1) //Not defined
                    .Single();

                var externalAssociation = _context.ExternalAssociations
                    .Where(x => x.Id == therapistOrAssociationId)
                    .Single();

                var externalAssociationClientSent = new ExternalAssociationClientsSent();

                externalAssociationClientSent.ExternalAssociation = externalAssociation;
                externalAssociationClientSent.Clients = client;
                externalAssociationClientSent.ExternalAssociationClientSentStatu = clientStatus;
                externalAssociationClientSent.SentToAssociationDate = DateTime.Now;
                externalAssociationClientSent.StatusChangedDate = DateTime.Now;

                _context.ExternalAssociationClientsSents.Add(externalAssociationClientSent);

                client.ExternalAssociationId = externalAssociation.Id;

                #region Email to association


                var resourceManager = new ResourceManager(typeof(Email));

                var culture = new CultureInfo(externalAssociation.LanguageId == 1 ? "fr-CA" : "en-CA");


                string emailSubject = resourceManager.GetString("ExternalAssociationSend_Subject", culture);




                string emailAdditionnalClientMessage = string.IsNullOrWhiteSpace(client.Message) ?
                    "" :
                    String.Format(resourceManager.GetString("ExternalAssociationSend_PartialClientMessage", culture),
                        client.Message
                    );


                string emailAdditionnalAdminNote = string.IsNullOrWhiteSpace(notes) ?
                  "" :
                  String.Format(resourceManager.GetString("ExternalAssociationSend_PartialAdminNote", culture),
                      notes
                  );



                var clientAvailabilities = _context.ClientAvailabilities
                    .Where(x => x.ClientId == client.Id)
                    .ToList();

                var clientDayIdAvailable = clientAvailabilities
                    .Select(x => x.DayOfTheWeek_PeriodOfTheDay.DaysOfTheWeek)
                    .ToList().Distinct();


                string availability = "";

                foreach (var day in clientDayIdAvailable)
                {
                    availability += $"{day.DayOfTheWeek}: ";

                    foreach (var period in clientAvailabilities
                        .Where(x => x.DayOfTheWeek_PeriodOfTheDay.DaysOfTheWeek == day)
                        .Select(x => x.DayOfTheWeek_PeriodOfTheDay.PeriodsOfTheDay)
                        .ToList())
                    {
                        availability += $"{period.PeriodOfTheDay} ";
                    }

                    availability += "<br />";
                }


                var clientInfos = new GetClientInfos(_context, clientId);
                clientInfos.GetMostRecentClientInfo();

                string emailContent = String.Format(resourceManager.GetString("ExternalAssociationSend_Content", culture),
                    clientInfos.FirstName + " " + clientInfos.LastName,
                    clientInfos.Email,
                    clientInfos.PhoneNumber,
                    clientInfos.City,
                    clientInfos.Region.Region1,
                    clientInfos.PostalCode,
                    client.ConsultationReasons.ConsultationReason1,
                    client.ClientsAgeRange.ClientAgeRange,
                    emailAdditionnalClientMessage,
                    client.RequestDate,
                    availability,
                    emailAdditionnalAdminNote
                );

                

                #endregion

                try
                {
                    _context.SaveChanges();
                    EmailHelper.Send(emailSubject, emailContent, externalAssociation.Email, "");

                    return true;
                }
                catch (Exception ex)
                {
                    return false;
                }

                #endregion
            }
        }

        /// <summary>
        /// Restore a deleted client request in the database
        /// </summary>
        /// <param name="_context">DB context</param>
        /// <param name="clientId">The client id</param>
        /// <returns>returns true if no error, otherwise, false</returns>
        public bool RestoreClientRequest(ReseauPsyEntities _context, int clientId)
        {
            var client = _context.Clients
                .Where(x => x.Id == clientId)
                .Single();

            client.IsDeleted = false;
            client.DeletedAdditionalNotes = null;
            client.ReasonDeleteClientRequestId = null;

            try
            {
                _context.SaveChanges();
                return true;
            }
            catch (Exception)
            {
                return false;
            }        
        }

        /// <summary>
        /// Retrieve the client request from the therapist waiting list
        /// </summary>
        /// <param name="_context">DB context</param>
        /// <param name="clientId">Client id</param>
        /// <returns>returns true if worked, false if error</returns>
        public bool RetrieveClientRequest(ReseauPsyEntities _context, int clientId)
        {
            var client = _context.Clients
                .Where(x => x.Id == clientId)
                .Single();

            var therapistClientRequest = _context.TherapistClientRequest
                .Where(x => x.ClientId == client.Id)
                .OrderByDescending(x => x.RequestSentDate)
                .First();

            //Cant retrieve the request since it was already accepted by a therapist
            if (therapistClientRequest.IsAccepted == true)
                return false;

            //The request is already refused, so no need to go further
            if (therapistClientRequest.IsAccepted == false)
                return true;

            therapistClientRequest.IsAccepted = false;

            try
            {
                _context.SaveChanges();
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }


        public Admin()
        {

        }

        public Admin(Business.Admin adminDb)
        {
            Id = adminDb.Id;
            Name = adminDb.Name;
            Email = adminDb.AspNetUser.Email;
            AspNetUserId = adminDb.AspNetUsersId;
            AspNetRole = adminDb.AspNetUser.AspNetRoles.Select(x => x.Name).FirstOrDefault();
        }
    }
}