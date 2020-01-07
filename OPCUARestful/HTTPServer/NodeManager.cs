using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

namespace OPCUAServer
{
    class NodeManager
    {
        private static string filePath = System.IO.Path.GetDirectoryName(Assembly.GetEntryAssembly().Location)+ "\\Sensor.PredifinedNode.xml";
        private Node _node;
        public List<Node> nodeClollection = new List<Node>();

        public NodeManager()
        {
            LoadPredifinedNode();
        }

        private void LoadPredifinedNode()
        {          
            XmlReaderSettings settings = new XmlReaderSettings();
            settings.IgnoreComments = true;
            settings.IgnoreWhitespace = true;

            XmlReader reader = XmlReader.Create(filePath,settings);
            while (reader.Read())
            {
                if(reader.NodeType == XmlNodeType.Element)
                {
                    switch (reader.Name)
                    {
                        case "NodeClass":
                            _node = new Node();
                            _node.NodeClass = XMLReadText(reader);
                            break;
                        case "NodeId":
                            string id = XMLReadText(reader);
                            _node.NodeID = id;
                            break;
                        case "BrowseName":
                            string name = XMLReadText(reader);
                            _node.BrowseName = name;
                            break;
                        case "Description":
                            string description = XMLReadText(reader);
                            _node.Description = description;
                            break;
                        case "Value":
                            _node.DefaultValue = XMLReadText(reader); ;
                            nodeClollection.Add(_node);
                            break;                        
                    }
                }
            }
            reader.Close();
        }

        private Node FindNode(string NodeID)
        {
            Node targetNode = null ;
            foreach(Node node in nodeClollection)
            {
                if (node.NodeID == NodeID)
                {
                    targetNode = node;
                }               
            }
            return targetNode;
        }

        private string XMLReadText(XmlReader xmlReader)
        {
            string text = null;
            XmlReader tReader = xmlReader.ReadSubtree();
            while (tReader.Read())
            {
                if (tReader.NodeType == XmlNodeType.Text)
                {
                    text = tReader.Value;
                }
            }
            return text;
        }

        public string ReadNode(string NodeID)
        {
            Node targetNode = FindNode(NodeID);
            return targetNode.DefaultValue;
        }

        public bool WriteNode(string NodeID,string ValueToWrite)
        {
            bool isSuccess = false;
            Node targetNode = FindNode(NodeID);
            targetNode.DefaultValue = ValueToWrite;
            //isSuccess = WriteToXML(targetNode, ValueToWrite);
            return isSuccess;
        }

        private bool TryOpen(string path)
        {
            try
            {
                var file = File.Open(path, FileMode.Open);
                file.Close();
                return true;
                
            }
            catch(Exception ex)
            {
                return false;
            }
        }

        public void UpdateXXMLdata()
        {
            try
            {
                XmlWriter xmlWriter = XmlWriter.Create(filePath);
                xmlWriter.WriteStartDocument(false);
                xmlWriter.WriteStartElement("Sensor");
                foreach (Node node in nodeClollection)
                {
                    string sensorType = node.BrowseName;
                    string nodeId = node.NodeID;
                    string value = node.DefaultValue;
                    WriteSensorNode(xmlWriter, sensorType, nodeId, value);
                }
                xmlWriter.WriteEndElement();
                xmlWriter.Flush();
            }
            catch(Exception ex)
            {
                Console.WriteLine("write failed to XML file");
                //UpdateXXMLdata();
            }
            

        }
        private void WriteSensorNode(XmlWriter writer,string sensorType,string nodeId,string value)
        {
            writer.WriteStartElement(sensorType);

            writer.WriteStartElement("NodeClass");
            writer.WriteString("Variable_2");
            writer.WriteEndElement();
            writer.WriteStartElement("NodeId");
            writer.WriteElementString("Identifier", nodeId);
            writer.WriteEndElement();
            writer.WriteStartElement("BrowseName");
            writer.WriteElementString("Name", sensorType);
            writer.WriteEndElement();
            writer.WriteStartElement("Description");
            writer.WriteElementString("Text", "This node include the value of " + sensorType);
            writer.WriteEndElement();
            writer.WriteStartElement("Value");
            writer.WriteElementString("string", value);
            writer.WriteEndElement();

            writer.WriteEndElement();



        }
        private bool WriteToXML(Node node, string value)
        {
            bool IsAccessible = false;
            XmlDocument xmlDoc = new XmlDocument();

            //while (TryOpen(filePath))
            //{
                try
                {
                    xmlDoc.Load(filePath);
                    XmlNodeList nodeList = xmlDoc.SelectSingleNode("Sensors").ChildNodes;
                        foreach (XmlNode xmlNode in nodeList)
                        {
                            XmlElement xe = (XmlElement)xmlNode;
                            if (xe.Name == node.BrowseName)
                            {
                                XmlNodeList nodes = xe.ChildNodes;
                                foreach (XmlNode xn in nodes)
                                {
                                    if (xn.Name == "Value")
                                    {
                                        XmlNode stringValue = xn.FirstChild;
                                        stringValue.InnerText = value;
                                        IsAccessible = true;
                                        break;
                                    }
                                }
                            }
                            xmlDoc.Save(filePath);
                        }
                    
                }
                catch (Exception ex)
                {                   
                    Console.WriteLine("retry to write to XML file");
                    WriteToXML(node, value);
                }

            //    if (IsAccessible)
            //        break;
            //}                                               
           return true;
        }   
      
    }
}
