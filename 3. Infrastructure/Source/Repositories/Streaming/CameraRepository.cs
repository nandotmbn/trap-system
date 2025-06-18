
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

      return new CameraResponse(HttpStatusCode.Accepted, "Kamera CCTV berhasil dihapus", camera);
    }
    catch
    {
      throw;
    }
  }
}