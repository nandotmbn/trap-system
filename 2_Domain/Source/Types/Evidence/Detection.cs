using System.Net;
using Domain.Models;

namespace Domain.Types
{
  public record DetectionResponse(HttpStatusCode StatusCode, string Message, Detection Data);
  public record DetectionsResponse(HttpStatusCode StatusCode, string Message, List<Detection> Data);
}