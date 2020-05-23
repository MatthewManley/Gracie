using System;
using System.Collections.Generic;
using System.Text;

namespace Gracie.ETF
{
    public sealed class EtfProperty : Attribute
    {
        public EtfProperty(string name, bool serializeIfNull = false)
        {
            if (string.IsNullOrWhiteSpace(name))
            {
                throw new ArgumentException("Key name cannot be null or empty", nameof(name));
            }

            Name = name;
            SerializeIfNull = serializeIfNull;
        }

        public string Name { get; }
        public bool SerializeIfNull { get; }
    }
}
