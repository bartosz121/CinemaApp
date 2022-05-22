using CinemaApp.Context;
using CinemaApp.Models;
using CinemaApp.Models.DataGrid;
using CinemaApp.Models.Forms;
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
    /// Interaction logic for AdminScreenings.xaml
    /// </summary>
    public partial class AdminScreenings : Page
    {
        private dbContext context;
        private Screening? selectedScreening;
        private List<Movie> moviesComboBoxList;
        private List<Room> roomsComboBoxList;
        public AdminScreenings()
        {
            InitializeComponent();
            context = new dbContext();
            moviesComboBoxList = context.Movies.ToList();
            roomsComboBoxList = context.Rooms.ToList();

            movieComboBox.ItemsSource = moviesComboBoxList;
            roomComboBox.ItemsSource = roomsComboBoxList;
            populateScreeningsDataGrid();
        }

        private void populateScreeningsDataGrid()
        {
            List<ScreeningDataGrid> screeningDataGrid = new List<ScreeningDataGrid>();

            context.Screenings.Include(s => s.Movie).Include(s => s.Tickets).Include(s => s.Room).ToList().ForEach(s =>
            {
                int screeningBoughtTickets = context.Tickets.Where(t => t.ScreeningId == s.Id).Count();
                int ticketsLeft = s.Room.Capacity - screeningBoughtTickets;
                screeningDataGrid.Add(new ScreeningDataGrid(s.Id, s.Movie.Title, s.Room.Name, s.Price, s.Date, ticketsLeft));
            });

            dataGrid.ItemsSource = screeningDataGrid;
        }

        private void btnClickAdd(object sender, RoutedEventArgs e)
        {
            try
            {
                Screening screening = getFormData();
                context.Screenings.Add(screening);
                context.SaveChanges();

                populateScreeningsDataGrid();
                showMessage("Screening added!");

            }
            catch (ArgumentException ex) {
                showMessage(ex.Message);
            }
        }

        private void btnClickUpdate(object sender, RoutedEventArgs e)
        {
            if (selectedScreening != null)
            {
                try
                {
                    Screening data = getFormData();
                    selectedScreening.MovieId = data.MovieId;
                    selectedScreening.RoomId = data.RoomId;
                    selectedScreening.Date = data.Date;
                    selectedScreening.Price = data.Price;
                    context.SaveChanges();

                    populateScreeningsDataGrid();
                    showMessage("Screening updated!");

                }
                catch (ArgumentException ex)
                {
                    showMessage(ex.Message);
                }
            }

        }

        private void btnClickDelete(object sender, RoutedEventArgs e)
        {
            if (selectedScreening != null)
            {
                context.Screenings.Remove(selectedScreening);
                context.SaveChanges();
                selectedScreening = null;
                populateScreeningsDataGrid();

                showMessage("Screening deleted!");
            }
        }

        private void dataGridSelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            var item = (DataGrid)sender;
            var screening = (ScreeningDataGrid)item.SelectedItem;

            if (screening != null)
            {
                selectedScreening = context.Screenings.Where(s => s.Id == screening.ScreeningId).First();
                displaySelectedScreening();
            }
        }

        private void displaySelectedScreening()
        {
            if (selectedScreening != null)
            {
                txtPrice.Text = selectedScreening.Price.ToString();
                datePicker.SelectedDate = selectedScreening.Date;

                movieComboBox.SelectedIndex = moviesComboBoxList.FindIndex(m => m.Id == selectedScreening.MovieId);
                roomComboBox.SelectedIndex = roomsComboBoxList.FindIndex(r => r.Id == selectedScreening.RoomId);
            }
        }

        private void showMessage(string message)
        {
            if (MainSnackbar.MessageQueue is { } messageQueue)
            {
                Task.Factory.StartNew(() => messageQueue.Enqueue(message));
            }
        }

        private Screening getFormData()
        {
            try
            {
                Movie? movie = moviesComboBoxList[movieComboBox.SelectedIndex];
                Room? room = roomsComboBoxList[roomComboBox.SelectedIndex];
                double price = double.Parse(txtPrice.Text);

                DateTime date = (DateTime)datePicker.SelectedDate;

                return new Screening{ MovieId = movie.Id, RoomId = room.Id, Date = date, Price = price };
            }
            catch
            {
                throw new ArgumentException("Invalid data");
            }
        }

    }
}
