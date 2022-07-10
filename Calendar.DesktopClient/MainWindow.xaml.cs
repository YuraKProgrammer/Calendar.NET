using Calendar.DesktopClient.Client;
using Calendar.DesktopClient.Windows;
using Calendar.Models;
using Kalantyr.Auth.Client;
using Kalantyr.Auth.Models;
using System;
using System.Threading;
using System.Threading.Tasks;
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
            _ec.OnDelete += OnEventDelete;
            _ec.OnEdit += OnEventEdit;
        }

        private async void OnEventDelete(Event ev)
        {
            if (MessageBox.Show("Вы уверены, что хотите удалить событие?","Подтверждение",MessageBoxButton.YesNo) != MessageBoxResult.Yes)
                return;
            try
            {
                ICalendarClient client = new CalendarClient(_calendarClientFactory);
                var eventsResult = await client.DeleteEventAsync(_ec.SelectedEvent, _tokenInfo.Value, CancellationToken.None);
                if (eventsResult.Error != null)
                    throw new Exception(eventsResult.Error.Message);
                MessageBox.Show("Событие удалено");
                await LoadEventsAsync();
            }
            catch(Exception ex)
            {
                App.ShowError(ex);
            }
        }

        private async void OnEventEdit(Event ev)
        {
            var window = new AddEventWindow(ev) { Owner = this };
            if (window.ShowDialog() == true)
                try
                {
                    ICalendarClient client = new CalendarClient(_calendarClientFactory);
                    var r = await client.EditAsync(ev, _tokenInfo.Value, System.Threading.CancellationToken.None);
                    if (r.Error != null)
                        throw new Exception(r.Error.Message);
                    await LoadEventsAsync();
                }
                catch (Exception ex)
                {
                    App.ShowError(ex);
                }
        }

        private async void OnLogin_Click(object sender, RoutedEventArgs e)
        {
            var window = new LoginWindow {Owner=this};
            if (window.ShowDialog() == true)
            {
                _tokenInfo = window.Token;
                await LoadEventsAsync();
                TuneControls();
            }
        }

        private async void OnRegister_Click(object sender, RoutedEventArgs e)
        {
            var window = new CreateUserWindow() { Owner=this };
            if (window.ShowDialog() == true)
            {
                _tokenInfo = window.Token;
                await LoadEventsAsync();
                TuneControls();
            }
        }

        private async void OnLogout_Click(object sender, RoutedEventArgs e)
        {
            if (_tokenInfo != null)
            {
                try
                {
                    IAuthClient client = App.CreateAuthClient();
                    var logoutResult = await client.LogoutAsync(_tokenInfo.Value, CancellationToken.None);
                    if (logoutResult.Error != null)
                    {
                        throw new Exception(logoutResult.Error.Message);
                    }
                    _tokenInfo = null;
                    MessageBox.Show("Выход выполнен");
                    _ec.Events = null;
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
                   await LoadEventsAsync();
                }
                catch (Exception ex)
                {
                    App.ShowError(ex);
                }
        }

        private async Task LoadEventsAsync()
        {
            try
            {
                ICalendarClient client = new CalendarClient(_calendarClientFactory);
                var eventsResult = await client.GetEventsAsync(_tokenInfo.Value, CancellationToken.None);
                if (eventsResult.Error != null)
                    throw new Exception(eventsResult.Error.Message);
                _ec.Events = eventsResult.Result;
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
            _miLogin.IsEnabled = _tokenInfo == null;
        }
    }
}
