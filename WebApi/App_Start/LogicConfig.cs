using Autofac;
using Autofac.Extras.CommonServiceLocator;
using Contracts;
using DAL;
using Logic;
using Logic.Interface;
using WebApi.Infrastructure.Handlers.Logging;
using WebApi.Infrastructure.Processes;

namespace WebApi.App_Start
{
    public class LogicConfig
    {
        public static void RegisterContext(ContainerBuilder builder)
        {
            builder.RegisterType<UserServices>().As<IUserServices>();
            builder.RegisterType<SupplierAgencyServices>().As<ISupplierAgencyServices>();
        }
    }
}