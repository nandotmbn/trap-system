using Infrastructure.Interfaces;
using Domain.Types;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;

namespace WebAPI.Controllers;

[ApiController]
[Route("api/classification")]
[Authorize]
public class ClassificationController(IClassification classification) : ControllerBase
{
  [HttpGet]
  public async Task<ActionResult<ClassificationsResponse>> GetClassifications(
  [FromQuery] string? search,
  [FromQuery] Guid? detectionId,
  [FromQuery] Guid? cameraId,
  [FromQuery] Guid? substationId,
  [FromQuery] SortType sortType = SortType.Desc,
  [FromQuery] ClassificationAttributes sortAttribute = ClassificationAttributes.CreatedAt,
  [FromQuery] int page = 1,
  [FromQuery] int limit = 10
)
  {
    var result = await classification.GetClassifications(search, detectionId, cameraId, substationId, sortType, sortAttribute, page, limit);
    return Ok(result);
  }

  [HttpGet("{classificationId}")]
  public async Task<ActionResult<ClassificationResponse>> GetClassification([FromRoute] Guid classificationId)
  {
    var result = await classification.GetClassification(classificationId);
    return Ok(result);
  }

  [HttpGet("count")]
  public async Task<ActionResult<CountResponse>> CountClassifications(
    [FromQuery] string? search,
    [FromQuery] Guid? detectionId,
    [FromQuery] Guid? cameraId,
    [FromQuery] Guid? substationId
  )
  {
    var result = await classification.CountClassifications(search, detectionId, cameraId, substationId);
    return Ok(result);
  }

  [HttpGet("distinct")]
  public async Task<ActionResult<StringsResponse>> DistinctPrediction()
  {
    var result = await classification.DistinctPrediction();
    return Ok(result);
  }
}