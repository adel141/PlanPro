using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Data.SqlClient;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace PlanPro
{
    public partial class ForgetPassword: Form
    {

        private string username;
        private string password;
        private const string  connectionString = @"Data Source=localhost;Initial Catalog=proplan;Integrated Security=True;";
        SqlConnection conn = new SqlConnection(connectionString);
        public ForgetPassword()
        {
            InitializeComponent();
        }
        private bool isvaliddata()
        {
            if (string.IsNullOrEmpty(usernameText.Text))
            {
                MessageBox.Show("Please enter your username.");
                return false;
            }
            if (string.IsNullOrEmpty(emailText.Text))
            {
                MessageBox.Show("Please enter your email.");
                return false;
            }

            return true;

        }
        private void siticoneButtonAdvanced2_Click(object sender, EventArgs e)
        {
            string otp;
            if(isvaliddata())
            {
                Random random = new Random();
                otp = random.Next(100000, 999999).ToString();
                conn.Open();
                string query = $"SELECT COUNT(*) FROM Usertb WHERE Username = '{usernameText.Text}' AND Email = '{emailText.Text}'";
                SqlCommand cmd = new SqlCommand(query, conn);
                int count = (int)cmd.ExecuteScalar();

                conn.Close();
                if (count > 0)
                {
                    MessageBox.Show("Your OTP is: " + otp + "\nPlease check your email for the OTP.");
                }
                else
                {
                    MessageBox.Show("Username or email does not match.");
                }
            }
        }
        private bool isvalidotp()
        {

            if (passwordText.Text != confirmPassword.Text)
            {
                MessageBox.Show("Passwords do not match.");
            }
            else if (string.IsNullOrEmpty(passwordText.Text) || string.IsNullOrEmpty(confirmPassword.Text))
            {
                MessageBox.Show("Please enter your new password.");
            }
            else if (string.IsNullOrEmpty(otpText.Text))
            {
                MessageBox.Show("Please enter the OTP.");
                return false;
            }
            return true;
        }
        private void siticoneButtonAdvanced1_Click(object sender, EventArgs e)
        {
            if (isvalidotp())
            {
                conn.Open();
                string updateQuery = $"UPDATE Usertb SET Password = '{passwordText.Text}' WHERE Username = '{usernameText.Text}' AND Email = '{emailText.Text}'";
                SqlCommand cmd = new SqlCommand(updateQuery, conn);
                cmd.ExecuteNonQuery();
                MessageBox.Show("Password updated successfully.");
                conn.Close();
                this.Hide();
                new Login().Show();
            }
        }
    }
}
