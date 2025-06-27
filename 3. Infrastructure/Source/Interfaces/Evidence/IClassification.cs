
using System.Text.Json.Serialization;
using Domain.Models;
using Domain.Types;

namespace Infrastructure.Interfaces;

[JsonConverter(typeof(JsonStringEnumConverter<ClassificationAttributes>))]
public enum ClassificationAttributes
{
  Confidence,
  Prediction,
  Camera,
  Substation,
  CreatedAt,
  UpdatedAt
};

public interface IClassification
{
  IQueryable<Classification> Query(string? search, Guid? detectionId, Guid? cameraId, Guid? substationId);
  Task<StringsResponse> DistinctPrediction();
  Task<ClassificationResponse> GetClassification(Guid classificationId);
  Task<ClassificationsResponse> GetClassifications(string? search, Guid? detectionId, Guid? cameraId, Guid? substationId, SortType sortType = SortType.Asc, ClassificationAttributes sortAttribute = ClassificationAttributes.CreatedAt, int page = 1, int limit = 10);
  Task<CountResponse> CountClassifications(string? search, Guid? detectionId, Guid? cameraId, Guid? substationId);
}