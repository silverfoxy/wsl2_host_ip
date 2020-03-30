using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.NetworkInformation;
using Microsoft.Extensions.Logging;

namespace wslw_host_ip
{
    class Program
    {
        const String hosts_file = "C:\\Windows\\System32\\drivers\\etc\\hosts";
        // const String hosts_file = "C:\\Users\\baminazad\\Documents\\hosts";
        const String wsl_host_entry = "wsl2.host";
        static void Main(string[] args)
        {
            var program = new Program();
            String wsl_ip = program.getWSLIP();
            program.updateHosts(wsl_ip);
        }

        String getWSLIP() 
        {
            foreach(NetworkInterface ni in NetworkInterface.GetAllNetworkInterfaces())
            {
                if(ni.NetworkInterfaceType == NetworkInterfaceType.Wireless80211 || ni.NetworkInterfaceType == NetworkInterfaceType.Ethernet)
                {
                    if (ni.Name == "vEthernet (WSL)") 
                    {
                        foreach (UnicastIPAddressInformation ip in ni.GetIPProperties().UnicastAddresses)
                        {
                            if (ip.Address.AddressFamily == System.Net.Sockets.AddressFamily.InterNetwork)
                            {
                                return ip.Address.ToString();
                            }
                        }
                    }
                    
                }  
            }
            return "NA";
        }

        void updateHosts(String wsl_ip) 
        {
            bool entry_exists = false;
            String[] lines = File.ReadLines(hosts_file).ToArray();
            for (int i = 0; i < lines.Length; i++)
            {
                if (lines[i].Contains(wsl_host_entry))
                {
                    lines[i] = wsl_ip + "\t" + wsl_host_entry;
                    entry_exists = true;
                    break;
                }
            }
            List<String> entries = lines.ToList();
            if (!entry_exists)
            {
                entries.Add(wsl_ip + "\t" + wsl_host_entry);
            }
            try
            {
                File.WriteAllLines(hosts_file, entries);
            }
            catch (System.UnauthorizedAccessException)
            {
                var loggerFactory = LoggerFactory.Create(builder =>
                {
                    builder
                        .AddFilter("Microsoft", LogLevel.Warning)
                        .AddFilter("System", LogLevel.Warning)
                        .AddFilter("LoggingConsoleApp.Program", LogLevel.Warning)
                        .AddEventLog();
                });
                ILogger logger = loggerFactory.CreateLogger<Program>();
                logger.LogError("Access denied when updating hosts file.");
                return;
            }
        }
    }
}
