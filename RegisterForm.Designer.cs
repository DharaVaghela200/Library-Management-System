// ============================================
// File: RegisterForm.Designer.cs
// ============================================
namespace LibraryManagementSystem
{
    partial class RegisterForm
    {
        private System.ComponentModel.IContainer components = null;
        protected override void Dispose(bool disposing)
        {
            if (disposing && components != null) components.Dispose();
            base.Dispose(disposing);
        }

        private void InitializeComponent()
        {
            this.lblTitle = new System.Windows.Forms.Label();
            this.lblName = new System.Windows.Forms.Label();
            this.txtName = new System.Windows.Forms.TextBox();
            this.lblEmail = new System.Windows.Forms.Label();
            this.txtEmail = new System.Windows.Forms.TextBox();
            this.lblPhone = new System.Windows.Forms.Label();
            this.txtPhone = new System.Windows.Forms.TextBox();
            this.lblUsername = new System.Windows.Forms.Label();
            this.txtUsername = new System.Windows.Forms.TextBox();
            this.lblPassword = new System.Windows.Forms.Label();
            this.txtPassword = new System.Windows.Forms.TextBox();
            this.btnRegister = new System.Windows.Forms.Button();
            this.btnCancel = new System.Windows.Forms.Button();
            this.SuspendLayout();

            int lx = 40, tx = 200, w = 250, lw = 150, sy = 30;

            // lblTitle
            this.lblTitle.Font = new System.Drawing.Font("Segoe UI", 16F, System.Drawing.FontStyle.Bold);
            this.lblTitle.ForeColor = System.Drawing.Color.FromArgb(31, 73, 125);
            this.lblTitle.Location = new System.Drawing.Point(lx, 20);
            this.lblTitle.Size = new System.Drawing.Size(400, 35);
            this.lblTitle.Text = "Student Registration";

            SetLabel(this.lblName, "Full Name", lx, 80, lw);
            SetText(this.txtName, tx, 77, w);

            SetLabel(this.lblEmail, "Email", lx, 80 + sy + 25, lw);
            SetText(this.txtEmail, tx, 80 + sy + 22, w);

            SetLabel(this.lblPhone, "Phone", lx, 80 + (sy + 25) * 2, lw);
            SetText(this.txtPhone, tx, 80 + (sy + 25) * 2 - 3, w);

            SetLabel(this.lblUsername, "Username", lx, 80 + (sy + 25) * 3, lw);
            SetText(this.txtUsername, tx, 80 + (sy + 25) * 3 - 3, w);

            SetLabel(this.lblPassword, "Password", lx, 80 + (sy + 25) * 4, lw);
            SetText(this.txtPassword, tx, 80 + (sy + 25) * 4 - 3, w);
            this.txtPassword.PasswordChar = '●';

            // btnRegister
            this.btnRegister.BackColor = System.Drawing.Color.FromArgb(31, 73, 125);
            this.btnRegister.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnRegister.FlatAppearance.BorderSize = 0;
            this.btnRegister.Font = new System.Drawing.Font("Segoe UI", 11F, System.Drawing.FontStyle.Bold);
            this.btnRegister.ForeColor = System.Drawing.Color.White;
            this.btnRegister.Location = new System.Drawing.Point(lx, 330);
            this.btnRegister.Size = new System.Drawing.Size(180, 38);
            this.btnRegister.Text = "Register";
            this.btnRegister.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnRegister.Click += new System.EventHandler(this.btnRegister_Click);

            // btnCancel
            this.btnCancel.BackColor = System.Drawing.Color.Gray;
            this.btnCancel.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnCancel.FlatAppearance.BorderSize = 0;
            this.btnCancel.Font = new System.Drawing.Font("Segoe UI", 11F);
            this.btnCancel.ForeColor = System.Drawing.Color.White;
            this.btnCancel.Location = new System.Drawing.Point(240, 330);
            this.btnCancel.Size = new System.Drawing.Size(120, 38);
            this.btnCancel.Text = "Cancel";
            this.btnCancel.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnCancel.Click += new System.EventHandler(this.btnCancel_Click);

            this.Controls.AddRange(new System.Windows.Forms.Control[] {
                lblTitle, lblName, txtName, lblEmail, txtEmail,
                lblPhone, txtPhone, lblUsername, txtUsername,
                lblPassword, txtPassword, btnRegister, btnCancel
            });
            this.ClientSize = new System.Drawing.Size(480, 400);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Register New Student";
            this.BackColor = System.Drawing.Color.White;
            this.ResumeLayout(false);
        }

        private void SetLabel(System.Windows.Forms.Label lbl, string text, int x, int y, int w)
        {
            lbl.Font = new System.Drawing.Font("Segoe UI", 10F);
            lbl.Location = new System.Drawing.Point(x, y);
            lbl.Size = new System.Drawing.Size(w, 22);
            lbl.Text = text;
        }

        private void SetText(System.Windows.Forms.TextBox txt, int x, int y, int w)
        {
            txt.Font = new System.Drawing.Font("Segoe UI", 10F);
            txt.Location = new System.Drawing.Point(x, y);
            txt.Size = new System.Drawing.Size(w, 28);
            txt.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
        }

        private System.Windows.Forms.Label lblTitle, lblName, lblEmail, lblPhone, lblUsername, lblPassword;
        private System.Windows.Forms.TextBox txtName, txtEmail, txtPhone, txtUsername, txtPassword;
        private System.Windows.Forms.Button btnRegister, btnCancel;
    }
}
