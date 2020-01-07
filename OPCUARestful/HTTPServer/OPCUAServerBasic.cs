﻿using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Monitor;
namespace OPCUAServer
{
    class OPCUAServerBasic
    {
        public const String VERSION = "HTTP/1.1";
        public const String NAME = "Restful OPCUA Server";
        private bool running = false;

        private TcpListener tcplistener;
        private NodeManager _nodeManger;


        public OPCUAServerBasic(string ip, int port)
        {
            tcplistener = new TcpListener(IPAddress.Parse(ip), port);
            _nodeManger = new NodeManager();
            Console.WriteLine("OPCUA server: {0} {1} ", ip,port);

        }

        public void Start()
        {
            Thread serverThread = new Thread(new ThreadStart(Run));
            serverThread.Start();
        }

        private void Run()
        {
            running = true;
            tcplistener.Start();
            int i = 1;
            while (running)
            {
                Console.WriteLine("{0} is running on....", NAME);
                Console.WriteLine("Waiting for connecting....");
                

                TcpClient tcpClient = tcplistener.AcceptTcpClient();//waiting for client connection
                Console.WriteLine("Client connected");

                if (tcpClient != null)
                {
                    HandleClient clientThread = new HandleClient(tcpClient,i.ToString(),_nodeManger);
                    Thread th = new Thread(clientThread.threadClient);
                    th.Start();
                    i++;
                }                                 
            }
            //tcpClient.Close();
        }

        class HandleClient
        {
            private TcpClient tcpClient;
            private string clientNum;
            private NodeManager _nodeManager;
            public HandleClient(TcpClient tcpClient, string clientNum,NodeManager nodeManager)
            {
                this._nodeManager = nodeManager;
                this.tcpClient = tcpClient;
                this.clientNum = clientNum;
            }

            public void threadClient()
            {
                String req = null;
                Byte[] bytes = new Byte[100000000];
                StopWatch st = new StopWatch("C:\\OPCUA\\performanceOPCUA_Server_CSharp_FromClient" + clientNum + ".log");
                st.WriteLog();
                NetworkStream stream = tcpClient.GetStream();
                int i;
                while (true)
                {
                    try
                    {
                        i = stream.Read(bytes, 0, bytes.Length);
                        st.Start();
                        req = System.Text.Encoding.ASCII.GetString(bytes, 0, i);
                        RequestManager newReq = RequestManager.GetRequest(req);
                        Response resp = Response.From(newReq, _nodeManager);
                        byte[] reponse = resp.SendResponse(tcpClient.GetStream(), newReq.MethodType);
                        st.Stop();
                        Console.WriteLine(newReq.MethodType + " :Request got reveived");

                        if (resp.body == null)
                            st.WriteLogServer(newReq.MethodType, reponse.Length.ToString());
                        else
                            st.WriteLogServer(newReq.MethodType, resp.body.Length.ToString());
                    }
                    catch (IOException ex)
                    {
                       Console.WriteLine("client is closed");
                       break;
                    }
                
                }
                
            }
        }

       /* private void HandleClient(TcpClient tcpClient)
        {
            String req = null;
            Byte[] bytes = new Byte[];

            StopWatch  st = new StopWatch("C:\\HTTP\\performanceHTTP_Server_CSharp.log");

            NetworkStream stream = tcpClient.GetStream();
            int i;
            while((i = stream.Read(bytes,0,bytes.Length)) != 0)
            {
                st.Start();
                req = System.Text.Encoding.ASCII.GetString(bytes, 0, i);
                Request newReq = Request.GetRequest(req);
                string method = newReq.MethodType;
                Console.WriteLine(method + " :Request got reveived");
                Response resp = Response.From(newReq);
                
                byte[] reponse = resp.SendResponse(tcpClient.GetStream(),method);
                st.Stop();

                st.WriteLogServer(method, reponse.Length);

            }*/
                        
        }
    }

