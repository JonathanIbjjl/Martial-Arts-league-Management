namespace Martial_Arts_league_Management2.HelperForms
{
    partial class WeightCategoryForm
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(WeightCategoryForm));
            this.ListPickWeight = new System.Windows.Forms.ListBox();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.lblCurrentCategory = new System.Windows.Forms.Label();
            this.btnCancel = new System.Windows.Forms.Button();
            this.ListShowWeight = new System.Windows.Forms.ListBox();
            this.radAdult = new System.Windows.Forms.RadioButton();
            this.btnOK = new System.Windows.Forms.Button();
            this.radChild = new System.Windows.Forms.RadioButton();
            this.groupBox1.SuspendLayout();
            this.groupBox2.SuspendLayout();
            this.SuspendLayout();
            // 
            // ListPickWeight
            // 
            this.ListPickWeight.FormattingEnabled = true;
            this.ListPickWeight.Location = new System.Drawing.Point(231, 19);
            this.ListPickWeight.Name = "ListPickWeight";
            this.ListPickWeight.Size = new System.Drawing.Size(193, 251);
            this.ListPickWeight.TabIndex = 1;
            this.ListPickWeight.SelectedIndexChanged += new System.EventHandler(this.ListPickWeight_SelectedIndexChanged);
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.groupBox2);
            this.groupBox1.Controls.Add(this.btnCancel);
            this.groupBox1.Controls.Add(this.ListShowWeight);
            this.groupBox1.Controls.Add(this.radAdult);
            this.groupBox1.Controls.Add(this.btnOK);
            this.groupBox1.Controls.Add(this.radChild);
            this.groupBox1.Controls.Add(this.ListPickWeight);
            this.groupBox1.Location = new System.Drawing.Point(12, 12);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(570, 284);
            this.groupBox1.TabIndex = 3;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "בחר קטגוריית משקל";
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.lblCurrentCategory);
            this.groupBox2.Location = new System.Drawing.Point(430, 204);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(128, 66);
            this.groupBox2.TabIndex = 4;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "קטגוריה נוכחית";
            // 
            // lblCurrentCategory
            // 
            this.lblCurrentCategory.Font = new System.Drawing.Font("Arial", 11.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(177)));
            this.lblCurrentCategory.Location = new System.Drawing.Point(6, 26);
            this.lblCurrentCategory.Name = "lblCurrentCategory";
            this.lblCurrentCategory.Size = new System.Drawing.Size(116, 28);
            this.lblCurrentCategory.TabIndex = 0;
            this.lblCurrentCategory.Text = "IBJJL";
            this.lblCurrentCategory.TextAlign = System.Drawing.ContentAlignment.TopCenter;
            // 
            // btnCancel
            // 
            this.btnCancel.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnCancel.Font = new System.Drawing.Font("Arial", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(177)));
            this.btnCancel.Location = new System.Drawing.Point(430, 145);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(128, 53);
            this.btnCancel.TabIndex = 3;
            this.btnCancel.Text = "ביטול";
            this.btnCancel.UseVisualStyleBackColor = true;
            this.btnCancel.Click += new System.EventHandler(this.btnCancel_Click);
            // 
            // ListShowWeight
            // 
            this.ListShowWeight.FormattingEnabled = true;
            this.ListShowWeight.Location = new System.Drawing.Point(11, 19);
            this.ListShowWeight.Name = "ListShowWeight";
            this.ListShowWeight.Size = new System.Drawing.Size(214, 251);
            this.ListShowWeight.TabIndex = 1;
            this.ListShowWeight.SelectedIndexChanged += new System.EventHandler(this.ListShowWeight_SelectedIndexChanged);
            // 
            // radAdult
            // 
            this.radAdult.AutoSize = true;
            this.radAdult.Location = new System.Drawing.Point(497, 19);
            this.radAdult.Name = "radAdult";
            this.radAdult.Size = new System.Drawing.Size(61, 17);
            this.radAdult.TabIndex = 3;
            this.radAdult.Text = "בוגרים";
            this.radAdult.UseVisualStyleBackColor = true;
            this.radAdult.CheckedChanged += new System.EventHandler(this.radAdult_CheckedChanged);
            // 
            // btnOK
            // 
            this.btnOK.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnOK.Font = new System.Drawing.Font("Arial", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(177)));
            this.btnOK.Location = new System.Drawing.Point(430, 86);
            this.btnOK.Name = "btnOK";
            this.btnOK.Size = new System.Drawing.Size(128, 53);
            this.btnOK.TabIndex = 2;
            this.btnOK.Text = "אישור";
            this.btnOK.UseVisualStyleBackColor = true;
            this.btnOK.Click += new System.EventHandler(this.btnOK_Click);
            // 
            // radChild
            // 
            this.radChild.AutoSize = true;
            this.radChild.Checked = true;
            this.radChild.Location = new System.Drawing.Point(435, 19);
            this.radChild.Name = "radChild";
            this.radChild.Size = new System.Drawing.Size(56, 17);
            this.radChild.TabIndex = 2;
            this.radChild.TabStop = true;
            this.radChild.Text = "ילדים";
            this.radChild.UseVisualStyleBackColor = true;
            this.radChild.CheckedChanged += new System.EventHandler(this.radChild_CheckedChanged);
            // 
            // WeightCategoryForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(227)))), ((int)(((byte)(154)))), ((int)(((byte)(44)))));
            this.ClientSize = new System.Drawing.Size(594, 303);
            this.Controls.Add(this.groupBox1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.Fixed3D;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "WeightCategoryForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "בחירת קטגוריית משקל לאירוע";
            this.Load += new System.EventHandler(this.WeightCategoryForm_Load);
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.groupBox2.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.ListBox ListPickWeight;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.Button btnCancel;
        private System.Windows.Forms.Button btnOK;
        private System.Windows.Forms.RadioButton radAdult;
        private System.Windows.Forms.RadioButton radChild;
        private System.Windows.Forms.ListBox ListShowWeight;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.Label lblCurrentCategory;
    }
}