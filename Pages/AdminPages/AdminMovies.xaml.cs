using CinemaApp.Context;
using CinemaApp.Models;
using CinemaApp.Models.Forms;
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
    /// Interaction logic for AdminMovies.xaml
    /// </summary>
    public partial class AdminMovies : Page
    {
        private dbContext context;
        private Movie? selectedMovie;
        public AdminMovies()
        {
            InitializeComponent();
            context = new dbContext();
            dataGrid.ItemsSource = context.Movies.ToList();
        }

        private void refreshDataGrid()
        {
            dataGrid.ItemsSource = context.Movies.ToList();
        }

        private void displaySelectedMovieData()
        {
            if (selectedMovie != null)
            {
                txtTitle.Text = selectedMovie.Title;
                txtYear.Text = selectedMovie.Year.ToString();
            }
        }

        private void dataGridSelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            var item = (DataGrid)sender;
            var movie = (Movie)item.SelectedItem;

            selectedMovie = movie;
            displaySelectedMovieData();
        }

        private void btnClickAdd(object sender, RoutedEventArgs e)
        {
            MovieFormData data;
            try
            {
                data = getFormData();
                Movie movie = new Movie { Title = data.Title, Year = data.Year };
                context.Movies.Add(movie);

                context.SaveChanges();
                refreshDataGrid();

                showMessage("Movie added!");

            }
            catch (ArgumentException ex)
            {
                showMessage(ex.Message);
            }
        }
        private void btnClickUpdate(object sender, RoutedEventArgs e)
        {
            if (selectedMovie != null)
            {
                MovieFormData data;
                try
                {
                    data = getFormData();
                    selectedMovie.Title = data.Title;
                    selectedMovie.Year = data.Year;

                    context.SaveChanges();
                    refreshDataGrid();

                    showMessage("Movie updated!");
                }
                catch (ArgumentException ex)
                {
                    showMessage(ex.Message);
                }
            }
        }

        private void btnClickDelete(object sender, RoutedEventArgs e)
        {
            if (selectedMovie != null){ 
                context.Movies.Remove(selectedMovie);
                context.SaveChanges();
                selectedMovie = null;
                refreshDataGrid();

                showMessage("Movie deleted!");
            }

        }

        private void showMessage(string message)
        {
            if (MainSnackbar.MessageQueue is { } messageQueue)
            {
                Task.Factory.StartNew(() => messageQueue.Enqueue(message));
            }
        }

        private MovieFormData getFormData()
        {
            string title = txtTitle.Text;

            int year;
            MovieFormData data;

            try
            {
                year = int.Parse(txtYear.Text);
                data = new MovieFormData(title, year);
                return data;
            } catch
            {
                throw new ArgumentException("Invalid data");
            }
        }
    }
}
