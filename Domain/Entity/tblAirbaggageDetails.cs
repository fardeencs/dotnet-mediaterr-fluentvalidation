namespace Domain.Entity
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class tblAirbaggageDetails
    {
        [Key]
        public long AirBaggageDetailsID { get; set; }

        public long? BookingRefID { get; set; }

        [Required]
        [StringLength(5)]
        public string PAXType { get; set; }

        public int? CabinBaggageQuantity { get; set; }

        [StringLength(5)]
        public string CabinBaggageUnit { get; set; }

        public int? CheckinBaggageQuantity { get; set; }

        [StringLength(10)]
        public string CheckinBaggageUnit { get; set; }

        [StringLength(5)]
        public string FromSeg { get; set; }

        [StringLength(5)]
        public string ToSeg { get; set; }
    }
}
