using System;
using System.Windows;

namespace Calendar.DesktopClient.Windows
{
    /// <summary>
    /// Окно для выбора диапазона дат
    /// </summary>
    public partial class DateRangeWindow
    {
        private const string TimeOfDayFormat = @"hh\:mm\:ss";

        /// <summary>
        /// Начальный момент времени
        /// </summary>
        public DateTime FromDate { get; private set; }

        /// <summary>
        /// Конечный момент времени
        /// </summary>
        public DateTime ToDate { get; private set; }

        public DateRangeWindow()
        {
            InitializeComponent();

            FromDate = DateTime.Now.AddDays(-3);
            ToDate = DateTime.Now.AddDays(3);

            _dpDateFrom.SelectedDate = FromDate;
            _tbTimeFrom.Text = FromDate.TimeOfDay.ToString(TimeOfDayFormat);

            _dpDateTo.SelectedDate = ToDate;
            _tbTimeTo.Text = ToDate.TimeOfDay.ToString(TimeOfDayFormat);
        }

        private void OnOkClick(object sender, RoutedEventArgs e)
        {
            try
            {
                if (_dpDateFrom.SelectedDate == null || _dpDateTo.SelectedDate == null)
                    throw new Exception("Выберите даты начала и конца");

                var timeFrom = TimeSpan.Parse(_tbTimeFrom.Text);
                FromDate = _dpDateFrom.SelectedDate.Value.Add(timeFrom);

                var timeTo = TimeSpan.Parse(_tbTimeTo.Text);
                ToDate = _dpDateTo.SelectedDate.Value.Add(timeTo);

                DialogResult = true;
            }
            catch (Exception exception)
            {
                App.ShowError(exception);
            }
        }
    }
}
