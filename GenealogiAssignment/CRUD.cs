using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Text;

namespace GenealogiAssignment
{
    class CRUD
    {
        private static readonly string databaseName = "Genealogy";

        private readonly SQLDatabase db = new SQLDatabase
        {
            DatabaseName = databaseName
        };

        internal Program Program
        {
            get => default;
            set
            {
            }
        }

        public void Create(Person person)
        {
            try
            {
                var connString = string.Format(db.ConnectionString, databaseName);
                using var cnn = new SqlConnection(connString);
                cnn.Open();
                
                var sql = "INSERT INTO FamilyTree (FirstName, LastName, BirthDate, DeathDate, Father, Mother) VALUES (@FirstName, @LastName, @BirthDate, @DeathDate, @Father, @Mother)";

                using var command = new SqlCommand(sql, cnn);
                command.Parameters.AddWithValue("@FirstName", person.FirstName);
                command.Parameters.AddWithValue("@LastName", person.LastName);
                command.Parameters.AddWithValue("@BirthDate", person.BirthDate);
                command.Parameters.AddWithValue("@DeathDate", person.DeathDate);
                command.Parameters.AddWithValue("@Father", person.Father);
                command.Parameters.AddWithValue("@Mother", person.Mother);
                command.ExecuteNonQuery();

                //return Read(person);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);

                //throw ex;
            }

        }
     
        public void Update(Person person)
        {
            db.ExecuteSQL(@"UPDATE FamilyTree 
                    SET FirstName = @FirstName, 
                        LastName = @LastName, 
                        BirthDate = @BirthDate, 
                        DeathDate = @DeathDate, 
                        Father = @Father, 
                        Mother = @Mother
                    WHERE Id = @Id;",
                    ("@FirstName", person.FirstName),
                    ("@LastName", person.LastName),
                    ("@BirthDate", person.BirthDate),
                    ("@DeathDate", person.DeathDate),
                    ("@Father", person.Father.ToString()),
                    ("@Mother", person.Mother.ToString()),
                    ("@Id", person.Id.ToString())
                    );
        }


        public void Delete(Person person)
        {
            // börja med sqlkoden
            db.ExecuteSQL($"DELETE FROM FamilyTree WHERE Id = {person.Id}");
        }




        public Person DoesPersonExist(string name)
        {
            //börja med sqlkod

            DataTable dt;

            if (name.Contains(" "))
            {
                var names = name.Split(' ');
                dt = db.GetDataTable("SELECT * FROM FamilyTree WHERE FirstName LIKE @FirstName AND LastName LIKE @LastName",
                                            ("@FirstName", names[0]),
                                            ("@LastName", names[1]));
            }
            else
            {
                dt = db.GetDataTable("SELECT * FROM FamilyTree WHERE FirstName LIKE @name OR LastName LIKE @name", ("@name", name));
            }
            if (dt.Rows.Count == 0)
            {
                Console.WriteLine($"Personen finns inte. Vi skapar {name}");
                return null;
            }

            Console.WriteLine($"{name} finns redan i databasen. Vi lägger till denna som förälder.");
            return GetPersonObject(dt.Rows[0]);
        }

        public Person Read(string name)
        {
            

            DataTable dt;

            if (name.Contains(" "))
            {
                var names = name.Split(' ');
                dt = db.GetDataTable("SELECT * FROM FamilyTree WHERE FirstName LIKE '@FirstName' AND LastName LIKE '@LastName'",
                                            ("@FirstName", names[0]),
                                            ("@LastName", names[1]));
            }
            else
            {
                dt = db.GetDataTable("SELECT * FROM FamilyTree WHERE FirstName LIKE @name OR LastName LIKE @name", ("@name", name));
            }
            if (dt.Rows.Count == 0)
            {
                return null;
            }

            return GetPersonObject(dt.Rows[0]);
        }

        public Person Read(Person person)
        {
            var row = db.GetDataTable("SELECT * FROM FamilyTree WHERE FirstName LIKE @FirstName", ("@FirstName", person.FirstName));

            //Console.WriteLine(row.Rows.Count);

            if (row.Rows.Count == 0)
                return null;

            return GetPersonObject(row.Rows[0]);
        }

        private static Person GetPersonObject(DataRow row)
        {
            return new Person
            {
                
                FirstName = row["FirstName"].ToString(),
                LastName = row["LastName"].ToString(),
                BirthDate = row["BirthDate"].ToString(),
                DeathDate = row["DeathDate"].ToString(),
                Father = (int)row["Father"],
                Mother = (int)row["Mother"],
                Id = (int)row["Id"]
            };

        }

        public List<Person> ListOfAllPeople()
        {
            var sql = "SELECT * FROM FamilyTree";
            var data = db.GetDataTable(sql);

            var list = new List<Person>();
            foreach(DataRow row in data.Rows)
            {
                list.Add(GetPersonObject(row));
            }
            return list;
        }

        public List<Person> ShowAllWithSameStartLetter(string letter)
        {
            string sqlString = @"SELECT * FROM  FamilyTree WHERE FirstName LIKE @FirstName + '%'";
            
            var data = db.GetDataTable(sqlString, ("@FirstName", letter));
            //var data = db.GetDataTable(sqlString);


            var list = new List<Person>();
            foreach (DataRow row in data.Rows)
            {
                list.Add(GetPersonObject(row));
            }
            return list;
        }

        public List<Person> ShowAllWithoutParents()
        {

            var data = db.GetDataTable("SELECT * FROM  FamilyTree WHERE  Father = 0 AND Mother = 0");

            var list = new List<Person>();
            foreach (DataRow row in data.Rows)
            {
                list.Add(GetPersonObject(row));
            }
            return list;
        }

        public List<Person> ShowSiblings(Person person)
        {
            var dad = person.Father;
            var mom = person.Mother;
            
            var data = db.GetDataTable($"SELECT * FROM  FamilyTree WHERE Father = {dad} AND Mother = {mom};");

            

            var list = new List<Person>();
            foreach (DataRow row in data.Rows)
            {
                list.Add(GetPersonObject(row));
            }
            return list;
        }

        public List<Person> ShowMother(Person person)
        {
            var mom = person.Mother;
            var data = db.GetDataTable($"SELECT * FROM  FamilyTree WHERE {mom} = ID");
            

            var list = new List<Person>();
            foreach (DataRow row in data.Rows)
            {
                list.Add(GetPersonObject(row));
            }
            return list;
        }

      
            public List<Person> ShowFather(Person person)
            {
                var dad = person.Father;
                var data = db.GetDataTable($"SELECT * FROM  FamilyTree WHERE {dad} = ID");


                var list = new List<Person>();
                foreach (DataRow row in data.Rows)
                {
                    list.Add(GetPersonObject(row));
                }
                return list;
            }

    }
}
