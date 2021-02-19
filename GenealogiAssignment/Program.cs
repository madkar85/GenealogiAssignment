using System;
using System.Runtime.CompilerServices;

namespace GenealogiAssignment
{
    class Program
    {
        static void Main(string[] args)
        {
            Welcome();

            var crud = new CRUD();

            var people = new People(crud);
            var Isabella = new Person
            {
                FirstName = "Isabella",
                LastName = "Jagekrans",
                BirthDate = "2019",
                DeathDate = " ",
                Mother = 0,
                Father = 0,
            };
            var Madeleine = new Person
            {
                FirstName = "Madeleine",
                LastName = "Karlsson",
                BirthDate = "1985",
                DeathDate = " ",
                Mother = 0,
                Father = 0,
            };

            var Mikael = new Person
            {
                FirstName = "Mikael",
                LastName = "Jagekrans",
                BirthDate = "1989",
                DeathDate = " ",
                Mother = 0,
                Father = 0,
            };

            Console.WriteLine($"Först ska vi skapa första personen, {Isabella.FirstName}.");
            Console.WriteLine();
            people.CreatePerson(Isabella);
            Console.WriteLine("Sedan ska vi skapa föräldrarna och lägga till hennes föräldrar, Madeleine och Mikael.");
            Console.WriteLine();
            people.CreatePerson(Mikael);
            var exists = crud.DoesPersonExist("Madeleine Karlsson");
            if (exists != null)
            {
                people.AddMother(Isabella, Madeleine);
            }
            else if (exists == null)
            {
                people.CreatePerson(Madeleine);
                people.AddMother(Isabella, Madeleine);
            }
            exists = crud.DoesPersonExist("Mikael Jagekrans");
            if (exists != null)
            {
                people.AddFather(Isabella, Mikael);
            }
            else if (exists == null)
            {
                people.CreatePerson(Madeleine);
                people.AddMother(Isabella, Madeleine);
            }

            Console.WriteLine("Nu lägger vi till resten av hennes familj.");
            Console.WriteLine("Tryck enter för att fortsätta.");
            Console.ReadLine();
            var Karin = new Person
            {
                FirstName = "Karin",
                LastName = "Karlsson",
                BirthDate = "1957",
                DeathDate = " ",
                Mother = 0,
                Father = 0,
            };
            var Kenth = new Person
            {
                FirstName = "Kenth",
                LastName = "Karlsson",
                BirthDate = "1957",
                DeathDate = " ",
                Mother = 0,
                Father = 0,
            };
            var Nina = new Person
            {
                FirstName = "Nina",
                LastName = "Jagekrans Krus",
                BirthDate = "1969",
                DeathDate = " ",
                Mother = 0,
                Father = 0,
            };
            var Per = new Person
            {
                FirstName = "Per",
                LastName = "Nauter",
                BirthDate = "1969",
                DeathDate = " ",
                Mother = 0,
                Father = 0,
            };
            var Theo = new Person
            {
                FirstName = "Theo",
                LastName = "Kattsson",
                BirthDate = "2018",
                DeathDate = " ",
                Mother = 0,
                Father = 0,
            };
            var Mercedes = new Person
            {
                FirstName = "Mercedes",
                LastName = "Kattsson",
                BirthDate = "2017",
                DeathDate = " ",
                Mother = 0,
                Father = 0,
            };
            var Benz = new Person
            {
                FirstName = "Benz",
                LastName = "Kattsson",
                BirthDate = "2017",
                DeathDate = " ",
                Mother = 0,
                Father = 0,
            };

            people.CreatePerson(Karin);
            people.CreatePerson(Kenth);
            people.CreatePerson(Nina);
            people.CreatePerson(Per);
            people.CreatePerson(Theo);
            people.CreatePerson(Mercedes);
            people.CreatePerson(Benz);

            people.AddFather(Madeleine, Kenth);
            people.AddMother(Madeleine, Karin);
            people.AddFather(Mikael, Per);
            people.AddMother(Mikael, Nina);
            people.AddFather(Theo, Mikael);
            people.AddMother(Theo, Madeleine);
            people.AddFather(Mercedes, Mikael);
            people.AddMother(Mercedes, Madeleine);
            people.AddFather(Benz, Mikael);
            people.AddMother(Benz, Madeleine);
            
            var person = crud.Read("Isabella");

            Console.WriteLine("En lista på alla i databasen");
            var listOfPeople = crud.ListOfAllPeople();
            foreach (var p in listOfPeople)
            {
                Print(p);
            }
            Console.ReadLine();

            Console.WriteLine("En lista på alla med förnamn som börjar på samma bokstav");
            var listOfSameLetterNames = crud.ShowAllWithSameStartLetter("M");
            foreach (var l in listOfSameLetterNames)
            {
                Print(l);
            }
            Console.ReadLine();

            Console.WriteLine("En lista på alla som saknar föräldrar");
            var peopleWithoutParents = crud.ShowAllWithoutParents();
            foreach (var human in peopleWithoutParents)
            {
                Print(human);
            }
            Console.ReadLine();

            person = crud.Read(Isabella);
            Console.WriteLine("En lista på alla som har syskon");
            var siblings = crud.ShowSiblings(person);
            foreach (var human in siblings)
            {
                Print(human);
            }
            Console.ReadLine();

            person = crud.Read(Madeleine);
            Console.WriteLine("Visa föräldrarna för en person");
            var mom = crud.ShowMother(person);
            foreach (var human in mom)
            {
                Print(human);
            }
            var dad = crud.ShowFather(person);
            foreach (var human in dad)
            {
                Print(human);
            }
            Console.Read();

            Console.WriteLine($"Nu tar vi bort en person ur databasen. Vi tar bort {Theo.FirstName}");
            crud.Delete(Theo);

        }

        public static void Welcome()
        {
            Console.WriteLine("Hej och välkommen till mitt familjeträd. Vi ska nu skapa en databas.");
            var db = new SQLDatabase();

            db.DropDatabase("Genealogy");  //körs om man har databasen sen tidigare
            db.CreateDatabase("Genealogy", true);
            db.CreateTable("FamilyTree",
                    @"ID int NOT NULL IDENTITY (1,1),
                    FirstName varchar (50),
                    LastName varchar (50),
                    BirthDate varchar (10),
                    DeathDate varchar (10),
                    Father int NOT NULL,
                    Mother int NOT NULL");
        }



        private static void Print(Person person)
        {
            Console.WriteLine($"ID: {person.Id}, Förnamn: {person.FirstName}, Efternamn: {person.LastName}, Födelseår: {person.BirthDate}, " +
                $" Dödsår: {person.DeathDate}, Mammas ID: {person.Mother}, Pappas ID: {person.Father}");
        }

    }
}