namespace Domain.Entity
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("tblBooking")]
    public partial class tblBooking
    {
        [Key]
        public long BookingRefID { get; set; }

        public long BookingID { get; set; }

        public int? Position { get; set; }

        public long? UserID { get; set; }

        public int? ServiceTypeId { get; set; }

        public DateTime? BookingDate { get; set; }

        public long? SupplierID { get; set; }

        [StringLength(150)]
        public string SupplierBookingReference { get; set; }

        public DateTime? CancellationDeadline { get; set; }

        [StringLength(5)]
        public string BookingStatusCode { get; set; }

        [StringLength(100)]
        public string AgencyRemarks { get; set; }

        public int? PaymentStatusID { get; set; }
    }
}
