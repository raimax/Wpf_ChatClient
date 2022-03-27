﻿using System;
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
                await client.Connect("127.0.0.1", 6666);

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