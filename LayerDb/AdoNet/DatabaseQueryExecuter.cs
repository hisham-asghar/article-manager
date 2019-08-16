using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Configuration;

namespace LayerDb.AdoNet
{
    public class DatabaseQueryExecuter<T> where T : new()
    {
        public static string Conn = null;
        private static string GetConnectionString()
        {
            Conn = Conn ?? ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString;
            return Conn;
        }

        public static T Get(string query, SqlConnection conn = null) 
        {
            try
            {
                conn = conn ?? new SqlConnection(GetConnectionString());
                conn.Open();

                var command = new SqlCommand(query, conn) { CommandTimeout = 0 };
                var reader = command.ExecuteReader();

                var obj = new ObjectMapper<T>().MapReaderToObject(reader);

                conn.Close();
                return obj;

            }
            catch (Exception)
            {
                conn?.Close();
                throw;
            }
        }
        public static List<T> GetAll(string query,SqlConnection conn = null)
        {

            try
            {
                conn = conn ?? new SqlConnection(GetConnectionString());
                conn.Open();

                var command = new SqlCommand(query, conn) { CommandTimeout = 0 };
                var reader = command.ExecuteReader();

                var obj = new ObjectMapper<T>().MapReaderToObjectList(reader);

                conn.Close();
                return obj;

            }
            catch (Exception)
            {
                conn?.Close();
                throw;
                //return default(List<T>);
            }
        }


        public static int Execute(string query, SqlConnection conn = null)
        {
            try
            {
                conn = conn ?? new SqlConnection(GetConnectionString());
                conn.Open();

                var command = new SqlCommand(query, conn) { CommandTimeout = 0 };
                var reader = command.ExecuteNonQuery();
                

                conn.Close();
                return reader;

            }
            catch (Exception)
            {
                conn?.Close();
                throw;
            }
        }


    }

    
}
