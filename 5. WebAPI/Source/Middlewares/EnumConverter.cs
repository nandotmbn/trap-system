using System;
using System.Net;
using System.Text.Json;
using System.Text.Json.Serialization;

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
