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
    
    public partial class PeriodsOfTheDay
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public PeriodsOfTheDay()
        {
            this.DayOfTheWeek_PeriodOfTheDay = new HashSet<DayOfTheWeek_PeriodOfTheDay>();
        }
    
        public int Id { get; set; }
        public string PeriodOfTheDay { get; set; }
        public string PeriodOfTheDayEN { get; set; }
    
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<DayOfTheWeek_PeriodOfTheDay> DayOfTheWeek_PeriodOfTheDay { get; set; }
    }
}
