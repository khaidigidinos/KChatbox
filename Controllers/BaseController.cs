using System;
using System.Threading.Tasks;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection;

namespace SignalRApi.Controllers
{
    public class BaseController : ControllerBase
    {
        private ISender _mediatr;
        protected ISender sender => _mediatr ??= HttpContext.RequestServices.GetService<ISender>();

        protected class ApiResponse
        {
            public object data { get; set; }
            public int status { get; set; }
            public string message { get; set; }
        }

        protected IActionResult MKReposnse(dynamic returnedApi)
        {
            var response = new ApiResponse()
            {
                data = returnedApi.data,
                status = returnedApi.status,
                message = returnedApi.message
            };
            return Ok(response);
        }
    }
}
