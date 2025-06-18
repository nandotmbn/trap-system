namespace Domain.Errors
{
  public class AcceptedException : Exception
  {
    public AcceptedException() : base() { }

    public AcceptedException(string message) : base(message) { }

    public AcceptedException(string message, Exception innerException) : base(message, innerException) { }
  }
}
