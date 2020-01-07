
namespace OPCUAServer
{
    class Node
    {
        private string _browseName;
        private string _description;
        private string _nodeID;
        private string _nodeClass;
        private string _defaultValue;
        //string browseName, string nodeID, string defaultValue, string nodeCliass, string description
        public Node()
        {
            //this._browseName = browseName;
            //this._nodeID = nodeID;
            //this._defaultValue = defaultValue;
            //this._nodeClass = nodeCliass;
            //this._description = description;
        }

        public string BrowseName
        {
            get;set;
        }

        public string NodeID
        {
            get; set;
        }

        public string Description
        {
            get; set;
        }

        public string NodeClass
        {
            get; set;
        }

        public string DefaultValue
        {
            get; set;
        }

    }
}
