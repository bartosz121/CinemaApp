using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CinemaApp.Models
{
    public class Screening
    {
        [Key]
        public int Id { get; set; }

        public int RoomId { get; set; }
        public int MovieId { get; set; }

        public double Price { get; set; }

        public DateTime Date { get; set; }

        public virtual List<Ticket> Tickets { get; set; }

        public virtual Room Room { get; set; }
        public virtual Movie Movie { get; set; }

    }
}
