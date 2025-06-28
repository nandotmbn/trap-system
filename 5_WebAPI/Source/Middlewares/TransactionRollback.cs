using Infrastructure.Database;

namespace WebAPI.Middlewares
{
  public class TransactionRollbackMiddleware
  {
    private readonly RequestDelegate _next;

    public TransactionRollbackMiddleware(RequestDelegate next)
    {
      _next = next;
    }

    public async Task Invoke(HttpContext context, AppDBContext dbContext)
    {
      try
      {
        await _next(context);
      }
      catch (Exception)
      {
        dbContext.Database.CurrentTransaction?.Rollback();
        throw; // Rethrow the exception to the caller
      }
    }
  }

}