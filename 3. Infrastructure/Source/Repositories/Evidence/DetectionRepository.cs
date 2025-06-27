
using System.Net;
using Domain.Errors;
using Domain.Models;
using Domain.Types;
using Infrastructure.Database;
using Infrastructure.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Repositories;

public class DetectionRepository(AppDBContext appDBContext) : IDetection
{
  public IQueryable<Detection> Query(string? search, Guid? cameraId, Guid? substationId, DateTime[]? range)
  {
    var query = appDBContext.Detections.AsQueryable();
    if (search != null)
    {
      query = query.Where(x => EF.Functions.Like(
        x!.Camera!.Substation!.Name.ToLower() + " " + 
        x.Camera.Name.ToLower(),
        $"%{search.ToLower()}%"));
    }
    if (cameraId != null)
    {
      query = query.Where(x => x.CameraId == cameraId);
    }
    if (substationId != null)
    {
      query = query.Where(x => x.Camera!.SubstationId == substationId);
    }
    if (range != null && range.Length == 2)
    {
      query = query.Where(x => x.CreatedAt >= range[0] && x.CreatedAt <= range[1]);
    }

    return query;
  }

  async public Task<DetectionResponse> GetDetection(Guid classificationId)
  {
    var user = await appDBContext.Detections
      .Where(e => e.Id == classificationId)
      .Where(e => !e.IsArchived)
      .FirstOrDefaultAsync() ?? throw new NotFoundException("Deteksi tidak ditemukan");

    return new DetectionResponse(HttpStatusCode.OK, "Deteksi berhasil didapatkan", user);
  }
  async public Task<DetectionsResponse> GetDetections(string? search, Guid? cameraId, Guid? substationId, DateTime[]? range, SortType sortType = SortType.Asc, DetectionAttributes sortAttribute = DetectionAttributes.CreatedAt, int page = 1, int limit = 10)
  {
    int itemsToSkip = (page - 1) * limit;
    var query = Query(search, cameraId, substationId, range); ;

    query = (sortType, sortAttribute) switch
    {
      (SortType.Asc, DetectionAttributes.Camera) => query.OrderBy(q => q.Camera!.Name),
      (SortType.Asc, DetectionAttributes.Substation) => query.OrderBy(q => q.Camera!.Substation!.Name),
      (SortType.Asc, DetectionAttributes.CreatedAt) => query.OrderBy(q => q.CreatedAt),
      (SortType.Asc, DetectionAttributes.UpdatedAt) => query.OrderBy(q => q.UpdatedAt),
      (SortType.Desc, DetectionAttributes.Camera) => query.OrderByDescending(q => q.Camera!.Name),
      (SortType.Desc, DetectionAttributes.Substation) => query.OrderByDescending(q => q.Camera!.Substation!.Name),
      (SortType.Desc, DetectionAttributes.CreatedAt) => query.OrderByDescending(q => q.CreatedAt),
      (SortType.Desc, DetectionAttributes.UpdatedAt) => query.OrderByDescending(q => q.UpdatedAt),
      _ => sortType == SortType.Asc ? query.OrderBy(q => q.CreatedAt) : query.OrderByDescending(q => q.CreatedAt)
    };

    query = query.Where(e => !e.IsArchived).Skip(itemsToSkip!).Take(limit!);
    var entities = await query.ToListAsync();
    return new DetectionsResponse(HttpStatusCode.OK, "Deteksi berhasil didapatkan", entities);
  }
  async public Task<CountResponse> CountDetections(string? search, Guid? cameraId, Guid? substationId, DateTime[]? range)
  {
    var query = Query(search, cameraId, substationId, range); ;
    var count = await query.Where(e => !e.IsArchived).CountAsync();
    return new CountResponse(HttpStatusCode.OK, "Total berhasil didapatkan", count);
  }
}