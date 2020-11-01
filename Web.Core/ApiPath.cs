using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Configuration;

namespace Web.Core
{
    public interface IApiPath
    {
        string BaseUrl { get; }
        string TokenUrl { get; }
        string GetTokenInfo { get; }
        string LoggedinUserInfo { get; }

        string GetAllPartnerData { get; }
    }


    public class ApiPath : IApiPath
    {
        readonly string _apiUrl;
        public ApiPath()
        {
            _apiUrl = ConfigurationManager.AppSettings["ApiUrl"].ToString();
        }
        public string BaseUrl
        {
            get { return _apiUrl + "/api"; }
        }
        public string TokenUrl
        {
            get { return _apiUrl + "/token"; }
        }
        public string GetTokenInfo
        {
            get { return BaseUrl + "/Account/GetTokenInfo"; }
        }

        public string LoggedinUserInfo
        {
            get
            {
                return BaseUrl + "/Account/GetLoggedinUserIfo";
            }
        }

        public string GetAllPartnerData { get { return BaseUrl + "/Category/GetAll"; } }

        

    }
}
