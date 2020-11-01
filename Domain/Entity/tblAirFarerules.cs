namespace Domain.Entity
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class tblAirFarerules
    {
        [Key]
        public long FareRuleID { get; set; }

        public long? BookingRefID { get; set; }

        public string FareRule { get; set; }

        [StringLength(10)]
        public string Segment { get; set; }

        [StringLength(10)]
        public string FareRef { get; set; }

        [StringLength(10)]
        public string FilingAirline { get; set; }

        [StringLength(10)]
        public string MarketingAirline { get; set; }
    }
}
