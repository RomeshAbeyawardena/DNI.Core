using DNI.Core.Contracts;
using DNI.Core.Domains;
using DNI.Core.Services.Attributes;
using Microsoft.AspNetCore.Mvc;

namespace DNI.Core.Services.Abstraction
{
    #pragma warning disable CA1012
    [Route("api/{controller}/{action}")]
    public abstract class DefaultApiControllerBase : DefaultControllerBase
    {
        public DefaultApiControllerBase(IMediatorService mediatorService, IMapperProvider mapperProvider) 
            : base(mediatorService, mapperProvider)
        {

        }
    }
    #pragma warning restore CA1012
}
