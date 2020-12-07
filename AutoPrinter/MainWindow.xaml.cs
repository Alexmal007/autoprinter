using AutoPrinter.Controllers;

using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;


namespace AutoPrinter
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    /// 
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();

            Settings.Load();
            ServerController.Init();

            Settings.OnSettingsUpdatedEvent += ServerController.Reboot;
            ServerController.OnFileDownloadedEvent += () => { PrinterController.PrintImage("temp/process_file.png"); };

            SettingsReset_Click(null, null);

            apNotifyIcon.TrayPopup = new Popup();
        }

        private void ApNotifyIcon_TrayMouseDoubleClick(object sender, RoutedEventArgs e)
        {
            AppController.OpenSettings();
        }

        private void SettingsSave_Click(object sender, RoutedEventArgs e)
        {
            var printer = deviceSelect.Text.Trim();
            var port = int.Parse(portSelect.Text.Trim());
            var url = urlSelect.Text.Trim();
            Settings.SelectedPrinter = printer;
            Settings.SelectedPort = port;
            Settings.SelectedUrl = url;
            Settings.Save();
        }

        private void SettingsReset_Click(object sender, RoutedEventArgs e)
        {
            var printer = Settings.SelectedPrinter;
            var port = Settings.SelectedPort;
            var url = Settings.SelectedUrl;

            var devices = PrinterController.FindDevices();
            foreach (string device in devices)
            {
                deviceSelect.Items.Add(device);
            }
            portSelect.Text = port.ToString();
            urlSelect.Text = url;
            deviceSelect.Text = printer;
        }

        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            e.Cancel = true;
            AppController.OpenSettings();
        }
    }
}
