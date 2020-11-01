namespace Domain.Entity
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("tblCity")]
    public partial class tblCity
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public tblCity()
        {
            tblAgencies = new HashSet<tblAgency>();
        }

        [Key]
        [StringLength(200)]
        public string CityCode { get; set; }

        [StringLength(2)]
        public string CountryCode { get; set; }

        [StringLength(50)]
        public string CityName { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<tblAgency> tblAgencies { get; set; }

        public virtual tblCountry tblCountry { get; set; }
    }
}
