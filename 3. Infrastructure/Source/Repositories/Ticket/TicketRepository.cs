
using System.Net;
using Domain.Errors;
using Domain.Models;
using Domain.Types;
using Infrastructure.Database;
using Infrastructure.Helpers;
using Infrastructure.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Repositories;

public class TicketRepository(AppDBContext appDBContext, IHttpContextAccessor accessor) : ITicket
{
  async public Task<TicketResponse> ChangeTicketStatus(Guid ticketId, TicketStatusRequest request)
  {
    var ticket = await appDBContext.Tickets
      .Where(e => e.Id == ticketId)
      .FirstOrDefaultAsync() ?? throw new NotFoundException("Tiket tidak ditemukan!");

    ticket.Status = request.Status;
    appDBContext.Update(ticket);

    return new TicketResponse(HttpStatusCode.Accepted, "Status tiket berhasil diubah!", ticket);
  }

  async public Task<Ticket> Create(TicketRequest request)
  {
    var userId = UserClaim.GetId(accessor.HttpContext!.User);

    var detection = await appDBContext.Detections
      .Where(e => e.Id == request.DetectionId)
      .FirstOrDefaultAsync() ?? throw new NotFoundException("Detection tidak ditemukan");
      
    var ticket = new Ticket
    {
      DetectionId = request.DetectionId,
      IsOpen = true,
      OperatorId = userId,
      Status = TicketStatus.Pending
    };
    await appDBContext.AddAsync(ticket);
    await appDBContext.SaveChangesAsync();

    return ticket;
  }

  async public Task<TicketResponse> CreateTicket(TicketRequest request)
  {
    using var transaction = appDBContext.Database.BeginTransaction();
    try
    {
      var ticket = await Create(request);
      transaction.Commit();
      return new TicketResponse(HttpStatusCode.Created, "Tiket berhasil dibuat", ticket);
    }
    catch
    {
      throw;
    }
  }

  async public Task<Ticket> Delete(Guid ticketId)
  {
    var ticket = await appDBContext.Tickets
      .Where(e => e.Id == ticketId)
      .Where(e => !e.IsArchived)
      .FirstOrDefaultAsync() ?? throw new NotFoundException("Tiket tidak ditemukan");

    await appDBContext.Tickets.ExecuteUpdateAsync(s => s.SetProperty(p => p.IsArchived, true));

    return ticket;
  }

  async public Task<TicketResponse> DeleteTicket(Guid ticketId)
  {
    using var transaction = appDBContext.Database.BeginTransaction();
    try
    {
      var ticket = await Delete(ticketId);
      transaction.Commit();

      return new TicketResponse(HttpStatusCode.Accepted, "Tiket berhasil dihapus", ticket);
    }
    catch
    {
      throw;
    }
  }

  public Task<TicketResponse> GenerateTicketReport(Guid ticketId)
  {
    throw new NotImplementedException();
  }

  async public Task<TicketResponse> ToogleOpenTicket(Guid ticketId)
  {
    var ticket = await appDBContext.Tickets
      .Where(e => e.Id == ticketId)
      .FirstOrDefaultAsync() ?? throw new NotFoundException("Tiket tidak ditemukan!");

    ticket.IsOpen = !ticket.IsOpen;
    appDBContext.Update(ticket);

    return new TicketResponse(HttpStatusCode.Accepted, "Status tiket berhasil diubah!", ticket);
  }
}