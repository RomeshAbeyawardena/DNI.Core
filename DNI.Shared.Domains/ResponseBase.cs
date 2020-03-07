using FluentValidation.Results;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;

namespace DNI.Shared.Domains
{
    public static class Response
    {
        public static bool IsSuccessful(IResponse response)
        {
            return response.IsSuccessful && response.Result != null;
        }

        public static TResponse Failed<TResponse>(params ValidationFailure[] validationErrors)
            where TResponse : ResponseBase
        {
            var response = Activator.CreateInstance<TResponse>();
            response.IsSuccessful = false;
            response.Errors = validationErrors;
            return response;
        }

        public static TResponse Success<TResponse>(object result, Action<TResponse> responseParameterConfiguration = default)
        {
            var responseType = typeof(TResponse);
            var response = Activator.CreateInstance<TResponse>();

            responseParameterConfiguration?.Invoke(response);

            var properties = responseType.GetProperties();

            var resultProperties = properties
                .Where(property => property.Name == nameof(ResponseBase.Result));

            if (!resultProperties.Any())
                throw new InvalidCastException();

            foreach (var resultProperty in resultProperties)
                resultProperty.SetValue(response, result);

            var successfulProperty = properties
                .FirstOrDefault(property => property.Name == nameof(ResponseBase.IsSuccessful));

            if (successfulProperty == null)
                throw new InvalidCastException();

            successfulProperty.SetValue(response, true);

            return response;
        }
    }

    public interface IResponse<T>
    {
        T Result { get; set; }
    }

    public interface IResponse : IResponse<object>
    {
        bool IsSuccessful { get; set; }
        IEnumerable<ValidationFailure> Errors { get; set; }
    }

    public abstract class ResponseBase : IResponse
    {
        public bool IsSuccessful { get; set; }
        public IEnumerable<ValidationFailure> Errors { get; set; }
        public object Result { get; set; }
    }

    public abstract class ResponseBase<T> : ResponseBase, IResponse<T>
    {
        [JsonIgnore]
        public new T Result
        {
            get => (T)base.Result;
            set => base.Result = value;
        }
    }
}
