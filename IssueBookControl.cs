// ============================================
// File: IssueBookControl.cs  (UserControl - Admin Only)
// Handles: Issue Books & Return Books
// ============================================
using System;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Windows.Forms;

namespace LibraryManagementSystem
{
    public class IssueBookControl : UserControl
    {
        private DataGridView dgvIssued;
        private ComboBox cmbBooks, cmbStudents;
        private DateTimePicker dtpDueDate;
        private Button btnIssue, btnReturn;
        private TabControl tabControl;

        public IssueBookControl()
        {
            BuildUI();
        }

        private void BuildUI()
        {
            this.BackColor = Color.FromArgb(245, 245, 250);

            var lbl = new Label { Text = "📋 Issue / Return Books", Location = new Point(15, 15),
                Size = new Size(400, 30), Font = new Font("Segoe UI", 14F, FontStyle.Bold),
                ForeColor = Color.FromArgb(31, 73, 125) };

            tabControl = new TabControl { Location = new Point(15, 55), Size = new Size(790, 510),
                Font = new Font("Segoe UI", 10F) };

            // ── TAB 1: Issue Book ──
            TabPage tabIssue = new TabPage("  📤 Issue Book  ");
            tabIssue.BackColor = Color.White;

            int lx = 30, lw = 110, cx = 150, cw = 350, ly = 30;

            tabIssue.Controls.Add(MkLbl("Select Book:", lx, ly, lw));
            cmbBooks = new ComboBox { Location = new Point(cx, ly), Size = new Size(cw, 30),
                Font = new Font("Segoe UI", 10F), DropDownStyle = ComboBoxStyle.DropDownList };
            tabIssue.Controls.Add(cmbBooks);

            ly += 50;
            tabIssue.Controls.Add(MkLbl("Select Student:", lx, ly, lw));
            cmbStudents = new ComboBox { Location = new Point(cx, ly), Size = new Size(cw, 30),
                Font = new Font("Segoe UI", 10F), DropDownStyle = ComboBoxStyle.DropDownList };
            tabIssue.Controls.Add(cmbStudents);

            ly += 50;
            tabIssue.Controls.Add(MkLbl("Due Date:", lx, ly, lw));
            dtpDueDate = new DateTimePicker { Location = new Point(cx, ly), Size = new Size(200, 28),
                Font = new Font("Segoe UI", 10F), MinDate = DateTime.Today.AddDays(1),
                Value = DateTime.Today.AddDays(14) };
            tabIssue.Controls.Add(dtpDueDate);

            ly += 55;
            btnIssue = MakeBtn("📤 Issue Book", lx, ly, 160, Color.FromArgb(31, 73, 125));
            btnIssue.Click += BtnIssue_Click;
            tabIssue.Controls.Add(btnIssue);

            // ── TAB 2: Return Book ──
            TabPage tabReturn = new TabPage("  📥 Return Book  ");
            tabReturn.BackColor = Color.White;

            var lblGrid = new Label { Text = "Currently Issued Books:", Location = new Point(20, 15),
                Size = new Size(300, 25), Font = new Font("Segoe UI", 11F, FontStyle.Bold) };
            tabReturn.Controls.Add(lblGrid);

            dgvIssued = new DataGridView { Location = new Point(20, 45), Size = new Size(740, 340),
                AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill,
                SelectionMode = DataGridViewSelectionMode.FullRowSelect,
                ReadOnly = true, AllowUserToAddRows = false, RowHeadersVisible = false,
                BackgroundColor = Color.White, Font = new Font("Segoe UI", 9.5F) };
            dgvIssued.DefaultCellStyle.SelectionBackColor = Color.FromArgb(31, 73, 125);
            tabReturn.Controls.Add(dgvIssued);

            btnReturn = MakeBtn("📥 Return Selected Book", 20, 400, 220, Color.FromArgb(0, 128, 0));
            btnReturn.Click += BtnReturn_Click;
            tabReturn.Controls.Add(btnReturn);

            tabControl.TabPages.Add(tabIssue);
            tabControl.TabPages.Add(tabReturn);
            tabControl.SelectedIndexChanged += (s, e) => {
                if (tabControl.SelectedIndex == 0) LoadCombos();
                else LoadIssuedBooks();
            };

            this.Controls.AddRange(new Control[] { lbl, tabControl });
            LoadCombos();
            LoadIssuedBooks();
        }

        private void LoadCombos()
        {
            // Books with available copies
            DataTable dtB = DatabaseHelper.ExecuteQuery(
                "SELECT BookID, Title + ' (Available: ' + CAST(AvailableCopies AS NVARCHAR) + ')' AS BookInfo FROM Books WHERE AvailableCopies > 0");
            cmbBooks.DataSource = dtB;
            cmbBooks.DisplayMember = "BookInfo";
            cmbBooks.ValueMember = "BookID";

            // All students
            DataTable dtS = DatabaseHelper.ExecuteQuery(
                "SELECT StudentID, FullName FROM Students ORDER BY FullName");
            cmbStudents.DataSource = dtS;
            cmbStudents.DisplayMember = "FullName";
            cmbStudents.ValueMember = "StudentID";
        }

        private void LoadIssuedBooks()
        {
            string q = @"SELECT ib.IssueID, b.Title AS [Book Title], s.FullName AS [Student],
                                ib.IssueDate, ib.DueDate, ib.Status
                         FROM IssuedBooks ib
                         JOIN Books b ON ib.BookID = b.BookID
                         JOIN Students s ON ib.StudentID = s.StudentID
                         WHERE ib.Status = 'Issued'
                         ORDER BY ib.IssueDate DESC";
            dgvIssued.DataSource = DatabaseHelper.ExecuteQuery(q);
            if (dgvIssued.Columns.Contains("IssueID")) dgvIssued.Columns["IssueID"].Visible = false;
        }

        private void BtnIssue_Click(object sender, EventArgs e)
        {
            if (cmbBooks.SelectedValue == null || cmbStudents.SelectedValue == null)
            {
                MessageBox.Show("Please select both a book and a student.", "Validation",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            int bookID = Convert.ToInt32(cmbBooks.SelectedValue);
            int studID = Convert.ToInt32(cmbStudents.SelectedValue);

            // Check already issued to this student
            string checkQ = @"SELECT COUNT(*) FROM IssuedBooks WHERE BookID=@b AND StudentID=@s AND Status='Issued'";
            int already = Convert.ToInt32(DatabaseHelper.ExecuteScalar(checkQ,
                new[] { new SqlParameter("@b", bookID), new SqlParameter("@s", studID) }));
            if (already > 0)
            {
                MessageBox.Show("This student already has this book issued.", "Warning",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            DatabaseHelper.ExecuteNonQuery("EXEC sp_IssueBook @BookID, @StudentID, @DueDate",
                new[] {
                    new SqlParameter("@BookID", bookID),
                    new SqlParameter("@StudentID", studID),
                    new SqlParameter("@DueDate", dtpDueDate.Value.Date)
                });

            MessageBox.Show("Book issued successfully!", "Success",
                MessageBoxButtons.OK, MessageBoxIcon.Information);
            LoadCombos();
        }

        private void BtnReturn_Click(object sender, EventArgs e)
        {
            if (dgvIssued.SelectedRows.Count == 0)
            {
                MessageBox.Show("Select an issued book to return.", "Validation",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            int issueID = Convert.ToInt32(dgvIssued.SelectedRows[0].Cells["IssueID"].Value);
            DatabaseHelper.ExecuteNonQuery("EXEC sp_ReturnBook @IssueID",
                new[] { new SqlParameter("@IssueID", issueID) });

            MessageBox.Show("Book returned successfully!", "Success",
                MessageBoxButtons.OK, MessageBoxIcon.Information);
            LoadIssuedBooks();
            LoadCombos();
        }

        private Label MkLbl(string t, int x, int y, int w) =>
            new Label { Text = t, Location = new Point(x, y), Size = new Size(w, 25),
                Font = new Font("Segoe UI", 10F) };

        private Button MakeBtn(string text, int x, int y, int w, Color bg)
        {
            var b = new Button { Text = text, Location = new Point(x, y), Size = new Size(w, 38),
                BackColor = bg, ForeColor = Color.White, FlatStyle = FlatStyle.Flat,
                Font = new Font("Segoe UI", 10F, FontStyle.Bold), Cursor = Cursors.Hand };
            b.FlatAppearance.BorderSize = 0;
            return b;
        }
    }
}
