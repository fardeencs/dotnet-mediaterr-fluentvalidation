namespace Domain.Entity
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class tblAgencySeacrhDetail
    {
        [Key]
        public long SearchID { get; set; }

        public string AgencyCode { get; set; }

        public DateTime SearchDateTime { get; set; }

      

        public string Search { get; set; }

        public int? Status { get; set; }

        [StringLength(10)]
        public string SupplierCode { get; set; }
      
        public string Key { get; set; }
    }
}
