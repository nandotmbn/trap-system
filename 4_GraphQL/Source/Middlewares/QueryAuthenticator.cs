using System.Reflection;
using HotChocolate.Resolvers;
using HotChocolate.Types.Descriptors;

namespace GraphQL.Middlewares;

[AttributeUsage(AttributeTargets.Method | AttributeTargets.Class)]
public class QueryAuthorizeAttribute : ObjectFieldDescriptorAttribute
{
	protected override void OnConfigure(IDescriptorContext context, IObjectFieldDescriptor descriptor, MemberInfo member)
	{
		descriptor.Use<QueryAuthorizeMiddleware>();
	}
}

public class QueryAuthorizeMiddleware
{
	private readonly FieldDelegate _next;

	public QueryAuthorizeMiddleware(FieldDelegate next)
	{
		_next = next;
	}

	public async Task InvokeAsync(IMiddlewareContext context)
	{
		var httpContextAccessor = context.Service<IHttpContextAccessor>();
		var httpContext = httpContextAccessor?.HttpContext;

		// 🔐 Custom authorization logic
		if (httpContext == null || !httpContext.User.Identity!.IsAuthenticated)
		{
			throw new GraphQLException(ErrorBuilder.New()
					.SetMessage("You are not authorized.")
					.SetCode("UNAUTHORIZED")
					.SetExtension("statusCode", 401)
					.Build());
		}

		await _next(context);
	}
}
