namespace WebApi.Models
{
    using Common;
    using MediatR;
    using System.Collections.Generic;
    using Domain.ViewModels;
    using Domain;


    public class SelectFlightModel : IAsyncRequest<ResponseObject>
    {
        public Commonrequestfarepricer CommonRequestFarePricer { get; set; }
        public List<SupplierAgencyDetails> SupplierAgencyDetails { get; set; }
    }

    public class Commonrequestfarepricer
    {
        public Body Body { get; set; }
    }

    public class Body
    {
        public Airrevalidate AirRevalidate { get; set; }
    }

    public class Airrevalidate
    {
        public string ARAgencyCode { get; set; }
        public string ARSupplierCode { get; set; }
        public string FareSourceCode { get; set; }
        public string SessionId { get; set; }
        public string Target { get; set; }
        public int ADT { get; set; }
        public int CHD { get; set; }
        public int INF { get; set; }
        public Segmentgroup[] segmentGroup { get; set; }
    }

    public class Segmentgroup
    {
        public Segmentinformation segmentInformation { get; set; }
    }

    public class Segmentinformation
    {
        public Flightdate flightDate { get; set; }
        public Boardpointdetails boardPointDetails { get; set; }
        public Offpointdetails offpointDetails { get; set; }
        public Companydetails companyDetails { get; set; }
        public Flightidentification flightIdentification { get; set; }
        public Flighttypedetails flightTypeDetails { get; set; }
    }

    public class Flightdate
    {
        public string departureDate { get; set; }
        public string departureTime { get; set; }
        public string arrivalDate { get; set; }
        public string arrivalTime { get; set; }
    }

    public class Boardpointdetails
    {
        public string trueLocationId { get; set; }
    }

    public class Offpointdetails
    {
        public string trueLocationId { get; set; }
    }

    public class Companydetails
    {
        public string marketingCompany { get; set; }
        public string operatingCompany { get; set; }
    }

    public class Flightidentification
    {
        public string flightNumber { get; set; }
        public string bookingClass { get; set; }
    }

    public class Flighttypedetails
    {
        public string flightIndicator { get; set; }
        public int itemNumber { get; set; }
    }

}
