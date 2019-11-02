using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

namespace Com.AugustCellars.CoAP.Examples
{
    class Devices
    {
        private static string XMLpath = "C:\\Devices\\DevicesXML.xml";
        private string deviceName;
        private string requestSize;
        private string filePath;
        private string requestData=null;

        public Devices(string deviceName, string requestSize,string methodType,string postData)
        {
            this.deviceName = deviceName;
            this.requestSize = requestSize;
            ParseXML();

            if(methodType == "GET")
                ReadFromTXT();
            if (methodType == "POST")
                WriteToTXT(postData);

        }
        public string DeviceDATA
        {
            get { return requestData; }
        }
        //parsing the xml file and get the file path
        private void ParseXML()
        {
            XmlDocument doc = new XmlDocument();
            doc.Load(XMLpath);

            XmlNode devicesNode = doc.SelectSingleNode("Devices");

            XmlNodeList devices = devicesNode.ChildNodes;
            foreach (XmlNode device in devices)
            {
                XmlElement deviceXe = (XmlElement)device;

                if (deviceXe.GetAttribute("id").ToString() == deviceName)
                {
                    XmlNodeList deviceInfo = deviceXe.ChildNodes;
                    foreach (XmlNode devicePath in deviceInfo)
                    {
                        XmlElement devicePathXe = (XmlElement)devicePath;
                        if (devicePathXe.GetAttribute("id").ToString() == requestSize)
                        {
                            filePath = devicePathXe.InnerText;
                        }
                    }

                }
            }
        }

        //read the content from a file
        private void ReadFromTXT()
        {
            string line;
            string path = "C:\\" + filePath;
            StreamReader file = new StreamReader(path);
            while((line = file.ReadLine()) != null)
            {
                requestData += line;
            }
            file.Close();
        }
 
        private void WriteToTXT(String postData)
        {
            string path = "C:\\" + filePath;
            StreamWriter file = new StreamWriter(path);
            file.Write(postData);
            file.Close();
        }

    }
}