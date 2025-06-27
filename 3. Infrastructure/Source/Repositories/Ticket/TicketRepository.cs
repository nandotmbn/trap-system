
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
    public IQueryable<Ticket> Query(string? search, string? ticketNumber, bool? isOpen, TicketStatus? ticketStatus, Guid? operatorId, Guid? detectionId)
  {
    var query = appDBContext.Tickets.AsQueryable();
    if (search != null)
    {
      query = query.Where(x => EF.Functions.Like(
        x.TicketNumber.ToLower() + " " +
        x.Operator!.FirstName.ToLower() + " " +
        x.Operator.LastName.ToLower() + " ",
        $"%{search.ToLower()}%"));
    }
    if (ticketNumber != null)
    {
      query = query.Where(x => EF.Functions.Like(x.TicketNumber.ToLower(), $"%{ticketNumber.ToLower()}%"));
    }
    if (isOpen != null)
    {
      query = query.Where(x => x.IsOpen == isOpen);
    }
    if (operatorId != null)
    {
      query = query.Where(x => x.OperatorId == operatorId);
    }
    if (detectionId != null)
    {
      query = query.Where(x => x.DetectionId == detectionId);
    }

    return query;
  }

  async public Task<TicketResponse> GetTicket(Guid chatId)
  {
    var user = await appDBContext.Tickets
      .Where(e => e.Id == chatId)
      .Where(e => !e.IsArchived)
      .FirstOrDefaultAsync() ?? throw new NotFoundException("Tiket tidak ditemukan");

    return new TicketResponse(HttpStatusCode.OK, "Tiket berhasil didapatkan", user);
  }
  async public Task<TicketsResponse> GetTickets(string? search, string? ticketNumber, bool? isOpen, TicketStatus? ticketStatus, Guid? operatorId, Guid? detectionId, SortType sortType = SortType.Asc, TicketAttributes sortAttribute = TicketAttributes.CreatedAt, int page = 1, int limit = 10)
  {
    int itemsToSkip = (page - 1) * limit;
    var query = Query(search, ticketNumber, isOpen, ticketStatus, operatorId, detectionId); ;

    query = (sortType, sortAttribute) switch
    {
      (SortType.Asc, TicketAttributes.TicketNumber) => query.OrderBy(q => q.TicketNumber),
      (SortType.Asc, TicketAttributes.IsOpen) => query.OrderBy(q => q.IsOpen),
      (SortType.Asc, TicketAttributes.Status) => query.OrderBy(q => q.Status),
      (SortType.Asc, TicketAttributes.Operator) => query.OrderBy(q => q.Operator!.FirstName),
      (SortType.Asc, TicketAttributes.CreatedAt) => query.OrderBy(q => q.CreatedAt),
      (SortType.Asc, TicketAttributes.UpdatedAt) => query.OrderBy(q => q.UpdatedAt),
      (SortType.Desc, TicketAttributes.TicketNumber) => query.OrderByDescending(q => q.TicketNumber),
      (SortType.Desc, TicketAttributes.IsOpen) => query.OrderByDescending(q => q.IsOpen),
      (SortType.Desc, TicketAttributes.Status) => query.OrderByDescending(q => q.Status),
      (SortType.Desc, TicketAttributes.Operator) => query.OrderByDescending(q => q.Operator!.FirstName),
      (SortType.Desc, TicketAttributes.CreatedAt) => query.OrderByDescending(q => q.CreatedAt),
      (SortType.Desc, TicketAttributes.UpdatedAt) => query.OrderByDescending(q => q.UpdatedAt),
      _ => sortType == SortType.Asc ? query.OrderBy(q => q.CreatedAt) : query.OrderByDescending(q => q.CreatedAt)
    };

    query = query.Where(e => !e.IsArchived).Skip(itemsToSkip!).Take(limit!);
    var entities = await query.ToListAsync();
    return new TicketsResponse(HttpStatusCode.OK, "Tiket berhasil didapatkan", entities);
  }
  async public Task<CountResponse> CountTickets(string? search, string? ticketNumber, bool? isOpen, TicketStatus? ticketStatus, Guid? operatorId, Guid? detectionId)
  {
    var query = Query(search, ticketNumber, isOpen, ticketStatus, operatorId, detectionId); ;
    var count = await query.Where(e => !e.IsArchived).CountAsync();
    return new CountResponse(HttpStatusCode.OK, "Total berhasil didapatkan", count);
  }

  async public Task<TicketResponse> ChangeTicketStatus(Guid ticketId, TicketStatusRequest request)
  {
    var ticket = await appDBContext.Tickets
      .Where(e => e.Id == ticketId)
      .FirstOrDefaultAsync() ?? throw new NotFoundException("Tiket tidak ditemukan!");

    ticket.Status = request.Status;
    appDBContext.Update(ticket);
    appDBContext.SaveChanges();

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
      TicketNumber = RandomNumeric.Generate(6),
      IsOpen = true,
      OperatorId = userId,
      Status = request.Status
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
    appDBContext.SaveChanges();

    return new TicketResponse(HttpStatusCode.Accepted, "Status tiket berhasil diubah!", ticket);
  }
}