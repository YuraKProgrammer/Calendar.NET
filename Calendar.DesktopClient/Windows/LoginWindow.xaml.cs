using Kalantyr.Auth.Client;
using Kalantyr.Auth.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace Calendar.DesktopClient.Windows
{
    /// <summary>
    /// Interaction logic for LoginWindow.xaml
    /// </summary>
    public partial class LoginWindow : Window
    {
        public LoginWindow()
        {
            InitializeComponent();
        }

        public TokenInfo Token { get; private set; }

        private async void Button_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                IAuthClient client = App.CreateAuthClient();
                LoginPasswordDto dto = new LoginPasswordDto { Login = _tb.Text, Password = _pb.Password };
                var loginResult = await client.LoginByPasswordAsync(dto,CancellationToken.None);
                if (loginResult.Error != null)
                {
                    throw new Exception(loginResult.Error.Message);
                }
                Token = loginResult.Result;
                DialogResult = true;
            }
            catch(Exception ex)
            {
                App.ShowError(ex);
            }
        }
    }
}
