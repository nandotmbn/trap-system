using Infrastructure.Interfaces;
using Domain.Types;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;

namespace WebAPI.Controllers;

[ApiController]
[Route("api/substation")]
[Authorize]
public class SubstationController(ISubstation substation) : ControllerBase
{
  [HttpGet]
  public async Task<ActionResult<SubstationsResponse>> GetSubstations(
    [FromQuery] string? search,
    [FromQuery] string? name,
    [FromQuery] string? address,
    [FromQuery] SortType sortType = SortType.Asc,
    [FromQuery] SubstationAttributes sortAttribute = SubstationAttributes.Name,
    [FromQuery] int page = 1,
    [FromQuery] int limit = 10
  )
  {
    var result = await substation.GetSubstations(search, name, address, sortType, sortAttribute, page, limit);
    return Ok(result);
  }

  [HttpGet("{substationId}")]
  public async Task<ActionResult<SubstationResponse>> GetSubstation([FromRoute] Guid substationId)
  {
    var result = await substation.GetSubstation(substationId);
    return Ok(result);
  }

  [HttpGet("count")]
  public async Task<ActionResult<CountResponse>> CountSubstations(
    [FromQuery] string? search,
    [FromQuery] string? name,
    [FromQuery] string? address
  )
  {
    var result = await substation.CountSubstations(search, name, address);
    return Ok(result);
  }

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