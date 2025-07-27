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
    public partial class Feedback: Form
    {
        string username;
        public Feedback(string username)
        {
            this.username = username;
            InitializeComponent();

        }
        private const string connectionString = @"Data Source=localhost;Initial Catalog=proplan;Integrated Security=True;";
        SqlConnection conn = new SqlConnection(connectionString);
        private string eventtitle,feedback, rating;


        private bool isValidData()
        {
            if (string.IsNullOrWhiteSpace(eventtitle) || string.IsNullOrWhiteSpace(feedback) || string.IsNullOrWhiteSpace(rating))
            {
                MessageBox.Show("Please fill in all fields.");
                return false;
            }
            return true;

        }

        private void siticoneButton2_Click(object sender, EventArgs e)
        {

            comboBox2.SelectedIndex = -1; // Clear the selection
            richTextBox1.Clear(); // Clear the feedback text
            siticoneRating1.Rating = 0F; // Reset the rating
        }

        private void siticoneButton3_Click(object sender, EventArgs e)
        {
            this.Hide();
            UserDashboard userDashboard = new UserDashboard(username);
            userDashboard.Show();
        }

        private void logoutBtn_Click(object sender, EventArgs e)
        {
            this.Hide();
            new Login().Show();
        }

        private void Feedback_Load(object sender, EventArgs e)
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
            eventtitle = comboBox2.Text;
            feedback = richTextBox1.Text;
            rating = siticoneRating1.Rating.ToString();
            if (isValidData())
            {
                    conn.Open();
                    string query = "INSERT INTO Feedbacktb ([user], EventTitle, Feedback, Rating) VALUES (@Username, @EventTitle, @Feedback, @Rating)";
                    using (SqlCommand cmd = new SqlCommand(query, conn))
                    {
                        cmd.Parameters.AddWithValue("@Username", username);
                        cmd.Parameters.AddWithValue("@EventTitle", eventtitle);
                        cmd.Parameters.AddWithValue("@Feedback", feedback);
                        cmd.Parameters.AddWithValue("@Rating", rating);
                        int rowsAffected = cmd.ExecuteNonQuery();
                        if (rowsAffected > 0)
                        {
                            MessageBox.Show("Feedback submitted successfully.");
                            comboBox2.SelectedIndex = -1; // Clear the selection
                        richTextBox1.Clear(); // Clear the feedback text
                        siticoneRating1.Rating = 0F; // Reset the rating

                    }
                        else
                        {
                            MessageBox.Show("Error submitting feedback. Please try again.");
                        }
                    }
                
            }
        }
    }
}
