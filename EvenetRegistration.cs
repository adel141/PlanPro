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

namespace PlanPro
{
    public partial class EvenetRegistration: Form
    {

        string username;

        private const string connectionString = @"Data Source=localhost;Initial Catalog=proplan;Integrated Security=True;";
        SqlConnection conn = new SqlConnection(connectionString);

        public EvenetRegistration(string username)
        {
            InitializeComponent();
            this.username = username;
        }

        private void EvenetRegistration_Load(object sender, EventArgs e)
        {
            conn.Open();
            string query = "SELECT title FROM eventtb";

            SqlCommand cmd = new SqlCommand(query, conn);
            SqlDataReader reader = cmd.ExecuteReader();

            while (reader.Read())
            {
                comboBox2.Items.Add(reader["title"].ToString());
            }

            reader.Close();
            conn.Close();
        
        }

        private void siticoneButton1_Click(object sender, EventArgs e)
        {
            string eventTitle = comboBox2.SelectedItem.ToString();
            string name = textBox1.Text;
            string email = textBox2.Text;
            string contact = textBox3.Text;
            string gender = "";
            if(radioButton1.Checked)
            {
                gender = radioButton1.Text;
            }
            else if(radioButton2.Checked)
            {
                gender = radioButton2.Text;
            }
            else
            {
                gender = radioButton3.Text;
            }
            string ticketQuantity = numericUpDown1.Text;
            string paymentMethod = comboBox1.Text;

            string selectedEvent = comboBox2.SelectedItem.ToString();
            string query = $"insert into EventRegistration (event, name, email, contactnumber, gender, ticketquantity,paymentmethod,username) " +
                           $"values('{eventTitle}','{name}','{email}','{contact}','{gender}','{ticketQuantity}','{paymentMethod}','{username}')";
                          

            conn.Open();

            SqlCommand cmd = new SqlCommand(query, conn);
         
            cmd.ExecuteNonQuery();
            MessageBox.Show("Registration Successful");
 
            conn.Close();
            
        }

        private void siticoneButton2_Click(object sender, EventArgs e)
        {
            this.Hide();
            UserDashboard userDashboard = new UserDashboard("username"); // Replace "username" with actual username variable if needed
            userDashboard.Show();
        }

        private void logoutBtn_Click(object sender, EventArgs e)
        {
            this.Hide();
            Login loginForm = new Login();
            loginForm.Show();
        }
    }
}
