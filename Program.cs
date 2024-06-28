using System.Diagnostics;
using System.Net;
using System.Reflection;
using System.Reflection.Metadata;
using System.Runtime.InteropServices;
using static System.Net.Mime.MediaTypeNames;
using System.Runtime.InteropServices;

namespace Minecraft_server_quick_install
{
    
    internal static class Program
    {
        private static int port;
        private static Process proc;
        static void Main(string[] args)
        {
            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine(" __                                 _              _          _  _          _ \r\n/ _\\  ___  _ __ __   __ ___  _ __  (_) _ __   ___ | |_  __ _ | || | __   __/ |\r\n\\ \\  / _ \\| '__|\\ \\ / // _ \\| '__| | || '_ \\ / __|| __|/ _` || || | \\ \\ / /| |\r\n_\\ \\|  __/| |    \\ V /|  __/| |    | || | | |\\__ \\| |_| (_| || || |  \\ V / | |\r\n\\__/ \\___||_|     \\_/  \\___||_|    |_||_| |_||___/ \\__|\\__,_||_||_|   \\_/  |_|");
           
            Console.WriteLine("For more information visit: ");
            if (File.Exists(GetCurrentPath() + "\\server.jar"))
            {
                StartServer();
            }
            else 
            {
                 DownloadServer();
            }
            Console.Clear();
            
            Console.WriteLine("Server started on address: " + Upnp.GetPublicIP() + ":" + port + "\n You can close this window");
            while (true) ;

        }

        static void StartServer() 
        {
            Console.WriteLine("Server detected. Starting port forwarding, then starting server \n \"Press any key to start\"");
            Console.ReadKey();
            port = 25565;


            Upnp.Setup();
          
            while (!Upnp.IsDeviceFound) ; // wait for Upnp to connect to router //
           
            if (!Upnp.IsPortForwarded(port)) 
            {
             
                Console.WriteLine("Port forward not detected, opening new port");
                Upnp.PortForward(port, port);
            }
            Console.WriteLine("Port forward detected");


            StartCommand();
           
        }

        static void DownloadServer() 
        {
            using (var client = new WebClient())
            {
                client.DownloadFile("https://download.getbukkit.org/spigot/spigot-1.21.jar", GetCurrentPath()+ "\\server.jar");
            }
            while (!File.Exists(GetCurrentPath()+ "\\server.jar")) ; // wait for file download //

            StartServer();

        }

        async static void StartCommand()
        { 

            ProcessStartInfo procStartInfo = new ProcessStartInfo("java")
            {
                // Set up the arguments for the command.
                Arguments = $"-Xmx1024M -Xms1024M -Dcom.mojang.eula.agree=true -jar \"{GetCurrentPath()}\"\\server.jar nogui pause",
                UseShellExecute = true,
                RedirectStandardOutput = false,
                RedirectStandardError = false,
                CreateNoWindow = false
            };


            proc = new Process
            {
                StartInfo = procStartInfo
            };
            
            try
            {

                proc.Start();


                string resultOfCommand = proc.StandardOutput.ReadLine();
               Console.WriteLine(resultOfCommand);
                
                string error = proc.StandardError.ReadToEnd();


                Console.WriteLine(resultOfCommand);
                if (!string.IsNullOrEmpty(error))
                {
                    Console.WriteLine("Error: " + error);
                }
                while (true)
                {
                    Console.WriteLine(resultOfCommand);
                }
               
            }
            catch (Exception e)
            {

                Console.WriteLine("Exception: " + e.Message);
            }

           
        }

        static String GetCurrentPath() 
        {
            return Directory.GetCurrentDirectory();
        }

       


    }
}