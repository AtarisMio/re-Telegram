using System.Net;
using System.Net.Sockets;

namespace P2P
{
    class MyUdpClient : UdpClient
    {
        public MyUdpClient()
            : base()
        {
        }

        public MyUdpClient(int port)
            : base(port)
        {            
        }

        new public int Send(byte[] data, int bytes, IPEndPoint remoteHost)
        {
            int ret = base.Send(data, bytes, remoteHost);
            System.Console.WriteLine(System.DateTime.Now.ToString() + ">>> " + remoteHost);
            System.Console.WriteLine(FormatterHelper.Deserialize(data));
            System.Console.WriteLine();
            return ret;
        }

        new public byte[] Receive(ref IPEndPoint remoteHost)
        {
            byte[] data = base.Receive(ref remoteHost);
            System.Console.WriteLine(System.DateTime.Now.ToString() + "<<< " + remoteHost);
            System.Console.WriteLine(FormatterHelper.Deserialize(data));
            System.Console.WriteLine();
            return data;
        }
    }
}
