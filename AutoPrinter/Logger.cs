using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace AutoPrinter
{
    class Logger
    {
        public static void Info(string msg)
        {
            var date = DateTime.Now;
            var dateLogName = $"{date.Day}-{date.Month}-{date.Year}";
            var fullStr = $"[{date.ToLongTimeString()}] {msg}\r\n";
            File.AppendAllText($"logs/{dateLogName}.log", fullStr);
        }
    }
}
