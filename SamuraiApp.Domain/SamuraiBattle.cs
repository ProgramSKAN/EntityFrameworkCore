using System;
using System.Collections.Generic;
using System.Text;

namespace SamuraiApp.Domain
{
    public class SamuraiBattle//for join between samurai and the maany battles samurai fought
    {

        public SamuraiBattle()
        {
            throw new System.NotImplementedException();
        }

        public int SamuraiId { get; set; }//it will become FK fo samurai class
        public int BattleId { get; set; }//it will become FK for battle class
        public Samurai samurai { get; set; }//optional
        public Battle Battle { get; set; }//optional
    }
}
