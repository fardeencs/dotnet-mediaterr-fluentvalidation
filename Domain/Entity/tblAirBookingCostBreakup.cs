namespace Domain.Entity
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("tblAirBookingCostBreakup")]
    public partial class tblAirBookingCostBreakup
    {
        [Key]
        public long BookingCostBreakupID { get; set; }

        public long? BookingCostID { get; set; }

        [Column(TypeName = "money")]
        public decimal? BaseNet { get; set; }

        [Column(TypeName = "money")]
        public decimal? TaxNet { get; set; }

        [Column(TypeName = "money")]
        public decimal? TotalNet { get; set; }

        [StringLength(5)]
        public string NetCurrency { get; set; }

        public int? MarkupTypeID { get; set; }

        public double? MarkupValue { get; set; }

        [StringLength(5)]
        public string MarkupCurrency { get; set; }

        [Column(TypeName = "money")]
        public decimal? SellAmount { get; set; }

        [StringLength(5)]
        public string SellCurrency { get; set; }

        [Column(TypeName = "money")]
        public decimal? AdditionalServiceFee { get; set; }

        [StringLength(5)]
        public string PaxType { get; set; }

        public int? NoOfPAx { get; set; }
    }
}
