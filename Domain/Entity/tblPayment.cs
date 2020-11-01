namespace Domain.Entity
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("tblPayment")]
    public partial class tblPayment
    {
        [Key]
        public long PaymentID { get; set; }

        public long? BookingRefID { get; set; }

        [Column(TypeName = "money")]
        public decimal? PaidAmount { get; set; }

        [StringLength(5)]
        public string CurrencyCode { get; set; }

        public DateTime? PaidDate { get; set; }

        public int? PaymentTypeID { get; set; }
    }
}
