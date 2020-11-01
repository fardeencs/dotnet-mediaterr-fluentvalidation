using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain
{

    public class SelectFlightResponse
    {
        public Fareinformationwithoutpnrreply FareInformationWithoutPNRReply { get; set; }
    }

    public class Fareinformationwithoutpnrreply
    {
        public string FareSessionid { get; set; }
        public string Farefaresourcecode { get; set; }
        public string Fareagencycode { get; set; }
        public string Fsupplier { get; set; }
        public Flightfaredetails FlightFareDetails { get; set; }
        public Paxfaredetail[] PaxFareDetails { get; set; }
        public Airsegment[] AirSegments { get; set; }
        public Fareerror[] FareErrors { get; set; }
        public Airfarerule[] AirFareRule { get; set; }
        public string Success { get; set; }
        public string Target { get; set; }
    }

    public class Flightfaredetails
    {
        public string TotalPax { get; set; }
        public string TotalFare { get; set; }
        public string Basefare { get; set; }
        public string TaxFare { get; set; }
        public string currency { get; set; }
        public Surchargesgroup surchargesGroup { get; set; }
    }

    public class Surchargesgroup
    {
        public string Amount { get; set; }
        public string countryCode { get; set; }
    }

    public class Paxfaredetail
    {
        public string TotalPax { get; set; }
        public string TotalPaxUnites { get; set; }
        public string PaxType { get; set; }
        public string TotalFare { get; set; }
        public string Basefare { get; set; }
        public string TaxFare { get; set; }
        public string currency { get; set; }
        public Surcharges surcharges { get; set; }
    }

    public class Surcharges
    {
        public string Amount { get; set; }
        public string countryCode { get; set; }
    }

    public class Airsegment
    {
        public Seginfo SegInfo { get; set; }
        public string FareSourceCode { get; set; }
        public Cabingroup CabinGroup { get; set; }
        public Baggageinformation[] BaggageInformations { get; set; }
    }

    public class Seginfo
    {
        public Fldata FLData { get; set; }
        public Dateandtimedetails DateandTimeDetails { get; set; }
    }

    public class Fldata
    {
        public string BoardPoint { get; set; }
        public string BoardTerminal { get; set; }
        public string Offpoint { get; set; }
        public string OffTerminal { get; set; }
        public string MarketingCompany { get; set; }
        public string OperatingCompany { get; set; }
        public string FlightNumber { get; set; }
        public string BookingClass { get; set; }
        public string Equiptype { get; set; }
        public string durationtime { get; set; }
    }

    public class Dateandtimedetails
    {
        public string DepartureDate { get; set; }
        public string DepartureTime { get; set; }
        public string ArrivalDate { get; set; }
        public string ArrivalTime { get; set; }
    }

    public class Cabingroup
    {
        public string designator { get; set; }
    }

    public class Baggageinformation
    {
        public string BagPaxType { get; set; }
        public string Baggage { get; set; }
        public string Unit { get; set; }
    }

    public class Fareerror
    {
        public string code { get; set; }
        public string errormessage { get; set; }
    }

    public class Airfarerule
    {
        public string Airline { get; set; }
        public string Departure { get; set; }
        public string Arrival { get; set; }
        public Fareruledetail[] FareRuleDetails { get; set; }
    }

    public class Fareruledetail
    {
        public string Rulehead { get; set; }
        public string RuleBody { get; set; }
    }

}
