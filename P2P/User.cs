using System;
using System.Net;
using System.Collections;

namespace P2P
{
    /// <summary>
    /// User 的摘要说明。
    /// </summary>
    [Serializable]
    public class User
    {
        protected string userName;
        protected IPEndPoint netPoint;
        public User(string UserName, IPEndPoint NetPoint)
        {
            this.userName = UserName;
            this.netPoint = NetPoint;
        }

        public string UserName
        {
            get { return userName; }
        }

        public IPEndPoint NetPoint
        {
            get { return netPoint; }
            set { netPoint = value; }
        }
    }

    /// <summary>
    /// UserCollection 的摘要说明。
    /// </summary>
    [Serializable]
    public class UserCollection : CollectionBase
    {
        public void Add(User user)
        {
            InnerList.Add(user);
        }

        public void Remove(User user)
        {
            InnerList.Remove(user);
        }

        public User this[int index]
        {
            get { return (User)InnerList[index]; }
        }

        public User Find(string userName)
        {
            foreach (User user in this)
            {
                if (string.Compare(userName, user.UserName, true) == 0)
                {
                    return user;
                }
            }
            return null;
        }
    }
}
