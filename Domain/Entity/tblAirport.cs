namespace Domain.Entity
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("tblAirport")]
    public partial class tblAirport
    {
        public long ID { get; set; }

        [Required]
        [StringLength(5)]
        public string IATA { get; set; }

        [StringLength(3)]
        public string CityCode { get; set; }

        [StringLength(200)]
        public string CityName { get; set; }
    }
}
