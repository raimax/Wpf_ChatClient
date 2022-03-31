using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

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
                _client.Listen(AddServerMessageToList, AddServerInfoMessageToList, AddOnlineUserToList, AddServerFileToMessageList);
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

        private void AddServerMessageToList(string message, string username)
        {
            StackPanel messageBox = GenerateServerMessageBox(ColorHelper.GetColor("#303030"), HorizontalAlignment.Left, username, message);
            MessageList.Children.Add(messageBox);
        }

        private void AddServerInfoMessageToList(string message)
        {
            TextBlock infoBox = GenerateInfoBox(message);
            MessageList.Children.Add(infoBox);
        }

        private void AddOnlineUserToList(List<string> users)
        {
            OnlineUsersList.Children.Clear();

            foreach (string username in users)
            {
                Border userBox = GenerateOnlineUserCard(username);
                OnlineUsersList.Children.Add(userBox);
            }
        }

        private void AddUserFileToMessageList(string fileName)
        {
            Border fileBox = GenerateFileBox(ColorHelper.GetColor("#19a3ff"), HorizontalAlignment.Right, fileName);
            MessageList.Children.Add(fileBox);
        }
        private void AddServerFileToMessageList(string fileName)
        {
            Border fileBox = GenerateFileBox(ColorHelper.GetColor("#303030"), HorizontalAlignment.Left, fileName);
            MessageList.Children.Add(fileBox);
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

        private StackPanel GenerateServerMessageBox(SolidColorBrush color, HorizontalAlignment alignment, string username, string message)
        {
            StackPanel stackPanel = new StackPanel();

            Border border = new Border();
            border.Margin = new Thickness(0, 0, 0, 2);
            border.Background = color;
            border.CornerRadius = new CornerRadius(15);
            border.HorizontalAlignment = alignment;

            Label label = new Label();
            label.Content = username;
            label.FontSize = 11;
            label.Foreground = ColorHelper.GetColor("#ddd");
            label.Padding = new Thickness(8, 0, 0, 0);

            TextBlock textBlock = new TextBlock();
            textBlock.Padding = new Thickness(8);
            textBlock.FontSize = 16;
            textBlock.Text = message;
            textBlock.TextWrapping = TextWrapping.Wrap;
            textBlock.Foreground = ColorHelper.GetColor("#fff");

            border.Child = textBlock;

            stackPanel.Children.Add(label);
            stackPanel.Children.Add(border);

            return stackPanel;
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

        private Border GenerateOnlineUserCard(string username)
        {
            Border border = new Border();
            border.Padding = new Thickness(20, 5, 20, 5);

            Grid grid = new Grid();
            ColumnDefinition column = new ColumnDefinition();
            column.Width = GridLength.Auto;
            ColumnDefinition column2 = new ColumnDefinition();
            column2.Width = new GridLength(1, GridUnitType.Star);
            grid.ColumnDefinitions.Add(column);
            grid.ColumnDefinitions.Add(column2);

            Ellipse ellipse = new Ellipse();
            ellipse.SetValue(Grid.ColumnProperty, 0);
            ellipse.Height = 50;
            ellipse.Width = 50;
            ellipse.HorizontalAlignment = HorizontalAlignment.Left;
            ImageBrush imageBrush = new ImageBrush();
            imageBrush.ImageSource = new BitmapImage(new Uri("pack://application:,,,/images/avatar1.jpg"));
            ellipse.Fill = imageBrush;

            grid.Children.Add(ellipse);

            Grid grid1 = new Grid();
            grid1.SetValue(Grid.ColumnProperty, 1);
            grid1.Margin = new Thickness(5, 0, 5, 0);
            RowDefinition row = new RowDefinition();
            row.Height = new GridLength(1, GridUnitType.Star);
            //RowDefinition row1 = new RowDefinition();
            //row1.Height = new GridLength(1, GridUnitType.Star);
            grid1.RowDefinitions.Add(row);
            //grid1.RowDefinitions.Add(row1);

            Label label = new Label();
            label.SetValue(Grid.RowProperty, 0);
            label.Content = username;
            label.FontSize = 14;
            label.VerticalAlignment = VerticalAlignment.Center;
            label.Foreground = ColorHelper.GetColor("#fff");

            //Label label2 = new Label();
            //label2.SetValue(Grid.RowProperty, 1);
            //label2.Content = "Description";
            //label2.VerticalAlignment = VerticalAlignment.Top;
            //label2.Foreground = ColorHelper.GetColor("#fff");

            grid1.Children.Add(label);
            //grid1.Children.Add(label2);

            grid.Children.Add(grid1);

            border.Child = grid;

            return border;
        }

        private Border GenerateFileBox(SolidColorBrush color, HorizontalAlignment alignment, string fileName)
        {
            Border border = new Border();
            border.Margin = new Thickness(0, 0, 0, 2);
            border.Background = color;
            border.CornerRadius = new CornerRadius(4);
            border.HorizontalAlignment = alignment;
            border.Padding = new Thickness(8);
            border.Cursor = Cursors.Hand;
            border.AddHandler(MouseLeftButtonUpEvent, new RoutedEventHandler(FileBox_Click), true);

            StackPanel stackPanel = new StackPanel();
            stackPanel.Orientation = Orientation.Horizontal;

            Image image = new Image();
            image.Source = new BitmapImage(new Uri("pack://application:,,,/images/download.png"));
            image.Height = 25;
            image.Width = 25;
            Label label = new Label();
            label.Content = fileName;
            label.Foreground = ColorHelper.GetColor("#fff");

            stackPanel.Children.Add(image);
            stackPanel.Children.Add(label);

            border.Child = stackPanel;

            return border;
        }

        private async void FileBox_Click(object sender, RoutedEventArgs e)
        {
            await _client.RequestFile(GetFileName(sender));
        }

        private string GetFileName(object sender)
        {
            StackPanel stackPanel = (StackPanel)(sender as Border).Child;
            string childname = "";
            foreach (UIElement element in stackPanel.Children)
            {
                if (element is Label)
                {
                    childname = (element as Label).Content.ToString();
                }
            }

            return childname;
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

            MainWindow mainWindow = new MainWindow();
            mainWindow.Input_Username.Text = _client.Username;

            WindowHelper.ChangeWindow(this, mainWindow);
        }

        private async void Btn_SendFile_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog fileDialog = new OpenFileDialog();

            if (fileDialog.ShowDialog() == true)
            {
                await _client.SendFile(fileDialog.SafeFileName, fileDialog.FileName);
                AddUserFileToMessageList(fileDialog.SafeFileName);
            }
        }
    }
}
