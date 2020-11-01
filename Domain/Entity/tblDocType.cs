namespace Domain.Entity
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("tblDocType")]
    public partial class tblDocType
    {
        [Key]
        public int DocTypeID { get; set; }

        [StringLength(25)]
        public string DocType { get; set; }
    }
}
