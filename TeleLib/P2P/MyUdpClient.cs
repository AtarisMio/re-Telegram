using Sockets.Plugin;
using System;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;
using TeleLib.P2P;

namespace P2P
{
    class MyUdpClient : UdpSocketReceiver
    {
        IPEndPoint localEP;
        public MyUdpClient():base()
        { }

        public MyUdpClient(int port):base()
        {
            localEP = new IPEndPoint("0.0.0.0", port);
        }

        public void Close()
        {
            Dispose();
        }

        public async Task Send(byte[] data, int bytes, IPEndPoint remoteHost)
        {
            var client = new UdpSocketClient();
            await client.SendToAsync(data, remoteHost.Address, remoteHost.Port);
            System.Console.WriteLine(System.DateTime.Now.ToString() + ">>> " + remoteHost);
            System.Console.WriteLine(FormatterHelper.Deserialize(data).ToString());
            System.Console.WriteLine();
        }

        public async Task<byte[]> Receive(IPEndPoint remoteHost)
        {
            var receiver = new UdpSocketReceiver();
            byte[] data = new byte[1];

            receiver.MessageReceived += (sender, args) =>
            {
                // get the remote endpoint details and convert the received data into a string
                data = args.ByteData;
            };
            await receiver.StartListeningAsync(remoteHost.Port);
            System.Console.WriteLine(System.DateTime.Now.ToString() + "<<< " + remoteHost);
            System.Console.WriteLine(FormatterHelper.Deserialize(data).ToString());
            System.Console.WriteLine();
            return data;
        }
    }
}
