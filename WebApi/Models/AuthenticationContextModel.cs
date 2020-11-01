using System;

namespace WebApi.Models
{
    public class AuthenticationContextModel
    {
        public string UserName { get; set; }
        public string ClearPassword { get; set; }
        public string RefreshTokenExpiration { get; set; }
        public Guid UserId { get; set; }
    }
}