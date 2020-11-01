using System.Data.Entity;
using Autofac;
using Autofac.Core;
using DAL;
using Web.Core.Client;
using WebApi.Infrastructure.Client;

namespace WebApi.App_Start
{
    public class ApiHelperConfig
    {
        public static void RegisterContext(ContainerBuilder builder)
        {
            //builder.RegisterType<ApiClient>().As<IApiClient>();
            //builder.RegisterType<PartnerClient>().As<IPartnerClient>();
            //builder.RegisterType<MediationEntities>()
            //        .As<DbContext>()
            //            .WithParameter((pi, c) => pi.Name == "connectionString",
            //                (pi, c) => c.Resolve<IConnectionStringProvider>().ConnectionString);

        }
    }
}