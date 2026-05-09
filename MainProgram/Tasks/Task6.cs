using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Net.NetworkInformation;
using System.Text;
using System.Text.RegularExpressions;

namespace MainProgram.Tasks;

class Task6
{
    public static void Run()
    {
        string baseIp = "192.168.1.";

        List<string> activeHosts = new List<string>();

        Console.WriteLine("Скануємо мережу...\n");

        for (int i = 1; i <= 254; i++)
        {
            string ip = baseIp + i;

            Ping ping = new Ping();
            Stopwatch sw = new Stopwatch();

            try
            {
                sw.Start();
                PingReply reply = ping.Send(ip, 200);
                sw.Stop();

                if (reply.Status == IPStatus.Success)
                {
                    string mac = GetMac(ip);

                    Console.WriteLine($"{ip} - АКТИВНИЙ - RTT: {reply.RoundtripTime} ms - MAC: {mac}");
                    activeHosts.Add(ip);
                }
            }
            catch
            {
                // ігнор
            }
        }

        Console.WriteLine("\nРезультат:");
        Console.WriteLine("Активні хости: " + activeHosts.Count);
    }

    static string GetMac(string ip)
    {
        try
        {
            System.Diagnostics.Process p = new System.Diagnostics.Process();
            p.StartInfo.FileName = "arp";
            p.StartInfo.Arguments = "-a";
            p.StartInfo.RedirectStandardOutput = true;
            p.StartInfo.UseShellExecute = false;
            p.StartInfo.CreateNoWindow = true;
            p.Start();

            string output = p.StandardOutput.ReadToEnd();
            p.WaitForExit();

            // find MAC for IP
            var match = Regex.Match(output,
                ip + @"\s+([0-9a-fA-F\-]{17})");

            if (match.Success)
                return match.Groups[1].Value;
        }
        catch { }

        return "unknown";
    }
}