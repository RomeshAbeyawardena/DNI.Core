namespace DNI.Core.Services.Extensions
{
    using System;

    public sealed class Instance<T>
    {
        private T value;
        private readonly Func<T> createExpression;

        private Instance(Func<T> createExpression)
        {
            this.createExpression = createExpression;
        }

        public static Instance<T> Create(Func<T> createExpression)
        {
            return new Instance<T>(createExpression);
        }

        public bool Is(Func<T, bool> isExpression)
        {
            return isExpression(Value);
        }

        public T Value { get => value ?? (value = createExpression()); }
    }
}
