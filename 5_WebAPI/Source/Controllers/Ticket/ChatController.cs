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
  [HttpGet]
  public async Task<ActionResult<ChatsResponse>> GetChats(
  [FromQuery] string? search,
  [FromQuery] string? message,
  [FromQuery] Guid? ticketId,
  [FromQuery] SortType sortType = SortType.Desc,
  [FromQuery] ChatAttributes sortAttribute = ChatAttributes.CreatedAt,
  [FromQuery] int page = 1,
  [FromQuery] int limit = 10
)
  {
    var result = await chat.GetChats(search, message, ticketId, sortType, sortAttribute, page, limit);
    return Ok(result);
  }

  [HttpGet("{chatId}")]
  public async Task<ActionResult<ChatResponse>> GetChat([FromRoute] Guid chatId)
  {
    var result = await chat.GetChat(chatId);
    return Ok(result);
  }

  [HttpGet("count")]
  public async Task<ActionResult<CountResponse>> CountChats(
    [FromQuery] string? search,
    [FromQuery] string? message,
    [FromQuery] Guid? ticketId
  )
  {
    var result = await chat.CountChats(search, message, ticketId);
    return Ok(result);
  }

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