using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

// Pour plus d'informations sur le modèle d'élément Page vierge, consultez la page https://go.microsoft.com/fwlink/?LinkId=234238

namespace Hello_World.pages
{
    /// <summary>
    /// Une page vide peut être utilisée seule ou constituer une page de destination au sein d'un frame.
    /// </summary>
    public sealed partial class clientsdAffaire : Page
    {
        public clientsdAffaire()
        {
            this.InitializeComponent();
            clientsResidentiel.ItemsSource = DataAccess.Clients.GetData("affaire");
        }

        private void clientsResidentiel_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

        }

        private void supprimer(object sender, RoutedEventArgs e)
        {
            DateTime tmp = DateTime.Now;
            TimeSpan ts = tmp - DataAccess.Dtime;
            if (ts.TotalSeconds < DataAccess.longueur)
            {
                DataAccess.Clients.DeleteData(nomClient.Text, "affaire", DataAccess.Dmail, DataAccess.Dpassword);
                clientsResidentiel.ItemsSource = DataAccess.Clients.GetData("affaire");
                DataAccess.Logs.AddLog("Supprimer Client d affaires");
            }
        }

        private void ajouter(object sender, RoutedEventArgs e)
        {
            DateTime tmp = DateTime.Now;
            TimeSpan ts = tmp - DataAccess.Dtime;
            if (ts.TotalSeconds < DataAccess.longueur)
            {
                DataAccess.Clients.AddData(nomClient.Text, "affaire", DataAccess.Dmail, DataAccess.Dpassword);
                clientsResidentiel.ItemsSource = DataAccess.Clients.GetData("affaire");
                DataAccess.Logs.AddLog("Ajout Client d affaires");
            }
        }
    }
}
