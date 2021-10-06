using Core.Entities;
using Infrastructure.Data;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Infrastructure.Services
{
    public interface IChannelService
    {
        List<Channel> GetChannels();
        Task AddUpdateChannel(Channel channelValues);

    }

    public class ChannelService : IChannelService
    {
        private readonly UpSurgeAppDbContext _context;

        public ChannelService(UpSurgeAppDbContext context)
        {
            _context = context;
        }

        public List<Channel> GetChannels()
        {
            var existingValues = _context.Channel.ToList();
            return existingValues?.ToList();
        }

        public async Task AddUpdateChannel(Channel channelValues)
        {
            if (channelValues.Id >= 0)
            {
                var existingValues = _context.Channel.FirstOrDefault(x => x.Id == channelValues.Id);
                if (existingValues == null)
                {
                    _context.Channel.Add(new Channel { Id = channelValues.Id, ChannelName = channelValues.ChannelName, TimeToke = channelValues.TimeToke });
                }
                else
                {
                    existingValues.Id = channelValues.Id;
                    existingValues.ChannelName = channelValues.ChannelName;
                    existingValues.TimeToke = channelValues.TimeToke;
                }

                await _context.SaveChangesAsync();

            }

        }

    }
}
