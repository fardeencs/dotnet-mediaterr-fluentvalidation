using System.Runtime.Serialization;
using Domain.Entity;

namespace Domain
{
    [DataContract(IsReference = true)]
    public class Supplier
    {
        [DataMember]
        public long SupplierId { get; set; }
        [DataMember]
        public string SupplierName { get; set; }
        [DataMember]
        public string SupplierCode { get; set; }
        [DataMember]
        public string AccountNumber { get; set; }
        [DataMember]
        public string UserName { get; set; }
        [DataMember]
        public string Password { get; set; }
        [DataMember]
        public string Status { get; set; }
        [DataMember]
        public long? AgencyID { get; set; }
        [DataMember]
        public virtual tblAgency tblAgency { get; set; }
    }
}
