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
        public InformationsLabel()
        {
            InitializeComponent();

            Log.Logged += Log_Logged;
        }

        void Log_Logged(object sender, LogEventArgs args) {
            lbInformation.Content += "\n" +  args.Message;
            Dispatcher.Invoke(DispatcherPriority.Render, EmptyDelegate);
        }

        public void Start() {
            lbInformation.Content = String.Empty;
            Visibility = Visibility.Visible;
            Dispatcher.Invoke(DispatcherPriority.Render, EmptyDelegate);

        }

        protected Action EmptyDelegate = delegate { };

        public void End() {
            Visibility = Visibility.Hidden;
        }
    }
}
