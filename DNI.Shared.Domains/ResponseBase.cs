using FluentValidation.Results;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DNI.Shared.Domains
{
    public static class Response
    {
        
        public static TResponse Failed<TResponse>(params ValidationFailure[] validationErrors)
            where TResponse : ResponseBase
        {
            var response =  Activator.CreateInstance<TResponse>();
            response.IsSuccessful = false;
            response.Errors = validationErrors;
            return response;
        }

        public static TResponse Success<TResponse>(object result)
        {
            var responseType = typeof(TResponse);
            var response =  Activator.CreateInstance<TResponse>();
            
            var properties = responseType.GetProperties();

            var resultProperties = properties.Where(property => property.Name == nameof(ResponseBase.Result));

            if(!resultProperties.Any())
                throw new InvalidCastException();

            foreach(var resultProperty in resultProperties)
                resultProperty.SetValue(response, result);

            var successfulProperty = properties.FirstOrDefault(property => property.Name == nameof(ResponseBase.IsSuccessful));

            if(successfulProperty == null)
                throw new InvalidCastException();

            successfulProperty.SetValue(response, true);

            return response;
        }
    }

    public abstract class ResponseBase
    {
        public bool IsSuccessful { get; set; }
        public IEnumerable<ValidationFailure> Errors { get; set; }
        public object Result { get; set; }
    }

    public abstract class ResponseBase<T> : ResponseBase
    {
        public new T Result { get; set; }
    }
}
