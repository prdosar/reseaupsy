using Business;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Web;

namespace ReseauPsy.Models.Admin
{
    public class EditTherapist
    {
        public bool Succeeded { get; set; }
        private DetailTherapistDatas _datas;
        private ReseauPsyEntities _context;
        private Business.Therapist _therapist;

        public EditTherapist(DetailTherapistDatas datas, ReseauPsyEntities context, int therapistId)
        {
            Succeeded = true;
            _datas = datas;
            _context = context;
            _therapist = _context.Therapists
                .Where(x => x.Id == therapistId)
                .Single();

            ModifyTherapistBaseInfo();
            SetTherapistInformations();
            SetTherapistPayInformation();
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
            _therapist.HiringDate = _datas.HiringDate;
            _therapist.MaxWeeklyRequest = _datas.MaxWeeklyRequest;
            _therapist.AdminNotes = _datas.AdminNotes;
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
        private void SetTherapistPayInformation()
        {
            RemoveOldTherapistPayInformations();

            foreach (var newPayInfo in _datas.Wages)
            {

                string[] payInfo = newPayInfo.Split(new string[] { "--split--" }, StringSplitOptions.None);
                int oldWageId = int.Parse(payInfo[0]);
                string wageName = payInfo[1];

                decimal.TryParse(payInfo[2], NumberStyles.Any, CultureInfo.InvariantCulture, out decimal therapisWage);
                decimal.TryParse(payInfo[3], NumberStyles.Any, CultureInfo.InvariantCulture, out decimal clientCost);
                bool isDefault = bool.Parse(payInfo[4]);

                var newTherapistPayInformation = new TherapistPayInformations();
                BindFutureAppointmentToNewWage(oldWageId, newTherapistPayInformation);

                newTherapistPayInformation.Therapists = _therapist;
                newTherapistPayInformation.ChangedDate = DateTime.Now;
                newTherapistPayInformation.TherapistHourlyWage = therapisWage;
                newTherapistPayInformation.ClientHourlyCost = clientCost;
                newTherapistPayInformation.IsTaxable = _datas.IsTaxable;
                newTherapistPayInformation.PayInfoName = wageName;
                newTherapistPayInformation.IsDefault = isDefault;
                newTherapistPayInformation.IsActive = true;

                _therapist.TherapistPayInformations.Add(newTherapistPayInformation);
            }
        }

        private void BindFutureAppointmentToNewWage(int oldWageId, TherapistPayInformations newTherapistPayInformation)
        {
            if (oldWageId == 0)
                return;

            var clientAppointments = _therapist.ClientAppointments
                    .Where(x => x.TherapistPayInformationId == oldWageId && x.ClientBillGeneratedDate == null)
                    .ToList();

            foreach (var appointment in clientAppointments)
            {
                appointment.TherapistPayInformations = newTherapistPayInformation;
            }
        }
        private void RemoveOldTherapistPayInformations()
        { 
            var oldPayInfo = _therapist.TherapistPayInformations.ToList();

            foreach (var payInfo in oldPayInfo)
                payInfo.IsActive = false;
        }


        private void SetTherapistSkills()
        {
            SetProfessionalTitles();
            SetClientTypes();
            SetClientAge();
            SetConsultationType();
            SetLangugages();
            SetAvailabilities();
            SetAssociations();
            SetOfferedServices();
            SetPracticeSector();
            SetTheoreticalOrientation();
            SetSpecificSkills();
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
        private void SetAssociations()
        {
            DeletePreviousAssociation();

            foreach (string association in _datas.Associations)
            {
                //Les assocaition sont enregistrer en string, il faut les formatter ici
                //Le format est le suivant [associationName --split-- associationNumber]

                var associationSplitted = association.Split(new string[] { "--split--" }, StringSplitOptions.None);

                string associationName = associationSplitted[0];
                string associationNumber = associationSplitted[1];

                var therapistAssociation = new TherapistAssociationAndProfessionalOrder();
                therapistAssociation.Therapist = _therapist;
                therapistAssociation.AssociationName = associationName;
                therapistAssociation.AssociationNumber = associationNumber;
                therapistAssociation.CreateDate = DateTime.Now;
                _therapist.TherapistAssociationAndProfessionalOrders.Add(therapistAssociation);
            }
        }
        private void SetOfferedServices()
        {
            //On doit supprimer le range pour ensuite les ajouter de nouveau
            _context.TherapistOfferedServices.RemoveRange(_therapist.TherapistOfferedServices);

            var service_serviceType = _context.OfferedService_OfferedServiceType.ToList();

            foreach (string service in _datas.OfferedServices)
            {
                var serviceId = Convert.ToInt32(service.Split(',')[0]);
                foreach (var serviceType in service.Substring(service.IndexOf(',') + 1).Split(','))
                {
                    var serviceTypeId = Convert.ToInt32(serviceType);
                    var therapistOfferedService = new TherapistOfferedService();

                    therapistOfferedService.Therapist = _therapist;
                    therapistOfferedService.OfferedService_OfferedServiceTypeId = service_serviceType
                        .Where(x => x.OfferedServiceId == serviceId && x.OfferedServiceTypeId == serviceTypeId)
                        .Select(x => x.Id)
                        .Single();

                    _therapist.TherapistOfferedServices.Add(therapistOfferedService);
                }
            }
        }
        private void SetPracticeSector()
        {
            //On doit supprimer le range pour ensuite les ajouter de nouveau
            _context.TherapistAreaOfPractices.RemoveRange(_therapist.TherapistAreaOfPractices);

            foreach (int practiceId in _datas.PracticeSectorIds)
            {
                var therapistAreaOfPractice = new TherapistAreaOfPractice();
                therapistAreaOfPractice.Therapist = _therapist;
                therapistAreaOfPractice.AreaOfPracticeId = practiceId;
                _therapist.TherapistAreaOfPractices.Add(therapistAreaOfPractice);
            }
        }
        private void SetTheoreticalOrientation()
        {
            //On doit supprimer le range pour ensuite les ajouter de nouveau
            _context.TherapistTheoreticalOrientations.RemoveRange(_therapist.TherapistTheoreticalOrientations);

            foreach (int theoreticalId in _datas.OrientationIds)
            {
                var therapistTheoreticalOrientation = new TherapistTheoreticalOrientation();
                therapistTheoreticalOrientation.Therapist = _therapist;
                therapistTheoreticalOrientation.TheoreticalOrientationId = theoreticalId;
                _therapist.TherapistTheoreticalOrientations.Add(therapistTheoreticalOrientation);
            }
        }
        private void SetSpecificSkills()
        {
            //On doit supprimer le range pour ensuite les ajouter de nouveau
            _context.TherapistSpecificSkills.RemoveRange(_therapist.TherapistSpecificSkills);

            foreach (int skillId in _datas.SkillIds)
            {
                var therapistSpecificSkill = new TherapistSpecificSkill();
                therapistSpecificSkill.Therapist = _therapist;
                therapistSpecificSkill.SpecificSkillId = skillId;
                _therapist.TherapistSpecificSkills.Add(therapistSpecificSkill);
            }
        }


        private void DeletePreviousAssociation()
        {
            var associationToDelete = _therapist.TherapistAssociationAndProfessionalOrders
                .Where(x => x.DeleteDate == null)
                .ToList();

            foreach (var association in associationToDelete)
            {
                association.DeleteDate = DateTime.Now;
            }
        }

    }
}