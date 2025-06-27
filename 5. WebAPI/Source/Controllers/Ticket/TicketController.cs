using Infrastructure.Interfaces;
using Domain.Types;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using Domain.Models;

namespace WebAPI.Controllers;

[ApiController]
[Route("api/ticket")]
[Authorize]
public class TicketController(ITicket ticket) : ControllerBase
{
  [HttpGet]
  public async Task<ActionResult<TicketsResponse>> GetTickets(
    [FromQuery] string? search,
    [FromQuery] string? ticketNumber,
    [FromQuery] bool? isOpen,
    [FromQuery] TicketStatus? ticketStatus,
    [FromQuery] Guid? operatorId,
    [FromQuery] Guid? detectionId,
    [FromQuery] SortType sortType = SortType.Desc,
    [FromQuery] TicketAttributes sortAttribute = TicketAttributes.CreatedAt,
    [FromQuery] int page = 1,
    [FromQuery] int limit = 10
  )
  {
    var result = await ticket.GetTickets(
      search,
      ticketNumber,
      isOpen,
      ticketStatus,
      operatorId,
      detectionId,
      sortType,
      sortAttribute,
      page,
      limit
    );
    return Ok(result);
  }

  [HttpGet("{ticketId}")]
  public async Task<ActionResult<TicketResponse>> GetTicket([FromRoute] Guid ticketId)
  {
    var result = await ticket.GetTicket(ticketId);
    return Ok(result);
  }

  [HttpGet("count")]
  public async Task<ActionResult<CountResponse>> CountTickets(
    [FromQuery] string? search,
    [FromQuery] string? ticketNumber,
    [FromQuery] bool? isOpen,
    [FromQuery] TicketStatus? ticketStatus,
    [FromQuery] Guid? operatorId,
    [FromQuery] Guid? detectionId
  )
  {
    var result = await ticket.CountTickets(
      search,
      ticketNumber,
      isOpen,
      ticketStatus,
      operatorId,
      detectionId
    );
    return Ok(result);
  }

  [HttpPost]
  public async Task<ActionResult<TicketResponse>> CreateTicket(TicketRequest request)
  {
    var result = await ticket.CreateTicket(request);
    return Created("", result);
  }

  [HttpPut("{ticketId}/toogle")]
  public async Task<ActionResult<TicketResponse>> ToogleOpenTicket([FromRoute] Guid ticketId)
  {
    var result = await ticket.ToogleOpenTicket(ticketId);
    return Accepted(result);
  }

  [HttpPut("{ticketId}/report")]
  public async Task<ActionResult<TicketResponse>> GenerateTicketReport([FromRoute] Guid ticketId)
  {
    var result = await ticket.GenerateTicketReport(ticketId);
    return Accepted(result);
  }

  [HttpPut("{ticketId}/status")]
  public async Task<ActionResult<TicketResponse>> ChangeTicketStatus([FromRoute] Guid ticketId, [FromBody] TicketStatusRequest request)
  {
    var result = await ticket.ChangeTicketStatus(ticketId, request);
    return Accepted(result);
  }

  [HttpDelete("{ticketId}")]
  public async Task<ActionResult<TicketResponse>> DeleteTicket([FromRoute] Guid ticketId)
  {
    var result = await ticket.DeleteTicket(ticketId);
    return Accepted(result);
  }
}