namespace DNI.Core.Contracts
{
    public interface IConvertor<TSource, TDestination>
    {
        bool TryConvert(TSource source, out TDestination result);
    }
}
