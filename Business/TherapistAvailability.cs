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
    
    public partial class TherapistAvailability
    {
        public int Id { get; set; }
        public int TherapistId { get; set; }
        public int DayOfTheWeek_PeriodOfTheDay_Id { get; set; }
    
        public virtual DayOfTheWeek_PeriodOfTheDay DayOfTheWeek_PeriodOfTheDay { get; set; }
        public virtual Therapist Therapist { get; set; }
    }
}
