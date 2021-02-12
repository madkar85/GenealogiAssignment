using System;

namespace GenealogiAssignment
{
    class Program
    {
        static void Main(string[] args)
        {
            var db = new SQLDatabase();

            db.DropDatabase("Genealogy");
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
                Mother = 0,
                Father = 0,

            };

            var crud = new CRUD();

            crud.Create(Isabella);

           
        }
    }
}
