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
    
    public partial class NatureAct
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public NatureAct()
        {
            this.ClientAppointments = new HashSet<ClientAppointments>();
        }
    
        public int Id { get; set; }
        public string NatureAct1 { get; set; }
        public string NatureActEN { get; set; }
    
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<ClientAppointments> ClientAppointments { get; set; }
    }
}
