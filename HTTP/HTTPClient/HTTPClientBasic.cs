using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Monitor;

namespace HTTPClient
{
    class HTTPClientBasic
    {
        private TcpClient Client;
        public  IPAddress Address;
        public int Port;
        private String Name;
        private double hardness = 0;
        
        public HTTPClientBasic(IPAddress address, int port, String name)
        {
            this.Address = address;
            this.Port = port;
            this.Name = name;                    
        }

        private string GetRandom(string[] arr)
        {
            Random ran = new Random();
            int n = ran.Next(arr.Length - 1);

            return arr[n];
        }

        public HTTPClientBasic() { }

        public void Start()
        {
            Thread clientThread = new Thread(new ThreadStart(Run));
            clientThread.Start();
        }

        private void Run()
        {
            Client = new TcpClient();
            Client.Connect(Address, Port);
            Console.WriteLine("Connected and {0} is running....", Name);
            NetworkStream netstream = Client.GetStream();
            StopWatch st = new StopWatch("C:\\HTTP\\performanceHTTP_CSharp_Client_" + Name + ".log");


            //generate the request, write to the stream and send to the server
            int verb = 1;  //select a request type        ----------------------------------------------------change request type  
            while (true)
            {
                // string[] arrDevice = { "A", "B", "C", "D", "E" };
                // string deviceName = GetRandom(arrDevice);
                // string[] arrRequestSize = { "10", "100", "1000", "10000" };
                string requestDataSize = "10000";// GetRandom(arrRequestSize);                //------------------------------------change the request size

                if (verb == 0)
                {
                    OnGet(netstream, st, "A", requestDataSize);
                }

                if(verb == 1)
                {
                    string postValue = null;
                    Random rand = new Random();
                    int value = rand.Next(10);
                    for(int i = 1; i<= Int32.Parse(requestDataSize); i++)
                    {
                        postValue += value.ToString();
                    }

                    OnPOST(netstream, st, postValue, "A", requestDataSize);    
                }                          
            }


        }

        private void OnGet(NetworkStream networkStream, StopWatch st , string deviceName, string requestDataSize)
        {
            Request reqObj = new Request("GET",deviceName,requestDataSize,null);
            String req = reqObj.GenerateRequest();
            //send the request
            Byte[] reqByte = System.Text.Encoding.ASCII.GetBytes(req); //request send 
            string requestSize = reqByte.Length.ToString();//reqByte.Length;
            //log request send time
            networkStream.Write(reqByte,0,reqByte.Length);//write request to the stream
            st.Start();
            Console.WriteLine("\r\n" + " :GET got send\r\n{0}",req);

            //receive the response
            Byte[] respByte = new Byte[100000];
            String resp = String.Empty;
            //Read the data from stream
            Int32 bytes = networkStream.Read(respByte, 0, respByte.Length);//read response from the stream 
            st.Stop();
            resp = System.Text.Encoding.ASCII.GetString(respByte, 0, bytes);
            Response newResp = Response.GetResponse(resp);


            st.WriteLogClient("GET", deviceName, requestSize, Name, newResp.StatusCode);
            Console.WriteLine("\r\n"+":Got Response=======================================\r\n{0}",resp);


        }

        private void OnPOST(NetworkStream networkStream, StopWatch st,string postValue, string deviceName, string requestDataSize)
        {
            Request reqObj = new Request("POST", deviceName,requestDataSize,postValue);
            String req = reqObj.GenerateRequest();
            //send the request
            Byte[] reqByte = System.Text.Encoding.ASCII.GetBytes(req);
            string requestSize = reqByte.Length.ToString();//reqByte.Length;
            networkStream.Write(reqByte, 0, reqByte.Length);
            st.Start();
            Console.WriteLine("\r\n"  + " :POST got send\r\n{0}", req);

            //receive the response
            Byte[] respByte = new Byte[100000];
            String resp = String.Empty;
            //read the data from the stream
            Int32 bytes = networkStream.Read(respByte, 0, respByte.Length);
            st.Stop();
            resp = System.Text.Encoding.ASCII.GetString(respByte, 0, bytes);
            Response newResp = Response.GetResponse(resp);
            st.WriteLogClient("POST", deviceName, requestDataSize, Name, newResp.StatusCode);
            Console.WriteLine("\r\n" + " :Got Response=======================================\r\n{0}", resp);
        }
    }
}
