using Business;
using ReseauPsy.Controllers.Admin;
using ReseauPsy.Models;
using ReseauPsy.Resources;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Web;
using Library.Helper;

namespace ReseauPsy.ViewModel.Therapist
{
    public class MySkillsViewModel
    {
        public List<Models.OfferedService> OfferedServices { get; set; }
        public List<DbTableIdNameProperties> AreasOfPractice { get; set; }
        public List<DbTableIdNameProperties> TheoreticalOrientations { get; set; }
        public List<DbTableIdNameProperties> SpecificSkills { get; set; }


        [Display(Name = "DetailsTherapist_Label_Association", ResourceType = typeof(Resource))]
        public List<string> TherapistAssociation { get; set; }


        [Display(Name = "DetailsTherapist_Label_OfferedSerive", ResourceType = typeof(Resource))]
        public List<Models.OfferedService> TherapistService { get; set; }


        [Display(Name = "DetailsTherapist_Label_AreaOfPractice", ResourceType = typeof(Resource))]
        public List<int> TherapistAreasOfPracticeIds { get; set; }


        [Display(Name = "DetailsTherapist_Label_TheoreticalOrientation", ResourceType = typeof(Resource))]
        public List<int> TherapistTheoreticalOrientationIds { get; set; }


        [Display(Name = "DetailsTherapist_Label_Skills", ResourceType = typeof(Resource))]
        public List<int> TherapistSpecificSkillIds { get; set; }



        public MySkillsViewModel()
        {
            this.OfferedServices = new List<Models.OfferedService>();
            this.AreasOfPractice = new List<DbTableIdNameProperties>();
            this.TheoreticalOrientations = new List<DbTableIdNameProperties>();
            this.SpecificSkills = new List<DbTableIdNameProperties>();
            this.TherapistAssociation = new List<string>();
            this.TherapistService = new List<Models.OfferedService>();
            this.TherapistAreasOfPracticeIds = new List<int>();
            this.TherapistTheoreticalOrientationIds = new List<int>();
            this.TherapistSpecificSkillIds = new List<int>();
        }


        public MySkillsViewModel(ReseauPsyEntities _context, Business.Therapist therapist)
            : this()
        {
            var offeredservices = _context.OfferedServices.ToList();
            var offeredServices_offeredServiceType = _context.OfferedService_OfferedServiceType.ToList();

           

            foreach (var service in offeredservices)
            {
                this.OfferedServices.Add(new Models.OfferedService
                {
                    Service = new DbTableIdNameProperties
                    {
                        Id = service.Id,
                        Name = CultureInfo.CurrentCulture.Name == "fr-CA" ? service.OfferedService1 : service.OfferedServiceEN
                    },


                    ServiceTypes = offeredServices_offeredServiceType
                        .Where(x => x.OfferedServiceId == service.Id)
                        .Select(x => new DbTableIdNameProperties
                        {
                            Id = x.OfferedServiceTypeId,
                            Name = CultureInfo.CurrentCulture.Name == "fr-CA" ? x.OfferedServiceType.ServiceType : x.OfferedServiceType.ServiceTypeEN
                        })
                        .ToList()
                });
            }

            this.AreasOfPractice = _context.AreasOfPractices
                .Select(x => new DbTableIdNameProperties
                {
                    Id = x.Id,
                    Name = CultureInfo.CurrentCulture.Name == "fr-CA" ? x.AreaOfPractice : x.AreaOfPracticeEN
                })
                .ToList();

            this.TheoreticalOrientations = _context.TheoreticalOrientations
                .Select(x => new DbTableIdNameProperties
                {
                    Id = x.Id,
                    Name = CultureInfo.CurrentCulture.Name == "fr-CA" ? x.TheoreticalOrientation1 : x.TheoreticalOrientationEN
                })
                .ToList();

            this.SpecificSkills = _context.SpecificSkills
                .Select(x => new DbTableIdNameProperties
                {
                    Id = x.Id,
                    Name = CultureInfo.CurrentCulture.Name == "fr-CA" ? x.SpecificSkill1 : x.SpecificSkillEN
                })
                .ToList();

            this.TherapistAreasOfPracticeIds = _context.TherapistAreaOfPractices
                .Where(x => x.TherapistId == therapist.Id)
                .Select(x => x.AreaOfPracticeId)
                .ToList();

            this.TherapistTheoreticalOrientationIds = _context.TherapistTheoreticalOrientations
                .Where(x => x.TherapistId == therapist.Id)
                .Select(x => x.TheoreticalOrientationId)
                .ToList();

            this.TherapistSpecificSkillIds = _context.TherapistSpecificSkills
                .Where(x => x.TherapistId == therapist.Id)
                .Select(x => x.SpecificSkillId)
                .ToList();

            var currentAssociation = _context.TherapistAssociationAndProfessionalOrders
                .Where(x => x.TherapistId == therapist.Id && x.DeleteDate == null)
                .ToList();


            foreach (var item in currentAssociation)
            {
                this.TherapistAssociation.Add(item.Id + "--split--" + item.AssociationName.Decode() + "--split--" + item.AssociationNumber.Decode());
            }

            var therapistOfferedService_OfferedServiceTypes = _context.TherapistOfferedServices
                .Where(x => x.TherapistId == therapist.Id)
                .ToList();

            var therapistServiceIds = therapistOfferedService_OfferedServiceTypes
                .Select(x => x.OfferedService_OfferedServiceType.OfferedService)
                .ToList();

            foreach (var service in therapistServiceIds.Distinct())
            {
                var OfferedService_OfferedServiceTypeDto = new Models.OfferedService();
                OfferedService_OfferedServiceTypeDto.Service = new DbTableIdNameProperties
                {
                    Id = service.Id,
                    Name = CultureInfo.CurrentCulture.Name == "fr-CA" ? service.OfferedService1 : service.OfferedServiceEN
                };

                OfferedService_OfferedServiceTypeDto.ServiceTypes = therapistOfferedService_OfferedServiceTypes
                    .Where(x => x.OfferedService_OfferedServiceType.OfferedServiceId == service.Id)
                    .Select(x => new DbTableIdNameProperties
                    {
                        Id = x.OfferedService_OfferedServiceType.OfferedServiceTypeId,
                        Name = CultureInfo.CurrentCulture.Name == "fr-CA" ? 
                            x.OfferedService_OfferedServiceType.OfferedServiceType.ServiceType :
                            x.OfferedService_OfferedServiceType.OfferedServiceType.ServiceTypeEN
                    })
                    .ToList();

                this.TherapistService.Add(OfferedService_OfferedServiceTypeDto);
            }
        }
    }
}