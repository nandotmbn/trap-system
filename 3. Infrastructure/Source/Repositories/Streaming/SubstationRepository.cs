
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