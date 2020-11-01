namespace Logic.Interface
{
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using Domain;

    public interface ISupplierAgencyServices
    {
        Task<List<SupplierAgencyDetails>> GetSupplierAgencyBasicDetails(string agencyCode, string status);
        Task<List<SupplierAgencyDetails>> GetSupplierAgencyBasicDetailswithsuppliercode(string agencyCode, string status, string supplierCode);
        SupplierAgencyDetails GetSupplierRouteBySupplierCodeAndAgencyCode(string agencyCode, string supplierCode, string routeFlag);
        Task<BookingData> BookFlights(string emailId, long agencyId, string mobileNo, string bookingRfNo, long supplierId);
        Task AddAirPassengers(List<Travelerinfo> message, string bookingId, string emailid, string telphone, string locationcode, string UserID);
        Task AddAirbookingCost(List<Travelerinfo> message, string bookingid);
        //       Task<bool> AddSupplierResponse(AgencySupplierResponseEntity message);
        Task<bool> AddSupplierResponse(List<AgencySupplierResponseEntity> message);
        Task<string> GetAgencySupplierResponse(AgencySupplierResponseEntity message);
        //Task<List<string>> GetAgencySupplierResponse(List<AgencySupplierResponseEntity> message, string supplierCode);
        Task<string> InsertIntotblairbookingcost(string BookingRefID, double TotalBaseNet,
          double TotalTaxNet, double TotalNet, string NetCurrency,
          int MarkupTypeID, double MarkupValue, string MarkupCurrency, double SellAmount,
          string SellCurrency, double AdditionalServiceFee, double CancellationAmount, string CancellationCurrency);
        Task InsertIntotblAirBookingCostBreakup(string bookingCostID, List<CostBreakuppax> costBreakuppax);
        Task InsertIntotblPayment(string BookingRefID, double paidAmount, string currencyCode, string paidDate, int paymentTypeID);
        Task InsertIntotblAirOriginDestinationOptions(string BookingRefID, List<Flleggroup> fLLegGroup);
        Task InsertIntotblAirbaggageDetails(string BookingRefID, List<AirBagDetails> airBagDetails);
        Task InsertIntotblAirFarerules(string BookingRefID, List<Fareruleseg> fareruleseg);
        Task UpdateTblBooking(string bookingRefID, string pnrNo);
        Task UpdateTblAirpassemgers(string bookingRefID, string pnrNo);
        Task InsertIntotblBookingHistory(string BookingRefID, string userID, string bookingStatusCode);
        Task UpdateTblAirpassemgersafterIssuedTicket(string bookingRefID, string pnrNo, Itineraryinformation[] itineraryInformation);
        Task<GetTripDetailsModelRS> GetTravellerDetailsfromDB(string bookingRefID);
        Task InsertIntotblBookingData(string BookingRefID, string PNR, string uniqueID);
        Task<string> GetUserIDfromRefID(string BookingRefID);
    }
}
