using System.Windows.Media;

namespace WpfChatClient
{
    public static class ColorHelper
    {
        public static SolidColorBrush GetColor(string colorCode)
        {
            return (SolidColorBrush)new BrushConverter().ConvertFromString(colorCode);
        }
    }
}
