using DNI.Shared.Contracts;

namespace DNI.Shared.Services
{
    public static class AppHost
    {
        public static IAppHost<TStartup> Build<TStartup>()
            where TStartup : class
        {
            return new DefaultAppHost<TStartup>();
        }
    }
}
