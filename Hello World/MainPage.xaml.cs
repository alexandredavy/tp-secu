using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Xml.Linq;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;
using System.Threading;
using Windows.System.Threading;

// Pour plus d'informations sur le modèle d'élément Page vierge, consultez la page https://go.microsoft.com/fwlink/?LinkId=402352&clcid=0x409

namespace Hello_World
{
    /// <summary>
    /// Une page vide peut être utilisée seule ou constituer une page de destination au sein d'un frame.
    /// </summary>
    public sealed partial class MainPage : Page
    {
        public static DateTime lastConnetion;
        public MainPage()
        {
            this.InitializeComponent();
            DataAccess.Clients.Initialize();
            DataAccess.Users.Initialize();
            DataAccess.Logs.Initialize();

            DataAccess.Logs.getData();

            frame.Navigate(typeof(pages.acceuil));

            //DataAccess.Clients.AddData("romain", "affaire","admin","admin");

            //DataAccess.Clients.DeleteData("romain", "admin", "admin");

            // affaire resid
            //clientsResidentiel.ItemsSource = DataAccess.Clients.GetData("affaire");//new List<string>() { "sfdsf", "hkfhjfhgc", "bvbvc ", "hgdcjc", "hjgdjc", "sfdsf", "hkfhjfhgc", "bvbvc ", "hgdcjc", "hjgdjc", "sfdsf", "hkfhjfhgc", "bvbvc ", "hgdcjc", "hjgdjc", "sfdsf", "hkfhjfhgc", "bvbvc ", "hgdcjc", "hjgdjc", "bvbvc ", "hgdcjc", "hjgdjc", "sfdsf", "hkfhjfhgc", "bvbvc ", "hgdcjc", "hjgdjc" };
        }

        private async void Button_Click(object sender, RoutedEventArgs e)
        {
            if ((lastConnetion == null | (DateTime.Now - lastConnetion).TotalSeconds > DataAccess.antiForceBrute) & DataAccess.tentativesEchoue < DataAccess.tentativep )
            {
                List<string> tmp = DataAccess.Users.connexion(inputMail.Text, inputPassword.Password);
                //test.Text = "role = " + tmp[1] + " statut = " + tmp[0];
                if (tmp[0] == "bonMotDePass")
                {
                    MediaElement mediaElement = new MediaElement();
                    var synth = new Windows.Media.SpeechSynthesis.SpeechSynthesizer();
                    Windows.Media.SpeechSynthesis.SpeechSynthesisStream stream = await synth.SynthesizeTextToStreamAsync(
                        "Bienvenue " + inputMail.Text);
                    mediaElement.SetSource(stream, stream.ContentType);
                    mediaElement.Play();
                    DataAccess.tentativesEchoue = 0;
                    affichage.Text = "";
                    DataAccess.Dmail = inputMail.Text;
                    DataAccess.Dpassword = inputPassword.Password;
                    DataAccess.Dtime = DateTime.Now;
                    frame.Navigate(typeof(pages.acceuil));
                    txtRole.Text = "Rôle :";
                    btparametre.Visibility = Visibility.Visible;
                    if (tmp[1] == "resid")
                    {
                        role.Text = "résidentiel";
                        btAffaires.Visibility = Visibility.Collapsed;
                        btResid.Visibility = Visibility.Visible;
                        btUser.Visibility = Visibility.Collapsed;
                        DataAccess.Logs.AddLog("connection d un préposé aux résidentiels");
                    }
                    else if (tmp[1] == "affaire")
                    {
                        role.Text = "affaire";
                        btAffaires.Visibility = Visibility.Visible;
                        btResid.Visibility = Visibility.Collapsed;
                        btUser.Visibility = Visibility.Collapsed;
                        DataAccess.Logs.AddLog("connection d un préposé aux affaires");
                    }
                    else
                    {
                        role.Text = "admin";
                        btAffaires.Visibility = Visibility.Visible;
                        btResid.Visibility = Visibility.Visible;
                        btUser.Visibility = Visibility.Visible;
                        DataAccess.Logs.AddLog("connection d un administrateur");
                    }
                }
                else
                {
                    lastConnetion = DateTime.Now;
                    DataAccess.tentativesEchoue += 1;
                    affichage.Text = "mauvais mot de passe";
                    DataAccess.Logs.AddLog("tentative de connection échouée");
                }
            }
            else if (DataAccess.tentativesEchoue > 5 )
            {
                affichage.Text = "anti-force brute, relancer";
            }
        }

        private void btAffaires_Click(object sender, RoutedEventArgs e)
        {
            frame.Navigate(typeof(pages.clientsdAffaire));
        }

        private void btResid_Click(object sender, RoutedEventArgs e)
        {
            frame.Navigate(typeof(pages.clientsResidentielsxaml));
        }

        private void btparametre_Click(object sender, RoutedEventArgs e)
        {
            frame.Navigate(typeof(pages.parametres));
        }

        private void btUser_Click(object sender, RoutedEventArgs e)
        {
            frame.Navigate(typeof(pages.newUser));
        }
    }
}
