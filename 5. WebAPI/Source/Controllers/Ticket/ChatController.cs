using Infrastructure.Interfaces;
using Domain.Types;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;

namespace WebAPI.Controllers;

[ApiController]
[Route("api/chat")]
[Authorize]
public class ChatController(IChat chat) : ControllerBase
{
  [HttpPost]
  public async Task<ActionResult<ChatResponse>> CreateChat(ChatRequest request)
  {
    var result = await chat.CreateChat(request);
    return Created("", result);
  }

  [HttpPut("{chatId}")]
  public async Task<ActionResult<ChatResponse>> UpdateChat([FromRoute] Guid chatId, [FromBody] ChatRequest request)
  {
    var result = await chat.UpdateChat(chatId, request);
    return Accepted(result);
  }

  [HttpDelete("{chatId}")]
  public async Task<ActionResult<ChatResponse>> DeleteChat([FromRoute] Guid chatId)
  {
    var result = await chat.DeleteChat(chatId);
    return Accepted(result);
  }
}