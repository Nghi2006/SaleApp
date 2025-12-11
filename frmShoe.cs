using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO;
using System.Data.SqlClient;

namespace SaleApp
{
    public partial class frmShoe : Form
    {
        string connectionString = "Server=DESKTOP-JSCD4Q8\\MASTERMOS;Database=ShoeStoreDB;Trusted_Connection=True;TrustServerCertificate=True;";
        private Form mainForm;
        public frmShoe()
        {
            InitializeComponent();
            LoadData();
        }

        private void LoadData()
        {
            try
            {
                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    conn.Open();
                    string query = "SELECT ProductID, ProductName, Unit, UnitPrice, PhotoData FROM PRODUCT";
                    SqlDataAdapter da = new SqlDataAdapter(query, conn);
                    DataTable dt = new DataTable();
                    da.Fill(dt);
                    dgvShoe.DataSource = dt;
                    txtTotalProducts.Text = dt.Rows.Count.ToString();        // Display the total number of products
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error loading data: " + ex.Message);
            }
        }

        // Add new product
        private void AddProduct()
        {
            if (!ValidateFields(out decimal unitPrice)) return;

            string productId = txtProductID.Text.Trim();

            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                conn.Open();
                string checkQuery = "SELECT COUNT(*) FROM PRODUCT WHERE ProductID=@ProductID";
                SqlCommand cmdCheck = new SqlCommand(checkQuery, conn);
                cmdCheck.Parameters.AddWithValue("@ProductID", productId);
                int count = (int)cmdCheck.ExecuteScalar();
                if (count > 0)
                {
                    MessageBox.Show("The ProductID already exists.");
                    return;
                }

                string insertQuery = "INSERT INTO PRODUCT (ProductID, ProductName, Unit, UnitPrice, PhotoData) " +
                                     "VALUES (@ProductID, @ProductName, @Unit, @UnitPrice, @PhotoData)";
                SqlCommand cmd = new SqlCommand(insertQuery, conn);
                cmd.Parameters.AddWithValue("@ProductID", productId);
                cmd.Parameters.AddWithValue("@ProductName", txtProductName.Text.Trim());
                cmd.Parameters.AddWithValue("@Unit", txtUnit.Text.Trim());
                cmd.Parameters.AddWithValue("@UnitPrice", unitPrice);
                if (picImage.Image != null)
                    cmd.Parameters.AddWithValue("@PhotoData", ImageToByte(picImage.Image));
                else
                    cmd.Parameters.AddWithValue("@PhotoData", DBNull.Value);

                cmd.ExecuteNonQuery();
                MessageBox.Show("Product added successfully!");
                LoadData();
                ClearFields();
            }
        }

        // Update Product
        private void UpdateProduct()
        {
            if (dgvShoe.CurrentRow == null)
            {
                MessageBox.Show("Please select a product to update.");
                return;
            }
            if (!ValidateFields(out decimal unitPrice)) return;

            string productId = txtProductID.Text.Trim();

            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                conn.Open();
                string updateQuery = "UPDATE PRODUCT SET ProductName=@ProductName, Unit=@Unit, UnitPrice=@UnitPrice, PhotoData=@PhotoData " +
                                     "WHERE ProductID=@ProductID";
                SqlCommand cmd = new SqlCommand(updateQuery, conn);
                cmd.Parameters.AddWithValue("@ProductID", productId);
                cmd.Parameters.AddWithValue("@ProductName", txtProductName.Text.Trim());
                cmd.Parameters.AddWithValue("@Unit", txtUnit.Text.Trim());
                cmd.Parameters.AddWithValue("@UnitPrice", unitPrice);
                if (picImage.Image != null)
                    cmd.Parameters.AddWithValue("@PhotoData", ImageToByte(picImage.Image));
                else
                    cmd.Parameters.AddWithValue("@PhotoData", DBNull.Value);


                cmd.ExecuteNonQuery();
                MessageBox.Show("Product updated successfully!");
                LoadData();
                ClearFields();
            }
        }

        // -------------------------
        // 4️⃣ Xóa sản phẩm
        // -------------------------
        private void DeleteProduct()
        {
            if (dgvShoe.CurrentRow == null)
            {
                MessageBox.Show("Please select a product to delete.");
                return;
            }

            string productId = txtProductID.Text.Trim();

            DialogResult dr = MessageBox.Show($"Are you sure you want to delete the prodcut {productId}?", "Delete Confirmation",
                                              MessageBoxButtons.YesNo, MessageBoxIcon.Warning);
            if (dr == DialogResult.No) return;

            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                conn.Open();

                string checkFK = "SELECT COUNT(*) FROM INVOICE_DETAIL WHERE ProductID=@ProductID";
                SqlCommand cmdCheck = new SqlCommand(checkFK, conn);
                cmdCheck.Parameters.AddWithValue("@ProductID", productId);
                int count = (int)cmdCheck.ExecuteScalar();
                if (count > 0)
                {
                    MessageBox.Show("The product cannot be deleted because it already exists in an invoice.");
                    return;
                }

                string deleteQuery = "DELETE FROM PRODUCT WHERE ProductID=@ProductID";
                SqlCommand cmdDelete = new SqlCommand(deleteQuery, conn);
                cmdDelete.Parameters.AddWithValue("@ProductID", productId);
                cmdDelete.ExecuteNonQuery();

                MessageBox.Show("Product deleted successfully!");
                LoadData();
                ClearFields();
            }
        }

        // Select / Delete image
        private void ChoosePicture()
        {
            OpenFileDialog ofd = new OpenFileDialog
            {
                Filter = "Image Files|*.jpg;*.jpeg;*.png;*.bmp;*.jfif"
            };
            if (ofd.ShowDialog() == DialogResult.OK)
                picImage.Image = Image.FromFile(ofd.FileName);
        }

        private void RemovePicture()
        {
            if (picImage.Image == null) return;

            DialogResult dr = MessageBox.Show("Are you sure you want to delete the product image?", "Confirm",
                                              MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            if (dr == DialogResult.Yes)
            {
                picImage.Image = null;

                if (!string.IsNullOrEmpty(txtProductID.Text.Trim()))
                {
                    using (SqlConnection conn = new SqlConnection(connectionString))
                    {
                        conn.Open();
                        string updateQuery = "UPDATE PRODUCT SET PhotoData=NULL WHERE ProductID=@ProductID";
                        SqlCommand cmd = new SqlCommand(updateQuery, conn);
                        cmd.Parameters.AddWithValue("@ProductID", txtProductID.Text.Trim());
                        cmd.ExecuteNonQuery();
                    }
                }
            }
        }

        // Clear form
        private void ClearFields()
        {
            txtProductID.Clear();
            txtProductName.Clear();
            txtUnit.Clear();
            txtUnitPrice.Clear();
            picImage.Image = null;
        }

        // Search Product 
        private void SearchProduct()
        {
            string keyword = txtSearch.Text.Trim();
            if (string.IsNullOrEmpty(keyword))
            {
                LoadData();
                return;
            }

            try
            {
                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    conn.Open();
                    string query = "SELECT ProductID, ProductName, Unit, UnitPrice, PhotoData " +
                                   "FROM PRODUCT WHERE ProductID LIKE @Keyword OR ProductName LIKE @Keyword";
                    SqlCommand cmd = new SqlCommand(query, conn);
                    cmd.Parameters.AddWithValue("@Keyword", "%" + keyword + "%");
                    SqlDataAdapter da = new SqlDataAdapter(cmd);
                    DataTable dt = new DataTable();
                    da.Fill(dt);
                    dgvShoe.DataSource = dt;
                    txtTotalProducts.Text = dt.Rows.Count.ToString();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error searching products: " + ex.Message);
            }
        }

        // DataGridView row click
        private void dgvShoe_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex < 0) return;

            DataGridViewRow row = dgvShoe.Rows[e.RowIndex];
            txtProductID.Text = row.Cells["ProductID"].Value.ToString();
            txtProductName.Text = row.Cells["ProductName"].Value.ToString();
            txtUnit.Text = row.Cells["Unit"].Value.ToString();
            txtUnitPrice.Text = row.Cells["UnitPrice"].Value.ToString();

            if (row.Cells["PhotoData"].Value != DBNull.Value)
            {
                picImage.Image = ByteToImage((byte[])row.Cells["PhotoData"].Value);
            }
            else
            {
                picImage.Image = null;
            }
        }

        // 9️⃣ Close form
        private void CloseForm()
        {
            this.Close();
            mainForm?.Show();
        }

        // -------------------------
        // Helper
        // -------------------------
        private bool ValidateFields(out decimal unitPrice)
        {
            unitPrice = 0;
            if (string.IsNullOrWhiteSpace(txtProductID.Text) ||
                string.IsNullOrWhiteSpace(txtProductName.Text) ||
                string.IsNullOrWhiteSpace(txtUnit.Text) ||
                string.IsNullOrWhiteSpace(txtUnitPrice.Text))
            {
                MessageBox.Show("Please fill in all required product information.");
                return false;
            }

            if (!decimal.TryParse(txtUnitPrice.Text, out unitPrice) || unitPrice <= 0)
            {
                MessageBox.Show("UnitPrice must be a number greater than 0.\r\n");
                return false;
            }
            return true;
        }

        private byte[] ImageToByte(Image img)
        {
            using (MemoryStream ms = new MemoryStream())
            {
                img.Save(ms, img.RawFormat);
                return ms.ToArray();
            }
        }

        private Image ByteToImage(byte[] bytes)
        {
            using (MemoryStream ms = new MemoryStream(bytes))
            {
                return Image.FromStream(ms);
            }
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            DialogResult dr = MessageBox.Show(
                "Are you sure you want to close Shoe Management?",
                "Confirm Exit",
                MessageBoxButtons.YesNo,
                MessageBoxIcon.Question
            );

            if (dr == DialogResult.Yes)
            {
                this.Close();           
                mainForm?.Show();       
            }
        }

        private void btnChoosePicture_Click(object sender, EventArgs e)
        {
            ChoosePicture();
        }

        private void btnRemovePicture_Click(object sender, EventArgs e)
        {
            RemovePicture();    
        }

        private void btnAdd_Click(object sender, EventArgs e)
        {
            AddProduct();
        }

        private void btnUpdate_Click(object sender, EventArgs e)
        {
            UpdateProduct();
        }

        private void btnDelete_Click(object sender, EventArgs e)
        {
            DeleteProduct();
        }

        private void btnSearch_Click(object sender, EventArgs e)
        {
            SearchProduct();
        }

        private void btnClear_Click(object sender, EventArgs e)
        {
            ClearFields();
        }
        protected override void OnPaint(PaintEventArgs e)
        {
            using (var brush = new System.Drawing.Drawing2D.LinearGradientBrush(
                this.ClientRectangle, Color.MistyRose, Color.Gainsboro, 90F))
            {
                e.Graphics.FillRectangle(brush, this.ClientRectangle);
            }
        }
    }
}
