using Calendar.DesktopClient.Client;
using Calendar.DesktopClient.Windows;
using Calendar.Models;
using Kalantyr.Auth.Client;
using Kalantyr.Auth.Models;
using System;
using System.Threading;
using System.Windows;

namespace Calendar.DesktopClient
{
    public partial class MainWindow : Window
    {
        private TokenInfo _tokenInfo;
        private readonly HttpClientFactory _calendarClientFactory = new HttpClientFactory(Settings.Default.CalendarServiceUrl);

        public MainWindow()
        {
            InitializeComponent();
            TuneControls();
        }

        private void OnLogin_Click(object sender, RoutedEventArgs e)
        {
            var window = new LoginWindow {Owner=this};
            if (window.ShowDialog() == true)
            {
                _tokenInfo = window.Token;
                TuneControls();
            }
        }

        private async void OnLogout_Click(object sender, RoutedEventArgs e)
        {
            if (_tokenInfo != null)
            {
                try
                {
                    IAuthClient client = new AuthClient(new HttpClientFactory(Settings.Default.AuthServiceUrl));
                    var logoutResult = await client.LogoutAsync(_tokenInfo.Value, CancellationToken.None);
                    if (logoutResult.Error != null)
                    {
                        throw new Exception(logoutResult.Error.Message);
                    }
                    _tokenInfo = null;
                    MessageBox.Show("Выход выполнен");
                    TuneControls();
                }
                catch (Exception ex)
                {
                    App.ShowError(ex);
                }
            }
        }

        private async void OnGetCount_Click(object sender, RoutedEventArgs e)
        {
            var window = new DateRangeWindow { Owner = this };
            if (window.ShowDialog() == true)
                try
                {
                    ICalendarClient client = new CalendarClient(_calendarClientFactory);
                    var r = await client.GetCountAsync(window.FromDate, window.ToDate, _tokenInfo.Value, System.Threading.CancellationToken.None);
                    if (r.Error != null)
                        throw new Exception(r.Error.Message);
                    MessageBox.Show(r.Result.ToString());
                }
                catch (Exception ex)
                {
                    App.ShowError(ex);
                }
        }

        private async void OnAddEvent_Click(object sender, RoutedEventArgs e)
        {
            var ev = new Event { Name = "Новое событие", Date = DateTime.Now };
            var window = new AddEventWindow(ev) { Owner = this };
            if (window.ShowDialog() == true)
                try
                {
                    ICalendarClient client = new CalendarClient(_calendarClientFactory);
                    var r = await client.AddAsync(ev, _tokenInfo.Value, System.Threading.CancellationToken.None);
                    if (r.Error != null)
                        throw new Exception(r.Error.Message);
                    MessageBox.Show("Событие добавлено");
                }
                catch (Exception ex)
                {
                    App.ShowError(ex);
                }
        }

        private void TuneControls()
        {
            _miLogout.IsEnabled = _tokenInfo != null;
            _miCalendar.IsEnabled = _tokenInfo != null;
        }
    }
}
