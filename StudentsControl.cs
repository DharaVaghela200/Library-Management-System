// ============================================
// File: StudentsControl.cs  (UserControl - Admin Only)
// ============================================
using System;
using System.Data.SqlClient;
using System.Drawing;
using System.Windows.Forms;

namespace LibraryManagementSystem
{
    public class StudentsControl : UserControl
    {
        private DataGridView dgv;
        private TextBox txtSearch, txtName, txtEmail, txtPhone, txtAddress;
        private Button btnSearch, btnUpdate, btnDelete, btnClear;
        private int selectedStudentID = -1;

        public StudentsControl()
        {
            BuildUI();
            LoadStudents();
        }

        private void BuildUI()
        {
            this.BackColor = Color.FromArgb(245, 245, 250);

            var lbl = new Label {
                Text = "Student Management", Location = new Point(15, 15),
                Size = new Size(400, 30), Font = new Font("Segoe UI", 14F, FontStyle.Bold),
                ForeColor = Color.FromArgb(31, 73, 125)
            };

            txtSearch = new TextBox {
                Location = new Point(15, 55), Size = new Size(280, 28),
                Font = new Font("Segoe UI", 10F)
            };
            SetWatermark(txtSearch, "Search by name or email...");

            btnSearch = MakeBtn("Search", 305, 53, 100, Color.FromArgb(31, 73, 125));
            btnSearch.Click += (s, e) => {
                string search = txtSearch.ForeColor == Color.Gray ? "" : txtSearch.Text.Trim();
                LoadStudents(search);
            };

            dgv = new DataGridView {
                Location = new Point(15, 90), Size = new Size(790, 240),
                AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill,
                SelectionMode = DataGridViewSelectionMode.FullRowSelect,
                ReadOnly = true, AllowUserToAddRows = false, RowHeadersVisible = false,
                BackgroundColor = Color.White, Font = new Font("Segoe UI", 9.5F)
            };
            dgv.DefaultCellStyle.SelectionBackColor = Color.FromArgb(31, 73, 125);
            dgv.CellClick += Dgv_CellClick;

            var pForm = new Panel {
                BackColor = Color.White, Location = new Point(15, 340), Size = new Size(790, 200)
            };

            int ly = 15;
            pForm.Controls.Add(MkLbl("Full Name", 20, ly));
            txtName = MkTxt(130, ly, 200); pForm.Controls.Add(txtName);

            pForm.Controls.Add(MkLbl("Email", 350, ly));
            txtEmail = MkTxt(420, ly, 200); pForm.Controls.Add(txtEmail);

            ly += 50;
            pForm.Controls.Add(MkLbl("Phone", 20, ly));
            txtPhone = MkTxt(130, ly, 200); pForm.Controls.Add(txtPhone);

            pForm.Controls.Add(MkLbl("Address", 350, ly));
            txtAddress = MkTxt(420, ly, 340); pForm.Controls.Add(txtAddress);

            ly += 55;
            btnUpdate = MakeBtn("Update", 20, ly, 110, Color.FromArgb(31, 73, 125));
            btnUpdate.Click += BtnUpdate_Click;
            pForm.Controls.Add(btnUpdate);

            btnDelete = MakeBtn("Delete", 140, ly, 110, Color.FromArgb(192, 0, 0));
            btnDelete.Click += BtnDelete_Click;
            pForm.Controls.Add(btnDelete);

            btnClear = MakeBtn("Clear", 260, ly, 90, Color.Gray);
            btnClear.Click += (s, e) => ClearForm();
            pForm.Controls.Add(btnClear);

            this.Controls.AddRange(new Control[] { lbl, txtSearch, btnSearch, dgv, pForm });
        }

        private void SetWatermark(TextBox txt, string watermark)
        {
            txt.Text = watermark;
            txt.ForeColor = Color.Gray;
            txt.GotFocus += (s, e) => {
                if (txt.ForeColor == Color.Gray) { txt.Text = ""; txt.ForeColor = Color.Black; }
            };
            txt.LostFocus += (s, e) => {
                if (string.IsNullOrWhiteSpace(txt.Text)) { txt.Text = watermark; txt.ForeColor = Color.Gray; }
            };
        }

        private void LoadStudents(string search = "")
        {
            string q = @"SELECT StudentID, FullName, Email, Phone, Address, EnrollmentDate
                         FROM Students
                         WHERE FullName LIKE @s OR Email LIKE @s
                         ORDER BY FullName";
            dgv.DataSource = DatabaseHelper.ExecuteQuery(q,
                new[] { new SqlParameter("@s", "%" + search + "%") });
            if (dgv.Columns.Contains("StudentID"))
                dgv.Columns["StudentID"].Visible = false;
        }

        private void Dgv_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex < 0) return;
            var row = dgv.Rows[e.RowIndex];
            selectedStudentID = Convert.ToInt32(row.Cells["StudentID"].Value);
            txtName.Text    = row.Cells["FullName"].Value?.ToString() ?? "";
            txtEmail.Text   = row.Cells["Email"].Value?.ToString() ?? "";
            txtPhone.Text   = row.Cells["Phone"].Value?.ToString() ?? "";
            txtAddress.Text = row.Cells["Address"].Value?.ToString() ?? "";
            txtName.ForeColor = txtEmail.ForeColor =
            txtPhone.ForeColor = txtAddress.ForeColor = Color.Black;
        }

        private void BtnUpdate_Click(object sender, EventArgs e)
        {
            if (selectedStudentID < 0) { MessageBox.Show("Select a student first."); return; }
            string q = "UPDATE Students SET FullName=@n, Email=@e, Phone=@p, Address=@a WHERE StudentID=@id";
            DatabaseHelper.ExecuteNonQuery(q, new[] {
                new SqlParameter("@n",  txtName.Text.Trim()),
                new SqlParameter("@e",  txtEmail.Text.Trim()),
                new SqlParameter("@p",  txtPhone.Text.Trim()),
                new SqlParameter("@a",  txtAddress.Text.Trim()),
                new SqlParameter("@id", selectedStudentID)
            });
            MessageBox.Show("Student updated!", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
            ClearForm(); LoadStudents();
        }

        private void BtnDelete_Click(object sender, EventArgs e)
        {
            if (selectedStudentID < 0) { MessageBox.Show("Select a student first."); return; }
            if (MessageBox.Show("Delete this student?", "Confirm",
                MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
            {
                DatabaseHelper.ExecuteNonQuery("DELETE FROM Students WHERE StudentID=@id",
                    new[] { new SqlParameter("@id", selectedStudentID) });
                ClearForm(); LoadStudents();
            }
        }

        private void ClearForm()
        {
            selectedStudentID = -1;
            txtName.Text = txtEmail.Text = txtPhone.Text = txtAddress.Text = "";
        }

        private Label MkLbl(string t, int x, int y) =>
            new Label {
                Text = t, Location = new Point(x, y), Size = new Size(105, 22),
                Font = new Font("Segoe UI", 10F)
            };

        private TextBox MkTxt(int x, int y, int w) =>
            new TextBox {
                Location = new Point(x, y), Size = new Size(w, 28),
                Font = new Font("Segoe UI", 10F), BorderStyle = BorderStyle.FixedSingle
            };

        private Button MakeBtn(string text, int x, int y, int w, Color bg)
        {
            var b = new Button {
                Text = text, Location = new Point(x, y), Size = new Size(w, 34),
                BackColor = bg, ForeColor = Color.White, FlatStyle = FlatStyle.Flat,
                Font = new Font("Segoe UI", 9.5F), Cursor = Cursors.Hand
            };
            b.FlatAppearance.BorderSize = 0;
            return b;
        }
    }
}
