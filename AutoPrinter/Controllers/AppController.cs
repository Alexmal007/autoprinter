using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace AutoPrinter.Controllers
{
    class AppController
    {
        public static void OpenSettings()
        {
            var window = App.Current.MainWindow;

            if (window.IsVisible && window.WindowState != WindowState.Minimized)
            {
                window.Visibility = Visibility.Collapsed;
            }
            else
            {
                window.Visibility = Visibility.Visible;
                window.WindowState = WindowState.Normal;
                window.Activate();
            }
        }
    }
}
