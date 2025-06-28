
using System.Net;
using Domain.Errors;
using Domain.Models;
using Domain.Types;
using Infrastructure.Database;
using Infrastructure.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Repositories;

public class CameraRepository(AppDBContext appDBContext) : ICamera
{
  public IQueryable<Camera> Query(string? search, string? name, string? ip, int? port, int? channel, int? subtype, string? username, Guid? substationId)
  {
    var query = appDBContext.Cameras.AsQueryable();
    if (search != null)
    {
      query = query.Where(x => EF.Functions.Like(
        x.Name.ToLower() + " " +
        x.IP!.ToLower() + " " +
        x.Substation!.Name.ToLower() + " " +
        x.Substation.Address.ToLower(),
        $"%{search.ToLower()}%"));
    }
    if (name != null)
    {
      query = query.Where(x => EF.Functions.Like(x.Name.ToLower(), $"%{name.ToLower()}%"));
    }
    if (ip != null)
    {
      query = query.Where(x => EF.Functions.Like(x.IP!.ToLower(), $"%{ip.ToLower()}%"));
    }
    if (port != null)
    {
      query = query.Where(x => EF.Functions.Like(x.Port.ToString(), $"%{port}%"));
    }
    if (username != null)
    {
      query = query.Where(x => EF.Functions.Like(x.Username!.ToLower(), $"%{username.ToLower()}%"));
    }
    if (channel != null)
    {
      query = query.Where(x => x.Channel == channel);
    }
    if (subtype != null)
    {
      query = query.Where(x => x.Subtype == subtype);
    }
    if (substationId != null)
    {
      query = query.Where(x => x.SubstationId == substationId);
    }

    return query;
  }

  async public Task<CameraResponse> GetCamera(Guid userId)
  {
    var user = await appDBContext.Cameras
      .Where(e => e.Id == userId)
      .Where(e => !e.IsArchived)
      .FirstOrDefaultAsync() ?? throw new NotFoundException("Kamera CCTV tidak ditemukan");

    return new CameraResponse(HttpStatusCode.OK, "Kamera CCTV berhasil didapatkan", user);
  }
  async public Task<CamerasResponse> GetCameras(string? search, string? name, string? ip, int? port, int? channel, int? subtype, string? username, Guid? substationId, SortType sortType = SortType.Asc, CameraAttributes sortAttribute = CameraAttributes.Name, int page = 1, int limit = 10)
  {
    int itemsToSkip = (page - 1) * limit;
    var query = Query(search, name, ip, port, channel, subtype, username, substationId);

    query = (sortType, sortAttribute) switch
    {
      (SortType.Asc, CameraAttributes.Name) => query.OrderBy(q => q.Name),
      (SortType.Asc, CameraAttributes.IP) => query.OrderBy(q => q.IP),
      (SortType.Asc, CameraAttributes.Channel) => query.OrderBy(q => q.Channel),
      (SortType.Asc, CameraAttributes.Port) => query.OrderBy(q => q.Port),
      (SortType.Asc, CameraAttributes.Substation) => query.OrderBy(q => q.Substation!.Name),
      (SortType.Asc, CameraAttributes.Subtype) => query.OrderBy(q => q.Subtype),
      (SortType.Asc, CameraAttributes.Username) => query.OrderBy(q => q.Username),
      (SortType.Asc, CameraAttributes.CreatedAt) => query.OrderBy(q => q.CreatedAt),
      (SortType.Asc, CameraAttributes.UpdatedAt) => query.OrderBy(q => q.UpdatedAt),
      (SortType.Desc, CameraAttributes.Name) => query.OrderByDescending(q => q.Name),
      (SortType.Desc, CameraAttributes.IP) => query.OrderByDescending(q => q.IP),
      (SortType.Desc, CameraAttributes.Channel) => query.OrderByDescending(q => q.Channel),
      (SortType.Desc, CameraAttributes.Port) => query.OrderByDescending(q => q.Port),
      (SortType.Desc, CameraAttributes.Substation) => query.OrderByDescending(q => q.Substation!.Name),
      (SortType.Desc, CameraAttributes.Subtype) => query.OrderByDescending(q => q.Subtype),
      (SortType.Desc, CameraAttributes.Username) => query.OrderByDescending(q => q.Username),
      (SortType.Desc, CameraAttributes.CreatedAt) => query.OrderByDescending(q => q.CreatedAt),
      (SortType.Desc, CameraAttributes.UpdatedAt) => query.OrderByDescending(q => q.UpdatedAt),
      _ => sortType == SortType.Asc ? query.OrderBy(q => q.CreatedAt) : query.OrderByDescending(q => q.CreatedAt)
    };

    query = query.Where(e => !e.IsArchived).Skip(itemsToSkip!).Take(limit!);
    var entities = await query.ToListAsync();
    return new CamerasResponse(HttpStatusCode.OK, "Kamera CCTV berhasil didapatkan", entities);
  }
  async public Task<CountResponse> CountCameras(string? search, string? name, string? ip, int? port, int? channel, int? subtype, string? username, Guid? substationId)
  {
    var query = Query(search, name, ip, port, channel, subtype, username, substationId); ;
    var count = await query.Where(e => !e.IsArchived).CountAsync();
    return new CountResponse(HttpStatusCode.OK, "Total berhasil didapatkan", count);
  }

  async public Task<Camera> Create(CameraRequest request)
  {
    var camera = new Camera
    {
      Name = request.Name,
      IP = request.IP,
      Channel = request.Channel,
      Username = request.Username,
      Password = request.Password,
      Port = request.Port,
      Subtype = request.Subtype,
      SubstationId = request.SubstationId,
    };
    await appDBContext.AddAsync(camera);
    await appDBContext.SaveChangesAsync();

    return camera;
  }

  async public Task<CameraResponse> CreateCamera(CameraRequest request)
  {
    using var transaction = appDBContext.Database.BeginTransaction();
    try
    {
      var camera = await Create(request);
      transaction.Commit();
      return new CameraResponse(HttpStatusCode.Created, "Kamera CCTV berhasil dibuat", camera);
    }
    catch
    {
      throw;
    }
  }

  async public Task<Camera> Delete(Guid cameraId)
  {
    var camera = await appDBContext.Cameras
      .Where(e => e.Id == cameraId)
      .Where(e => !e.IsArchived)
      .FirstOrDefaultAsync() ?? throw new NotFoundException("Kamera CCTV tidak ditemukan");

    await appDBContext.Cameras.ExecuteUpdateAsync(s => s.SetProperty(p => p.IsArchived, true));

    return camera;
  }

  async public Task<CameraResponse> DeleteCamera(Guid cameraId)
  {
    using var transaction = appDBContext.Database.BeginTransaction();
    try
    {
      var camera = await Delete(cameraId);
      transaction.Commit();

      return new CameraResponse(HttpStatusCode.Accepted, "Kamera CCTV berhasil dihapus", camera);
    }
    catch
    {
      throw;
    }
  }

  async public Task<Camera> Update(Guid cameraId, CameraRequest request)
  {
    var camera = await appDBContext.Cameras
      .Where(e => e.Id == cameraId)
      .Where(e => !e.IsArchived)
      .FirstOrDefaultAsync() ?? throw new NotFoundException("Kamera CCTV tidak ditemukan");

    camera.Name = request.Name;
    camera.IP = request.IP;
    camera.Channel = request.Channel;
    camera.Username = request.Username;
    camera.Password = request.Password;
    camera.Port = request.Port;
    camera.Subtype = request.Subtype;
    camera.SubstationId = request.SubstationId;

    appDBContext.Update(camera);
    appDBContext.SaveChanges();

    return camera;
  }

  async public Task<CameraResponse> UpdateCamera(Guid cameraId, CameraRequest request)
  {
    using var transaction = appDBContext.Database.BeginTransaction();
    try
    {
      var camera = await Update(cameraId, request);
      transaction.Commit();

      return new CameraResponse(HttpStatusCode.Accepted, "Kamera CCTV berhasil diubah", camera);
    }
    catch
    {
      throw;
    }
  }
}