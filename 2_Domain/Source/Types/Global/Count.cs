using System.Net;

namespace Domain.Types
{
  public record CountResponse(HttpStatusCode StatusCode, string Message, int Data);
  public record CountAmountResponse(HttpStatusCode StatusCode, string Message, double? Data);
}