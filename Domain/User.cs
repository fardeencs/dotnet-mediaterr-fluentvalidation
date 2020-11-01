using System.Collections.Generic;
using System.Runtime.Serialization;

namespace Domain
{
    [DataContract(IsReference = true)]
    public class User
    {
        public User()
        {
            Agencies = new HashSet<Agency>();
        }
        [DataMember]
        public long UserID { get; set; }
        [DataMember]
        public long? AgencyID { get; set; }
        [DataMember]
        public int? TitleID { get; set; }

        [DataMember]
        public string FirstName { get; set; }

        [DataMember]
        public string LastName { get; set; }

        [DataMember]
        public string Contact { get; set; }

        [DataMember]
        public string Mobile { get; set; }

        [DataMember]
        public string EmailID { get; set; }

        [DataMember]
        public string Designation { get; set; }

        [DataMember]
        public string Username { get; set; }

        [DataMember]
        public string Password { get; set; }
        [DataMember]
        public int? LoginStatus { get; set; }
        [DataMember]
        public long? CreatedBy { get; set; }
        [DataMember]
        public int? AdminStatus { get; set; }
        [DataMember]
        public virtual ICollection<Agency> Agencies { get; set; }
    }
}