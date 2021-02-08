using System;
using System.Globalization;
using System.Reflection;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;

namespace BeatMapInfo
{
    
    public class IntEnumConverter : JsonConverter
    {
        /// <summary>
        /// Writes the JSON representation of the object.
        /// </summary>
        /// <param name="writer">The <see cref="JsonWriter"/> to write to.</param>
        /// <param name="value">The value.</param>
        /// <param name="serializer">The calling serializer.</param>
        public override void WriteJson(JsonWriter writer, object? value, JsonSerializer serializer)
        {
            if (value == null)
            {
                writer.WriteNull();
                return;
            }

            Enum e = (Enum)value;
            writer.WriteValue((int)value);
        }

        /// <summary>
        /// Reads the JSON representation of the object.
        /// </summary>
        /// <param name="reader">The <see cref="JsonReader"/> to read from.</param>
        /// <param name="objectType">Type of the object.</param>
        /// <param name="existingValue">The existing value of object being read.</param>
        /// <param name="serializer">The calling serializer.</param>
        /// <returns>The object value.</returns>
        public override object? ReadJson(JsonReader reader, Type objectType, object? existingValue, JsonSerializer serializer)
        {
            if (reader.TokenType == JsonToken.Null)
                return null;
            

            bool isNullable = ReflectionUtils.IsNullableType(objectType);
            Type t = isNullable ? Nullable.GetUnderlyingType(objectType) : objectType;

            try
            {
                if (reader.TokenType == JsonToken.Integer)
                {
                    return Enum.Parse(t, reader.Value?.ToString());
                }
            }
            catch (Exception ex)
            {
                throw new JsonSerializationException($"Error converting value {reader.Value} to type '{objectType}'.", ex);
            }
            
            throw new JsonSerializationException($"Error converting value {reader.Value} to type '{objectType}'.");
        }

        /// <summary>
        /// Determines whether this instance can convert the specified object type.
        /// </summary>
        /// <param name="objectType">Type of the object.</param>
        /// <returns>
        /// <c>true</c> if this instance can convert the specified object type; otherwise, <c>false</c>.
        /// </returns>
        public override bool CanConvert(Type objectType)
        {
            Type t = ReflectionUtils.IsNullableType(objectType)
                ? Nullable.GetUnderlyingType(objectType)
                : objectType;

            return t.GetTypeInfo().IsEnum;
        }
    }

    public static class ReflectionUtils
    {

        public static bool IsGenericType(this Type type)
        {
            return type.GetTypeInfo().IsGenericType;
        }
        
        public static bool IsNullableType(Type t)
        {
            return (t.IsGenericType() && t.GetGenericTypeDefinition() == typeof(Nullable<>));
        }
    }
}