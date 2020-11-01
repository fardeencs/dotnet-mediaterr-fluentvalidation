namespace Domain.Entity
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("tblSupplierMaster")]
    public partial class tblSupplierMaster
    {
        [Key]
        public long SupplierId { get; set; }

        [Required]
        [StringLength(100)]
        public string SupplierName { get; set; }

        [StringLength(100)]
        public string BaseUrl { get; set; }

        [Required]
        [StringLength(20)]
        public string SupplierCode { get; set; }

        [StringLength(50)]
        public string AccountNumber { get; set; }

        [Required]
        [StringLength(50)]
        public string UserName { get; set; }

        [Required]
        [StringLength(100)]
        public string Password { get; set; }

        [StringLength(1)]
        public string Status { get; set; }

        public long? AgencyID { get; set; }

        public virtual tblAgency tblAgency { get; set; }
    }
}
