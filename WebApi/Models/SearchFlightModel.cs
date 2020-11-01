namespace WebApi.Models
{
    using Common;
    using MediatR;
    using System.Collections.Generic;
    using Domain;

    public class Rootobject : IAsyncRequest<ResponseObject>
    {
        public Commonrequestsearch CommonRequestSearch { get; set; }
        public List<Origindestinationinformation> OriginDestinationInformation { get; set; }
        public List<SupplierAgencyDetails> SupplierAgencyDetails { get; set; }
        public string Currency { get; set; }
        public string PreferredAirline { get; set; }
        public string NonStop { get; set; }
        public string cabin { get; set; }
        public string IsRefundable { get; set; }
        public string Maxstopquantity { get; set; }
        public string Triptype { get; set; }
        public string PreferenceLevel { get; set; }
        public string Target { get; set; }
        public Passengertypequantity PassengerTypeQuantity { get; set; }
    }

    public class Commonrequestsearch
    {
        public int numberOfUnits { get; set; }
        public string typeOfUnit { get; set; }
        public string AgencyCode { get; set; }
    }

    public class Passengertypequantity
    {
        public int ADT { get; set; }
        public int CHD { get; set; }
        public int INF { get; set; }
    }

    public class Origindestinationinformation
    {
        public string DepartureDate { get; set; }
        public string OriginLocation { get; set; }
        public string DestinationLocation { get; set; }
        public Radiusinformation RadiusInformation { get; set; }
    }

    public class Radiusinformation
    {
        public string _FromValue { get; set; }
        public string _ToValue { get; set; }
    }
    
}