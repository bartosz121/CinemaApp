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

namespace CinemaApp.Pages.AdminPages
{
    /// <summary>
    /// Interaction logic for AdminTickets.xaml
    /// </summary>
    public partial class AdminTickets : Page
    {
        private dbContext context;
        private Ticket? selectedTicket;
        private List<User> usersComboBoxList;
        private List<Screening> screeningsComboBoxList;
        public AdminTickets()
        {
            InitializeComponent();
            context = new dbContext();
            usersComboBoxList = context.Users.ToList();
            screeningsComboBoxList = context.Screenings.Include(s => s.Movie).Where(s => s.Date >= DateTime.Now ).ToList();

            usersComboBox.ItemsSource = usersComboBoxList;
            screeningsComboBox.ItemsSource = screeningsComboBoxList;

            populateTicketsDataGrid();
        }

        private void populateTicketsDataGrid()
        {
            dataGrid.ItemsSource = context.Tickets.Include(t => t.User).Include(t => t.Screening).Include(t => t.Screening.Movie).ToList();
        }

        private void dataGridSelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            var item = (DataGrid)sender;
            var ticket = (Ticket)item.SelectedItem;

            selectedTicket = ticket;
            displaySelectedTicketData();
        }

        private void displaySelectedTicketData()
        {
            if (selectedTicket != null)
            {
                usersComboBox.SelectedIndex = usersComboBoxList.FindIndex(u => u.Id == selectedTicket.UserId);
                screeningsComboBox.SelectedIndex = screeningsComboBoxList.FindIndex(s => s.Id == selectedTicket.ScreeningId);
            }
        }

        private Ticket getFormData()
        {
            try
            {
                User? user = usersComboBoxList[usersComboBox.SelectedIndex];
                Screening? screening = screeningsComboBoxList[screeningsComboBox.SelectedIndex];

                return new Ticket { UserId = user.Id, ScreeningId = screening.Id };
            }
            catch
            {
                throw new ArgumentException("Invalid data");
            }
        }

        private void btnClickAdd(object sender, RoutedEventArgs e)
        {
            try
            {
                Ticket ticket = getFormData();
                context.Tickets.Add(ticket);
                context.SaveChanges();

                populateTicketsDataGrid();
                showMessage("Ticket added!");

            }
            catch (ArgumentException ex)
            {
                showMessage(ex.Message);
            }

        }

        private void btnClickUpdate(object sender, RoutedEventArgs e)
        {
            if (selectedTicket != null)
            {
                try
                {
                    Ticket ticket = getFormData();
                    selectedTicket.UserId = ticket.UserId;
                    selectedTicket.ScreeningId = ticket.ScreeningId;
                    context.SaveChanges();

                    populateTicketsDataGrid();

                    showMessage("Ticket updated!");
                }
                catch (ArgumentException ex)
                {
                    showMessage(ex.Message);
                }

            }
        }

        private void btnClickDelete(object sender, RoutedEventArgs e)
        {
            if (selectedTicket != null)
            {
                context.Tickets.Remove(selectedTicket);
                context.SaveChanges();
                selectedTicket = null;
                populateTicketsDataGrid();
                showMessage("Ticket deleted!");
            }
        }

        private void showMessage(string message)
        {
            if (MainSnackbar.MessageQueue is { } messageQueue)
            {
                Task.Factory.StartNew(() => messageQueue.Enqueue(message));
            }
        }
    }
}
