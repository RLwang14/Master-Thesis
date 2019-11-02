using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HTTPServer
{
    class Request
    {
        public String MethodType { get; set; }//GET POST PUT
        public String URL { get; set; }
        public String Host { get; set; }//localhost:8080
        public string body { get; set; }
        public string deviceName { get; set; }
        public string requestDataSize { get; set; }

        private Request(String methodType, String url,string body, string deviceName, string requestdataSize )
        {
            this.MethodType = methodType;
            this.URL = url;
            this.body = body;
            this.deviceName = deviceName;
            this.requestDataSize = requestdataSize;
        }     

        public static Request GetRequest(String request)
        {
            if (String.IsNullOrEmpty(request))
                return null;

            String[] tokens = request.Split(' ');
            String methodType = tokens[0];
            String url = tokens[1];

            string[] tokens3 = url.Split('/');
            string deviceName = tokens3[0];
            string requsetDataSize = tokens3[1];

            string[] stringSeparators = new string[] { "\r\n\r\n" };
            string[] tokens2 = request.Split(stringSeparators, StringSplitOptions.None);
            string body = tokens2[1];
           
            return new Request(methodType, url,body, deviceName, requsetDataSize);
        }
    }
}
