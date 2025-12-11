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
    public partial class frmBillManagement : Form
    {
        string connectionString =
        "Server=DESKTOP-JSCD4Q8\\MASTERMOS;Database=ShoeStoreDB;Trusted_Connection=True;TrustServerCertificate=True;";
        public frmBillManagement()
        {
            InitializeComponent();
        }

        private void frmBillManagement_Load(object sender, EventArgs e)
        {
            LoadEmployees();
            LoadCustomers();
            LoadProducts();
            LoadInvoiceIDs();

            LoadAllInvoiceDetails();
        }
   
        private void LoadAllInvoiceDetails()
        {
            using (var conn = new SqlConnection(connectionString))
            {
                string sql = @"
SELECT 
     i.InvoiceID AS [Invoice ID],
     c.FullName AS [Customer Name],
     p.ProductName AS [Product Name],
     d.Quantity AS [Quantity],
     p.UnitPrice AS [Unit Price],
     (d.Quantity * p.UnitPrice) AS [Total Amount]
FROM INVOICE_DETAIL d
JOIN INVOICE i ON d.InvoiceID = i.InvoiceID
JOIN PRODUCT p ON d.ProductID = p.ProductID
JOIN CUSTOMER c ON i.CustomerID = c.CustomerID";

                var da = new SqlDataAdapter(sql, conn);
                var dt = new DataTable();
                da.Fill(dt);
                dgvBill.DataSource = dt;
                dgvBill.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            }
        }

        private void LoadEmployees()
        {
            using (var conn = new SqlConnection(connectionString))
            {
                string sql = "SELECT EmployeeID, LastName + ' ' + FirstName AS FullName FROM EMPLOYEE";
                var da = new SqlDataAdapter(sql, conn);
                var dt = new DataTable();
                da.Fill(dt);

                cboEmployeeID.DisplayMember = "EmployeeID"; 
                cboEmployeeID.ValueMember = "EmployeeID";   
                cboEmployeeID.DataSource = dt;
                cboEmployeeID.SelectedIndex = -1;
            }
        }

        private void LoadCustomers()
        {
            using (var conn = new SqlConnection(connectionString))
            {
                string sql = "SELECT CustomerID, FullName, Address, Phone FROM CUSTOMER";
                var da = new SqlDataAdapter(sql, conn);
                var dt = new DataTable();
                da.Fill(dt);

                cboCustomerID.DisplayMember = "CustomerID";
                cboCustomerID.ValueMember = "CustomerID";
                cboCustomerID.DataSource = dt;
                cboCustomerID.SelectedIndex = -1;
            }
        }

        private void LoadProducts()
        {
            using (var conn = new SqlConnection(connectionString))
            {
                string sql = "SELECT ProductID, ProductName, UnitPrice FROM PRODUCT";
                var da = new SqlDataAdapter(sql, conn);
                var dt = new DataTable();
                da.Fill(dt);

                cboShoeID.DisplayMember = "ProductID";
                cboShoeID.ValueMember = "ProductID";
                cboShoeID.DataSource = dt;
                cboShoeID.SelectedIndex = -1;
            }
        }

        private void LoadInvoiceIDs()
        {
            using (var conn = new SqlConnection(connectionString))
            {
                string sql = "SELECT InvoiceID FROM INVOICE";
                var da = new SqlDataAdapter(sql, conn);
                var dt = new DataTable();
                da.Fill(dt);

                cboInvoiceID.DisplayMember = "InvoiceID";
                cboInvoiceID.ValueMember = "InvoiceID";
                cboInvoiceID.DataSource = dt;
                cboInvoiceID.SelectedIndex = -1;
            }
        }

        // Event Handlers for ComboBoxes
        private void cboEmployeeID_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (cboEmployeeID.SelectedIndex < 0) return;
            string empID = cboEmployeeID.SelectedValue.ToString();

            using (var conn = new SqlConnection(connectionString))
            {
                string sql = "SELECT LastName + ' ' + FirstName AS FullName FROM EMPLOYEE WHERE EmployeeID=@EmployeeID";
                var cmd = new SqlCommand(sql, conn);
                cmd.Parameters.AddWithValue("@EmployeeID", empID);
                conn.Open();
                txtEmployeeName.Text = cmd.ExecuteScalar()?.ToString();
            }
        }

        private void cboCustomerID_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (cboCustomerID.SelectedIndex < 0) return;
            string custID = cboCustomerID.SelectedValue.ToString();

            using (var conn = new SqlConnection(connectionString))
            {
                string sql = "SELECT FullName, Address, Phone FROM CUSTOMER WHERE CustomerID=@CustomerID";
                var cmd = new SqlCommand(sql, conn);
                cmd.Parameters.AddWithValue("@CustomerID", custID);
                conn.Open();
                using (var rd = cmd.ExecuteReader())
                {
                    if (rd.Read())
                    {
                        txtCustomerName.Text = rd["FullName"].ToString();
                        txtAddress.Text = rd["Address"].ToString();
                        txtPhoneNumber.Text = rd["Phone"].ToString();
                    }
                }
            }
        }
        
        private void cboShoeID_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (cboShoeID.SelectedIndex < 0) return;
            string prodID = cboShoeID.SelectedValue.ToString();

            using (var conn = new SqlConnection(connectionString))
            {
                string sql = "SELECT ProductName, UnitPrice FROM PRODUCT WHERE ProductID=@ProductID";
                var cmd = new SqlCommand(sql, conn);
                cmd.Parameters.AddWithValue("@ProductID", prodID);
                conn.Open();
                using (var rd = cmd.ExecuteReader())
                {
                    if (rd.Read())
                    {
                        txtShoeName.Text = rd["ProductName"].ToString();
                        txtUnitPrice.Text = rd["UnitPrice"].ToString();
                        CalculateTotal();
                    }
                }
            }
        }
        
        private void txtQuantity_TextChanged(object sender, EventArgs e)
        {
            CalculateTotal();
        }
        private void CalculateTotal()
        {
            if (decimal.TryParse(txtUnitPrice.Text, out decimal unitPrice) &&
                int.TryParse(txtQuantity.Text, out int qty))
            {
                txtTotalAmount.Text = (unitPrice * qty).ToString("0");
            }
            else
            {
                txtTotalAmount.Text = "";
            }
        }

        // Add New Invoice
        private void btnAdd_Click(object sender, EventArgs e)
        {
            txtInvoiceID.Clear();
            dtpDateOfSale.Value = DateTime.Now;
            cboEmployeeID.SelectedIndex = -1;
            txtEmployeeName.Clear();
            cboCustomerID.SelectedIndex = -1;
            txtCustomerName.Clear();
            txtAddress.Clear();
            txtPhoneNumber.Clear();
            cboShoeID.SelectedIndex = -1;
            txtShoeName.Clear();
            txtUnitPrice.Clear();
            txtQuantity.Clear();
            txtTotalAmount.Clear();
            dgvBill.DataSource = null;
        }

        // Save Invoice
        private void btnSave_Click(object sender, EventArgs e)
        {
            if (cboEmployeeID.SelectedIndex < 0 || cboCustomerID.SelectedIndex < 0 || cboShoeID.SelectedIndex < 0)
            {
                MessageBox.Show("Please select an employee, a customer, and a product.");
                return;
            }
           
            if (!int.TryParse(txtQuantity.Text, out int qty) || qty <= 0)
            {
                MessageBox.Show("The quantity entered is invalid.");
                return;
            }
          
            if (!decimal.TryParse(txtUnitPrice.Text, out decimal unitPrice))
            {
                MessageBox.Show("The unit price entered is invalid.");
                return;
            }

            string empID = cboEmployeeID.SelectedValue.ToString();
            string custID = cboCustomerID.SelectedValue.ToString();
            string prodID = cboShoeID.SelectedValue.ToString();
            DateTime saleDate = dtpDateOfSale.Value;
            decimal total = unitPrice * qty;

            using (var conn = new SqlConnection(connectionString))
            {
                conn.Open();
                using (var tran = conn.BeginTransaction())
                {
                    try
                    {
                        int invoiceID;

                        if (string.IsNullOrWhiteSpace(txtInvoiceID.Text))
                        {
                            string sqlInsertInvoice = @"
INSERT INTO INVOICE(CustomerID, EmployeeID, InvoiceDate)
VALUES(@CustomerID, @EmployeeID, @InvoiceDate);
SELECT CAST(SCOPE_IDENTITY() AS int);";

                            var cmdInvoice = new SqlCommand(sqlInsertInvoice, conn, tran);
                            cmdInvoice.Parameters.AddWithValue("@CustomerID", custID);
                            cmdInvoice.Parameters.AddWithValue("@EmployeeID", empID);
                            cmdInvoice.Parameters.AddWithValue("@InvoiceDate", saleDate);
                            invoiceID = Convert.ToInt32(cmdInvoice.ExecuteScalar());
                            txtInvoiceID.Text = invoiceID.ToString();
                        }
                        else
                        {
                            invoiceID = int.Parse(txtInvoiceID.Text);
                        }

                        string sqlInsertDetail = @"
INSERT INTO INVOICE_DETAIL(InvoiceID, ProductID, Quantity)
VALUES(@InvoiceID, @ProductID, @Quantity)";

                        var cmdDetail = new SqlCommand(sqlInsertDetail, conn, tran);
                        cmdDetail.Parameters.AddWithValue("@InvoiceID", invoiceID);
                        cmdDetail.Parameters.AddWithValue("@ProductID", prodID);
                        cmdDetail.Parameters.AddWithValue("@Quantity", qty);
                        cmdDetail.ExecuteNonQuery();

                        tran.Commit();
                    }
                    catch (Exception ex)
                    {
                        tran.Rollback();
                        MessageBox.Show("Error saving invoice: " + ex.Message);
                        return;
                    }
                }
            }

            LoadInvoiceDetails(int.Parse(txtInvoiceID.Text));
            MessageBox.Show("Invoice saved successfully!");
        }

        // Load Invoice Details
        private void LoadInvoiceDetails(int invoiceID)
        {
            using (var conn = new SqlConnection(connectionString))
            {
                string sql = @"
SELECT d.ProductID AS [ProductID],
       p.ProductName AS [ProductName],
       d.Quantity AS [Quantity],
       p.UnitPrice AS [UnitPrice],
       (d.Quantity * p.UnitPrice) AS [Total]
FROM INVOICE_DETAIL d
JOIN PRODUCT p ON d.ProductID = p.ProductID
WHERE d.InvoiceID=@InvoiceID";

                var da = new SqlDataAdapter(sql, conn);
                da.SelectCommand.Parameters.AddWithValue("@InvoiceID", invoiceID);
                var dt = new DataTable();
                da.Fill(dt);
                dgvBill.DataSource = dt;
                dgvBill.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            }
        }

        // Cancel
        private void btnCancel_Click(object sender, EventArgs e)
        {
            txtInvoiceID.Clear();
            dtpDateOfSale.Value = DateTime.Now;
            cboEmployeeID.SelectedIndex = -1;
            txtEmployeeName.Clear();
            cboCustomerID.SelectedIndex = -1;
            txtCustomerName.Clear();
            txtAddress.Clear();
            txtPhoneNumber.Clear();
            cboShoeID.SelectedIndex = -1;
            txtShoeName.Clear();
            txtUnitPrice.Clear();
            txtQuantity.Clear();
            txtTotalAmount.Clear();
            dgvBill.DataSource = null;
        }

        // Print Invoice
        private void btnPrint_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtInvoiceID.Text))
            {
                MessageBox.Show("Please select or save the invoice before printing.");
                return;
            }

            int invoiceID = int.Parse(txtInvoiceID.Text);

            using (var conn = new SqlConnection(connectionString))
            {
                string sql = @"
SELECT 
    i.InvoiceID,
    i.InvoiceDate,
    c.FullName AS CustomerName,
    c.Address AS CustomerAddress,
    c.Phone AS CustomerPhone,
    e.LastName + ' ' + e.FirstName AS EmployeeName,
    p.ProductName,
    d.Quantity,
    p.UnitPrice,
    (d.Quantity * p.UnitPrice) AS TotalAmount
FROM INVOICE_DETAIL d
JOIN INVOICE i ON d.InvoiceID = i.InvoiceID
JOIN CUSTOMER c ON i.CustomerID = c.CustomerID
JOIN EMPLOYEE e ON i.EmployeeID = e.EmployeeID
JOIN PRODUCT p ON d.ProductID = p.ProductID
WHERE i.InvoiceID = @InvoiceID";

                var cmd = new SqlCommand(sql, conn);
                cmd.Parameters.AddWithValue("@InvoiceID", invoiceID);
                var dt = new DataTable();

                var da = new SqlDataAdapter(cmd);
                da.Fill(dt);

                if (dt.Rows.Count == 0)
                {
                    MessageBox.Show("No invoice information was found.");
                    return;
                }

                DataRow firstRow = dt.Rows[0];
                string header = $"Hóa đơn #{invoiceID}\n" +
                                $"Ngày: {Convert.ToDateTime(firstRow["InvoiceDate"]).ToShortDateString()}\n" +
                                $"Khách hàng: {firstRow["CustomerName"]} - {firstRow["CustomerPhone"]}\n" +
                                $"Địa chỉ: {firstRow["CustomerAddress"]}\n" +
                                $"Nhân viên bán: {firstRow["EmployeeName"]}\n\n" +
                                $"Chi tiết sản phẩm:\n";

                string details = "";
                decimal totalInvoice = 0;
                foreach (DataRow row in dt.Rows)
                {
                    string productName = row["ProductName"].ToString();
                    int qty = Convert.ToInt32(row["Quantity"]);
                    decimal unitPrice = Convert.ToDecimal(row["UnitPrice"]);
                    decimal total = Convert.ToDecimal(row["TotalAmount"]);

                    details += $"{productName} - Quantity: {qty}, Unit Price: {unitPrice:0,#}₫, Total: {total:0,#}₫\n";
                    totalInvoice += total;
                }

                details += $"\nTotal Amount: {totalInvoice:0,#}₫";

                MessageBox.Show(header + details, $"Hóa đơn #{invoiceID}");
            }

        }

        // Close form 
        private void btnClose_Click(object sender, EventArgs e)
        {
            DialogResult result = MessageBox.Show(
      "Are you sure you want to exit?",
      "Confirm Exit",
      MessageBoxButtons.YesNo,
      MessageBoxIcon.Question
  );

            if (result == DialogResult.Yes)
            {
                Form mainForm = Application.OpenForms["MainForm"];
                if (mainForm != null)
                {
                    mainForm.Show(); 
                }

                this.Close(); 
            }
        }
        protected override void OnPaint(PaintEventArgs e)
        {
            using (var brush = new System.Drawing.Drawing2D.LinearGradientBrush(
    this.ClientRectangle, Color.LemonChiffon, Color.LightGoldenrodYellow, 90F))
            {
                e.Graphics.FillRectangle(brush, this.ClientRectangle);
            }
        }
        
        // Search Invoice
        private void btnSearchInvoice_Click(object sender, EventArgs e)
        {
            if (cboInvoiceID.SelectedIndex < 0)
            {
                LoadAllInvoiceDetails();
                return;
            }

            if (!int.TryParse(cboInvoiceID.SelectedValue.ToString(), out int invoiceID))
            {
                MessageBox.Show("Invalid Invoice ID.");
                return;
            }

            LoadInvoiceDetails(invoiceID);
        }
    }
}
   



