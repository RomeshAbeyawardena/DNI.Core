using DNI.Shared.Domains;
using DNI.Shared.Services.Attributes;
using Microsoft.AspNetCore.Mvc;

namespace DNI.Shared.Services.Abstraction
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
