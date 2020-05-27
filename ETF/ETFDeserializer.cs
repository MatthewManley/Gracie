using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Text;
using System.Numerics;

namespace Gracie.ETF
{
    public static class ETFDeserializer
    {
        public static object Deserialize(byte[] buffer, ref int position)
        {
            var value = buffer[position];
            position += 1;
            switch (value)
            {
                case ETFConstants.SMALL_INTEGER_EXT:
                    return DeserializeByte(buffer, ref position);
                case ETFConstants.INTEGER_EXT:
                    return DeserializeInt32(buffer, ref position);
                case ETFConstants.ATOM_EXT:
                    return DeserializeAtom(buffer, ref position);
                case ETFConstants.STRING_EXT:
                    return DeserializeStringExt(buffer, ref position);
                case ETFConstants.LIST_EXT:
                    return DeserializeList(buffer, ref position);
                case ETFConstants.BINARY_EXT:
                    return DeserializeBinary(buffer, ref position);
                case ETFConstants.SMALL_BIG_EXT:
                    return DeserializeSmallBigExt(buffer, ref position);
                case ETFConstants.MAP_EXT:
                    return DeserializeMap(buffer, ref position);
                case ETFConstants.NIL_EXT:
                    return null;
                case ETFConstants.FORMAT_VERSION:
                case ETFConstants.NEW_FLOAT_EXT:
                case ETFConstants.BIT_BINARY_EXT:
                case ETFConstants.COMPRESSED:
                case ETFConstants.FLOAT_EXT:
                case ETFConstants.REFERENCE_EXT:
                case ETFConstants.PORT_EXT:
                case ETFConstants.PID_EXT:
                case ETFConstants.SMALL_TUPLE_EXT:
                case ETFConstants.LARGE_TUPLE_EXT:
                case ETFConstants.LARGE_BIG_EXT:
                case ETFConstants.NEW_FUN_EXT:
                case ETFConstants.EXPORT_EXT:
                case ETFConstants.NEW_REFERENCE_EXT:
                case ETFConstants.SMALL_ATOM_EXT:
                case ETFConstants.FUN_EXT:
                case ETFConstants.ATOM_UTF8_EXT:
                case ETFConstants.SMALL_ATOM_UTF8_EXT:
                default:
                    throw new NotSupportedException();
            }
        }

        public static Dictionary<string, object> DeserializeMap(byte[] buffer, ref int position)
        {
            var mapItemCount = Convert.ToInt32(DeserializeUInt32(buffer, ref position));
            var result = new Dictionary<string, object>(mapItemCount);
            for (var i = 0; i < mapItemCount; i++)
            {
                string key;
                var keyObj = Deserialize(buffer, ref position);
                if (keyObj is string keyStr)
                {
                    key = keyStr;
                }
                else if (keyObj is byte[] keyBytes)
                {
                    key = ETFConstants.Latin1.GetString(keyBytes);
                }
                else
                {
                    //TODO : Custom exception
                    throw new Exception("Unexpected format for map key");
                }
                var value = Deserialize(buffer, ref position);
                result.Add(key, value);
            }
            return result;
        }

        public static string DeserializeAtom(byte[] buffer, ref int position)
        {
            var length = DeserializeUInt16(buffer, ref position);
            var result = ETFConstants.Latin1.GetString(buffer, position, length);
            position += length;
            if (result == "nil")
                return null;
            return result;
        }

        public static string DeserializeStringExt(byte[] buffer, ref int position)
        {
            var length = DeserializeInt32(buffer, ref position);
            var result = ETFConstants.Latin1.GetString(buffer, position, length);
            position += length;
            return result;
        }

        public static BigInteger DeserializeSmallBigExt(byte[] buffer, ref int position)
        {
            var n = DeserializeByte(buffer, ref position);
            var sign = DeserializeByte(buffer, ref position);
            var result = new BigInteger(0);
            var twoFiftySix = new BigInteger(256);
            for (int i = 0; i < n; i++)
            {
                var d = DeserializeByte(buffer, ref position);
                result += d * BigInteger.Pow(twoFiftySix, i);
            }
            if (sign == 1)
                result = -result;
            return result;

        }

        public static List<object> DeserializeList(byte[] buffer, ref int position)
        {
            var length = DeserializeUInt32(buffer, ref position);
            var result = new List<object>();
            for (int i = 0; i < length; i++)
            {
                var value = Deserialize(buffer, ref position);
                result.Add(value);
            }
            position += 1; // Disreguard the Tail
            return result;
        }

        public static byte[] DeserializeBinary(byte[] buffer, ref int position)
        {

            var length = DeserializeUInt32(buffer, ref position);
            var signed = Convert.ToInt32(length);
            var result = new Span<byte>(buffer, position, signed).ToArray();
            position += signed;
            return result;
        }

        public static int DeserializeInt32(byte[] buffer, ref int position)
        {
            var span = new Span<byte>(buffer, position, 4);
            if (BitConverter.IsLittleEndian)
                span.Reverse();
            var result = MemoryMarshal.Read<int>(span);
            position += 4;
            return result;
        }

        public static uint DeserializeUInt32(byte[] buffer, ref int position)
        {
            var span = new Span<byte>(buffer, position, 4);
            if (BitConverter.IsLittleEndian)
                span.Reverse();
            var result = MemoryMarshal.Read<uint>(span);
            position += 4;
            return result;
        }

        public static ushort DeserializeUInt16(byte[] buffer, ref int position)
        {
            var span = new Span<byte>(buffer, position, 2);
            if (BitConverter.IsLittleEndian)
                span.Reverse();
            var result = MemoryMarshal.Read<ushort>(span);
            position += 2;
            return result;
        }

        public static byte DeserializeByte(byte[] buffer, ref int position)
        {
            var value = buffer[position];
            position += 1;
            return value;
        }
    }
}
