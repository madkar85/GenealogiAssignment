using System;
using System.Collections.Generic;
using System.Text;

namespace GenealogiAssignment
{
    class People
    {
        private readonly CRUD crud;

        public People(CRUD crud)
        {
            this.crud = crud;
        }

        internal Program Program
        {
            get => default;
            set
            {
            }
        }
         //Hänvisar till metoden i CRUD-klassen som skapar person i databasen
        public void CreatePerson(Person person)
        {

            
            crud.Create(person);
        }

        //Hänvisar till metoden i CRUD-klassen som uppdaterar personen med mammas id
        public void AddMother(Person person, Person mother)
        {
              person = crud.Read(person);
              mother = crud.Read(mother);

            person.Mother = mother.Id;

            crud.Update(person);
        }

        //Hänvisar till metoden i CRUD-klassen som uppdaterar personen med pappas id
        public void AddFather(Person person, Person father)
        {
            person = crud.Read(person);
            father = crud.Read(father);



            person.Father = father.Id;

            crud.Update(person);
        }

    }
}
