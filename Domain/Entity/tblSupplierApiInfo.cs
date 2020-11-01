namespace Domain.Entity
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("tblSupplierApiInfo")]
    public partial class tblSupplierApiInfo
    {
        [Key]
        public long SupplierApiInfoId { get; set; }

        public long SupplierId { get; set; }

        [Required]
        [StringLength(100)]
        public string RequestUrl { get; set; }

        [StringLength(50)]
        public string SupplierRoute { get; set; }
    }
}
