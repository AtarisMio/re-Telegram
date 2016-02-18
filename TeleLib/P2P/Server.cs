using System;
using System.Net;
using System.Threading;
using TeleLib.P2P;

namespace P2P
{
    /// <summary>
    /// Server 的摘要说明。
    /// </summary>
    public class Server
    {
        public const int SERVER_LISTENING_PORT = 2280;

        private MyUdpClient server;
        private UserCollection userList;
        private Thread serverThread;
        private IPEndPoint remotePoint;

        public Server()
        {
            userList = new UserCollection();
            remotePoint = new IPEndPoint("0.0.0.0", 0);
            serverThread = new Thread(new ThreadStart(Run));
        }

        public void Start()
        {
            try
            {
                server = new MyUdpClient(SERVER_LISTENING_PORT);
                serverThread.Start();
                Console.WriteLine("P2P Server started, waiting client connect...");
            }
            catch (Exception exp)
            {
                Console.WriteLine("Start P2P Server error: " + exp.Message);
                throw exp;
            }
        }

        public void Stop()
        {
            Console.WriteLine("P2P Server stopping...");
            try
            {
                serverThread.Abort();
                server.Close();
                Console.WriteLine("Stop OK.");
            }
            catch (Exception exp)
            {
                Console.WriteLine("Stop error: " + exp.Message);
                throw exp;
            }

        }

        private async void Run()
        {
            byte[] buffer = null;
            while (true)
            {
                byte[] msgBuffer = await server.Receive(remotePoint);
                try
                {
                    object msgObj = FormatterHelper.Deserialize(msgBuffer);
                    Type msgType = msgObj.GetType();
                    if (msgType == typeof(LoginMessage))
                    {
                        // 转换接受的消息
                        LoginMessage lginMsg = (LoginMessage)msgObj;
                        Console.WriteLine("{0}: user {1} sign in.", System.DateTime.Now.ToString(), lginMsg.UserName);

                        // 添加用户到列表
                        IPEndPoint userEndPoint = new IPEndPoint(remotePoint.Address, remotePoint.Port);
                        User user = new User(lginMsg.UserName, userEndPoint);
                        userList.Add(user);

                        // 发送应答消息
                        ListUserResponseMessage usersMsg = new ListUserResponseMessage(userList);
                        buffer = FormatterHelper.Serialize(usersMsg);
                        server.Send(buffer, buffer.Length, remotePoint);
                        Console.WriteLine("Send:" + usersMsg);
                    }
                    else if (msgType == typeof(LogoutMessage))
                    {
                        // 转换接受的消息
                        LogoutMessage lgoutMsg = (LogoutMessage)msgObj;
                        Console.WriteLine("{0}: {1} sign out", System.DateTime.Now.ToString(), lgoutMsg.UserName);

                        // 从列表中删除用户
                        User lgoutUser = userList.Find(lgoutMsg.UserName);
                        if (lgoutUser != null)
                        {
                            userList.Remove(lgoutUser);
                        }
                    }
                    else if (msgType == typeof(TranslateMessage))
                    {
                        // 转换接受的消息
                        TranslateMessage transMsg = (TranslateMessage)msgObj;
                        Console.WriteLine("{0}(1) wants to p2p {2}", remotePoint.Address.ToString(), transMsg.UserName, transMsg.ToUserName);

                        // 获取目标用户
                        User toUser = userList.Find(transMsg.ToUserName);

                        // 转发Purch Hole请求消息
                        if (toUser == null)
                        {
                            Console.WriteLine("Remote host {0} cannot be found at index server", transMsg.ToUserName);
                        }
                        else
                        {
                            PingMessage transMsg2 = new PingMessage(remotePoint);
                            buffer = FormatterHelper.Serialize(transMsg);
                            server.Send(buffer, buffer.Length, toUser.NetPoint);
                            Console.WriteLine("Send:" + transMsg2);
                        }
                    }
                    else if (msgType == typeof(ListUserMessage))
                    {
                        // 发送当前用户信息到所有登录客户
                        ListUserResponseMessage srvResMsg = new ListUserResponseMessage(userList);
                        buffer = FormatterHelper.Serialize(srvResMsg);
                        foreach (User user in userList.InnerList)
                        {
                            server.Send(buffer, buffer.Length, user.NetPoint);
                            Console.WriteLine("Send:" + srvResMsg);
                        }
                    }
                    Thread.Sleep(500);
                }
                catch (Exception e)
                {
                    System.Console.WriteLine(e.Message);
                    System.Console.WriteLine(e.StackTrace);
                }
            }
        }
    }
}


