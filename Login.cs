using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.SqlClient;
using System.Windows.Forms;

namespace PlanPro
{
    public partial class Login : Form
    {
        public Login()
        {
            InitializeComponent();
        }
        private void clear()
        {
            usernameText.Clear();
            passwordText.Clear();
        }
        private void connect()
        {

            string username = usernameText.Text;
            string password = passwordText.Text;
            string connectionString = @"Data Source=localhost;Initial Catalog=proplan;Integrated Security=True;";
            SqlConnection conn = new SqlConnection(connectionString);
            
            conn.Open();
            string query = $"SELECT COUNT(*) FROM Usertb WHERE Username = '{username}' AND Password = '{password}'";
            SqlCommand cmd = new SqlCommand(query, conn);
            int count = (int)cmd.ExecuteScalar();
            if (count > 0)
            {
                string roleQuery = $"SELECT Role FROM Usertb WHERE Username = '{username}'";
                SqlCommand roleCmd = new SqlCommand(roleQuery, conn);
                SqlDataReader reader = roleCmd.ExecuteReader();
                if (reader.Read())
                {
                    string role = reader["Role"].ToString();
                    if (role == "Admin")
                    {
                        new AdminDashboard(username).Show();
                        this.Hide();

                    }
                    else if (role == "Attendee")
                    {

                        new UserDashboard(username).Show();
                        this.Hide();
                    }
                    else if (role == "Organizer")
                    {
                        new Organizer(username).Show();
                        this.Hide();
                    }
                    else if (role == "Venue Manager")
                    {
                        new VenueDashboard(username).Show();
                        this.Hide();
                    }
                }
            }
            else
            {
                MessageBox.Show("Invalid username or password.");
            }
            
        }
        private bool IsValidData()
        {
            if (string.IsNullOrEmpty(usernameText.Text))
            {
                MessageBox.Show("Please enter your username.");
                return false;
            }
            if (string.IsNullOrEmpty(passwordText.Text))
            {
                MessageBox.Show("Please enter your password.");
                return false;
            }
            return true;
        }



        private void siticoneButtonAdvanced2_Click(object sender, EventArgs e)
        {
            clear();
        }


        private void label1_Click(object sender, EventArgs e)
        {
            this.Hide();
            new Registration().Show(); // Assuming you have a Registration form to navigate to
        }

        private void siticoneButtonAdvanced1_Click(object sender, EventArgs e)
        {
            if (IsValidData())
            {
                connect();
            }
        }

        private void label2_Click(object sender, EventArgs e)
        {
            this.Hide();
            new ForgetPassword().Show();
        }
    }
}
