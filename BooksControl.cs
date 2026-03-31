// ============================================
// File: BooksControl.cs  (UserControl)
// Manages: View / Add / Edit / Delete Books
// ============================================
using System;
using System.Data.SqlClient;
using System.Drawing;
using System.Windows.Forms;

namespace LibraryManagementSystem
{
    public class BooksControl : UserControl
    {
        private DataGridView dgvBooks;
        private TextBox txtSearch, txtTitle, txtAuthor, txtISBN, txtCategory, txtPublisher, txtCopies;
        private Button btnSearch, btnAdd, btnUpdate, btnDelete, btnClear;
        private Label lblTitle2;
        private Panel panelForm;
        private int selectedBookID = -1;

        public BooksControl()
        {
            BuildUI();
            LoadBooks();
        }

        private void BuildUI()
        {
            this.BackColor = Color.FromArgb(245, 245, 250);

            lblTitle2 = MakeLabel("Book Management", 15, 15, 400, 30,
                new Font("Segoe UI", 14F, FontStyle.Bold), Color.FromArgb(31, 73, 125));

            // Search
            txtSearch = MakeTxt(15, 55, 300);
            SetWatermark(txtSearch, "Search by title or author...");

            btnSearch = MakeBtn("Search", 325, 53, 100, Color.FromArgb(31, 73, 125));
            btnSearch.Click += (s, e) => {
                string search = txtSearch.ForeColor == Color.Gray ? "" : txtSearch.Text.Trim();
                LoadBooks(search);
            };

            // Grid
            dgvBooks = new DataGridView {
                Location = new Point(15, 90), Size = new Size(790, 260),
                BackgroundColor = Color.White,
                AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill,
                SelectionMode = DataGridViewSelectionMode.FullRowSelect,
                ReadOnly = true, AllowUserToAddRows = false, RowHeadersVisible = false,
                Font = new Font("Segoe UI", 9.5F)
            };
            dgvBooks.DefaultCellStyle.SelectionBackColor = Color.FromArgb(31, 73, 125);
            dgvBooks.CellClick += DgvBooks_CellClick;

            // Form panel
            panelForm = new Panel {
                BackColor = Color.White, Location = new Point(15, 360), Size = new Size(790, 210)
            };

            int col1x = 20, col2x = 300, col3x = 560, ly = 15, lw = 120, tw = 160;

            panelForm.Controls.Add(MakeLabel("Title *",    col1x, ly, lw, 22));
            txtTitle = MakeTxt(col1x + lw, ly, tw);
            panelForm.Controls.Add(txtTitle);

            panelForm.Controls.Add(MakeLabel("Author *",   col2x, ly, lw, 22));
            txtAuthor = MakeTxt(col2x + lw, ly, tw);
            panelForm.Controls.Add(txtAuthor);

            panelForm.Controls.Add(MakeLabel("ISBN",       col3x, ly, 50, 22));
            txtISBN = MakeTxt(col3x + 55, ly, 155);
            panelForm.Controls.Add(txtISBN);

            int ly2 = ly + 50;
            panelForm.Controls.Add(MakeLabel("Category",   col1x, ly2, lw, 22));
            txtCategory = MakeTxt(col1x + lw, ly2, tw);
            panelForm.Controls.Add(txtCategory);

            panelForm.Controls.Add(MakeLabel("Publisher",  col2x, ly2, lw, 22));
            txtPublisher = MakeTxt(col2x + lw, ly2, tw);
            panelForm.Controls.Add(txtPublisher);

            panelForm.Controls.Add(MakeLabel("Total Copies", col3x, ly2, 85, 22));
            txtCopies = MakeTxt(col3x + 90, ly2, 120);
            txtCopies.Text = "1";
            panelForm.Controls.Add(txtCopies);

            int btnY = ly2 + 55;
            int btnX = col1x;

            if (SessionHelper.IsAdmin)
            {
                btnAdd = MakeBtn("Add Book", btnX, btnY, 105, Color.FromArgb(0, 128, 0));
                btnAdd.Click += BtnAdd_Click;
                panelForm.Controls.Add(btnAdd);
                btnX += 115;

                btnUpdate = MakeBtn("Update", btnX, btnY, 100, Color.FromArgb(31, 73, 125));
                btnUpdate.Click += BtnUpdate_Click;
                panelForm.Controls.Add(btnUpdate);
                btnX += 110;

                btnDelete = MakeBtn("Delete", btnX, btnY, 100, Color.FromArgb(192, 0, 0));
                btnDelete.Click += BtnDelete_Click;
                panelForm.Controls.Add(btnDelete);
                btnX += 110;
            }

            btnClear = MakeBtn("Clear", btnX, btnY, 90, Color.Gray);
            btnClear.Click += (s, e) => ClearForm();
            panelForm.Controls.Add(btnClear);

            this.Controls.AddRange(new Control[] { lblTitle2, txtSearch, btnSearch, dgvBooks, panelForm });
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

        private void LoadBooks(string search = "")
        {
            string q = @"SELECT BookID, Title, Author, ISBN, Category, Publisher,
                                 TotalCopies, AvailableCopies
                         FROM Books
                         WHERE Title LIKE @s OR Author LIKE @s
                         ORDER BY Title";
            dgvBooks.DataSource = DatabaseHelper.ExecuteQuery(q,
                new[] { new SqlParameter("@s", "%" + search + "%") });
            if (dgvBooks.Columns.Contains("BookID"))
                dgvBooks.Columns["BookID"].Visible = false;
        }

        private void DgvBooks_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex < 0) return;
            var row = dgvBooks.Rows[e.RowIndex];
            selectedBookID    = Convert.ToInt32(row.Cells["BookID"].Value);
            txtTitle.Text     = row.Cells["Title"].Value?.ToString() ?? "";
            txtAuthor.Text    = row.Cells["Author"].Value?.ToString() ?? "";
            txtISBN.Text      = row.Cells["ISBN"].Value?.ToString() ?? "";
            txtCategory.Text  = row.Cells["Category"].Value?.ToString() ?? "";
            txtPublisher.Text = row.Cells["Publisher"].Value?.ToString() ?? "";
            txtCopies.Text    = row.Cells["TotalCopies"].Value?.ToString() ?? "1";
            // Restore black color when filled from grid
            txtTitle.ForeColor = txtAuthor.ForeColor = txtISBN.ForeColor =
            txtCategory.ForeColor = txtPublisher.ForeColor = txtCopies.ForeColor = Color.Black;
        }

        private void BtnAdd_Click(object sender, EventArgs e)
        {
            if (!ValidateForm()) return;
            string q = @"INSERT INTO Books (Title,Author,ISBN,Category,Publisher,TotalCopies,AvailableCopies)
                         VALUES (@t,@a,@i,@c,@pub,@tot,@tot)";
            DatabaseHelper.ExecuteNonQuery(q, new[] {
                new SqlParameter("@t",   txtTitle.Text.Trim()),
                new SqlParameter("@a",   txtAuthor.Text.Trim()),
                new SqlParameter("@i",   txtISBN.Text.Trim()),
                new SqlParameter("@c",   txtCategory.Text.Trim()),
                new SqlParameter("@pub", txtPublisher.Text.Trim()),
                new SqlParameter("@tot", int.Parse(txtCopies.Text.Trim()))
            });
            MessageBox.Show("Book added successfully!", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
            ClearForm(); LoadBooks();
        }

        private void BtnUpdate_Click(object sender, EventArgs e)
        {
            if (selectedBookID < 0) { MessageBox.Show("Select a book first."); return; }
            if (!ValidateForm()) return;
            string q = @"UPDATE Books SET Title=@t, Author=@a, ISBN=@i, Category=@c,
                                Publisher=@pub, TotalCopies=@tot WHERE BookID=@id";
            DatabaseHelper.ExecuteNonQuery(q, new[] {
                new SqlParameter("@t",   txtTitle.Text.Trim()),
                new SqlParameter("@a",   txtAuthor.Text.Trim()),
                new SqlParameter("@i",   txtISBN.Text.Trim()),
                new SqlParameter("@c",   txtCategory.Text.Trim()),
                new SqlParameter("@pub", txtPublisher.Text.Trim()),
                new SqlParameter("@tot", int.Parse(txtCopies.Text.Trim())),
                new SqlParameter("@id",  selectedBookID)
            });
            MessageBox.Show("Book updated!", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
            ClearForm(); LoadBooks();
        }

        private void BtnDelete_Click(object sender, EventArgs e)
        {
            if (selectedBookID < 0) { MessageBox.Show("Select a book first."); return; }
            if (MessageBox.Show("Delete this book?", "Confirm",
                MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
            {
                DatabaseHelper.ExecuteNonQuery("DELETE FROM Books WHERE BookID=@id",
                    new[] { new SqlParameter("@id", selectedBookID) });
                MessageBox.Show("Book deleted.", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                ClearForm(); LoadBooks();
            }
        }

        private bool ValidateForm()
        {
            if (string.IsNullOrWhiteSpace(txtTitle.Text) || string.IsNullOrWhiteSpace(txtAuthor.Text))
            {
                MessageBox.Show("Title and Author are required.", "Validation",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return false;
            }
            if (!int.TryParse(txtCopies.Text, out _))
            {
                MessageBox.Show("Copies must be a number.", "Validation",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return false;
            }
            return true;
        }

        private void ClearForm()
        {
            selectedBookID = -1;
            txtTitle.Text = txtAuthor.Text = txtISBN.Text =
            txtCategory.Text = txtPublisher.Text = "";
            txtCopies.Text = "1";
        }

        private Label MakeLabel(string text, int x, int y, int w, int h,
            Font font = null, Color? color = null)
        {
            return new Label {
                Text = text, Location = new Point(x, y), Size = new Size(w, h),
                Font = font ?? new Font("Segoe UI", 10F),
                ForeColor = color ?? Color.Black
            };
        }

        private TextBox MakeTxt(int x, int y, int w)
        {
            return new TextBox {
                Location = new Point(x, y), Size = new Size(w, 28),
                Font = new Font("Segoe UI", 10F), BorderStyle = BorderStyle.FixedSingle
            };
        }

        private Button MakeBtn(string text, int x, int y, int w, Color bg)
        {
            var b = new Button {
                Text = text, Location = new Point(x, y), Size = new Size(w, 34),
                BackColor = bg, ForeColor = Color.White,
                FlatStyle = FlatStyle.Flat, Font = new Font("Segoe UI", 9.5F),
                Cursor = Cursors.Hand
            };
            b.FlatAppearance.BorderSize = 0;
            return b;
        }
    }
}
