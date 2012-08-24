using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Threading;
using RemixBoard.Core;

namespace RemixBoard.Wpf
{
    /// <summary>
    /// Interaction logic for InformationsLabel.xaml
    /// </summary>
    public partial class InformationsLabel : UserControl
    {
        public InformationsLabel() {
            InitializeComponent();

            Log.Logged += Log_Logged;
        }

        private void Log_Logged(object sender, LogEventArgs args) {
            Dispatcher.Invoke(DispatcherPriority.Render, new Action<string>(UpdateInformation), args.Message);
        }

        private void UpdateInformation(string message) {
            lbInformation.Content += "\n" + message;
        }

        public void Start() {
            lbInformation.Content = String.Empty;
            Visibility = Visibility.Visible;
        }

        public void End() {
            Visibility = Visibility.Hidden;
        }
    }
}