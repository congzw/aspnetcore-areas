using Microsoft.Extensions.DependencyInjection;

namespace MyApp.Common.Modules
{
    public interface IModuleServiceContext
    {
        IServiceCollection ApplicationServices { get; set; }
    }

    public class DefaultModuleServiceContext : IModuleServiceContext
    {
        public IServiceCollection ApplicationServices { get; set; }
    }
}
