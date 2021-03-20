using System.Globalization;
using BeatMapInfo.Json;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using Newtonsoft.Json.Serialization;

namespace BeatMapInfo
{
    internal static class BeatMapInfoConverter
    {
        public static readonly JsonSerializerSettings Settings = new JsonSerializerSettings
        {
            MetadataPropertyHandling = MetadataPropertyHandling.Ignore,
            DateParseHandling = DateParseHandling.None,
            ContractResolver = new CamelCasePropertyNamesContractResolver()
            {
                NamingStrategy = new BeatSaberNamingStrategy()
            },
            Converters =
            {
                new StringEnumConverter(new DefaultNamingStrategy(), false),
                new IsoDateTimeConverter { DateTimeStyles = DateTimeStyles.AssumeUniversal }
            },
        };
    }
}