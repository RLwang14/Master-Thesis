using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HTTPClient
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

            String StatusCode = tokens[1];

            String StatusMsg = tokens[2];
           
         
            return new Response(StatusCode, StatusMsg);
        }
    }
}

