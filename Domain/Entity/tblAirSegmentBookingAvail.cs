namespace Domain.Entity
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("tblAirSegmentBookingAvail")]
    public partial class tblAirSegmentBookingAvail
    {
        [Key]
        public long BookAvailID { get; set; }

        public long? SegmentID { get; set; }

        [StringLength(5)]
        public string ResBookDesigCode { get; set; }

        [StringLength(5)]
        public string AvailablePTC { get; set; }

        [StringLength(5)]
        public string ResBookDesigCabinCode { get; set; }

        [StringLength(30)]
        public string FareBasis { get; set; }
    }
}
