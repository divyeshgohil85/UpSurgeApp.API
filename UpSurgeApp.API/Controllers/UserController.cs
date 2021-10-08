using Core.Entities;
using Infrastructure.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Linq;
using System.Threading.Tasks;

namespace UpSurgeApp.API.Controllers
{
    [ApiController]
    [Authorize]
    [Route("[controller]")]

    public class UserController : Controller
    {
        private readonly IUserService _userService;
        private readonly IWebHostEnvironment _env;
        private readonly UserManager<AppUser> _userManager;

        public UserController(IUserService userService, IWebHostEnvironment env)
        {
            _userService = userService;
            _env = env;

        }

        [HttpGet("GetUsers")]
        public ActionResult<AppUserResponse> GetUsers()
        {
            var emailAddress = User.Claims.Where(x => x.Type.Contains("email")).Select(x => x.Value).FirstOrDefault();
            var result = _userService.GetUsers(emailAddress);
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

        [HttpPost("BlockUser")]
        public async Task<IActionResult> BlockUser(BlockUsers blockUser)
        {
            var emailAddress = User.Claims.Where(x => x.Type.Contains("email")).Select(x => x.Value).FirstOrDefault();

            var result = await _userService.BlockUserById(emailAddress, blockUser);
            return Ok(result);
        }
    }
}
