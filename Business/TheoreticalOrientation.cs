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
    
    public partial class TheoreticalOrientation
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public TheoreticalOrientation()
        {
            this.TherapistTheoreticalOrientations = new HashSet<TherapistTheoreticalOrientation>();
        }
    
        public int Id { get; set; }
        public string TheoreticalOrientation1 { get; set; }
        public string TheoreticalOrientationEN { get; set; }
    
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<TherapistTheoreticalOrientation> TherapistTheoreticalOrientations { get; set; }
    }
}
