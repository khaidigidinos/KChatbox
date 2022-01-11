using System;
using System.Linq;
using System.Security.Claims;
using Microsoft.AspNetCore.SignalR;
namespace SignalRApi
{
    public class CustomUserIdProvider : IUserIdProvider
    {
        public string GetUserId(HubConnectionContext connection)
        {
            return connection.GetHttpContext()?.User?.Claims.Where(claim => claim.Type == "id").Select(claim => claim.Value).FirstOrDefault();
        }
    }
}
