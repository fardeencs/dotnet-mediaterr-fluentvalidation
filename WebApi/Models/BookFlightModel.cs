
namespace WebApi.Models
{

    using Common;
    using MediatR;
    using System.Collections.Generic;
    using Domain.ViewModels;
    using Domain;


    public class BookFlightModel : IAsyncRequest<ResponseObject>
    {
      //  public Bookflight BookFlight { get; set; }
        public BookFlightEntity BookFlightEntity { get; set; }
        // public List<SupplierAgencyDetails> SupplierAgencyDetails { get; set; }

        public TotalFareGroup Totalfaregroup { get; set; }
        public List<AirBagDetails> AirBagDetails { get; set; }
        public List<Fareruleseg> Fareruleseg { get; set; }
        public List<CostBreakuppax> CostBreakuppax { get; set; }
    }

    public class BookFlightEntity
    {
        public BookFlight BookFlight { get; set; }

    }

    public class BookFlight
    {
        public string AgencyCode { get; set; }
        public string BookingId { get; set; }
        public string SupplierCode { get; set; }
        public List<SupplierAgencyDetails> SupplierAgencyDetails { get; set; }
        public string Faresourcecode { get; set; }
        public string SessionId { get; set; }
        public string Target { get; set; }
        public Fdetails Fdetails { get; set; }
        public Customerinfo CustomerInfo { get; set; }
        public Paymentinfo PaymentInfo { get; set; }
        public List< Travelerinfo> TravelerInfo { get; set; }
        public List<Flleggroup> FLLegGroup { get; set; }
        public string AreaCityCode { get; set; }
        public string PhoneNumber { get; set; }
        public string Email { get; set; }
        public string[] AddressLine { get; set; }
        public string CityName { get; set; }
        public string CityCode { get; set; }
        public string PostalCode { get; set; }
        public int ADT { get; set; }
        public int CHD { get; set; }
        public int INF { get; set; }
    }

    public class Fdetails
    {
        public string FlightId { get; set; }
        public string ClientIP { get; set; }
        public string ConfirmedPrice { get; set; }
        public string ConfirmedCurrency { get; set; }
    }

    public class Customerinfo
    {
        public string Sex { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string CompanyName { get; set; }
        public string Street { get; set; }
        public string HouseNo { get; set; }
        public string Zip { get; set; }
        public string City { get; set; }
        public string CountryCode { get; set; }
        public string PhoneCountry { get; set; }
        public string PhoneArea { get; set; }
        public string PhoneNumber { get; set; }
        public string Email { get; set; }
    }

    public class Paymentinfo
    {
        public string PaymentCode { get; set; }
        public string Holder { get; set; }
        public string Number { get; set; }
        public string CVC { get; set; }
        public string Expiry { get; set; }
    }
    
    public class Segment
    {
        public string FareBasis { get; set; }
        public string DepartureDate { get; set; }
        public string DepartureTime { get; set; }
        public string ArrivalDate { get; set; }
        public string ArrivalTime { get; set; }
        public string DepartureFrom { get; set; }
        public string DepartureTo { get; set; }
        public string MarketingCompany { get; set; }
        public string OperatingCompany { get; set; }
        public string FlightNumber { get; set; }
        public string BookingClass { get; set; }
        public string TerminalTo { get; set; }
        public string TerminalFrom { get; set; }
        public string Flightequip { get; set; }

    }




}


