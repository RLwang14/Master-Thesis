using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace HTTPClient
{
    class Program
    {
        static void Main(string[] args)
        {
            string clientNum = args[0];

            HTTPClientBasic client = new HTTPClientBasic(IPAddress.Parse(args[1]), int.Parse(args[2]), clientNum);
            client.Start();

            Console.ReadLine();
        }
    }
}
/*Connection-specific DNS Suffix: ent.bhicorp.com
Description: Intel(R) Ethernet Connection I219-LM
Physical Address: ‎28-F1-0E-41-F5-6D
DHCP Enabled: Yes
IPv4 Address: 10.49.137.96
IPv4 Subnet Mask: 255.255.255.0
Lease Obtained: Monday, September 2, 2019 9:56:24 AM
Lease Expires: Thursday, September 5, 2019 10:27:20 AM
IPv4 Default Gateway: 10.49.137.1
IPv4 DHCP Server: 10.49.100.150
IPv4 DNS Servers: 10.49.100.20, 147.108.144.20, 10.163.110.1, 10.163.110.2
IPv4 WINS Servers: 10.49.100.20, 147.108.144.20
NetBIOS over Tcpip Enabled: Yes
Link-local IPv6 Address: fe80::780c:1ea7:7f64:243e%13
IPv6 Default Gateway: 
IPv6 DNS Server: */