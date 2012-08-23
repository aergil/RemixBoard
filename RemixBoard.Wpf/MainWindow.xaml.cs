using System;
using System.Threading;
using System.Windows;
using System.Windows.Controls;
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
        }

        private void RafraichirToutAsync() {
            btRefresh.IsEnabled = false;
            stackPanel1.IsEnabled = false;
            DescriptionBrowser.Visibility = Visibility.Hidden;
            Informations.Start();

            ThreadStart start = delegate {
                                    var op = Dispatcher.BeginInvoke(DispatcherPriority.Loaded, new Action(RafraichirTout));
                                    op.Completed += Op_Completed;
                                };
            new Thread(start).Start();
        }

        private void RafraichirTout() {
            JobsSeeker.Instance.RefreshEntrepotJobs();
            AffichertousLesJobs();
        }

        private void Op_Completed(object sender, EventArgs e) {
            btRefresh.IsEnabled = true;
            stackPanel1.IsEnabled = true;
            Informations.End();
            DescriptionBrowser.Visibility = Visibility.Visible;
        }

        private void AffichertousLesJobsAsync() {
            ThreadStart start = () => Dispatcher.Invoke(DispatcherPriority.Loaded, new Action(AffichertousLesJobs));
            new Thread(start).Start();
        }

        private void AffichertousLesJobs() {
            tbVille.IsEnabled = true;
            tbContrat.IsEnabled = true;
            tbMotsClefs.IsEnabled = true;
            btRefresh.IsEnabled = true;

            tbVille.Text = String.Empty;
            tbContrat.Text = String.Empty;
            tbMotsClefs.Text = string.Empty;

            JobList.ItemsSource = Entrepots.Jobs.GetAll();
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
            JobList.ItemsSource = Entrepots.Jobs.Filtrer(contrat, ville, mots);
        }

        private void AfficherLesJobsFavorisAsync() {
            ThreadStart start = () => Dispatcher.Invoke(DispatcherPriority.Background, new Action(AfficherLesJobsFavoris));
            new Thread(start).Start();
        }

        private void AfficherLesJobsFavoris() {
            tbContrat.Text = String.Empty;
            tbVille.Text = String.Empty;
            tbMotsClefs.Text = string.Empty;

            tbVille.IsEnabled = false;
            tbContrat.IsEnabled = false;
            btRefresh.IsEnabled = false;
            tbMotsClefs.IsEnabled = false;

            JobList.ItemsSource = Entrepots.Jobs.GetByFavoris();
        }

        private void MettreAJourJobFavoriAsync(Job job) {
            ThreadStart start = () => Dispatcher.Invoke(DispatcherPriority.Background, new Action<Job>(MettreAJourJobFavori), job);
            new Thread(start).Start();
        }

        private void MettreAJourJobFavori(Job job) {
            Entrepots.Jobs.Update(job);
        }

        private void AfficherDescription(Job job) {
            var description = new JobHtml(job).Description();
            DescriptionBrowser.NavigateToString(description);
        }

        private void JobTemplate_Selected(object sender, System.Windows.Input.MouseButtonEventArgs e) {
            var stackPanel = ((StackPanel) sender);
            var job = ((Job) stackPanel.DataContext);

            AfficherDescription(job);

            if (stackPanelWithFocus != null)
                stackPanelWithFocus.Style = (Style) (Resources["JobPanel"]);
            stackPanel.Style = (Style) (Resources["JobPanelSelected"]);
            stackPanelWithFocus = stackPanel;
        }

        private void JobTemplate_Titre_MouseDown(object sender, System.Windows.Input.MouseButtonEventArgs e) {
            var labelTitre = ((Label) sender);
            var webDetailJob = ((Job) labelTitre.DataContext).Url;
            try {
                if (!string.IsNullOrEmpty(webDetailJob))
                    System.Diagnostics.Process.Start(webDetailJob);
            }
            catch {
                Log.Error(this, "Erreur lors de l'ouverture de l'url " + webDetailJob, null);
            }
        }

        private void JobTemplate_Entreprise_MouseDown(object sender, System.Windows.Input.MouseButtonEventArgs e) {
            var labelEntreprise = ((Label) sender);
            var entrepriseWebSite = ((Job) labelEntreprise.DataContext).EntrepriseWebSite;
            try {
                if (!string.IsNullOrEmpty(entrepriseWebSite))
                    System.Diagnostics.Process.Start(entrepriseWebSite);
            }
            catch {
                Log.Error(this, "Erreur lors de l'ouverture de l'url " + entrepriseWebSite, null);
            }
        }

        private void JobTemplate_Favori_Changed(object sender, RoutedEventArgs e) {
            var checkBox = ((CheckBox) sender);
            var job = (Job) checkBox.DataContext;
            if (checkBox.IsChecked != null)
                job.Favoris = checkBox.IsChecked.Value;

            MettreAJourJobFavoriAsync(job);
        }

        private void VilleOuContrat_KeyUp(object sender, System.Windows.Input.KeyEventArgs e) {
            FiltrerAsync();
        }

        private void TbMotsClefs_KeyUp(object sender, System.Windows.Input.KeyEventArgs e) {
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

            System.Diagnostics.Process.Start(e.Uri.AbsoluteUri);
            e.Cancel = true;
        }

        private StackPanel stackPanelWithFocus;
    }
}