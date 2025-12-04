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
using System.IO;

namespace SaleApp
{
    public partial class frmStatisticReport : Form
    {
        string connectionString =
          "Server=DESKTOP-JSCD4Q8\\MASTERMOS;Database=ShoeStoreDB;Trusted_Connection=True;TrustServerCertificate=True;";
        public frmStatisticReport()
        {
            InitializeComponent();

            // Setup ComboBox
            cboTimeType.Items.Clear();
            cboTimeType.Items.Add("Day");
            cboTimeType.Items.Add("Month");
            cboTimeType.Items.Add("Year");
            cboTimeType.SelectedIndex = 0;

            // Setup DataGridViews
            SetupDgv(dgvProductStock);
            SetupDgv(dgvSaleProfit);

            // Load dữ liệu ban đầu
            LoadProductStock();
            LoadSaleProfit();

            // Setup txtTotal
            txtTotal.ReadOnly = true;
            txtTotal.BackColor = Color.LightYellow;
            txtTotal.TextAlign = HorizontalAlignment.Right;
        }
        private void SetupDgv(DataGridView dgv)
        {
            dgv.AutoGenerateColumns = true;
            dgv.AllowUserToAddRows = false;
            dgv.ReadOnly = true;
            dgv.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
        }

        #region Product & Stock
        private void LoadProductStock(string productIdFilter = "")
        {
            try
            {
                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    string query = @"
                        SELECT p.ProductID, p.ProductName, p.Unit, p.UnitPrice,
                               ISNULL(SUM(d.Quantity),0) AS TotalSold
                        FROM PRODUCT p
                        LEFT JOIN INVOICE_DETAIL d ON p.ProductID = d.ProductID
                        LEFT JOIN INVOICE i ON d.InvoiceID = i.InvoiceID
                        WHERE (@ProductID = '' OR p.ProductID LIKE '%' + @ProductID + '%')
                        GROUP BY p.ProductID, p.ProductName, p.Unit, p.UnitPrice
                        ORDER BY p.ProductID";

                    using (SqlCommand cmd = new SqlCommand(query, conn))
                    {
                        cmd.Parameters.AddWithValue("@ProductID", productIdFilter);
                        SqlDataAdapter da = new SqlDataAdapter(cmd);
                        DataTable dt = new DataTable();
                        da.Fill(dt);
                        dgvProductStock.DataSource = dt;
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error loading product stock: " + ex.Message);
            }
        }

        private void btnSearchProduct_Click(object sender, EventArgs e)
        {
            LoadProductStock(txtProductID.Text.Trim());
        }

        private void btnExportProduct_Click(object sender, EventArgs e)
        {
            ExportDgvToCsv(dgvProductStock, "ProductStockReport.csv");
        }
        #endregion

        #region Sales & Profit
        private void LoadSaleProfit()
        {
            try
            {
                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    conn.Open();

                    string query = @"
                        SELECT 
                            p.ProductID,
                            p.ProductName,
                            SUM(d.Quantity) AS QuantitySold,
                            SUM(d.Quantity * p.UnitPrice) AS Revenue,
                            SUM(d.Quantity * p.UnitPrice) - (SUM(d.Quantity * p.UnitPrice) * 0.3) AS Profit
                        FROM INVOICE_DETAIL d
                        JOIN PRODUCT p ON d.ProductID = p.ProductID
                        JOIN INVOICE i ON d.InvoiceID = i.InvoiceID
                        WHERE 1=1
                    ";

                    string timeType = cboTimeType.SelectedItem?.ToString();
                    DateTime selectedDate = dtpTime.Value;

                    if (!string.IsNullOrEmpty(timeType))
                    {
                        if (timeType == "Day")
                            query += " AND CAST(i.InvoiceDate AS DATE) = @SelectedDate";
                        else if (timeType == "Month")
                            query += " AND MONTH(i.InvoiceDate) = @Month AND YEAR(i.InvoiceDate) = @Year";
                        else if (timeType == "Year")
                            query += " AND YEAR(i.InvoiceDate) = @Year";
                    }

                    query += " GROUP BY p.ProductID, p.ProductName ORDER BY p.ProductID";

                    using (SqlCommand cmd = new SqlCommand(query, conn))
                    {
                        if (!string.IsNullOrEmpty(timeType))
                        {
                            if (timeType == "Day")
                                cmd.Parameters.AddWithValue("@SelectedDate", selectedDate.Date);
                            else if (timeType == "Month")
                            {
                                cmd.Parameters.AddWithValue("@Month", selectedDate.Month);
                                cmd.Parameters.AddWithValue("@Year", selectedDate.Year);
                            }
                            else if (timeType == "Year")
                                cmd.Parameters.AddWithValue("@Year", selectedDate.Year);
                        }

                        SqlDataAdapter da = new SqlDataAdapter(cmd);
                        DataTable dt = new DataTable();
                        da.Fill(dt);
                        dgvSaleProfit.DataSource = dt;

                        // Tính tổng doanh thu/lợi nhuận
                        decimal totalRevenue = 0;
                        decimal totalProfit = 0;
                        foreach (DataRow row in dt.Rows)
                        {
                            totalRevenue += row.Field<decimal>("Revenue");
                            totalProfit += row.Field<decimal>("Profit");
                        }
                        txtTotal.Text = $"Total Revenue: {totalRevenue:C} | Total Profit: {totalProfit:C}";
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error loading sales/profit: " + ex.Message);
            }
        }

        private void btnSearchSale_Click(object sender, EventArgs e)
        {
            LoadSaleProfit();
        }

        private void btnExportSale_Click(object sender, EventArgs e)
        {
            ExportDgvToCsv(dgvSaleProfit, "SalesProfitReport.csv");
        }
        #endregion

        #region Common
        private void ExportDgvToCsv(DataGridView dgv, string defaultFileName)
        {
            if (dgv.Rows.Count == 0)
            {
                MessageBox.Show("No data to export.", "Export", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            SaveFileDialog sfd = new SaveFileDialog
            {
                Filter = "CSV file (*.csv)|*.csv",
                FileName = defaultFileName
            };

            if (sfd.ShowDialog() == DialogResult.OK)
            {
                try
                {
                    using (StreamWriter sw = new StreamWriter(sfd.FileName))
                    {
                        // Header
                        for (int i = 0; i < dgv.Columns.Count; i++)
                        {
                            sw.Write($"\"{dgv.Columns[i].HeaderText}\"");
                            if (i < dgv.Columns.Count - 1)
                                sw.Write(",");
                        }
                        sw.WriteLine();

                        // Rows
                        foreach (DataGridViewRow row in dgv.Rows)
                        {
                            if (!row.IsNewRow)
                            {
                                for (int i = 0; i < dgv.Columns.Count; i++)
                                {
                                    sw.Write($"\"{row.Cells[i].Value?.ToString()}\"");
                                    if (i < dgv.Columns.Count - 1)
                                        sw.Write(",");
                                }
                                sw.WriteLine();
                            }
                        }
                    }
                    MessageBox.Show("Export successful!", "Export", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error exporting CSV: " + ex.Message, "Export", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        private void btnRefresh_Click(object sender, EventArgs e)
        {
            if (tabStatistic.SelectedTab == tabProductStock)
            {
                txtProductID.Clear();
                LoadProductStock();
            }
            else if (tabStatistic.SelectedTab == tabSaleProfit)
            {
                cboTimeType.SelectedIndex = 0;
                dtpTime.Value = DateTime.Today;
                LoadSaleProfit();
            }
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            DialogResult result = MessageBox.Show(
                "Do you want to exit?",
                "Confirmation",
                MessageBoxButtons.YesNo,
                MessageBoxIcon.Question
            );

            if (result == DialogResult.Yes)
            {
                Application.OpenForms["MainForm"]?.Show();
                this.Close();
            }
    }

        private void dtpTime_ValueChanged(object sender, EventArgs e)
        {
            dtpTime.Format = DateTimePickerFormat.Short;
        }
        #endregion
        protected override void OnPaint(PaintEventArgs e)
        {
            using (var brush = new System.Drawing.Drawing2D.LinearGradientBrush(
                this.ClientRectangle, Color.MistyRose, Color.Thistle, 90F))
            {
                e.Graphics.FillRectangle(brush, this.ClientRectangle);
            }
        }
    }
}
   