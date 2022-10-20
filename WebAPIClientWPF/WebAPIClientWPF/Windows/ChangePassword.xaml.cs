using System.Windows;
using WebAPIClientWPF.Classes;

namespace WebAPIClientWPF.Windows
{
    public partial class ChangePassword : Window
    {
        private bool UnLockButtons
        {
            set
            {
                btCancel.IsEnabled = value;
                btSave.IsEnabled = value;
            }
        }

        public ChangePassword()
        {
            InitializeComponent();
        }

        private void BtCancel_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }

        private async void BtSave_Click(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrEmpty(tBOldPassword.Password))
            {
                popupOldPassword.ShowPopup("Enter old password");
                tBOldPassword.Focus();
                return;
            }
            if (string.IsNullOrEmpty(tBNewPassword.Password))
            {
                popupNewPassword.ShowPopup("Enter a new password");
                tBNewPassword.Focus();
                return;
            }
            UnLockButtons = false;
            if (!await HttpRequest.TryLogin(Authorization.CurrentLogin, tBOldPassword.Password))
            {
                UnLockButtons = true;
                popupOldPassword.ShowPopup("Wrong old password");
                tBOldPassword.Clear();
                tBOldPassword.Focus();
                return;
            }
            if (await HttpRequest.UpdatePassword(Authorization.CurrentLogin, tBNewPassword.Password))
            {
                MessageBox.Show("Password changed successfully", "Successfully", MessageBoxButton.OK, MessageBoxImage.Information);
                Close();
            }
            UnLockButtons = true;
        }
    }
}
