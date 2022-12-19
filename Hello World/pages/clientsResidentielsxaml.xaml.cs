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
    public sealed partial class clientsResidentielsxaml : Page
    {
        public clientsResidentielsxaml()
        {
            this.InitializeComponent();
            clientsResidentiel.ItemsSource = DataAccess.Clients.GetData("resid");
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
                DataAccess.Clients.DeleteData(nomClient.Text,"resid", DataAccess.Dmail, DataAccess.Dpassword);
                clientsResidentiel.ItemsSource = DataAccess.Clients.GetData("resid");
                DataAccess.Logs.AddLog("Supprimer Client résidentiel");
            }
        }

        private void ajouter(object sender, RoutedEventArgs e)
        {
            DateTime tmp = DateTime.Now;
            TimeSpan ts = tmp - DataAccess.Dtime;
            if(ts.TotalSeconds < DataAccess.longueur)
            {
                DataAccess.Clients.AddData(nomClient.Text, "resid", DataAccess.Dmail, DataAccess.Dpassword);
                clientsResidentiel.ItemsSource = DataAccess.Clients.GetData("resid");
                DataAccess.Logs.AddLog("Ajout Client résidentiel");
            }
        }
    }
}
