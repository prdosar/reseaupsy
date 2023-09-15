using ReseauPsy.ViewModel.Therapist;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace ReseauPsy.Models.Therapist
{
    public class TpsRequiredCustomValidator : ValidationAttribute
    {
        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            var viewModel = (MyProfileViewModel)validationContext.ObjectInstance;
            if (viewModel.IsApplyTaxes)
            {
                if (viewModel.TherapistTpsNumber == null)
                    return new ValidationResult(ReseauPsy.Resources.Global.TPSRequired);
            }

            return ValidationResult.Success;
        }
    }

    public class TvqRequiredCustomValidator : ValidationAttribute
    {
        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            var viewModel = (MyProfileViewModel)validationContext.ObjectInstance;
            if (viewModel.IsApplyTaxes)
            {
                if (viewModel.TherapistTvqNumber == null)
                    return new ValidationResult(ReseauPsy.Resources.Global.TVQRequired);
            }

            return ValidationResult.Success;
        }
    }
}