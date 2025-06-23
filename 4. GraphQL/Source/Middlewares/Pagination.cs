using HotChocolate.Types.Descriptors;
using System.Reflection;

namespace GraphQL.Middlewares;

[AttributeUsage(AttributeTargets.Method | AttributeTargets.Property)]
public class PaginationAttribute : ObjectFieldDescriptorAttribute
{
    public int DefaultLimit { get; set; } = 10;
    public int MaxLimit { get; set; } = 100;

    protected override void OnConfigure(IDescriptorContext context, IObjectFieldDescriptor descriptor, MemberInfo member)
    {
        descriptor.Argument("page", a => a.Type<IntType>().DefaultValue(1).Description("Page number (starts from 1)"));
        descriptor.Argument("limit", a => a.Type<IntType>().DefaultValue(DefaultLimit).Description($"Number of items per page (max: {MaxLimit})"));
        
        // Store pagination configuration in context data
        descriptor.Extend().OnBeforeCreate(definition =>
        {
            definition.ContextData["pagination.enabled"] = true;
            definition.ContextData["pagination.defaultLimit"] = DefaultLimit;
            definition.ContextData["pagination.maxLimit"] = MaxLimit;
        });
        
        // DON'T add middleware here - it will run too early
        // We'll configure it globally to run after all other middleware
    }
}