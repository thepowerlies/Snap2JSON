using System;
using System.Diagnostics;
using System.IO;
using System.Threading.Tasks;

namespace Snap2Json.Log
{
    public class Logger
    {
        private static readonly string LogDirectoryPath = $"{Directory.GetCurrentDirectory()}\\log";
        private static readonly string LogFilePath = $"{LogDirectoryPath}\\error.log";
        private static bool _appendLogFlag = true;

        private static StreamWriter _writer;

        public static async Task LogAsync(string text)
        {
            if (!Directory.Exists(LogDirectoryPath))
            {
                Directory.CreateDirectory(LogDirectoryPath);
            }
            
            _writer = new StreamWriter(LogFilePath, _appendLogFlag);
            
            await _writer.WriteLineAsync($"{DateTime.Now.ToShortDateString()} : {text}{_writer.NewLine}----------------------------------------------------------------------");
            Debug.WriteLine(($"{DateTime.Now.ToShortDateString()} : {text}{_writer.NewLine}----------------------------------------------------------------------"));

            await _writer.FlushAsync();
        }
        

        public static async Task LogExceptionAsync(Exception e)
        {
            try
            {
                var message = $"{e.Message}\r\n{e.StackTrace}";
                await LogAsync(message);
            }
            catch (Exception exception)
            {
                Console.WriteLine(exception);
                throw;
            }
           
        }
    }
}