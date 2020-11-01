using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web;

namespace WebApi.Infrastructure.Common
{
    public static class ConficBase
    {
        public static string GetConfigAppValue(string key)
        {
            return ConfigurationManager.AppSettings[key].ToString();
        }
    }
}