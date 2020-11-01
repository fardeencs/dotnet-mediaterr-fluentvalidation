using Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WebApi.Infrastructure.Handlers.Features.Mediation;
using WebApi.Models;

namespace WebApi.Infrastructure.Client
{
    public interface IPartnerClient
    {
        Task<PartnerResponse> GetPartnerAllData();
        Task<ResponsePackage> GetPartnerData(string baseUri, string reqUri);
        Task<ResponsePackage> GetPartnerData(string baseUri, string reqUri, Models.Rootobject model);

        Task<ResponsePackage> Getselectflight(string baseUri, string reqUri, SelectFlightModel model);

        Task<ResponsePackage> GetBookflight(string baseUri, string reqUri, Models.BookFlightEntity model);

        Task<ResponsePackage> GetIssueTicketflight(string baseUri, string reqUri, Models.IssueTickettModel model);
    }
}
