using Microsoft.Owin.Security.OAuth;
using Owin;


namespace WebApi
{
    public class AuthConfig
    {
        public static OAuthBearerAuthenticationOptions OAuthBearerOptions { get; private set; }

        public static void ConfigureOAuth(IAppBuilder app)
        {
        }
    }
}