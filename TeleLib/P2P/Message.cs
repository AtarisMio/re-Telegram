using System;
using System.Collections.Generic;
using TeleLib.P2P;

namespace P2P
{
    /// <summary>
    /// Message base class
    /// </summary>
    [Serializable]
    public abstract class Message
    {
    }

    /// <summary>
    /// 客户端发送到服务器的消息基类
    /// </summary>
    [Serializable]
    public abstract class Client2ServerMessage : Message
    {
        private string userName;
        public string UserName
        {
            get { return userName; }
        }

        protected Client2ServerMessage(string username)
        {
            userName = username;
        }
    }

    /// <summary>
    /// 用户登录消息
    /// </summary>
    ///  
    [Serializable]
    public class LoginMessage : Client2ServerMessage
    {
        private string password;
        public string Password
        {
            get { return password; }
        }

        public LoginMessage(string userName, string passWord)
            : base(userName)
        {
            this.password = passWord;
        }
    }

    /// <summary>
    /// 用户登出消息
    /// </summary>
    [Serializable]
    public class LogoutMessage : Client2ServerMessage
    {
        public LogoutMessage(string userName)
            : base(userName)
        { }
    }

    /// <summary>
    /// 请求用户列表消息
    /// </summary>
    [Serializable]
    public class ListUserMessage : Client2ServerMessage
    {
        public ListUserMessage(string userName)
            : base(userName)
        { }
    }

    /// <summary>
    /// 请求Purch Hole消息
    /// </summary>
    [Serializable]
    public class TranslateMessage : Client2ServerMessage
    {
        protected string toUserName;
        public TranslateMessage(string userName, string toUserName)
            : base(userName)
        {
            this.toUserName = toUserName;
        }
        public string ToUserName
        {
            get { return this.toUserName; }
        }
    }

    /// <summary>
    /// 服务器发送到客户端消息基类
    /// </summary>
    [Serializable]
    public abstract class Server2ClientMessage : Message
    {
    }

    /// <summary>
    /// 请求用户列表应答消息
    /// </summary>
    [Serializable]
    public class ListUserResponseMessage : Server2ClientMessage
    {
        private UserCollection userList;

        public ListUserResponseMessage(UserCollection users)
        {
            this.userList = users;
        }

        public List<User> UserList
        {
            get { return userList.InnerList; }
        }
    }

    /// <summary>
    /// 转发请求Purch Hole消息
    /// </summary>
    [Serializable]
    public class PingMessage : Server2ClientMessage
    {
        protected IPEndPoint remotePoint;

        public PingMessage(IPEndPoint point)
        {
            this.remotePoint = point;
        }

        public IPEndPoint RemotePoint
        {
            get { return remotePoint; }
        }
    }


    /// <summary>
    /// 点对点消息基类
    /// </summary>
    [Serializable]
    public abstract class P2PMessage : Message
    {
    }

    /// <summary>
    /// 测试消息
    /// </summary>
    [Serializable]
    public class P2PTextMessage : P2PMessage
    {
        private string message;

        public P2PTextMessage(string msg)
        {
            message = msg;
        }

        public string Message
        {
            get { return message; }
        }
    }

    /// <summary>
    /// 测试应答消息
    /// </summary>
    [Serializable]
    public class P2PAckMessage : P2PMessage
    {
    }

    /// <summary>
    /// P2P Purch Hole Message
    /// </summary>
    [Serializable]
    public class TrashMessage : P2PMessage
    {
    }
}
