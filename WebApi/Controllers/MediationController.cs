using System.Net;
using System.Net.Http;
using System.Reflection;
using System.Threading.Tasks;
using System.Web.Http;
using MediatR;
using WebApi.Infrastructure.Handlers.Features.Mediation;
using WebApi.Models;
using Domain;

namespace WebApi.Controllers
{
    [RoutePrefix("api/mediation")]
    public class MediationController : BaseController
    {
        public MediationController(IMediator mediatR)
        {
            MediatR = mediatR;
        }

        [HttpGet]
        [Route("version")]
        public string Version()
        {
            return $"{Assembly.GetExecutingAssembly().GetName().Version}";
        }


        [HttpGet]
        //[Route("getall/{page:int}/{pageSize:int}/{SearchString?}/{SortOrder?}", Name = "GetAll")]
        //[Route("getall/{page:int}/{pageSize:int}", Name = "GetAll")]
        [Route("getall")]
        public async Task<IHttpActionResult> GetAll()
        {
            GetAllDataRequest model = new GetAllDataRequest();
            var result = await MediatR.SendAsync(model);
            return ResponseMessage(Request.CreateResponse(HttpStatusCode.OK, result));
        }

        [HttpPost]
        [Route("search/flights")]
        public async Task<IHttpActionResult> SearchFlights(WebApi.Models.Rootobject model)
        {
            var result = await MediatR.SendAsync(model);
            return ResponseMessage(Request.CreateResponse(HttpStatusCode.OK, result));
        }



        [HttpPost]
        [Route("select/flights")]
        public async Task<IHttpActionResult> SelectFlights(SelectFlightModel model)
        {
            var result = await MediatR.SendAsync(model);
            return ResponseMessage(Request.CreateResponse(HttpStatusCode.OK, result));
        }


        [HttpPost]
        [Route("book/flights")]
        public async Task<IHttpActionResult> BookFlights(BookFlightModel model)
        {
            var result = await MediatR.SendAsync(model);
            return ResponseMessage(Request.CreateResponse(HttpStatusCode.OK, result));
        }

        [HttpPost]
        [Route("issueticket")]
        public async Task<IHttpActionResult> IssueTicket(WebApi.Models.IssueTickettModel model)
        {
            var result = await MediatR.SendAsync(model);
            return ResponseMessage(Request.CreateResponse(HttpStatusCode.OK, result));
        }
        [HttpPost]
        [Route("getDBtripdetails")]
        public async Task<IHttpActionResult> GetTripDetails(WebApi.Models.GetTripDetailsModel model)
        {
            var result = await MediatR.SendAsync(model);
            return ResponseMessage(Request.CreateResponse(HttpStatusCode.OK, result));
        }
    }
}