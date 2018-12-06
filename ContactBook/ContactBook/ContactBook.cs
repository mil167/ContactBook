using ContactBook.contactbookClasses;
using Npgsql;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ContactBook
{
    public partial class ContactBook : Form
    {
        public ContactBook()
        {
            InitializeComponent();
        }

        ContactClass c = new ContactClass();



        private void ContactBook_Load(object sender, EventArgs e)
        {
            DataTable dt = c.Select();
            contactList.DataSource = dt;
        }


        private void addButton_Click(object sender, EventArgs e)
        {
            //Get the value of user input
            c.LastName = lastNameForm.Text;
            c.FirstName = firstNameForm.Text;
            c.PhoneNumber = phoneNumberForm.Text;
            c.Address = addressForm.Text;

            //Insert data into the database
            bool insertStatus = c.Insert(c);
            if (insertStatus)
            {
                MessageBox.Show("Contact successfully added!");
                Clear();
            }
            else
            {
                MessageBox.Show("Error adding contact. Please try again.");
            }

            //Load updated database in the GridView
            DataTable dt = c.Select();
            contactList.DataSource = dt;
        }

        private void exitButton_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        public void Clear()
        {
            lastNameForm.Text = "";
            firstNameForm.Text = "";
            phoneNumberForm.Text = "";
            addressForm.Text = "";
        }

        private void updateButton_Click(object sender, EventArgs e)
        {
            //Get the data from the text boxes
            c.ContactID = int.Parse(contactIDForm.Text);
            c.LastName = lastNameForm.Text;
            c.FirstName = firstNameForm.Text;
            c.PhoneNumber = phoneNumberForm.Text;
            c.Address = addressForm.Text;

            //Update data in the database
            bool updateSuccess = c.Update(c);

            if(updateSuccess)
            {
                MessageBox.Show("Contact successfully updated!");
                //Load updated database in the GridView
                DataTable dt = c.Select();
                contactList.DataSource = dt;

                Clear();
            }
            else
            {
                MessageBox.Show("Error updating contact! Please try again.");
            }
        }

        private void contactList_RowHeaderMouseClick(object sender, DataGridViewCellMouseEventArgs e)
        {
            //Get the data from a specific row that the user clicks on
            int rowIndex = e.RowIndex;
            contactIDForm.Text = contactList.Rows[rowIndex].Cells[0].Value.ToString();
            lastNameForm.Text = contactList.Rows[rowIndex].Cells[1].Value.ToString();
            firstNameForm.Text = contactList.Rows[rowIndex].Cells[2].Value.ToString();
            phoneNumberForm.Text = contactList.Rows[rowIndex].Cells[3].Value.ToString();
            addressForm.Text = contactList.Rows[rowIndex].Cells[4].Value.ToString();
        }

        private void clearButton_Click(object sender, EventArgs e)
        {
            Clear();
        }

        private void deleteButton_Click(object sender, EventArgs e)
        {
            //Get the data from the text boxes
            c.ContactID = int.Parse(contactIDForm.Text);
            bool deleteSuccess = c.Delete(c);
            if(deleteSuccess)
            {
                MessageBox.Show("Contact deleted successfully!");
                //Load updated database in the GridView
                DataTable dt = c.Select();
                contactList.DataSource = dt;
            }
            else
            {
                MessageBox.Show("Error deleting contact! Please try again.");
            }
        }

        static string myConnectionString = ConfigurationManager.ConnectionStrings["ContactBook.Properties.Settings.contactbookConnectionString"].ConnectionString;

        private void searchForm_TextChanged(object sender, EventArgs e)
        {
            //Get the value from the search text box
            string keyword = searchForm.Text;

            NpgsqlConnection conn = new NpgsqlConnection(myConnectionString);
            try
            {
                conn.Open();
                // Select data from the database
                string query = "SELECT contactID, lastName, firstName, phoneNumber, address " +
                    "FROM contacts WHERE lastName like '%" + keyword + "%' OR firstName like '%" +
                    keyword + "%'";
                //Create command using query string and connection
                NpgsqlCommand cmd = new NpgsqlCommand(query, conn);
                // Fill DataTable with result from the adapter
                NpgsqlDataAdapter adapter = new NpgsqlDataAdapter(cmd);
                DataTable dt = new DataTable();
                adapter.Fill(dt);
                contactList.DataSource = dt;
            }
            catch(Exception ex)
            {

            }
            finally
            {
                conn.Close();
            }
        }
    }
}
