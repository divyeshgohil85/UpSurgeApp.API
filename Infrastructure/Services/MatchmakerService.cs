using Core.Entities;
using Infrastructure.Data;
using System.Linq;
using System.Threading.Tasks;

namespace Infrastructure.Services
{
    public interface IMatchmakerService
    {
        MatchmakerUserPreferencesValues GetUserPreferences(int userId);
        Task UpdateUserPreferenceValues(MatchmakerUserPreferencesValues matchmakerUserPreferencesValues);
    }

    public class MatchmakerService : IMatchmakerService
    {
        private readonly UpSurgeAppDbContext _context;

        public MatchmakerService(UpSurgeAppDbContext context)
        {
            _context = context;
        }

        public MatchmakerUserPreferencesValues GetUserPreferences(int userId)
        {
            var existingValues = _context.MatchmakerUserPreferences.FirstOrDefault(x => x.UserId == userId);
            return existingValues?.Values;
        }

        public async Task UpdateUserPreferenceValues(MatchmakerUserPreferencesValues matchmakerUserPreferencesValues)
        {
            var existingValues = _context.MatchmakerUserPreferences.FirstOrDefault(x => x.UserId == matchmakerUserPreferencesValues.UserId);
            if(existingValues == null)
            {
                _context.MatchmakerUserPreferences.Add(new MatchmakerUserPreference { UserId = matchmakerUserPreferencesValues.UserId, Values = matchmakerUserPreferencesValues });
            }
            else
            {
                existingValues.Values = matchmakerUserPreferencesValues;
            }

            await _context.SaveChangesAsync();
        }
    }
}
