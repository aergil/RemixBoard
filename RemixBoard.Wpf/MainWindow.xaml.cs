using System;
using System.ComponentModel;
using System.Threading;
using System.Windows;
using System.Windows.Threading;
using RemixBoard.Core;
using RemixBoard.Core.JobsWebSiteSeeker;

namespace RemixBoard.Wpf
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow() {
            InitializeComponent();
            RafraichirToutAsync();

            JobListe.AfficherDescription += AfficherDescription;
        }

        private void AfficherDescription(object sender, JobEventHandlerArgs args) {
            var description = new JobHtml(args.Job).Description();
            DescriptionBrowser.NavigateToString(description);
        }

        private void RafraichirToutAsync() {
            btRefresh.IsEnabled = false;
            JobListe.IsEnabled = false;
            DescriptionBrowser.Visibility = Visibility.Hidden;
            Informations.Start();

            var backgroundWorker = new BackgroundWorker();
            backgroundWorker.DoWork += BackgroundWorker_DoWork;
            backgroundWorker.RunWorkerCompleted += BackgroundWorker_RunWorkerCompleted;
            backgroundWorker.RunWorkerAsync();
        }

        private void BackgroundWorker_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e) {
            JobListe.IsEnabled = true;
            Informations.End();
            DescriptionBrowser.Visibility = Visibility.Visible;

            AffichertousLesJobs();
        }

        private void BackgroundWorker_DoWork(object sender, DoWorkEventArgs e) {
            JobsSeeker.Instance.RefreshEntrepotJobs();
        }

        private void AffichertousLesJobsAsync() {
            ThreadStart start = () => Dispatcher.Invoke(DispatcherPriority.Input, new Action(AffichertousLesJobs));
            new Thread(start).Start();
        }

        private void AffichertousLesJobs() {
            ActiverLeHeader(true);

            JobListe.ItemsSource = Entrepots.Jobs.GetAll();
        }

        private void ActiverLeHeader(bool active) {
            tbVille.IsEnabled = active;
            tbContrat.IsEnabled = active;
            tbMotsClefs.IsEnabled = active;
            btRefresh.IsEnabled = active;

            tbVille.Text = String.Empty;
            tbContrat.Text = String.Empty;
            tbMotsClefs.Text = string.Empty;
        }

        private void FiltrerAsync() {
            var contrat = tbContrat.Text;
            var ville = tbVille.Text;
            var motsClefs = tbMotsClefs.Text;

            ThreadStart start = () => Dispatcher.Invoke(DispatcherPriority.Background,
                                                        new Action<string, string, string>(Filtrer),
                                                        contrat, ville, motsClefs);
            new Thread(start).Start();
        }

        private void Filtrer(string contrat, string ville, string motsClefs) {
            var mots = motsClefs.Split(' ');
            JobListe.ItemsSource = Entrepots.Jobs.Filtrer(contrat, ville, mots);
        }

        private void AfficherLesJobsFavorisAsync() {
            ThreadStart start = () => Dispatcher.Invoke(DispatcherPriority.Background, new Action(AfficherLesJobsFavoris));
            new Thread(start).Start();
        }

        private void AfficherLesJobsFavoris() {
            ActiverLeHeader(false);
            JobListe.ItemsSource = Entrepots.Jobs.GetByFavoris();
        }


        private void Filtres_KeyUp(object sender, System.Windows.Input.KeyEventArgs e) {
            FiltrerAsync();
        }

        private void RafraichirListeJobs_Click(object sender, RoutedEventArgs e) {
            RafraichirToutAsync();
        }

        private void AfficherFavoris_Checked(object sender, RoutedEventArgs e) {
            AfficherLesJobsFavorisAsync();
        }

        private void AfficherFavoris_Unchecked(object sender, RoutedEventArgs e) {
            AffichertousLesJobsAsync();
        }

        private void DescriptionBrowser_Navigating(object sender, System.Windows.Navigation.NavigatingCancelEventArgs e) {
            if (e.Uri == null) return;

            try {
                System.Diagnostics.Process.Start(e.Uri.AbsoluteUri);
                e.Cancel = true;
            }
            catch {
                Log.Error(this, "Erreur lors de l'ouverture de l'url " + e.Uri.AbsoluteUri, null);
            }
        }
    }
}