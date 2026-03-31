// ============================================
// File: MainDashboard.Designer.cs
// ============================================
namespace LibraryManagementSystem
{
    partial class MainDashboard
    {
        private System.ComponentModel.IContainer components = null;
        protected override void Dispose(bool disposing)
        {
            if (disposing && components != null) components.Dispose();
            base.Dispose(disposing);
        }

        private void InitializeComponent()
        {
            this.panelTop = new System.Windows.Forms.Panel();
            this.lblHeader = new System.Windows.Forms.Label();
            this.lblUser = new System.Windows.Forms.Label();
            this.btnLogout = new System.Windows.Forms.Button();
            this.panelSide = new System.Windows.Forms.Panel();
            this.btnDashboard = new System.Windows.Forms.Button();
            this.btnBooks = new System.Windows.Forms.Button();
            this.btnStudents = new System.Windows.Forms.Button();
            this.btnIssue = new System.Windows.Forms.Button();
            this.btnMyBooks = new System.Windows.Forms.Button();
            this.panelContent = new System.Windows.Forms.Panel();
            this.panelTop.SuspendLayout();
            this.panelSide.SuspendLayout();
            this.SuspendLayout();

            // panelTop
            this.panelTop.BackColor = System.Drawing.Color.FromArgb(31, 73, 125);
            this.panelTop.Controls.Add(this.lblHeader);
            this.panelTop.Controls.Add(this.lblUser);
            this.panelTop.Controls.Add(this.btnLogout);
            this.panelTop.Dock = System.Windows.Forms.DockStyle.Top;
            this.panelTop.Height = 60;

            this.lblHeader.Font = new System.Drawing.Font("Segoe UI", 16F, System.Drawing.FontStyle.Bold);
            this.lblHeader.ForeColor = System.Drawing.Color.White;
            this.lblHeader.Location = new System.Drawing.Point(20, 12);
            this.lblHeader.Size = new System.Drawing.Size(400, 35);
            this.lblHeader.Text = "📚 Library Management System";

            this.lblUser.Font = new System.Drawing.Font("Segoe UI", 10F);
            this.lblUser.ForeColor = System.Drawing.Color.LightBlue;
            this.lblUser.Location = new System.Drawing.Point(700, 20);
            this.lblUser.Size = new System.Drawing.Size(200, 22);
            this.lblUser.TextAlign = System.Drawing.ContentAlignment.MiddleRight;

            this.btnLogout.BackColor = System.Drawing.Color.FromArgb(192, 0, 0);
            this.btnLogout.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnLogout.FlatAppearance.BorderSize = 0;
            this.btnLogout.Font = new System.Drawing.Font("Segoe UI", 10F);
            this.btnLogout.ForeColor = System.Drawing.Color.White;
            this.btnLogout.Location = new System.Drawing.Point(910, 12);
            this.btnLogout.Size = new System.Drawing.Size(90, 35);
            this.btnLogout.Text = "Logout";
            this.btnLogout.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnLogout.Click += new System.EventHandler(this.btnLogout_Click);

            // panelSide
            this.panelSide.BackColor = System.Drawing.Color.FromArgb(40, 55, 75);
            this.panelSide.Controls.Add(this.btnDashboard);
            this.panelSide.Controls.Add(this.btnBooks);
            this.panelSide.Controls.Add(this.btnStudents);
            this.panelSide.Controls.Add(this.btnIssue);
            this.panelSide.Controls.Add(this.btnMyBooks);
            this.panelSide.Location = new System.Drawing.Point(0, 60);
            this.panelSide.Size = new System.Drawing.Size(200, 600);

            CreateSideBtn(this.btnDashboard, "🏠  Dashboard", 0, this.btnDashboard_Click);
            CreateSideBtn(this.btnBooks, "📖  Manage Books", 1, this.btnBooks_Click);
            CreateSideBtn(this.btnStudents, "👥  Students", 2, this.btnStudents_Click);
            CreateSideBtn(this.btnIssue, "📋  Issue / Return", 3, this.btnIssue_Click);
            CreateSideBtn(this.btnMyBooks, "📌  My Books", 4, this.btnMyBooks_Click);

            // panelContent
            this.panelContent.BackColor = System.Drawing.Color.FromArgb(245, 245, 250);
            this.panelContent.Location = new System.Drawing.Point(200, 60);
            this.panelContent.Size = new System.Drawing.Size(820, 600);

            // MainDashboard
            this.ClientSize = new System.Drawing.Size(1020, 660);
            this.Controls.Add(this.panelTop);
            this.Controls.Add(this.panelSide);
            this.Controls.Add(this.panelContent);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.MaximizeBox = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Library Management System";
            this.Load += new System.EventHandler(this.MainDashboard_Load);
            this.panelTop.ResumeLayout(false);
            this.panelSide.ResumeLayout(false);
            this.ResumeLayout(false);
        }

        private void CreateSideBtn(System.Windows.Forms.Button btn, string text, int index,
            System.EventHandler handler)
        {
            btn.BackColor = System.Drawing.Color.FromArgb(40, 55, 75);
            btn.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            btn.FlatAppearance.BorderSize = 0;
            btn.Font = new System.Drawing.Font("Segoe UI", 11F);
            btn.ForeColor = System.Drawing.Color.White;
            btn.Location = new System.Drawing.Point(0, index * 60);
            btn.Size = new System.Drawing.Size(200, 55);
            btn.Text = text;
            btn.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            btn.Padding = new System.Windows.Forms.Padding(15, 0, 0, 0);
            btn.Cursor = System.Windows.Forms.Cursors.Hand;
            btn.Click += handler;
        }

        private System.Windows.Forms.Panel panelTop, panelSide;
        public System.Windows.Forms.Panel panelContent;
        private System.Windows.Forms.Label lblHeader, lblUser;
        private System.Windows.Forms.Button btnLogout, btnDashboard, btnBooks, btnStudents, btnIssue, btnMyBooks;
    }
}
