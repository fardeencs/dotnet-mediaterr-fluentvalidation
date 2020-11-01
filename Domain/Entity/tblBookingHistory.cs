namespace Domain.Entity
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("tblBookingHistory")]
    public partial class tblBookingHistory
    {
        [Key]
        public long BookingHistoryID { get; set; }

        public long? BookingRefID { get; set; }

        [StringLength(5)]
        public string BookingStatusCode { get; set; }

        public long? UserID { get; set; }

        public DateTime? ActionDate { get; set; }
    }
}
