using Core.Entities;
using Infrastructure.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;


namespace UpSurgeApp.API.Controllers
{
    [ApiController]
    //[Authorize]
    [Route("[controller]")]
    public class ChannelController : Controller
    {
        private readonly IChannelService _channelService;

        public ChannelController(IChannelService channelService)
        {
            _channelService = channelService;
        }

        [HttpGet("GetChannels")]
        public ActionResult<Channel> GetChannels()
        {
            var result = _channelService.GetChannels();
            return Ok(result);
        }


        [HttpPost("AddUpdateChannel")]
        public async Task<IActionResult> AddUpdateChannel(Channel model)
        {
            await _channelService.AddUpdateChannel(model);
            return Ok();
        }
    }
}
