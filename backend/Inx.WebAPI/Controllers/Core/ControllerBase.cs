using System;
using System.Threading.Tasks;
using Inx.Data.Entities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Identity;

namespace Inx.WebAPI.Controllers.Core
{
    public abstract class InxControllerBase : Controller
    {
        readonly UserManager<UserEntity> userManager;

        public InxControllerBase(UserManager<UserEntity> userManager)
        {
            this.userManager = userManager;
        }

        protected async Task<UserEntity> GetCurrentUserAsync()
        {
            return await userManager.GetUserAsync(User);
        } 
    }
}
