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
    }
}
