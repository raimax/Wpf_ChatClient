using System;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;

namespace WpfChatClient
{
    /// <summary>
    /// Interaction logic for ChatWindow.xaml
    /// </summary>
    public partial class ChatWindow : Window
    {
        private readonly Client _client;

        public ChatWindow(Client client)
        {
            InitializeComponent();
            _client = client;
            Label_Username.Content = client.Username;

            Task.Run(() =>
            {
                _client.Listen(AddServerMessageToList, AddServerInfoMessageToList);
            });
        }

        private async void btn_sendMessage_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (!string.IsNullOrWhiteSpace(input_message.Text))
                {
                    await _client.SendMessage(input_message.Text);
                    AddUserMessageToList(input_message.Text);
                    input_message.Clear();
                    MessageListViewer.ScrollToBottom();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void AddUserMessageToList(string message)
        {
            Border messageBox = GenerateMessageBox(ColorHelper.GetColor("#19a3ff"), HorizontalAlignment.Right, message);
            MessageList.Children.Add(messageBox);
        }

        private void AddServerMessageToList(string message)
        {
            Border messageBox = GenerateMessageBox(ColorHelper.GetColor("#303030"), HorizontalAlignment.Left, message);
            MessageList.Children.Add(messageBox);
        }

        private void AddServerInfoMessageToList(string message)
        {
            TextBlock infoBox = GenerateInfoBox(message);
            MessageList.Children.Add(infoBox);
        }

        private Border GenerateMessageBox(SolidColorBrush color, HorizontalAlignment alignment, string message)
        {
            Border border = new Border();
            border.Margin = new Thickness(0, 0, 0, 2);
            border.Background = color;
            border.CornerRadius = new CornerRadius(15);
            border.HorizontalAlignment = alignment;

            TextBlock textBlock = new TextBlock();
            textBlock.Padding = new Thickness(8);
            textBlock.FontSize = 16;
            textBlock.Text = message;
            textBlock.TextWrapping = TextWrapping.Wrap;
            textBlock.Foreground = ColorHelper.GetColor("#fff");

            border.Child = textBlock;

            return border;
        }

        private TextBlock GenerateInfoBox(string message)
        {
            TextBlock textBlock = new TextBlock();
            textBlock.Padding = new Thickness(3);
            textBlock.FontSize = 13;
            textBlock.Text = message;
            textBlock.TextWrapping = TextWrapping.Wrap;
            textBlock.Foreground = ColorHelper.GetColor("#808080");
            textBlock.HorizontalAlignment = HorizontalAlignment.Center;

            return textBlock;
        }

        private void input_message_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                btn_sendMessage_Click(sender, e);
            }
        }

        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            _client.Close();
        }

        private void Window_Closed(object sender, EventArgs e)
        {
            Environment.Exit(0);
        }

        private void Btn_Exit_Click(object sender, RoutedEventArgs e)
        {
            _client.Close();

            WindowHelper.ChangeWindow(this, new MainWindow());
        }
    }
}
