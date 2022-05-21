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
    /// Interaction logic for UserTickets.xaml
    /// </summary>
    public partial class UserTickets : Page
    {
        private dbContext context;
        private User user;
        private Ticket? selectedTicket;

        public UserTickets(User u)
        {
            InitializeComponent();
            user = u;
            context = new dbContext();

            populateTicketsDataGrid(getCheckboxValue());
        }

        private void populateTicketsDataGrid(bool showPast)
        {
            dataGrid.ItemsSource = context.Tickets
                .Where(t => t.UserId == this.user.Id)
                .Where(t => showPast ? t.Screening.Date >= DateTime.Now : true )
                .Include(t => t.Screening.Movie)
                .Include(t => t.Screening.Room).ToList();
        }

        private void dataGridSelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            var item = (DataGrid)sender;
            var ticket = (Ticket)item.SelectedItem;

            selectedTicket = ticket;
        }

        private void btnClickRefund(object sender, RoutedEventArgs e)
        {
            if (selectedTicket != null)
            {
                if (selectedTicket.Screening.Date < DateTime.Now)
                {
                    MessageBox.Show("Cant refund tickets from the past!");
                }
                else
                {
                    context.Tickets.Remove(selectedTicket);
                    context.SaveChanges();
                    selectedTicket = null;
                    populateTicketsDataGrid(getCheckboxValue());
                }
            }
        }

        private void filterBtnClicked(object sender, RoutedEventArgs e)
        {
            populateTicketsDataGrid(getCheckboxValue());
        }

        private bool getCheckboxValue()
        {
            var val = filterBtn.IsChecked;

            if (val == true)
            {
                return true;
            }
            return false;
        }
    }
}
