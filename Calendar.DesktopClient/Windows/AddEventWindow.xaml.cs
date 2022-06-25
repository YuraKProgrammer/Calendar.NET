using Calendar.Models;
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
using System.Windows.Shapes;

namespace Calendar.DesktopClient.Windows
{
    /// <summary>
    /// Interaction logic for AddEventWindow.xaml
    /// </summary>
    public partial class AddEventWindow : Window
    {
        private const string TimeOfDayFormat = @"hh\:mm\:ss";
        public Event Ev { get; private set; }
        public AddEventWindow()
        {
            InitializeComponent();
        }

        public AddEventWindow(Event ev) : this()
        {
            Ev = ev;
            _tb1.Text = ev.Name;
            _dp.SelectedDate = ev.Date;
            _tb2.Text = ev.Date.TimeOfDay.ToString(TimeOfDayFormat);
        }

        private void ButtonClick (object sender, RoutedEventArgs e)
        {
            try 
            {
                var timeFrom = TimeSpan.Parse(_tb2.Text);
                Ev.Date = _dp.SelectedDate.Value.Add(timeFrom);
                Ev.Name = _tb1.Text;
                DialogResult = true;
            }
            catch (Exception ex)
            {
                App.ShowError(ex);
            }
        }
    }
}
