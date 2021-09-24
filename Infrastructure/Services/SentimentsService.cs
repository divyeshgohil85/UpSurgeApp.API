using Core.Entities;
using EFCore.BulkExtensions;
using Infrastructure.Data;
using Infrastructure.SentiOne;
using System.Threading.Tasks;

namespace Infrastructure.Services
{
    public interface ISentimentsService
    {
        Task Load();
    }

    public class SentimentsService : ISentimentsService
    {
        private readonly SentiOneHttpClient _sentiOneHttpClient;
        private readonly UpSurgeAppDbContext _context;

        public SentimentsService(SentiOneHttpClient sentiOneHttpClient, UpSurgeAppDbContext context)
        {
            _sentiOneHttpClient = sentiOneHttpClient;
            _context = context;
        }

        public async Task Load()
        {
            _context.Truncate<SentioneTopic>();

            var sentimentTopics = await _sentiOneHttpClient.GetTopicsAsync();
            foreach (var topic in sentimentTopics)
            {
                _context.SentioneTopics.Add(new SentioneTopic { Name = topic.Name, Id = topic.Id });
                _context.SaveChanges();
            }
        }
    }
}
