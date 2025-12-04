using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace SaleApp
{
     public partial class frmStaff : Form
    {
        string connectionString =
    "Server=DESKTOP-JSCD4Q8\\MASTERMOS;Database=ShoeStoreDB;Trusted_Connection=True;TrustServerCertificate=True;";
        // **BỔ SUNG:** Biến thành viên để lưu trữ dữ liệu ảnh (Byte Array)
        private byte[] _employeePhotoBytes = null;

        public frmStaff()
        {
            InitializeComponent();
            dgvStaff.CellClick += dgvStaff_CellClick; // gán sự kiện CellClick
        }
        private void frmStaff_Load(object sender, EventArgs e)
        {
            // đảm bảo DataGridView sẽ tự sinh cột nếu bạn chưa cấu hình
            dgvStaff.AutoGenerateColumns = true; 

            LoadStaff();
            ClearFields();
        }

        private void LoadStaff()
        {
            try
            {
                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    conn.Open();

                    string query = @"
                SELECT EmployeeID, LastName, FirstName, Gender, BirthDate,
                       Address, PhoneNumber, PhotoData
                FROM EMPLOYEE
                ORDER BY EmployeeID";

                    SqlDataAdapter da = new SqlDataAdapter(query, conn);
                    DataTable dt = new DataTable();
                    da.Fill(dt);

                    // Bổ sung cột GenderText
                    dt.Columns.Add("GenderText", typeof(string));
                    foreach (DataRow row in dt.Rows)
                    {
                        bool male = row["Gender"] != DBNull.Value && (bool)row["Gender"];
                        row["GenderText"] = male ? "Male" : "Female";
                    }

                    dgvStaff.DataSource = dt;

                    // Đặt lại HeaderText cho từng cột
                    dgvStaff.Columns["EmployeeID"].HeaderText = "Employee ID";
                    dgvStaff.Columns["LastName"].HeaderText = "Last Name";
                    dgvStaff.Columns["FirstName"].HeaderText = "First Name";
                    dgvStaff.Columns["GenderText"].HeaderText = "Gender";
                    dgvStaff.Columns["BirthDate"].HeaderText = "Birth Date";
                    dgvStaff.Columns["Address"].HeaderText = "Address";
                    dgvStaff.Columns["PhoneNumber"].HeaderText = "Phone Number";
                    dgvStaff.Columns["PhotoData"].HeaderText = "Photo";

                    // Ẩn cột Gender vì đã dùng GenderText
                    if (dgvStaff.Columns.Contains("Gender"))
                        dgvStaff.Columns["Gender"].Visible = false;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi tải dữ liệu nhân viên: " + ex.Message,
                                "Database Error",
                                MessageBoxButtons.OK,
                                MessageBoxIcon.Error);
            }
        }

        private void ClearFields()
        {
            txtEmployeeID.Clear();
            txtLastName.Clear();
            txtFirstName.Clear();
            txtAddress.Clear();
            txtPhoneNumber.Clear();
            rdoMale.Checked = true;
            rdoFemale.Checked = false;
            dtpDateofBirth.Value = DateTime.Now;

            // Dispose ảnh hiện tại nếu có để tránh leak
            if (picImage.Image != null)
            {
                picImage.Image.Dispose();
                picImage.Image = null;
            }

            // Reset dữ liệu ảnh (byte array)
            _employeePhotoBytes = null;

            txtEmployeeID.Focus();
        }

        private void btnDelete_Click(object sender, EventArgs e)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(txtEmployeeID.Text))
                {
                    MessageBox.Show("Please enter Employee ID to delete.", "Missing Data", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                string empID = txtEmployeeID.Text.Trim();

                DialogResult result = MessageBox.Show(
                    $"Are you sure you want to delete employee {empID}?",
                    "Confirm Delete",
                    MessageBoxButtons.YesNo,
                    MessageBoxIcon.Question);

                if (result != DialogResult.Yes) return;

                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    conn.Open();

                    const string checkQuery = "SELECT COUNT(1) FROM EMPLOYEE WHERE EmployeeID = @EmployeeID";
                    using (SqlCommand cmdCheck = new SqlCommand(checkQuery, conn))
                    {
                        cmdCheck.Parameters.AddWithValue("@EmployeeID", empID);
                        int count = Convert.ToInt32(cmdCheck.ExecuteScalar());
                        if (count == 0)
                        {
                            MessageBox.Show("Employee ID does not exist!", "Not Found", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                            return;
                        }
                    }

                    const string deleteQuery = "DELETE FROM EMPLOYEE WHERE EmployeeID = @EmployeeID";
                    using (SqlCommand cmd = new SqlCommand(deleteQuery, conn))
                    {
                        cmd.Parameters.AddWithValue("@EmployeeID", empID);
                        int rows = cmd.ExecuteNonQuery();
                        if (rows > 0)
                        {
                            MessageBox.Show("Employee deleted successfully.", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                            LoadStaff();
                            ClearFields();
                        }
                        else
                        {
                            MessageBox.Show("No rows deleted.", "Info", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error deleting employee: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        
        private void btnSearch_Click(object sender, EventArgs e)
        {
            try
            {
                string keyword = txtEmployeeID.Text.Trim();
                string lname = txtLastName.Text.Trim();

                if (string.IsNullOrWhiteSpace(keyword) && string.IsNullOrWhiteSpace(lname))
                {
                    MessageBox.Show("Please enter Employee ID or Last Name to search.", "Missing Search Criteria", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    LoadStaff();
                    return;
                }

                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    conn.Open();

                    string query = @"
                SELECT EmployeeID, LastName, FirstName, Gender, BirthDate, Address, PhoneNumber, PhotoData
                FROM EMPLOYEE
                WHERE EmployeeID LIKE @Keyword OR LastName LIKE @LastName
                ORDER BY EmployeeID";

                    using (SqlDataAdapter da = new SqlDataAdapter(query, conn))
                    {
                        da.SelectCommand.Parameters.AddWithValue("@Keyword", "%" + keyword + "%");
                        da.SelectCommand.Parameters.AddWithValue("@LastName", "%" + lname + "%");

                        DataTable dt = new DataTable();
                        da.Fill(dt);

                        if (!dt.Columns.Contains("GenderText"))
                            dt.Columns.Add("GenderText", typeof(string));

                        foreach (DataRow row in dt.Rows)
                        {
                            bool isMale = row["Gender"] != DBNull.Value && (bool)row["Gender"];
                            row["GenderText"] = isMale ? "Male" : "Female";
                        }

                        dgvStaff.DataSource = dt;

                        if (dgvStaff.Columns.Contains("Gender"))
                            dgvStaff.Columns["Gender"].Visible = false;
                        if (dgvStaff.Columns.Contains("GenderText"))
                            dgvStaff.Columns["GenderText"].HeaderText = "Gender";
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error searching employee: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnChoosePicture_Click(object sender, EventArgs e)
        {
            using (OpenFileDialog ofd = new OpenFileDialog()
            {
                Title = "Select Employee Image",
                Filter = "Image Files|*.jpg;*.jpeg;*.png;*.bmp;*.gif"
            })
            {
                if (ofd.ShowDialog() == DialogResult.OK)
                {
                    try
                    {
                        // Đọc file vào byte[] trước để tránh bị lock file khi dùng Image.FromFile
                        byte[] imgBytes = File.ReadAllBytes(ofd.FileName);

                        // Dispose ảnh cũ nếu có
                        if (picImage.Image != null)
                        {
                            picImage.Image.Dispose();
                            picImage.Image = null;
                        }

                        // Gán ảnh bằng MemoryStream (không khóa file)
                        using (var ms = new MemoryStream(imgBytes))
                        {
                            picImage.Image = Image.FromStream(ms);
                        }

                        // Lưu đường dẫn gốc vào Tag (nếu cần lưu file vật lý sau này)
                        picImage.Tag = ofd.FileName;

                        // Lưu byte[] để commit vào DB
                        _employeePhotoBytes = imgBytes;
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show("Error loading image: " + ex.Message,
                                        "Image Error",
                                        MessageBoxButtons.OK,
                                        MessageBoxIcon.Error);
                    }
                }
            }
        }
        private void btnClose_Click(object sender, EventArgs e)
        {
            try
            {
                // Hiển thị thông báo xác nhận
                DialogResult result = MessageBox.Show(
                    "Are you sure you want to exit?",  // Nội dung thông báo
                    "Confirm Exit",                    // Tiêu đề hộp thoại
                    MessageBoxButtons.YesNo,           // Hai nút Yes và No
                    MessageBoxIcon.Question            // Biểu tượng dấu hỏi
                );

                if (result == DialogResult.Yes)
                {
                    // Kiểm tra xem MainForm có đang mở không
                    Form mainForm = Application.OpenForms["MainForm"];
                    if (mainForm != null)
                    {
                        this.Hide();      // Ẩn form hiện tại
                        mainForm.Show();  // Hiển thị MainForm
                    }
                    else
                    {
                        // Đóng hẳn ứng dụng nếu không tìm thấy MainForm
                        this.Close();
                    }
                }
                // Nếu người dùng chọn No -> không làm gì
            }
            catch (Exception ex)
            {
                MessageBox.Show(
                    "Error closing form: " + ex.Message,
                    "Error",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error
                );
            }
        }

        private void dgvStaff_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            try
            {
                if (e.RowIndex < 0) return;
                DataGridViewRow row = dgvStaff.Rows[e.RowIndex];

                // Basic fields (safe null-guards)
                txtEmployeeID.Text = row.Cells["EmployeeID"].Value?.ToString() ?? "";
                txtLastName.Text = row.Cells["LastName"].Value?.ToString() ?? "";
                txtFirstName.Text = row.Cells["FirstName"].Value?.ToString() ?? "";

                // Gender safe
                bool gender = false;
                if (row.Cells["Gender"].Value != DBNull.Value && row.Cells["Gender"].Value != null)
                    gender = Convert.ToBoolean(row.Cells["Gender"].Value);
                rdoMale.Checked = gender;
                rdoFemale.Checked = !gender;

                // BirthDate safe
                if (row.Cells["BirthDate"].Value != DBNull.Value && row.Cells["BirthDate"].Value != null)
                    dtpDateofBirth.Value = Convert.ToDateTime(row.Cells["BirthDate"].Value);
                else
                    dtpDateofBirth.Value = DateTime.Now;

                // Address / Phone
                txtAddress.Text = row.Cells["Address"].Value == DBNull.Value ? "" : row.Cells["Address"].Value?.ToString() ?? "";
                txtPhoneNumber.Text = row.Cells["PhoneNumber"].Value == DBNull.Value ? "" : row.Cells["PhoneNumber"].Value?.ToString() ?? "";

                // PhotoData -> byte[] -> PictureBox
                if (row.Cells["PhotoData"].Value != DBNull.Value && row.Cells["PhotoData"].Value != null)
                {
                    _employeePhotoBytes = row.Cells["PhotoData"].Value as byte[];
                    if (_employeePhotoBytes != null)
                    {
                        // Dispose ảnh cũ trước
                        if (picImage.Image != null)
                        {
                            picImage.Image.Dispose();
                            picImage.Image = null;
                        }

                        using (MemoryStream ms = new MemoryStream(_employeePhotoBytes))
                        {
                            picImage.Image = Image.FromStream(ms);
                        }

                        // clear Tag because image is from DB
                        picImage.Tag = null;
                    }
                    else
                    {
                        _employeePhotoBytes = null;
                        picImage.Image = null;
                        picImage.Tag = null;
                    }
                }
                else
                {
                    _employeePhotoBytes = null;
                    if (picImage.Image != null)
                    {
                        picImage.Image.Dispose();
                        picImage.Image = null;
                    }
                    picImage.Tag = null;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error selecting row: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnAdd_Click(object sender, EventArgs e)
        {
            try
            {
                // Validate bắt buộc
                if (string.IsNullOrWhiteSpace(txtEmployeeID.Text) ||
                    string.IsNullOrWhiteSpace(txtLastName.Text) ||
                    string.IsNullOrWhiteSpace(txtFirstName.Text))
                {
                    MessageBox.Show("Please enter Employee ID, Last Name, and First Name.",
                                    "Missing Data", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                string empID = txtEmployeeID.Text.Trim();
                string lastName = txtLastName.Text.Trim();
                string firstName = txtFirstName.Text.Trim();
                bool gender = rdoMale.Checked;
                DateTime birthDate = dtpDateofBirth.Value.Date;
                string address = string.IsNullOrWhiteSpace(txtAddress.Text) ? null : txtAddress.Text.Trim();
                string phone = string.IsNullOrWhiteSpace(txtPhoneNumber.Text) ? null : txtPhoneNumber.Text.Trim();

                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    conn.Open();

                    // Kiểm tra trùng
                    const string checkQuery = "SELECT COUNT(1) FROM EMPLOYEE WHERE EmployeeID = @EmployeeID";
                    using (SqlCommand cmdCheck = new SqlCommand(checkQuery, conn))
                    {
                        cmdCheck.Parameters.AddWithValue("@EmployeeID", empID);
                        int count = Convert.ToInt32(cmdCheck.ExecuteScalar());
                        if (count > 0)
                        {
                            MessageBox.Show("Employee ID already exists!", "Duplicate ID", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                            return;
                        }
                    }

                    const string insertQuery = @"
                INSERT INTO EMPLOYEE (EmployeeID, LastName, FirstName, Gender, BirthDate, Address, PhoneNumber, PhotoData)
                VALUES (@EmployeeID, @LastName, @FirstName, @Gender, @BirthDate, @Address, @PhoneNumber, @PhotoData)";

                    using (SqlCommand cmd = new SqlCommand(insertQuery, conn))
                    {
                        cmd.Parameters.AddWithValue("@EmployeeID", empID);
                        cmd.Parameters.AddWithValue("@LastName", lastName);
                        cmd.Parameters.AddWithValue("@FirstName", firstName);
                        cmd.Parameters.AddWithValue("@Gender", gender);
                        cmd.Parameters.AddWithValue("@BirthDate", birthDate);
                        cmd.Parameters.AddWithValue("@Address", (object)address ?? DBNull.Value);
                        cmd.Parameters.AddWithValue("@PhoneNumber", (object)phone ?? DBNull.Value);
                        cmd.Parameters.AddWithValue("@PhotoData", (object)_employeePhotoBytes ?? DBNull.Value);

                        int rows = cmd.ExecuteNonQuery();
                        if (rows > 0)
                        {
                            MessageBox.Show("New employee added successfully.", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                            LoadStaff();
                            ClearFields();
                        }
                        else
                        {
                            MessageBox.Show("No rows inserted.", "Info", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error adding employee: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        private void btnUpdate_Click(object sender, EventArgs e)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(txtEmployeeID.Text))
                {
                    MessageBox.Show("Please enter Employee ID to update.", "Missing Data",
                                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                string empID = txtEmployeeID.Text.Trim();
                string lastName = txtLastName.Text.Trim();
                string firstName = txtFirstName.Text.Trim();
                bool gender = rdoMale.Checked;
                DateTime birthDate = dtpDateofBirth.Value.Date;
                string address = string.IsNullOrWhiteSpace(txtAddress.Text) ? null : txtAddress.Text.Trim();
                string phone = string.IsNullOrWhiteSpace(txtPhoneNumber.Text) ? null : txtPhoneNumber.Text.Trim();

                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    conn.Open();

                    const string checkQuery = "SELECT COUNT(1) FROM EMPLOYEE WHERE EmployeeID = @EmployeeID";
                    using (SqlCommand cmdCheck = new SqlCommand(checkQuery, conn))
                    {
                        cmdCheck.Parameters.AddWithValue("@EmployeeID", empID);
                        int count = Convert.ToInt32(cmdCheck.ExecuteScalar());
                        if (count == 0)
                        {
                            MessageBox.Show("Employee ID does not exist!", "Not Found", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                            return;
                        }
                    }

                    const string updateQuery = @"
                UPDATE EMPLOYEE
                SET LastName = @LastName,
                    FirstName = @FirstName,
                    Gender = @Gender,
                    BirthDate = @BirthDate,
                    Address = @Address,
                    PhoneNumber = @PhoneNumber,
                    PhotoData = @PhotoData
                WHERE EmployeeID = @EmployeeID";

                    using (SqlCommand cmd = new SqlCommand(updateQuery, conn))
                    {
                        cmd.Parameters.AddWithValue("@EmployeeID", empID);
                        cmd.Parameters.AddWithValue("@LastName", lastName);
                        cmd.Parameters.AddWithValue("@FirstName", firstName);
                        cmd.Parameters.AddWithValue("@Gender", gender);
                        cmd.Parameters.AddWithValue("@BirthDate", birthDate);
                        cmd.Parameters.AddWithValue("@Address", (object)address ?? DBNull.Value);
                        cmd.Parameters.AddWithValue("@PhoneNumber", (object)phone ?? DBNull.Value);
                        cmd.Parameters.AddWithValue("@PhotoData", (object)_employeePhotoBytes ?? DBNull.Value);

                        int rows = cmd.ExecuteNonQuery();
                        if (rows > 0)
                        {
                            MessageBox.Show("Employee information updated successfully.", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                            LoadStaff();
                        }
                        else
                        {
                            MessageBox.Show("No rows updated.", "Info", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error updating employee: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        protected override void OnPaint(PaintEventArgs e)
        {
            using (var brush = new System.Drawing.Drawing2D.LinearGradientBrush(
                this.ClientRectangle, Color.PaleTurquoise, Color.White, 90F))
            {
                e.Graphics.FillRectangle(brush, this.ClientRectangle);
            }
        }
    }
}

 


    