using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace CinemaApp.Models
{
    public class User
    {
        [Key]
        public int Id { get; set; }
        public string Username { get; set; }
        public string Password { get; set; }

        [DefaultValue(false)]
        public bool isAdmin { get; set; }

        public virtual List<Ticket> Tickets { get; set; }
    }
}