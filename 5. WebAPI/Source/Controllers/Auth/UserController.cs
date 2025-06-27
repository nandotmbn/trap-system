using Infrastructure.Interfaces;
using Domain.Types;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using Domain.Models;

namespace WebAPI.Controllers;

[ApiController]
[Route("api/user")]
[Authorize]
public class UserController(IUser user) : ControllerBase
{
  [HttpGet]
  public async Task<ActionResult<UsersResponse>> GetUsers(
    [FromQuery] string? search,
    [FromQuery] string? name,
    [FromQuery] string? username,
    [FromQuery] string? phoneNumber,
    [FromQuery] UserType? type,
    [FromQuery] SortType sortType = SortType.Asc,
    [FromQuery] UserAttributes sortAttribute = UserAttributes.FirstName,
    [FromQuery] int page = 1,
    [FromQuery] int limit = 10
  )
  {
    var result = await user.GetUsers(search, name, username, phoneNumber, type, sortType, sortAttribute, page, limit);
    return Ok(result);
  }

  [HttpGet("{userId}")]
  public async Task<ActionResult<UserResponse>> GetUser([FromRoute] Guid userId)
  {
    var result = await user.GetUser(userId);
    return Ok(result);
  }

  [HttpGet("count")]
  public async Task<ActionResult<CountResponse>> CountUsers(
    [FromQuery] string? search,
    [FromQuery] string? name,
    [FromQuery] string? username,
    [FromQuery] string? phoneNumber,
    [FromQuery] UserType? type
  )
  {
    var result = await user.CountUsers(search, name, username, phoneNumber, type);
    return Ok(result);
  }

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