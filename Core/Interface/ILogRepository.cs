using Core.Entities;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Core.Interface
{
    public interface ILogRepository
    {

        void AddErrorLog(Exception ex);
    }
}
