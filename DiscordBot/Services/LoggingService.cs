using Discord;
using Discord.Commands;
using Discord.WebSocket;
using System;
using System.IO;
using System.Threading.Tasks;

namespace DiscordBot
{
    class LoggingService
    {
        private readonly DiscordSocketClient _discord;
        private readonly CommandService _commands;
        private readonly object _mutex;

        private string _logDirectory { get; }
        private string _logFile => Path.Combine(_logDirectory, $"{DateTime.UtcNow.ToString("yyyy-MM-dd")}.txt");

        public LoggingService(DiscordSocketClient discord, CommandService commands)
        {
            _logDirectory = Path.Combine(AppContext.BaseDirectory, "logs");

            _discord = discord;
            _commands = commands;
            _mutex = new object();

            _discord.Log += OnLogAsync;
            _commands.Log += OnLogAsync;
        }

        private Task OnLogAsync(LogMessage msg)
        {
            string logText;
            lock (_mutex)
            {
                if (!Directory.Exists(_logDirectory))
                    Directory.CreateDirectory(_logDirectory);
                if (!File.Exists(_logFile))
                    File.Create(_logFile).Dispose();

                logText = $"{DateTime.UtcNow.ToString("hh:mm:ss")} [{msg.Severity}] {msg.Source}: {msg.Exception?.ToString() ?? msg.Message}";
                File.AppendAllText(_logFile, logText + "\n");
            }

            return Console.Out.WriteLineAsync(logText);
        }
    }
}
