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
    public partial class VenueDashboard: Form
    {
        private string role;
        public VenueDashboard(string role)
        {
            InitializeComponent();
            this.role = role;
        }

        private void viewbtn_Click(object sender, EventArgs e)
        {
            this.Hide();
            View v1 = new View(role);
            v1.Show();
        }

        private void logoutBtn_Click(object sender, EventArgs e)
        {
            this.Hide();
            Login loginForm = new Login();
            loginForm.Show();
        }
    }
}
