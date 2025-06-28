using System.Net;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Reflection;
using Microsoft.AspNetCore.OpenApi;
using Microsoft.OpenApi.Models;

public class EnumConverter : JsonConverter<Enum>
{
    public override bool CanConvert(Type typeToConvert)
    {
        // Only handle enum types excluding HttpStatusCode
        return typeToConvert.IsEnum && typeToConvert != typeof(HttpStatusCode);
    }

    public override Enum Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
    {
        // Convert the string back to an enum
        var stringValue = reader.GetString();
        return (Enum)Enum.Parse(typeToConvert, stringValue!);
    }

    public override void Write(Utf8JsonWriter writer, Enum value, JsonSerializerOptions options)
    {
        // Write the enum as a string
        writer.WriteStringValue(value.ToString());
    }
}

public class JsonStringEnumSchemaTransformer : IOpenApiSchemaTransformer
{
    public Task TransformAsync(OpenApiSchema schema, OpenApiSchemaTransformerContext context, CancellationToken cancellationToken)
    {
        if (context.JsonTypeInfo.Type.IsEnum)
        {
            var jsonStringEnumConverter = context.JsonTypeInfo.Type.GetCustomAttributes<JsonConverterAttribute>(false).FirstOrDefault()?.ConverterType;
            if (jsonStringEnumConverter is not null && jsonStringEnumConverter.IsGenericType && jsonStringEnumConverter.GetGenericTypeDefinition() == typeof(JsonStringEnumConverter<>)) 
            {
                schema.Type = "string";
            }
        }
        return Task.CompletedTask;
    }
}
