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
using Microsoft.IdentityModel.Tokens;

namespace PlanPro
{
    public partial class Organizer: Form
    {
        private const string connectionString = @"Data Source=localhost;Initial Catalog=proplan;Integrated Security=True;";
        private SqlConnection conn = new SqlConnection(connectionString);
        string user;

        public Organizer(string username)
        {
            InitializeComponent();
            LoadVenuesIntoComboBox();
            this.organizerName = username;
            this.user= username;

        }


        private string title,
            description,
            eventDate,
            endDate,
            startTime,
            endTime,
            venueId,
            organizerName;

        private void addeventbtn_Click(object sender, EventArgs e)
        {
            creatEventPanel.Visible = true;
            viewEventpanel.Visible = false;
        }

        private void showalleventBtn_Click(object sender, EventArgs e)
        {
            conn.Open();
            string query = $"SELECT * FROM Eventtb where organizerid ='{organizerName}'";
            SqlCommand cmd = new SqlCommand(query, conn);
            SqlDataAdapter adapter = new SqlDataAdapter(cmd);
            DataSet ds = new DataSet();
            adapter.Fill(ds);
            DataTable dt = ds.Tables[0];

            eventdatagrid.AutoGenerateColumns = true;
            eventdatagrid.AutoResizeRows(DataGridViewAutoSizeRowsMode.AllCells);
            eventdatagrid.DataSource = dt;
            conn.Close();


        }

        private void enableEditEventBtn_Click(object sender, EventArgs e)
        {

        }

        private void eventdatagrid_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            textBox2.Text = eventdatagrid.Rows[e.RowIndex].Cells[0].Value.ToString();
            textBox1.Text = eventdatagrid.Rows[e.RowIndex].Cells[1].Value.ToString();
            richTextBox1.Text = eventdatagrid.Rows[e.RowIndex].Cells[2].Value.ToString();
            dateTimePicker2.Text = eventdatagrid.Rows[e.RowIndex].Cells[3].Value.ToString();
            siticoneTimePicker1.Text = eventdatagrid.Rows[e.RowIndex].Cells[4].Value.ToString();
            siticoneTimePicker2.Text = eventdatagrid.Rows[e.RowIndex].Cells[5].Value.ToString();
            comboBox1.Text=eventdatagrid.Rows[e.RowIndex].Cells[6].Value.ToString();
            textBox3.Text = eventdatagrid.Rows[e.RowIndex].Cells[7].Value.ToString();

        }

        private void logoutBtn_Click(object sender, EventArgs e)
        {
            this.Hide();
            new Login().Show();
        }

        private void siticoneImageButton1_Click(object sender, EventArgs e)
        {
            new OrganizerProfile(user).Show();
            this.Hide();
        }

        private void clear()
        {
            textBox1.Clear();
            textBox2.Clear();
            richTextBox1.Clear();
            dateTimePicker2.ResetText();

        }

        private void eventDeleteBtn_Click(object sender, EventArgs e)
        {
            conn.Open();
            string deleteQuery = $"DELETE FROM Eventtb WHERE eventId='{textBox2.Text}'";
            SqlCommand cmd = new SqlCommand(deleteQuery, conn);
            cmd.ExecuteNonQuery();
            MessageBox.Show("Event deleted successfully.");
            clear();
            eventdatagrid.Refresh();
            conn.Close();

        }

        private void updateEventBtn_Click(object sender, EventArgs e)
        {
            title = textBox1.Text;
            description = richTextBox1.Text;
            eventDate = dateTimePicker2.Value.ToString("yyyy-MM-dd");
            DateTime dt = DateTime.Parse(siticoneTimePicker1.GetTime().ToString());
            string timeOnly = dt.ToString("HH:mm");  // 24-hour format
            TimeSpan startTime = TimeSpan.Parse(timeOnly);
            dt = DateTime.Parse(siticoneTimePicker2.GetTime().ToString());
            timeOnly = dt.ToString("HH:mm");  // 24-hour 
            TimeSpan endTime = TimeSpan.Parse(timeOnly);
            venueId = comboBox1.Text;


            conn.Open();
            string updateQuery = $"UPDATE Eventtb SET title='{title}', description='{description}', eventDate='{eventDate}', startTime='{startTime}', endTime='{endTime}', venueid='{venueId}' WHERE eventId='{textBox2.Text}'";
            SqlCommand cmd = new SqlCommand(updateQuery, conn);
            cmd.ExecuteNonQuery();
            MessageBox.Show("Event Updated successfully.");
            eventdatagrid.Refresh();
            conn.Close();
            clear();

        }

        private void siticoneButton2_Click(object sender, EventArgs e)
        {
            viewEventpanel.Visible = true;
            creatEventPanel.Visible = false;
        }

        private bool isValidData()
        {
            if (string.IsNullOrEmpty(eventTitleText.Text))
            {
                MessageBox.Show("Please enter the event title.");
                return false;
            }
            if (string.IsNullOrEmpty(descTox.Text))
            {
                MessageBox.Show("Please enter the event description.");
                return false;
            }
            if (string.IsNullOrEmpty(dateTimePicker1.Text))
            {
                MessageBox.Show("Please enter the Event date.");
                return false;
            }
            if (string.IsNullOrEmpty(stratTimeText.Text))
            {
                MessageBox.Show("Please enter the start time.");
                return false;
            }
            if (string.IsNullOrEmpty(EndTimeText.Text))
            {
                MessageBox.Show("Please enter the end time.");
                return false;
            }
            if (VenueNameText.SelectedIndex == -1)
            {
                MessageBox.Show("Please select a venue.");
                return false;
            }
            return true;
        }
        private void creatnewEvent_Click(object sender, EventArgs e)
        {
            
            if(isValidData())
            {
                conn.Open();
                title = eventTitleText.Text;
                description = descTox.Text;
                eventDate = dateTimePicker1.Value.ToString("yyyy-MM-dd");
                DateTime dt = DateTime.Parse(stratTimeText.GetTime().ToString());
                string timeOnly = dt.ToString("HH:mm");  // 24-hour format
                TimeSpan startTime = TimeSpan.Parse(timeOnly);
                dt = DateTime.Parse(EndTimeText.GetTime().ToString());
                timeOnly = dt.ToString("HH:mm");  // 24-hour 
                TimeSpan endTime = TimeSpan.Parse(timeOnly);
                venueId = VenueNameText.SelectedItem.ToString(); 
                string insertQuery = $"INSERT INTO Eventtb (title, description, eventDate, startTime, endTime, organizerID,venueid) " +
                                     $"VALUES ('{title}', '{description}', '{eventDate}', '{startTime}', '{endTime}', '{organizerName}','{venueId}')";
                SqlCommand cmd = new SqlCommand(insertQuery, conn);
                cmd.ExecuteNonQuery();
                MessageBox.Show("Event created successfully.");
                conn.Close();
            }
        }
        private void LoadVenuesIntoComboBox()
        {
                
            conn.Open();
            string query = "SELECT VenueName FROM Venueinfo"; 

            SqlCommand cmd = new SqlCommand(query, conn);
            SqlDataReader reader = cmd.ExecuteReader();

            while (reader.Read())
            {
                VenueNameText.Items.Add(reader["VenueName"].ToString());
                comboBox1.Items.Add(reader["VenueName"].ToString());
            }

            reader.Close();
            conn.Close();
        }



    }
}
