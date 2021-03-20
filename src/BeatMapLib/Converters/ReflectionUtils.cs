using System;
using System.Reflection;

namespace BeatMapInfo
{
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