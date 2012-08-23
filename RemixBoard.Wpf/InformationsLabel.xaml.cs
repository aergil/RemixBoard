using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
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

            Log.Logged += new LogEventHandler(Log_Logged);
        }

        void Log_Logged(object sender, LogEventArgs args) {
            lbInformation.Content += "\n" +  args.Message;
            this.Dispatcher.Invoke(DispatcherPriority.Render, EmptyDelegate);
        }

        public void Start() {
            lbInformation.Content = String.Empty;
            this.Visibility = Visibility.Visible;
            this.Dispatcher.Invoke(DispatcherPriority.Render, EmptyDelegate);

        }

        protected Action EmptyDelegate = delegate() { };

        public void End() {
            this.Visibility = Visibility.Hidden;
        }
    }
}
