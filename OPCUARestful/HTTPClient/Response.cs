using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RestfulOPCUAClient
{
    class Response
    {
        public String StatusCode { get; set; }
        public String Data { get; set; }
        public String StatusMsg { get; set; }
        public string ContentType { get; set; }

        public string PackageNumber { get; set; }
        private Response(String StatusCode, String StatusMsg)
        {
            this.StatusCode = StatusCode;
            this.StatusMsg = StatusMsg;
            
        }

        public static Response GetResponse(String response)
        {
            if (String.IsNullOrEmpty(response))
                return null;

            String[] tokens = response.Split(' ');
            String StatusCode;
            String StatusMsg;
            if (tokens.Length == 1)
            {
                StatusCode = "404";
                StatusMsg = "Bad";


            }
            else
            {
                StatusCode = tokens[1];

                StatusMsg = tokens[2];

            }



            return new Response(StatusCode, StatusMsg);
        }
    }
}

