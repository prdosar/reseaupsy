//------------------------------------------------------------------------------
// <auto-generated>
//     Ce code a été généré à partir d'un modèle.
//
//     Des modifications manuelles apportées à ce fichier peuvent conduire à un comportement inattendu de votre application.
//     Les modifications manuelles apportées à ce fichier sont remplacées si le code est régénéré.
// </auto-generated>
//------------------------------------------------------------------------------

namespace Business
{
    using System;
    using System.Collections.Generic;
    
    public partial class TherapistPayInformation
    {
        public int Id { get; set; }
        public int TherapistId { get; set; }
        public System.DateTime ChangedDate { get; set; }
        public decimal TherapistHourlyWage { get; set; }
        public decimal ClientHourlyCost { get; set; }
        public bool IsTaxable { get; set; }
    
        public virtual Therapist Therapist { get; set; }
    }
}
