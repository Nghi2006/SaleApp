namespace SaleApp
{
    partial class MainForm
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(MainForm));
            this.label1 = new System.Windows.Forms.Label();
            this.btnStaff = new System.Windows.Forms.Button();
            this.btnCustomer = new System.Windows.Forms.Button();
            this.btnShoe = new System.Windows.Forms.Button();
            this.btnBill = new System.Windows.Forms.Button();
            this.btnExit = new System.Windows.Forms.Button();
            this.picShoeStore = new System.Windows.Forms.PictureBox();
            this.lblWelcome = new System.Windows.Forms.Label();
            this.lblRole = new System.Windows.Forms.Label();
            this.btnStatistic = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this.picShoeStore)).BeginInit();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font(".VnBlackH", 13.8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.Location = new System.Drawing.Point(188, 24);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(482, 36);
            this.label1.TabIndex = 0;
            this.label1.Text = "Shoe Store Management System";
            // 
            // btnStaff
            // 
            this.btnStaff.Font = new System.Drawing.Font("Microsoft Sans Serif", 10.2F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnStaff.Location = new System.Drawing.Point(32, 105);
            this.btnStaff.Name = "btnStaff";
            this.btnStaff.Size = new System.Drawing.Size(215, 37);
            this.btnStaff.TabIndex = 1;
            this.btnStaff.Text = "Staff Management";
            this.btnStaff.UseVisualStyleBackColor = true;
            this.btnStaff.Click += new System.EventHandler(this.btnStaff_Click);
            // 
            // btnCustomer
            // 
            this.btnCustomer.Font = new System.Drawing.Font("Microsoft Sans Serif", 10.2F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnCustomer.Location = new System.Drawing.Point(32, 180);
            this.btnCustomer.Name = "btnCustomer";
            this.btnCustomer.Size = new System.Drawing.Size(215, 37);
            this.btnCustomer.TabIndex = 2;
            this.btnCustomer.Text = "Customer Management";
            this.btnCustomer.UseVisualStyleBackColor = true;
            this.btnCustomer.Click += new System.EventHandler(this.btnCustomer_Click);
            // 
            // btnShoe
            // 
            this.btnShoe.Font = new System.Drawing.Font("Microsoft Sans Serif", 10.2F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnShoe.Location = new System.Drawing.Point(32, 243);
            this.btnShoe.Name = "btnShoe";
            this.btnShoe.Size = new System.Drawing.Size(215, 37);
            this.btnShoe.TabIndex = 3;
            this.btnShoe.Text = "Shoe Management";
            this.btnShoe.UseVisualStyleBackColor = true;
            this.btnShoe.Click += new System.EventHandler(this.btnShoe_Click);
            // 
            // btnBill
            // 
            this.btnBill.Font = new System.Drawing.Font("Microsoft Sans Serif", 10.2F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnBill.Location = new System.Drawing.Point(32, 305);
            this.btnBill.Name = "btnBill";
            this.btnBill.Size = new System.Drawing.Size(215, 37);
            this.btnBill.TabIndex = 4;
            this.btnBill.Text = "Bill Management";
            this.btnBill.UseVisualStyleBackColor = true;
            this.btnBill.Click += new System.EventHandler(this.btnBill_Click);
            // 
            // btnExit
            // 
            this.btnExit.Font = new System.Drawing.Font("Microsoft Sans Serif", 10.2F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnExit.Location = new System.Drawing.Point(95, 425);
            this.btnExit.Name = "btnExit";
            this.btnExit.Size = new System.Drawing.Size(91, 37);
            this.btnExit.TabIndex = 5;
            this.btnExit.Text = "Exit";
            this.btnExit.UseVisualStyleBackColor = true;
            this.btnExit.Click += new System.EventHandler(this.btnExit_Click_1);
            // 
            // picShoeStore
            // 
            this.picShoeStore.Image = ((System.Drawing.Image)(resources.GetObject("picShoeStore.Image")));
            this.picShoeStore.Location = new System.Drawing.Point(292, 105);
            this.picShoeStore.Name = "picShoeStore";
            this.picShoeStore.Size = new System.Drawing.Size(476, 346);
            this.picShoeStore.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.picShoeStore.TabIndex = 6;
            this.picShoeStore.TabStop = false;
            // 
            // lblWelcome
            // 
            this.lblWelcome.AutoSize = true;
            this.lblWelcome.Location = new System.Drawing.Point(286, 425);
            this.lblWelcome.Name = "lblWelcome";
            this.lblWelcome.Size = new System.Drawing.Size(0, 16);
            this.lblWelcome.TabIndex = 7;
            // 
            // lblRole
            // 
            this.lblRole.AutoSize = true;
            this.lblRole.Location = new System.Drawing.Point(357, 425);
            this.lblRole.Name = "lblRole";
            this.lblRole.Size = new System.Drawing.Size(0, 16);
            this.lblRole.TabIndex = 8;
            // 
            // btnStatistic
            // 
            this.btnStatistic.Font = new System.Drawing.Font("Microsoft Sans Serif", 10.2F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnStatistic.Location = new System.Drawing.Point(32, 370);
            this.btnStatistic.Name = "btnStatistic";
            this.btnStatistic.Size = new System.Drawing.Size(215, 38);
            this.btnStatistic.TabIndex = 9;
            this.btnStatistic.Text = "Statistic and Report";
            this.btnStatistic.UseVisualStyleBackColor = true;
            this.btnStatistic.Click += new System.EventHandler(this.btnStatistic_Click);
            // 
            // MainForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(800, 504);
            this.Controls.Add(this.btnStatistic);
            this.Controls.Add(this.lblRole);
            this.Controls.Add(this.lblWelcome);
            this.Controls.Add(this.picShoeStore);
            this.Controls.Add(this.btnExit);
            this.Controls.Add(this.btnBill);
            this.Controls.Add(this.btnShoe);
            this.Controls.Add(this.btnCustomer);
            this.Controls.Add(this.btnStaff);
            this.Controls.Add(this.label1);
            this.Name = "MainForm";
            this.Text = "MainForm";
            ((System.ComponentModel.ISupportInitialize)(this.picShoeStore)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Button btnStaff;
        private System.Windows.Forms.Button btnCustomer;
        private System.Windows.Forms.Button btnShoe;
        private System.Windows.Forms.Button btnBill;
        private System.Windows.Forms.Button btnExit;
        private System.Windows.Forms.PictureBox picShoeStore;
        private System.Windows.Forms.Label lblWelcome;
        private System.Windows.Forms.Label lblRole;
        private System.Windows.Forms.Button btnStatistic;
    }
}