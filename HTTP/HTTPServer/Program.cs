using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HTTPServer
{
	class Program
	{
		static void Main(string[] args)
		{

			HTTPServerBasic server = new HTTPServerBasic(args[0], int.Parse(args[1]));
			server.Start();

			Console.ReadLine();


		}
	}
}
