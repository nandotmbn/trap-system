using Domain.Errors;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace WebAPI.Middlewares
{
  public class ExceptionWrapper : IExceptionFilter
  {
    private static void ImplementException(ExceptionContext context, int statusCode)
    {
      var errorResponse = new ResponseException
      {
        StatusCode = statusCode,
        Message = context.Exception.Message,
      };

      context.Result = new ObjectResult(errorResponse)
      {
        StatusCode = statusCode
      };
      context.ExceptionHandled = true;
    }

    public void OnException(ExceptionContext context)
    {
      switch (context.Exception)
      {
        case BadRequestException:
          ImplementException(context, 400);
          break;
        case ForbiddenException:
          ImplementException(context, 403);
          break;
        case GoneException:
          ImplementException(context, 410);
          break;
        case NotFoundException:
          ImplementException(context, 404);
          break;
        case InternalServerErrorException:
          ImplementException(context, 500);
          break;
        case AcceptedException:
          ImplementException(context, 202);
          break;
        case ConflictException:
          ImplementException(context, 409);
          break;
        default:
          break;
      }

    }
  }
}
