using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Text;

namespace GenealogiAssignment
{
    class CRUD
    {

        public string DatabaseName { get; set; } = "Genealogy";

        public void Create(Person person)
        {
            var db = new SQLDatabase();

            try
            {
                var connString = string.Format(db.ConnectionString, DatabaseName);
                using (var cnn = new SqlConnection(connString))
                {
                    cnn.Open();
                    var sql = "INSERT INTO FamilyTree (FirstName, LastName, BirthDate, DeathDate, Father, Mother) VALUES (@FirstName, @LastName, @BirthDate, @DeathDate, @Father, @Mother)";
                    using (var command = new SqlCommand(sql, cnn))
                    {
                        command.Parameters.AddWithValue("@FirstName", person.FirstName);
                        command.Parameters.AddWithValue("@LastName", person.LastName);
                        command.Parameters.AddWithValue("@BirthDate", person.BirthDate);
                        command.Parameters.AddWithValue("@DeathDate", person.DeathDate);
                        command.Parameters.AddWithValue("@Father", person.Father);
                        command.Parameters.AddWithValue("@Mother", person.Mother);
                        command.ExecuteNonQuery();
                    }
                }
            }
            catch (System.Exception ex)
            {
                System.Console.WriteLine(ex.Message);
            }
           
        }

        public void Delete(Person person)
        {

        }
        /*
        public bool DoesPersonExist(string name)
        {

        }
        public bool DoesPersonExist(int id)
        {

        }
     
        public void GetFather (string name)
        {

        }

        public void GetFather (int id)
        {

        }

        public void GetMother (string name)
        {

        }

        public void GetMother(int id)
        {

        }

        public List<Person> List(string filter = "firstName LIKE @input", string paramValue)
        {

        }

        public Person Read (string name)
        {

        }
        public void Update(Person person)
        {

        }
        */



    }
}
