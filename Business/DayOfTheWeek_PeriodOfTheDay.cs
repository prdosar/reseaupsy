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
    
    public partial class DayOfTheWeek_PeriodOfTheDay
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public DayOfTheWeek_PeriodOfTheDay()
        {
            this.TherapistAvailabilities = new HashSet<TherapistAvailability>();
            this.ClientAvailabilities = new HashSet<ClientAvailability>();
        }
    
        public int Id { get; set; }
        public int DaysOfTheWeekId { get; set; }
        public int PeriodsOfTheDayId { get; set; }
    
        public virtual DaysOfTheWeek DaysOfTheWeek { get; set; }
        public virtual PeriodsOfTheDay PeriodsOfTheDay { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<TherapistAvailability> TherapistAvailabilities { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<ClientAvailability> ClientAvailabilities { get; set; }
    }
}
