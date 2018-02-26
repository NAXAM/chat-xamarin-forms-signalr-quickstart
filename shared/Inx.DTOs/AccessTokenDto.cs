using System;
namespace Inx.DTOs
{
    public class AccessTokenDto
    {
        public string AccessToken { get; set; }

        public string TokenType { get; set; }

        public int ExpiresIn { get; set; }
    }
}
