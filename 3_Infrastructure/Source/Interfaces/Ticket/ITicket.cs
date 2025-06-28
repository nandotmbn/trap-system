
using System.Text.Json.Serialization;
using Domain.Models;
using Domain.Types;

namespace Infrastructure.Interfaces;

[JsonConverter(typeof(JsonStringEnumConverter<TicketAttributes>))]
public enum TicketAttributes
{
  IsOpen,
  TicketNumber,
  Status,
  Operator,
  CreatedAt,
  UpdatedAt
};

public interface ITicket
{
  IQueryable<Ticket> Query(string? search, string? ticketNumber, bool? isOpen, TicketStatus? ticketStatus, Guid? operatorId, Guid? detectionId);
  Task<TicketResponse> GetTicket(Guid ticketId);
  Task<TicketsResponse> GetTickets(string? search, string? ticketNumber, bool? isOpen, TicketStatus? ticketStatus, Guid? operatorId, Guid? detectionId, SortType sortType = SortType.Asc, TicketAttributes sortAttribute = TicketAttributes.CreatedAt, int page = 1, int limit = 10);
  Task<CountResponse> CountTickets(string? search, string? ticketNumber, bool? isOpen, TicketStatus? ticketStatus, Guid? operatorId, Guid? detectionId);

  Task<Ticket> Create(TicketRequest request);
  Task<Ticket> Delete(Guid ticketId);
  Task<TicketResponse> CreateTicket(TicketRequest request);
  Task<TicketResponse> ToogleOpenTicket(Guid ticketId);
  Task<TicketResponse> ChangeTicketStatus(Guid ticketId, TicketStatusRequest request);
  Task<TicketResponse> GenerateTicketReport(Guid ticketId);
  Task<TicketResponse> DeleteTicket(Guid ticketId);
}