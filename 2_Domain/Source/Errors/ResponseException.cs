namespace Domain.Errors
{
  public class ResponseException
  {
    public int StatusCode { get; set; }
    public required string Message { get; set; } = "Error";
  }

}