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
                DataAccess.Users.addUser(mail.Text, password.Text, rl);
                rl = null;
                Frame.Navigate(typeof(pages.newUser));
            }
        }

        private void supUser_Click(object sender, RoutedEventArgs e)
        {
            if (mail.Text != null)
            {
                DataAccess.Users.delUser(mail.Text);
                rl = null;
                Frame.Navigate(typeof(pages.newUser));
            }
        }
    }
}
