using Business;
using HelpersLibrary;
using Library.Helper;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using ReseauPsy.Helper;
using ReseauPsy.Models;
using ReseauPsy.Models.Therapist;
using ReseauPsy.ViewModel.Admin;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Security;

namespace ReseauPsy.LeadManagement
{
    public class Therapist
    {
        public int Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public string PhoneNumber { get; set; }
        public Gender Gender { get; set; }
        public string Adress { get; set; }
        public string City { get; set; }
        public string PostalCode { get; set; }
        public Region Region { get; set; }
        public Accreditation Accreditation { get; set; }
        public string OpqNumber { get; set; }
        public bool? TakeClientFromAssuranceCompany { get; set; }
        public bool? TakeClientFromThirdPartyPayer { get; set; }
        public bool? IsInterestedInFormation { get; set; }
        public DateTime HiringDate { get; set; }

        public bool EditProfile(TherapistProfileDatas datas, ReseauPsyEntities _context, Business.Therapist therapist)
        {
            #region we delete the whole content of the many to many relationship tables

            _context.TherapistProfessionalTitles.RemoveRange(therapist.TherapistProfessionalTitles);
            _context.TherapistClientTypes.RemoveRange(therapist.TherapistClientTypes);
            _context.TherapistLanguages.RemoveRange(therapist.TherapistLanguages);
            _context.TherapistClientAgeRanges.RemoveRange(therapist.TherapistClientAgeRanges);
            _context.TherapistConsultationTypes.RemoveRange(therapist.TherapistConsultationTypes);
            _context.TherapistAvailabilities.RemoveRange(therapist.TherapistAvailabilities);


            #endregion


            ModifyIdentityUser.ModifyEmail(therapist.AspNetUserId, datas.Email);
            therapist.MaxWeeklyRequest = datas.MaxWeeklyRequest;

            var therapistInfo = _context.TherapistInfo.Create();

            therapistInfo.TherapistId = therapist.Id;
            therapistInfo.FirstName = datas.FirstName;
            therapistInfo.LastName = datas.LastName;
            therapistInfo.Email = datas.Email;
            therapistInfo.PhoneNumber = datas.PhoneNumber;
            therapistInfo.GenderId = datas.GenderId;
            therapistInfo.Adress = datas.Adress;
            therapistInfo.City = datas.City;
            therapistInfo.PostalCode = datas.PostalCode;
            therapistInfo.RegionId = datas.RegionId;
            therapistInfo.AccreditationId = datas.AccreditationId;
            therapistInfo.CertificationAndSpecialities = datas.Certification;
            therapistInfo.TakeClientFromAssuranceCompany = datas.TakeAssuaranceClient;
            therapistInfo.TakeClientFromThirdPartyPayer = datas.TakeThirdPartyClient;
            therapistInfo.IsInterestedForFormation = datas.IsInterestedFormation;
            therapistInfo.TpsNumber = datas.TpsNumber;
            therapistInfo.TvqNumber = datas.TvqNumber;
            therapistInfo.ModificationDate = DateTime.Now;

            //on met le OPQ number si c'est un psychologue ou un psychothérapeute
            if (datas.AccreditationId == 1 || datas.AccreditationId == 2)
                therapistInfo.OpqNumber = datas.OpqNumber;

            foreach (int id in datas.ProfessionalTitlesIds)
            {
                var therapistProfessionalTitle = new TherapistProfessionalTitle();
                therapistProfessionalTitle.Therapist = therapist;
                therapistProfessionalTitle.ProfessionalTitleId = id;
                therapist.TherapistProfessionalTitles.Add(therapistProfessionalTitle);
            }

            foreach (int id in datas.ClientTypeIds)
            {
                var therapistClientType = new TherapistClientType();
                therapistClientType.Therapist = therapist;
                therapistClientType.ClientTypeId = id;
                therapist.TherapistClientTypes.Add(therapistClientType);
            }

            foreach (int id in datas.ClientAgeIds)
            {
                var therapistClientAgeRange = new TherapistClientAgeRange();
                therapistClientAgeRange.Therapist = therapist;
                therapistClientAgeRange.ClientAgeRangeId = id;
                therapist.TherapistClientAgeRanges.Add(therapistClientAgeRange);
            }

            foreach (int id in datas.ClientTypeIds)
            {
                var therapistConsultationType = new TherapistConsultationType();
                therapistConsultationType.Therapist = therapist;
                therapistConsultationType.ConsultationTypeId = id;
                therapist.TherapistConsultationTypes.Add(therapistConsultationType);

            }

            foreach (int id in datas.LanguageIds)
            {
                var therapistLanguage = new TherapistLanguage();
                therapistLanguage.Therapist = therapist;
                therapistLanguage.LanguageId = id;
                therapist.TherapistLanguages.Add(therapistLanguage);
            }

            foreach (int availability in datas.AvailabilityIds)
            {
                var therapistAvailability = new TherapistAvailability();
                therapistAvailability.Therapist = therapist;
                therapistAvailability.DayOfTheWeek_PeriodOfTheDay_Id = availability;
                therapist.TherapistAvailabilities.Add(therapistAvailability);
            }

            _context.TherapistInfo.Add(therapistInfo);

            try
            {
                _context.SaveChanges();
                return true;
            }
            catch (Exception ex)
            {
                return false ;
            }


        }

    }
}