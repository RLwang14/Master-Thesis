using System;
using Com.AugustCellars.CoAP.Examples.Resources;
using Com.AugustCellars.CoAP.Server;

namespace Com.AugustCellars.CoAP.Examples
{
    public class ExampleServer
    {
        public static void Main(String[] args)
        {
            string ipadress = args[0];
            string port = args[1];
            int portInt = Int32.Parse(port);

            // System.Net.EndPoint endpoint= System.Net.EndPoint  (ipadress, portInt);

            //System.Net.EndPoint endPoint = System.Net.IPAddress.Parse(ipadress);
            System.Net.IPAddress ip = System.Net.IPAddress.Parse(ipadress);
            CoapServer server = new CoapServer();
            server.AddEndPoint(ip, portInt);
            
            server.Add(new HelloWorldResource("hello"));
            server.Add(new FibonacciResource("fibonacci"));
            server.Add(new StorageResource("Devices"));
            server.Add(new ImageResource("image"));
            server.Add(new MirrorResource("mirror"));
            server.Add(new LargeResource("large"));
            server.Add(new CarelessResource("careless"));
            server.Add(new SeparateResource("separate"));
            server.Add(new TimeResource("time"));

            try
            {
                server.Start();
                Console.Write("CoAP server [{0}] is listening on", server.Config.Version);

                foreach (var item in server.EndPoints)
                {
                    Console.Write(" ");
                    Console.Write(item.LocalEndPoint);
                }
                Console.WriteLine();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }

            Console.WriteLine("Press any key to exit.");
            Console.ReadKey();
        }
    }
}
