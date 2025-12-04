using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace SaleApp
{
    public partial class MainForm : Form
    {
        private string fullName;
        private string role;

        public MainForm(string fullName, string role)
        {
            InitializeComponent();

            // Lưu dữ liệu
            this.fullName = fullName;
            this.role = role;

            // Gán text trước rồi cho biến mất
            lblWelcome.Text = "Welcome, " + fullName;
            lblRole.Text = "Role: " + role;

            // Ẩn label trên view chính 
            this.lblWelcome.Visible = false;
            this.lblRole.Visible = false;

            ApplyRolePermissions();      // Áp dụng phân quyền khi form load
        }

        // ROLE-BASED ACCESS CONTROL
        private void ApplyRolePermissions()
        {
            switch (role)
            {
                case "Admin":
                    // Admin có toàn quyền – không khóa gì cả
                    break;

                case "Sales":
                    btnStaff.Enabled = false;
                    btnStatistic.Enabled = false;
                    break;

                default:
                    // Role không hợp lệ -> khóa hết (tăng bảo mật)
                    btnStaff.Enabled = false;
                    btnCustomer.Enabled = false;
                    btnShoe.Enabled = false;
                    btnBill.Enabled = false;
                    btnStatistic.Enabled = false;
                    break;
            }
        }

        private void btnStaff_Click(object sender, EventArgs e)
        {
            if (role != "Admin")
            {
                MessageBox.Show("You do not have permission to access Staff Management.", "Access Denied");
                return;
            }

            frmStaff f = new frmStaff();
            f.Show();
            this.Hide();
        }

        private void btnExit_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }


        private void btnBill_Click(object sender, EventArgs e)
        {
            if (role == "Warehouse")
            {
                MessageBox.Show("You do not have permission to access Bill Management.", "Access Denied");
                return;
            }

            frmBillManagement bill = new frmBillManagement();
            bill.Show();
            this.Hide();
        }

        private void btnExit_Click_1(object sender, EventArgs e)
        {
            DialogResult result = MessageBox.Show(
       "Are you sure you want to exit the application?",
       "Exit Confirmation",
       MessageBoxButtons.YesNo,
       MessageBoxIcon.Question
   );

            if (result == DialogResult.Yes)
            {
                Application.Exit();
            }
        }

        private void btnCustomer_Click(object sender, EventArgs e)
        {
            try
            {
                // Tạo instance frmCustomer
                frmCustomer customerForm = new frmCustomer();

                // Gán sự kiện FormClosed để khi frmCustomer đóng, MainForm có thể hiển thị lại
                customerForm.FormClosed += (s, args) => this.Show();

                // Hiển thị frmCustomer
                customerForm.Show();

                // Ẩn MainForm
                this.Hide();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error opening Customer form: " + ex.Message);
            }
        }

        private void btnShoe_Click(object sender, EventArgs e)
        {
            try
            {
            // Tạo form frmShoe, truyền MainForm (this) vào constructor
            frmShoe shoeForm = new frmShoe();
            shoeForm.FormClosed += (s, args) => this.Show();
            shoeForm.Show();

            // Ẩn MainForm
            this.Hide();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error opening Customer form: " + ex.Message);
            }
        }

        private void btnStatistic_Click(object sender, EventArgs e)
        {
            if (role != "Admin")
            {
                MessageBox.Show("You do not have permission to access Statistics and Reports.", "Access Denied");
                return;
            }

            frmStatisticReport f = new frmStatisticReport();
            f.Show();
            this.Hide();
        }
        protected override void OnPaint(PaintEventArgs e)
        {
            using (var brush = new System.Drawing.Drawing2D.LinearGradientBrush(
                this.ClientRectangle, Color.Linen, Color.Beige, 90F)) // từ trên xuống dưới
            {
                e.Graphics.FillRectangle(brush, this.ClientRectangle);
            }
        }
    }
}


  