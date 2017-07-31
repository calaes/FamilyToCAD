using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace FamilyToCAD
{
    public partial class Form2 : Form
    {
        private string s_TemplateFile;
        private string s_FamilyFile;
        private string s_ProjectLocation;
        private string s_ExportFileType;

        public Form2()
        {
            InitializeComponent();
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

        private void button3_Click(object sender, EventArgs e)
        {
            /// <summary>
            /// If the userform has valid inputs, runs the corresponding input actions
            /// </summary>
            if (textBox1.Text == "")
            {
                MessageBox.Show("Please Select a Template to load from");
                this.Focus();
            }
            else if (textBox2.Text == "")
            {
                MessageBox.Show("Please Select a family to load");
                this.Focus();
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            /// <summary>
            /// Runs A Windows form to retrieve the requested Revit Template
            /// </summary>
        }

        private void button2_Click(object sender, EventArgs e)
        {
            /// <summary>
            /// Runs A Windows form to retrieve the requested Revit Family
            /// </summary>
        }

    }
}
