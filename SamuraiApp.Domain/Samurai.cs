using System;
using System.Collections.Generic;
using System.Text;

namespace SamuraiApp.Domain
{
    public class Samurai
    {
        public Samurai()
        {
            Quotes = new List<Quote>();
            SamuraiBattles = new List<SamuraiBattle>();//to make sure SamuraiBattles is not null 
        }
        public int Id { get; set; }
        public string Name { get; set; }
        public List<Quote> Quotes { get; set; }//list of quotes samurai spokes in the movie.instantiate it in the constructor
        public Clan Clan { get; set; }//clan that samurai fights for

        public List<SamuraiBattle> SamuraiBattles { get; set; }

        public Horse Horse { get; set; }
    }
}
