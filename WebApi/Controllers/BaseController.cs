using System.Web.Http;
using log4net;
using MediatR;

namespace WebApi.Controllers
{
    public class BaseController : ApiController
    {
        public IMediator MediatR { get; set; }
        //public ILog Log { get; set; }
    }
}