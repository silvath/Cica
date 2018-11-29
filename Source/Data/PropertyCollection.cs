using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;

namespace Data
{
    [CollectionDataContract]
    public class PropertyCollection : List<Property>
    {
        public Property Create(string attributeName, string propertyName, string value) 
        {
            Property property = this.GetProperty(propertyName);
            if (property == null) 
            {
                property = new Property();
                property.PropertyName = propertyName;
                property.AttributeName = attributeName;
                property.Value = value;
                this.Add(property);
            }
            return (property);
        }

        public Property GetProperty(string propertyName) 
        {
            foreach (Property property in this)
                if (property.PropertyName == propertyName)
                    return (property);
            return (null);
        }

        public string GetPropertyValue(string propertyName) 
        {
            Property property = GetProperty(propertyName);
            if (property != null)
                return (property.Value);
            return (string.Empty);
        } 

        public void Update(string propertyName, string value) 
        {
            Property property = GetProperty(propertyName);
            if (property == null)
                property = Create(string.Empty, propertyName, value);
            else
                property.Value = value;
        }

        public int Increment(string propertyName) 
        {
            Property property = GetProperty(propertyName);
            if (property == null)
                property = Create(string.Empty, propertyName, "0");
            int value = Int32.Parse(property.Value);
            value++;
            property.Value = value.ToString();
            return (value);
        }

        public bool IsEqual(PropertyCollection properties)
        {
            if (this.Count != properties.Count)
                return (false);
            for (int i = 0; i < this.Count; i++)
                if (!this[i].IsEqual(properties[i]))
                    return (false);
            return (true);
        }

        public PropertyCollection Clone() 
        {
            PropertyCollection clone = new PropertyCollection();
            foreach (Property property in this)
                clone.Add(property.Clone());
            return (clone);
        }
    }
}
