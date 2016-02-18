using System;
using System.Net;
using System.Threading;

namespace P2P
{
    /// <summary>
    /// Client 的摘要说明。
    /// </summary>
    public class Client : IDisposable
    {
        private const int MAXRETRY = 10;
        private MyUdpClient client;
        private IPEndPoint hostPoint;
        private IPEndPoint remotePoint;
        private UserCollection userList;
        private string myName;
        private bool ReceivedACK;
        private Thread listenThread;

        public Client(string serverIP)
        {
            ReceivedACK = false;
            remotePoint = new IPEndPoint(IPAddress.Any, 0);
            hostPoint = new IPEndPoint(IPAddress.Parse(serverIP), Server.SERVER_LISTENING_PORT);
            client = new MyUdpClient();
            userList = new UserCollection();
            listenThread = new Thread(new ThreadStart(Run));
        }

        public void Start()
        {
            if (this.listenThread.ThreadState == ThreadState.Unstarted)
            {
                this.listenThread.Start();
                Console.WriteLine("You can input you command:\n");
                Console.WriteLine("Command Type:\"send\",\"exit\",\"getu\"");
                Console.WriteLine("Example : send Username Message");
                Console.WriteLine("          exit");
                Console.WriteLine("          getu");
            }
        }

        public void ConnectToServer(string userName, string password)
        {
            myName = userName;

            // 发送登录消息到服务器
            LoginMessage lginMsg = new LoginMessage(userName, password);
            byte[] buffer = FormatterHelper.Serialize(lginMsg);
            client.Send(buffer, buffer.Length, hostPoint);

            // 接受服务器的登录应答消息
            buffer = client.Receive(ref remotePoint);
            ListUserResponseMessage srvResMsg = (ListUserResponseMessage)FormatterHelper.Deserialize(buffer);

            // 更新用户列表
            userList.Clear();
            foreach (User user in srvResMsg.UserList)
            {
                userList.Add(user);
            }

            this.DisplayUsers(userList);
        }

        /// <summary>
        /// 这是主要的函数：发送一个消息给某个用户(C)
        /// 流程：直接向某个用户的外网IP发送消息，如果此前没有联系过
        /// 那么此消息将无法发送，发送端等待超时。
        /// 超时后，发送端将发送一个请求信息到服务端，要求服务端发送
        /// 给客户C一个请求，请求C给本机发送打洞消息
        /// 以上流程将重复MAXRETRY次
        /// </summary>
        /// <param name="toUserName">对方用户名</param>
        /// <param name="message">待发送的消息</param>
        /// <returns></returns>
        private bool SendMessageTo(string toUserName, string message)
        {
            User toUser = userList.Find(toUserName);
            if (toUser == null)
            {
                return false;
            }
            for (int i = 0; i < MAXRETRY; i++)
            {
                P2PTextMessage workMsg = new P2PTextMessage(message);
                byte[] buffer = FormatterHelper.Serialize(workMsg);
                client.Send(buffer, buffer.Length, toUser.NetPoint);

                // 等待接收线程将标记修改
                for (int j = 0; j < 10; j++)
                {
                    if (this.ReceivedACK)
                    {
                        this.ReceivedACK = false;
                        return true;
                    }
                    else
                    {
                        Thread.Sleep(300);
                    }
                }

                // 没有接收到目标主机的回应，认为目标主机的端口映射没有
                // 打开，那么发送请求信息给服务器，要服务器告诉目标主机
                // 打开映射端口（UDP打洞）
                TranslateMessage transMsg = new TranslateMessage(myName, toUserName);
                buffer = FormatterHelper.Serialize(transMsg);
                client.Send(buffer, buffer.Length, hostPoint);

                // 等待对方先发送信息
                Thread.Sleep(100);
            }
            return false;
        }

        public void PaserCommand(string cmdstring)
        {
            cmdstring = cmdstring.Trim();
            string[] args = cmdstring.Split(new char[] { ' ' });
            if (args.Length > 0)
            {
                if (string.Compare(args[0], "exit", true) == 0)
                {
                    LogoutMessage lgoutMsg = new LogoutMessage(myName);
                    byte[] buffer = FormatterHelper.Serialize(lgoutMsg);
                    client.Send(buffer, buffer.Length, hostPoint);
                    // do clear something here
                    Dispose();
                    System.Environment.Exit(0);
                }
                else if (string.Compare(args[0], "send", true) == 0)
                {
                    if (args.Length > 2)
                    {
                        string toUserName = args[1];
                        string message = "";
                        for (int i = 2; i < args.Length; i++)
                        {
                            if (args[i] == "") message += " ";
                            else message += args[i];
                        }
                        if (this.SendMessageTo(toUserName, message))
                        {
                            Console.WriteLine("Send OK!");
                        }
                        else
                        {
                            Console.WriteLine("Send to " + toUserName + " Failed!");
                        }
                    }
                }
                else if (string.Compare(args[0], "getu", true) == 0)
                {
                    ListUserMessage getUserMsg = new ListUserMessage(myName);
                    byte[] buffer = FormatterHelper.Serialize(getUserMsg);
                    client.Send(buffer, buffer.Length, hostPoint);
                }
                else
                {
                    Console.WriteLine("Unknown command {0}", cmdstring);
                }
            }
        }

        private void DisplayUsers(UserCollection users)
        {
            foreach (User user in users)
            {
                Console.WriteLine("Username: {0}, IP:{1}, Port:{2}", user.UserName, user.NetPoint.Address.ToString(), user.NetPoint.Port);
            }
        }

        private void Run()
        {
            byte[] buffer;
            while (true)
            {
                buffer = client.Receive(ref remotePoint);
                object msgObj = FormatterHelper.Deserialize(buffer);
                Type msgType = msgObj.GetType();
                if (msgType == typeof(ListUserResponseMessage))
                {
                    // 转换消息
                    ListUserResponseMessage usersMsg = (ListUserResponseMessage)msgObj;
                    // 更新用户列表
                    userList.Clear();
                    foreach (User user in usersMsg.UserList)
                    {
                        userList.Add(user);
                    }
                    this.DisplayUsers(userList);
                }
                else if (msgType == typeof(PingMessage))
                {
                    // 转换消息
                    PingMessage purchReqMsg = (PingMessage)msgObj;
                    // 发送打洞消息到远程主机
                    TrashMessage trashMsg = new TrashMessage();
                    buffer = FormatterHelper.Serialize(trashMsg);
                    client.Send(buffer, buffer.Length, purchReqMsg.RemotePoint);
                }
                else if (msgType == typeof(P2PTextMessage))
                {
                    // 转换消息
                    P2PTextMessage workMsg = (P2PTextMessage)msgObj;
                    Console.WriteLine("Receive a message: {0}", workMsg.Message);
                    // 发送应答消息
                    P2PAckMessage ackMsg = new P2PAckMessage();
                    buffer = FormatterHelper.Serialize(ackMsg);
                    client.Send(buffer, buffer.Length, remotePoint);
                }
                else if (msgType == typeof(P2PAckMessage))
                {
                    this.ReceivedACK = true;
                }
                else if (msgType == typeof(TrashMessage))
                {
                    Console.WriteLine("Recieve a trash message");
                }
                Thread.Sleep(100);
            }
        }
        #region IDisposable 成员

        public void Dispose()
        {
            try
            {
                this.listenThread.Abort();
                this.client.Close();
            }
            catch
            { }
        }

        #endregion
    }
}
