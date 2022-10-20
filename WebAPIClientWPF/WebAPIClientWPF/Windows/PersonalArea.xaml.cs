using System.Windows;
using WebAPIClientWPF.Classes;

namespace WebAPIClientWPF.Windows
{
    public partial class PersonalArea : Window
    {
        private string _login;
        private bool UnLockButtons
        {
            set
            {
                btChangeNickname.IsEnabled = value;
                btChangePassword.IsEnabled = value;
                btDeleteAccount.IsEnabled = value;
            }
        }

        public PersonalArea(string login)
        {
            InitializeComponent();
            _login = login;
            FillFields();
        }

        private void BtExit_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }

        private void BtChangePassword_Click(object sender, RoutedEventArgs e)
        {
            ChangePassword changePassword = new ChangePassword();
            changePassword.ShowDialog();
            FillFields();
        }

        private void BtChangeNickname_Click(object sender, RoutedEventArgs e)
        {
            ChangeNickname changeNickname = new ChangeNickname();
            changeNickname.ShowDialog();
            FillFields();
        }

        private async void BtDeleteAccount_Click(object sender, RoutedEventArgs e)
        {
            if (MessageBox.Show("Are you sure you want to delete your account?", "Account deleting", MessageBoxButton.YesNo, MessageBoxImage.Question)== MessageBoxResult.Yes)
            {
                UnLockButtons = false;
                if (await HttpRequest.DeleteRequest(Authorization.CurrentLogin))
                {
                    Close();
                }
                UnLockButtons = true;
            }
        }

        private async void FillFields()
        {
            User user = await HttpRequest.GetUser(_login);
            labelNickname.Text = user.nickname;
            labelMoneyValue.Text = user.money.ToString();
            labelMoneyValue.Text = user.record.ToString();
        }
    }
}
