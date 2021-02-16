using System;
using System.Runtime.CompilerServices;

namespace GenealogiAssignment
{
    class Program
    {
        static void Main(string[] args)
        {
            var db = new SQLDatabase();

            db.DropDatabase("Genealogy");  //körs om man har databasen sen tidigare
            db.CreateDatabase("Genealogy", true);
            db.CreateTable("FamilyTree",
                    @"ID int NOT NULL IDENTITY (1,1),
                    FirstName varchar (50),
                    LastName varchar (50),
                    BirthDate varchar (10),
                    DeathDate varchar (10),
                    Father int,
                    Mother int");
            var Isabella = new Person
            {
                FirstName = "Isabella",
                LastName = "Jagekrans",
                BirthDate = "20190702",
                DeathDate = " ",
                Mother = -1,
                Father = -1,

            }; 

            var Madeleine = new Person
            {
                FirstName = "Madeleine",
                LastName = "Karlsson",
                BirthDate = "19851231",
                DeathDate = " ",
                Mother = -1,
                Father = -1,


            };

            
            var Mikael = new Person
            {
                FirstName = "Mikael",
                LastName = "Jagekrans",
                BirthDate = "19891012",
                DeathDate = " ",
                Mother = -1,
                Father = -1,

            }; 

            var crud = new CRUD();

           
            crud.Create(Isabella);
            crud.Create(Madeleine);
            crud.Create(Mikael);
            var person = crud.Read("Isabella");
            crud.AddMother(Isabella, Madeleine);
            crud.AddFather(Isabella, Mikael);
            person = crud.Read(Isabella);

            Print(person);

        }

        
        private static void Print(Person person)
        {
            Console.WriteLine($"{person.Id}, {person.FirstName}, {person.LastName}, {person.BirthDate}, {person.DeathDate}, {person.Mother}, {person.Father}");
        }
    }
}
