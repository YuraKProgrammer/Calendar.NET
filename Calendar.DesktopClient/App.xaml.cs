using Calendar.DesktopClient.Wrappers;
using Kalantyr.Auth.Client;
using System;
using System.Windows;

namespace Calendar.DesktopClient
{
    public partial class App
    {
        public static void ShowError(Exception error)
        {
            var e = error.GetBaseException();
            MessageBox.Show(e.Message + Environment.NewLine + Environment.NewLine + e.StackTrace);
        }

        public static IAuthClient CreateAuthClient()
        {
            var client = new AuthClient(new HttpClientFactory(Settings.Default.AuthServiceUrl));
            return new AuthClientRetryWrapper( new AuthClientWrapper(client));
        }
    }
}
