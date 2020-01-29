using DNI.Shared.Contracts;

namespace DNI.Shared.Services
{
    public static class Switch
    {
        public static ISwitch<TKey, TValue> Create<TKey, TValue>()
        {
            return DefaultSwitch<TKey, TValue>.Create();
        }

        public static ISwitch<string, object> CreateObjectDictionary()
        {
            return Create<string, object>();
        }

        public static ISwitch<string, string> CreateStringDictionary()
        {
            return Create<string, string>();
        }
    }
}
