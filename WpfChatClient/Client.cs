using System;
using System.Net.Sockets;
using System.Threading.Tasks;
using System.Windows;

namespace WpfChatClient
{
    public class Client
    {
        private readonly TcpClient _client = new TcpClient();
        private NetworkStream _stream;
        public string Username { get; set; }

        public async Task Connect(string ipAddress, int port)
        {
            await _client.ConnectAsync(ipAddress, port);
            _stream = _client.GetStream();
            await SendMessage(Username);
        }

        public async Task SendMessage(string message)
        {
            try
            {
                Message message2 = new Message() { Data = message };
                byte[] replyMessage = Serializer.Serialize(message2);

                await _stream.WriteAsync(replyMessage, 0, replyMessage.Length);
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error in SendMessage: " + ex.Message);
            }
        }

        public void Listen(Action<string> onMessegedReveived)
        {
            byte[] bytes = new byte[1024];
            Message message;
            int count;

            try
            {
                while ((count = _stream.Read(bytes, 0, bytes.Length)) != 0)
                {
                    message = Serializer.Deserialize(bytes);

                    Application.Current.Dispatcher.Invoke(delegate
                    {
                        onMessegedReveived(message.Data);
                    });

                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error in Listen: " + ex.Message);
                Close();
            }
        }

        public void Close()
        {
            Console.WriteLine("Closing client");
            _stream.Close();
            _client.Close();
        }
    }
}
