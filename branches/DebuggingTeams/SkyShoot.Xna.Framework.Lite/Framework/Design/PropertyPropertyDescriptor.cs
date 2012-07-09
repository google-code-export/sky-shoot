using System;
using System.Reflection;

namespace SkyShoot.XNA.Framework.Design
{
	internal class PropertyPropertyDescriptor : MemberPropertyDescriptor
    {
        private PropertyInfo _property;

        public PropertyPropertyDescriptor(PropertyInfo property) : base(property)
        {
            this._property = property;
        }

        public override object GetValue(object component)
        {
            return this._property.GetValue(component, null);
        }

        public override void SetValue(object component, object value)
        {
            this._property.SetValue(component, value, null);
            this.OnValueChanged(component, EventArgs.Empty);
        }

        public override Type PropertyType
        {
            get
            {
                return this._property.PropertyType;
            }
        }
    }
}

