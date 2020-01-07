using System;
using System.Net;
using System.Net.Sockets;
using System.Threading;
using Monitor;

namespace RestfulOPCUAClient
{
    class OPCUAClientBasic
    {
        private TcpClient Client;
        public  IPAddress Address;
        public int Port;
        private String Name;
        private double hardness = 0;
        
        public OPCUAClientBasic(IPAddress address, int port, String name)
        {
            this.Address = address;
            this.Port = port;
            this.Name = name;                    
        }
        public OPCUAClientBasic()
        {

        }

        private string GetRandom(string[] arr)
        {
            Random ran = new Random();
            int n = ran.Next(arr.Length - 1);

            return arr[n];
        }

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
            StopWatch st = new StopWatch("C:\\OPCUA\\performanceOPCUA_CSharp_Client_" + Name + ".log");

            int verb = 1;
            string requestDataSize = "2000";
            string nodeID = "ns=2;i="+requestDataSize;      //------------------------------------change the request size     
          
            //generate the request, write to the stream and send to the server
            while (true)
            {
                // string[] arrDevice = { "A", "B", "C", "D", "E" };
                // string deviceName = GetRandom(arrDevice);
                // string[] arrRequestSize = { "10", "100", "1000", "10000" };
               // GetRandom(arrRequestSize);                
                if(verb == 0)
                {
                    OnPOST(netstream, st, null, "A", nodeID,"Read", requestDataSize);
                }

                if(verb == 1)
                {
                    string postValue = GenerateRequestData(Int32.Parse(requestDataSize));
                    OnPOST(netstream, st, postValue, "A", nodeID, "Write", requestDataSize);
                }                                                         
            }


        }
        private static string GenerateRequestData(int size)
        {
            string responseData = null;
            Random rd = new Random();
            int randomValue = rd.Next(10);
            for (int i = 1; i <= size; i++)
            {
                responseData += randomValue.ToString();
            }
            return responseData;
        }
        private void OnPOST(NetworkStream networkStream, StopWatch st,string postValue, string deviceName, string nodeID,string methodType, string requestDataSize)
        {
            Request reqObj = new Request(methodType, nodeID ,postValue);
            String req = reqObj.GenerateRequest();
            //send the request
            Byte[] reqByte = System.Text.Encoding.ASCII.GetBytes(req);
            string requestSize = reqByte.Length.ToString();//reqByte.Length;
              networkStream.Write(reqByte, 0, reqByte.Length);
            st.Start();
            Console.WriteLine("\r\n"  + " :POST got send\r\n{0}", req);

            //receive the response
            Byte[] respByte = new Byte[1000000];
            String resp = String.Empty;
            //read the data from the stream
            Int32 bytes = networkStream.Read(respByte, 0, respByte.Length);
            st.Stop();
            resp = System.Text.Encoding.ASCII.GetString(respByte, 0, bytes);
            Response newResp = Response.GetResponse(resp);
            st.WriteLogClient(methodType, deviceName, requestDataSize, Name, newResp.StatusCode);
            Console.WriteLine("\r\n" + " :Got Response=======================================\r\n{0}", resp);
        }
    }
}
