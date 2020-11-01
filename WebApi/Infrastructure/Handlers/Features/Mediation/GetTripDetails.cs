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


namespace WebApi.Infrastructure.Handlers.Features.Mediation
{
    public class GetTripDetails: IAsyncRequestHandler<GetTripDetailsModel, ResponseObject>
    {
        private readonly IPartnerClient partnerClient;
        private readonly ISupplierAgencyServices supplierAgencyServices;
        public GetTripDetails(ISupplierAgencyServices _supplierAgencyServices)
        {
            this.supplierAgencyServices = _supplierAgencyServices;
            var apiClient = new ApiClient();
            partnerClient = new PartnerClient(apiClient);
        }
        private async Task<GetTripDetailsModelRS> GetTraveldetails(List<Domain.GetTripDetailsModelRS> list, Models.GetTripDetailsModel model)
        {
            GetTripDetailsModelRS _GetTripDetailsModelRS = new GetTripDetailsModelRS();

            _GetTripDetailsModelRS = await supplierAgencyServices.GetTravellerDetailsfromDB(model.ConnectiontoDBreq.BookingRefID.ToString());                      
            return _GetTripDetailsModelRS;
        }
        public async Task<ResponseObject> Handle(GetTripDetailsModel message)
        {
            List<Domain.GetTripDetailsModelRS> getTripDetailsModelRS = new List<Domain.GetTripDetailsModelRS>();
            GetTripDetailsModelRS _GetTripDetailsModelRS = new GetTripDetailsModelRS();
            _GetTripDetailsModelRS = await GetTraveldetails(getTripDetailsModelRS, message);

            var response = new ResponseObject
            {
                ResponseMessage = new HttpResponseMessage(HttpStatusCode.OK),
                Data = _GetTripDetailsModelRS,
                Message = "Data retrieved Successfully",
                IsSuccessful = true
            };
            return response;



        }
    }
}