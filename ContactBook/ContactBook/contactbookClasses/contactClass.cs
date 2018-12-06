using Npgsql;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ContactBook.contactbookClasses
{
    class ContactClass
    {
        //Getter and Setter functions
        public int ContactID { get; set; }

        public string FirstName { get; set; }

        public string LastName { get; set; }

        public string PhoneNumber { get; set; }

        public string Address { get; set; }

        static string myConnectionString = ConfigurationManager.ConnectionStrings["ContactBook.Properties.Settings.contactbookConnectionString"].ConnectionString;
        
        //Select data from database
        public DataTable Select()
        {
            DataTable dt = new DataTable();
            // Connect to Postgres
            NpgsqlConnection conn = new NpgsqlConnection(myConnectionString);
            try
            {
                conn.Open();
                // Select data from the database
                string query = "SELECT contactID, lastName, firstName, phoneNumber, address FROM contacts";
                //Create command using query string and connection
                NpgsqlCommand cmd = new NpgsqlCommand(query, conn);
                // Fill DataTable with result from the adapter
                NpgsqlDataAdapter adapter = new NpgsqlDataAdapter(cmd);
                adapter.Fill(dt);
            }
            catch(Exception e)
            {

            }
            finally
            {
                conn.Close();
            }
            return dt;
        }

        //Insert data into database
        public bool Insert(ContactClass c)
        {
            // Connect to Postgres
            NpgsqlConnection conn = new NpgsqlConnection(myConnectionString);
            bool querySuccess = false;
            try
            {
                //Insert new contact into the database
                string query = "INSERT INTO contacts (lastName, firstName, phoneNumber, address) VALUES(@lastName, @firstName, @phoneNumber, @address)";
                //Create command using query string and connection
                NpgsqlCommand cmd = new NpgsqlCommand(query, conn);

                cmd.Parameters.AddWithValue("@lastName", c.LastName);
                cmd.Parameters.AddWithValue("@firstName", c.FirstName);
                cmd.Parameters.AddWithValue("@phoneNumber", c.PhoneNumber);
                cmd.Parameters.AddWithValue("@address", c.Address);

                conn.Open();
                int rows = cmd.ExecuteNonQuery();

                //If the query runs successfully, the value of rows will be greater than 0, else it will be 0
                if(rows > 0)
                {
                    querySuccess = true;
                }
                else
                {
                    querySuccess = false;
                }
                
            }
            catch(Exception e)
            {

            }
            finally
            {
                conn.Close();
            }
            return querySuccess;
        }

        //Update data in database
        public bool Update(ContactClass c)
        {
            // Connect to Postgres
            NpgsqlConnection conn = new NpgsqlConnection(myConnectionString);
            bool querySuccess = false;
            try
            {
                //Update contact inside the database
                string query = "UPDATE contacts SET lastName=@lastName, " +
                    "firstName=@firstName, phoneNumber=@phoneNumber, address=@address WHERE " +
                    "contactID=@contactID";
                //Create command using query string and connection
                NpgsqlCommand cmd = new NpgsqlCommand(query, conn);

                cmd.Parameters.AddWithValue("@lastName", c.LastName);
                cmd.Parameters.AddWithValue("@firstName", c.FirstName);
                cmd.Parameters.AddWithValue("@phoneNumber", c.PhoneNumber);
                cmd.Parameters.AddWithValue("@address", c.Address);
                cmd.Parameters.AddWithValue("@contactID", c.ContactID);

                conn.Open();
                int rows = cmd.ExecuteNonQuery();

                //If the query runs successfully, the value of rows will be greater than 0, else it will be 0
                if (rows > 0)
                {
                    querySuccess = true;
                }
                else
                {
                    querySuccess = false;
                }

            }
            catch (Exception e)
            {

            }
            finally
            {
                conn.Close();
            }
            return querySuccess;
        }
        //Delete data in database
        public bool Delete(ContactClass c)
        {
            // Connect to Postgres
            NpgsqlConnection conn = new NpgsqlConnection(myConnectionString);
            bool querySuccess = false;
            try
            {
                //Update contact inside the database
                string query = "DELETE FROM contacts WHERE contactID=@contactID";
                //Create command using query string and connection
                NpgsqlCommand cmd = new NpgsqlCommand(query, conn);

                cmd.Parameters.AddWithValue("@contactID", c.ContactID);

                conn.Open();
                int rows = cmd.ExecuteNonQuery();

                //If the query runs successfully, the value of rows will be greater than 0, else it will be 0
                if (rows > 0)
                {
                    querySuccess = true;
                }
                else
                {
                    querySuccess = false;
                }

            }
            catch (Exception e)
            {

            }
            finally
            {
                conn.Close();
            }
            return querySuccess;
        }
    }
}
