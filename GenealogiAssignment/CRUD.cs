using System;
using System.Collections.Generic;
using System.Data;
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
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }

        }
        public void AddMother(Person person, Person mother)
        {
            var db = new SQLDatabase
            {
                DatabaseName = "Genealogy"
            };

           person = Read(person);
           mother = Read(mother);

            

            person.Mother = mother.Id;

            Update(person);
        }
        public void AddFather(Person person, Person father)
        {
            var db = new SQLDatabase
            {
                DatabaseName = "Genealogy"
            };

            person = Read(person);
            father = Read(father);



            person.Father = father.Id;

            Update(person);
        }
        public void Update(Person person)
        {
            var db = new SQLDatabase
            {
                DatabaseName = DatabaseName
            };
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
            var db = new SQLDatabase();
            // börja med sqlkoden
            db.ExecuteSQL($"DELETE FROM FamilyTree WHERE Id = {person.Id}");
        }




        public Person DoesPersonExist(string name)
        {
            //börja med sqlkod
            var db = new SQLDatabase
            {
                DatabaseName = "Genealogy"
            };

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

        public Person Read(string name)
        {
            //börja med sqlkod
            var db = new SQLDatabase
            {
                DatabaseName = "Genealogy"
            };

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
            
            var db = new SQLDatabase
            {
                DatabaseName = DatabaseName
            };

            var row = db.GetDataTable("SELECT * FROM FamilyTree WHERE FirstName LIKE @FirstName", ("@FirstName", person.FirstName));

            Console.WriteLine(row.Rows.Count);

            if (row.Rows.Count == 0)
                return null;

            return GetPersonObject(row.Rows[0]);
        }

        private static Person GetPersonObject(DataRow row)
        {
            return new Person
            {
                Id = (int)row["Id"],
                FirstName = row["FirstName"].ToString(),
                LastName = row["LastName"].ToString(),
                BirthDate = row["BirthDate"].ToString(),
                DeathDate = row["DeathDate"].ToString(),
                Father = (int)row["Father"],
                Mother = (int)row["Mother"]
            };

        }

        public void GetFather(string name)
        {
            var db = new SQLDatabase
            {
                DatabaseName = "Genealogy"
            };
            //börja med sqlkod
        }
        /*
    public bool DoesPersonExist(int id)
    {
        //börja med sqlkod
    }


    public void GetFather (int id)
    {
        //börja med sqlkod
    }

    public void GetMother (string name)
    {
        //börja med sqlkod
    }

    public void GetMother(int id)
    {
            //börja med sqlkod
    }

    public List<Person> List(string filter = "firstName LIKE @input", string paramValue)
    {
            //börja med sqlkod
    }

    public Person Read (string name)
    {
        //börja med sqlkod

    */
    }
}
