using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;

namespace WebAPIClientWPF.Classes
{
    static internal class Extensions
    {
        public static void ShowPopup(this Popup popup, string text)
        {
            popup.IsOpen = true;
            if (popup.Child == null)
            {
                MessageBox.Show("Popup has no TextBlock", "Failed to show popup", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }
            if (popup.Child is TextBlock textBlock)
            {
                textBlock.Text = text;
            }
        }
    }
}
