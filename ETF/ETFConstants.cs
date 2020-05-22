using System.Text;

namespace Gracie.ETF
{
    public static class ETFConstants
    {
        public static readonly Encoding Latin1 = Encoding.GetEncoding("ISO-8859-1");
        public const byte FORMAT_VERSION = 131;
        public const byte NEW_FLOAT_EXT = 70;
        public const byte BIT_BINARY_EXT = 77;
        public const byte COMPRESSED = 80;
        public const byte SMALL_INTEGER_EXT = 97;
        public const byte INTEGER_EXT = 98;
        public const byte FLOAT_EXT = 99;
        public const byte ATOM_EXT = 100;
        public const byte REFERENCE_EXT = 101;
        public const byte PORT_EXT = 102;
        public const byte PID_EXT = 103;
        public const byte SMALL_TUPLE_EXT = 104;
        public const byte LARGE_TUPLE_EXT = 105;
        public const byte NIL_EXT = 106;
        public const byte STRING_EXT = 107;
        public const byte LIST_EXT = 108;
        public const byte BINARY_EXT = 109;
        public const byte SMALL_BIG_EXT = 110;
        public const byte LARGE_BIG_EXT = 111;
        public const byte NEW_FUN_EXT = 112;
        public const byte EXPORT_EXT = 113;
        public const byte NEW_REFERENCE_EXT = 114;
        public const byte SMALL_ATOM_EXT = 115;
        public const byte MAP_EXT = 116;
        public const byte FUN_EXT = 117;
        public const byte ATOM_UTF8_EXT = 118;
        public const byte SMALL_ATOM_UTF8_EXT = 119;
    }
}
