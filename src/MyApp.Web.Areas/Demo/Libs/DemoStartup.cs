using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using MyApp.Common.Modules;
using MyApp.Domain.Menus;
using MyApp.Web.Areas.Demo.Libs.AppServices;
using MyApp.Web.Areas.Demo.Libs.Domain;

namespace MyApp.Web.Areas.Demo.Libs
{
    public class DemoStartup : IModuleStartup
    {
        public int Order { get; } = 0;

        public void ConfigureServices(IServiceCollection services)
        {
            services.AddSingleton<IFooSingleton, FooService>();
            services.AddScoped<IFooScoped, FooService>();
            services.AddTransient<IFooTransient, FooService>();
            
            services.AddSingleton<IMenuProvider, DemoMenuProvider>();
        }

        public void Configure(IApplicationBuilder builder)
        {
        }
    }
}
