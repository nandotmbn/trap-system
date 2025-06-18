
using Domain.Models;
using Domain.Types;

namespace Infrastructure.Interfaces
{
  public interface ICamera
  {
    Task<Camera> Create(CameraRequest request);
    Task<Camera> Update(Guid cameraId, CameraRequest request);
    Task<Camera> Delete(Guid cameraId);
    Task<CameraResponse> CreateCamera(CameraRequest request);
    Task<CameraResponse> UpdateCamera(Guid cameraId, CameraRequest request);
    Task<CameraResponse> DeleteCamera(Guid cameraId);
  }
}