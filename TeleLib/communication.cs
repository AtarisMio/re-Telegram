using System;
using System.Net;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Sockets.Plugin;
using System.Threading;

namespace TeleLib
{
    public class Communication
    {
        private TcpSocketClient client = new TcpSocketClient();
        private TcpSocketListener server = new TcpSocketListener();
        public static readonly Communication current = new Communication();
        private static string address_con_to;
        private static string address_loc;
        private static readonly int[] port = { 2333, 2411, 12450, 17173, 12580 };
        public void send_text(string src,out int length)
        {
            client.ConnectAsync(address_con_to, port[0]);
            byte[] bytes = (new System.Text.UTF8Encoding()).GetBytes(src);
            length = bytes.Length;
            foreach (byte a in bytes)
            {
                client.WriteStream.WriteByte(a);
                client.WriteStream.FlushAsync();
                Task.Delay(50);
            }
            client.DisconnectAsync();
        }
        public void send_byte(byte[] src, out int length)
        {
            client.ConnectAsync(address_con_to, port[0]);
            length = src.Length;
            foreach (byte a in src)
            {
                client.WriteStream.WriteByte(a);
                client.WriteStream.FlushAsync();
                Task.Delay(50);
            }
            client.DisconnectAsync();
        }
        public string revc_text(int length)
        {
            string result = "";
            var listener = new TcpSocketListener();
            listener.ConnectionReceived += async (sender, args) =>
              {
                  var _client = args.SocketClient;
                  List<byte> all_byte = new List<byte>();
                  for (int i = 0; i < length; i++)
                  {
                      var nextByte = await Task.Run(() => client.ReadStream.ReadByte());
                      all_byte.Add((byte)nextByte);

                  }
                  result = (new System.Text.UTF8Encoding()).GetString(all_byte.ToArray(), 0, all_byte.Count);
              };
            listener.StartListeningAsync(port[0]);
            return result;
        }

        public static string Address_Con_To
        {
            get
            {
                return address_con_to;
            }

            set
            {
                address_con_to = value;
            }
        }

        public static string Address_Loc
        {
            get
            {
                return address_loc;
            }

            set
            {
                address_loc = value;
            }
        }
    }
}
