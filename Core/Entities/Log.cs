using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Core.Entities
{
    public class Log
    {
        public Log()
        {
            CreatedAt = DateTime.UtcNow;
        }
        [Key]
        public int Id { get; set; }
        public string Message { get; set; }
        public System.DateTime DateAdded { get; set; }
        public string LogType { get; set; }
        public bool IsSent { get; set; }
        public string PageName { get; set; }
        public DateTime CreatedAt { get; set; }


    }
}
