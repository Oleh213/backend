using System;
using WebShop.Main.Context;

namespace WebShop.Main.Interfaces
{
	public interface ILoggerBL
	{
		void AddLog(LoggerLevel loggerLevel, string message);
	}
}

