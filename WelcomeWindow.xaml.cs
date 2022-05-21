using CinemaApp.Context;
using CinemaApp.Models;
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
    /// Interaction logic for WelcomeWindow.xaml
    /// </summary>
    public partial class WelcomeWindow : Window
    {
        private dbContext context;
        public WelcomeWindow()
        {
            InitializeComponent();
            this.context = new dbContext();
        }

        private void exitApp(object sender, RoutedEventArgs e)
        {
            Application.Current.Shutdown();
        }

        private void handleSignInClick(object sender, RoutedEventArgs e)
        {
            string username = txtUsername.Text;
            string password = txtPassword.Password.ToString();

            bool isDataValid = context.Users.Any(u => u.Username == username && u.Password == password);

            if (isDataValid)
            {
                User user = context.Users.Single(u => u.Username == username);
                Window nextWindow;
                if (user.isAdmin)
                {
                    nextWindow = new AdminWindow(user);
                }
                else
                {
                    nextWindow = new MainWindow(user);
                }

                nextWindow.Show();
                this.Close();
            }
            else
            {
                MessageBox.Show("Invalid login credentials.");
            }
        }

        private void handleSignUpClick(object sender, RoutedEventArgs e)
        {
            string username = txtUsername.Text;
            string password = txtPassword.Password.ToString();

            if (String.IsNullOrEmpty(username) || String.IsNullOrEmpty(password))
            {
                MessageBox.Show("Invalid data");
                return;
            }

            bool usernameExists = context.Users.Any(u => u.Username == username);

            if (usernameExists)
            {
                MessageBox.Show("User with this username already exists.");
                return;
            }

            if (password.Length < 8)
            {
                MessageBox.Show("Password length must be > 8");
            } else
            {
                User user = new User { Username=username, Password=password};
                context.Users.Add(user);
                context.SaveChanges();
                MessageBox.Show("Success!");
                handleSignInClick(sender, e);
            }


        }
    }
}
