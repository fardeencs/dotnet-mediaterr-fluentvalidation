using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain
{
    public class IssueTicketEntity
    {

        public Tripdetailsresult TripDetailsResult { get; set; }
    }

    public class Tripdetailsresult
    {
        public string BookingId { get; set; }
        public string Success { get; set; }
        public string Target { get; set; }
        public string TicketStatus { get; set; }
        public string BookingStatus { get; set; }
        public string UniqueID { get; set; }
        public Tkerror[] TKErrors { get; set; }
        public Itineraryinformation[] ItineraryInformation { get; set; }
        public Reservationitem[] ReservationItem { get; set; }
        public Triptotalfare TripTotalFare { get; set; }
    }

    public class Triptotalfare
    {
        public string EquiFare { get; set; }
        public string Tax { get; set; }
        public string TotalFare { get; set; }
        public string Currency { get; set; }
    }

    public class Tkerror
    {
        public string Code { get; set; }
        public string Meassage { get; set; }
    }

    public class Itineraryinformation
    {
        public Customerinformation CustomerInformation { get; set; }
        public Etickets ETickets { get; set; }
        public Itinerarypricing ItineraryPricing { get; set; }
    }

    public class Customerinformation
    {
        public Age Age { get; set; }
        public Paxdetails Paxdetails { get; set; }
    }

    public class Age
    {
        public string Months { get; set; }
        public string Years { get; set; }
    }

    public class Paxdetails
    {
        public string PassengerFirstName { get; set; }
        public string PassengerLastName { get; set; }
        public string Title { get; set; }
        public string DateOfBirth { get; set; }
        public string EmailAddress { get; set; }
        public string Gender { get; set; }
        public string KnownTravelerNo { get; set; }
        public string NameNumber { get; set; }
        public string NationalID { get; set; }
        public string PassengerNationality { get; set; }
        public string PassengerType { get; set; }
        public DateTime PassportExpiresOn { get; set; }
        public string PassportIssueCountry { get; set; }
        public string PassportNationality { get; set; }
        public string PassportNumber { get; set; }
        public string PhoneNumber { get; set; }
        public string PostCode { get; set; }
    }

    public class Etickets
    {
        public string ItemRPH { get; set; }
        public string eTicketNumber { get; set; }
        public string SSRs { get; set; }
    }

    public class Itinerarypricing
    {
        public string EquiFare { get; set; }
        public string Tax { get; set; }
        public string TotalFare { get; set; }
        public string currency { get; set; }
    }

    public class Reservationitem
    {
        public string AirEquipmentType { get; set; }
        public string AirlinePNR { get; set; }
        public string ArrivalCode { get; set; }
        public string ArrivalDateTime { get; set; }
        public string ArrivalTer { get; set; }
        public string Baggage { get; set; }
        public string CabinClassText { get; set; }
        public string DepartureCode { get; set; }
        public string DepartureDateTime { get; set; }
        public string DepartureTer { get; set; }
        public string FlightNumber { get; set; }
        public string ItemRPH { get; set; }
        public string JDuration { get; set; }
        public string MarketingCode { get; set; }
        public string Totalpax { get; set; }
        public string OperatingCode { get; set; }
        public string StopQuantity { get; set; }
    }

}
