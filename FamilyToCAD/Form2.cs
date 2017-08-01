using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Microsoft.Win32;
using System.IO;

namespace FamilyToCAD
{
    public partial class Form2 : Form
    {
        private string s_TemplateFile;
        private string s_FamilyFile;
        private string s_ProjectLocation;
        private string s_ExportFileType;
        private string s_FileExportSetup;
        private RegistryKey templatekey;
        private string revityear = "2014";
        private string keystr = @"Software\FamilyToDWG";

        public Form2()
        {
            ///<summary>
            ///Initializes the userform
            ///Fills in template location if found in registry
            ///</summary>
            string subkeystr = "TemplateLocation" + revityear;
            templatekey = Registry.CurrentUser.OpenSubKey(keystr, true);

            InitializeComponent();

            if (templatekey == null)
            {
                templatekey = Registry.CurrentUser.CreateSubKey(keystr);
                templatekey.SetValue(subkeystr, "");
            }
            else
            {

                if (File.Exists(templatekey.GetValue(@subkeystr,"").ToString()))
                {
                    this.TB_TemplateLoc.Text = templatekey.GetValue(subkeystr, null).ToString();
                }
            }

            this.ComBox_Project.SelectedIndex = 1;
            this.ComBox_ExpFileType.SelectedIndex = 0;
            this.ComBox_FileExportSetup.SelectedIndex = 0;
        }

        public string TemplateFile
        {
            get
            {
                return s_TemplateFile;
            }
            set
            {
                s_TemplateFile = value;
            }
        }

        public string FamilyFile
        {
            get
            {
                return s_FamilyFile;
            }
            set
            {
                s_FamilyFile = value;
            }
        }

        public string ProjectLocation
        {
            get
            {
                return s_ProjectLocation;
            }
            set
            {
                s_ProjectLocation = value;
            }
        }

        public string ExportFileType
        {
            get
            {
                return s_ExportFileType;
            }
            set
            {
                s_ExportFileType = value;
            }
        }

        public string FileExportSetup
        {
            get
            {
                return s_FileExportSetup;
            }
            set
            {
                s_FileExportSetup = value;
            }
        }

        private void button3_Click(object sender, EventArgs e)
        {
            /// <summary>
            /// If the userform has valid inputs, runs the corresponding input actions
            /// </summary>

            Boolean b_Run = false;
            if (this.ComBox_Project.Text == "<Current Project>")
            {
                if (this.TB_FamilyLoc.Text == "")
                {
                    MessageBox.Show("Please Select a family to load");
                    this.Focus();
                }
                else
                {
                    b_Run = true;
                }
            }
            else if (this.ComBox_Project.Text == "New Project")
            {
                if (this.TB_FamilyLoc.Text == "")
                {
                    MessageBox.Show("Please Select a family to load");
                    this.Focus();
                }
                else if (this.TB_TemplateLoc.Text == "")
                {
                    MessageBox.Show("Please Select a template to load");
                    this.Focus();
                }
                else
                {
                    b_Run = true;
                }
            }

            templatekey.SetValue("TemplateLocation" + revityear, this.TB_TemplateLoc.Text);
            this.FamilyFile = this.TB_FamilyLoc.Text;
            this.TemplateFile = this.TB_TemplateLoc.Text;
            this.ExportFileType = this.ComBox_ExpFileType.Text;
            this.ProjectLocation = this.ComBox_Project.Text;
            this.FileExportSetup = this.ComBox_FileExportSetup.Text;

            if (b_Run)
            {
                this.Close();
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            /// <summary>
            /// Runs A Windows form to retrieve the requested Revit Template
            /// </summary>
            var templateFD = new OpenFileDialog();
            templateFD.Filter = "rte files (*.rte)|*.rte";
            templateFD.Title = "Choose a Template";
            templateFD.ShowDialog();
            if (templateFD.FileName != "")
            {
                this.TB_TemplateLoc.Text = templateFD.FileName;
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            /// <summary>
            /// Runs A Windows form to retrieve the requested Revit Family
            /// </summary>
            var FD = new OpenFileDialog();
            FD.Filter = "rfa files (*.rfa)|*.rfa";
            FD.Title = "Choose A RevitRFA Family file";
            FD.ShowDialog();
            if (FD.FileName != "")
            {
                this.TB_FamilyLoc.Text = FD.FileName;
            }
        }

        private void ComBox_Project_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (this.ComBox_Project.Text == "<Current Project")
            {
                this.TB_TemplateLoc.Enabled = false;
                this.TB_TemplateLoc.ReadOnly = true;
            }
            else if (this.ComBox_Project.Text == "New Project")
            {
                this.TB_TemplateLoc.Enabled = true;
                this.TB_TemplateLoc.ReadOnly = false;
            }
        }
    }
}
