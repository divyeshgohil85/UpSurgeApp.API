using System;
using System.Collections.Generic;
using System.Text;

namespace Infrastructure.Data.Repository
{
    public class ValuesService
    {
        private readonly AppDbContext _context;
        public ValuesService(AppDbContext context)
        {
            _context = context;
        }
    }
}
