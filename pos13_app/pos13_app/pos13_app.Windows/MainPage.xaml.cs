using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Runtime.Serialization;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.Storage;
using Windows.Storage.Streams;
using Windows.UI.Popups;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

// The Blank Page item template is documented at http://go.microsoft.com/fwlink/?LinkId=234238
using pos13_app.Models;
using pos13_app.Modules;

namespace pos13_app
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class MainPage : Page
    {
        private ModCurrentUser CurrentUser;

        public pos13_app_services.Pos13_app_serviceClient service;
        public MainPage()
        {
            this.InitializeComponent();
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            service = new pos13_app_services.Pos13_app_serviceClient();
        }

        private void cmdQuit_Click(object sender, RoutedEventArgs e)
        {
            Application.Current.Exit();
        }

        private async void cmdLogin_Click(object sender, RoutedEventArgs e)
        {
            CurrentUser = new ModCurrentUser();

            string UserName = this.UserNameMem.Text;
            string Password = this.PasswordMem.Password;

            if (await service.isLoginAsync(UserName, Password))
            {
                CurrentUser.SaveCurrentUser(UserName, Password);
                this.Frame.Navigate(typeof (SysMenu));
            }
            else
            {
                this.Frame.Navigate(typeof (CannotLogin));
            }
        }

    }
}
 


