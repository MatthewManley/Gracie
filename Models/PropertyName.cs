using System;
using System.Collections.Generic;
using System.Text;

namespace Gracie.Models
{
    public sealed class PropertyName : Attribute
    {
        public PropertyName(string name)
        {
            if (string.IsNullOrWhiteSpace(name))
            {
                throw new ArgumentException("Key name cannot be null or empty", nameof(name));
            }

            Name = name;
        }

        public string Name { get; }
    }
}
