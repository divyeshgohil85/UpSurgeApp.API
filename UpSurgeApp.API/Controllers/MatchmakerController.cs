using Core.Entities;
using Infrastructure.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace UpSurgeApp.API.Controllers
{
    [ApiController]
    [Authorize]
    [Route("[controller]")]
    public class MatchmakerController : ControllerBase
    {
        private readonly IMatchmakerService _matchmakerService;

        public MatchmakerController(IMatchmakerService matchmakerService)
        {
            _matchmakerService = matchmakerService;
        }

        [HttpGet("GetUserPreferences")]
        public ActionResult<MatchmakerUserPreferencesValues> GetUserPreferences(int userId)
        {
            var result = _matchmakerService.GetUserPreferences(userId);
            return Ok(result);
        }

        [HttpPost("UpdateUserPreferenceValues")]
        public async Task<IActionResult> UpdateUserPreferenceValues(MatchmakerUserPreferencesValues matchmakerUserPreferenceValues)
        {
            await _matchmakerService.UpdateUserPreferenceValues(matchmakerUserPreferenceValues);
            return Ok();
        }
    }
}
