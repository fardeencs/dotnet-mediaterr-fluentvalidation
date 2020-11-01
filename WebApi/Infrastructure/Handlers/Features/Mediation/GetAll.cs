using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using AutoMapper;
using Common;
using MediatR;
using Web.Core;
using Web.Core.Client;
using WebApi.Infrastructure.ApiModels;
using WebApi.Infrastructure.Client;
using Domain;
using System.Web.Script.Serialization;
using Newtonsoft.Json;


#pragma warning disable 1998
#pragma warning disable 618

namespace WebApi.Infrastructure.Handlers.Features.Mediation
{

    #region Handler
    public class GetAll : IAsyncRequestHandler<GetAllDataRequest, ResponseObject>
    {
        private const string PartnertUri = "http://localhost:18313/partnerone/all/sync";
        private const string IdKey = "id";
        private readonly IPartnerClient partnerClient;
        
        public GetAll()
        {
            var apiClient = new ApiClient();
            partnerClient = new PartnerClient(apiClient);
        }

        public async Task<ResponseObject> Handle(GetAllDataRequest message)
        {
            // var result1 = await partnerClient.GetPartnerAllData();
            List<Rootobject> allsupplierData = new List<Rootobject>();
            Rootobject partnerResponse1 = await GetDataFromPartnerOne(allsupplierData);
            Rootobject partnerResponse2 = await GetDataFromPartnerTwo(allsupplierData);
            Rootobject partnerResponse3 = await GetDataFromPartnerThree(allsupplierData);
            if (partnerResponse1 == null && partnerResponse2 == null && partnerResponse3 == null) return null;

            Rootobject filteredData = FilteredData(allsupplierData);

            var response = new ResponseObject
            {
                ResponseMessage = new HttpResponseMessage(HttpStatusCode.OK),
                Data = filteredData,
                Message = "Data retrieved Successfully",
                IsSuccessful = true
            };
            return response;
        }

        private Rootobject FilteredData(List<Rootobject> allsupplierData)
        {
            var allsupplieFlightIndexData = allsupplierData.SelectMany(x => x.fareMasterPricerTravelBoardSearchReply.flightIndex).ToList();

            Faremasterpricertravelboardsearchreply fistRooTObject = allsupplierData.First().fareMasterPricerTravelBoardSearchReply;
            Rootobject rootObject = new Rootobject();

            Faremasterpricertravelboardsearchreply faremasterpricertravelboardsearchreply = GetCommonRootObject(fistRooTObject);

            List<Flightindex> flightindexList = new List<Flightindex>();


            /*during debugg uncomment this code and varify your correct data*/
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string jsonString = serializer.Serialize(allsupplieFlightIndexData);

            var groupedFlightsByKey = allsupplieFlightIndexData
                                        .GroupBy(item => item.SegmentRef.key)
                                        .ToDictionary(grp => grp.Key, grp => grp.ToList());

            foreach (var item in groupedFlightsByKey)
            {
                Flightindex flightindex = item.Value.OrderBy(x => x.fare.amount).FirstOrDefault();
                flightindexList.Add(flightindex);
            }

            faremasterpricertravelboardsearchreply.flightIndex = flightindexList;

            rootObject.fareMasterPricerTravelBoardSearchReply = faremasterpricertravelboardsearchreply;

            return rootObject;

        }

        private static Faremasterpricertravelboardsearchreply GetCommonRootObject(Faremasterpricertravelboardsearchreply fistRooTObject)
        {
            Conversionrate conversionrate = new Conversionrate();
            Conversionratedetail conversionratedetail = new Conversionratedetail()
            {
                currency = fistRooTObject.conversionRate.conversionRateDetail.currency
            };
            conversionrate.conversionRateDetail = conversionratedetail;

            Replystatus replystatus = new Replystatus();
            Status status = new Status()
            {
                advisoryTypeInfo = fistRooTObject.replyStatus.status.advisoryTypeInfo
            };
            replystatus.status = status;

            Faremasterpricertravelboardsearchreply faremasterpricertravelboardsearchreply = new Faremasterpricertravelboardsearchreply();
            faremasterpricertravelboardsearchreply.conversionRate = conversionrate;
            faremasterpricertravelboardsearchreply.replyStatus = replystatus;
            return faremasterpricertravelboardsearchreply;
        }
        private async Task<Domain.Rootobject> GetDataFromPartnerOne(List<Domain.Rootobject> list)
        {
            string baseUri = "http://localhost:18313/";
            //string reqUri = "partnerone/all/async";
            string reqUri = "mystifly-partner/rootobject";
            var result = await partnerClient.GetPartnerData(baseUri, reqUri);
            string strData = JsonConvert.SerializeObject(result.Data);
            Domain.Rootobject partnerResponseEntity = JsonConvert.DeserializeObject<Domain.Rootobject>(strData);
            list.Add(partnerResponseEntity);
            return partnerResponseEntity;
        }

        private async Task<Domain.Rootobject> GetDataFromPartnerTwo(List<Domain.Rootobject> list)
        {
            string baseUri = "http://localhost:18292/";
            string reqUri = "partnertwo/python";
            var result = await partnerClient.GetPartnerData(baseUri, reqUri);
            string strData = JsonConvert.SerializeObject(result.Data);
            Domain.Rootobject partnerResponseEntity = JsonConvert.DeserializeObject<Domain.Rootobject>(strData);
            list.Add(partnerResponseEntity);
            return partnerResponseEntity;
        }

        private async Task<Domain.Rootobject> GetDataFromPartnerThree(List<Domain.Rootobject> list)
        {
            string baseUri = "http://localhost:18293/";
            string reqUri = "partnerthree/amadeus";
            var result = await partnerClient.GetPartnerData(baseUri, reqUri);
            string strData = JsonConvert.SerializeObject(result.Data);
            Domain.Rootobject partnerResponseEntity = JsonConvert.DeserializeObject<Domain.Rootobject>(strData);
            list.Add(partnerResponseEntity);
            return partnerResponseEntity;
        }
        #region --test code--
        //public async Task<ResponseObject> Handle(GetAllDataRequest message)
        //{
        //    var result = new Logic.Mediation().GetAll();
        //    if (result == null) return null;

        //    var totalCount = result.ToList().Count;
        //    var totalPages = (int)Math.Ceiling((double)totalCount / message.PageSize);

        //    //result = result
        //    //    .Skip(message.PageSize * message.Page)
        //    //    .Take(message.PageSize)
        //    //    .ToList();

        //    //var mappedResult = Mapper.Map<IEnumerable<Domain.ResponseEntity>, List<ResponseModel>>(result);
        //    var mappedResult = result.Select(Mapper.Map<ResponseModel>).ToList();

        //    var output = new GetAllDataResponse
        //    {
        //        CurrentCount = message.Page,
        //        NextLink = "",
        //        PreviousLink = "",
        //        TotalCount = totalCount,
        //        TotalPages = totalPages,
        //        ResponseModels = mappedResult
        //    };

        //    var response = new ResponseObject
        //    {
        //        ResponseMessage = new HttpResponseMessage(HttpStatusCode.OK),
        //        Data = output,
        //        Message = "Data retrieved Successfully",
        //        IsSuccessful = true
        //    };
        //    return response;
        //}
        #endregion
    }

    #endregion

    #region Request/Response Model

    public class GetAllDataRequest : IAsyncRequest<ResponseObject>
    {
        //public string SortOrder { get; set; }
        //public string SearchString { get; set; }
        //public string CurrentFilter { get; set; }
        public int Page { get; set; }
        public int PageSize { get; set; }
    }

    public class GetAllDataResponse : IAsyncRequest<ResponseObject>
    {
        public List<ResponseModel> ResponseModels { get; set; }
        public string NextLink { get; set; }
        public string PreviousLink { get; set; }
        public int TotalPages { get; set; }
        public int TotalCount { get; set; }
        public int CurrentCount { get; set; }
    }

    public class ResponseModel
    {
        public string UserName { get; set; }
        public string Password { get; set; }
        public DateTime DateCaptured { get; set; }
        public string UserId { get; set; }
    }
    #endregion
}