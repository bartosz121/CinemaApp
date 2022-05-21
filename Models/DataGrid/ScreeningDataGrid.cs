using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CinemaApp.Models.DataGrid
{
    public class ScreeningDataGrid
    {
        public ScreeningDataGrid(int screeningId, string movieTitle, string roomName, double price, DateTime date, int ticketsLeft)
        {
            ScreeningId = screeningId;
            MovieTitle = movieTitle;
            RoomName = roomName;
            Price = price;
            Date = date;
            TicketsLeft = ticketsLeft;
        }

        public int ScreeningId { get; set; }

        public string MovieTitle { get; set; }

        public string RoomName { get; set; }

        public double Price { get; set; }

        public DateTime Date { get; set; }

        public int TicketsLeft { get; set; }

    }
}
