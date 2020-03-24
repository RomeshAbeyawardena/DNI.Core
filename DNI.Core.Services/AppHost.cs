namespace DNI.Core.Services
{
    using DNI.Core.Contracts;

    public static class AppHost
    {
        public static IAppHost<TStartup> Build<TStartup>()
            where TStartup : class
        {
            return new DefaultAppHost<TStartup>();
        }
    }
}
