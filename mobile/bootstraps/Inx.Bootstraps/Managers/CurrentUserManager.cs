using System;
using Inx.Bootstraps.Models;
namespace Inx.Bootstraps.Managers
{
    public interface ICurrentUserManager
    {
        ProfileModel Profile { get; set; }
    }

    public class CurrentUserManager : ICurrentUserManager
    {
        public CurrentUserManager()
        {
        }

        ProfileModel profile;
        public ProfileModel Profile
        {
            get => profile;
            set => profile = value;
        }
    }
}
