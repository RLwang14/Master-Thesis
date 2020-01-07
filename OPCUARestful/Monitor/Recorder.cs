using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Monitor
{
    public class Recorder
    {
        private string path;
        public Recorder(string path)
        {
            this.path = path;
        }


        public void WriteLog(string verb, string sequenceNum, string packagesize)
        {
            string currentTime = DateTime.Now.ToString("HH:mm:ss.ffffff");
            //String path = "C:\\performanceHTTP_CSharp.log";
            StreamWriter sw = new StreamWriter(this.path, true);
            sw.WriteLine(verb + "," + sequenceNum + "," + currentTime + "," + packagesize);
            sw.Flush();
            sw.Close();

        }

        public void WriteLog2(string verb, string sequenceNum, string packagesize,string status)
        {
            string currentTime = DateTime.Now.ToString("HH:mm:ss.ffffff");

            //String path = "C:\\performanceHTTP_CSharp.log";
            StreamWriter sw = new StreamWriter(this.path, true);
            sw.WriteLine(verb + "," + sequenceNum + "," + currentTime + "," + packagesize + "," + status);
            sw.Flush();
            sw.Close();

        }
    }
}
