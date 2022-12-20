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
    public sealed partial class newUser : Page
    {
        public static string rl;
        public newUser()
        {
            this.InitializeComponent();
            listUser.ItemsSource = DataAccess.Users.getUser();
            conditionPassword.Visibility = Visibility.Collapsed;
            conditionDelai.Visibility = Visibility.Collapsed;
        }

        private void HandleCheck(object sender, RoutedEventArgs e)
        {
            RadioButton rb = sender as RadioButton;
            rl = rb.Name;
        }
        private void ajUser_Click(object sender, RoutedEventArgs e)
        {
            if(rl != null & mail.Text!= null & password.Text != null)
            {
                if (password.Text.Any(char.IsDigit) && password.Text.Any(char.IsLower) && password.Text.Any(char.IsUpper))
                {
                    conditionPassword.Visibility = Visibility.Collapsed;
                    DateTime tmp = DateTime.Now;
                    TimeSpan ts = tmp - DataAccess.Dtime;
                    if (ts.TotalSeconds < DataAccess.longueur)
                    {
                        conditionDelai.Visibility = Visibility.Collapsed;
                        DataAccess.Users.addUser(mail.Text, password.Text, rl);
                        rl = null;
                        Frame.Navigate(typeof(pages.newUser));
                    }
                    else
                    {
                        conditionDelai.Visibility = Visibility.Visible;
                    }
                }
                else
                {
                    conditionPassword.Visibility = Visibility.Visible;
                }
            }
        }

        private void supUser_Click(object sender, RoutedEventArgs e)
        {
            if (mail.Text != null)
            {
                DateTime tmp = DateTime.Now;
                TimeSpan ts = tmp - DataAccess.Dtime;
                if (ts.TotalSeconds < DataAccess.longueur)
                {
                    conditionDelai.Visibility = Visibility.Collapsed;
                    DataAccess.Users.delUser(mail.Text);
                    rl = null;
                    Frame.Navigate(typeof(pages.newUser));
                }
                else
                {
                    conditionDelai.Visibility = Visibility.Visible;
                }
            }
        }
    }
}
