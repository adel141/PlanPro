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
    public partial class ManageAttendeeProfile: Form

    {
        private const string connectionString = @"Data Source=localhost;Initial Catalog=proplan;Integrated Security=True;";
        SqlConnection conn = new SqlConnection(connectionString);
        string username;
        private string name, email, passwordt, confirmpass, role, phone;
        public ManageAttendeeProfile(string username)
        {
            InitializeComponent();
            this.username = username;
        }

        private void siticoneButton2_Click(object sender, EventArgs e)
        {
            this.Hide();
            UserDashboard userDashboard = new UserDashboard(username);
            userDashboard.Show();
        }

        private void logoutBtn_Click(object sender, EventArgs e)
        {
            this.Hide();
            Login loginForm = new Login();
            loginForm.Show();
        }

        private void enableEditUserBtn_Click_1(object sender, EventArgs e)
        {

            nametextb.Enabled = true;
            emailtextb.Enabled = true;
            passwordtextb.Enabled = true;
            confirmpasswordtextb.Enabled = true;
            phonetextb.Enabled = true;
            updateUserBtn.Enabled = true;
            roletextb.Enabled = true;

        }

        private bool isValidData()
        {
            if (string.IsNullOrEmpty(name) || string.IsNullOrEmpty(email) || string.IsNullOrEmpty(passwordt) || string.IsNullOrEmpty(confirmpass) || string.IsNullOrEmpty(phone) || string.IsNullOrEmpty(role))
            {
                MessageBox.Show("Please fill all fields.");
                return false;
            }
            if (passwordt != confirmpass)
            {
                MessageBox.Show("Passwords do not match.");
                return false;
            }
            return true;
        }


        private void updateUserBtn_Click(object sender, EventArgs e)
        {

            name = nametextb.Text;
            phone = phonetextb.Text;
            email = emailtextb.Text;
            passwordt = passwordtextb.Text;
            confirmpass = confirmpasswordtextb.Text;
            role = roletextb.Text;
            if (!isValidData())
            {
                return;
            }

            {

                conn.Open();
                string updateQuery = $"Update usertb set name='{name}', email='{email}', password='{passwordt}', phone='{phone}', role='{role}' where username='{username}'";
                SqlCommand cmd = new SqlCommand(updateQuery, conn);
                cmd.ExecuteNonQuery();
                MessageBox.Show("Updated User Info.");
                conn.Close();
            }
            this.Hide();
            UserDashboard userDashboard = new UserDashboard(username);
            userDashboard.Show();
        }

        private void ManageAttendeeProfile_Load(object sender, EventArgs e)
        {
            nametextb.Enabled = false;
            emailtextb.Enabled = false;
            passwordtextb.Enabled = false;
            confirmpasswordtextb.Enabled = false;
            phonetextb.Enabled = false;
            updateUserBtn.Enabled = false;
            roletextb.Enabled = false;
            LoadUserData();

        }
        private void LoadUserData()
        {

            conn.Open();
            string searchQuery = $"Select * from usertb where username ='{username}'";
            SqlCommand cmd = new SqlCommand(searchQuery, conn);
            SqlDataReader reader = cmd.ExecuteReader();
            while (reader.Read())
            {
                nametextb.Text = reader.GetString(0);
                emailtextb.Text = reader.GetString(2);
                passwordtextb.Text = reader.GetString(3);
                confirmpasswordtextb.Text = reader.GetString(3);
                phonetextb.Text = reader.GetInt32(4).ToString();
                roletextb.Text = reader.GetString(5);

            }
            reader.Close();
            conn.Close();
        }
    }
}
