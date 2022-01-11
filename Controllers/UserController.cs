using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SignalRApi.Features.User.Queries.InfoUser;
using SignalRApi.Features.User.Queries.LoginUser;
using SignalRApi.Feautures.User.Commands.RegisterUser;
using SignalRApi.Feautures.User.Commands.UpdateFirebaseTokenCommand;
using SignalRApi.Feautures.User.Queries.FindUsersQuery;

namespace SignalRApi.Controllers
{
    [Route("api/v1/user")]
    public class UserController : BaseController
    {
        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginUser loginUser, string clientId)
        {
            loginUser.ClientId = clientId;
            var token = await sender.Send(loginUser);

            return MKReposnse(new
            {
                data = token,
                status = StatusCodes.Status200OK,
                message = "Login successfully"
            });
        }

        [HttpGet("info")]
        public async Task<IActionResult> Info()
        {
            var userInfo = await sender.Send(new InfoUser());

            return MKReposnse(new
            {
                data = userInfo,
                status = StatusCodes.Status200OK,
                message = "Get info successfully"
            });
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] RegisterUser registerInfo, string clientId)
        {
            registerInfo.ClientId = clientId;
            var token = await sender.Send(registerInfo);

            return MKReposnse(new
            {
                data = token,
                status = StatusCodes.Status200OK,
                message = "Register successfully"
            });
        }

        [HttpPost("logout")]
        public  IActionResult Logout()
        { 
            return MKReposnse(new
            {
                data = (string) null,
                status = StatusCodes.Status200OK,
                message = "Not implemented"
            });
        }

        [HttpPost("find")]
        public async Task<IActionResult> FindUsers([FromBody] FindUsersQuery query)
        {
            var data = await sender.Send(query);
            return MKReposnse(new
            {
                data = data,
                status = StatusCodes.Status200OK,
                message = "Get users list successfully"
            });
        }

        [HttpPost("updateFirebasetoken")]
        public IActionResult UpdateFirebaseToken([FromBody] UpdateFirebaseTokenCommand command)
        {
            // TODO
            sender.Send(command);
            return MKReposnse(new
            {
                data = (string) null,
                status = StatusCodes.Status200OK,
                message = "Request is prrocessing"
            });
        }
    }
}
