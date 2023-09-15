using Business;
using HelpersLibrary;
using Library.Helper;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ReseauPsy.Models.Admin
{
    public class CreateTherapist
    {
        public bool Succeeded { get; set; }

        private DetailTherapistDatas _datas;
        private ReseauPsyEntities _context;


        private string _password;
        private Business.Therapist _therapist;
        private ApplicationUser _user;


        public CreateTherapist(DetailTherapistDatas datas, ReseauPsyEntities context)
        {
            Succeeded = true;
            _datas = datas;
            _context = context;

            CreateIdentity();
            CreateTherapistInDb();
            SetTherapistInformations();
            SetTherapistSkills();
            SetTherapistPayInformation();
            SendCreatedAccountEmail();

            try
            {
                _context.SaveChanges();
            }
            catch (Exception ex)
            {
                Succeeded = false;
            }
        }

        private void CreateIdentity()
        {
            //On crée l'user identity
            _password = PasswordGenerator.GenerateRandomPassword();
            ApplicationUserManager userManager = HttpContext.Current.GetOwinContext().GetUserManager<ApplicationUserManager>();
            _user = new ApplicationUser { UserName = _datas.Email, Email = _datas.Email };
            IdentityResult resultCreateUser = userManager.Create(_user, _password);

            //Si l'ajout dans identity n'a pas fonctionné
            if (!resultCreateUser.Succeeded)
                Succeeded = false;

            //On ajoute le role
            userManager.AddToRole(_user.Id, RoleName.Therapist);
        }
        private void CreateTherapistInDb()
        {
            _therapist = new Business.Therapist();
            _therapist.HiringDate = _datas.HiringDate;
            _therapist.MaxWeeklyRequest = _datas.MaxWeeklyRequest;
            _therapist.AdminNotes = _datas.AdminNotes;
            _therapist.CreationDate = DateTime.Now;
            _therapist.IsDeleted = false;
            _therapist.AspNetUserId = _user.Id;

            _context.Therapists.Add(_therapist);
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
            therapistInfo.TpsNumber = _datas.TpsNumber;
            therapistInfo.TvqNumber = _datas.TvqNumber;
            therapistInfo.TakeClientFromAssuranceCompany = _datas.TakeAssuaranceClient;
            therapistInfo.TakeClientFromThirdPartyPayer = _datas.TakeThirdPartyClient;
            therapistInfo.IsInterestedForFormation = _datas.IsInterestedFormation;
            therapistInfo.ModificationDate = DateTime.Now;

            //on met le OPQ number si c'est un psychologue ou un psychothérapeute
            if (_datas.AccreditationId == 1 || _datas.AccreditationId == 2)
                therapistInfo.OpqNumber = _datas.OpqNumber;

            _therapist.TherapistInfo.Add(therapistInfo);
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
        private void SetTherapistPayInformation()
        {
            foreach (var newPayInfo in _datas.Wages)
            {
                var newTherapistPayInformation = new TherapistPayInformations();

                string[] payInfo = newPayInfo.Split(new string[] { "--split--" }, StringSplitOptions.None);
                string wageName = payInfo[1];
                decimal therapisWage = decimal.Parse(payInfo[2]);
                decimal clientCost = decimal.Parse(payInfo[3]);
                bool isDefault = bool.Parse(payInfo[4]);

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
        private void SendCreatedAccountEmail()
        {
            string emailContent = String.Format(ReseauPsy.Resources.Email.CreateTherapist_Content,
                HttpContext.Current.Request.Url.Scheme + "://" + HttpContext.Current.Request.Url.Authority,
                _datas.Email,
                _password);

            EmailHelper.Send(ReseauPsy.Resources.Email.CreateTherapist_Subject, emailContent, _datas.Email, "");
        }




        private void SetProfessionalTitles()
        {
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
            var dayOfTheWeek_PeriodOfTheDay = _context.DayOfTheWeek_PeriodOfTheDay.ToList();

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
            foreach (int skillId in _datas.SkillIds)
            {
                var therapistSpecificSkill = new TherapistSpecificSkill();
                therapistSpecificSkill.Therapist = _therapist;
                therapistSpecificSkill.SpecificSkillId = skillId;
                _therapist.TherapistSpecificSkills.Add(therapistSpecificSkill);
            }
        }

    }
}