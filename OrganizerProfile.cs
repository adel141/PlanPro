using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.StartPanel;

namespace PlanPro
{
    public partial class OrganizerProfile: Form
    {
        private const string connectionString = @"Data Source=localhost;Initial Catalog=proplan;Integrated Security=True;";
        SqlConnection conn = new SqlConnection(connectionString);
        string username;
        private string name, email, passwordt, confirmpass, role, phone;

        private void logoutBtn_Click(object sender, EventArgs e)
        {
            this.Hide();
            Login loginForm = new Login();
            loginForm.Show();
        }

        private void OrganizerProfile_Load(object sender, EventArgs e)
        {
            conn.Open();
            string query = $"SELECT * FROM usertb WHERE username='{username}'";
            SqlCommand cmd = new SqlCommand(query, conn);
            SqlDataReader reader = cmd.ExecuteReader();


            while (reader.Read())
            {
                nametextb.Text = reader["name"].ToString();
                emailtextb.Text = reader["email"].ToString();
                passwordtextb.Text = reader["password"].ToString();
                confirmpasswordtextb.Text = reader["password"].ToString();
                phonetextb.Text = reader["phone"].ToString();
                roletextb.Text = reader["role"].ToString();
            }
            conn.Close();
        }

        private void pictureBox1_Click(object sender, EventArgs e)
        {

        }

        public OrganizerProfile(string username)
        {
            InitializeComponent();
            this.username = username;
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

        private void siticoneButton2_Click(object sender, EventArgs e)
        {
            this.Hide();
            new Organizer(username).Show();
        }

        private void enableEditUserBtn_Click(object sender, EventArgs e)
        {
            nametextb.Enabled = true;
            emailtextb.Enabled = true;
            passwordtextb.Enabled = true;
            confirmpasswordtextb.Enabled = true;
            phonetextb.Enabled = true;
            updateUserBtn.Enabled = true;
            roletextb.Enabled = true;
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
    }
}
