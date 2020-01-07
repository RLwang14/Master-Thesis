using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;
using Monitor;

namespace OPCUAServer
{
    class Response
    {
        private String StatusCode;
        private String Data;
        private String StatusMsg;
        private String ContentType;
        private static string value;
        private static string packageNumber;

        public String body { get { return Data; } }

        private Response(String statusCode, String statusMsg, String contentType, String data)
        {
            this.Data = data;
            this.StatusCode = statusCode;
            this.StatusMsg = statusMsg;
            this.ContentType = contentType;
            
        }

        private Response(String statusCode, String statusMsg, string data)
        {
            this.Data = data;
            this.StatusCode = statusCode;
            this.StatusMsg = statusMsg;
        }
        

        

        public static Response From(RequestManager request, NodeManager nodeManager)
        {
            if (request == null)
                return MakeNullRequest();

            string nodeID = request.nodeID;

            if (request.MethodType == "action=Read")
            {
                //Devices di = new Devices(request.deviceName, request.requestDataSize, request.MethodType,null);

               string responseData = nodeManager.ReadNode(nodeID);
               return new Response("200", "OK", responseData);

            }
            else if(request.MethodType == "action=Write")
            {
                // string body = request.body;
                //   updateData(body);
                //Devices di = new Devices(request.deviceName, request.requestDataSize, request.MethodType, body); 
                string valueToWrite = request.value;
                nodeManager.WriteNode(nodeID, valueToWrite);
                return new Response("200", "OK", null);
            }
            else
            {
                return MakeMethodNotAllowed();
            }
            

        }

        private static Response MakeNotFound()
        {
            return new Response("404", "Not allowed", "text/html", null);
        }

        private static Response MakeMethodNotAllowed()
        {
            return new Response("405", "Method not allowed", "text/html", null);
        }

        private static Response MakeNullRequest()
        {
            return new Response("400", "Bad Request", "text/html", null);
        }

        public Byte[] SendResponse(NetworkStream stream,string method)
        {
            DateTime currentTime = new DateTime();
            currentTime = DateTime.Now;
            byte[] response = null;
            string responseString = null;


            if (method == "action=Read")
            {
               responseString = String.Format("{0} {1} {2}\r\nContent-Type: application/http+xml; charset=utf-8; action=ReadResponse\r\nContent-Length: {3}\r\n\r\n{4}",
               OPCUAServerBasic.VERSION, StatusCode, StatusMsg, Data.Length,Data);
               response = System.Text.Encoding.ASCII.GetBytes(responseString);
            }
               
            if(method == "action=Write")
            {

               responseString = String.Format("{0} {1} {2}\r\nContent-Type: application/http+xml; charset=utf-8; action=WriteResponse",
                    OPCUAServerBasic.VERSION, StatusCode, StatusMsg);
               response = System.Text.Encoding.ASCII.GetBytes(responseString);
            }

            stream.Write(response, 0, response.Length);
            Console.WriteLine(method + " :Response got sent");
            return response;
        }
    }
}
