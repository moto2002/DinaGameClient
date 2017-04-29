using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections;
using System.Threading;
using System.Net;
using System.Net.Sockets;
using System.IO;
using System.Runtime.InteropServices;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;
using Assets.Scripts.Lib.Log;
using Assets.Scripts.Proto;
using UnityEngine;
using Assets.Scripts.Manager;

namespace Assets.Scripts.Lib.Net
{
    enum SocketState { NONE, CONNECTING, CONNECTED, CLOSE }

    public class NetClient : MonoBehaviour
    {
        public delegate void OnConectedHandler();

        public event OnConectedHandler OnConected;

        private static readonly int messageHeadLen = 2;
        private static Logger log = LoggerFactory.GetInstance().GetLogger(typeof(NetClient));

        private string host;
        private int port;

        private short currentPackageLength = 0;

        private ProtocolMapping mapping;

        private byte[] sendBuffer = new byte[1024];
        private BinaryWriter sender = null;

        private Socket clientSocket;

        private AsyncCallback callBack;

        private SocketState state = SocketState.NONE;

        private long m_lLastPingTime = 0;
        private int m_pingTimeount = 0;
        public delegate void PingDelegate(long currentTime);
        private PingDelegate m_pingDelegate;

        public void Awake()
        {
            mapping = new ProtocolMapping();
            sender = new BinaryWriter(new MemoryStream(sendBuffer));
        }
		

        public void RegisterProtocol(int protocolId, ProtocolHandler handler, Type type)
        {
            mapping.AddProtocolHandler(protocolId, handler, type);
        }

        public void UnRegisterProtocol(int protocolId, ProtocolHandler handler)
        { 

        }

        public void Connect(string _host, int _port, int reconnectCount, string endian)
        {
            host = _host;
            port = _port;
            clientSocket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            IPAddress ipAddress = IPAddress.Parse(host);
            IPEndPoint ipEndPoint = new IPEndPoint(ipAddress, port);
            callBack = new AsyncCallback(SocketConnectAsyncCallback);
            clientSocket.BeginConnect(ipEndPoint, callBack, clientSocket);
            state = SocketState.CONNECTING;
        }

        private void SocketConnectAsyncCallback(IAsyncResult asyncConnect)
        {
            
            if(clientSocket.Connected)
			{
				log.Debug("链接成功");
			}
			else
			{
				log.Debug("链接失败");
			}
            
        }

        public void DisposeConnect()
        {
            if (clientSocket != null)
            {
                if (clientSocket.Connected == true)
                {
                    clientSocket.Shutdown(SocketShutdown.Both);
                }
                clientSocket.Close();
                state = SocketState.CLOSE;
            }
            clientSocket = null;
            state = SocketState.CLOSE;
        }

        private void Ping()
        {
            long currentTime = Environment.TickCount;
            if (currentTime - m_lLastPingTime < m_pingTimeount)
                return;

            m_pingDelegate(currentTime);

            m_lLastPingTime = currentTime;
        }

        byte[] recvBuffer = new byte[64 * 1024];
        byte[] unsignedShort = new byte[2];

        public void Update()
        {
            if (clientSocket == null)
            {
                return;
            }

            if (state == SocketState.CONNECTING && clientSocket.Connected)
            {
                OnConected();
                state = SocketState.CONNECTED;
            }
            else if (state == SocketState.CONNECTED && clientSocket.Connected)
            {
                Ping();
                if (clientSocket.Available > 0)
                {
                    if (clientSocket.Available < messageHeadLen)
                    {
                        log.Error("[KG_Socket.onRecvPackage]Receive a buffer:Size < " + messageHeadLen);
                        return;
                    }

                    if (currentPackageLength == 0)
                    {
                        clientSocket.Receive(unsignedShort);
                        currentPackageLength = BitConverter.ToInt16(unsignedShort, 0);
                    }

                    int packageDataLength = currentPackageLength - messageHeadLen;

                    if (packageDataLength <= 0)
                    {
                        log.Error("packageDataLength <= 0");
                    }

                    if (packageDataLength > clientSocket.Available)
                    {
                        log.Warn("[KG_Socket.onRecvPackage]Package is not integrated packageDataLength=" + packageDataLength + ", _clientSocket.bytesAvailable=" + clientSocket.Available);
                        return;
                    }

                    currentPackageLength = 0;


                    int nRecvSize = clientSocket.Receive(recvBuffer, packageDataLength, 0);


                    if (nRecvSize != packageDataLength)
                    {
                        log.Error("nRecvSize != packageDataLength");
                    }

                    if (nRecvSize > 0)
                    {
                        HandlerMessage(recvBuffer, nRecvSize);
                    }
                }
            }
            else if(state == SocketState.CONNECTED && (clientSocket == null || !clientSocket.Connected))
            {
                Debug.Log("连接中断");
                state = SocketState.CLOSE;
            }
        }

        private ProcessInfo lastProcessInfo;
        private void HandlerMessage(byte[] recvBuffer, int nRecvSize)
        {
            MemoryStream stream = new MemoryStream(recvBuffer);
            BinaryReader read = new BinaryReader(stream);

            while (stream.Position != stream.Length && stream.Position < nRecvSize)
            {
                int protocolID = read.PeekChar();
                //log.Debug("receive protocolID=" + protocolID);

                ProcessInfo processInfo = mapping.GetProcessInfo(protocolID);
                log.Assert(processInfo != null);

                Type type = processInfo.type;

                
                if (type == null)
                {
                    log.Error("协议没解释＝" + protocolID + "  LastProtocolId: " + (lastProcessInfo != null?lastProcessInfo.protocolId:0));
					return;
                }

                KProtoBuf protoBuf = (KProtoBuf)Activator.CreateInstance(type);
                protoBuf.unpack(read);
                if (processInfo == null)
                {
                    log.Error("未处理的消息 " + protocolID);
                }
				
				if (ConfigManager.GetInstance().DebugMode)
				{
					protoBuf.AddHandler(processInfo);
	                protoBuf.DispatchDelegate();
	                protoBuf.RemoveHandler(processInfo);
				}
				else
				{
					try
	                {
	                    protoBuf.AddHandler(processInfo);
	                    protoBuf.DispatchDelegate();
	                    protoBuf.RemoveHandler(processInfo);
	                }
	                catch (System.Exception e)
	                {
	                    Debug.LogError("处理协议 " + (KS2C_Protocol)protocolID + " 异常" + e.ToString());
	                }
					
				}
                

                lastProcessInfo = processInfo;
            }
        }

        public void SendMessage( KProtoBuf proto)
        {
            long begin = sender.Seek(0, SeekOrigin.Begin);
            sender.Write((short)0);
            proto.pack(sender);
            long end = sender.Seek(0, SeekOrigin.Current); 
            long len = end - begin;
            sender.Seek(0, SeekOrigin.Begin);
            sender.Write((short)len); 
            sender.Seek((int)len, SeekOrigin.Current);
            sender.Flush();
            clientSocket.Send(sendBuffer, (int)len, SocketFlags.None);
        }


        public static NetClient NewInstance(string name, NetClient.PingDelegate pingDelegate, int pingTimeout)
        {
            GameObject go = new GameObject(name);
			go.hideFlags = HideFlags.HideAndDontSave;
            DontDestroyOnLoad(go);
            NetClient netClient = go.AddComponent<NetClient>();
            netClient.m_pingDelegate = pingDelegate;
            netClient.m_pingTimeount = pingTimeout;
            return netClient;
        }
    }
}
