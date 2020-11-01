using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain
{
    public class BookFlightResponse
    {


            public Bookflightresult BookFlightResult { get; set; }
        }

        public class Bookflightresult
        {
            public Error[] Errors { get; set; }
            public string Status { get; set; }
            public string Success { get; set; }
            public string Target { get; set; }
            public string TktTimeLimit { get; set; }
            public string UniqueID { get; set; }
            public string Airlinepnr { get; set; }
            public string Sessionid { get; set; }
            public string Faresourcecode { get; set; }
        }

        public class Error
        {
            public string Code { get; set; }
            public string Meassage { get; set; }
        }

    }

