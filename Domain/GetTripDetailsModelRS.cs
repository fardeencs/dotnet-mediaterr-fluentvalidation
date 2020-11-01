using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain
{

    public class GetTripDetailsModelRS
    {
        public Dbconnectionresponse DBConnectionResponse { get; set; }
    }

    public class Dbconnectionresponse
    {
        public string AgencyCode { get; set; }
        public string BookingId { get; set; }
        public string SupplierCode { get; set; }
        public Dbtotalfaregroup DBTotalfaregroup { get; set; }
        public Dbairbagdetail[] DBairBagDetails { get; set; }
        public Dbtravelerinfo[] DBTravelerInfo { get; set; }
        public int ADT { get; set; }
        public int CHD { get; set; }
        public int INF { get; set; }
        public Dbcostbreakuppax[] DBcostBreakuppax { get; set; }
        public Dbfareruleseg[] DBfareruleseg { get; set; }
        public Dbflleggroup[] DBFLLegGroup { get; set; }
        public Dbfdetails DBFdetails { get; set; }
        public Dbcustomerinfo DBCustomerInfo { get; set; }
    }

    public class Dbtotalfaregroup
    {
        public string totalBaseNet { get; set; }
        public string totalTaxNet { get; set; }
        public string markupTypeID { get; set; }
        public string netCurrency { get; set; }
        public string markupValue { get; set; }
        public string markupCurrency { get; set; }
        public string sellAmount { get; set; }
        public string sellCurrency { get; set; }
        public string additionalServiceFee { get; set; }
        public string cancellationAmount { get; set; }
        public string cancellationCurrency { get; set; }
        public string paidDate { get; set; }
        public string paidAmount { get; set; }
        public string paymentTypeID { get; set; }
    }

    public class Dbfdetails
    {
        public string FlightId { get; set; }
        public string ClientIP { get; set; }
        public string ConfirmedPrice { get; set; }
        public string ConfirmedCurrency { get; set; }
    }

    public class Dbcustomerinfo
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

    public class Dbairbagdetail
    {
        public string paxType { get; set; }
        public string cabinBaggageQuantity { get; set; }
        public string cabinBaggageUnit { get; set; }
        public string checkinBaggageQuantity { get; set; }
        public string checkinBaggageUnit { get; set; }
    }

    public class Dbtravelerinfo
    {
        public string PassengerType { get; set; }
        public string GivenName { get; set; }
        public string NamePrefix { get; set; }
        public string Surname { get; set; }
        public string BirthDate { get; set; }
        public string DocType { get; set; }
        public string DocumentNumber { get; set; }
        public string DocIssueCountry { get; set; }
        public string ExpireDate { get; set; }
        public string AirPnr { get; set; }
        public string AirTicketNo { get; set; }
    }

    public class Dbcostbreakuppax
    {
        public string paxType { get; set; }
        public string paxquantity { get; set; }
        public string paxtotalBaseNet { get; set; }
        public string paxtotalTaxNet { get; set; }
        public string paxmarkupTypeID { get; set; }
        public string paxnetCurrency { get; set; }
        public string paxmarkupValue { get; set; }
        public string paxmarkupCurrency { get; set; }
        public string paxsellAmount { get; set; }
        public string paxsellCurrency { get; set; }
        public string paxadditionalServiceFee { get; set; }
        public string paxpaidAmount { get; set; }
    }

    public class Dbfareruleseg
    {
        public string fareRule { get; set; }
        public string Segment { get; set; }
        public string FareRef { get; set; }
        public string FilingAirline { get; set; }
        public string MarketingAirline { get; set; }
    }

    public class Dbflleggroup
    {
        public string elapsedtime { get; set; }
        public string from { get; set; }
        public string to { get; set; }
        public Dbsegment[] DBsegments { get; set; }
    }

    public class Dbsegment
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
        public string terminalTo { get; set; }
        public string terminalFrom { get; set; }
        public string flightequip { get; set; }
        public string cabin { get; set; }
    }

}
