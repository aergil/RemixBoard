using System.Windows;
using RemixBoard.Core;
using RemixBoard.Datas;
using RemixBoard.Datas.Infrastructure;

namespace RemixBoard.Wpf
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        public App() {
            NhSessionManagement.Initialize();
            Entrepots.Jobs = new NhEntrepotJobs();
            Log.Info(this, "Application Start");
        }
    }
}