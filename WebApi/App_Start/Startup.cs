﻿using System.Web.Http;
using Autofac;
using Autofac.Integration.WebApi;
using log4net.Config;
using Microsoft.Owin;
using Microsoft.Owin.Cors;
using Owin;
using WebApi;
using WebApi.App_Start;

[assembly: OwinStartup(typeof (Startup))]
[assembly: XmlConfigurator(Watch = true)]

namespace WebApi
{
    public class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            var config = new HttpConfiguration();
            var builder = new ContainerBuilder();
            AutofacConfig.RegisterAutofacIoc(app, config, builder);
            AuthConfig.ConfigureOAuth(app);
            AutoMapperConfig.RegisterMapping();
            ContextConfig.RegisterContext(builder);
            LogicConfig.RegisterContext(builder);
            ApiHelperConfig.RegisterContext(builder);
            Log4NetConfig.RegisterLogger(builder);
            FluentValidationConfig.RegisterValidation(builder, config);
            var container = builder.Build();
            config.DependencyResolver = new AutofacWebApiDependencyResolver(container);
            WebApiConfig.Register(config);
            XmlConfigurator.Configure();
            builder.RegisterWebApiFilterProvider(GlobalConfiguration.Configuration);
            app.UseAutofacMiddleware(container);
            app.UseAutofacWebApi(config);
            app.UseWebApi(config);
            //app.UseCors(CorsOptions.AllowAll);
        }
    }
}