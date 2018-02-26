using System;
using System.Collections.Generic;
namespace Inx.Bootstraps.Managers
{
    public interface IAuthManager
    {
        KeyValuePair<string, string> AuthorizeHeader { get; set; }
    }

    public class AuthManager : IAuthManager
    {
        public AuthManager()
        {
        }

        KeyValuePair<string, string> authorizeHeader;
        public KeyValuePair<string, string> AuthorizeHeader
        {
            get => authorizeHeader;
            set => authorizeHeader = value;
        }
    }
}
