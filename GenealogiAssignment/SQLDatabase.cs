using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq.Expressions;
using System.Text;

namespace GenealogiAssignment
{
    class SQLDatabase
    {
        //adress till databasen
        internal string ConnectionString { get; set; } = @"Data Source=.\SQLExpress;Integrated Security=true;database={0}";

        internal string DatabaseName { get; set; } = "Genealogy";

        internal CRUD CRUD
        {
            get => default;
            set
            {
            }
        }

        //Skapar en databas
        internal void CreateDatabase(string name, bool OpenNewDatabase = false)
        {
            ExecuteSQL("CREATE DATABASE " + name);
            if (OpenNewDatabase) DatabaseName = name;

        }

        //Skapa en tabell
        internal void CreateTable(string name, string fields)
        {
            ExecuteSQL($"CREATE TABLE {name} ({fields})");
        }

        //Ta bort databas
        internal void DropDatabase(string name)
        {
            DatabaseName = "Master";

            // Database is being used issue - https://stackoverflow.com/a/20569152/15032536
            ExecuteSQL(" alter database [" + name + "] set single_user with rollback immediate");

            ExecuteSQL("DROP DATABASE " + name);
        }

        //Kör SQL-kommando
        internal long ExecuteSQL(string sqlString, params (string, string)[] parameters)
        {
            long rowsAffected = 0;
            try
            {
                var connString = string.Format(ConnectionString, DatabaseName);//tar emot adressen till databasen och databasnamnet och skapar en connection
                using (var cnn = new SqlConnection(connString))
                {
                    cnn.Open();
                    using (var command = new SqlCommand(sqlString, cnn))
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

        //Sätter parametrar
        private void SetParameters((string, string)[] parameters, SqlCommand command)
        {
            foreach (var item in parameters)
            {
                command.Parameters.AddWithValue(item.Item1, item.Item2);
            }
        }

        //Kör sökningen i databasen.
        internal DataTable GetDataTable(string sqlString, params (string, string)[] parameters)
        {
            var dt = new DataTable();
            var connString = string.Format(ConnectionString, DatabaseName);
            try
            {
                using var cnn = new SqlConnection(connString);
                cnn.Open();
                using var command = new SqlCommand(sqlString, cnn);
                SetParameters(parameters, command);
                try
                {
                    using (var adapter = new SqlDataAdapter(command))
                    {
                        adapter.Fill(dt);
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
            }

            return dt;
        }

    }
}
