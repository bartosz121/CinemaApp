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
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace CinemaApp.Pages.AdminPages
{
    /// <summary>
    /// Interaction logic for AdminUsers.xaml
    /// </summary>
    public partial class AdminUsers : Page
    {
        private dbContext context;
        private User? selectedUser;
        public AdminUsers()
        {
            InitializeComponent();
            context = new dbContext();

            populateUsersDataGrid();
        }

        private void populateUsersDataGrid()
        {
            dataGrid.ItemsSource = context.Users.ToList();
        }

        private void dataGridSelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            var item = (DataGrid)sender;
            var user = (User)item.SelectedItem;

            if (user != null)
            {
                selectedUser = context.Users.Where(u => u.Id == user.Id).First();
            }
        }

        private void btnClickDelete(object sender, RoutedEventArgs e)
        {
            if (selectedUser != null)
            {
                context.Users.Remove(selectedUser);
                context.SaveChanges();
                selectedUser = null;

                if (MainSnackbar.MessageQueue is { } messageQueue)
                {
                    var message = "User deleted!";
                    Task.Factory.StartNew(() => messageQueue.Enqueue(message));
                }

                populateUsersDataGrid();
            }
        }
    }
}
