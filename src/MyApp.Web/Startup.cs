using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using MyApp.Common.Modules.Extensions;
using MyApp.Web.Boots;

namespace MyApp.Web
{
    public class Startup
    {
        private readonly ILogger<Startup> _logger;
        private readonly IHostingEnvironment _env;

        public Startup(ILogger<Startup> logger, IHostingEnvironment env)
        {
            _logger = logger;
            _env = env;
        }

        public void ConfigureServices(IServiceCollection services)
        {
            services.AddMyModules();

            var mvcBuilder = services.AddMvc();
            mvcBuilder.AddMyModulePart();
            mvcBuilder.SetCompatibilityVersion(CompatibilityVersion.Version_2_2);
        }

        public void Configure(IApplicationBuilder app)
        {
            if (_env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseMyStaticFiles(_env, _logger);
            
            //do not forget to add[Area("Foo")] in controllers of area
            app.UseMvc(routes =>
            {
                routes.MapRoute(
                    name: "areas",
                    template: "{area:exists}/{controller=Home}/{action=Index}/{id?}");

                routes.MapRoute(
                    name: "default",
                    template: "{controller=Home}/{action=Index}/{id?}");
            });
        }
    }
}
