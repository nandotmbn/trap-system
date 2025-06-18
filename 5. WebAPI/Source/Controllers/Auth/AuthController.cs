using Infrastructure.Interfaces;
using Domain.Types;
using Microsoft.AspNetCore.Mvc;

namespace WebAPI.Controllers
{
  [ApiController]
  [Route("api/auth")]
  public class AuthController(IAuth auth) : ControllerBase
  {

    [HttpPost("register")]
    public async Task<ActionResult<RegistrationResponse>> Register(RegistrationRequest loginRequest)
    {
      var result = await auth.Register(loginRequest);
      return Created("", result);
    }

    [HttpPost("login")]
    public async Task<ActionResult<LoginResponse>> Login(LoginRequest loginRequest)
    {
      var result = await auth.Login(loginRequest);
      return Ok(result);
    }
  }
}