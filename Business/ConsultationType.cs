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
    
    public partial class ConsultationType
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public ConsultationType()
        {
            this.ClientConsultationTypes = new HashSet<ClientConsultationType>();
            this.TherapistConsultationTypes = new HashSet<TherapistConsultationType>();
            this.ClientAppointments = new HashSet<ClientAppointments>();
        }
    
        public int Id { get; set; }
        public string ConsultationType1 { get; set; }
        public string ConsultationTypeEN { get; set; }
    
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<ClientConsultationType> ClientConsultationTypes { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<TherapistConsultationType> TherapistConsultationTypes { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<ClientAppointments> ClientAppointments { get; set; }
    }
}
