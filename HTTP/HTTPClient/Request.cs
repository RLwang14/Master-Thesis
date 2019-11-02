using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HTTPClient
{
    class Request
    {
        private String MethodType;
        private string Data;
        private String deviceName;
        private string requestSize;
        public Request(String methodType, String deviceName,string requestSize, string data)
        {
            this.MethodType = methodType;
            this.deviceName = deviceName;
            this.Data = data;
            this.requestSize = requestSize;
        }

        public String GenerateRequest()
        {
            HTTPClientBasic clientBasic = new HTTPClientBasic();
            String requestClient = null;
            string requestUrl = deviceName + "/" + requestSize;

            if (MethodType.Equals("GET"))
            {
                
                string host = clientBasic.Address + ":" + clientBasic.Port;
                requestClient = String.Format("{0} {1} {2}\r\nHost:{3}\r\n\r\n{4}",
                                                     MethodType, requestUrl, "HTTP/1.1", "0", Data);
            }else if (MethodType.Equals("POST"))
            {
                requestClient = String.Format("{0} {1} {2} \r\nHost:{3}\r\nContent-length:{4}\r\nContent-type:{5}\r\n\r\n{6}", //host = clientBasic.Address + ":" + clientBasic.Port
                                                     MethodType, requestUrl, "HTTP/1.1", "0", Data.Length, "application/x-www-form-urlencoded", Data);
            }        
          return requestClient;
        }
    }
}
