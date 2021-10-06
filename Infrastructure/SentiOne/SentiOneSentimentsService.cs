using Infrastructure.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.SentiOne
{
    public interface ISentiOneSentimentsService
    {
        Task CacheTotals();
    }

    public class SentiOneSentimentsService : ISentiOneSentimentsService
    {
        private readonly SentiOneHttpClient _sentiOneHttpClient;
        private readonly UpSurgeAppDbContext _context;

        public SentiOneSentimentsService(SentiOneHttpClient sentiOneHttpClient, UpSurgeAppDbContext context)
        {
            _sentiOneHttpClient = sentiOneHttpClient;
            _context = context;
        }

        public Task CacheTotals()
        {
            throw new NotImplementedException();
        }
    }
}
