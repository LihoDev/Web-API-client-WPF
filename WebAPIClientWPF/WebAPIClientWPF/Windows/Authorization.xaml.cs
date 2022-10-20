using System.Windows;
using WebAPIClientWPF.Classes;
using WebAPIClientWPF.Windows;

namespace WebAPIClientWPF
{
    public partial class Authorization : Window
    {
        public static string CurrentLogin { get; private set; }
        private bool UnLockButtons
        {
            set
            {
                btCreateAccount.IsEnabled = value;
                btEnter.IsEnabled = value;
            }
        }

        public Authorization()
        {
            InitializeComponent();
        }

        private async void BtEnter_Click(object sender, RoutedEventArgs e)
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
            UnLockButtons = false;
            if (!await HttpRequest.TryLogin(tBLogin.Text, tbPassword.Password))
            {
                MessageBox.Show("Wrong login or password", "Authorization failed", MessageBoxButton.OK, MessageBoxImage.Warning);
                UnLockButtons = true;
                return;
            }
            UnLockButtons = true;
            CurrentLogin = tBLogin.Text;
            tBLogin.Clear();
            tbPassword.Clear();
            PersonalArea personalArea = new PersonalArea(CurrentLogin);
            Hide();
            personalArea.ShowDialog();
            Show();
        }

        private void BtCreateAccount_Click(object sender, RoutedEventArgs e)
        {
            CreateAccount createAccount = new CreateAccount();
            Hide();
            createAccount.ShowDialog();
            Show();
        }
    }
}
