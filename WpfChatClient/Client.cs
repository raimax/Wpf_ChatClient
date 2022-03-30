using System;
using System.Collections.Generic;
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
                Message message2 = new Message();
                message2.Data.Add(message);
                byte[] replyMessage = Serializer.Serialize(message2);

                await _stream.WriteAsync(replyMessage, 0, replyMessage.Length);
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error in SendMessage: " + ex.Message);
            }
        }

        public void Listen(Action<string> onMessegedReveived, Action<string> onInfoRecveived, Action<List<string>> onUsersListReceived)
        {
            byte[] bytes = new byte[1024];
            Message message;
            int count;

            try
            {
                while ((count = _stream.Read(bytes, 0, bytes.Length)) != 0)
                {
                    message = Serializer.Deserialize(bytes);

                    if (message.Type == Message.MessageType.Message)
                    {
                        Application.Current.Dispatcher.Invoke(delegate
                        {
                            onMessegedReveived(message.Data[0]);
                        });
                    }
                    else if (message.Type == Message.MessageType.UserList)
                    {
                        Application.Current.Dispatcher.Invoke(delegate
                        {
                            onUsersListReceived(message.Data);
                        });
                    }
                    else if (message.Type == Message.MessageType.Info)
                    {
                        Application.Current.Dispatcher.Invoke(delegate
                        {
                            onInfoRecveived(message.Data[0]);
                        });
                    }
                }
            }
            catch (Exception)
            {
                Close();
            }
        }

        public void Close()
        {
            Console.WriteLine("Closing client");
            _client.Client.Close();
            _client.Close();
        }
    }
}
