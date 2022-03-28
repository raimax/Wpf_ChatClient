using System;
using System.Windows;

namespace WpfChatClient
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        private async void btn_connect_Click(object sender, RoutedEventArgs e)
        {
            btn_connect.IsEnabled = false;
            btn_connect.Content = "Connecting...";
            btn_connect.Foreground = SystemColors.GrayTextBrush;

            try
            {
                Client client = new Client();
                client.Username = Input_Username.Text;
                await client.Connect(Input_IpAddress.Text, int.Parse(Input_Port.Text));

                WindowHelper.ChangeWindow(this, new ChatWindow(client));
            }
            catch (Exception ex)
            {
                btn_connect.IsEnabled = true;
                btn_connect.Content = "Connect";
                btn_connect.Foreground = SystemColors.WindowBrush;
                MessageBox.Show(ex.Message);
            }
        }
    }
}
