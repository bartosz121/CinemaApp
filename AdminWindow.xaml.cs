using CinemaApp.Context;
using CinemaApp.Models;
using CinemaApp.Pages.AdminPages;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace CinemaApp
{
    /// <summary>
    /// Interaction logic for AdminWindow.xaml
    /// </summary>
    public partial class AdminWindow : Window
    {
        private dbContext context;
        private User user { get; set; }
        public AdminWindow(User user)
        {
            InitializeComponent();
            this.user = user;
            this.context = new dbContext();
            MainFrame.Content = new AdminMovies();
        }

        private void exitApp(object sender, RoutedEventArgs e)
        {
            Application.Current.Shutdown();
        }

        private void drawerMoviesBtn(object sender, RoutedEventArgs e)
        {
            MainFrame.Content = new AdminMovies();
            MenuToggleButton.IsChecked = false;
        }

        private void drawerScreeningsBtn(object sender, RoutedEventArgs e)
        {
            MainFrame.Content = new AdminScreenings();
            MenuToggleButton.IsChecked = false;
        }

        private void drawerTicketsBtn(object sender, RoutedEventArgs e)
        {
            MainFrame.Content = new AdminTickets();
            MenuToggleButton.IsChecked = false;
        }

        private void drawerUsersBtn(object sender, RoutedEventArgs e)
        {
            MainFrame.Content = new AdminUsers();
            MenuToggleButton.IsChecked = false;
        }
    }
}
