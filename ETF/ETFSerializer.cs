using System;
using System.Collections.Generic;
using System.Drawing;
using System.Runtime.InteropServices;
using System.Text;

namespace Gracie.ETF
{
    public static class ETFSerializer
    {
        public delegate int SerializeItem(byte[] buffer, int position);

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
    }
}
