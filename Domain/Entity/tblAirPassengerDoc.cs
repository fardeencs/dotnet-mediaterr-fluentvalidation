namespace Domain.Entity
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class tblAirPassengerDoc
    {
        public long ID { get; set; }

        public long? PaxID { get; set; }

        [StringLength(30)]
        public string DocumentNumber { get; set; }

        [Column(TypeName = "date")]
        public DateTime? ExpiryDate { get; set; }

        [StringLength(50)]
        public string IssueLocation { get; set; }

        [StringLength(2)]
        public string IssueCountry { get; set; }

        public int? DocTypeID { get; set; }
    }
}
