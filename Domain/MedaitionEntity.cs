using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain
{




    public class MediationRequestObject
    {
        public MedaitionEntity MedaitionEntity { get; set; }
    }
    public class MedaitionEntity
    {

        public TotalFareGroup Totalfaregroup { get; set; }
        public List<AirBagDetails> AirBagDetails { get; set; }
        public List<Fareruleseg> Fareruleseg { get; set; }
        public List<CostBreakuppax> CostBreakuppax { get; set; }
    }
    public class TotalFareGroup
    {
        public string TotalBaseNet { get; set; }
        public string TotalTaxNet { get; set; }
        public string MarkupTypeID { get; set; }
        public string NetCurrency { get; set; }
        public string MarkupValue { get; set; }
        public string MarkupCurrency { get; set; }
        public string SellAmount { get; set; }
        public string SellCurrency { get; set; }
        public string AdditionalServiceFee { get; set; }
        public string CancellationAmount { get; set; }
        public string CancellationCurrency { get; set; }
        public string PaidDate { get; set; }
        public string PaidAmount { get; set; }
        public string PaymentTypeID { get; set; }






    }
    public class AirBagDetails
    {
        public string PAXTyp { get; set; }
        public string CabinBaggageQuantity { get; set; }
        public string CabinBaggageUnit { get; set; }
        public string CheckinBaggageQuantity { get; set; }
        public string CheckinBaggageUnit { get; set; }
        public string fromSeg { get; set; }
        public string toseg { get; set; }

    }
    public class Fareruleseg
    {
        public string FareRule { get; set; }
        public string FareRef { get; set; }
        public string Segment { get; set; }
        public string FilingAirline { get; set; }
        public string MarketingAirline { get; set; }

    }
    public class CostBreakuppax
    {
        public string PaxType { get; set; }
        public string PaxtotalBaseNet { get; set; }
        public string PaxtotalTaxNet { get; set; }
        public string PaxmarkupTypeID { get; set; }
        public string PaxnetCurrency { get; set; }
        public string PaxmarkupValue { get; set; }
        public string PaxsellAmount { get; set; }
        public string PaxmarkupCurrency { get; set; }
        public string PaxsellCurrency { get; set; }
        public string PaxadditionalServiceFee { get; set; }
        public string PaxcancellationAmount { get; set; }
        public string PaxcancellationCurrency { get; set; }
        public string PaxpaidAmount { get; set; }
        public string TotalPaxQuantity { get; set; }

    }
    public class BookingData
    {
        public Int64 BookingRefID { get; set; }
        public Int64 userID { get; set; }
    }
}
