using LuzzedroCMS.Domain.Abstract;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using LuzzedroCMS.Domain.Entities;

namespace LuzzedroCMS.Domain.Concrete
{
    public class EFLogRepository : ILogRepository
    {
        private EFDbContext context = new EFDbContext();

        public IQueryable<Log> Logs
        {
            get
            {
                return context.Logs;
            }
        }

        public IQueryable<Log> LogsByUserID(int userID)
        {
            return context.Logs.Where(p => p.UserID == userID);
        }

        public void Remove(int logID)
        {
            Log log = context.Logs.Find(logID);
            if (log != null)
            {
                context.Logs.Remove(log);
            }
            context.SaveChanges();
        }

        public void Save(Log log)
        {
            if (log.LogID == 0)
            {
                Log dbEntry = context.Logs.Add(new Log
                {
                    UserID = log.UserID,
                    Date = DateTime.Now
                });
            }
            else
            {
                Log dbEntry = context.Logs.Find(log.LogID);
                if (dbEntry != null)
                {
                    dbEntry.UserID = log.UserID;
                    dbEntry.Date = DateTime.Now;
                }
            }
            context.SaveChanges();
        }
    }
}