using Infrastructure.Interfaces;
using Domain.Types;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;

namespace WebAPI.Controllers;

[ApiController]
[Route("api/user")]
[Authorize]
public class UserController(IUser user) : ControllerBase
{
  [HttpPut("{userId}/recovery")]
  public async Task<ActionResult<UserResponse>> RecoveryUser([FromRoute] Guid userId, [FromBody] RecoveryRequest request)
  {
    var result = await user.RecoveryUser(userId, request);
    return Accepted("", result);
  }

  [HttpPut("{userId}")]
  public async Task<ActionResult<UserResponse>> UpdateUser([FromRoute] Guid userId, [FromBody] UserRequest request)
  {
    var result = await user.UpdateUser(userId, request);
    return Accepted(result);
  }

  [HttpDelete("{userId}")]
  public async Task<ActionResult<UserResponse>> DeleteUser([FromRoute] Guid userId)
  {
    var result = await user.DeleteUser(userId);
    return Accepted(result);
  }
}