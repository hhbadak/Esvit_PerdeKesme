using System.Windows.Forms;
using System;

namespace PerdeKesme
{
    partial class Home
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
            this.tb_barcode = new System.Windows.Forms.TextBox();
            this.cb_result = new System.Windows.Forms.ComboBox();
            this.cb_fire = new System.Windows.Forms.CheckBox();
            this.dgv_Stoning = new System.Windows.Forms.DataGridView();
            this.label2 = new System.Windows.Forms.Label();
            this.lbl_number = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)(this.dgv_Stoning)).BeginInit();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(162)));
            this.label1.Location = new System.Drawing.Point(42, 15);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(82, 13);
            this.label1.TabIndex = 0;
            this.label1.Text = "Yapılan İşlem";
            // 
            // tb_barcode
            // 
            this.tb_barcode.Location = new System.Drawing.Point(187, 32);
            this.tb_barcode.Name = "tb_barcode";
            this.tb_barcode.Size = new System.Drawing.Size(159, 20);
            this.tb_barcode.TabIndex = 2;
            this.tb_barcode.KeyDown += new System.Windows.Forms.KeyEventHandler(this.tb_barcode_KeyDown);
            // 
            // cb_result
            // 
            this.cb_result.FormattingEnabled = true;
            this.cb_result.Location = new System.Drawing.Point(3, 31);
            this.cb_result.Name = "cb_result";
            this.cb_result.Size = new System.Drawing.Size(156, 21);
            this.cb_result.TabIndex = 1;
            // 
            // cb_fire
            // 
            this.cb_fire.AutoSize = true;
            this.cb_fire.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(162)));
            this.cb_fire.Location = new System.Drawing.Point(12, 58);
            this.cb_fire.Name = "cb_fire";
            this.cb_fire.Size = new System.Drawing.Size(79, 17);
            this.cb_fire.TabIndex = 3;
            this.cb_fire.Text = "ISKARTA";
            this.cb_fire.UseVisualStyleBackColor = true;
            // 
            // dgv_Stoning
            // 
            this.dgv_Stoning.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgv_Stoning.Location = new System.Drawing.Point(3, 81);
            this.dgv_Stoning.Name = "dgv_Stoning";
            this.dgv_Stoning.Size = new System.Drawing.Size(796, 372);
            this.dgv_Stoning.TabIndex = 5;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(162)));
            this.label2.Location = new System.Drawing.Point(230, 16);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(67, 13);
            this.label2.TabIndex = 0;
            this.label2.Text = "Barkod No";
            // 
            // lbl_number
            // 
            this.lbl_number.AutoSize = true;
            this.lbl_number.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(162)));
            this.lbl_number.Location = new System.Drawing.Point(402, 34);
            this.lbl_number.Name = "lbl_number";
            this.lbl_number.Size = new System.Drawing.Size(11, 13);
            this.lbl_number.TabIndex = 0;
            this.lbl_number.Text = ".";
            // 
            // Home
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(800, 450);
            this.Controls.Add(this.lbl_number);
            this.Controls.Add(this.dgv_Stoning);
            this.Controls.Add(this.cb_fire);
            this.Controls.Add(this.cb_result);
            this.Controls.Add(this.tb_barcode);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Name = "Home";
            this.Text = "Taharet Boru Montaj Kabini";
            this.Load += new System.EventHandler(this.Home_Load);
            ((System.ComponentModel.ISupportInitialize)(this.dgv_Stoning)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }


        private void Tb_barcode_KeyDown(object sender, KeyEventArgs e)
        {
            throw new NotImplementedException();
        }

        #endregion

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox tb_barcode;
        private System.Windows.Forms.ComboBox cb_result;
        private System.Windows.Forms.CheckBox cb_fire;
        private System.Windows.Forms.DataGridView dgv_Stoning;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label lbl_number;
    }
}