using System;
using System.Collections.Generic;
using System.Text;

namespace Gracie.Gateway
{
    public static class CastHelpers
    {
        public static T SimpleCast<T>(object obj)
        {
            if (obj is null)
            {
                return default;
            }
            if (obj is T cast)
            {
                return cast;
            }
            //TODO : custom exception for unexpected type
            throw new Exception();
        }

        public static int? IntCast(object obj)
        {
            if (obj is null)
            {
                return null;
            }
            if (obj is int intResult)
            {
                return intResult;
            }
            if (obj is byte byteResult)
            {
                return Convert.ToInt32(byteResult);
            }
            throw new Exception();
        }
    }
}
