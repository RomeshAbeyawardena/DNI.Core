namespace DNI.Core.Services.Abstraction
{
    using AutoMapper;
    using DNI.Core.Contracts;
    using DNI.Core.Domains;
    using DNI.Core.Services.Attributes;
    using MediatR;
    using Microsoft.AspNetCore.Mvc;

#pragma warning disable CA1012
    [Route("api/{controller}/{action}")]
    public abstract class DefaultApiControllerBase : DefaultControllerBase
    {
        public DefaultApiControllerBase(IMediator mediatorService, IMapper mapperProvider)
            : base(mediatorService, mapperProvider)
        {
        }
    }
#pragma warning restore CA1012
}
