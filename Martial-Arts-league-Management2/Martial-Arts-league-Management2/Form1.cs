using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace MartialArts
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        public OpenFileDialog fd = null;

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

     




        private void Form1_Load(object sender, EventArgs e)
        {

        }

        private void numericUpDown1_ValueChanged(object sender, EventArgs e)
        {
            // change the value of the brackets number of contenders
            GeneralBracket.setNumberOfContenders((byte)numNumberOfContenders.Value);
            btnBrowse.Focus();
        }

     

        private void txtPath_TextChanged_1(object sender, EventArgs e)
        {
            btnLoadFile.Enabled = true;
        }

        private void btnLoadFile_Click_1(object sender, EventArgs e)
        {
            if (txtPath.Text != string.Empty)
            {
                var start = DateTime.Now;
                if (LoadFile() == false)
                {
                    return;
                }
                else
                {
                    // TODO: make the dgv properly
                    dgvMain.DataSource = GlobalVars.ListOfContenders;
                }
                
            }
            else
            {
                Helpers.DefaultMessegeBox("יש לבחור קובץ אקסל לטעינה", "לא נבחר קובץ", MessageBoxIcon.Information);
            }
        }

        private bool LoadFile()
        {

            if (GlobalVars.ListOfContenders.Count > 1)
            {
                if (Helpers.YesNoMessegeBox("?קיימת רשימת מתחרים בזכרון המערכת " + " " + "האם ברצונך להחליף אותם", "קיימים נתונים בזכרון", MessageBoxIcon.Question) == false)
                    return false;
                else
                {
                    GlobalVars.ListOfContenders.Clear();
                    dgvMain.DataSource = null;
                }
            }

            GlobalVars.IsLoading = true;
            // excel instance
            ExcelOperations Eo = new ExcelOperations(txtPath.Text);
            if (Eo.GetContenders() == false)
            {
                MessageBox.Show("התוכנית הפסיקה את פעולתה", "תקלה קריטית", MessageBoxButtons.OK, MessageBoxIcon.Error, MessageBoxDefaultButton.Button1, MessageBoxOptions.RightAlign);
                GlobalVars.IsLoading = false;
                return false;
            }
            else
            {
                Eo.Dispose();
                GlobalVars.IsLoading = false;
                btnBuiledBrackets.Enabled = true;
                return true;
            }
        }


        private void btnBuiledBrackets_Click(object sender, EventArgs e)
        {
            BuiledBrackets();
        }

        private void BuiledBrackets()
        {

            if (GlobalVars.IsLoading == true)
                return;
            if (GlobalVars.ListOfContenders.Count < 2)
            {
                Helpers.DefaultMessegeBox("לא קיימים משתתפים לבניית בתים" + Environment.NewLine + "אנא טען קובץ", "חסרים נתונים", MessageBoxIcon.Warning);
            }
            else
            {
                GlobalVars.IsLoading = true;
                BracketsBuilder b = new BracketsBuilder(MartialArts.GlobalVars.ListOfContenders, false);
                b.Init();
                GlobalVars.IsLoading = false;

                Helpers.DefaultMessegeBox("DONE", "", MessageBoxIcon.Asterisk);
            }
        }


    }
}
