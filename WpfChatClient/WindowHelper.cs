using System.Windows;

namespace WpfChatClient
{
    public static class WindowHelper
    {
        public static void ChangeWindow(Window currentWindow, Window newWindow)
        {
            currentWindow.Visibility = Visibility.Hidden;
            newWindow.Show();
        }
    }
}
