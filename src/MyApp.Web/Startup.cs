using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.FileProviders;
using Microsoft.Extensions.Logging;
using MyApp.Web.Areas.Demo;
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
            var mvcBuilder = services.AddMvc();
            
            //services.AddRazorPages(opt => opt.RootDirectory = "/")
            //    .AddRazorRuntimeCompilation(
            //        opt => { opt.FileProviders.Add(new PhysicalFileProvider(WebRoot)); });

            mvcBuilder.AddApplicationPart(typeof(DemoHelper).Assembly);

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
