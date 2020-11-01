using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebApi.Models
{
    using Common;
    using MediatR;
    public class GetTripDetailsModel : IAsyncRequest<ResponseObject>
    {
        public Connectiontodbreq ConnectiontoDBreq { get; set; }
    }

    public class Connectiontodbreq
    {
        public string AgencyCodeDb { get; set; }
        public string SupplierCodeDb { get; set; }
        public string BookingRefID { get; set; }
        public string SupplierUniqueId { get; set; }
        public string SupplierPnr { get; set; }
    }

}