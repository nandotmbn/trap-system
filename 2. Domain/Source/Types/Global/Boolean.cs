using System.Net;

namespace Domain.Types
{
  public record BooleanResponse(HttpStatusCode StatusCode, string Message, bool Data);
}