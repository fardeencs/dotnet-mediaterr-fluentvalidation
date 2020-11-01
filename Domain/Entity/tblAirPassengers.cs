namespace Domain.Entity
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class tblAirPassengers
    {
        [Key]
        public long PaxID { get; set; }

        public long? BookingRefID { get; set; }

        public int? TitleID { get; set; }

        public int? PaxTypeID { get; set; }

        [StringLength(20)]
        public string GivenName { get; set; }

        [StringLength(20)]
        public string Surname { get; set; }

        [StringLength(150)]
        public string EmailID { get; set; }

        [StringLength(20)]
        public string TelPhoneType { get; set; }

        [StringLength(10)]
        public string LocationCode { get; set; }

        public DateTime? DateofBirth { get; set; }

        [StringLength(20)]
        public string PNR { get; set; }

        [StringLength(50)]
        public string ETicketNo { get; set; }

        public long? UserID { get; set; }
    }
}
