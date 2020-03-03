using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CinemaTerminal
{
    public class Film
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int Length { get; set; }
        public byte[] Poster { get; set; }
        public int Age { get; set; }
        public string ShortDescription { get; set; }
        public int IdGenre { get; set; }
    }
}