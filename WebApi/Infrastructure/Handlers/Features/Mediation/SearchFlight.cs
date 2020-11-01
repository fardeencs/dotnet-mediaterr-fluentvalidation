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
using Newtonsoft.Json;
using Logic.Interface;
using WebApi.Infrastructure.Common;
using System.Web.Script.Serialization;
using System;
using System.IO;
using System.Xml;


#pragma warning disable 1998
#pragma warning disable 618

namespace WebApi.Infrastructure.Handlers.Features.Mediation
{
    public class SearchFlight : IAsyncRequestHandler<Models.Rootobject, ResponseObject>
    {
        #region --private members--
        private const string BaseUrlMystyfly = "baseurlMystyfly";
        private const string BaseUrlPython = "baseurlPython";
        private const string BaseUrlAmadeous = "baseurlAmadeous";
        private const string amadeusjsonurl = "Amadeousjsonurl";


        private const string ReqUrlMystyfly = "requrlMystyfly";
        private const string ReqUrlPython = "requrlPython";
        private const string ReqUrlAmadeous = "requrlAmadeous";

        private const string SupplierCodeMystyfly = "supplierCodeMystyfly";
        private const string SupplierCodePython = "supplierCodePython";
        private const string SupplierCodeAmadeous = "supplierCodeAmadeous";

        private readonly IPartnerClient partnerClient;
        private readonly ISupplierAgencyServices supplierAgencyServices;

        #endregion
        //private readonly IApiClient apiClient;
        //public SearchFlight(IPartnerClient _partnerClient, IApiClient _apiClient)
        //{
        //    var apiClient = new ApiClient();
        //    partnerClient = new PartnerClient(apiClient);
        //    //apiClient = _apiClient;
        //    //partnerClient = _partnerClient;
        //}

        #region --conntructor--
        public SearchFlight(ISupplierAgencyServices _supplierAgencyServices)
        {
            this.supplierAgencyServices = _supplierAgencyServices;
            var apiClient = new ApiClient();
            partnerClient = new PartnerClient(apiClient);
        }

        #endregion

        #region --handler--
        public async Task<ResponseObject> Handle(Models.Rootobject message)
        {
            var allSupplierBasicDetails = await supplierAgencyServices.GetSupplierAgencyBasicDetails(message.CommonRequestSearch.AgencyCode, "T");

            //if (allSupplierBasicDetails != null && allSupplierBasicDetails.Count() > 0)
            //{
            //    message.SupplierAgencyDetails = allSupplierBasicDetails;
            //    //JavaScriptSerializer serializer = new JavaScriptSerializer();
            //    //string jsonString = serializer.Serialize(message);
            //}
            string key = GenrateSavedResponseKey(message);

            List<Domain.Rootobject> allsupplierData = new List<Domain.Rootobject>();

            CreateResponseTime("Mystifly");
            //  bool mystiflyResponse = await GetDataFromMystifly(allsupplierData, message, allSupplierBasicDetails, key);
            Task<bool> mystiflyResponse = GetDataFromMystifly(allsupplierData, message, allSupplierBasicDetails, key);

            //var pythonWatch = System.Diagnostics.Stopwatch.StartNew();
            CreateResponseTime("Python");
            //bool pythonResponse = await GetDataFromPython(allsupplierData, message, allSupplierBasicDetails, key);
            Task<bool> pythonResponse = GetDataFromPython(allsupplierData, message, allSupplierBasicDetails, key);

            CreateResponseTime("Amedious");
            // bool amadiousResponse = await GetDataFromAmadeus(allsupplierData, message, allSupplierBasicDetails, key);
            Task<bool> amadiousResponse = GetDataFromAmadeus(allsupplierData, message, allSupplierBasicDetails, key);

            //await Task.Delay(15000);

            await mystiflyResponse;
            await pythonResponse;
            await amadiousResponse;


            if (allsupplierData == null || allsupplierData.Count() <= 0) return null;

            Domain.Rootobject filteredData = FilteredData(allsupplierData);

            var response = new ResponseObject
            {
                ResponseMessage = new HttpResponseMessage(HttpStatusCode.OK),
                Data = filteredData,
                Message = "Data retrieved Successfully",
                IsSuccessful = true
            };
            return response;
        }

        #endregion

        #region --request to indivisual partners api's-- 
        private async Task<bool> GetDataFromMystifly(List<Domain.Rootobject> list, Models.Rootobject model, List<SupplierAgencyDetails> allSupplierAgencyDetails, string key)
        {
            AddSupplierDetailsToRequestObject(model, allSupplierAgencyDetails, SupplierCodeMystyfly);

            // JavaScriptSerializer serializer = new JavaScriptSerializer();
            // string jsonString = serializer.Serialize(model);
            string strData = string.Empty;
            string baseUri = ConficBase.GetConfigAppValue(BaseUrlMystyfly);
            string reqUri = ConficBase.GetConfigAppValue(ReqUrlMystyfly);

            bool isFetchedFromDb = false;
            strData = await GetSupplierResponseFromDb(model, ConficBase.GetConfigAppValue(SupplierCodeMystyfly), key);
            if (string.IsNullOrEmpty(strData))
            {
                var result = await partnerClient.GetPartnerData(baseUri, reqUri, model);
                strData = JsonConvert.SerializeObject(result.Data);
                isFetchedFromDb = true;
            }
            Domain.Rootobject partnerResponseEntity = JsonConvert.DeserializeObject<Domain.Rootobject>(strData);
            if (partnerResponseEntity != null)
            {
                if (partnerResponseEntity.fareMasterPricerTravelBoardSearchReply.flightIndex != null
                                && partnerResponseEntity.fareMasterPricerTravelBoardSearchReply.flightIndex.ToList().Count() > 0)
                {
                    list.Add(partnerResponseEntity);

                    if (isFetchedFromDb)
                    {
                        AddSearchDetails(model, strData, ConficBase.GetConfigAppValue(SupplierCodeMystyfly), key);
                    }
                    // CreateJSONDoc(strData, model.CommonRequestSearch.AgencyCode, ConficBase.GetConfigAppValue(SupplierCodeMystyfly), key);
                    return true;
                }
            }

            return false;
        }

        private async Task<bool> GetDataFromPython(List<Domain.Rootobject> list, Models.Rootobject model, List<SupplierAgencyDetails> allSupplierAgencyDetails, string key)
        {
            string strData = string.Empty;
            AddSupplierDetailsToRequestObject(model, allSupplierAgencyDetails, SupplierCodePython);
            string baseUri = ConficBase.GetConfigAppValue(BaseUrlPython);
            string reqUri = ConficBase.GetConfigAppValue(ReqUrlPython);

            bool isFetchedFromDb = false;

            strData = await GetSupplierResponseFromDb(model, ConficBase.GetConfigAppValue(SupplierCodePython), key);
            if (string.IsNullOrEmpty(strData))
            {
                var result = await partnerClient.GetPartnerData(baseUri, reqUri, model);
                strData = JsonConvert.SerializeObject(result.Data);
                isFetchedFromDb = true;
            }
            Domain.Rootobject partnerResponseEntity = JsonConvert.DeserializeObject<Domain.Rootobject>(strData);
            if (partnerResponseEntity != null)
            {
                if (partnerResponseEntity.fareMasterPricerTravelBoardSearchReply.flightIndex != null && partnerResponseEntity.fareMasterPricerTravelBoardSearchReply.flightIndex.ToList().Count() > 0)
                {
                    list.Add(partnerResponseEntity);
                    if (isFetchedFromDb)
                    {
                        AddSearchDetails(model, strData, ConficBase.GetConfigAppValue(SupplierCodePython), key);
                    }
                    //   CreateJSONDoc(strData, model.CommonRequestSearch.AgencyCode, ConficBase.GetConfigAppValue(SupplierCodePython), key);
                    return true;
                }
            }

            return false;
        }

        private async Task<bool> GetDataFromAmadeus(List<Domain.Rootobject> list, Models.Rootobject model, List<SupplierAgencyDetails> allSupplierAgencyDetails, string key)
        {
            string strData = string.Empty;
            AddSupplierDetailsToRequestObject(model, allSupplierAgencyDetails, SupplierCodeAmadeous);

            string baseUri = ConficBase.GetConfigAppValue(BaseUrlAmadeous);
            string reqUri = ConficBase.GetConfigAppValue(ReqUrlAmadeous);
            bool isFetchedFromDb = false;

            //  strData = await GetSupplierResponseFromDb(model, ConficBase.GetConfigAppValue(SupplierCodeAmadeous), key);
            if (string.IsNullOrEmpty(strData))
            {
                var result = await partnerClient.GetPartnerData(baseUri, reqUri, model);
                strData = JsonConvert.SerializeObject(result.Data);
                isFetchedFromDb = true;
            }
            Domain.Rootobject partnerResponseEntity = JsonConvert.DeserializeObject<Domain.Rootobject>(strData);
            if (partnerResponseEntity != null)
            {
                if (partnerResponseEntity.fareMasterPricerTravelBoardSearchReply.flightIndex != null && partnerResponseEntity.fareMasterPricerTravelBoardSearchReply.flightIndex.ToList().Count() > 0)
                {
                    list.Add(partnerResponseEntity);
                    if (isFetchedFromDb)
                    {
                        AddSearchDetails(model, strData, ConficBase.GetConfigAppValue(SupplierCodeAmadeous), key);
                    }
                    //CreateJSONDoc(strData, model.CommonRequestSearch.AgencyCode, ConficBase.GetConfigAppValue(SupplierCodeAmadeous), key);

                    return true;
                }
            }
            return false;
        }

        #endregion

        #region --add supplier details to request object--
        private static void AddSupplierDetailsToRequestObject(Models.Rootobject model, List<SupplierAgencyDetails> allSupplierAgencyDetails, string supplierCodeKey)
        {
            List<SupplierAgencyDetails> supplierAgencyDetails = new List<SupplierAgencyDetails>();
            string supplierCode = ConficBase.GetConfigAppValue(supplierCodeKey);
            SupplierAgencyDetails supplierDetails = allSupplierAgencyDetails.FirstOrDefault(x => x.SupplierCode == supplierCode);
            string amaurl = ConficBase.GetConfigAppValue(amadeusjsonurl);
            supplierDetails.BaseUrl = amaurl;
            supplierAgencyDetails.Add(supplierDetails);

            if (supplierDetails != null)
                model.SupplierAgencyDetails = supplierAgencyDetails;
        }
        #endregion

        #region --manupulation by madiation after reciveing response from partners--
        private Domain.Rootobject FilteredData(List<Domain.Rootobject> allsupplierData)
        {
            var allsupplieFlightIndexData = allsupplierData.SelectMany(x => x.fareMasterPricerTravelBoardSearchReply.flightIndex).ToList();

            Faremasterpricertravelboardsearchreply fistRooTObject = allsupplierData.First().fareMasterPricerTravelBoardSearchReply;
            Domain.Rootobject rootObject = new Domain.Rootobject();

            Faremasterpricertravelboardsearchreply faremasterpricertravelboardsearchreply = GetCommonRootObject(fistRooTObject);

            List<Flightindex> flightindexList = new List<Flightindex>();


            /*during debugg uncomment this code and varify your correct data*/
            //JavaScriptSerializer serializer = new JavaScriptSerializer();
            //string jsonString = serializer.Serialize(allsupplieFlightIndexData);

            var groupedFlightsByKey = allsupplieFlightIndexData
                                        .GroupBy(item => item.SegmentRef.key)
                                        .ToDictionary(grp => grp.Key, grp => grp.ToList());

            foreach (var item in groupedFlightsByKey)
            {
                Flightindex flightindex = item.Value.OrderBy(x => x.fare.amount).FirstOrDefault();
                flightindexList.Add(flightindex);
            }

            faremasterpricertravelboardsearchreply.flightIndex = flightindexList.OrderBy(x => x.fare.amount).ToList();

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

        #endregion

        #region --get supplier response from db--
        private async Task<string> GetSupplierResponseFromDb(Models.Rootobject model, string supplierCode, string key)
        {
            AgencySupplierResponseEntity addSupplierDetails = new AgencySupplierResponseEntity();
            try
            {
                DateTime currentDateTime = DateTime.Now;
                addSupplierDetails.AgencyCode = model.CommonRequestSearch.AgencyCode;
                addSupplierDetails.SearchDateTime = currentDateTime;
                addSupplierDetails.Key = key;
                addSupplierDetails.Status = 1;
                addSupplierDetails.SupplierCode = supplierCode;

                string response = await supplierAgencyServices.GetAgencySupplierResponse(addSupplierDetails);

                return response;
            }
            catch (Exception ex)
            {
                return null;
            }


        }
        #endregion

        #region --save supplier response in db--
        private void AddSearchDetails(Models.Rootobject model, string response, string supplierCode, string key)
        {
            List<AgencySupplierResponseEntity> addSupplierDetails = new List<AgencySupplierResponseEntity>();
            try
            {

                // DateTime depDate = DateTime.ParseExact(_.DepartureDate, "dd-MM-yyyy", System.Globalization.CultureInfo.InvariantCulture);
                DateTime currentDateTime = DateTime.Now;
                addSupplierDetails.Add(new AgencySupplierResponseEntity()
                {

                    AgencyCode = model.CommonRequestSearch.AgencyCode,
                    SearchDateTime = currentDateTime,

                    Key = key,
                    Search = response,
                    Status = 1,
                    SupplierCode = supplierCode,

                });


                supplierAgencyServices.AddSupplierResponse(addSupplierDetails);
            }
            catch (Exception ex)
            {

                throw;
            }

        }
        #endregion

        public void CreateJSONDoc(string JSONDATA, string Agency, string Supplier, string key)
        {
            try
            {
                string date = DateTime.Now.ToString();
                string storePath = System.Web.HttpContext.Current.Server.MapPath("") + "/" + Agency + "/" + Supplier + "/SearchResponse/";
                if (!Directory.Exists(storePath))
                    Directory.CreateDirectory(storePath);
                string filename = key + ".json";
                string tempfilename = storePath + "/" + filename;
                File.WriteAllText(tempfilename, JSONDATA);
            }
            catch (Exception ex)
            {
                // Response.Write("XmlException: " + xmlEx.Message);
            }
        }

        public void CreateResponseTime(string partnerIdentity)
        {
            string content = "";
            try
            {
                string date = DateTime.Now.ToString();
                string storePath = System.Web.HttpContext.Current.Server.MapPath("") + "/ResponseTime/";
                if (!Directory.Exists(storePath))
                    Directory.CreateDirectory(storePath);
                string filename = "responsetime-" + partnerIdentity + ".txt";
                string tempfilename = storePath + "/" + filename;
                using (StreamWriter writetext = new StreamWriter(tempfilename))
                {
                    content = "=> " + date + "-" + partnerIdentity + Environment.NewLine;
                    writetext.WriteLine(content);
                }

                //  File.WriteAllText(tempfilename, content);
            }
            catch (Exception ex)
            {
            }
        }

        #region --genrate response key--
        private static string GenrateSavedResponseKey(Models.Rootobject message)
        {
            string key = "";
            string seginf = "";
            string passinf = "";
            int count = message.OriginDestinationInformation.Count();

            for (int i = 0; i < count; i++)
            {
                seginf += message.OriginDestinationInformation[i].OriginLocation + "-" + message.OriginDestinationInformation[i].DestinationLocation + "-" + message.OriginDestinationInformation[i].DepartureDate + "-";
            }
            passinf = message.PassengerTypeQuantity.ADT + "-" + message.PassengerTypeQuantity.CHD + "-" + message.PassengerTypeQuantity.INF + "-" + message.cabin;

            key = seginf + passinf;
            return key;
        }
        #endregion

    }
}