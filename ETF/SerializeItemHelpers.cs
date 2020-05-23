using System;
using System.Collections.Generic;
using System.Numerics;
using System.Text;

namespace Gracie.ETF
{
    // this class exists to help generate ETFSerializer.SerializeItem instances
    public static class SerializeItemHelpers
    {
        public static ETFSerializer.SerializeItem SerializeGeneric<T>(T item, Func<byte[], int, T, int> func)
        {
            return (byte[] buffer, int position) =>
            {
                return func(buffer, position, item);
            };
        }

        public static ETFSerializer.SerializeItem SerializeMapExt(List<(string, ETFSerializer.SerializeItem)> items) =>
            SerializeGeneric(items, ETFSerializer.SerializeMapExt);

        public static ETFSerializer.SerializeItem SerializeAtomExt(string value) =>
            SerializeGeneric(value, ETFSerializer.SerializeAtomExt);

        public static ETFSerializer.SerializeItem SerializeSmallIntegerExt(byte value) =>
            SerializeGeneric(value, ETFSerializer.SerializeSmallIntegerExt);

        public static ETFSerializer.SerializeItem SerializeIntegerExt(int value) =>
            SerializeGeneric(value, ETFSerializer.SerializeIntegerExt);

        public static ETFSerializer.SerializeItem SerializeBinaryExt(string value) =>
            SerializeGeneric(value, ETFSerializer.SerializeBinaryExt);

        public static ETFSerializer.SerializeItem SerializeSmallBigExt(BigInteger value) =>
            SerializeGeneric(value, ETFSerializer.SerializeSmallBigExt);
    }
}
