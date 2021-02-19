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

        public void CreatePerson(Person person)
        {

            // return crud.Create(person);
            crud.Create(person);
        }

        public void AddMother(Person person, Person mother)
        {
              person = crud.Read(person);
              mother = crud.Read(mother);

            person.Mother = mother.Id;

            crud.Update(person);
        }
        public void AddFather(Person person, Person father)
        {
            person = crud.Read(person);
            father = crud.Read(father);



            person.Father = father.Id;

            crud.Update(person);
        }

    }
}
