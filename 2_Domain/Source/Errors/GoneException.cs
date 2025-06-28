namespace Domain.Errors
{
  public class GoneException : Exception
  {
    public GoneException() : base() { }

    public GoneException(string message) : base(message) { }

    public GoneException(string message, Exception innerException) : base(message, innerException) { }
  }
}
