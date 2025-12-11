using System;
using System.Data.SqlClient;
using System.Drawing;
using System.Windows.Forms;

namespace SaleApp
{
    public partial class frmLogin : Form
    {
        string connectionString =
       "Server=DESKTOP-JSCD4Q8\\MASTERMOS;Database=ShoeStoreDB;Trusted_Connection=True;TrustServerCertificate=True;";
        public frmLogin()
        {
            InitializeComponent();
        }

        private void btnLogin_Click(object sender, EventArgs e)
        {
            string username = txtUser.Text.Trim();
            string password = txtPass.Text.Trim();

            if (string.IsNullOrEmpty(username) || string.IsNullOrEmpty(password))
            {
                MessageBox.Show("Please enter Username and Password!");
                return;
            }

            try
            {
                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    conn.Open();

                    string query =
                    "SELECT a.Username, (e.LastName + ' ' + e.FirstName) AS FullName, a.RoleName " +
                    "FROM ACCOUNT a " +
                    "JOIN EMPLOYEE e ON a.EmployeeID = e.EmployeeID " +
                    "WHERE a.Username = @user AND a.PasswordHash = @pass AND a.IsActive = 1";

                    SqlCommand cmd = new SqlCommand(query, conn);
                    cmd.Parameters.AddWithValue("@user", username);
                    cmd.Parameters.AddWithValue("@pass", password);

                    SqlDataReader reader = cmd.ExecuteReader();

                    if (reader.Read())
                    {
                        string fullName = reader["FullName"].ToString();
                        string role = reader["RoleName"].ToString(); // Admin, Sales, Warehouse

                        MessageBox.Show($"Welcome {fullName} ({role})!", "Login Successful");

                        MainForm main = new MainForm(fullName, role);
                        main.Show();
                        this.Hide();
                    }
                    else
                    {
                        MessageBox.Show("Invalid username or password!", "Login Failed");
                    }

                    reader.Close();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error: " + ex.Message);
            }
        }
        // Color 
        protected override void OnPaint(PaintEventArgs e)
        {
            using (System.Drawing.Drawing2D.LinearGradientBrush brush =
                   new System.Drawing.Drawing2D.LinearGradientBrush(this.ClientRectangle, Color.LightSkyBlue, Color.White, 90F))
            {
                e.Graphics.FillRectangle(brush, this.ClientRectangle); 
            }
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            txtUser.Clear();
            txtPass.Clear();
            txtUser.Focus();
        }
    }
}




