using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SignalRApi.Feautures.Room.Commands.CreateNewRoomCommand;
using SignalRApi.Feautures.Room.Queries.GetMessagesForRoomQuery;
using SignalRApi.Feautures.Room.Queries.GetRoomForUser;

namespace SignalRApi.Controllers
{
    [Route("api/v1/room")]
    public class RoomController : BaseController
    {
        [HttpGet("all")]
        public async Task<IActionResult> GetRoomsOfUser()
        {
            var data = await sender.Send(new GetRoomsForUserQuery());
            return MKReposnse(new {
                data = data,
                status = StatusCodes.Status200OK,
                message = "Get list of Rooms sucessfully."
            });
        }

        [HttpPost("create")]
        public async Task<IActionResult> CreateNewRoom([FromBody] CreateNewRoomCommand command)
        {
            var data = await sender.Send(command);
            return MKReposnse(new
            {
                data = data,
                status = StatusCodes.Status200OK,
                message = "Create new room sucessfully."
            });
        }

        [HttpGet("getMessages")]
        public async Task<IActionResult> GetMessages(string roomId)
        {
            var data = await sender.Send(new GetMessagesForRoomQuery() { RoomId = roomId });
            return MKReposnse(new
            {
                data = data,
                status = StatusCodes.Status200OK,
                message = $"Get messages for room with Id {roomId} successfully."
            });
        } 
    }
}
