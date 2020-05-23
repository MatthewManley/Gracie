using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Drawing;
using System.Linq;
using System.Numerics;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Text;

namespace Gracie.ETF
{
    public static class ETFSerializer
    {
        public delegate int SerializeItem(byte[] buffer, int position);

        public static int ObjectToTerm(byte[] buffer, int position, object obj)
        {
            buffer[position] = ETFConstants.FORMAT_VERSION;
            var map = ObjectToMap(obj);
            return 1 + SerializeMapExt(buffer, position + 1, map);
        }

        public static List<(string, SerializeItem)> ObjectToMap(object obj)
        {
            var properties = obj.GetType().GetProperties()
                .Select(x => (x, x.GetCustomAttribute<EtfProperty>()))
                .Where((x) => x.Item2 != null).ToList();
            var result = new List<(string, SerializeItem)>();
            foreach (var (property, propertyName) in properties)
            {
                var t = property.PropertyType;
                if (t.BaseType == typeof(Enum))
                {
                    t = Enum.GetUnderlyingType(t);
                }
                else if (t.IsGenericType && t.GetGenericTypeDefinition() == typeof(Nullable<>))
                {
                    var value = property.GetValue(obj);
                    if (value is null)
                    {
                        if (propertyName.SerializeIfNull)
                        {
                            result.Add((propertyName.Name, SerializeItemHelpers.SerializeAtomExt("nil")));
                        }
                        continue;
                    }
                    else
                    {
                        t = t.GenericTypeArguments.First();
                    }
                }
                var tc = Type.GetTypeCode(t);
                SerializeItem newSerializeItem = null;
                switch (tc)
                {
                    case TypeCode.Boolean:
                        {
                            var value = (bool)property.GetValue(obj) ? "true" : "false";
                            newSerializeItem = SerializeItemHelpers.SerializeBinaryExt(value);
                        }
                        break;
                    case TypeCode.Byte:
                        {
                            var value = (byte)property.GetValue(obj);
                            newSerializeItem = SerializeItemHelpers.SerializeSmallIntegerExt(value);
                        }
                        break;
                    case TypeCode.Char:
                        break;
                    case TypeCode.DateTime:
                        break;
                    case TypeCode.DBNull:
                        break;
                    case TypeCode.Decimal:
                        break;
                    case TypeCode.Double:
                        break;
                    case TypeCode.Empty:
                        break;
                    case TypeCode.Int16:
                        break;
                    case TypeCode.Int32:
                        {
                            var value = (int)property.GetValue(obj);
                            newSerializeItem = SerializeItemHelpers.SerializeIntegerExt(value);
                        } 
                        break;
                    case TypeCode.Int64:
                        {
                            var value = (long)property.GetValue(obj);
                            var big = new BigInteger(value);
                            newSerializeItem = SerializeItemHelpers.SerializeSmallBigExt(big);
                        }
                        break;
                    case TypeCode.Object:
                        {
                            var value = property.GetValue(obj);
                            var valueMap = ObjectToMap(value);
                            newSerializeItem = SerializeItemHelpers.SerializeMapExt(valueMap);
                        }
                        break;
                    case TypeCode.SByte:
                        break;
                    case TypeCode.Single:
                        break;
                    case TypeCode.String:
                        {
                            var value = (string)property.GetValue(obj);
                            if (value is null)
                            {
                                if (propertyName.SerializeIfNull)
                                {
                                    newSerializeItem = SerializeItemHelpers.SerializeAtomExt("nil");
                                }
                            }
                            else
                            {
                                newSerializeItem = SerializeItemHelpers.SerializeBinaryExt(value);
                            }
                            break;
                        }
                        break;
                    case TypeCode.UInt16:
                        break;
                    case TypeCode.UInt32:
                        break;
                    case TypeCode.UInt64:
                        {
                            var value = (ulong)property.GetValue(obj);
                            var big = new BigInteger(value);
                            newSerializeItem = SerializeItemHelpers.SerializeSmallBigExt(big);
                        }
                        break;
                    default:
                        break;
                }
                if (newSerializeItem != null)
                {
                    result.Add((propertyName.Name, newSerializeItem));
                }
            }
            return result;
        }

        public static int SerializeMapExt(byte[] buffer, int position, List<(string, SerializeItem)> items)
        {
            int length = 0;
            buffer[position] = ETFConstants.MAP_EXT;
            length += 1;
            var unsignedListLength = Convert.ToUInt32(items.Count);
            length += SerializeUInt32(buffer, position + length, unsignedListLength);
            foreach (var (name, serializeItem) in items)
            {
                length += SerializeBinaryExt(buffer, position + length, name);
                length += serializeItem(buffer, position + length);
            }
            return length;
        }

        public static int SerializeBinaryExt(byte[] buffer, int position, string value)
        {
            int length = 0;
            buffer[position] = ETFConstants.BINARY_EXT;
            length += 1;
            var unsignedStringLength = Convert.ToUInt32(value.Length);
            length += SerializeUInt32(buffer, position + length, unsignedStringLength);
            length += ETFConstants.Latin1.GetBytes(value, 0, value.Length, buffer, position + length);
            return length;
        }

        public static int SerializeAtomExt(byte[] buffer, int position, string value)
        {
            int length = 0;
            buffer[position] = ETFConstants.ATOM_EXT;
            length += 1;
            var unsignedStringLength = Convert.ToUInt16(value.Length);
            length += SerializeUInt16(buffer, position + length, unsignedStringLength);
            length +=  ETFConstants.Latin1.GetBytes(value, 0, value.Length, buffer, position + length);
            return length;
        }

        public static int SerializeInt32(byte[] buffer, int position, int value)
        {
            var span = new Span<byte>(buffer, position, 4);
            MemoryMarshal.Write<int>(span, ref value);
            if (BitConverter.IsLittleEndian)
                span.Reverse();
            return 4;
        }

        public static int SerializeUInt32(byte[] buffer, int position, uint value)
        {
            var span = new Span<byte>(buffer, position, 4);
            MemoryMarshal.Write<uint>(span, ref value);
            if (BitConverter.IsLittleEndian)
                span.Reverse();
            return 4;
        }

        public static int SerializeUInt16(byte[] buffer, int position, ushort value)
        {
            var span = new Span<byte>(buffer, position, 2);
            MemoryMarshal.Write<ushort>(span, ref value);
            if (BitConverter.IsLittleEndian)
                span.Reverse();
            return 2;
        }

        public static int SerializeSmallIntegerExt(byte[] buffer, int position, byte value)
        {
            buffer[position] = ETFConstants.SMALL_INTEGER_EXT;
            buffer[position + 1] = value;
            return 2;
        }

        public static int SerializeIntegerExt(byte[] buffer, int position, int value)
        {
            buffer[position] = ETFConstants.INTEGER_EXT;
            position += 1;
            var length = 1;
            length += SerializeInt32(buffer, position, value);
            return length;
        }

        public static int SerializeSmallBigExt(byte[] buffer, int position, BigInteger value)
        {
            buffer[position] = ETFConstants.SMALL_BIG_EXT;
            if (value >= 0)
            {
                buffer[position + 2] = 0;
            }
            else
            {
                buffer[position + 2] = 1;
                value = -value;
            }
            var bytes = value.ToByteArray();
            buffer[position + 1] = Convert.ToByte(bytes.Length);
            bytes.CopyTo(buffer, position + 3);
            return bytes.Length + 3;
        }
    }
}
