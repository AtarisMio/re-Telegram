using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TeleLib.P2P
{
    public class IPEndPoint
    {
        public const int MaxPort = 65535;
        public const int MinPort = 0;
        internal const int AnyPort = MinPort;
        private string m_Address;
        private int m_Port;
        public IPEndPoint(string address, int port)
        {
            m_Address = address;
            m_Port = port;
        }
        internal static IPEndPoint Any = new IPEndPoint("0.0.0.0", AnyPort);
        internal string toString()
        {
            return " "+m_Address + ":" + m_Port+" ";
        }
        public string Address
        {
            get
            {
                return m_Address;
            }

            set
            {
                m_Address = value;
            }
        }

        public int Port
        {
            get
            {
                return m_Port;
            }

            set
            {
                m_Port = value;
            }
        }
    }
}
