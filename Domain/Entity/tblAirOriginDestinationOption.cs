namespace Domain.Entity
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class tblAirOriginDestinationOption
    {
        [Key]
        public long OriginDestinationID { get; set; }

        public long? BookingRefID { get; set; }

        public int? RefNumber { get; set; }

        public int? DirectionID { get; set; }

        [StringLength(4)]
        public string ElapsedTime { get; set; }
    }
}
