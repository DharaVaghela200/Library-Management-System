// ============================================
// File: LoginForm.cs
// ============================================
using System;
using System.Data;
using System.Data.SqlClient;
using System.Windows.Forms;

namespace LibraryManagementSystem
{
    public partial class LoginForm : Form
    {
        public LoginForm()
        {
            InitializeComponent();
        }

        private void btnLogin_Click(object sender, EventArgs e)
        {
            lblError.Text = "";
            string username = txtUsername.Text.Trim();
            string password = txtPassword.Text.Trim();

            if (string.IsNullOrEmpty(username) || string.IsNullOrEmpty(password))
            {
                lblError.Text = "Please enter username and password.";
                return;
            }

            string query = "SELECT UserID, Username, Role FROM Users WHERE Username=@u AND Password=@p";
            SqlParameter[] p = {
                new SqlParameter("@u", username),
                new SqlParameter("@p", password)
            };

            DataTable dt = DatabaseHelper.ExecuteQuery(query, p);

            if (dt.Rows.Count > 0)
            {
                SessionHelper.UserID   = Convert.ToInt32(dt.Rows[0]["UserID"]);
                SessionHelper.Username = dt.Rows[0]["Username"].ToString();
                SessionHelper.Role     = dt.Rows[0]["Role"].ToString();

                // If student, load StudentID
                if (SessionHelper.IsStudent)
                {
                    string sq = "SELECT StudentID FROM Students WHERE UserID=@uid";
                    SqlParameter[] sp = { new SqlParameter("@uid", SessionHelper.UserID) };
                    object sid = DatabaseHelper.ExecuteScalar(sq, sp);
                    if (sid != null) SessionHelper.StudentID = Convert.ToInt32(sid);
                }

                this.Hide();
                MainDashboard dash = new MainDashboard();
                dash.FormClosed += (s, args) => this.Close();
                dash.Show();
            }
            else
            {
                lblError.Text = "Invalid username or password.";
            }
        }

        private void btnRegister_Click(object sender, EventArgs e)
        {
            RegisterForm reg = new RegisterForm();
            reg.ShowDialog();
        }
    }
}
