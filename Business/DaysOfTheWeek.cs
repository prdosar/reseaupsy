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
    
    public partial class DaysOfTheWeek
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public DaysOfTheWeek()
        {
            this.DayOfTheWeek_PeriodOfTheDay = new HashSet<DayOfTheWeek_PeriodOfTheDay>();
        }
    
        public int Id { get; set; }
        public string DayOfTheWeek { get; set; }
        public string DayOfTheWeekEN { get; set; }
    
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<DayOfTheWeek_PeriodOfTheDay> DayOfTheWeek_PeriodOfTheDay { get; set; }
    }
}