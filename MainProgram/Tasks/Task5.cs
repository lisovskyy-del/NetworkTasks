using System;
using System.Collections.Generic;
using System.Diagnostics.Metrics;
using System.Net;
using System.Text;

namespace MainProgram.Tasks;

class Task5
{
    public static void Run()
    {
        string ipInput = InputHelpers.StringInput("Введіть IP-адресу: ");

        int cidr = InputHelpers.IntInput("Введіть маску підмережі (CIDR): ");

        IPAddress ip = IPAddress.Parse(ipInput);

        byte[] ipBytes = ip.GetAddressBytes();
        byte[] maskBytes = CidrToMask(cidr);

        byte[] network = new byte[4];
        byte[] broadcast = new byte[4];

        for (int i = 0; i < 4; i++)
        {
            network[i] = (byte)(ipBytes[i] & maskBytes[i]);
        }

        for (int i = 0; i < 4; i++)
        {
            broadcast[i] = (byte)(network[i] | (~maskBytes[i] & 255));
        }

        string firstHost = Increment(network);
        string lastHost = Decrement(broadcast);

        int hostBits = 32 - cidr;
        int hosts = (int)Math.Pow(2, hostBits) - 2;

        Console.WriteLine("\nРезультати:");
        Console.WriteLine("IP-адреса: " + ipInput);
        Console.WriteLine("Маска підмережі: " + ToIP(maskBytes));
        Console.WriteLine("Мережева адреса: " + ToIP(network));
        Console.WriteLine("Broadcast адреса: " + ToIP(broadcast));
        Console.WriteLine("Перший хост: " + firstHost);
        Console.WriteLine("Останній хост: " + lastHost);
        Console.WriteLine("Кількість хостів: " + hosts);
        Console.WriteLine("Клас мережі: " + GetClass(ipBytes[0]));
    }

    static byte[] CidrToMask(int cidr)
    {
        uint mask = 0xFFFFFFFF << (32 - cidr);

        return new byte[]
        {
            (byte)((mask >> 24) & 255),
            (byte)((mask >> 16) & 255),
            (byte)((mask >> 8) & 255),
            (byte)(mask & 255)
        };
    }

    static string ToIP(byte[] ip)
    {
        return string.Join(".", ip);
    }

    static string Increment(byte[] ip)
    {
        byte[] copy = (byte[])ip.Clone();
        copy[3]++;
        return ToIP(copy);
    }

    static string Decrement(byte[] ip)
    {
        byte[] copy = (byte[])ip.Clone();
        copy[3]--;
        return ToIP(copy);
    }

    static int CountHostBits(byte[] mask)
    {
        int count = 0;

        foreach (byte b in mask)
        {
            count += CountZeros(b);
        }

        return count;
    }

    static int CountZeros(byte b)
    {
        int c = 0;

        for (int i = 0; i < 8; i++)
        {
            if ((b & (1 << i)) == 0)
                c++;
        }

        return c;
    }

    static string GetClass(byte firstOctet)
    {
        if (firstOctet <= 126) return "A";
        if (firstOctet <= 191) return "B";
        if (firstOctet <= 223) return "C";
        return "Unknown";
    }
}