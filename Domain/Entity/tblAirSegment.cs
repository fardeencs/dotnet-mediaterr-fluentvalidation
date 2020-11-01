namespace Domain.Entity
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("tblAirSegment")]
    public partial class tblAirSegment
    {
        [Key]
        public long SegmentID { get; set; }

        public long? OriginDestinationID { get; set; }

        public DateTime? DepartureDateTime { get; set; }

        public DateTime? ArrivalDateTime { get; set; }

        [StringLength(10)]
        public string FlightNumber { get; set; }

        public int? Status { get; set; }

        [StringLength(5)]
        public string DepartureAirportLocationCode { get; set; }

        [StringLength(5)]
        public string DepartureAirportTerminal { get; set; }

        [StringLength(5)]
        public string ArrivalAirportLocationCode { get; set; }

        [StringLength(5)]
        public string ArrivalAirportTerminal { get; set; }

        [StringLength(5)]
        public string OperatingAirlineCode { get; set; }

        [StringLength(10)]
        public string EquipmentAirEquipType { get; set; }

        [StringLength(5)]
        public string MarketingAirlineCode { get; set; }
    }
}
