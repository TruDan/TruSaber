using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;

namespace TruSaber.Debugging
{
    public interface IDebugService
    {
        ObservableDictionary<string, object> Properties { get; }
        
        void UpdateProperty(string name, object value);

    }

    public class NoopDebugService : IDebugService
    {
        public ObservableDictionary<string, object> Properties { get; } = new ObservableDictionary<string, object>();
        public void UpdateProperty(string name, object value) { }
    }
    
    
    public class ObservableDictionary<TKey, TValue> : ObservableCollection<KeyValuePair<TKey, TValue>>
    {
        public ObservableDictionary()
        {
        }

        public bool Remove(TKey key)
        {
            var match = this.FirstOrDefault(kvp => kvp.Key.Equals(key));
            return base.Remove(match);
        }

        public TValue GetValue(TKey key) => this.FirstOrDefault(kvp => kvp.Key.Equals(key)).Value;

        public void SetValue(TKey key, TValue value)
        {
            var match = this.FirstOrDefault(kvp => kvp.Key.Equals(key));
            if (EqualityComparer<KeyValuePair<TKey, TValue>>.Default.Equals(match, default))
            {
                this.Add(new KeyValuePair<TKey, TValue>(key, value));
            }
            else
            {
                var index = this.IndexOf(match);
                this.SetItem(index, new KeyValuePair<TKey, TValue>(key, value));
            }
        }
        
        public TValue this[TKey key]
        {
            get => GetValue(key);
            set => SetValue(key, value);
        }
    }
}