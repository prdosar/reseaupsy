using Business;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ReseauPsy.Models.Therapist
{
    public class EditProfile
    {
        public bool Succeeded { get; set; }
        private TherapistProfileDatas _datas;
        private ReseauPsyEntities _context;
        private Business.Therapist _therapist;

        public EditProfile(TherapistProfileDatas datas, ReseauPsyEntities context, Business.Therapist therapist)
        {
            Succeeded = true;
            _datas = datas;
            _context = context;
            _therapist = therapist;


            ModifyTherapistBaseInfo();
            SetTherapistInformations();
            SetTherapistSkills();

            try
            {
                _context.SaveChanges();
            }
            catch (Exception ex)
            {
                Succeeded = false;
            }
        }

        private void ModifyTherapistBaseInfo()
        {
            _therapist.MaxWeeklyRequest = _datas.MaxWeeklyRequest;
        }
        private void SetTherapistInformations()
        {
            var therapistInfo = new TherapistInfo();
            therapistInfo.Therapists = _therapist;
            therapistInfo.FirstName = _datas.FirstName;
            therapistInfo.LastName = _datas.LastName;
            therapistInfo.Email = _datas.Email;
            therapistInfo.PhoneNumber = _datas.PhoneNumber;
            therapistInfo.GenderId = _datas.GenderId;
            therapistInfo.Adress = _datas.Adress;
            therapistInfo.City = _datas.City;
            therapistInfo.PostalCode = _datas.PostalCode;
            therapistInfo.RegionId = _datas.RegionId;
            therapistInfo.AccreditationId = _datas.AccreditationId;
            therapistInfo.CertificationAndSpecialities = _datas.Certification;
            therapistInfo.TakeClientFromAssuranceCompany = _datas.TakeAssuaranceClient;
            therapistInfo.TakeClientFromThirdPartyPayer = _datas.TakeThirdPartyClient;
            therapistInfo.IsInterestedForFormation = _datas.IsInterestedFormation;
            therapistInfo.ModificationDate = DateTime.Now;
            therapistInfo.TpsNumber = _datas.TpsNumber;
            therapistInfo.TvqNumber = _datas.TvqNumber;
            therapistInfo.OpqNumber = null;

            //on met le OPQ number si c'est un psychologue ou un psychothérapeute
            if (_datas.AccreditationId == 1 || _datas.AccreditationId == 2)
                therapistInfo.OpqNumber = _datas.OpqNumber;

            _therapist.TherapistInfo.Add(therapistInfo);
        }
        private void SetTherapistSkills()
        {
            SetProfessionalTitles();
            SetClientTypes();
            SetLangugages();
            SetClientAge();
            SetConsultationType();
            SetAvailabilities();
        }




        private void SetProfessionalTitles()
        {
            //On doit supprimer le range pour ensuite les ajouter de nouveau
            _context.TherapistProfessionalTitles.RemoveRange(_therapist.TherapistProfessionalTitles);

            foreach (int id in _datas.ProfessionalTitlesIds)
            {
                var therapistProfessionalTitle = new TherapistProfessionalTitle();
                therapistProfessionalTitle.Therapist = _therapist;
                therapistProfessionalTitle.ProfessionalTitleId = id;
                _therapist.TherapistProfessionalTitles.Add(therapistProfessionalTitle);
            }
        }
        private void SetClientTypes()
        {
            //On doit supprimer le range pour ensuite les ajouter de nouveau
            _context.TherapistClientTypes.RemoveRange(_therapist.TherapistClientTypes);

            foreach (int id in _datas.ClientTypeIds)
            {
                var therapistClientType = new TherapistClientType();
                therapistClientType.Therapist = _therapist;
                therapistClientType.ClientTypeId = id;
                _therapist.TherapistClientTypes.Add(therapistClientType);
            }
        }
        private void SetLangugages()
        {
            //On doit supprimer le range pour ensuite les ajouter de nouveau
            _context.TherapistLanguages.RemoveRange(_therapist.TherapistLanguages);
            foreach (int id in _datas.LanguageIds)
            {
                var therapistLanguage = new TherapistLanguage();
                therapistLanguage.Therapist = _therapist;
                therapistLanguage.LanguageId = id;
                _therapist.TherapistLanguages.Add(therapistLanguage);
            }
        }
        private void SetClientAge()
        {
            //On doit supprimer le range pour ensuite les ajouter de nouveau
            _context.TherapistClientAgeRanges.RemoveRange(_therapist.TherapistClientAgeRanges);
            foreach (int id in _datas.ClientAgeIds)
            {
                var therapistClientAgeRange = new TherapistClientAgeRange();
                therapistClientAgeRange.Therapist = _therapist;
                therapistClientAgeRange.ClientAgeRangeId = id;
                _therapist.TherapistClientAgeRanges.Add(therapistClientAgeRange);
            }
        }
        private void SetConsultationType()
        {
            //On doit supprimer le range pour ensuite les ajouter de nouveau
            _context.TherapistConsultationTypes.RemoveRange(_therapist.TherapistConsultationTypes);
            foreach (int id in _datas.ConsultationTypesIds)
            {
                var therapistConsultationType = new TherapistConsultationType();
                therapistConsultationType.Therapist = _therapist;
                therapistConsultationType.ConsultationTypeId = id;
                _therapist.TherapistConsultationTypes.Add(therapistConsultationType);

            }
        }
        private void SetAvailabilities()
        {
            //On doit supprimer le range pour ensuite les ajouter de nouveau
            _context.TherapistAvailabilities.RemoveRange(_therapist.TherapistAvailabilities);

            foreach (int availability in _datas.AvailabilityIds)
            {
                var therapistAvailability = new TherapistAvailability();
                therapistAvailability.Therapist = _therapist;
                therapistAvailability.DayOfTheWeek_PeriodOfTheDay_Id = availability;
                _therapist.TherapistAvailabilities.Add(therapistAvailability);
            }
        }
    }
}