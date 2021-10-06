using Core.Entities;
using Infrastructure.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace UpSurgeApp.API.Controllers
{
    [ApiController]
    //[Authorize]
    [Route("[controller]")]

    public class UserController : Controller
    {
        private readonly IUserService _userService;
        private readonly IWebHostEnvironment _env;

        public UserController(IUserService userService, IWebHostEnvironment env)
        {
            _userService = userService;
            _env = env;
        }

        [HttpGet("GetUsers")]
        public ActionResult<AppUserResponse> GetUsers()
        {
            var result = _userService.GetUsers();
            return Ok(result);
        }


        [HttpPost("AddUpdateUser")]
        public async Task<IActionResult> AddUpdateUser(AppUser model)
        {
            await _userService.AddUpdateUser(model);
            return Ok();
        }

        [HttpGet("GetImageURL")]
        public string GetImageURL(string base64string)
        {
            var folderPath = System.IO.Path.Combine(_env.ContentRootPath, "Images/" + base64string);
            if (!System.IO.Directory.Exists(folderPath))
            {
                System.IO.Directory.CreateDirectory(folderPath);
            }

            return folderPath;

        }
    }
}
