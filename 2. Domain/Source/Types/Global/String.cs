using System.Net;

namespace Domain.Types
{
  public record StringResponse(HttpStatusCode StatusCode, string Message, string Data);
}