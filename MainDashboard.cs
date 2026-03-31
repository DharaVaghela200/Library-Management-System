// ============================================
// File: MainDashboard.cs
// ============================================
using System;
using System.Data;
using System.Drawing;
using System.Windows.Forms;

namespace LibraryManagementSystem
{
    public partial class MainDashboard : Form
    {
        public MainDashboard()
        {
            InitializeComponent();
        }

        private void MainDashboard_Load(object sender, EventArgs e)
        {
            lblUser.Text = $"👤 {SessionHelper.Username}  [{SessionHelper.Role}]";

            // Show/hide buttons based on role
            btnStudents.Visible = SessionHelper.IsAdmin;
            btnIssue.Visible    = SessionHelper.IsAdmin;
            btnMyBooks.Visible  = SessionHelper.IsStudent;

            ShowDashboardHome();
        }

        private void LoadPanel(UserControl uc)
        {
            panelContent.Controls.Clear();
            uc.Dock = DockStyle.Fill;
            panelContent.Controls.Add(uc);
        }

        private void ShowDashboardHome()
        {
            panelContent.Controls.Clear();

            // Stats cards
            string[] titles = { "Total Books", "Available Books", "Total Students", "Issued Books" };
            string[] queries = {
                "SELECT COUNT(*) FROM Books",
                "SELECT SUM(AvailableCopies) FROM Books",
                "SELECT COUNT(*) FROM Students",
                "SELECT COUNT(*) FROM IssuedBooks WHERE Status='Issued'"
            };
            Color[] colors = {
                Color.FromArgb(31, 73, 125),
                Color.FromArgb(0, 128, 0),
                Color.FromArgb(148, 88, 0),
                Color.FromArgb(192, 0, 0)
            };

            for (int i = 0; i < titles.Length; i++)
            {
                if (!SessionHelper.IsAdmin && (i == 2 || i == 3)) continue;

                Panel card = new Panel();
                card.BackColor = colors[i];
                card.Size = new Size(180, 100);
                card.Location = new Point(20 + i * 200, 30);

                Label lVal = new Label();
                lVal.Font = new Font("Segoe UI", 22F, FontStyle.Bold);
                lVal.ForeColor = Color.White;
                lVal.Location = new Point(15, 15);
                lVal.Size = new Size(150, 45);
                lVal.Text = DatabaseHelper.ExecuteScalar(queries[i])?.ToString() ?? "0";

                Label lTitle = new Label();
                lTitle.Font = new Font("Segoe UI", 10F);
                lTitle.ForeColor = Color.FromArgb(220, 230, 255);
                lTitle.Location = new Point(15, 60);
                lTitle.Size = new Size(150, 25);
                lTitle.Text = titles[i];

                card.Controls.Add(lVal);
                card.Controls.Add(lTitle);
                panelContent.Controls.Add(card);
            }

            Label lblWelcome = new Label();
            lblWelcome.Font = new Font("Segoe UI", 14F, FontStyle.Bold);
            lblWelcome.ForeColor = Color.FromArgb(31, 73, 125);
            lblWelcome.Location = new Point(20, 155);
            lblWelcome.Size = new Size(760, 35);
            lblWelcome.Text = $"Welcome, {SessionHelper.Username}! You are logged in as {SessionHelper.Role}.";
            panelContent.Controls.Add(lblWelcome);
        }

        private void btnDashboard_Click(object sender, EventArgs e) => ShowDashboardHome();

        private void btnBooks_Click(object sender, EventArgs e)
        {
            LoadPanel(new BooksControl());
        }

        private void btnStudents_Click(object sender, EventArgs e)
        {
            if (SessionHelper.IsAdmin) LoadPanel(new StudentsControl());
        }

        private void btnIssue_Click(object sender, EventArgs e)
        {
            if (SessionHelper.IsAdmin) LoadPanel(new IssueBookControl());
        }

        private void btnMyBooks_Click(object sender, EventArgs e)
        {
            if (SessionHelper.IsStudent) LoadPanel(new MyBooksControl());
        }

        private void btnLogout_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show("Are you sure you want to logout?", "Logout",
                MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
            {
                SessionHelper.Clear();
                this.Close();
            }
        }
    }
}
