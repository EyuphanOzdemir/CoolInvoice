﻿using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InvoicrInfrastructure.Logger.XUnitLogger
{
	public class XUnitLoggerWrapper<T> : ILogger<T>
	{
		private readonly ILogger _logger;

		public XUnitLoggerWrapper(ILogger logger)
		{
			_logger = logger ?? throw new ArgumentNullException(nameof(logger));
		}

		public IDisposable BeginScope<TState>(TState state)
		{
			return _logger.BeginScope(state);
		}

		public bool IsEnabled(LogLevel logLevel)
		{
			return _logger.IsEnabled(logLevel);
		}

		public void Log<TState>(LogLevel logLevel, EventId eventId, TState state, Exception exception, Func<TState, Exception, string> formatter)
		{
			_logger.Log(logLevel, eventId, state, exception, formatter);
		}
	}
}
