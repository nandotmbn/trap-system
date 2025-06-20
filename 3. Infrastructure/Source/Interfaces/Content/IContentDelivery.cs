using Domain.Types;
using Microsoft.AspNetCore.Http;

namespace Infrastructure.Interfaces
{
  public interface IContentDelivery
  {
    Task<GetContentDeliveriesResponse> GetContentDelivery(string? title, bool? isArchived, int? limit = 10, int? page = 1);
    Task<GetContentDeliveryByIdResponse> GetContentDeliveryById(Guid? cdnId);
    Task<GetContentDeliveryByIdResponse> CreateContentDelivery(IFormFile formFile);
    Task<GetContentDeliveryByIdResponse> UpdateContentDelivery(Guid? cdnId, IFormFile formFile);
    Task<GetContentDeliveryByIdResponse> ArchiveContentDelivery(Guid? cdnId);
    Task<GetContentDeliveryByIdResponse> DeleteContentDelivery(Guid? cdnId);
  }
}