namespace Domain.Entity
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("tblUser")]
    public partial class tblUser
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public tblUser()
        {
            tblAgencies = new HashSet<tblAgency>();
        }

        [Key]
        public long UserID { get; set; }

        public long? AgencyID { get; set; }

        public int? TitleID { get; set; }

        [StringLength(50)]
        public string FirstName { get; set; }

        [StringLength(50)]
        public string LastName { get; set; }

        [StringLength(20)]
        public string Contact { get; set; }

        [StringLength(20)]
        public string Mobile { get; set; }

        [StringLength(100)]
        public string EmailID { get; set; }

        [StringLength(50)]
        public string Designation { get; set; }

        [StringLength(100)]
        public string Username { get; set; }

        [StringLength(50)]
        public string Password { get; set; }

        public int? LoginStatus { get; set; }

        public long? CreatedBy { get; set; }

        public int? AdminStatus { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<tblAgency> tblAgencies { get; set; }
    }
}
