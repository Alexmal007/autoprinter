using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.IO;
using System.Drawing.Printing;
using System.Net;
using System.Net.Sockets;
using System.Windows;

namespace AutoPrinter.Controllers
{
    class ServerController
    {
        public delegate void FileDownloaded();
        public static event FileDownloaded OnFileDownloadedEvent;

        private static bool _stopServer = false;

        public static void Init()
        {
            if (!Directory.Exists("print"))
            {
                Directory.CreateDirectory("print");
            }

            var serverListener = Task.Run(() => Listen());
        }

        private static void Listen()
        {
            try
            {
                _stopServer = false;
                var port = Settings.SelectedPort;
                IPAddress localhost = IPAddress.Parse("127.0.0.1");
                var server = new TcpListener(localhost, port);

                server.Start();

                var buffer = new byte[256];
                string data = null;

                Logger.Info("Launching the server...");

                while (!_stopServer)
                {
                    Logger.Info($"Waiting for connection at port {port}...");
                    var client = server.AcceptTcpClient();

                    data = null;
                    var stream = client.GetStream();
                    int i;
                    while ((i = stream.Read(buffer, 0, buffer.Length)) != 0)
                    {
                        data = Encoding.ASCII.GetString(buffer, 0, i);
                        Logger.Info($"Incoming message: {data}");

                        var response = Encoding.ASCII.GetBytes("Received!");

                        stream.Write(response, 0, response.Length);
                        DownloadImage();
                    }
                    client.Close();
                }

                server.Stop();
                server.Server.Dispose();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Server crashed!");
                Logger.Info($"Server crashed with following reason: {ex.Message}");
            }
        }

        public static void Reboot()
        {
            _stopServer = true;
            Init();
        }

        private static void DownloadImage()
        {
            using (var client = new WebClient())
            {
                client.DownloadFile(Settings.SelectedUrl, "temp/process_file.png");
            }
            OnFileDownloadedEvent();
        }
    }
}
