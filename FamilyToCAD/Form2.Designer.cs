namespace FamilyToCAD
{
    partial class Form2
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
            this.LBL_TempLoc = new System.Windows.Forms.Label();
            this.TB_TemplateLoc = new System.Windows.Forms.TextBox();
            this.But_TemplateBrowse = new System.Windows.Forms.Button();
            this.LBL_Proj = new System.Windows.Forms.Label();
            this.ComBox_Project = new System.Windows.Forms.ComboBox();
            this.LBL_FileType = new System.Windows.Forms.Label();
            this.ComBox_ExpFileType = new System.Windows.Forms.ComboBox();
            this.LBL_Family = new System.Windows.Forms.Label();
            this.TB_FamilyLoc = new System.Windows.Forms.TextBox();
            this.But_FamilyBrowse = new System.Windows.Forms.Button();
            this.But_Export = new System.Windows.Forms.Button();
            this.ComBox_FileExportSetup = new System.Windows.Forms.ComboBox();
            this.LBL_FileExportSetup = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // LBL_TempLoc
            // 
            this.LBL_TempLoc.AutoSize = true;
            this.LBL_TempLoc.Location = new System.Drawing.Point(29, 110);
            this.LBL_TempLoc.Name = "LBL_TempLoc";
            this.LBL_TempLoc.Size = new System.Drawing.Size(140, 20);
            this.LBL_TempLoc.TabIndex = 0;
            this.LBL_TempLoc.Text = "Template Location";
            // 
            // TB_TemplateLoc
            // 
            this.TB_TemplateLoc.Location = new System.Drawing.Point(207, 104);
            this.TB_TemplateLoc.Name = "TB_TemplateLoc";
            this.TB_TemplateLoc.Size = new System.Drawing.Size(434, 26);
            this.TB_TemplateLoc.TabIndex = 1;
            // 
            // But_TemplateBrowse
            // 
            this.But_TemplateBrowse.Location = new System.Drawing.Point(682, 105);
            this.But_TemplateBrowse.Name = "But_TemplateBrowse";
            this.But_TemplateBrowse.Size = new System.Drawing.Size(75, 30);
            this.But_TemplateBrowse.TabIndex = 2;
            this.But_TemplateBrowse.Text = "Browse";
            this.But_TemplateBrowse.UseVisualStyleBackColor = true;
            this.But_TemplateBrowse.Click += new System.EventHandler(this.button1_Click);
            // 
            // LBL_Proj
            // 
            this.LBL_Proj.AutoSize = true;
            this.LBL_Proj.Location = new System.Drawing.Point(29, 47);
            this.LBL_Proj.Name = "LBL_Proj";
            this.LBL_Proj.Size = new System.Drawing.Size(58, 20);
            this.LBL_Proj.TabIndex = 3;
            this.LBL_Proj.Text = "Project";
            // 
            // ComBox_Project
            // 
            this.ComBox_Project.FormattingEnabled = true;
            this.ComBox_Project.Items.AddRange(new object[] {
            "<Current Project>",
            "New Project"});
            this.ComBox_Project.Location = new System.Drawing.Point(207, 39);
            this.ComBox_Project.Name = "ComBox_Project";
            this.ComBox_Project.Size = new System.Drawing.Size(378, 28);
            this.ComBox_Project.TabIndex = 4;
            this.ComBox_Project.SelectedIndexChanged += new System.EventHandler(this.ComBox_Project_SelectedIndexChanged);
            // 
            // LBL_FileType
            // 
            this.LBL_FileType.AutoSize = true;
            this.LBL_FileType.Location = new System.Drawing.Point(29, 298);
            this.LBL_FileType.Name = "LBL_FileType";
            this.LBL_FileType.Size = new System.Drawing.Size(118, 20);
            this.LBL_FileType.TabIndex = 5;
            this.LBL_FileType.Text = "Export FileType";
            // 
            // ComBox_ExpFileType
            // 
            this.ComBox_ExpFileType.FormattingEnabled = true;
            this.ComBox_ExpFileType.Items.AddRange(new object[] {
            "DWG (2000)",
            "DWG (2004)",
            "DWG (2007)",
            "DWG (2010)",
            "DWG (2013)",
            "SAT"});
            this.ComBox_ExpFileType.Location = new System.Drawing.Point(207, 290);
            this.ComBox_ExpFileType.Name = "ComBox_ExpFileType";
            this.ComBox_ExpFileType.Size = new System.Drawing.Size(222, 28);
            this.ComBox_ExpFileType.TabIndex = 6;
            // 
            // LBL_Family
            // 
            this.LBL_Family.AutoSize = true;
            this.LBL_Family.Location = new System.Drawing.Point(29, 172);
            this.LBL_Family.Name = "LBL_Family";
            this.LBL_Family.Size = new System.Drawing.Size(120, 20);
            this.LBL_Family.TabIndex = 7;
            this.LBL_Family.Text = "Family RFA File";
            // 
            // TB_FamilyLoc
            // 
            this.TB_FamilyLoc.Location = new System.Drawing.Point(207, 166);
            this.TB_FamilyLoc.Name = "TB_FamilyLoc";
            this.TB_FamilyLoc.Size = new System.Drawing.Size(431, 26);
            this.TB_FamilyLoc.TabIndex = 8;
            // 
            // But_FamilyBrowse
            // 
            this.But_FamilyBrowse.Location = new System.Drawing.Point(682, 172);
            this.But_FamilyBrowse.Name = "But_FamilyBrowse";
            this.But_FamilyBrowse.Size = new System.Drawing.Size(75, 26);
            this.But_FamilyBrowse.TabIndex = 9;
            this.But_FamilyBrowse.Text = "Browse";
            this.But_FamilyBrowse.UseVisualStyleBackColor = true;
            this.But_FamilyBrowse.Click += new System.EventHandler(this.button2_Click);
            // 
            // But_Export
            // 
            this.But_Export.Location = new System.Drawing.Point(207, 359);
            this.But_Export.Name = "But_Export";
            this.But_Export.Size = new System.Drawing.Size(385, 75);
            this.But_Export.TabIndex = 10;
            this.But_Export.Text = "Run Export";
            this.But_Export.UseVisualStyleBackColor = true;
            this.But_Export.Click += new System.EventHandler(this.button3_Click);
            // 
            // ComBox_FileExportSetup
            // 
            this.ComBox_FileExportSetup.FormattingEnabled = true;
            this.ComBox_FileExportSetup.Items.AddRange(new object[] {
            "Single",
            "Multiple"});
            this.ComBox_FileExportSetup.Location = new System.Drawing.Point(207, 234);
            this.ComBox_FileExportSetup.Name = "ComBox_FileExportSetup";
            this.ComBox_FileExportSetup.Size = new System.Drawing.Size(216, 28);
            this.ComBox_FileExportSetup.TabIndex = 11;
            // 
            // LBL_FileExportSetup
            // 
            this.LBL_FileExportSetup.AutoSize = true;
            this.LBL_FileExportSetup.Location = new System.Drawing.Point(33, 234);
            this.LBL_FileExportSetup.Name = "LBL_FileExportSetup";
            this.LBL_FileExportSetup.Size = new System.Drawing.Size(131, 20);
            this.LBL_FileExportSetup.TabIndex = 12;
            this.LBL_FileExportSetup.Text = "File Export Setup";
            // 
            // Form2
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(9F, 20F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(780, 446);
            this.Controls.Add(this.LBL_FileExportSetup);
            this.Controls.Add(this.ComBox_FileExportSetup);
            this.Controls.Add(this.But_Export);
            this.Controls.Add(this.But_FamilyBrowse);
            this.Controls.Add(this.TB_FamilyLoc);
            this.Controls.Add(this.LBL_Family);
            this.Controls.Add(this.ComBox_ExpFileType);
            this.Controls.Add(this.LBL_FileType);
            this.Controls.Add(this.ComBox_Project);
            this.Controls.Add(this.LBL_Proj);
            this.Controls.Add(this.But_TemplateBrowse);
            this.Controls.Add(this.TB_TemplateLoc);
            this.Controls.Add(this.LBL_TempLoc);
            this.Name = "Form2";
            this.Text = "Form2";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label LBL_TempLoc;
        private System.Windows.Forms.TextBox TB_TemplateLoc;
        private System.Windows.Forms.Button But_TemplateBrowse;
        private System.Windows.Forms.Label LBL_Proj;
        private System.Windows.Forms.ComboBox ComBox_Project;
        private System.Windows.Forms.Label LBL_FileType;
        private System.Windows.Forms.ComboBox ComBox_ExpFileType;
        private System.Windows.Forms.Label LBL_Family;
        private System.Windows.Forms.TextBox TB_FamilyLoc;
        private System.Windows.Forms.Button But_FamilyBrowse;
        private System.Windows.Forms.Button But_Export;
        private System.Windows.Forms.ComboBox ComBox_FileExportSetup;
        private System.Windows.Forms.Label LBL_FileExportSetup;
    }
}