using Infrastructure.Interfaces;
using Domain.Types;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using System.ComponentModel.DataAnnotations;

namespace WebAPI.Controllers;

[ApiController]
[Route("api/detection")]
[Authorize]
public class DetectionController(IDetection detection) : ControllerBase
{
  [HttpGet]
  public async Task<ActionResult<DetectionsResponse>> GetDetections(
  [FromQuery] string? search,
  [FromQuery] Guid? cameraId,
  [FromQuery] DateTime[]? range,
  [FromQuery] Guid? substationId,
  [FromQuery] SortType sortType = SortType.Desc,
  [FromQuery] DetectionAttributes sortAttribute = DetectionAttributes.CreatedAt,
  [FromQuery] int page = 1,
  [FromQuery] int limit = 10
)
  {
    var result = await detection.GetDetections(search, cameraId, substationId, range, sortType, sortAttribute, page, limit);
    return Ok(result);
  }

  [HttpGet("{detectionId}")]
  public async Task<ActionResult<DetectionResponse>> GetDetection([FromRoute] Guid detectionId)
  {
    var result = await detection.GetDetection(detectionId);
    return Ok(result);
  }

  [HttpGet("count")]
  public async Task<ActionResult<CountResponse>> CountDetections(
    [FromQuery] string? search,
    [FromQuery] Guid? cameraId,
    [FromQuery] DateTime[]? range,
    [FromQuery] Guid? substationId
  )
  {
    var result = await detection.CountDetections(search, cameraId, substationId, range);
    return Ok(result);
  }
}