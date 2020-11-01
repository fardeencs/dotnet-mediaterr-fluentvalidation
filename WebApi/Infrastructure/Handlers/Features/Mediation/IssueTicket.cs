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
    public class IssueTicket : IAsyncRequestHandler<Models.IssueTickettModel, ResponseObject>
    {

        private const string BaseUrlMystyfly = "baseurlMystyfly";

        private const string FselecturlMystyfly = "FTicketurlMystyfly";

        private readonly IPartnerClient partnerClient;
        private readonly ISupplierAgencyServices supplierAgencyServices;


        public IssueTicket(ISupplierAgencyServices _supplierAgencyServices)
        {
            this.supplierAgencyServices = _supplierAgencyServices;
            var apiClient = new ApiClient();
            partnerClient = new PartnerClient(apiClient);
        }


        private async Task<bool> GetDataFromMystifly(List<Domain.IssueTicketEntity> list, Models.IssueTickettModel model)
        {
            var supplierAgencyDetails = supplierAgencyServices.GetSupplierRouteBySupplierCodeAndAgencyCode(model.ticketCreateTSTFromPricing.AgencyCode
               , model.ticketCreateTSTFromPricing.SupplierCode, "issueticket");
            //code to add supplier details in to request
            var allSupplierBasicDetails = await supplierAgencyServices.GetSupplierAgencyBasicDetailswithsuppliercode(model.ticketCreateTSTFromPricing.AgencyCode, "T", model.ticketCreateTSTFromPricing.SupplierCode);
            model.SupplierAgencyDetails = allSupplierBasicDetails;

            model.ticketCreateTSTFromPricing.AgencyCode = "";
            model.ticketCreateTSTFromPricing.SupplierCode = "";


            var result = await partnerClient.GetIssueTicketflight(supplierAgencyDetails.BaseUrl, supplierAgencyDetails.RequestUrl, model);
            string strData = JsonConvert.SerializeObject(result.Data);
            Domain.IssueTicketEntity partnerResponseEntity = JsonConvert.DeserializeObject<Domain.IssueTicketEntity>(strData);
            if (partnerResponseEntity != null)
            {
                if (partnerResponseEntity.TripDetailsResult.ItineraryInformation.Length > 0)
                {
                    //update details here strat                   
                    await supplierAgencyServices.UpdateTblBooking(partnerResponseEntity.TripDetailsResult.BookingId.ToString(), partnerResponseEntity.TripDetailsResult.ReservationItem[0].AirlinePNR);
                    await supplierAgencyServices.InsertIntotblBookingHistory(partnerResponseEntity.TripDetailsResult.BookingId.ToString(), model.ticketCreateTSTFromPricing.UserID.ToString(), "OK");
                    await supplierAgencyServices.UpdateTblAirpassemgersafterIssuedTicket(partnerResponseEntity.TripDetailsResult.BookingId.ToString(), partnerResponseEntity.TripDetailsResult.ReservationItem[0].AirlinePNR, partnerResponseEntity.TripDetailsResult.ItineraryInformation);
                    //update details here end
                }
                list.Add(partnerResponseEntity);
                return true;
            }
            return false;
        }




        public async Task<ResponseObject> Handle(IssueTickettModel message)
        {
            List<Domain.IssueTicketEntity> allsupplierData = new List<Domain.IssueTicketEntity>();

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