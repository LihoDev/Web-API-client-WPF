using System.Windows;
using WebAPIClientWPF.Classes;

namespace WebAPIClientWPF.Windows
{
    public partial class CreateAccount : Window
    {
        private bool UnLockButtons
        {
            set
            {
                btCancel.IsEnabled = value;
                btSave.IsEnabled = value;
            }
        }

        public CreateAccount()
        {
            InitializeComponent();
        }

        private void BtCancel_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }

        private async void BtSave_Click(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrEmpty(tBLogin.Text))
            {
                popupLogin.ShowPopup("Enter login");
                tBLogin.Focus();
                return;
            }
            if (string.IsNullOrEmpty(tbPassword.Password))
            {
                popupPassword.ShowPopup("Enter password");
                tbPassword.Focus();
                return;
            }
            if (string.IsNullOrEmpty(tbNickname.Text))
            {
                popupNickname.ShowPopup("Enter nickname");
                tbNickname.Focus();
                return;
            }
            UnLockButtons = false;
            User user = await HttpRequest.GetUser(tBLogin.Text);
            if (!string.IsNullOrEmpty(user.login))
            {
                UnLockButtons = true;
                popupLogin.ShowPopup("This login is busy");
                tBLogin.Focus();
                return;
            }
            if (await HttpRequest.AddUser(tBLogin.Text, tbPassword.Password, tbNickname.Text))
            {
                MessageBox.Show("Account created successfully", "Successfully", MessageBoxButton.OK, MessageBoxImage.Information);
                Close();
            }
            UnLockButtons = true;
        }
    }
}
