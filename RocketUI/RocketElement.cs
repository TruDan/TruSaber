using System.Collections.Generic;
using RocketUI.Serialization;

namespace RocketUI
{

    public interface IRocketElement
    {
        PropertyStore         Properties { get; }
        IList<IRocketElement> Children   { get; }
    }
    public abstract class RocketElement : IRocketElement
    {
        private PropertyStore _properties;

        /// <summary>
        /// Gets the dictionary of properties for this widget
        /// </summary>
        public PropertyStore Properties
        {
            get
            {
                if (_properties == null)
                {
                    _properties = new PropertyStore(this);
                }

                return _properties;
            }
        }

        public virtual IList<IRocketElement> Children
        {
            get => null;
        }
    }
}