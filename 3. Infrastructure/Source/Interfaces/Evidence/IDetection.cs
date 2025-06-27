
using System.Text.Json.Serialization;
using Domain.Models;
using Domain.Types;

namespace Infrastructure.Interfaces;

[JsonConverter(typeof(JsonStringEnumConverter<DetectionAttributes>))]
public enum DetectionAttributes
{
  Camera,
  Substation,
  CreatedAt,
  UpdatedAt
};

public interface IDetection
{
  IQueryable<Detection> Query(string? search, Guid? cameraId, Guid? substationId, DateTime[]? range);
  Task<DetectionResponse> GetDetection(Guid detectionId);
  Task<DetectionsResponse> GetDetections(string? search, Guid? cameraId, Guid? substationId, DateTime[]? range, SortType sortType = SortType.Asc, DetectionAttributes sortAttribute = DetectionAttributes.CreatedAt, int page = 1, int limit = 10);
  Task<CountResponse> CountDetections(string? search, Guid? cameraId, Guid? substationId, DateTime[]? range);
}