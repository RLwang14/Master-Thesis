using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OPCUAServer
{
    class RequestManager
    {
        public String MethodType { get; set; }//GET POST PUT
        public String nodeID { get; set; }
        public String value { get; set; }//localhost:8080    
        public string Encoding { get; set; }

        private RequestManager(String methodType, String nodeID, string value, string encoding)
        {
            this.MethodType = methodType;
            this.nodeID = nodeID;
            this.value = value;     
            this.Encoding = encoding;
        }     

        public static RequestManager GetRequest(String request)
        {
            if (String.IsNullOrEmpty(request))
                return null;

            string[] stringSeparators = new string[] { "\r\n\r\n" };
            string[] tokens = request.Split(stringSeparators, StringSplitOptions.None);
            string header = tokens[0];
            string body = tokens[1];

            string[] stringSeparators1 = new string[] { "\r\n" };
            string[] tokens1 = header.Split(stringSeparators1, StringSplitOptions.None);
            string requestLine = tokens1[0];
            string ContentType = tokens1[1];

            string[] tokens2 = ContentType.Split(' ');
            string encodingType = tokens2[1];
            string charset = tokens2[2];
            string methodType = tokens2[3];

            string[] tokens3 = body.Split(' ');
            string nodeID = tokens3[0];
            string valueToWrite = tokens3[1];

            return new RequestManager(methodType, nodeID, valueToWrite,encodingType);
        }
    }
}
