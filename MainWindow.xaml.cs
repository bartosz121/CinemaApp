using CinemaApp.Context;
using CinemaApp.Models;
using CinemaApp.Pages;
using CinemaApp.Pages.UserPages;
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
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace CinemaApp
{
    public partial class MainWindow : Window
    {
        private dbContext context;
        protected User user { get; set; }
        public MainWindow(User user)
        {
            InitializeComponent();
            this.context = new dbContext();
            this.user = user;
            MainFrame.Content = new UserScreenings(this.user);
        }

        private void exitApp(object sender, RoutedEventArgs e)
        {
            Application.Current.Shutdown();
        }

        private void screeningsBtnClick(object sender, RoutedEventArgs e)
        {
            MainFrame.Content = new UserScreenings(user);
            this.MenuToggleButton.IsChecked = false;
        }

        private void ticketsBtnClick(object sender, RoutedEventArgs e)
        {
            MainFrame.Content = new UserTickets(user);
            this.MenuToggleButton.IsChecked = false;
        }
    }
}
