using Core.Entities;
using Core.Interface;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Data.Repository
{
    public class LogService 
    {
        private static readonly LogService instance = new LogService();

        private static AppDbContext _context;
        private static readonly LogService _instance;

        static LogService()
        {
            _instance = new LogService();
        }
        public static LogService Instance(AppDbContext context)
        {
            _context = context;

            return _instance;
        }
        public void AddErrorLogException(Exception ex, string page)
        {
            AddLogException(ex, "Normal", page);
        }

        public void AddErrorLog(string ex, string page)
        {
            AddLog(ex, "Normal", page);
        }

        private void AddLog(string ex, string LogType, string Page)
        {
            try
            {
                Log log = new Log();
                log.DateAdded = DateTime.Now;
                log.Message = ex;
                log.LogType = "Normal";
                log.IsSent = false;
                log.PageName = Page;
                _context.Logs.Add(log);
                _context.SaveChanges();


            }
            catch (Exception ex1)
            {
                var error = ex1.Message;
                // Cannot Log when error occure in Log :(
            }


        }

        private void AddLogException(Exception ex, string LogType, string Page)
        {
            try
            {
                Log log = new Log();
                log.DateAdded = DateTime.Now;
                log.Message = ex.StackTrace;
                log.LogType = ex.Message;
                log.IsSent = false;
                log.PageName = Page;
                _context.Logs.Add(log);
                _context.SaveChanges();


            }
            catch (Exception ex1)
            {
                var error = ex1.Message;
                // Cannot Log when error occure in Log :(
            }


        }
    }
}
