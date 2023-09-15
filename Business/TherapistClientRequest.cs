//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace Business
{
    using System;
    using System.Collections.Generic;
    
    public partial class TherapistClientRequest
    {
        public int Id { get; set; }
        public int ClientId { get; set; }
        public int TherapistId { get; set; }
        public System.DateTime RequestSentDate { get; set; }
        public string AdditionMessageFromTherapist { get; set; }
        public string AdditionalMessageFromAdmin { get; set; }
        public Nullable<bool> IsAccepted { get; set; }
    
        public virtual Clients Clients { get; set; }
        public virtual Therapist Therapists { get; set; }
    }
}