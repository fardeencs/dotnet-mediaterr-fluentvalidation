namespace Domain.Entity
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("tblAirBookingData")]
    public partial class tblAirBookingData
    {
        public long ID { get; set; }

        public long? BookingRefID { get; set; }

        [StringLength(20)]
        public string PNR { get; set; }

        [StringLength(20)]
        public string UniqueID { get; set; }
    }
}
