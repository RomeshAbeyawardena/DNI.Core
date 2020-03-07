using DNI.Core.Contracts;

namespace DNI.Core.Services
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
