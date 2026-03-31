// ============================================
// File: RegisterForm.cs
// ============================================
using System;
using System.Data.SqlClient;
using System.Windows.Forms;

namespace LibraryManagementSystem
{
    public partial class RegisterForm : Form
    {
        public RegisterForm()
        {
            InitializeComponent();
        }

        private void btnRegister_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtName.Text) ||
                string.IsNullOrWhiteSpace(txtUsername.Text) ||
                string.IsNullOrWhiteSpace(txtPassword.Text))
            {
                MessageBox.Show("Name, Username, and Password are required.", "Validation",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            // Check username exists
            string checkQ = "SELECT COUNT(*) FROM Users WHERE Username=@u";
            SqlParameter[] cp = { new SqlParameter("@u", txtUsername.Text.Trim()) };
            int count = Convert.ToInt32(DatabaseHelper.ExecuteScalar(checkQ, cp));
            if (count > 0)
            {
                MessageBox.Show("Username already exists. Choose another.", "Error",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            // Insert User
            string userQ = "INSERT INTO Users (Username, Password, Role) VALUES (@u, @p, 'Student'); SELECT SCOPE_IDENTITY();";
            SqlParameter[] up = {
                new SqlParameter("@u", txtUsername.Text.Trim()),
                new SqlParameter("@p", txtPassword.Text.Trim())
            };
            object userID = DatabaseHelper.ExecuteScalar(userQ, up);

            if (userID != null)
            {
                // Insert Student
                string studQ = @"INSERT INTO Students (FullName, Email, Phone, UserID)
                                 VALUES (@name, @email, @phone, @uid)";
                SqlParameter[] sp = {
                    new SqlParameter("@name", txtName.Text.Trim()),
                    new SqlParameter("@email", txtEmail.Text.Trim()),
                    new SqlParameter("@phone", txtPhone.Text.Trim()),
                    new SqlParameter("@uid", Convert.ToInt32(userID))
                };
                DatabaseHelper.ExecuteNonQuery(studQ, sp);

                MessageBox.Show("Registration successful! You can now login.", "Success",
                    MessageBoxButtons.OK, MessageBoxIcon.Information);
                this.Close();
            }
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
