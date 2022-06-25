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
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Calendar.DesktopClient.Controls
{
    /// <summary>
    /// Interaction logic for EventsControl.xaml
    /// </summary>
    public partial class EventsControl : UserControl
    {
        public event Action<Event> OnDelete;
        public Event SelectedEvent { 
            get 
            {
                return _lb.SelectedItem as Event;
            } 
        }
        public IReadOnlyCollection<Event> Events {
            get
            {
                return _lb.ItemsSource as IReadOnlyCollection<Event>;
            }
            set 
            {
                _lb.ItemsSource = value;
            } 
        }
        public EventsControl()
        {
            InitializeComponent();
        }

        private void OnDeleteClick(object sender, RoutedEventArgs e)
        {
            if (SelectedEvent!=null)
                OnDelete?.Invoke(SelectedEvent);
        }
    }
}
