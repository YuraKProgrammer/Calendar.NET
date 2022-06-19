using Calendar.DesktopClient.Client;
using Calendar.DesktopClient.Windows;
using Calendar.Models;
using Kalantyr.Auth.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Calendar.DesktopClient
{
    public partial class MainWindow : Window
    {
        private TokenInfo _tokenInfo;
        private HttpClientFactory _calendarClientFactory = new HttpClientFactory(Settings.Default.CalendarServiceUrl);
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
            ICalendarClient client = new CalendarClient(_calendarClientFactory);
            var r = await client.GetCountAsync(new DateTime(2000, 10, 10), new DateTime(2020, 5, 5), _tokenInfo.Value, System.Threading.CancellationToken.None);
            if (r.Error != null)
                throw new Exception(r.Error.Message);
            MessageBox.Show(r.Result.ToString());
        }

        private async void OnAddEvent_Click(object sender, RoutedEventArgs e)
        {
            var ev = new Event { Date = DateTime.Now.AddDays(1), Name = "Новое событие" };
            ICalendarClient client = new CalendarClient(_calendarClientFactory);
            var r = await client.AddAsync(ev, _tokenInfo.Value, System.Threading.CancellationToken.None);
        }
    }
}
