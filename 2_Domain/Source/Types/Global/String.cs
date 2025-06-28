using System.Net;

namespace Domain.Types
{
  public record StringResponse(HttpStatusCode StatusCode, string Message, string Data);
  public record StringsResponse(HttpStatusCode StatusCode, string Message, List<string> Data);
}