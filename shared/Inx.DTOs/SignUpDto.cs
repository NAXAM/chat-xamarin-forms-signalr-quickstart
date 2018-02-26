using System;
namespace Inx.DTOs
{
    public class SignUpDto : SignInDto
    {
        public string PasswordConfirmation { get; set; }
    }
}