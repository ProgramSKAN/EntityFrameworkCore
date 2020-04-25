using Microsoft.EntityFrameworkCore;
using SamuraiApp.Data;
using SamuraiApp.Domain;
using System;
using System.Collections.Generic;
using System.Linq;

namespace ConsoleApp
{
    class Program
    {
        private static SamuraiContext _context = new SamuraiContext();
        public static void Main(string[] args)
        {
            _context.Database.EnsureCreated();
            //GetSamurais("Before Add:");
            //AddSamurai();
            //GetSamurais("After Add:");
            //InsertMultipleSamurais();
            //InsertVariousTypes();
            //QueryFilters();
            //RetrieveAndUpdateSamurai();
            //RetrieveAndUpdateMultipleSamurais();
            //InsertBattle();
            QueryAndUpdateBattle_Disconnected();
            Console.Write("press any key...");
            Console.ReadKey();
        }


        private static void InsertMultipleSamurais()
        {
            var samurai1 = new Samurai { Name = "andrew multi" };
            var samurai2 = new Samurai { Name = "rand multi" };
            var samurai3 = new Samurai { Name = "andrew multi" };
            var samurai4 = new Samurai { Name = "rand multi" };
            /*context.Samurais.Add(samurai);
            context.Samurais.Add(samurai2);
            or */

            _context.Samurais.AddRange(samurai1, samurai2, samurai3, samurai4);

            /*//or
            var samuraiList = new List<Samurai>();
            //add items to list
            context.Samurais.AddRange(samuraiList);*/

            _context.SaveChanges();
        }

        //track via DBSet (DBset indicatess type)> context.Samurais.Add(...);context.Samurais.AddRange(...);
        //track via DBContext (context will discover types automatically)> context.Add(...);context.AddRange(...);
        private static void InsertVariousTypes()
        {
            var samurai = new Samurai { Name = "mike" };
            var clan = new Clan { ClanName = "imperial clan" };
            _context.AddRange(samurai, clan);
            _context.SaveChanges();
        }

        private static void AddSamurai()
        {
            var samurai = new Samurai { Name = "jimmy" };
            _context.Samurais.Add(samurai);//DBContext "Tracks" or "change tracks" entities
            _context.SaveChanges();//it will rollback all(default) the transactions if error occurs
        }

        private static void GetSamurais(string text)
        {
            var samurais = _context.Samurais.ToList();
            Console.WriteLine($"{text}: Samurai count is {samurais.Count}");
            foreach(var samurai in samurais)
            {
                Console.WriteLine(samurai.Name);
            }

            /*//instead of converting to list and enumerating , we can diractly enumerate query.but causes performance issues since it access the data until foreach completes.so prefer tolist()
            var query = context.Samurais;
            foreach(var samurai in query)
            {
                Console.WriteLine(samurai.Name);
            }*/
        }

        private static void QueryFilters()
        {
            var samurais = _context.Samurais.Where(s => s.Name == "jimmy").ToList();
            //or//var samurais = _context.Samurais.Where(s => EF.Functions.Like(s.Name,"%jimmy%")).ToList();
            //or//var samurais = _context.Samurais.Where(s => s.Name.Contains("%jimmy%")).ToList();

            //find one
            //var samurai = _context.Samurais.Where(s => EF.Functions.Like(s.Name, "%jimmy%")).FirstOrDefault();
            //or//var samurai = _context.Samurais.FirstOrDefault(s => s.Name == "jimmy");

            //var samurai = _context.Samurais.FirstOrDefault(s => s.Id==2);
            //or//var samurai = _context.Samurais.Find(2);

            var last = _context.Samurais.OrderBy(s => s.Id).LastOrDefault(s => s.Name == "jimmy");//select top1 ......where name="jimmy" order by id desc.
            //lastordefault throws exception without order by
        }

        private static void RetrieveAndUpdateSamurai()
        {
            var samurai = _context.Samurais.FirstOrDefault();
            samurai.Name += "san";
            _context.SaveChanges();
        }

        //note:EFCore needs atleast 4objects that are either new,changed or deleted inorder to batch the commands
        private static void RetrieveAndUpdateMultipleSamurais()
        {
            //SKIP & TAKE combo are great for paging data
            var samurais = _context.Samurais.Skip(1).Take(3).ToList();
            samurais.ForEach(s => s.Name += "san");
            _context.SaveChanges();
        }

        private static void RetrieveAndDeleteSamurai()
        {
            var samurai = _context.Samurais.Find(18);
            _context.Samurais.Remove(samurai);
            _context.SaveChanges();
            //recommanded storedproc instead
        }

        private static void InsertBattle()
        {
            _context.Battles.Add(new Battle
            {
                Name = "Battle of Okehazama",
                StartDate = new DateTime(1560, 05, 01),
                EndDate = new DateTime(1560, 06, 15)
            });
            _context.SaveChanges();
        }

        //In disconnected scenarios(eg:website) where DB donno whats happening on the client DBset
        //its upto to you to inform the context about the object state
        //update the objects that EFCore has no clue about>>below
        private static void QueryAndUpdateBattle_Disconnected()
        {
            var battle = _context.Battles.AsNoTracking().FirstOrDefault();//emulate disconnected scenario
            battle.EndDate = new DateTime(1560, 06, 30);
            using(var newContextInstance=new SamuraiContext())
            {
                newContextInstance.Battles.Update(battle);
                newContextInstance.SaveChanges();
            }//context will start tracking object and mark its state as modified.
             //Here, EFCore knows only something got changed but donno what changed
             //so,in this case, even though only enddate changed,it will send all the values to backend

        }
    }
}
