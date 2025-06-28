using System.Net;
using Domain.Models;

namespace Domain.Types
{
	public record GetContentDeliveriesResponse(HttpStatusCode StatusCode, string Message, List<ContentDelivery>? Data);
	public record GetContentDeliveryByIdResponse(HttpStatusCode StatusCode, string Message, ContentDelivery? Data);
}