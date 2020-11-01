using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain
{


    public class Rootobject
    {
        public Faremasterpricertravelboardsearchreply fareMasterPricerTravelBoardSearchReply { get; set; }
    }

    public class Faremasterpricertravelboardsearchreply
    {
        public Replystatus replyStatus { get; set; }
        public Conversionrate conversionRate { get; set; }
        public List<Flightindex> flightIndex { get; set; }
    }

    public class Replystatus
    {
        public Status status { get; set; }
    }

    public class Status
    {
        public string advisoryTypeInfo { get; set; }
    }

    public class Conversionrate
    {
        public Conversionratedetail conversionRateDetail { get; set; }
        public string SessionId { get; set; }
    }

    public class Conversionratedetail
    {
        public string currency { get; set; }
    }

    public class Flightindex
    {
        public Segmentref SegmentRef { get; set; }
        public Typeref TypeRef { get; set; }
        public Reccount reccount { get; set; }
        public Fare fare { get; set; }
        public Faredetails fareDetails { get; set; }
        public Bagdetails Bagdetails { get; set; }
        public Description Description { get; set; }
        public Groupofflight[] groupOfFlights { get; set; }
    }

    public class Segmentref
    {
        public string segRef { get; set; }
        public string PSessionId { get; set; }
        public string supplier { get; set; }
        public string key { get; set; }
    }

    public class Typeref
    {
        public string type { get; set; }
    }

    public class Reccount
    {
        public string rec_id { get; set; }
        public string combi_id { get; set; }
    }

    public class Fare
    {
        public string amount { get; set; }
        public string taxfare { get; set; }
        public string basefare { get; set; }
        public string MarkupFare { get; set; }
        public string currency { get; set; }
    }

    public class Faredetails
    {
        public string fareBasis { get; set; }
    }

    public class Bagdetails
    {
        public string freeAllowance { get; set; }
        public string Qcode { get; set; }
        public string unit { get; set; }
    }

    public class Description
    {
        public string Pricingmsg { get; set; }
    }

    public class Groupofflight
    {
        public Flightproposal FlightProposal { get; set; }
        public Flightdetail[] flightDetails { get; set; }
    }

    public class Flightproposal
    {
        public string elapse { get; set; }
        public string unitQualifier { get; set; }
        public string VAirline { get; set; }
        public string Ocarrier { get; set; }
    }

    public class Flightdetail
    {
        public Finfo Finfo { get; set; }
        public Flightcharacteristics flightCharacteristics { get; set; }
    }

    public class Finfo
    {
        public Datetime DateTime { get; set; }
        public Location location { get; set; }
        public Companyid companyId { get; set; }
        public string flightNo { get; set; }
        public string eqpType { get; set; }
        public string eTicketing { get; set; }
        public string attributeType { get; set; }
        public string Elapsedtime { get; set; }
        public string Bookingclass { get; set; }

        


    }

    public class Datetime
    {
        public string Depdate { get; set; }
        public string Deptime { get; set; }
        public string Arrdate { get; set; }
        public string Arrtime { get; set; }
        public string Variation { get; set; }
    }

    public class Location
    {
        public string locationFrom { get; set; }
        public string Fromterminal { get; set; }
        public string LocationTo { get; set; }
        public string Toterminal { get; set; }
    }

    public class Companyid
    {
        public string mCarrier { get; set; }
        public string oCarrier { get; set; }
    }

    public class Flightcharacteristics
    {
        public string inFlightSrv { get; set; }
    }
}
