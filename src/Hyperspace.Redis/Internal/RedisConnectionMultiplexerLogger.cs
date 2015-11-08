using Microsoft.Framework.Logging;
using System;
using System.IO;
using System.Text;

namespace Hyperspace.Redis.Internal
{
    public class RedisConnectionMultiplexerLogger : TextWriter, ILogger
    {
        private readonly ILogger _logger;
        private readonly StringBuilder _buffer;

        public RedisConnectionMultiplexerLogger(ILogger logger)
        {
            _logger = logger;
            _buffer = new StringBuilder();
            Encoding = Encoding.UTF8;
            DefualtLogLevel = LogLevel.Debug;
        }

        public override Encoding Encoding { get; }

        public LogLevel DefualtLogLevel
        {
            get;
            set;
        }

        public void Log(LogLevel logLevel, int eventId, object state, Exception exception, Func<object, Exception, string> formatter)
        {
            _logger.Log(logLevel, eventId, state, exception, formatter);
        }

        public bool IsEnabled(LogLevel logLevel)
        {
            return _logger.IsEnabled(logLevel);
        }

        public IDisposable BeginScopeImpl(object state)
        {
            return _logger.BeginScopeImpl(state);
        }

        public override void Write(char value)
        {
            _buffer.Append(value);
            if (_buffer.Length >= NewLine.Length)
            {
                var isNewLine = true;
                for (var i = 0; i < NewLine.Length; i++)
                {
                    if (NewLine[i] == _buffer[_buffer.Length - NewLine.Length + i])
                        continue;
                    isNewLine = false;
                    break;
                }
                if (isNewLine)
                {
                    var log = _buffer.ToString();
                    _buffer.Clear();
                    _logger.Log(DefualtLogLevel, 0, log, null, (o, e) => (string)o);
                }
            }
        }

        public override void WriteLine(string value)
        {
            if (_buffer.Length > 0)
            {
                var log = _buffer.ToString();
                _buffer.Clear();
                _logger.Log(DefualtLogLevel, 0, log + value, null, (o, e) => (string) o);
            }
            else
            {
                _logger.Log(DefualtLogLevel, 0, value, null, (o, e) => (string) o);
            }
        }

    }
}
