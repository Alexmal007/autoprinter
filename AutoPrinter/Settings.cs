using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using AutoPrinter.Controllers;
using System.Windows;

namespace AutoPrinter
{
    class Settings
    {
        public delegate void SettingsUpdatedHandler();
        public static event SettingsUpdatedHandler OnSettingsUpdatedEvent;

        public static string SelectedPrinter { get; set; }
        public static int SelectedPort { get; set; }
        public static string SelectedUrl { get; set; }

        public static void Save() {
            OnSettingsUpdatedEvent();
            File.WriteAllText("cfg/settings.cfg", $"port={SelectedPort}\r\ndevice={SelectedPrinter}\r\nurl={SelectedUrl}");
        }

        public static void Load() {
            if (!Directory.Exists("cfg")) { Directory.CreateDirectory("cfg"); }
            if (!Directory.Exists("logs")) { Directory.CreateDirectory("logs"); }
            if (!Directory.Exists("temp")) { Directory.CreateDirectory("temp"); }

            if (File.Exists("cfg/settings.cfg"))
            {
                var storedSettings = new Dictionary<string, string>();
                var selectedRows = File.ReadAllLines("cfg/settings.cfg").ToList();

                foreach (string row in selectedRows)
                {
                    var key = row.Split('=')[0].Trim();
                    var val = row.Split('=')[1].Trim();
                    if (!string.IsNullOrWhiteSpace(key) && !string.IsNullOrWhiteSpace(val))
                    {
                        storedSettings.Add(key, val);
                    }
                }
                

                try
                {
                    SelectedPort = storedSettings.ContainsKey("port") ? int.Parse(storedSettings["port"]) : 25120;
                    SelectedPrinter = storedSettings.ContainsKey("device") ? storedSettings["device"] : PrinterController.GetDefaultPrinter();
                    SelectedUrl = storedSettings.ContainsKey("url") ? storedSettings["url"] : "https://alexmal.ru/auto_print/test.png";
                }
                catch (FormatException ignore)
                {
                    
                }
            }
            else
            {
                SelectedPort = 25120;
                SelectedPrinter = PrinterController.GetDefaultPrinter();
                SelectedUrl = "https://alexmal.ru/auto_print/test.png";
                File.WriteAllText("cfg/settings.cfg", $"port={SelectedPort}\r\ndevice={SelectedPrinter}\r\nurl={SelectedUrl}");
            }
        }
    }
}
