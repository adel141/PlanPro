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
using Microsoft.IdentityModel.Tokens;

namespace PlanPro
{
    public partial class AdminDashboard: Form
    {
        private string username,name, email, passwordt, confirmpass, role, phone;
        private const string  connectionString = @"Data Source=localhost;Initial Catalog=proplan;Integrated Security=True;";

        private SqlConnection conn = new SqlConnection(connectionString);

        public AdminDashboard(string username)
        {
            InitializeComponent();
            this.username=username;
            adminprofile();
        }
        private void adminprofile()
        {
            conn.Open();
            string getQuery = $"Select * from usertb where username='{this.username}' ";
            SqlCommand cmd = new SqlCommand(getQuery, conn);
            SqlDataReader reader = cmd.ExecuteReader();
            while (reader.Read())
            {
                name = reader.GetString(0);
                username = reader.GetString(1);
                email = reader.GetString(2);
                passwordt = reader.GetString(3);
                confirmpass = reader.GetString(3);
                phone = reader.GetInt32(4).ToString();
               

            }
            reader.Close();
            nameText.Text = name;
            usernameText.Text = username;
            emailText.Text = email;
            password.Text = passwordt;
            confirmpasstext.Text = passwordt;
            phoneText.Text = phone;
            conn.Close();
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

        private void updateAdminInfoBtn_Click(object sender, EventArgs e)
        {
            role = "admin";
            if (IsValidData())
            {
                updateAdminProfile();
            }
        }

        private void updateUserBtn_Click(object sender, EventArgs e)

        {

            role = roletextb.Text;
            if (IsValidData())
            {
                updateUserProfile();
            }
            userDataGrid.Refresh();

            nametextb.Enabled = false;
            emailtextb.Enabled = false;
            passwordtextb.Enabled = false;
            confirmpasswordtextb.Enabled = false;
            phonetextb.Enabled = false;
            updateUserBtn.Enabled = false;
            roletextb.Enabled = false;
        }

        private void userDeleteBtn_Click(object sender, EventArgs e)
        {
            conn.Open();
            string deleteQuery = $"Delete from usertb where username='{searchUsertext.Text}'";
            SqlCommand cmd = new SqlCommand(deleteQuery, conn);
            cmd.ExecuteNonQuery();

            userDataGrid.Refresh();
            MessageBox.Show("User Deleted Successfully.");
            conn.Close();
            nametextb.Clear();
            emailtextb.Clear();
            passwordtextb.Clear();
            confirmpasswordtextb.Clear();
            phonetextb.Clear();

            roletextb.SelectedIndex = -1;
            userDeleteBtn.Enabled = false;
            enableEditUserBtn.Enabled = false;
        }

        private void addUserBtn_Click(object sender, EventArgs e)
        {
            new AddUser().Show();
            this.Hide();
        }

        private void siticoneButtonAdvanced4_Click(object sender, EventArgs e)
        {
            conn.Open();
            string getQuery = "Select * from venueinfo";
            SqlCommand cmd = new SqlCommand(getQuery, conn);
            SqlDataAdapter adapter = new SqlDataAdapter(cmd);
            DataSet ds = new DataSet();
            adapter.Fill(ds);
            DataTable dt = ds.Tables[0];
            dataGridView1.AutoGenerateColumns = true;
            dataGridView1.AutoResizeRows(DataGridViewAutoSizeRowsMode.AllCells);
            dataGridView1.DataSource = dt;
            conn.Close();
        }

        private void siticoneButtonAdvanced3_Click(object sender, EventArgs e)
        {

        }



        private void siticonePanel1_Paint(object sender, PaintEventArgs e)
        {

        }

        private bool IsValidData()
        {
            if (name.IsNullOrEmpty())
            {
                MessageBox.Show("Please enter your name.");
                return false;
            }
            else if (username.IsNullOrEmpty())
            {
                MessageBox.Show("Please enter your username.");
                return false;
            }
            else if (email.IsNullOrEmpty())
            {
                MessageBox.Show("Please enter your email.");
                return false;
            }
            else if (passwordt.IsNullOrEmpty())
            {
                MessageBox.Show("Please enter your password.");
                return false;
            }
            else if (passwordt != confirmpass)
            {
                MessageBox.Show("Passwords do not match.");
                return false;
            }
            else if (phone.IsNullOrEmpty())
            {
                MessageBox.Show("Please enter your phone number.");
                return false;
            }
            else if (!email.Contains("@"))
            {
                MessageBox.Show("Please enter a valid email address.");
                return false;
            }
            else if(phone.Length !=10)
            {
                MessageBox.Show("Please enter a valid phone number.");
                return false;
            }
            else if (role.IsNullOrEmpty())
            {
                MessageBox.Show("Please select a role.");
                return false;
            }

            else
            {

                return true;
            }
        }

        private void updateUserProfile()
        {
            name = nametextb.Text;
            phone = phonetextb.Text;
            email = emailtextb.Text;
            passwordt = passwordtextb.Text;
            confirmpass = confirmpasswordtextb.Text;
            role = roletextb.Text;
            
            conn.Open();
            string updateQuery = $"Update usertb set name='{name}', email='{email}', password='{passwordt}', phone='{phone}', role='{role}' where username='{username}'";
            SqlCommand cmd = new SqlCommand(updateQuery, conn);
            cmd.ExecuteNonQuery();
            MessageBox.Show("Updated User Info.");
            conn.Close();
        }
        
        private void updateAdminProfile()
        {
            name = nameText.Text;
            username = usernameText.Text;
            email = emailText.Text;
            phone = phoneText.Text;
            passwordt = password.Text;
            role = "admin";
            confirmpass = confirmpasstext.Text;



            conn.Open();
            string updateQuery = $"Update usertb set name='{name}', username ='{username}', email='{email}',password='{passwordt}', phone='{phone}' where username='{username}'";
            SqlCommand cmd = new SqlCommand(updateQuery, conn);
            cmd.ExecuteNonQuery();
            MessageBox.Show("Updated Admin Info.");
            conn.Close();
        }

        private void logoutBtn_Click(object sender, EventArgs e)
        {
            this.Hide();
            new Login().Show();
        }

        private void viewAllUserbtn_Click(object sender, EventArgs e)
        {
            conn.Open();
            string getQuery = "Select * from usertb where username<>'admin'";
            SqlCommand cmd = new SqlCommand(getQuery, conn);
            SqlDataAdapter adapter = new SqlDataAdapter(cmd);
            DataSet ds = new DataSet();
            adapter.Fill(ds);
            DataTable dt = ds.Tables[0];
            userDataGrid.AutoGenerateColumns = true;
            userDataGrid.AutoResizeRows(DataGridViewAutoSizeRowsMode.AllCells);
            userDataGrid.DataSource = dt;
            conn.Close();

        }

        private void SearchUserBtn_Click(object sender, EventArgs e)
        {
            if (searchUsertext.Text.IsNullOrEmpty() || searchUsertext.Text.Equals("admin"))
            {
                MessageBox.Show("Please enter a vaild username to search.");

            }
            else
            {
                conn.Open();
                string searchQuery = $"Select * from usertb where username ='{searchUsertext.Text}'";
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
                userDeleteBtn.Enabled = true;
                enableEditUserBtn.Enabled = true;



            }
        }




    }
}
