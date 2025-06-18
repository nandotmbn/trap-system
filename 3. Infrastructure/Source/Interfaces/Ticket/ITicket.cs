
using Domain.Models;
using Domain.Types;

namespace Infrastructure.Interfaces
{
  public interface ITicket
  {
    Task<Ticket> Create(TicketRequest request);
    Task<Ticket> Delete(Guid ticketId);
    Task<TicketResponse> CreateTicket(TicketRequest request);
    Task<TicketResponse> ToogleOpenTicket(Guid ticketId);
    Task<TicketResponse> ChangeTicketStatus(Guid ticketId, TicketStatusRequest request);
    Task<TicketResponse> GenerateTicketReport(Guid ticketId);
    Task<TicketResponse> DeleteTicket(Guid ticketId);
  }
}