using System.Net;

namespace Domain.Types
{
  public enum SortType
  {
    Asc,
    Desc
  }
  public record BooleanResponse(HttpStatusCode StatusCode, string Message, bool Data);
}