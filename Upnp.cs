using Mono.Nat;
using System.Net.NetworkInformation;

namespace Minecraft_server_quick_install
{
    using Microsoft.Win32.SafeHandles;
    using Mono.Nat;
    public static class Upnp
    {
        private static Mono.Nat.INatDevice device;
        public static bool IsDeviceFound=false;
        public static async void Setup() 
        {
            Console.WriteLine("Starting Setup");
            NatUtility.DeviceFound += DeviceFound;
            Console.WriteLine("Starting Discovery");
           

            try
            {
                NatUtility.StartDiscovery();
               
            }
            catch (Exception ex)
            {
                Console.WriteLine("ERROR: " + ex.ToString());
                Console.WriteLine("Press any key to attempt again");
                Console.ReadKey();
                Setup();
            }
        
                      
        }

        public static string GetPublicIP() 
        {
           return device.GetExternalIP().ToString();
        }

        public static bool IsPortForwarded(int portToCheck) 
        {
           Mapping[] allPortForwards = device.GetAllMappings();
            foreach (Mapping mapping in allPortForwards) 
            {
                if (mapping.PublicPort == portToCheck)
                {
                    return true;
                }
            }
            return false;
        }
        private static void DeviceFound(object sender, DeviceEventArgs args) 
        {
           device = args.Device;
            Console.WriteLine("Router upnp found!");
          IsDeviceFound = true;
            Console.WriteLine(IsDeviceFound);
        
        }

        public static Mapping PortForward(int PortExternal, int PortInternal)
        {
            try
            {
                Mapping mapping = new Mapping(Protocol.Tcp, PortInternal, PortExternal);

                device.CreatePortMap(mapping);
                return mapping;
            } catch (Exception ex) 
            {
                Console.WriteLine("Error when creating port forward " + ex);
                Console.WriteLine("Press any key to attempt again");
                Console.ReadKey();
                PortForward(PortExternal, PortInternal);
                return null;
            }

        }

        public static void PortForwardDelete(Mapping PortToDel) 
        {
            device.DeletePortMap(PortToDel);
        }


       
    }
}