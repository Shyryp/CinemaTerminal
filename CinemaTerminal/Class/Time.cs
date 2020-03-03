using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CinemaTerminal
{
    public class Time
    {
        public int Id { get; set; }
        public TimeSpan SessionTime { get; set; }
        public DateTime SessionDate { get; set; }
        public decimal Cost { get; set; }
        public int IdFilm { get; set; }
    }
}
