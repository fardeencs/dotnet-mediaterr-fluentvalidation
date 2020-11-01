using System.Collections.Generic;
using System.Runtime.Serialization;

namespace Domain
{
    [DataContract(IsReference = true)]
    public class Agency
    {
        public Agency()
        {
            Suppliers = new HashSet<Supplier>();
        }
        [DataMember]
        public long AgencyID { get; set; }
        [DataMember]
        public string AgencyCode { get; set; }
        [DataMember]
        public string AgencyName { get; set; }
        [DataMember]
        public long? AgencyTypeID { get; set; }
        [DataMember]
        public string CityCode { get; set; }
        [DataMember]
        public string Address { get; set; }
        [DataMember]
        public long? PostCode { get; set; }
        [DataMember]
        public string Fax { get; set; }
        [DataMember]
        public string Office { get; set; }
        [DataMember]
        public string Residence { get; set; }
        [DataMember]
        public string EmailID { get; set; }
        [DataMember]
        public string Website { get; set; }
        [DataMember]

        public int? LoginStatus { get; set; }
        [DataMember]

        public int? CreditAgentStatus { get; set; }
        [DataMember]

        public long? CreatedBy { get; set; }
        [DataMember]

        public virtual ICollection<Supplier> Suppliers { get; set; }

        //public virtual tblAgencyType tblAgencyType { get; set; }

        //public virtual tblCity tblCity { get; set; }

        public virtual User User { get; set; }
    }
}
