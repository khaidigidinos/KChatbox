using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using Microsoft.AspNetCore.Http;
using SignalRApi.ViewModels;

namespace SignalRApi.Services
{
    public class CurrentUserService
    {
        private readonly IHttpContextAccessor _httpContextAccessor;

        public CurrentUserService(IHttpContextAccessor httpContextAccessor)
        {
            _httpContextAccessor = httpContextAccessor;
        }

        public string CurrentUserId() => _httpContextAccessor.HttpContext?.User?.FindFirstValue("id");
        public string CurrentUserName() => _httpContextAccessor.HttpContext?.User?.FindFirstValue(ClaimTypes.Name);
        public string CurrentUserEmail() => _httpContextAccessor.HttpContext?.User?.FindFirstValue(ClaimTypes.Email);
        public List<string> CurrentUserRoles() => _httpContextAccessor.HttpContext?.User?.Claims.Where(claim => claim.Type == ClaimTypes.Role)
            .Select(claim => claim.Value)
            .ToList();

        public UserViewModel CurrentUserInfo()
        {
            return new UserViewModel()
            {
                Id = CurrentUserId(),
                Name = CurrentUserName(),
                Email = CurrentUserEmail(),
                Roles = CurrentUserRoles()
            };
        }
    }
}
