using Infrastructure.Interfaces;
using Domain.Types;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;

namespace WebAPI.Controllers
{
  [ApiController]
  [Route("api/substation")]
  [Authorize]
  public class SubstationController(ISubstation substation) : ControllerBase
  {
    [HttpPost]
    public async Task<ActionResult<SubstationResponse>> CreateSubstation(SubstationRequest request)
    {
      var result = await substation.CreateSubstation(request);
      return Created("", result);
    }

    [HttpPut("{substationId}")]
    public async Task<ActionResult<SubstationResponse>> UpdateSubstation([FromRoute] Guid substationId, [FromBody] SubstationRequest request)
    {
      var result = await substation.UpdateSubstation(substationId, request);
      return Accepted(result);
    }

    [HttpDelete("{substationId}")]
    public async Task<ActionResult<SubstationResponse>> DeleteSubstation([FromRoute] Guid substationId)
    {
      var result = await substation.DeleteSubstation(substationId);
      return Accepted(result);
    }
  }
}