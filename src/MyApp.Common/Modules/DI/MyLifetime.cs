namespace MyApp.Common.Modules.DI
{
    public interface IMyLifetime
    {

    }

    public interface IMySingleton : IMyLifetime
    {
    }

    public interface IMyScoped : IMyLifetime
    {
    }

    public interface IMyTransient : IMyLifetime
    {
    }
}
