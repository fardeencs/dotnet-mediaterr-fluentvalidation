using Autofac;
using Autofac.Extras.CommonServiceLocator;

namespace WebApi.App_Start
{
    public class ContextConfig
    {
        public static void RegisterContext(ContainerBuilder builder)
        {
            builder.RegisterType<AutofacServiceLocator>().AsImplementedInterfaces();
        }
    }
}