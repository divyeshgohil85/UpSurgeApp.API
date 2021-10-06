using System;
using System.Collections.Generic;
using System.Text;

namespace Infrastructure.Data.Repository
{
    public class ValuesService
    {
        private readonly UpSurgeAppDbContext _context;
        public ValuesService(UpSurgeAppDbContext context)
        {
            _context = context;
        }
    }
}
