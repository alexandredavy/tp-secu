﻿using System;
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
    public sealed partial class parametres : Page
    {
        public parametres()
        {
            this.InitializeComponent();
            histLog.ItemsSource = DataAccess.Logs.GetLogs();
            delay.PlaceholderText = DataAccess.longueur.ToString();
            delayps.PlaceholderText= DataAccess.antiForceBrute.ToString();
            tentatvie.PlaceholderText = DataAccess.tentativep.ToString();

            if (DataAccess.Users.connexion(DataAccess.Dmail, DataAccess.Dpassword)[1] == "admin")
            {
                vlist.Visibility = Visibility.Visible;
                txtDelay.Visibility = Visibility.Visible;
                delay.Visibility = Visibility.Visible;
                delayps.Visibility = Visibility.Visible;
                txtDelayps.Visibility = Visibility.Visible;
                txtTentative.Visibility = Visibility.Visible;
                tentatvie.Visibility = Visibility.Visible;
            }
        }

        private void histLog_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            DateTime tmp = DateTime.Now;
            TimeSpan ts = tmp - DataAccess.Dtime;
            if (ts.TotalSeconds < DataAccess.longueur)
            {
                if (newpassword1.Text == newpassword2.Text)
                {
                    if (newpassword1.Text != "")
                    {
                        DataAccess.Users.changePassword(DataAccess.Dmail, DataAccess.Dpassword, newpassword1.Text);
                        DataAccess.Dpassword= newpassword1.Text;
                        DataAccess.Logs.AddLog("nouveau mot de passe");
                    }
                }
                if (delay.Text != "")
                {
                    DataAccess.longueur = int.Parse(delay.Text);
                    DataAccess.Logs.AddLog("nouveau delay de session");
                }
                if (delayps.Text != "")
                {
                    DataAccess.antiForceBrute = int.Parse(delayps.Text);
                    DataAccess.Logs.AddLog("nouveau delay de mot de passe");
                }
                if (tentatvie.Text != "")
                {
                    DataAccess.tentativep = int.Parse(tentatvie.Text);
                    DataAccess.Logs.AddLog("nouveau nombre de tentatives");
                }
                Frame.Navigate(typeof(parametres));
                DataAccess.Logs.savedata();
            }
        }

        private void delay_BeforeTextChanging(TextBox sender, TextBoxBeforeTextChangingEventArgs args)
        {
            args.Cancel = args.NewText.Any(c => !char.IsDigit(c));
        }

        private void delayps_BeforeTextChanging(TextBox sender, TextBoxBeforeTextChangingEventArgs args)
        {
            args.Cancel = args.NewText.Any(c => !char.IsDigit(c));
        }

        private void tentatvie_BeforeTextChanging(TextBox sender, TextBoxBeforeTextChangingEventArgs args)
        {
            args.Cancel = args.NewText.Any(c => !char.IsDigit(c));
        }
    }
}