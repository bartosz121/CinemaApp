using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CinemaApp.Models
{
    public class Ticket
    {
        [Key]
        public int Id { get; set; }

        public int UserId { get; set; }

        public int ScreeningId { get; set; }

        public virtual User User { get; set; }

        public virtual Screening Screening { get; set; }
    }
}
