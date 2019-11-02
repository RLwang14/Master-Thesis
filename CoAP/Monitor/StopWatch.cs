using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace MonitorX
{
    public class StopWatch
    {
        [System.Runtime.InteropServices.DllImport("Kernel32.dll")]
        static extern bool QueryPerformanceCounter(ref long count);

        [System.Runtime.InteropServices.DllImport("Kernel32.dll")]
        static extern bool QueryPerformanceFrequency(ref long count);
        private string path;
        public StopWatch(string path)
        {

            this.path = path;
        }
        private const int DURATION = 1;
        private const int CURRENT_TIME = 0;

        long count = 0;
        private long currentCount = 0;
        long count1 = 0;
        long freq = 0;
        double result = 0;
        private double currentTime = 0;
        [STAThread]
        public void Start()
        {
            QueryPerformanceFrequency(ref freq);
            QueryPerformanceCounter(ref count);
        }

        public void Stop()
        {
            QueryPerformanceCounter(ref count1);
            count = count1 - count;
            result = (double)(count) / (double)freq * 1000000;
        }

        public void WriteLogClient(string methodType, string deviceName, string requestDataSize, string clientNum, string status)
        {
            StreamWriter sw = new StreamWriter(this.path, true);
            if (methodType == "GET")
            {

                sw.WriteLine("H,0" + "," + clientNum + "," + deviceName + "," + requestDataSize + "," + status + "," + result);//packageSize = request data size
                sw.Close();

            }

            if (methodType == "POST")
            {
                sw.WriteLine("H,1" + "," + clientNum + "," + deviceName + "," + requestDataSize + "," + status + "," + result);
                sw.Close();
            }

        }

        public void WriteLogServer(string methodType, string deviceName, string responseSize)
        {
            StreamWriter sw = new StreamWriter(this.path, true);
            if (methodType == "GET")
            {
                sw.WriteLine("H,0" + "," + responseSize + "," + result);//packageSize = response data size
                sw.Close();
            }


            if (methodType == "POST")
            {
                sw.WriteLine("H,1" + "," + responseSize + "," + result);
                sw.Close();
            }

        }

        public void WriteLog(string msg, int type)
        {

            //String path = "C:\\performanceHTTP_CSharp.log";
            StreamWriter sw = new StreamWriter(this.path, true);
            switch (type)
            {
                case CURRENT_TIME:
                    sw.WriteLine(msg + " at: " + currentTime);
                    break;
                case DURATION:
                    sw.WriteLine(msg + "-------------------------------------------------" + result);
                    break;

            }
            sw.Flush();
            sw.Close();

        }


    }
}
