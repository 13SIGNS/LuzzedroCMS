using LuzzedroCMS.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LuzzedroCMS.Domain.Abstract
{
    public interface ILogRepository
    {
        IQueryable<Log> Logs { get; }
        IQueryable<Log> LogsByUserID(int userID);
        void Save(Log log);
        void Remove(int logID);
    }
}