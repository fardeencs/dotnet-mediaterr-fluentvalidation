
namespace WebApi.Models
{
    using Common;
    using MediatR;
    using System.Collections.Generic;
    using Domain.ViewModels;
    using Domain;

    public class IssueTickettModel : IAsyncRequest<ResponseObject>
    {


        public Ticketcreatetstfrompricing ticketCreateTSTFromPricing { get; set; }
        public List<SupplierAgencyDetails> SupplierAgencyDetails { get; set; }
        public Psalist[] psaList { get; set; }
    }

    public class Ticketcreatetstfrompricing
    {
        public string AgencyCode { get; set; }
        public string SupplierCode { get; set; }
        public string FareSourceCode { get; set; }
        public string SessionId { get; set; }
        public string Target { get; set; }
        public string UniqueID { get; set; }
        public string BookingRefID { get; set; }
        public string UserID { get; set; }
    }

    public class Psalist
    {
        public Itemreference itemReference { get; set; }
    }

    public class Itemreference
    {
        public string referenceType { get; set; }
        public string uniqueReference { get; set; }
    }

}
