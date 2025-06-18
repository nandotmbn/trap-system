using Infrastructure.Interfaces;
using Domain.Types;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;

namespace WebAPI.Controllers;

[ApiController]
[Route("api/ticket")]
[Authorize]
public class TicketController(ITicket ticket) : ControllerBase
{
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
  public async Task<ActionResult<TicketResponse>> ChangeTicketStatus([FromRoute] Guid ticketId, [FromRoute] TicketStatusRequest request)
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