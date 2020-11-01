namespace Domain.Entity
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("tblTitle")]
    public partial class tblTitle
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int TitleID { get; set; }

        [StringLength(10)]
        public string Title { get; set; }
    }
}
