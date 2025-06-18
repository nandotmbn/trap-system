using Infrastructure.Interfaces;
using Domain.Types;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;

namespace WebAPI.Controllers
{
  [ApiController]
  [Route("api/camera")]
  [Authorize]
  public class CameraController(ICamera camera) : ControllerBase
  {
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
}