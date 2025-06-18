using System.ComponentModel.DataAnnotations;
using System.Net;
using Domain.Models;

namespace Domain.Types
{

  public class SubstationRequest
  {
    [Required, MaxLength(20)]
		public string Name { get; set; } = string.Empty;
		public string Address { get; set; } = string.Empty;
  }

  public record SubstationResponse(HttpStatusCode StatusCode, string Message, Substation Data);
  public record SubstationsResponse(HttpStatusCode StatusCode, string Message, List<Substation> Data);
}