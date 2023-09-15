using ReseauPsy.ViewModel.Therapist;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace ReseauPsy.Models.Therapist
{
    public class OpqRequiredCustomValidator : ValidationAttribute
    {
        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            var viewModel = (MyProfileViewModel)validationContext.ObjectInstance;
            if (viewModel.TherapistAccreditationId == 1 || viewModel.TherapistAccreditationId == 2)
            {
                if (viewModel.TherapistOpqNumber == null)
                {
                    return new ValidationResult(ReseauPsy.Resources.Global.OPQRequired);
                }
            }

            return ValidationResult.Success;
        }
    }
}