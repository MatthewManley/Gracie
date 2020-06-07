using System;
using System.Collections.Generic;
using System.Text;

namespace Gracie.Http
{
    public enum TokenType
    {
        Bot,
        Bearer
    }

    public static class TokenTypeToString
    {
        public static string StringValue(this TokenType tokenType)
        {
            switch (tokenType)
            {
                case TokenType.Bot:
                    return "Bot";
                case TokenType.Bearer:
                    return "Bearer";
                default:
                    throw new NotSupportedException();
            }
        }
    }
}
