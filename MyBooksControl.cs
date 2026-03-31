// ============================================
// File: MyBooksControl.cs  (UserControl - Student Only)
// Shows the currently logged-in student's issued books
// ============================================
using System;
using System.Data.SqlClient;
using System.Drawing;
using System.Windows.Forms;

namespace LibraryManagementSystem
{
    public class MyBooksControl : UserControl
    {
        private TabControl tabControl;

        public MyBooksControl()
        {
            BuildUI();
        }

        private void BuildUI()
        {
            this.BackColor = Color.FromArgb(245, 245, 250);

            var lbl = new Label {
                Text = "My Library Records", Location = new Point(15, 15),
                Size = new Size(400, 30), Font = new Font("Segoe UI", 14F, FontStyle.Bold),
                ForeColor = Color.FromArgb(31, 73, 125)
            };

            tabControl = new TabControl {
                Location = new Point(15, 55), Size = new Size(790, 510),
                Font = new Font("Segoe UI", 10F)
            };

            // ── Tab 1: My Currently Issued Books ──
            TabPage tabIssued = new TabPage("  My Issued Books  ");
            tabIssued.BackColor = Color.White;

            var dgvIssued = BuildGrid();
            dgvIssued.Location = new Point(15, 15);
            dgvIssued.Size = new Size(750, 410);
            tabIssued.Controls.Add(dgvIssued);

            string qIssued = @"SELECT b.Title AS [Book Title], b.Author,
                                       ib.IssueDate, ib.DueDate,
                                       CASE WHEN ib.DueDate < GETDATE()
                                            THEN 'Overdue' ELSE 'Active' END AS [Status]
                                FROM IssuedBooks ib
                                JOIN Books b ON ib.BookID = b.BookID
                                WHERE ib.StudentID = @sid AND ib.Status = 'Issued'
                                ORDER BY ib.IssueDate DESC";
            dgvIssued.DataSource = DatabaseHelper.ExecuteQuery(qIssued,
                new[] { new SqlParameter("@sid", SessionHelper.StudentID) });

            dgvIssued.CellFormatting += (s, e) => {
                if (e.RowIndex < 0 || e.ColumnIndex < 0) return;
                if (dgvIssued.Columns[e.ColumnIndex].Name == "Status" &&
                    e.Value?.ToString() == "Overdue")
                {
                    e.CellStyle.ForeColor = Color.Red;
                    e.CellStyle.Font = new Font("Segoe UI", 9.5F, FontStyle.Bold);
                }
            };

            // ── Tab 2: Browse Available Books ──
            TabPage tabAvail = new TabPage("  Browse Books  ");
            tabAvail.BackColor = Color.White;

            var txtSrch = new TextBox {
                Location = new Point(15, 15), Size = new Size(280, 28),
                Font = new Font("Segoe UI", 10F)
            };
            SetWatermark(txtSrch, "Search by title or author...");
            tabAvail.Controls.Add(txtSrch);

            var btnSrch = MakeBtn("Search", 305, 13, 100, Color.FromArgb(31, 73, 125));
            tabAvail.Controls.Add(btnSrch);

            var dgvAvail = BuildGrid();
            dgvAvail.Location = new Point(15, 50);
            dgvAvail.Size = new Size(750, 380);
            tabAvail.Controls.Add(dgvAvail);

            Action<string> loadAvail = (search) => {
                string q = @"SELECT Title, Author, Category, Publisher,
                                    AvailableCopies AS [Copies Available]
                             FROM Books
                             WHERE AvailableCopies > 0
                               AND (Title LIKE @s OR Author LIKE @s)
                             ORDER BY Title";
                dgvAvail.DataSource = DatabaseHelper.ExecuteQuery(q,
                    new[] { new SqlParameter("@s", "%" + search + "%") });
            };
            loadAvail("");
            btnSrch.Click += (s, e) => {
                string search = txtSrch.ForeColor == Color.Gray ? "" : txtSrch.Text.Trim();
                loadAvail(search);
            };

            // ── Tab 3: Borrowing History ──
            TabPage tabHistory = new TabPage("  My History  ");
            tabHistory.BackColor = Color.White;

            var dgvHist = BuildGrid();
            dgvHist.Location = new Point(15, 15);
            dgvHist.Size = new Size(750, 430);
            tabHistory.Controls.Add(dgvHist);

            string qHist = @"SELECT b.Title AS [Book Title], b.Author,
                                     ib.IssueDate, ib.DueDate, ib.ReturnDate, ib.Status
                              FROM IssuedBooks ib
                              JOIN Books b ON ib.BookID = b.BookID
                              WHERE ib.StudentID = @sid
                              ORDER BY ib.IssueDate DESC";
            dgvHist.DataSource = DatabaseHelper.ExecuteQuery(qHist,
                new[] { new SqlParameter("@sid", SessionHelper.StudentID) });

            tabControl.TabPages.Add(tabIssued);
            tabControl.TabPages.Add(tabAvail);
            tabControl.TabPages.Add(tabHistory);

            this.Controls.AddRange(new Control[] { lbl, tabControl });
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

        private DataGridView BuildGrid()
        {
            var g = new DataGridView {
                AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill,
                SelectionMode = DataGridViewSelectionMode.FullRowSelect,
                ReadOnly = true, AllowUserToAddRows = false, RowHeadersVisible = false,
                BackgroundColor = Color.White, Font = new Font("Segoe UI", 9.5F)
            };
            g.DefaultCellStyle.SelectionBackColor = Color.FromArgb(31, 73, 125);
            return g;
        }

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
