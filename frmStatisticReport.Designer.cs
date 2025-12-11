namespace SaleApp
{
    partial class frmStatisticReport
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
            this.label1 = new System.Windows.Forms.Label();
            this.tabStatistic = new System.Windows.Forms.TabControl();
            this.tabProductStock = new System.Windows.Forms.TabPage();
            this.btnExportProduct = new System.Windows.Forms.Button();
            this.btnSearchProduct = new System.Windows.Forms.Button();
            this.dgvProductStock = new System.Windows.Forms.DataGridView();
            this.label2 = new System.Windows.Forms.Label();
            this.txtProductID = new System.Windows.Forms.TextBox();
            this.tabSaleProfit = new System.Windows.Forms.TabPage();
            this.dtpTime = new System.Windows.Forms.DateTimePicker();
            this.cboTimeType = new System.Windows.Forms.ComboBox();
            this.btnExportSale = new System.Windows.Forms.Button();
            this.btnSearchSale = new System.Windows.Forms.Button();
            this.dgvSaleProfit = new System.Windows.Forms.DataGridView();
            this.label5 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.txtTotal = new System.Windows.Forms.TextBox();
            this.btnClose = new System.Windows.Forms.Button();
            this.btnRefresh = new System.Windows.Forms.Button();
            this.tabStatistic.SuspendLayout();
            this.tabProductStock.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvProductStock)).BeginInit();
            this.tabSaleProfit.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvSaleProfit)).BeginInit();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font(".VnBlackH", 13.8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.Location = new System.Drawing.Point(232, 22);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(362, 36);
            this.label1.TabIndex = 0;
            this.label1.Text = "Statistics And Reports ";
            // 
            // tabStatistic
            // 
            this.tabStatistic.Controls.Add(this.tabProductStock);
            this.tabStatistic.Controls.Add(this.tabSaleProfit);
            this.tabStatistic.Cursor = System.Windows.Forms.Cursors.Default;
            this.tabStatistic.Font = new System.Drawing.Font("Microsoft Sans Serif", 10.2F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.tabStatistic.Location = new System.Drawing.Point(39, 78);
            this.tabStatistic.Name = "tabStatistic";
            this.tabStatistic.SelectedIndex = 0;
            this.tabStatistic.Size = new System.Drawing.Size(769, 458);
            this.tabStatistic.TabIndex = 1;
            // 
            // tabProductStock
            // 
            this.tabProductStock.Controls.Add(this.btnExportProduct);
            this.tabProductStock.Controls.Add(this.btnSearchProduct);
            this.tabProductStock.Controls.Add(this.dgvProductStock);
            this.tabProductStock.Controls.Add(this.label2);
            this.tabProductStock.Controls.Add(this.txtProductID);
            this.tabProductStock.Location = new System.Drawing.Point(4, 29);
            this.tabProductStock.Name = "tabProductStock";
            this.tabProductStock.Padding = new System.Windows.Forms.Padding(3);
            this.tabProductStock.Size = new System.Drawing.Size(761, 425);
            this.tabProductStock.TabIndex = 0;
            this.tabProductStock.Text = "Product & Stock";
            this.tabProductStock.UseVisualStyleBackColor = true;
            // 
            // btnExportProduct
            // 
            this.btnExportProduct.Location = new System.Drawing.Point(275, 332);
            this.btnExportProduct.Name = "btnExportProduct";
            this.btnExportProduct.Size = new System.Drawing.Size(132, 35);
            this.btnExportProduct.TabIndex = 3;
            this.btnExportProduct.Text = "Export Product";
            this.btnExportProduct.UseVisualStyleBackColor = true;
            // 
            // btnSearchProduct
            // 
            this.btnSearchProduct.Location = new System.Drawing.Point(36, 332);
            this.btnSearchProduct.Name = "btnSearchProduct";
            this.btnSearchProduct.Size = new System.Drawing.Size(141, 35);
            this.btnSearchProduct.TabIndex = 3;
            this.btnSearchProduct.Text = "Search Product";
            this.btnSearchProduct.UseVisualStyleBackColor = true;
            this.btnSearchProduct.Click += new System.EventHandler(this.btnSearchProduct_Click);
            // 
            // dgvProductStock
            // 
            this.dgvProductStock.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvProductStock.Location = new System.Drawing.Point(36, 22);
            this.dgvProductStock.Name = "dgvProductStock";
            this.dgvProductStock.RowHeadersWidth = 51;
            this.dgvProductStock.RowTemplate.Height = 24;
            this.dgvProductStock.Size = new System.Drawing.Size(685, 232);
            this.dgvProductStock.TabIndex = 2;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(32, 286);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(88, 20);
            this.label2.TabIndex = 1;
            this.label2.Text = "Product ID";
            // 
            // txtProductID
            // 
            this.txtProductID.Location = new System.Drawing.Point(147, 279);
            this.txtProductID.Name = "txtProductID";
            this.txtProductID.Size = new System.Drawing.Size(260, 27);
            this.txtProductID.TabIndex = 0;
            // 
            // tabSaleProfit
            // 
            this.tabSaleProfit.Controls.Add(this.dtpTime);
            this.tabSaleProfit.Controls.Add(this.cboTimeType);
            this.tabSaleProfit.Controls.Add(this.btnExportSale);
            this.tabSaleProfit.Controls.Add(this.btnSearchSale);
            this.tabSaleProfit.Controls.Add(this.dgvSaleProfit);
            this.tabSaleProfit.Controls.Add(this.label5);
            this.tabSaleProfit.Controls.Add(this.label4);
            this.tabSaleProfit.Controls.Add(this.label3);
            this.tabSaleProfit.Controls.Add(this.txtTotal);
            this.tabSaleProfit.Font = new System.Drawing.Font("Microsoft Sans Serif", 10.2F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.tabSaleProfit.Location = new System.Drawing.Point(4, 29);
            this.tabSaleProfit.Name = "tabSaleProfit";
            this.tabSaleProfit.Padding = new System.Windows.Forms.Padding(3);
            this.tabSaleProfit.Size = new System.Drawing.Size(761, 425);
            this.tabSaleProfit.TabIndex = 1;
            this.tabSaleProfit.Text = "Sales & Profit";
            this.tabSaleProfit.UseVisualStyleBackColor = true;
            // 
            // dtpTime
            // 
            this.dtpTime.Location = new System.Drawing.Point(136, 65);
            this.dtpTime.Name = "dtpTime";
            this.dtpTime.Size = new System.Drawing.Size(306, 27);
            this.dtpTime.TabIndex = 10;
            this.dtpTime.ValueChanged += new System.EventHandler(this.dtpTime_ValueChanged);
            // 
            // cboTimeType
            // 
            this.cboTimeType.FormattingEnabled = true;
            this.cboTimeType.Location = new System.Drawing.Point(136, 18);
            this.cboTimeType.Name = "cboTimeType";
            this.cboTimeType.Size = new System.Drawing.Size(306, 28);
            this.cboTimeType.TabIndex = 9;
            // 
            // btnExportSale
            // 
            this.btnExportSale.Location = new System.Drawing.Point(225, 157);
            this.btnExportSale.Name = "btnExportSale";
            this.btnExportSale.Size = new System.Drawing.Size(140, 35);
            this.btnExportSale.TabIndex = 7;
            this.btnExportSale.Text = "Export Sale";
            this.btnExportSale.UseVisualStyleBackColor = true;
            // 
            // btnSearchSale
            // 
            this.btnSearchSale.Location = new System.Drawing.Point(38, 157);
            this.btnSearchSale.Name = "btnSearchSale";
            this.btnSearchSale.Size = new System.Drawing.Size(141, 35);
            this.btnSearchSale.TabIndex = 8;
            this.btnSearchSale.Text = "Search Sale";
            this.btnSearchSale.UseVisualStyleBackColor = true;
            this.btnSearchSale.Click += new System.EventHandler(this.btnSearchSale_Click);
            // 
            // dgvSaleProfit
            // 
            this.dgvSaleProfit.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvSaleProfit.Location = new System.Drawing.Point(38, 210);
            this.dgvSaleProfit.Name = "dgvSaleProfit";
            this.dgvSaleProfit.RowHeadersWidth = 51;
            this.dgvSaleProfit.RowTemplate.Height = 24;
            this.dgvSaleProfit.Size = new System.Drawing.Size(685, 189);
            this.dgvSaleProfit.TabIndex = 6;
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(34, 65);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(50, 20);
            this.label5.TabIndex = 5;
            this.label5.Text = "Time ";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(34, 26);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(86, 20);
            this.label4.TabIndex = 5;
            this.label4.Text = "Time Type";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(34, 116);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(46, 20);
            this.label3.TabIndex = 5;
            this.label3.Text = "Total";
            // 
            // txtTotal
            // 
            this.txtTotal.Location = new System.Drawing.Point(136, 109);
            this.txtTotal.Name = "txtTotal";
            this.txtTotal.Size = new System.Drawing.Size(587, 27);
            this.txtTotal.TabIndex = 4;
            // 
            // btnClose
            // 
            this.btnClose.Font = new System.Drawing.Font("Microsoft Sans Serif", 10.2F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnClose.Location = new System.Drawing.Point(466, 551);
            this.btnClose.Name = "btnClose";
            this.btnClose.Size = new System.Drawing.Size(108, 37);
            this.btnClose.TabIndex = 3;
            this.btnClose.Text = "Close";
            this.btnClose.UseVisualStyleBackColor = true;
            this.btnClose.Click += new System.EventHandler(this.btnClose_Click);
            // 
            // btnRefresh
            // 
            this.btnRefresh.Font = new System.Drawing.Font("Microsoft Sans Serif", 10.2F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnRefresh.Location = new System.Drawing.Point(278, 551);
            this.btnRefresh.Name = "btnRefresh";
            this.btnRefresh.Size = new System.Drawing.Size(108, 35);
            this.btnRefresh.TabIndex = 3;
            this.btnRefresh.Text = "Refresh";
            this.btnRefresh.UseVisualStyleBackColor = true;
            this.btnRefresh.Click += new System.EventHandler(this.btnRefresh_Click);
            // 
            // frmStatisticReport
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(837, 609);
            this.Controls.Add(this.btnRefresh);
            this.Controls.Add(this.tabStatistic);
            this.Controls.Add(this.btnClose);
            this.Controls.Add(this.label1);
            this.Name = "frmStatisticReport";
            this.Text = "frmStatisticReport";
            this.tabStatistic.ResumeLayout(false);
            this.tabProductStock.ResumeLayout(false);
            this.tabProductStock.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvProductStock)).EndInit();
            this.tabSaleProfit.ResumeLayout(false);
            this.tabSaleProfit.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvSaleProfit)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TabControl tabStatistic;
        private System.Windows.Forms.TabPage tabProductStock;
        private System.Windows.Forms.TabPage tabSaleProfit;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox txtProductID;
        private System.Windows.Forms.DataGridView dgvProductStock;
        private System.Windows.Forms.Button btnExportProduct;
        private System.Windows.Forms.Button btnSearchProduct;
        private System.Windows.Forms.Button btnExportSale;
        private System.Windows.Forms.Button btnSearchSale;
        private System.Windows.Forms.DataGridView dgvSaleProfit;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.ComboBox cboTimeType;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.DateTimePicker dtpTime;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.TextBox txtTotal;
        private System.Windows.Forms.Button btnClose;
        private System.Windows.Forms.Button btnRefresh;
    }
}