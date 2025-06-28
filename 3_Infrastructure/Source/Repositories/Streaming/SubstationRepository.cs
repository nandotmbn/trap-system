
using System.Net;
using Domain.Errors;
using Domain.Models;
using Domain.Types;
using Infrastructure.Database;
using Infrastructure.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Repositories;

public class SubstationRepository(AppDBContext appDBContext) : ISubstation
{
  public IQueryable<Substation> Query(string? search, string? name, string? address)
  {
    var query = appDBContext.Substations.AsQueryable();
    if (search != null)
    {
      query = query.Where(x => EF.Functions.Like(
        x.Name.ToLower() + " " +
        x.Address!.ToLower(),
        $"%{search.ToLower()}%"));
    }
    if (name != null)
    {
      query = query.Where(x => EF.Functions.Like(x.Name.ToLower(), $"%{name.ToLower()}%"));
    }
    if (address != null)
    {
      query = query.Where(x => EF.Functions.Like(x.Address.ToLower(), $"%{address.ToLower()}%"));
    }

    return query;
  }

  async public Task<SubstationResponse> GetSubstation(Guid userId)
  {
    var user = await appDBContext.Substations
      .Where(e => e.Id == userId)
      .Where(e => !e.IsArchived)
      .FirstOrDefaultAsync() ?? throw new NotFoundException("Gardu induk tidak ditemukan");

    return new SubstationResponse(HttpStatusCode.OK, "Gardu induk berhasil didapatkan", user);
  }
  async public Task<SubstationsResponse> GetSubstations(string? search, string? name, string? address, SortType sortType = SortType.Asc, SubstationAttributes sortAttribute = SubstationAttributes.Name, int page = 1, int limit = 10)
  {
    int itemsToSkip = (page - 1) * limit;
    var query = Query(search, name, address); ;

    query = (sortType, sortAttribute) switch
    {
      (SortType.Asc, SubstationAttributes.Name) => query.OrderBy(q => q.Name),
      (SortType.Asc, SubstationAttributes.Address) => query.OrderBy(q => q.Address),
      (SortType.Asc, SubstationAttributes.CreatedAt) => query.OrderBy(q => q.CreatedAt),
      (SortType.Asc, SubstationAttributes.UpdatedAt) => query.OrderBy(q => q.UpdatedAt),
      (SortType.Desc, SubstationAttributes.Name) => query.OrderByDescending(q => q.Name),
      (SortType.Desc, SubstationAttributes.Address) => query.OrderByDescending(q => q.Address),
      (SortType.Desc, SubstationAttributes.CreatedAt) => query.OrderByDescending(q => q.CreatedAt),
      (SortType.Desc, SubstationAttributes.UpdatedAt) => query.OrderByDescending(q => q.UpdatedAt),
      _ => sortType == SortType.Asc ? query.OrderBy(q => q.CreatedAt) : query.OrderByDescending(q => q.CreatedAt)
    };

    query = query.Where(e => !e.IsArchived).Skip(itemsToSkip!).Take(limit!);
    var entities = await query.ToListAsync();
    return new SubstationsResponse(HttpStatusCode.OK, "Gardu induk berhasil didapatkan", entities);
  }
  async public Task<CountResponse> CountSubstations(string? search, string? name, string? address)
  {
    var query = Query(search, name, address); ;
    var count = await query.Where(e => !e.IsArchived).CountAsync();
    return new CountResponse(HttpStatusCode.OK, "Total berhasil didapatkan", count);
  }

  async public Task<Substation> Create(SubstationRequest request)
  {
    var substation = new Substation
    {
      Name = request.Name,
      Address = request.Address,
    };
    await appDBContext.AddAsync(substation);
    await appDBContext.SaveChangesAsync();

    return substation;
  }

  async public Task<SubstationResponse> CreateSubstation(SubstationRequest request)
  {
    using var transaction = appDBContext.Database.BeginTransaction();
    try
    {
      var substation = await Create(request);
      transaction.Commit();
      return new SubstationResponse(HttpStatusCode.Created, "Gardu induk berhasil dibuat", substation);
    }
    catch
    {
      throw;
    }
  }

  async public Task<Substation> Delete(Guid substationId)
  {
    var substation = await appDBContext.Substations
      .Where(e => e.Id == substationId)
      .Where(e => !e.IsArchived)
      .FirstOrDefaultAsync() ?? throw new NotFoundException("Gardu induk tidak ditemukan");

    await appDBContext.Substations.ExecuteUpdateAsync(s => s.SetProperty(p => p.IsArchived, true));

    return substation;
  }

  async public Task<SubstationResponse> DeleteSubstation(Guid substationId)
  {
    using var transaction = appDBContext.Database.BeginTransaction();
    try
    {
      var substation = await Delete(substationId);
      transaction.Commit();

      return new SubstationResponse(HttpStatusCode.Accepted, "Gardu induk berhasil dihapus", substation);
    }
    catch
    {
      throw;
    }
  }

  async public Task<Substation> Update(Guid substationId, SubstationRequest request)
  {
    var substation = await appDBContext.Substations
      .Where(e => e.Id == substationId)
      .Where(e => !e.IsArchived)
      .FirstOrDefaultAsync() ?? throw new NotFoundException("Gardu induk tidak ditemukan");

    substation.Name = request.Name;
    substation.Address = request.Address;

    appDBContext.Update(substation);
    appDBContext.SaveChanges();

    return substation;
  }

  async public Task<SubstationResponse> UpdateSubstation(Guid substationId, SubstationRequest request)
  {
    using var transaction = appDBContext.Database.BeginTransaction();
    try
    {
      var substation = await Update(substationId, request);
      transaction.Commit();

      return new SubstationResponse(HttpStatusCode.Accepted, "Gardu induk berhasil diubah", substation);
    }
    catch
    {
      throw;
    }
  }
}