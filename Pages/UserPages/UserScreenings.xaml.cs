using CinemaApp.Context;
using CinemaApp.Models;
using Microsoft.EntityFrameworkCore;
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

namespace CinemaApp.Pages.UserPages
{
    /// <summary>
    /// Interaction logic for UserScreenings.xaml
    /// </summary>
    public partial class UserScreenings : Page
    {
        private dbContext context;
        private User user;
        private Screening? selectedScreening;
        public UserScreenings(User u)
        {
            InitializeComponent();
            user = u;
            context = new dbContext();

            dataGrid.ItemsSource = context.Screenings
            .Where(s => s.Date >= DateTime.Now)
            .Include(s => s.Room)
            .Include(s => s.Movie).ToList();
        }

        private void dataGridSelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            var item = (DataGrid)sender;
            var screening = (Screening)item.SelectedItem;

            selectedScreening = screening;

            displaySelectedScreeningData();
        }

        private void displaySelectedScreeningData()
        {
            if (selectedScreening != null)
            {
                txtTitle.Text = selectedScreening.Movie.Title;
                txtDate.Text = selectedScreening.Date.ToString("dd/MM/yyyy");
                txtRoom.Text = selectedScreening.Room.Name;
                txtPrice.Text = selectedScreening.Price.ToString();
            }
        }

        private void btnBuyClick(object sender, RoutedEventArgs e)
        {
            if (selectedScreening != null)
            {
                Ticket ticket = new Ticket { ScreeningId = selectedScreening.Id, UserId = user.Id};
                context.Tickets.Add(ticket);
                context.SaveChanges();

                if (MainSnackbar.MessageQueue is { } messageQueue)
                {
                    var message = $"Ticket for '{selectedScreening.Movie.Title}' bought!";
                    Task.Factory.StartNew(() => messageQueue.Enqueue(message));
                }
            }
        }
    }
}
