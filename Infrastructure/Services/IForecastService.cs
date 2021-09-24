using Core.Entities;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Infrastructure.Services
{
    public interface IForecastService
    {
        Task Calculate();
        Task<IEnumerable<Forecast>> GetGainers(int skip, int take);
        Task<IEnumerable<Forecast>> GetLoosers(int skip, int take);
    }
}
