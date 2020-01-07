using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RestfulOPCUAClient
{
    class Request
    {
        private String MethodType;
        private string Data;
        private string requestSize;
        private string _nodeID;

        public Request(String methodType, string nodeID, string data)
        {
            this.MethodType = methodType;
            this.Data = data;
            this._nodeID = nodeID;
        }

        public String GenerateRequest()
        {
            OPCUAClientBasic clientBasic = new OPCUAClientBasic();
            String requestClient = null;
            string requestbody = _nodeID + " " + Data;

            if (MethodType == "Read")
            {
                requestClient = String.Format("POST {0} {1}\r\nContent-type: {2}\r\nContent-length:{3}\r\n\r\n{4}", //host = clientBasic.Address + ":" + clientBasic.Port
                                                    "/OPCUAServer", "HTTP/1.1", "application/http+xml; charset=utf-8; action=Read", requestbody.Length, requestbody);
            }
            else if(MethodType == "Write")
            {
                requestClient = String.Format("POST {0} {1}\r\nContent-type: {2}\r\nContent-length:{3}\r\n\r\n{4}", //host = clientBasic.Address + ":" + clientBasic.Port
                                                     "/OPCUAServer", "HTTP/1.1",  "application/http+xml; charset=utf-8; action=Write", requestbody.Length, requestbody);
            }
                           
          return requestClient;
        }
    }
}
