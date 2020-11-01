using Common;
using MediatR;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Net;
using System.Net.Http;
using Web.Core.Client;
using WebApi.Infrastructure.Client;
using Domain;
using System.Web.Script.Serialization;
using Newtonsoft.Json;
using Logic.Interface;
using WebApi.Infrastructure.Common;
using System;
using WebApi.Models;


#pragma warning disable 1998
#pragma warning disable 618

namespace WebApi.Infrastructure.Handlers.Features.Mediation
{
    public class BookFlight : IAsyncRequestHandler<BookFlightModel, ResponseObject>
    {

        private readonly IPartnerClient partnerClient;
        private readonly ISupplierAgencyServices supplierAgencyServices;

        public BookFlight(ISupplierAgencyServices _supplierAgencyServices)
        {
            this.supplierAgencyServices = _supplierAgencyServices;
            var apiClient = new ApiClient();
            partnerClient = new PartnerClient(apiClient);
        }

        private async Task<bool> GetDataFromMystifly(List<Domain.BookFlightResponse> list, BookFlightModel model)
        {
            var supplierAgencyDetails = supplierAgencyServices.GetSupplierRouteBySupplierCodeAndAgencyCode(model.BookFlightEntity.BookFlight.AgencyCode
               , model.BookFlightEntity.BookFlight.SupplierCode, "book/flights");

            //codt to add booking details in DB
            BookingData _BookingData = new BookingData();
            _BookingData = await supplierAgencyServices.BookFlights(model.BookFlightEntity.BookFlight.Email, supplierAgencyDetails.AgencyID, model.BookFlightEntity.BookFlight.PhoneNumber,
                      model.BookFlightEntity.BookFlight.Faresourcecode, supplierAgencyDetails.SupplierId);

            //code to add air passengetdetails in to DB
            await supplierAgencyServices.AddAirPassengers(model.BookFlightEntity.BookFlight.TravelerInfo, _BookingData.BookingRefID.ToString(), model.BookFlightEntity.BookFlight.CustomerInfo.Email, model.BookFlightEntity.BookFlight.CustomerInfo.PhoneNumber, model.BookFlightEntity.BookFlight.CustomerInfo.PhoneCountry, _BookingData.userID.ToString());
            // await supplierAgencyServices.AddAirbookingCost(model.BookFlight.TravelerInfo, bookingID);
            //Add data to tblairbookingcost start
            var BookingCostID = await supplierAgencyServices.InsertIntotblairbookingcost(_BookingData.BookingRefID.ToString(), Convert.ToDouble(model.Totalfaregroup.TotalBaseNet),
                Convert.ToDouble(model.Totalfaregroup.TotalTaxNet), Convert.ToDouble(model.Totalfaregroup.PaidAmount), model.Totalfaregroup.NetCurrency,
                Convert.ToInt16(model.Totalfaregroup.MarkupTypeID), Convert.ToDouble(model.Totalfaregroup.MarkupValue), model.Totalfaregroup.MarkupCurrency,
                Convert.ToDouble(model.Totalfaregroup.SellAmount), model.Totalfaregroup.SellCurrency,
               Convert.ToDouble(model.Totalfaregroup.AdditionalServiceFee), Convert.ToDouble(model.Totalfaregroup.CancellationAmount), model.Totalfaregroup.CancellationCurrency);
            //Add data to tblairbookingcost end
            //Add to AirBookingCostBreakup start

            await supplierAgencyServices.InsertIntotblAirBookingCostBreakup(BookingCostID.ToString(), model.CostBreakuppax);
            //Add to AirBookingCostBreakup end
            //Add to PaymentDetails start
            await supplierAgencyServices.InsertIntotblPayment(_BookingData.BookingRefID.ToString(), Convert.ToDouble(model.Totalfaregroup.PaidAmount), model.Totalfaregroup.SellCurrency, model.Totalfaregroup.PaidDate, Convert.ToInt16(model.Totalfaregroup.PaymentTypeID));
            //Add to PaymentDetails End
            //Add to AirOrigingDestination start
            await supplierAgencyServices.InsertIntotblAirOriginDestinationOptions(_BookingData.BookingRefID.ToString(), model.BookFlightEntity.BookFlight.FLLegGroup);
            //Add to AirOrigingDestination End
            //Add to AirbaggageDetails start
            await supplierAgencyServices.InsertIntotblAirbaggageDetails(_BookingData.BookingRefID.ToString(), model.AirBagDetails);
            //Add to AirbaggageDetails End
            //Add to farerules start
            await supplierAgencyServices.InsertIntotblAirFarerules(_BookingData.BookingRefID.ToString(), model.Fareruleseg);
            //Add to farerules End
            //code to add supplier details in to request         
            var allSupplierBasicDetails = await supplierAgencyServices.GetSupplierAgencyBasicDetailswithsuppliercode(model.BookFlightEntity.BookFlight.AgencyCode, "T", model.BookFlightEntity.BookFlight.SupplierCode);
            model.BookFlightEntity.BookFlight.SupplierAgencyDetails = allSupplierBasicDetails;

            model.BookFlightEntity.BookFlight.SupplierCode = "";
            model.BookFlightEntity.BookFlight.BookingId = _BookingData.BookingRefID.ToString();

            string modelStr = JsonConvert.SerializeObject(model.BookFlightEntity);

            var result = await partnerClient.GetBookflight(supplierAgencyDetails.BaseUrl, supplierAgencyDetails.RequestUrl, model.BookFlightEntity);
            string strData = JsonConvert.SerializeObject(result.Data);
            Domain.BookFlightResponse partnerResponseEntity = JsonConvert.DeserializeObject<Domain.BookFlightResponse>(strData);
            if (partnerResponseEntity != null)
            {
                string airlinePNR = partnerResponseEntity.BookFlightResult.Airlinepnr;
                string uniqueID = partnerResponseEntity.BookFlightResult.UniqueID;
                if ((airlinePNR != "NIL" || uniqueID != "NIL") && (airlinePNR != "NA" || uniqueID != "NA"))
                {
                    //Update tblbooking (UniqId,Pnr no)
                    await supplierAgencyServices.InsertIntotblBookingData(_BookingData.BookingRefID.ToString(), airlinePNR, uniqueID);
                    await supplierAgencyServices.UpdateTblBooking(_BookingData.BookingRefID.ToString(), partnerResponseEntity.BookFlightResult.Airlinepnr);
                    await supplierAgencyServices.UpdateTblAirpassemgers(_BookingData.BookingRefID.ToString(), partnerResponseEntity.BookFlightResult.Airlinepnr);
                    await supplierAgencyServices.InsertIntotblBookingHistory(_BookingData.BookingRefID.ToString(), _BookingData.userID.ToString(), "HK");
                    //Update tblairpassengerdetails (Pnr no)
                }
                list.Add(partnerResponseEntity);
                return true;
            }
            return false;
        }

        //private static void AvoidBookFlightObject(BookFlightModel model)
        //{
        //    model.BookFlight.SupplierCode = "";
        //    model.BookFlight.AirBagDetails.RemoveRange(0, model.BookFlight.AirBagDetails.ToList().Count());// = "";
        //    model.BookFlight.Fareruleseg.RemoveRange(0, model.BookFlight.Fareruleseg.ToList().Count());
        //    //model.BookFlight.Totalfaregroup. //= "";
        //    //model.BookFlight.CostBreakuppax = "";
        //    model.BookFlight.PaymentInfo.PaidAmount = "";
        //    model.BookFlight.PaymentInfo.PaymentTypeID = "";
        //    model.BookFlight.PaymentInfo.PaidDate = "";
        //    model.BookFlight.FLLegGroup.ForEach(_ =>
        //    {
        //        _.Segments.ForEach(s =>
        //        {
        //            s.TerminalFrom = "";
        //            s.TerminalTo = "";
        //            s.Flightequip = "";
        //        });
        //    });
        //}

        public async Task<ResponseObject> Handle(BookFlightModel message)
        {
            List<Domain.BookFlightResponse> allsupplierData = new List<Domain.BookFlightResponse>();

            bool mystiflyResponse = await GetDataFromMystifly(allsupplierData, message);

            var response = new ResponseObject
            {
                ResponseMessage = new HttpResponseMessage(HttpStatusCode.OK),
                Data = allsupplierData,
                Message = "Data retrieved Successfully",
                IsSuccessful = true
            };
            return response;
        }
    }
}