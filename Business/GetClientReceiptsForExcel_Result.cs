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
    
    public partial class GetClientReceiptsForExcel_Result
    {
        public int Id { get; set; }
        public System.DateTime AppointmentDate { get; set; }
        public Nullable<int> ClientPaymentReceiptNumber { get; set; }
        public string NatureAct { get; set; }
        public string Secteur { get; set; }
        public string ConsultationType { get; set; }
        public string ClientName { get; set; }
        public string TherapistName { get; set; }
    }
}