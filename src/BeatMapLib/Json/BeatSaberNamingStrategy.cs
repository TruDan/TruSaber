using Newtonsoft.Json.Serialization;

namespace BeatMapInfo.Json
{
    public class BeatSaberNamingStrategy : CamelCaseNamingStrategy
    {
        protected override string ResolvePropertyName(string name)
        {
            var camelCasedName =  base.ResolvePropertyName(name);
            return "_" + camelCasedName;
        }
    }
}