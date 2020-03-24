namespace DNI.Core.Contracts
{
    using System;

    public interface IInstanceServiceInjector
    {
        object CreateInstance(Type serviceType);

        TService CreateInstance<TService>();
    }
}
