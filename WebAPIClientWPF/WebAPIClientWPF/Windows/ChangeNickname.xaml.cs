using System.Windows;
using WebAPIClientWPF.Classes;

namespace WebAPIClientWPF.Windows
{
    public partial class ChangeNickname : Window
    {
        private bool UnLockButtons
        {
            set
            {
                btCancel.IsEnabled = value;
                btSave.IsEnabled = value;
            }
        }

        public ChangeNickname()
        {
            InitializeComponent();
        }

        private void BtCancel_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }

        private async void BtSave_Click(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrEmpty(tBNickname.Text))
            {
                popupNickname.ShowPopup("Enter nickname");
                tBNickname.Focus();
                return;
            }
            UnLockButtons = false;
            if (await HttpRequest.UpdateNickname(Authorization.CurrentLogin, tBNickname.Text))
            {
                MessageBox.Show("Nickname changed successfully", "Successfully", MessageBoxButton.OK, MessageBoxImage.Information);
                Close();
            }
            UnLockButtons = true;
        }
    }
}
