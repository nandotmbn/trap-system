using Infrastructure.Interfaces;
using Domain.Types;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;

namespace WebAPI.Controllers;

[ApiController]
[Route("api/camera")]
[Authorize]
public class CameraController(ICamera camera) : ControllerBase
{
  [HttpGet]
  public async Task<ActionResult<CamerasResponse>> GetCameras(
    [FromQuery] string? search,
    [FromQuery] string? name,
    [FromQuery] string? ip,
    [FromQuery] int? port,
    [FromQuery] int? channel,
    [FromQuery] int? subtype,
    [FromQuery] string? username,
    [FromQuery] Guid? substationId,
    [FromQuery] SortType sortType = SortType.Asc,
    [FromQuery] CameraAttributes sortAttribute = CameraAttributes.Name,
    [FromQuery] int page = 1,
    [FromQuery] int limit = 10
  )
  {
    var result = await camera.GetCameras(search, name, ip, port, channel, subtype, username, substationId, sortType, sortAttribute, page, limit);
    return Ok(result);
  }

  [HttpGet("{cameraId}")]
  public async Task<ActionResult<CameraResponse>> GetCamera([FromRoute] Guid cameraId)
  {
    var result = await camera.GetCamera(cameraId);
    return Ok(result);
  }

  [HttpGet("count")]
  public async Task<ActionResult<CountResponse>> CountCameras(
    [FromQuery] string? search,
    [FromQuery] string? name,
    [FromQuery] string? ip,
    [FromQuery] int? port,
    [FromQuery] int? channel,
    [FromQuery] int? subtype,
    [FromQuery] string? username,
    [FromQuery] Guid? substationId
  )
  {
    var result = await camera.CountCameras(search, name, ip, port, channel, subtype, username, substationId);
    return Ok(result);
  }

  [HttpPost]
  public async Task<ActionResult<CameraResponse>> CreateCamera(CameraRequest request)
  {
    var result = await camera.CreateCamera(request);
    return Created("", result);
  }

  [HttpPut("{cameraId}")]
  public async Task<ActionResult<CameraResponse>> UpdateCamera([FromRoute] Guid cameraId, [FromBody] CameraRequest request)
  {
    var result = await camera.UpdateCamera(cameraId, request);
    return Accepted(result);
  }

  [HttpDelete("{cameraId}")]
  public async Task<ActionResult<CameraResponse>> DeleteCamera([FromRoute] Guid cameraId)
  {
    var result = await camera.DeleteCamera(cameraId);
    return Accepted(result);
  }
}