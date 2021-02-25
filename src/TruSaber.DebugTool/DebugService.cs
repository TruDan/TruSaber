using TruSaber.Debugging;

namespace TruSaber.DebugTool
{
    public class DebugService : IDebugService
    {
        private ObservableDictionary<string, object> _properties;

        public ObservableDictionary<string, object> Properties => _properties;
        
        public DebugService()
        {
            _properties = new ObservableDictionary<string, object>();
        }
        
        public void UpdateProperty(string name, object value)
        {
            _properties.SetValue(name, value);
        }
    }
}