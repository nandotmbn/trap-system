using System.Net;
using System.Text.Json.Serialization;

namespace Domain.Types
{
	[JsonConverter(typeof(JsonStringEnumConverter<SortType>))]
  public enum SortType
  {
    Asc,
    Desc
  }
  public record BooleanResponse(HttpStatusCode StatusCode, string Message, bool Data);
}