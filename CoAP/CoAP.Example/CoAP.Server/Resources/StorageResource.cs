using System;
using System.Collections.Generic;
using System.Text;
using Com.AugustCellars.CoAP.Server.Resources;
using MonitorX;

namespace Com.AugustCellars.CoAP.Examples.Resources
{
    class StorageResource : Resource
    {
        private String _content = "2";

        public StorageResource(String name)
            : base(name)
        { }
        StopWatch st = new StopWatch("C:\\CoAP\\performanceCoAP_Server_CSharp.log");

        private string GenerateResponseData(int size)
        {
            string responseData = null;
            for (int i = 1; i <= size; i++)
            {
                responseData += "1";
            }
            return responseData;
        }
        private void updateData(string value)
        {
            string data = value;
        }
        protected override void DoGet(CoapExchange exchange)
        {
            st.Start();
            string path = exchange.Request.UriQuery;
            string[] tokens = path.Split('/');
            string deviceName = tokens[0];
            string requstdatasize = tokens[1];
           // Devices di = new Devices(eviceName,requstdatasize,"GET",null);
            _content = GenerateResponseData(Int32.Parse(requstdatasize));
            if (_content != null)
            {
                exchange.Respond(_content);
            }
            else
            {
                String subtree = LinkFormat.Serialize(this, null);
                exchange.Respond(StatusCode.Content, subtree, MediaType.ApplicationLinkFormat);
            }
            st.Stop();
            int responseSize = exchange.getResponseSize();
            st.WriteLogServer("GET",deviceName, requstdatasize);
        }

        protected override void DoPost(CoapExchange exchange)
        {
            st.Start();
            string path = exchange.Request.UriQuery;
            string[] tokens = path.Split('/');
            string deviceName = tokens[0];
            string requstdatasize = tokens[1];
            String postValue = exchange.Request.PayloadString;
            // Devices di = new Devices(deviceName, requstdatasize, "POST", postValue);        
            updateData(postValue);
            Response response = new Response(StatusCode.Changed);
            exchange.Respond(response);
            st.Stop();
            int responseSize = response.ToString().Length;
            st.WriteLogServer("POST",deviceName, responseSize.ToString());
        }
        
        protected override void DoPut(CoapExchange exchange)
        {
            string payload = exchange.Request.PayloadString;
            string[] payloads = payload.Split(':');
            _content = payloads[0];
            string sequencNum = payloads[1];
            exchange.Respond(StatusCode.Changed);
        }

        protected override void DoDelete(CoapExchange exchange)
        {
            this.Delete();
            exchange.Respond(StatusCode.Deleted);
        }

        private IResource Create(LinkedList<String> path)
        {
            String segment;

            do
            {
                if (path.Count == 0)
                    return this;
                segment = path.First.Value;
                path.RemoveFirst();
            } while (segment.Length == 0 || segment.Equals("/"));

            StorageResource resource = new StorageResource(segment);
            Add(resource);
            return resource.Create(path);
        }
    }
}
