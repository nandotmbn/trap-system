
using System.Text.Json.Serialization;
using Domain.Models;
using Domain.Types;

namespace Infrastructure.Interfaces;

[JsonConverter(typeof(JsonStringEnumConverter<CameraAttributes>))]
public enum CameraAttributes
{
  Name,
  IP,
  Port,
  Channel,
  Subtype,
  Username,
  Substation,
  CreatedAt,
  UpdatedAt
};

public interface ICamera
{
  IQueryable<Camera> Query(string? search, string? name, string? ip, int? port, int? channel, int? subtype, string? username, Guid? substationId);

  Task<CameraResponse> GetCamera(Guid cameraId);
  Task<CamerasResponse> GetCameras(string? search, string? name, string? ip, int? port, int? channel, int? subtype, string? username, Guid? substationId, SortType sortType = SortType.Asc, CameraAttributes sortAttribute = CameraAttributes.Name, int page = 1, int limit = 10);
  Task<CountResponse> CountCameras(string? search, string? name, string? ip, int? port, int? channel, int? subtype, string? username, Guid? substationId);

  Task<Camera> Create(CameraRequest request);
  Task<Camera> Update(Guid cameraId, CameraRequest request);
  Task<Camera> Delete(Guid cameraId);
  Task<CameraResponse> CreateCamera(CameraRequest request);
  Task<CameraResponse> UpdateCamera(Guid cameraId, CameraRequest request);
  Task<CameraResponse> DeleteCamera(Guid cameraId);
}
