using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MySql.Data.MySqlClient;

namespace Heimaverkefni4
{
    class Database
    {
        private static string server;
        private static string database;
        private static string uid;
        private static string password;

        static string constring = null;
        static string query = null;

        static MySqlConnection sqlconnection;
        static MySqlCommand newsqlquery;
        static MySqlDataReader sqlreader = null;

        public static void NewMovie(string nafn, string ar)
        {
            if (OpenConnection())
            {
                query = "INSERT INTO movies SET Nafn = '" + nafn + "',Utgafudagur = '" + ar + "'";
                newsqlquery = new MySqlCommand(query,sqlconnection);
                newsqlquery.ExecuteNonQuery();
                CloseConnection();
            }
        }

        public static bool Rent(string customerid, string movieid, string utdagur, string skiladagur, string athugasemd)
        {
            if (OpenConnection())
            {
                query = "INSERT INTO customer_has_movie set Customer_ID='" + customerid + "',Movie_ID='" + movieid + "',Utdagur='" + utdagur + "',Skiladagur='" + skiladagur + "',Athugasemd='" + athugasemd + "',Buinadskila='0'";
                newsqlquery = new MySqlCommand(query,sqlconnection);
                newsqlquery.ExecuteNonQuery();
                CloseConnection();
                return true;
            }
            return false;
        }

        public static bool RentReturn()
        {
            if (OpenConnection())
            {
                query = "";
                newsqlquery = new MySqlCommand(query, sqlconnection);
                newsqlquery.ExecuteNonQuery();
                CloseConnection();
                return true;
            }
            return false;
        }

        public static List<string> AllRented()
        {
            List<string> customer = new List<string>();
            string lina = null;
            if (OpenConnection())
            {
                query = "SELECT * FROM movies INNER JOIN customer_has_movie ON movies.ID = Movie_ID INNER JOIN customer ON customer.ID = Customer_ID WHERE Buinadskila = 0";
                newsqlquery = new MySqlCommand(query, sqlconnection);

                sqlreader = newsqlquery.ExecuteReader();
                while (sqlreader.Read())
                {
                    lina += (sqlreader.GetValue(1).ToString());
                    customer.Add(lina);
                    lina = null;
                }
                CloseConnection();
            }
            return customer;
        }

        public static List<string> AllNotRented()
        {
            List<string> customer = new List<string>();
            string lina = null;
            if (OpenConnection())
            {
                query = "SELECT * FROM movies LEFT JOIN customer_has_movie ON movies.ID = Movie_ID LEFT JOIN customer ON customer.ID = Customer_ID WHERE movies.Nafn NOT IN(SELECT movies.nafn FROM movies INNER JOIN customer_has_movie ON movies.ID = Movie_ID INNER JOIN customer ON customer.ID = Customer_ID WHERE Buinadskila = 0) GROUP BY movies.Nafn";
                newsqlquery = new MySqlCommand(query, sqlconnection);

                sqlreader = newsqlquery.ExecuteReader();
                while (sqlreader.Read())
                {
                    lina += (sqlreader.GetValue(0).ToString() + ";");
                    lina += (sqlreader.GetValue(1).ToString());
                    customer.Add(lina);
                    lina = null;
                }
                CloseConnection();
            }
            return customer;
        }

        public static List<string> AllMovies()
        {
            List<string> customer = new List<string>();
            string lina = null;
            if (OpenConnection())
            {
                query = "SELECT * FROM movies";
                newsqlquery = new MySqlCommand(query, sqlconnection);

                sqlreader = newsqlquery.ExecuteReader();
                while (sqlreader.Read())
                {
                    for (int i = 0; i < sqlreader.FieldCount; i++)
                    {
                        lina += (sqlreader.GetValue(i).ToString()) + ";";
                    }
                    customer.Add(lina);
                    lina = null;
                }
                CloseConnection();
            }
            return customer;
        }

        public static void DeleteCustomer(string kt)
        {
            if (OpenConnection())
            {
                query = "DELETE FROM customer WHERE Kennitala = '" + kt + "'";
                newsqlquery = new MySqlCommand(query, sqlconnection);
                newsqlquery.ExecuteNonQuery();
                CloseConnection();
            }
        }

        public static void UpdateCustomer(string id, string kt, string nafn, string simi)
        {
            if (OpenConnection())
            {
                query = "UPDATE customer SET Kennitala = '" + kt + "', Nafn = '" + nafn + "',Simi='" + simi + "' WHERE id = '" + id + "'";
                newsqlquery = new MySqlCommand(query, sqlconnection);
                newsqlquery.ExecuteNonQuery();
                CloseConnection();
            }
        }

        public static void NewCustomer(string kt, string nafn, string simi)
        {
            if (OpenConnection())
            {
                query = "INSERT INTO customer (Nafn,Kennitala,Simi) VALUES('" + nafn + "','" + kt + "','" + simi + "')";
                newsqlquery = new MySqlCommand(query,sqlconnection);
                newsqlquery.ExecuteNonQuery();
                CloseConnection();
            }
        }

        public static List<string> AllCustomers()
        {
            List<string> customer = new List<string>();
            string lina = null;
            if (OpenConnection())
            {
                query = "SELECT * FROM customer";
                newsqlquery = new MySqlCommand(query,sqlconnection);

                sqlreader = newsqlquery.ExecuteReader();
                while (sqlreader.Read())
                {
                    for (int i = 0; i < sqlreader.FieldCount; i++)
                    {
                        lina += (sqlreader.GetValue(i).ToString()) + ";";
                    }
                    customer.Add(lina);
                    lina = null;
                }
                CloseConnection();
            }
            return customer;
        }

        public static List<string> AllCustomersNF()
        {
            List<string> customer = new List<string>();
            string lina = null;
            if (OpenConnection())
            {
                query = "SELECT ID,Kennitala,Nafn FROM customer";
                newsqlquery = new MySqlCommand(query, sqlconnection);

                sqlreader = newsqlquery.ExecuteReader();
                while (sqlreader.Read())
                {
                    for (int i = 0; i < sqlreader.FieldCount; i++)
                    {
                        lina += (sqlreader.GetValue(i).ToString()) + ";";
                    }
                    customer.Add(lina);
                    lina = null;
                }
                CloseConnection();
            }
            return customer;
        }

        public static void Connect()
        {
            server = "localhost";
            database = "2105973019_forheima4";
            uid = "root";
            password = "";

            constring = "server= " + server + ";userid= " + uid + ";password= " + password + ";database= " + database;

            sqlconnection = new MySqlConnection(constring);
        }

         private static bool OpenConnection()
        {
            try
            {
                sqlconnection.Open();
                return true;
            }
            catch (MySqlException ex)
            {
                throw ex;
            }
        }

        private static bool CloseConnection()
        {
            try
            {
                sqlconnection.Close();
                return true;
            }
            catch (MySqlException ex)
            {
                throw ex;
            }
        }
    }
}
