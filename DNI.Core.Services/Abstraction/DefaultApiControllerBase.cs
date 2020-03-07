using DNI.Core.Domains;
using DNI.Core.Services.Attributes;
using Microsoft.AspNetCore.Mvc;

namespace DNI.Core.Services.Abstraction
{
    [HandleModelStateError]
    public abstract class DefaultApiControllerBase : DefaultControllerBase
    {
        protected ActionResult ResponseResult(ResponseBase response)
        {
            if(response.IsSuccessful)
                return Ok(response);

            AddErrorsToModelState(response);

            return BadRequest(ModelState);
        }
    }
}
