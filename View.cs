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
    public partial class View: Form
    {
        string username;
        private const string connectionString = @"Data Source=localhost;Initial Catalog=proplan;Integrated Security=True;";
        public View(string username)
        {
            InitializeComponent();

            LoadData();
            this.username = username;
        }

        private void LoadData()
        {
            try
            {
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();
                    string query = "SELECT venueID, venueName, location, capacity, status FROM venueInfo";
                    SqlDataAdapter adapter = new SqlDataAdapter(query, connection);
                    DataTable dt = new DataTable();
                    adapter.Fill(dt);

                    dataGridView1.DataSource = dt;
                    dataGridView1.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error loading data: " + ex.Message);
            }
        }
        private void add_Click(object sender, EventArgs e)
        {

            if (IsValid())
            {

                using (SqlConnection con = new SqlConnection(connectionString))
                {
                    SqlCommand cmd = new SqlCommand("INSERT INTO venueInfo VALUES (@venueID, @venueName, @capacity, @location, @status)", con);
                    cmd.CommandType = CommandType.Text;
                    cmd.Parameters.AddWithValue("@venueID", txtvID.Text);
                    cmd.Parameters.AddWithValue("@venueName", txtVName.Text);
                    cmd.Parameters.AddWithValue("@capacity", txtCapacity.Text);
                    cmd.Parameters.AddWithValue("@location", txtlocation.Text);
                    cmd.Parameters.AddWithValue("@status", txtstatus.Text);

                    con.Open();
                    cmd.ExecuteNonQuery();
                    con.Close();
                }

                MessageBox.Show("New venue is successfully saved in the database", "Saved", MessageBoxButtons.OK, MessageBoxIcon.Information);
                LoadData();
                ResetFormControls();
            }
        }

        private bool IsValid()
        {
            if (txtvID.Text == string.Empty)
            {
                MessageBox.Show("Venue ID is required", "Failed", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }

            return true;
        }

        private void Update_Click(object sender, EventArgs e)
        {
            if (int.TryParse(txtvID.Text, out int venueID) && venueID > 0)
            {
                using (SqlConnection con = new SqlConnection(connectionString))
                {
                    SqlCommand cmd = new SqlCommand("UPDATE venueInfo SET venueID = @venueID, venueName=@venueName, capacity = @capacity, location = @location , status = @status WHERE venueID = @venueID", con);
                    cmd.CommandType = CommandType.Text;
                    cmd.Parameters.AddWithValue("@venueID", txtvID.Text);
                    cmd.Parameters.AddWithValue("@venueName", txtVName.Text);
                    cmd.Parameters.AddWithValue("@capacity", txtCapacity.Text);
                    cmd.Parameters.AddWithValue("@location", txtlocation.Text);
                    cmd.Parameters.AddWithValue("@status", txtstatus.Text);

                    con.Open();
                    cmd.ExecuteNonQuery();
                    con.Close();
                }


                MessageBox.Show("Venue Information is successfully updated", "UPDATED", MessageBoxButtons.OK, MessageBoxIcon.Information);
                LoadData();
                ResetFormControls();
            }
            else
            {
                MessageBox.Show(" Please Enter Venue Information ", "SELECT?", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void Delete_Click(object sender, EventArgs e)
        {

            if (int.TryParse(txtvID.Text, out int venueID) && venueID > 0)
            {
                using (SqlConnection con = new SqlConnection(connectionString))
                {
                    SqlCommand cmd = new SqlCommand("DELETE From venueInfo WHERE venueID = @venueID", con);
                    cmd.CommandType = CommandType.Text;
                    cmd.Parameters.AddWithValue("@venueID", txtvID.Text);

                    con.Open();
                    cmd.ExecuteNonQuery();
                    con.Close();
                }


                MessageBox.Show("Venue Information is Deleted", "DELETED", MessageBoxButtons.OK, MessageBoxIcon.Information);
                LoadData();
                ResetFormControls();
            }
            else
            {
                MessageBox.Show(" Please Enter Venue Information ", "SELECT?", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void ResetFormControls()
        {
            txtvID.Clear();
            txtVName.Clear();
            txtlocation.Clear();
            txtstatus.Clear();
            txtCapacity.Clear();

            txtvID.Focus();


        }
        private void Reset_Click(object sender, EventArgs e)
        {

            ResetFormControls();
        }

        private void BackButton_Click(object sender, EventArgs e)
        {
            this.Hide();
            VenueDashboard F1 = new VenueDashboard(username);
            F1.Show();
        }

        private void logoutBtn_Click(object sender, EventArgs e)
        {
            this.Hide();
            Login loginForm = new Login();
            loginForm.Show();
        }
    }
}
