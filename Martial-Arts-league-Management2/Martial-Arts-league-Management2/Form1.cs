using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Martial_Arts_league_Management2
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        public OpenFileDialog fd = null;
        private List<Contenders.Contender> ContendersList;
        private void button1_Click(object sender, EventArgs e)
        {
            fd = new OpenFileDialog();
            // show only excel files
            fd.Filter = "Excel Files|*.xls;*.xlsx;*.xlsm";

            if (fd.ShowDialog() == DialogResult.OK)
            {
                txtPath.Text = fd.FileName;
              
            }

        }

        private void btnLoadFile_Click(object sender, EventArgs e)
        {
            if (txtPath.Text != string.Empty)
            {
                LoadFile();
                Contenders.BracketsCreator test = new Contenders.BracketsCreator(GlobalVars.ListOfContenders);
            }
            else
            {
                Helpers.DefaultMessegeBox("יש לבחור קובץ אקסל לטעינה", "לא נבחר קובץ", MessageBoxIcon.Information);
            }
        }

        private void LoadFile()
        {
            // excel instance
            ExcelOperations Eo = new ExcelOperations(txtPath.Text);
            if (Eo.GetContenders() == false)
            {
                MessageBox.Show("קרתה תקלה בטעינת האקסל, התוכנית הפסיקה את פעולתה", "תקלה קריטית", MessageBoxButtons.OK, MessageBoxIcon.Error, MessageBoxDefaultButton.Button1, MessageBoxOptions.RightAlign);
                return;
            }
            else
            {

                Eo.Dispose();
            }
        }

        private void txtPath_TextChanged(object sender, EventArgs e)
        {
            btnLoadFile.Enabled = true;
        }

        private void Form1_Load(object sender, EventArgs e)
        {

        }

        private void numericUpDown1_ValueChanged(object sender, EventArgs e)
        {
            // change the value of the brackets number of contenders
            GeneralBracket.setNumberOfContenders((byte)numNumberOfContenders.Value);
            btnBrowse.Focus();
        }
    }
}
