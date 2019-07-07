using Autofac;
using Autofac.Integration.Mvc;
using Autofac.Integration.WebApi;
using GitHubUsers.HttpClients;
using GitHubUsers.Services;
using System.Reflection;
using System.Web.Http;
using System.Web.Mvc;

namespace GitHubUsers
{
    public class AutofacConfig
    {
        public static void RegisterDependencies()
        {
            var builder = new ContainerBuilder();

            // Register MVC controllers.
            builder.RegisterControllers(typeof(MvcApplication).Assembly);

            // Register API controllers.
            builder.RegisterApiControllers(Assembly.GetExecutingAssembly());

            // Register individual components
            builder.RegisterType<GitHubHttpClient>().As<IHttpClient>();
            builder.RegisterType<GitHubService>().As<IGitHubService>();

            var container = builder.Build();

            // Get HttpConfiguration.
            var config = GlobalConfiguration.Configuration;

            // Web API dependency resolver
            config.DependencyResolver = new AutofacWebApiDependencyResolver(container);

            // MVC dependency resolver
            DependencyResolver.SetResolver(new AutofacDependencyResolver(container));
        }
    }
}
