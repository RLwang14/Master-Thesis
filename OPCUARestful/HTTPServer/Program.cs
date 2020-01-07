using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OPCUAServer
{
	class Program
	{
		static void Main(string[] args)
		{           
            OPCUAServerBasic server = new OPCUAServerBasic(args[0], int.Parse(args[1]));
			server.Start();
			Console.ReadLine();
		}
	}
}
