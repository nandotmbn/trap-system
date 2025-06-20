using Infrastructure.Interfaces;
using Domain.Types;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;

namespace RESTAPI.Controllers
{
  [ApiController]
  [Authorize]
  [Route("api/cdn")]
  public class ContentDeliveryController(IContentDelivery _cdn) : ControllerBase
  {
    private readonly IContentDelivery cdn = _cdn;

    [HttpGet]
    public async Task<ActionResult<GetContentDeliveriesResponse>> Login([FromQuery] string? title,  [FromQuery] bool isArchived, [FromQuery] int limit = 10, [FromQuery] int page = 1)
    {
      var result = await cdn.GetContentDelivery(title, isArchived, limit, page);
      return Ok(result);
    }

    [HttpGet("cdnId/{cdnId}")]
    public async Task<ActionResult<GetContentDeliveryByIdResponse>> Activate(Guid cdnId)
    {
      var result = await cdn.GetContentDeliveryById(cdnId);
      return Ok(result);
    }

    [HttpPost("")]
    public async Task<ActionResult<GetContentDeliveryByIdResponse>> ResendActivation(IFormFile formFile)
    {
      var result = await cdn.CreateContentDelivery(formFile);
      return Ok(result);
    }

    [HttpPut("cdnId/{cdnId}")]
    public async Task<ActionResult<GetContentDeliveryByIdResponse>> UpdateUser([FromRoute] Guid cdnId, IFormFile formFile)
    {
      var result = await cdn.UpdateContentDelivery(cdnId, formFile);
      return result;
    }

    [HttpDelete("cdnId/{cdnId}/archive")]
    public async Task<ActionResult<GetContentDeliveryByIdResponse>> ArchiveUserById([FromRoute] Guid cdnId)
    {
      var result = await cdn.ArchiveContentDelivery(cdnId);
      return result;
    }

    [HttpDelete("cdnId/{cdnId}/delete")]
    public async Task<ActionResult<GetContentDeliveryByIdResponse>> DeleteUserById([FromRoute] Guid cdnId)
    {
      var result = await cdn.DeleteContentDelivery(cdnId);
      return result;
    }

  }
}