using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Data.SqlClient;

namespace SaleApp
{
    public partial class frmCustomer : Form
    {
        string connectionString =
        "Server=DESKTOP-JSCD4Q8\\MASTERMOS;Database=ShoeStoreDB;Trusted_Connection=True;TrustServerCertificate=True;";

        public frmCustomer()
        {
            InitializeComponent();
            dgvCustomer.CellClick += dgvCustomer_CellContentClick;
        }

        private void frmCustomer_Load(object sender, EventArgs e)
        {
            dgvCustomer.AutoGenerateColumns = true;
            LoadCustomer();
            ClearFields();
        }
        // a) Load dữ liệu khách hàng
        private void LoadCustomer()
        {
            try
            {
                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    conn.Open();
                    string query = "SELECT CustomerID, FullName, Address, Phone FROM CUSTOMER ORDER BY CustomerID";
                    SqlDataAdapter da = new SqlDataAdapter(query, conn);
                    DataTable dt = new DataTable();
                    da.Fill(dt);
                    dgvCustomer.DataSource = dt;

                    // Đặt HeaderText
                    dgvCustomer.Columns["CustomerID"].HeaderText = "Customer ID";
                    dgvCustomer.Columns["FullName"].HeaderText = "Full Name";
                    dgvCustomer.Columns["Address"].HeaderText = "Address";
                    dgvCustomer.Columns["Phone"].HeaderText = "Phone Number";
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi tải dữ liệu khách hàng: " + ex.Message);
            }
        }

        // f) Clear Fields
        private void ClearFields()
        {
            txtCustomerID.Clear();
            txtFullName.Clear();
            txtAddress.Clear();
            txtPhoneNumber.Clear();
            txtCustomerID.Focus();
        }

        // b) Thêm khách hàng mới
        private void btnAdd_Click(object sender, EventArgs e)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(txtCustomerID.Text) || string.IsNullOrWhiteSpace(txtFullName.Text))
                {
                    MessageBox.Show("Please enter Customer ID and Full Name.", "Missing Data", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                string custID = txtCustomerID.Text.Trim();
                string fullName = txtFullName.Text.Trim();
                string address = string.IsNullOrWhiteSpace(txtAddress.Text) ? null : txtAddress.Text.Trim();
                string phone = string.IsNullOrWhiteSpace(txtPhoneNumber.Text) ? null : txtPhoneNumber.Text.Trim();

                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    conn.Open();

                    // Kiểm tra tồn tại
                    string checkQuery = "SELECT COUNT(1) FROM CUSTOMER WHERE CustomerID = @CustomerID";
                    using (SqlCommand cmdCheck = new SqlCommand(checkQuery, conn))
                    {
                        cmdCheck.Parameters.AddWithValue("@CustomerID", custID);
                        int count = Convert.ToInt32(cmdCheck.ExecuteScalar());
                        if (count > 0)
                        {
                            MessageBox.Show("Customer ID already exists!", "Duplicate ID", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                            return;
                        }
                    }

                    // Insert
                    string insertQuery = "INSERT INTO CUSTOMER (CustomerID, FullName, Address, Phone) VALUES (@CustomerID, @FullName, @Address, @Phone)";
                    using (SqlCommand cmd = new SqlCommand(insertQuery, conn))
                    {
                        cmd.Parameters.AddWithValue("@CustomerID", custID);
                        cmd.Parameters.AddWithValue("@FullName", fullName);
                        cmd.Parameters.AddWithValue("@Address", (object)address ?? DBNull.Value);
                        cmd.Parameters.AddWithValue("@Phone", (object)phone ?? DBNull.Value);

                        int rows = cmd.ExecuteNonQuery();
                        if (rows > 0)
                        {
                            MessageBox.Show("Customer added successfully.");
                            LoadCustomer();
                            ClearFields();
                        }
                        else
                        {
                            MessageBox.Show("No rows inserted.");
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error adding customer: " + ex.Message);
            }
        }

        // c) Cập nhật khách hàng
        private void btnUpdate_Click(object sender, EventArgs e)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(txtCustomerID.Text))
                {
                    MessageBox.Show("Please enter Customer ID to update.", "Missing Data", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                string custID = txtCustomerID.Text.Trim();
                string fullName = txtFullName.Text.Trim();
                string address = string.IsNullOrWhiteSpace(txtAddress.Text) ? null : txtAddress.Text.Trim();
                string phone = string.IsNullOrWhiteSpace(txtPhoneNumber.Text) ? null : txtPhoneNumber.Text.Trim();

                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    conn.Open();

                    string checkQuery = "SELECT COUNT(1) FROM CUSTOMER WHERE CustomerID = @CustomerID";
                    using (SqlCommand cmdCheck = new SqlCommand(checkQuery, conn))
                    {
                        cmdCheck.Parameters.AddWithValue("@CustomerID", custID);
                        int count = Convert.ToInt32(cmdCheck.ExecuteScalar());
                        if (count == 0)
                        {
                            MessageBox.Show("Customer ID does not exist!", "Not Found", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                            return;
                        }
                    }

                    string updateQuery = "UPDATE CUSTOMER SET FullName=@FullName, Address=@Address, Phone=@Phone WHERE CustomerID=@CustomerID";
                    using (SqlCommand cmd = new SqlCommand(updateQuery, conn))
                    {
                        cmd.Parameters.AddWithValue("@CustomerID", custID);
                        cmd.Parameters.AddWithValue("@FullName", fullName);
                        cmd.Parameters.AddWithValue("@Address", (object)address ?? DBNull.Value);
                        cmd.Parameters.AddWithValue("@Phone", (object)phone ?? DBNull.Value);

                        int rows = cmd.ExecuteNonQuery();
                        if (rows > 0)
                        {
                            MessageBox.Show("Customer updated successfully.");
                            LoadCustomer();
                        }
                        else
                        {
                            MessageBox.Show("No rows updated.");
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error updating customer: " + ex.Message);
            }
        }

        // d) Xóa khách hàng
        private void btnDelete_Click(object sender, EventArgs e)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(txtCustomerID.Text))
                {
                    MessageBox.Show("Please enter Customer ID to delete.", "Missing Data", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                string custID = txtCustomerID.Text.Trim();

                DialogResult result = MessageBox.Show($"Are you sure you want to delete customer {custID}?", "Confirm Delete", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                if (result != DialogResult.Yes) return;

                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    conn.Open();

                    string checkQuery = "SELECT COUNT(1) FROM CUSTOMER WHERE CustomerID=@CustomerID";
                    using (SqlCommand cmdCheck = new SqlCommand(checkQuery, conn))
                    {
                        cmdCheck.Parameters.AddWithValue("@CustomerID", custID);
                        int count = Convert.ToInt32(cmdCheck.ExecuteScalar());
                        if (count == 0)
                        {
                            MessageBox.Show("Customer ID does not exist!", "Not Found", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                            return;
                        }
                    }

                    string deleteQuery = "DELETE FROM CUSTOMER WHERE CustomerID=@CustomerID";
                    using (SqlCommand cmd = new SqlCommand(deleteQuery, conn))
                    {
                        cmd.Parameters.AddWithValue("@CustomerID", custID);
                        int rows = cmd.ExecuteNonQuery();
                        if (rows > 0)
                        {
                            MessageBox.Show("Customer deleted successfully.");
                            LoadCustomer();
                            ClearFields();
                        }
                        else
                        {
                            MessageBox.Show("No rows deleted.");
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error deleting customer: " + ex.Message);
            }
        }

        // e) Tìm kiếm khách hàng
        private void btnSearch_Click(object sender, EventArgs e)
        {
            try
            {
                string custID = txtCustomerID.Text.Trim();
                string fullName = txtFullName.Text.Trim();

                if (string.IsNullOrWhiteSpace(custID) && string.IsNullOrWhiteSpace(fullName))
                {
                    MessageBox.Show("Enter Customer ID or Full Name to search.", "Missing Data", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    LoadCustomer();
                    return;
                }

                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    conn.Open();
                    string query = @"SELECT CustomerID, FullName, Address, Phone
                                     FROM CUSTOMER
                                     WHERE CustomerID LIKE @CustomerID OR FullName LIKE @FullName
                                     ORDER BY CustomerID";
                    using (SqlDataAdapter da = new SqlDataAdapter(query, conn))
                    {
                        da.SelectCommand.Parameters.AddWithValue("@CustomerID", "%" + custID + "%");
                        da.SelectCommand.Parameters.AddWithValue("@FullName", "%" + fullName + "%");
                        DataTable dt = new DataTable();
                        da.Fill(dt);
                        dgvCustomer.DataSource = dt;
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error searching customer: " + ex.Message);
            }
        }

        // g) Chọn khách hàng từ DataGridView
        private void dgvCustomer_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            try
            {
                if (e.RowIndex < 0) return;
                DataGridViewRow row = dgvCustomer.Rows[e.RowIndex];

                txtCustomerID.Text = row.Cells["CustomerID"].Value?.ToString() ?? "";
                txtFullName.Text = row.Cells["FullName"].Value?.ToString() ?? "";
                txtAddress.Text = row.Cells["Address"].Value?.ToString() ?? "";
                txtPhoneNumber.Text = row.Cells["Phone"].Value?.ToString() ?? "";
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error selecting row: " + ex.Message);
            }
        }

        // Close button
        private void btnClose_Click(object sender, EventArgs e)
        {
            Form mainForm = Application.OpenForms["MainForm"];
            if (mainForm != null)
            {
                this.Hide();
                mainForm.Show();
            }
            else
            {
                this.Close();
            }
        }

        private void btnClose_Click_1(object sender, EventArgs e)
        {
            try
            {
                // Hiển thị hộp thoại xác nhận
                DialogResult result = MessageBox.Show(
                    "Are you sure you want to exit?",  // Nội dung thông báo
                    "Confirm Exit",                    // Tiêu đề hộp thoại
                    MessageBoxButtons.YesNo,           // Hai nút Yes và No
                    MessageBoxIcon.Question            // Biểu tượng dấu hỏi
                );

                if (result == DialogResult.Yes)
                {
                    // Lấy MainForm đang mở
                    Form mainForm = Application.OpenForms["MainForm"];
                    if (mainForm != null)
                    {
                        // Hiển thị lại MainForm
                        mainForm.Show();
                    }

                    // Đóng frmCustomer
                    this.Close();
                }
                // Nếu người dùng chọn No -> không làm gì
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error closing form: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        protected override void OnPaint(PaintEventArgs e)
        {
            using (var brush = new System.Drawing.Drawing2D.LinearGradientBrush(
    this.ClientRectangle, Color.MistyRose, Color.LavenderBlush, 90F))
            {
                e.Graphics.FillRectangle(brush, this.ClientRectangle);
            }
        }
    }
}
