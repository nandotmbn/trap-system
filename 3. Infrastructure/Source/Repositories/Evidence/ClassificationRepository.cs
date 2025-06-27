
using System.Net;
using Domain.Errors;
using Domain.Models;
using Domain.Types;
using Infrastructure.Database;
using Infrastructure.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Repositories;

public class ClassificationRepository(AppDBContext appDBContext) : IClassification
{
  public IQueryable<Classification> Query(string? search, Guid? detectionId, Guid? cameraId, Guid? substationId)
  {
    var query = appDBContext.Classifications.AsQueryable();
    if (search != null)
    {
      query = query.Where(x => EF.Functions.Like(
        x.Detection!.Camera!.Substation!.Name.ToLower() + " " + 
        x.Detection.Camera.Name.ToLower() + " " + 
        x.Prediction.ToLower() + " ",
        $"%{search.ToLower()}%"));
    }
    if (detectionId != null)
    {
      query = query.Where(x => x.DetectionId == detectionId);
    }
    if (cameraId != null)
    {
      query = query.Where(x => x.Detection!.CameraId == cameraId);
    }
    if (substationId != null)
    {
      query = query.Where(x => x.Detection!.Camera!.SubstationId == substationId);
    }

    return query;
  }

  async public Task<ClassificationResponse> GetClassification(Guid classificationId)
  {
    var user = await appDBContext.Classifications
      .Where(e => e.Id == classificationId)
      .Where(e => !e.IsArchived)
      .FirstOrDefaultAsync() ?? throw new NotFoundException("Klasifikasi tidak ditemukan");

    return new ClassificationResponse(HttpStatusCode.OK, "Klasifikasi berhasil didapatkan", user);
  }
  async public Task<ClassificationsResponse> GetClassifications(string? search, Guid? detectionId, Guid? cameraId, Guid? substationId, SortType sortType = SortType.Asc, ClassificationAttributes sortAttribute = ClassificationAttributes.CreatedAt, int page = 1, int limit = 10)
  {
    int itemsToSkip = (page - 1) * limit;
    var query = Query(search, detectionId, cameraId, substationId); ;

    query = (sortType, sortAttribute) switch
    {
      (SortType.Asc, ClassificationAttributes.Camera) => query.OrderBy(q => q.Detection!.Camera!.Name),
      (SortType.Asc, ClassificationAttributes.Substation) => query.OrderBy(q => q.Detection!.Camera!.Substation!.Name),
      (SortType.Asc, ClassificationAttributes.Prediction) => query.OrderBy(q => q.Prediction),
      (SortType.Asc, ClassificationAttributes.Confidence) => query.OrderBy(q => q.Confidence),
      (SortType.Asc, ClassificationAttributes.CreatedAt) => query.OrderBy(q => q.CreatedAt),
      (SortType.Asc, ClassificationAttributes.UpdatedAt) => query.OrderBy(q => q.UpdatedAt),
      (SortType.Desc, ClassificationAttributes.Camera) => query.OrderByDescending(q => q.Detection!.Camera!.Name),
      (SortType.Desc, ClassificationAttributes.Substation) => query.OrderByDescending(q => q.Detection!.Camera!.Substation!.Name),
      (SortType.Desc, ClassificationAttributes.CreatedAt) => query.OrderByDescending(q => q.CreatedAt),
      (SortType.Desc, ClassificationAttributes.UpdatedAt) => query.OrderByDescending(q => q.UpdatedAt),
      (SortType.Desc, ClassificationAttributes.Prediction) => query.OrderByDescending(q => q.Prediction),
      (SortType.Desc, ClassificationAttributes.Confidence) => query.OrderByDescending(q => q.Confidence),
      _ => sortType == SortType.Asc ? query.OrderBy(q => q.CreatedAt) : query.OrderByDescending(q => q.CreatedAt)
    };

    query = query.Where(e => !e.IsArchived).Skip(itemsToSkip!).Take(limit!);
    var entities = await query.ToListAsync();
    return new ClassificationsResponse(HttpStatusCode.OK, "Klasifikasi berhasil didapatkan", entities);
  }
  async public Task<CountResponse> CountClassifications(string? search, Guid? detectionId, Guid? cameraId, Guid? substationId)
  {
    var query = Query(search, detectionId, cameraId, substationId); ;
    var count = await query.Where(e => !e.IsArchived).CountAsync();
    return new CountResponse(HttpStatusCode.OK, "Total berhasil didapatkan", count);
  }

  async public Task<StringsResponse> DistinctPrediction()
  {
    return new StringsResponse(HttpStatusCode.OK, "Prediksi berhasil didapatkan", await appDBContext.Classifications.Select(x => x.Prediction).Distinct().ToListAsync());
  }
}