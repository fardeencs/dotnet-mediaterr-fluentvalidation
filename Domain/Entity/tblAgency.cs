namespace Domain.Entity
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("tblAgency")]
    public partial class tblAgency
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public tblAgency()
        {
            tblSupplierMasters = new HashSet<tblSupplierMaster>();
        }

        [Key]
        public long AgencyID { get; set; }

        [Required]
        [StringLength(100)]
        public string AgencyCode { get; set; }

        [StringLength(100)]
        public string AgencyName { get; set; }

        public long? AgencyTypeID { get; set; }

        [StringLength(200)]
        public string CityCode { get; set; }

        [StringLength(150)]
        public string Address { get; set; }

        public long? PostCode { get; set; }

        [StringLength(30)]
        public string Fax { get; set; }

        [StringLength(20)]
        public string Office { get; set; }

        [StringLength(20)]
        public string Residence { get; set; }

        [StringLength(50)]
        public string EmailID { get; set; }

        [StringLength(20)]
        public string Website { get; set; }

        public int? LoginStatus { get; set; }

        public int? CreditAgentStatus { get; set; }

        public long? CreatedBy { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<tblSupplierMaster> tblSupplierMasters { get; set; }

        public virtual tblAgencyType tblAgencyType { get; set; }

        public virtual tblCity tblCity { get; set; }

        public virtual tblUser tblUser { get; set; }
    }
}
