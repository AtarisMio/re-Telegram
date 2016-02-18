using System;
using System.Net;
using System.Collections;
using System.Collections.Generic;
using TeleLib.P2P;

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
    public class UserCollection
    {
        private List<User> innerList;
        
        public List<User> InnerList
        {
            get
            {
                return innerList;
            }
        }

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
            get { return InnerList[index]; }
        }

        public User Find(string userName)
        {
            foreach (User user in InnerList)
            {
                if (string.Compare(userName, user.UserName) == 0)
                {
                    return user;
                }
            }
            return null;
        }
        public void Clear()
        {
            InnerList.Clear();
        }
    }
}
