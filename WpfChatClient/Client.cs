using System;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace WpfChatClient
{
    public class Client
    {
        private TcpClient _client = new TcpClient();
        private NetworkStream _stream;
        public string Username { get; set; }

        public async Task Connect(string ipAddress, int port)
        {
            await _client.ConnectAsync(ipAddress, port);
            _stream = _client.GetStream();
        }

        public async Task SendMessage(string message)
        {
            byte[] buffer = Encoding.UTF8.GetBytes(message);
            await _stream.WriteAsync(buffer, 0, buffer.Length);
        }

        public void Listen(Action<string> onMessegedReveived)
        {
            byte[] bytes = new byte[1024];
            string message;
            int count;

            while ((count = _stream.Read(bytes, 0, bytes.Length)) != 0)
            {
                message = Encoding.ASCII.GetString(bytes, 0, count);

                Application.Current.Dispatcher.Invoke(delegate
                {
                    onMessegedReveived(message);
                });

            }
        }
    }
}
