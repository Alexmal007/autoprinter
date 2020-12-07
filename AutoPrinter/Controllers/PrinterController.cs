using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Drawing.Printing;
using System.IO;
using System.Linq;
using System.Printing;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace AutoPrinter.Controllers
{
    class PrinterController
    {
        
        public static void Print(string filename, EventHandler CallBack) {
            try
            {
                var printFont = new Font("Arial", 10);
                PrintDocument pd = new PrintDocument();
                var x = pd.PrinterSettings.DefaultPageSettings.PrintableArea.Width;
                var y = pd.PrinterSettings.DefaultPageSettings.PrintableArea.Height;
                pd.Disposed += CallBack;
                pd.PrintPage += (sender, args) =>
                {
                    System.Drawing.Image i = System.Drawing.Image.FromFile(filename);
                    System.Drawing.Point p = new System.Drawing.Point(100, 100);
                    //args.Graphics.DrawImage(i, 10, 10, i.Width, i.Height);
                    args.Graphics.DrawImage(i, 10, 10, x, y);
                };
                pd.Print();
                pd.Dispose();
                //CallBack(null, new PrintEventArgs());
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        public static void PrintImage(string file)
        {
            PrintDocument pd = new PrintDocument();
            pd.PrintPage += (sender, e) =>
            {
                var _pd = sender as PrintDocument;
                pd.PrinterSettings.PrinterName = Settings.SelectedPrinter;
                var img = new Bitmap(file);
                //Rectangle m = new Rectangle(0, 0, (int)_pd.PrinterSettings.DefaultPageSettings.PrintableArea.Height, (int)_pd.PrinterSettings.DefaultPageSettings.PrintableArea.Width);
                Rectangle m = new Rectangle(0, 0, (int)_pd.PrinterSettings.DefaultPageSettings.PrintableArea.Width, (int)_pd.PrinterSettings.DefaultPageSettings.PrintableArea.Height);
                e.Graphics.DrawImage(img, m);
            };
            pd.Print();
        }

        public static List<string> FindDevices()
        {
            var installedPrinters = PrinterSettings.InstalledPrinters.Cast<string>().ToList();
            return installedPrinters;
        }

        public static string GetDefaultPrinter()
        {
            var printers = FindDevices();
            var defaultName = "";
            foreach (string name in printers)
            {
                var ps = new PrinterSettings();
                ps.PrinterName = name;
                if (ps.IsDefaultPrinter)
                {
                    defaultName = name;
                    return defaultName;
                }
            }
            return defaultName;
        } 
    }
}
