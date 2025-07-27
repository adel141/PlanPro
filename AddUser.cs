using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Microsoft.Data.SqlClient;
using Microsoft.IdentityModel.Tokens;

namespace PlanPro
{
    public partial class AddUser: Form
    {
        public AddUser()
        {
            InitializeComponent();
        }


        private void connect()
        {
            string name = nameText.Text;
            string username = usernameText.Text;
            string email = emailText.Text;
            string pass = passwordText.Text;
            long phone = Convert.ToInt32(phoneText.Text);
            string role = roleText.Text;

            string connectionString = @"Data Source=localhost;Initial Catalog=proplan;";

            SqlConnection conn = new SqlConnection(connectionString);

            conn.Open();
            string checkQuery = $"SELECT COUNT(*) FROM Usertb WHERE Username = '{username}'";
            SqlCommand checkCmd = new SqlCommand(checkQuery, conn);
            int count = (int)checkCmd.ExecuteScalar();
            if (count > 0)
            {
                MessageBox.Show("Username already exists. Please choose a different one.");
                conn.Close();
                return;
            }
            else
            {
                string query = $"INSERT INTO Usertb (Name, Username, Email, Password, Phone, Role) VALUES ('{name}', '{username}', '{email}', '{pass}', {phone}, '{role}')";
                SqlCommand cmd = new SqlCommand(query, conn);
                cmd.ExecuteNonQuery();
            }
            conn.Close();
        }

        private bool IsValidData()
        {
            if (nameText.Text.IsNullOrEmpty())
            {
                MessageBox.Show("Please enter your name.");
                return false;
            }
            else if (usernameText.Text.IsNullOrEmpty())
            {
                MessageBox.Show("Please enter your username.");
                return false;
            }
            else if (emailText.Text.IsNullOrEmpty())
            {
                MessageBox.Show("Please enter your email.");
                return false;
            }
            else if (passwordText.Text.IsNullOrEmpty())
            {
                MessageBox.Show("Please enter your password.");
                return false;
            }
            else if (passwordText.Text != confirmPassword.Text)
            {
                MessageBox.Show("Passwords do not match.");
                return false;
            }
            else if (phoneText.Text.IsNullOrEmpty())
            {
                MessageBox.Show("Please enter your phone number.");
                return false;
            }
            else if (!emailText.Text.Contains("@"))
            {
                MessageBox.Show("Please enter a valid email address.");
                return false;
            }
            else if (phoneText.Text.Length < 11)
            {
                MessageBox.Show("Please enter a valid phone number.");
                return false;
            }
            else if (roleText.Text.IsNullOrEmpty())
            {
                MessageBox.Show("Please select a role.");
                return false;
            }
            else if (roleText.Text != "Organizer" && roleText.Text != "Attendee" && roleText.Text != "Venue Manager")
            {

                MessageBox.Show("Please select a valid role ('Organizer' or 'Attendee' or 'Venue Manager')." + roleText.Text);
                return false;
            }
            else
            {

                return true;
            }
        }
        private void clear()
        {
            nameText.Clear();
            usernameText.Clear();
            emailText.Clear();
            passwordText.Clear();
            confirmPassword.Clear();
            phoneText.Clear();
            roleText.SelectedIndex = -1; // Reset the role selection
        }

        private void siticoneButtonAdvanced1_Click(object sender, EventArgs e)
        {

            if (IsValidData())
            {
                try
                {
                    connect();
                    clear();
                    MessageBox.Show("Registration successful! You can now log in with your credentials.");
                    this.Hide();
                    new Login().Show();
                }
                catch (Exception ex)
                {
                    MessageBox.Show("An error occurred while registering: " + ex.Message);
                }
            }
        }

        private void siticoneButtonAdvanced2_Click(object sender, EventArgs e)
        {

            clear();
        }

        private void logoutBtn_Click(object sender, EventArgs e)
        {
            this.Hide();
            new AdminDashboard("admin").Show();
        }
    }
}
