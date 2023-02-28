using System;
using System.ComponentModel.DataAnnotations;

namespace WebShop.Main.Context
{
	public class Logger
	{
        public Guid LoggerId { get; set; }

        public LoggerLevel LoggerLevel { get; set; }

        public string Message { get; set; }

        public DateTime LogTime { get; set; }
    }

    public enum LoggerLevel
    {
        Info,
        Warn,
        Error,
    }
} 
