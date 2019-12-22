using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Net;
using System.Net.Sockets;

namespace ServerOne
{
    /// <summary>
    /// 客户端类
    /// </summary>
     public  class ClientPeer
    {
        public Socket clientSocket { get; set; }
        private SocketMessage smg = new SocketMessage();
        public ClientPeer()
        {
            receiveArgs = new SocketAsyncEventArgs();
            receiveArgs.UserToken = this;
            receiveArgs.SetBuffer(new byte[1024], 0, 1024);
            SendArgs = new SocketAsyncEventArgs();
            SendArgs.Completed += SendArgs_Completed;
        }

        #region 接收数据
        /// <summary>
        /// 数据包解析完成后的回调委托
        /// </summary>
        /// <param name="client"></param>
        /// <param name="value"></param>
        public delegate void ReceiveCompleted(ClientPeer client, SocketMessage msg);
        public ReceiveCompleted receiveCompleted;

        /// <summary>
        /// 存储接收到数据的缓存区
        /// </summary>
        private List<byte> dataCache = new List<byte>();
        /// <summary>
        /// 接收的异步套接字操作请求
        /// </summary>
        public SocketAsyncEventArgs receiveArgs { get; set; }
        /// <summary>
        /// 是否正在处理数据
        /// </summary>
        private bool isReceiveProcess = false;

        /// <summary>
        /// client层处理数据包
        /// </summary>
        /// <param name="package"></param>
        public void StartReceive(byte[] package)
        {
           // Tool.PrintMessage("执行StartReceive()方法:在client层处理数据包");
            dataCache.AddRange(package);
            if (!isReceiveProcess)
            {
                ProccessReveive();
            }
        }
        /// <summary>
        /// 处理数据
        /// </summary>
        private void ProccessReveive()
        {
            isReceiveProcess = true;
            byte[] data= EncodeTool.DecodePackage(ref dataCache);
            if (data == null) //数据包解析失败
            {
              //  Tool.PrintMessage("Client层解析数据包失败");
                isReceiveProcess = false;
                return;
            }
            SocketMessage msg = EncodeTool.DecodeMessage(data);

            if (receiveCompleted != null)
            {
                receiveCompleted(this, msg);
            }

            ProccessReveive(); //递归调用持续处理
        }

        #endregion

        #region 断开连接

        /// <summary>
        /// 断开连接
        /// </summary>
        public void Disconnect()  //断开连接并且可以重用
        {
            dataCache.Clear(); //清空数据缓存区
            isReceiveProcess = false;
            sendQueue.Clear(); //清空消息队列
            isSendProcess = false;

            clientSocket.Shutdown(SocketShutdown.Both);
            clientSocket.Close();
            clientSocket = null;
        }

        #endregion

        #region 发送数据

        /// <summary>
        /// 发送消息的队列
        /// </summary>
        private Queue<byte[]> sendQueue = new Queue<byte[]>();
        /// <summary>
        /// 是否正在发送
        /// </summary>
        private bool isSendProcess = false;
        /// <summary>
        /// 发送的异步socket操作
        /// </summary>
        private SocketAsyncEventArgs SendArgs;
        /// <summary>
        /// 发送时断开连接的委托
        /// </summary>
        /// <param name="client"></param>
        /// <param name="reason"></param>
        public delegate void SendDisconnect(ClientPeer client, string reason);
        public SendDisconnect sendDiconnect;
        /// <summary>
        /// 服务器向客户端发送数据
        /// </summary>
        /// <param name="opCode">操作码</param>
        /// <param name="subCode">子操作</param>
        /// <param name="value">参数</param>
        public void SendMessage(int opCode,int subCode,object value)
        {
            // SocketMessage smg = new SocketMessage(opCode,subCode,value);
            smg.Change(opCode, subCode, value);
            byte[] data = EncodeTool.EncodeMessage(smg); //构建SocketMessage
            byte[] package = EncodeTool.EncodePackage(data); //构建数据包
            sendQueue.Enqueue(package); //将要发送的消息存入消息队列
            if (!isSendProcess)
            {
                ProcessSend();
            }
        }

        /// <summary>
        /// 处理发送
        /// </summary>
        private void Send()
        {
            isSendProcess = true;
            if (sendQueue.Count == 0) //如果消息队列中没有消息就停止
            {
                isSendProcess = false;
                return;
            }
            byte[] package = sendQueue.Dequeue(); //从消息队列里取出一个消息
            SendArgs.SetBuffer(package, 0, package.Length); //设置socekt操作的缓存区
            bool result = clientSocket.SendAsync(SendArgs);
            if (result == false)
            {
                ProcessSend();
            }
        }
        /// <summary>
        /// 异步发送处理完成时的回调
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void SendArgs_Completed(object sender,SocketAsyncEventArgs e)
        {
            ProcessSend();
        }
        /// <summary>
        /// 发送完成时调用
        /// </summary>
        private void ProcessSend()
        {
           
            if (SendArgs.SocketError != SocketError.Success) //发送是否出错
            {
                Tool.PrintMessage("发送出错，客户端断开连接");
                sendDiconnect(this, SendArgs.SocketError.ToString());
            }
            else
            {
                Send();
            }
        }

        #endregion
    }
}
