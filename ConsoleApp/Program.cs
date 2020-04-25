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
            //QueryAndUpdateBattle_Disconnected();
            //InsertNewSamuraiWithManyQuotes();
            //InsertNewSamuraiWithAQuote();
            //AddQuoteToExistingSamuraiWhileTracked();
            AddQuoteToExistingSamuraiNotTracked(2);
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
            foreach (var samurai in samurais)
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
            using (var newContextInstance = new SamuraiContext())
            {
                newContextInstance.Battles.Update(battle);
                newContextInstance.SaveChanges();
            }//context will start tracking object and mark its state as modified.
             //Here, EFCore knows only something got changed but donno what changed
             //so,in this case, even though only enddate changed,it will send all the values to backend

        }

        private static void InsertNewSamuraiWithAQuote()
        {
            var samurai = new Samurai
            {
                Name = "Kambei Shimada",
                Quotes = new List<Quote>
                {
                    new Quote{ Text="I've come to save you"}
                }
            };
            _context.Samurais.Add(samurai);
            _context.SaveChanges();
        }
        private static void InsertNewSamuraiWithManyQuotes()
        {
            var samurai = new Samurai
            {
                Name = "Kyuzo",
                Quotes = new List<Quote>
                {
                    new Quote{ Text="watch out of my sword!"},
                    new Quote {Text="I told you to watch out of the sharp sword! oh well!"}
                }
            };
            _context.Samurais.Add(samurai);
            _context.SaveChanges();
        }

        private static void AddQuoteToExistingSamuraiWhileTracked()
        {
            var samurai = _context.Samurais.FirstOrDefault();
            samurai.Quotes.Add(new Quote
            {
                Text = "I bet you are happy that I've saved you!"
            });//In this case, the context is tracking the samurai at the time we added the quote
            _context.SaveChanges();
        }


        //DISCONNECTED SCENARIO
        private static void AddQuoteToExistingSamuraiNotTracked(int samuraiId)
        {
            var samurai = _context.Samurais.Find(samuraiId);
            samurai.Quotes.Add(new Quote
            {
                Text = "Now I saved you, will you feed me dinner?"
            });
            using (var newContext = new SamuraiContext())
            {
                newContext.Samurais.Update(samurai);//here it sees samurai already has an Id which determines the Quote must be new because it doesnt have an Id.and default behaviour is Quotes FK value should be the value of samuraiId because they are connected.
                //so change traker takes all this info and response to new child of existing parent

                //newContext.Samurais.Update(samurai); //performance
                newContext.SaveChanges();
            }
        }//this works fine but we have extra update method>less performance
        //remedy::newContext.Samurais.Update(samurai);//>Attach connects the object and sets its state to unmodified


        private static void EagerLoadSamuraiWithQuotes()
        {
            //Include children
            var samuraiWithQuotes = _context.Samurais.Where(s => s.Name.Contains("Jimmy"))
                                                    .Include(s => s.Quotes).ToList();//Inclue> left joins to get quotes data

            //firstordefault,find > gives single samurai object not dbset.so include wont work on it
            /*//we can also use include on include.like getting traslated quotes after including Quotes
             ie.include children and grand children
            _context.Samurais.Include(s=>s.Quotes)
                                .ThenInclude(q=>q.Translations);*/

            /*//include just grandchildren
            _context.Samurais.Include(s=>s.Quotes.Translations)*/

            /*//include different children
            _context.Samurais.Include(s => s.Quotes)
                               .Include(s => s.Clan);*/

        }

        private static void ProjectSomeProperties()
        {
            var someProperties = _context.Samurais.Select(s => new { s.Id, s.Name }).ToList();
            //whereever we return above need not match to any type.(Anonymous type)
            //if we want to move that Anonymous type data to a type the do

            var idsAndNames = _context.Samurais.Select(s => new IdAndName(s.Id, s.Name)).ToList();
        }
        public struct IdAndName
        {
            public IdAndName(int id, string name)
            {
                Id = id;
                Name = name;
            }

            public int Id;
            public string Name;
        }



        private static void ProjectSamuraisWithQuotes()
        {
            /*var somePropertiesWithQuotes = _context.Samurais
                .Select(s => new { s.Id, s.Name,s.Quotes,s.Quotes.Count })
                .ToList();*/

            /*var somePropertiesWithQuotes = _context.Samurais
                .Select(s => new
                {
                    s.Id,
                    s.Name,
                    HappyQuotes = s.Quotes.Where(q => q.Text.Contains("happy"))
                })
                .ToList();*/

            //EFCore can only track entities recognized by the DBContext model.
            //Ananonymous types are not tracked
            //Entities that are properties of an anonymous type are tracked> ex: Quotes here

            var samuraisWithHappyQuotes = _context.Samurais
                .Select(s => new
                {
                    Samirai = s,
                    HappyQuotes = s.Quotes.Where(q => q.Text.Contains("happy"))
                })
                .ToList();
            var firstsamurai = samuraisWithHappyQuotes[0].Samirai.Name += "The Happiest";
        }

        private static void ExplicitLoadQuotes()
        {
            var samurai = _context.Samurais.FirstOrDefault(s => s.Name.Contains("jimmy"));
            _context.Entry(samurai).Collection(s => s.Quotes).Load();
            _context.Entry(samurai).Reference(s => s.Horse).Load();
        }

        private static void LazyLoadQuotes()
        {
            //by default lazy loading is off
            //to enable:
            //1.every single navigation property in the model must be virtual
            //2.add nuget> Microsoft.EntityFramework.Proxies
            //3.ModelBuilder.UseLazyLoadingProxies()

            var samurai = _context.Samurais.FirstOrDefault(s => s.Name.Contains("jimmy"));
            var quoteCount = samurai.Quotes.Count();
        }

        private static void FilteringWithRelatedData()
        {
            //Samurais who got quotes with happy.but i want to see only samurai not quote
            var samurais = _context.Samurais
                .Where(s => s.Quotes.Any(q => q.Text.Contains("happy")))//sub queries
                .ToList();
        }

        //CONNECTED:DBContext is aware of all changes made to objects that is it tracking
        private static void ModifyingRelatedDataWhenTracked()
        {
            var samurai = _context.Samurais.Include(s => s.Quotes).FirstOrDefault(s => s.Id == 2);
            samurai.Quotes[0].Text = "Did you hear that?";
            //_context.Quotes.Remove(samurai.Quotes[2]);//we can also delete while the change tracker is in scope
            _context.SaveChanges();
        }

        //DISCONNECTED:DBContext has no clue about history of objects before they are attached
        private static void ModifyingRelatedDataWhenNotTracked()
        {
            var samurai = _context.Samurais.Include(s => s.Quotes).FirstOrDefault(s => s.Id == 2);
            var quote = samurai.Quotes[0];
            quote.Text = "Did you hear that again?";
            using (var newContext = new SamuraiContext())
            {
                //newContext.Quotes.Update(quote);
                newContext.Entry(quote).State = EntityState.Modified;//now change tracker tracks only that 1 changed quote and sends only i update instead of 3,4 updates for all the quotes when we use .update()
                newContext.SaveChanges();
            }
        }

        //MANY TO MANY RELATIONSHIP
        //since there is no samuraiBattles DBSet,we can't use context.SamuraiBattles.Add().so use just .Add()
        private static void JoinBattleAndSamurai()
        {
            //samurai and battle already exists and we have their IDs
            var sbJoin = new SamuraiBattle { SamuraiId = 1, BattleId = 3 };
            _context.Add(sbJoin);
            _context.SaveChanges();

            //if i have a battle object in memory and want to specify that particular samurai fought in that battle
            var battle = _context.Battles.Find(1);
            battle.SamuraiBattles.Add(new SamuraiBattle { SamuraiId = 21 });
            _context.SaveChanges();

        }
        private static void RemoveJoinBetweenSamuraiAndBattleSimple()
        {
            var join = new SamuraiBattle { BattleId = 1, SamuraiId = 2 };
            _context.Remove(join);
            _context.SaveChanges();
        }


        private static void GetSamuraiWithBattles()
        {
            var samuraiWithBattle = _context.Samurais
                .Include(s => s.SamuraiBattles)
                .ThenInclude(sb => sb.Battle)
                .FirstOrDefault(samurai => samurai.Id == 2);

            //or we can use projection
            var samuraiWithBattlesCleaner = _context.Samurais.Where(s => s.Id == 2)
                .Select(s => new
                {
                    Samurai = s,
                    Battle = s.SamuraiBattles.Select(sb => sb.Battle)
                })
                .FirstOrDefault();
        }

        //ONE TO ONE RELATIONSHIP
        private static void AddNewSamuraiWithHorse()
        {
            var samurai = new Samurai { Name = "Jina Ujichika" };
            samurai.Horse = new Horse { Name = "Silver" };
            _context.Samurais.Add(samurai);
            _context.SaveChanges();
        }
        private static void AddNewHorseToSamuraiUsingId()
        {
            var horse = new Horse { Name = "Scout", SamuraiId = 2 };
            _context.Add(horse);
            _context.SaveChanges();
        }

        //if samurai object already in memory
        private static void AddNewHorseToSamuraiObject()
        {
            var samurai = _context.Samurais.Find(22);
            samurai.Horse = new Horse { Name = "Black Beauty" };
            _context.SaveChanges();
        }
        private static void AddNewHorseToDisconnectedSamuraiObject()
        {
            var samurai = _context.Samurais.AsNoTracking().FirstOrDefault(s => s.Id == 23);
            samurai.Horse = new Horse { Name = "Mr. Ed" };
            using (var newContext = new SamuraiContext())
            {
                newContext.Attach(samurai);
                newContext.SaveChanges();
            }
        }

        //if samurai and horse already in memory and then we replace the horse> it will delete horse from th DB and insert the horse.because the constraints wont allow horse without samurai
        private static void ReplaceHorse()
        {
            var samurai = _context.Samurais.Include(s => s.Horse).FirstOrDefault(s => s.Id == 23);

            //var samurai = _context.Samurais.Find(23);//works only if the horse is in the context
            samurai.Horse = new Horse { Name = "Trigger" };
            _context.SaveChanges();
        }

        private static void GetSamuraisWithHorse()
        {
            var samurai = _context.Samurais.Include(s => s.Horse).ToList();
        }
        private static void GetHorseWithSamurai()
        {
            var hourseWithoutSamurai = _context.Set<Horse>().Find(3);
            var horseWithSamurai = _context.Samurais.Include(s => s.Horse).FirstOrDefault(s => s.Horse.Id == 3);
            var horseWithSamurais = _context.Samurais
                                            .Where(s => s.Horse != null)
                                            .Select(s => new { Horse = s.Horse, Samurai = s })
                                            .ToList();
        }

        //ONE TO MANY> ONE Clan to MANY Samurais
        private static void GetsamuraiWithClan()
        {
            var samurai = _context.Samurais.Include(s => s.Clan).FirstOrDefault();
        }
        private static void GetClanWithSamurais()
        {
            //var clan=_context.Clans.Include(c=>c.?????)//can't do this

            var clan = _context.Clans.Find(3);
            var samuraiForClan = _context.Samurais.Where(s => s.Clan.Id == 3).ToList();
        }
    }
}
