using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using MyApp.Common.Modules.Extensions;

namespace MyApp.Web
{
    public class Startup
    {
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddMyModules();
        }

        public void Configure(IApplicationBuilder app)
        {
            app.UseMyModules();
        }
    }
}
