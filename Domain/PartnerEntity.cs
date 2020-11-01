namespace Domain
{
    using System;
    using System.Collections.Generic;

    public class PartnerEntity 
    {
        public PartnerEntity()
        {
            PartnerFlightsEntity = new HashSet<PartnerFlightEntity>();
        }

        public int PartnerId { get; set; }
        public string Name { get; set; }
        public string Address { get; set; }
        public string Contact { get; set; }
        public string Email { get; set; }
        public string API { get; set; }
        public virtual ICollection<PartnerFlightEntity> PartnerFlightsEntity { get; set; }
    }

    public class PartnerFlightEntity
    {
        public string FlightNo { get; set; }
        public string FlightName { get; set; }
        public DateTime FlightDate { get; set; }
        public string FlightFrom { get; set; }
        public string FlightTo { get; set; }
        public int SeatsAvailable { get; set; }  
    }
}
