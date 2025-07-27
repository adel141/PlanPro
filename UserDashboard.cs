using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace PlanPro
{
    public partial class UserDashboard: Form
    {
        private string username;
        public UserDashboard(string username)

        {
            InitializeComponent();
            this.username = username;
        }

        private void UserDashboard_Load(object sender, EventArgs e)
        {

        }

        private void siticoneButton1_Click(object sender, EventArgs e)
        {
        }

        private void logoutBtn_Click(object sender, EventArgs e)
        {
            this.Hide();
            Login loginForm = new Login();
            loginForm.Show();
        }

        private void siticoneButton2_Click(object sender, EventArgs e)
        {

            this.Hide();
            EvenetRegistration eventRegistrationForm = new EvenetRegistration(username);
            eventRegistrationForm.Show();
        }

        private void siticoneButton4_Click(object sender, EventArgs e)
        {
            this.Hide();
            Feedback feedbackForm = new Feedback(username);
            feedbackForm.Show();
        }

        private void siticoneButton3_Click(object sender, EventArgs e)
        {
            this.Hide();
            ManageAttendeeProfile userProfileForm = new ManageAttendeeProfile(username);
            userProfileForm.Show();

        }
    }
}
