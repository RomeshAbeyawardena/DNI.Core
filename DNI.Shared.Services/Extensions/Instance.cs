using System;

namespace DNI.Shared.Services.Extensions
{
    public sealed class Instance<T>
    {
        private T _value;
        private readonly Func<T> _createExpression;
        private Instance(Func<T> createExpression)
        {
            _createExpression = createExpression;
        }

        public static Instance<T> Create(Func<T> createExpression)
        {
            return new Instance<T>(createExpression);
        }

        public bool Is(Func<T, bool> isExpression)
        {
            return isExpression(Value);
        }

        public T Value { get => _value ?? (_value = _createExpression()); }
    }
}
