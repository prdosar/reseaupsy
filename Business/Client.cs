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
    
    public partial class Client
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public Client()
        {
            this.ClientConsultationTypes = new HashSet<ClientConsultationType>();
            this.ExternalAssociationClientsSents = new HashSet<ExternalAssociationClientsSent>();
            this.TherapistClientRequests = new HashSet<TherapistClientRequest>();
            this.ClientAvailabilities = new HashSet<ClientAvailability>();
            this.ClientAppointments = new HashSet<ClientAppointment>();
            this.ClientLanguages = new HashSet<ClientLanguage>();
        }
    
        public int Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public string PhoneNumber { get; set; }
        public string City { get; set; }
        public int RegionId { get; set; }
        public string PostalCode { get; set; }
        public int ConsultationReasonId { get; set; }
        public int ClientAgeRangeId { get; set; }
        public string Message { get; set; }
        public System.DateTime RequestDate { get; set; }
        public Nullable<int> TherapistClientRequestId { get; set; }
        public Nullable<int> ExternalAssociationId { get; set; }
        public Nullable<int> ReasonDeleteClientRequestId { get; set; }
        public string DeletedAdditionalNotes { get; set; }
        public bool IsDeleted { get; set; }
        public int LanguageId { get; set; }
    
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<ClientConsultationType> ClientConsultationTypes { get; set; }
        public virtual ClientsAgeRange ClientsAgeRange { get; set; }
        public virtual ConsultationReason ConsultationReason { get; set; }
        public virtual ExternalAssociation ExternalAssociation { get; set; }
        public virtual ReasonsDeleteClientRequest ReasonsDeleteClientRequest { get; set; }
        public virtual Region Region { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<ExternalAssociationClientsSent> ExternalAssociationClientsSents { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<TherapistClientRequest> TherapistClientRequests { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<ClientAvailability> ClientAvailabilities { get; set; }
        public virtual TherapistClientRequest TherapistClientRequest { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<ClientAppointment> ClientAppointments { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<ClientLanguage> ClientLanguages { get; set; }
        public virtual Language Languages { get; set; }
    }
}
