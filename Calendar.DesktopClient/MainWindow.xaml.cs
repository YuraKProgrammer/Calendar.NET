using Calendar.DesktopClient.Client;
using Calendar.DesktopClient.Windows;
using Calendar.Models;
using Kalantyr.Auth.Models;
using System;
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
        }

        private void OnLogin_Click(object sender, RoutedEventArgs e)
        {
            var window = new LoginWindow {Owner=this};
            if (window.ShowDialog() == true)
            {
                _tokenInfo = window.Token;
            }
        }

        private async void OnGetCount_Click(object sender, RoutedEventArgs e)
        {
            var window = new DateRangeWindow { Owner = this };
            if (window.ShowDialog() == true)
            {
                ICalendarClient client = new CalendarClient(_calendarClientFactory);
                var r = await client.GetCountAsync(window.FromDate, window.ToDate, _tokenInfo.Value, System.Threading.CancellationToken.None);
                if (r.Error != null)
                    throw new Exception(r.Error.Message);
                MessageBox.Show(r.Result.ToString());
            }
        }

        private async void OnAddEvent_Click(object sender, RoutedEventArgs e)
        {
            var ev = new Event { Date = DateTime.Now.AddDays(1), Name = "Новое событие" };
            ICalendarClient client = new CalendarClient(_calendarClientFactory);
            var r = await client.AddAsync(ev, _tokenInfo.Value, System.Threading.CancellationToken.None);
        }
    }
}
