namespace Domain.Entity
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("tblAirBookingCost")]
    public partial class tblAirBookingCost
    {
        [Key]
        public long BookingCostID { get; set; }

        public long? BookingRefID { get; set; }

        [Column(TypeName = "money")]
        public decimal? TotalBaseNet { get; set; }

        [Column(TypeName = "money")]
        public decimal? TotalTaxNet { get; set; }

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

        [Column(TypeName = "money")]
        public decimal? CancellationAmount { get; set; }

        [StringLength(5)]
        public string CancellationCurrency { get; set; }
    }
}
