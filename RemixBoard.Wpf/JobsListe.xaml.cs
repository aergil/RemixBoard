using System;
using System.Collections.Generic;
using System.Threading;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Threading;
using RemixBoard.Core;

namespace RemixBoard.Wpf
{
    /// <summary>
    /// Affiche la liste des Jobs
    /// </summary>
    public partial class JobsListe : UserControl
    {
        public JobsListe()
        {
            InitializeComponent();
        }

        public IList<Job> ItemsSource {
            set { JobList.ItemsSource = value; }
        }

        private void JobTemplate_Selected(object sender, MouseButtonEventArgs e) {
            var stackPanel = ((StackPanel) sender);
            var job = ((Job) stackPanel.DataContext);

            if(AfficherDescription != null)
                AfficherDescription(sender, new JobEventHandlerArgs(job));

            if (stackPanelWithFocus != null)
                stackPanelWithFocus.Style = (Style) (Resources["JobPanel"]);
            stackPanel.Style = (Style) (Resources["JobPanelSelected"]);
            stackPanelWithFocus = stackPanel;
        }

        private void JobTemplate_Titre_MouseDown(object sender, MouseButtonEventArgs e) {
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

        private void JobTemplate_Entreprise_MouseDown(object sender, MouseButtonEventArgs e) {
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

        private void MettreAJourJobFavoriAsync(Job job)
        {
            ThreadStart start = () => Dispatcher.Invoke(DispatcherPriority.Background, new Action<Job>(MettreAJourJobFavori), job);
            new Thread(start).Start();
        }

        private void MettreAJourJobFavori(Job job)
        {
            Entrepots.Jobs.Update(job);
        }

        private StackPanel stackPanelWithFocus;
  


        public event JobEventHandler AfficherDescription;
  
    }
    public delegate void JobEventHandler(object sender, JobEventHandlerArgs args);
}