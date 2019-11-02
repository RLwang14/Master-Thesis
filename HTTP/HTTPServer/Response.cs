using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;
using Monitor;

namespace HTTPServer
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

        private Response(String statusCode, String statusMsg, String data)
        {
            this.Data = data;
            this.StatusCode = statusCode;
            this.StatusMsg = statusMsg;

        }
        private static string GenerateResponseData(int size)
        {
            string responseData = null;
            for (int i = 1; i <= size; i++)
            {
                responseData += "1";
            }
            return responseData;
        }
        private static void updateData(string value)
        {
            string data = value;
        }

        

        public static Response From(Request request)
        {
            if (request == null)
                return MakeNullRequest();           

            if (request.MethodType == "GET")
            {
                //Devices di = new Devices(request.deviceName, request.requestDataSize, request.MethodType,null);

                string responseData = GenerateResponseData(Int32.Parse(request.requestDataSize));
                return new Response("200", "OK", responseData);

            }
            else if(request.MethodType == "POST")
            {
                string body = request.body;
                updateData(body);
                //Devices di = new Devices(request.deviceName, request.requestDataSize, request.MethodType, body);               
                return new Response("200", "OK", null);
            }
            else
            {
                return MakeMethodNotAllowed();
            }
            
            return MakeNotFound();


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


            if (method == "GET")
            {
               responseString = String.Format("{0} {1} {2}\r\n\r\n{3}",
               HTTPServerBasic.VERSION, StatusCode, StatusMsg, Data);
               response = System.Text.Encoding.ASCII.GetBytes(responseString);
            }
               
            if(method == "POST")
            {

               responseString = String.Format("{0} {1} {2}\r\n",
                    HTTPServerBasic.VERSION, StatusCode, StatusMsg);
               response = System.Text.Encoding.ASCII.GetBytes(responseString);
            }

            stream.Write(response, 0, response.Length);
            Console.WriteLine(method + " :Response got sent");
            return response;
        }
    }
}
