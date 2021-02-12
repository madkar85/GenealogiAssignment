using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Text;

namespace GenealogiAssignment
{
    class SQLDatabase
    {
        //adress till databasen
        internal string ConnectionString { get; set; } = @"Data Source=.\SQLExpress;Integrated Security=true;database={0}";

        internal string DatabaseName { get; set; } = "Genealogy";

        internal void CreateDatabase(string name, bool OpenNewDatabase = false)
        {
            ExecuteSQL("CREATE DATABASE " + name);
            if (OpenNewDatabase) DatabaseName = name;

        }
        internal void CreateTable(string name, string fields)
        {
            ExecuteSQL($"CREATE TABLE {name} ({fields})");
        }

        internal void DropDatabase(string name)
        {
            DatabaseName = "Master";

            // Database is being used issue - https://stackoverflow.com/a/20569152/15032536
            ExecuteSQL(" alter database [" + name + "] set single_user with rollback immediate");

            ExecuteSQL("DROP DATABASE " + name);
        }

        internal long ExecuteSQL(string sqlString, params (string, string)[] parameters)
        {
            long rowsAffected = 0;
            try
            {
                var connString = string.Format(ConnectionString, DatabaseName);//tar emot adressen till databasen och databasnamnet och skapar en connection
                using (var cnn = new SqlConnection(connString))
                {
                    cnn.Open();
                    using(var command = new SqlCommand(sqlString, cnn))
                    {
                        SetParameters(parameters, command);
                        rowsAffected = command.ExecuteNonQuery();
                    }
                }
            }
            catch (System.Exception ex)
            {
                System.Console.WriteLine(ex);
            }

            return rowsAffected;
        }

        //
        private void SetParameters((string, string)[] parameters, SqlCommand command)
        {
            foreach (var item in parameters)
            {
                command.Parameters.AddWithValue(item.Item1, item.Item2);
            }
        }



    }
}
