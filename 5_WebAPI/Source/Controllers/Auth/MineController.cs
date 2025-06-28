using Infrastructure.Interfaces;
using Domain.Types;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using Infrastructure.Helpers;

namespace WebAPI.Controllers;

[ApiController]
[Route("api/mine")]
[Authorize]
public class MineController(IMine mine) : ControllerBase
{
  [HttpGet]
  public async Task<ActionResult<UserResponse>> MyProfile(
  )
  {
    var userId = UserClaim.GetId(HttpContext.User);
    var result = await mine.MyProfile(userId);
    return Ok(result);
  }

  [HttpPut]
  public async Task<ActionResult<UserResponse>> ChangeMyProfile(SelfRequest request)
  {
    var result = await mine.ChangeMyProfile(request);
    return Accepted(result);
  }

  [HttpPut("change-password")]
  public async Task<ActionResult<UserResponse>> ChangeMyPassword(ChangePasswordRequest request)
  {
    var result = await mine.ChangeMyPassword(request);
    return Accepted(result);
  }
}