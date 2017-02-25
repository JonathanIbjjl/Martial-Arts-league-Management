using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using BusinessClocks.ExecutiveClocks;
using System.Reflection;
using System.Diagnostics;

namespace MartialArts
{

    public partial class Form1 : Form
    {
        BracketsBuilder Brackets;
        public Form1()
        {
            InitializeComponent();
            UnPlacedFpanel.Size = new Size(Visual.VisualContender.ContMainPanel_Size.Width + 25, UnPlacedFpanel.Height);
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
          
              
               System.Threading.Thread waitThread = new System.Threading.Thread(LoadWaitClock);
               waitThread.Start();

               System.Threading.Thread load = new System.Threading.Thread(LoadFileThread);
               load.Start();

        }


        private void LoadFileThread()
        {
            if (txtPath.Text != string.Empty)
            {
                
                if (LoadFile() == false)
                {
                    this.Invoke(new Action(wClock.Dispose));
                    return;
                }
                else
                {
                    // TODO: make the dgv properly
                    this.Invoke(new Action(LoadDgv));
                    this.Invoke(new Action(wClock.Dispose));
                
                }

            }
            else
            {
                GlobalVars.ListOfContenders.Clear();
                Helpers.DefaultMessegeBox("יש לבחור קובץ אקסל לטעינה", "לא נבחר קובץ", MessageBoxIcon.Information);
            }
        }


        private bool LoadFile()
        {

            if (GlobalVars.ListOfContenders.Count > 1)
            {
                Martial_Arts_league_Management2.PromtForm promt = new Martial_Arts_league_Management2.PromtForm("קיימת רשימת מתחרים בזיכרון המערכת, האם ברצונך להחליף אותם?");

                if (promt.ShowDialog() == DialogResult.No)
                    return false;
                else
                {
                    GlobalVars.ListOfContenders.Clear();
                    dgvMain.DataSource = null;
                }

                promt.Dispose();
            }

            GlobalVars.IsLoading = true;

            // excel instance
            ExcelOperations Eo = new ExcelOperations(txtPath.Text);
            if (Eo.GetContenders() == false)
            {
             //   MessageBox.Show("התוכנית הפסיקה את פעולתה", "תקלה קריטית", MessageBoxButtons.OK, MessageBoxIcon.Error, MessageBoxDefaultButton.Button1, MessageBoxOptions.RightAlign);
                GlobalVars.IsLoading = false;
                return false;
            }
            else
            {
                Eo.Dispose();
                GlobalVars.IsLoading = false;
               this.Invoke(new Action(EnablebtbBuiledBrackets));
                return true;
            }
        }

        public void EnablebtbBuiledBrackets()
        {
            btnBuiledBrackets.Enabled = true;
        }


        private void btnBuiledBrackets_Click(object sender, EventArgs e)
        {
            if (BuiletBracketsAgain() == true)
            {
                System.Threading.Thread waitThread = new System.Threading.Thread(LoadWaitClock);
                waitThread.Start();

                System.Threading.Thread load = new System.Threading.Thread(BuiledBrackets);
                load.Start();
            }

        }

        private void BuiledBrackets()
        {

            if (GlobalVars.IsLoading == true)
                return;
            if (GlobalVars.ListOfContenders.Count < 2)
            {
                this.Invoke(new Action(wClock.Dispose));
                Helpers.DefaultMessegeBox("לא קיימים משתתפים לבניית בתים" + Environment.NewLine + "אנא טען קובץ", "חסרים נתונים", MessageBoxIcon.Warning);
            }
            else
            {
                GlobalVars.IsLoading = true;
                Brackets = new BracketsBuilder(MartialArts.GlobalVars.ListOfContenders, false);
                Brackets.Init();
                GlobalVars.IsLoading = false;
                this.Invoke(new Action(wClock.Dispose));
                this.Invoke(new Action(TestToDelete));
            }
        }

      
        private BusinessClocks.ExecutiveClocks.WaitClock wClock;
        private void LoadWaitClock()
        {
           FilesPanel.Invoke(new Action(AddClock));
        }

        private void AddClock()
        {

            Int16 height = (Int16)((btnBuiledBrackets.Location.Y + btnBuiledBrackets.Height) - txtPath.Location.Y);
            var x = btnBuiledBrackets.Location.X - 100 - 40;
            var y = txtPath.Location.Y-5;
               
            wClock = new WaitClock(100, 100,"...המתן");
            wClock.ClockBackGroundColor = FilesPanel.BackColor;
            wClock.LoadFont("ARIAL", 9, FontStyle.Bold);
            wClock.OuterCircleWeight = 10;
            wClock.InnerCircleWeight = 5;
            wClock.OuterCircleColor = Color.FromArgb(227, 154, 44);
            wClock.setArrayColors(new Color[] { Color.FromArgb(28,28,28), Color.Maroon });

            wClock.Create(true);
            wClock.Clock.Location = new Point(x, y);
            FilesPanel.Controls.Add(wClock.Clock);
        }

      

        private void LoadDgv()
        {
            if (dgvMain.Rows.Count > 0)
            {
                dgvMain.Rows.Clear();
                dgvMain.Columns.Clear();
            }

            dgvMain.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dgvMain.DoubleBuffered(true);
            dgvMain.EnableHeadersVisualStyles = false;

            dgvMain.ColumnHeadersDefaultCellStyle.BackColor = Color.FromArgb(28, 28, 28);
            dgvMain.ColumnHeadersDefaultCellStyle.ForeColor = Color.FromArgb(227, 154, 44);
            this.dgvMain.RowHeadersWidth = 70;
            dgvMain.AutoResizeColumns(DataGridViewAutoSizeColumnsMode.Fill);

            if (GlobalVars.ListOfContenders.Count < 1)
                return;

            dgvMain.Columns.Add("FirstName", "שם");
            dgvMain.Columns.Add("LastName", "שם משפחה");
            dgvMain.Columns.Add("HebrewBeltColor", "חגורה");
            dgvMain.Columns.Add("GetWeightValue", "קטגוריית משקל");
            dgvMain.Columns.Add("Weight", "משקל מדוייק");
            dgvMain.Columns.Add("GetAgeValue", "קטגוריית גיל");
            dgvMain.Columns.Add("Email", "אימייל");
            dgvMain.Columns.Add("PhoneNumber", "טלפון");
            dgvMain.Columns.Add("AcademyName", "אקדמיה");
            dgvMain.Columns.Add("CoachName", "שם מאמן");
            dgvMain.Columns.Add("CoachPhone", "טלפון מאמן");
            dgvMain.Columns.Add("IsMale", "מגדר");
            dgvMain.Columns.Add("IsAllowedVersusMan", "פקטור מגדר");
            dgvMain.Columns.Add("IsAllowedAgeGradeAbove", "פקטור גיל");
            dgvMain.Columns.Add("IsAllowedBeltGradeAbove", "פקטור חגורה");
            dgvMain.Columns.Add("IsAllowedWeightGradeAbove", "פקטור משקל");

     

            // add rows
            dgvMain.Rows.Add(GlobalVars.ListOfContenders.Count);
            for (int i = 0; i < GlobalVars.ListOfContenders.Count; i++)
            {
                dgvMain.Rows[i].Cells["FirstName"].Value = GlobalVars.ListOfContenders[i].FirstName;
                dgvMain.Rows[i].Cells["LastName"].Value = GlobalVars.ListOfContenders[i].LastName;

                dgvMain.Rows[i].Cells["HebrewBeltColor"].Value = GlobalVars.ListOfContenders[i].HebrewBeltColor;

                dgvMain.Rows[i].Cells["GetWeightValue"].Value = GlobalVars.ListOfContenders[i].GetWeightValue;
                dgvMain.Rows[i].Cells["Weight"].Value = GlobalVars.ListOfContenders[i].Weight;
                dgvMain.Rows[i].Cells["GetAgeValue"].Value = GlobalVars.ListOfContenders[i].GetAgeValue;
                dgvMain.Rows[i].Cells["Email"].Value = GlobalVars.ListOfContenders[i].Email;
                dgvMain.Rows[i].Cells["PhoneNumber"].Value = GlobalVars.ListOfContenders[i].PhoneNumber;
                dgvMain.Rows[i].Cells["AcademyName"].Value = GlobalVars.ListOfContenders[i].AcademyName;
                dgvMain.Rows[i].Cells["CoachName"].Value = GlobalVars.ListOfContenders[i].CoachName;
                dgvMain.Rows[i].Cells["CoachPhone"].Value = GlobalVars.ListOfContenders[i].CoachPhone;

                if (GlobalVars.ListOfContenders[i].IsMale == true)
                dgvMain.Rows[i].Cells["IsMale"].Value = "זכר";
                else
                    dgvMain.Rows[i].Cells["IsMale"].Value = "נקבה";

                if (GlobalVars.ListOfContenders[i].IsMale == false && GlobalVars.ListOfContenders[i].IsAllowedVersusMan == true)
                    dgvMain.Rows[i].Cells["IsAllowedVersusMan"].Value = "כן";
                else
                    dgvMain.Rows[i].Cells["IsAllowedVersusMan"].Value = "לא";

                dgvMain.Rows[i].Cells["IsAllowedAgeGradeAbove"].Value = (GlobalVars.ListOfContenders[i].IsAllowedAgeGradeAbove==true) ? "כן" : "לא";
                dgvMain.Rows[i].Cells["IsAllowedBeltGradeAbove"].Value = (GlobalVars.ListOfContenders[i].IsAllowedBeltGradeAbove == true) ? "כן" : "לא";
                dgvMain.Rows[i].Cells["IsAllowedWeightGradeAbove"].Value = (GlobalVars.ListOfContenders[i].IsAllowedWeightGradeAbove == true) ? "כן" : "לא";

                // color
                dgvMain.Rows[i].DefaultCellStyle.BackColor = GlobalVars.ListOfContenders[i].GetBeltColorValue;
                if (GlobalVars.ListOfContenders[i].Belt == 9000)
                   dgvMain.Rows[i].DefaultCellStyle.ForeColor = Color.White;

                this.dgvMain.Rows[i].HeaderCell.Value = GlobalVars.ListOfContenders[i].SystemID.ToString();


            }

            dgvMain.Columns["Email"].AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;
            dgvMain.Columns["AcademyName"].AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;


            // sort
            this.dgvMain.Sort(this.dgvMain.Columns["HebrewBeltColor"], ListSortDirection.Ascending);

 


        }

        private void dgvMain_DoubleClick(object sender, EventArgs e)
        {
         
           Emails.OpenEmail(dgvMain.Rows[dgvMain.CurrentRow.Index].Cells[5].Value.ToString());
        }

        private void dgvMain_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }

        private void קרדיטיםToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Martial_Arts_league_Management2.Credits credits = new Martial_Arts_league_Management2.Credits();
            credits.ShowDialog();
        }

        private void ייצארשימתמתחריםלאקסלToolStripMenuItem_Click(object sender, EventArgs e)
        {



            if (dgvMain.RowCount > 0)
            {
                // promt the user to permit
                using (var promt = new Martial_Arts_league_Management2.PromtForm("האם לייצא את רשימת המתחרים לקובץ אקסל?"))
                {
                    var result = promt.ShowDialog();
                    if (result == DialogResult.No)
                    {
                        return;
                    }
                }

                this.tabControl1.SelectedTab = tabPage1;

                System.Threading.Thread waitThread = new System.Threading.Thread(LoadWaitClock);
                waitThread.Start();

                System.Threading.Thread load = new System.Threading.Thread(ExportDgvToExcel);
                load.Start();

            }
            else
            {
                Helpers.ShowGenericPromtForm("אין פריטים לייצוא ברשימה");
            }
        }

        private void ExportDgvToExcel()
        {
            MartialArts.ExportDgvToExcel export = new ExportDgvToExcel("");
            export.Export(dgvMain);
            this.Invoke(new Action(wClock.Dispose));
        }

        private void pictureBox1_Click(object sender, EventArgs e)
        {
            Random r = new Random();
            Visual.VisualContender f = new Visual.VisualContender(GlobalVars.ListOfContenders[r.Next(1,60)]);
            f.Init();
            f.Vcontender.Location = new Point(pictureBox1.Location.X + pictureBox1.Width + 5, pictureBox1.Location.Y);
            FilesPanel.Controls.Add(f.Vcontender);

            Visual.VisualContender f1 = new Visual.VisualContender(GlobalVars.ListOfContenders[r.Next(1, 60)]);
            f1.Init();
            f1.Vcontender.Location = new Point(pictureBox1.Location.X + pictureBox1.Width + 5, f.Vcontender.Location.Y + f.Vcontender.Height + 3);
            FilesPanel.Controls.Add(f1.Vcontender);

            Visual.VisualContender f2 = new Visual.VisualContender(GlobalVars.ListOfContenders[r.Next(1, 60)]);
            f2.Init();
            f2.Vcontender.Location = new Point(pictureBox1.Location.X + pictureBox1.Width + 5, f1.Vcontender.Location.Y + f1.Vcontender.Height + 3);
            FilesPanel.Controls.Add(f2.Vcontender);

        }

        private void button1_Click_1(object sender, EventArgs e)
        {

            foreach (MartialArts.Bracket b in Brackets.BracketsList)
            {
                foreach (Contenders.Contender c in b.ContendersList)
                {
                    Visual.VisualContender f = new Visual.VisualContender(c);
                    f.Init();
                    BracktsFPanel.Controls.Add(f.Vcontender);
                }
                Label l = new Label();
                l.Size = new Size(507, 36);
                BracktsFPanel.Controls.Add(l);
            }
        }

        private bool BuiletBracketsAgain()
        {
            if (Brackets != null)
            {
                using (Martial_Arts_league_Management2.PromtForm promt = new Martial_Arts_league_Management2.PromtForm("כבר יצרת בתים האם לדרוס את הבתים הקיימים?"))
                {
                    if (promt.ShowDialog() == DialogResult.OK)
                    {
                        Brackets = null;
                        BracktsFPanel.Controls.Clear();
                        UnPlacedFpanel.Controls.Clear();
                        return true;
                    }
                    else
                    {
                        return false;
                    }
                }
            }
            else
            {
                return true;
            }
        }

        
        private void TestToDelete()
        {
            tabControl1.SelectedTab = tabPage2;
            Brackets.BracketsList = Brackets.BracketsList.AsEnumerable().OrderByDescending(x => x.NumberOfContenders).ToList();
            foreach (MartialArts.Bracket b in Brackets.BracketsList)
            {
                //foreach (Contenders.Contender c in b.ContendersList)
                //{
                //    Visual.VisualContender f = new Visual.VisualContender(c);
                //    f.Init();
                //    flowLayoutPanel1.Controls.Add(f.Vcontender);
                //}
                //Label l = new Label();
                //l.Size = new Size(507, 36);
                //flowLayoutPanel1.Controls.Add(l);

                Visual.VisualBracket br = new Visual.VisualBracket(b);
                br.Init();
                BracktsFPanel.Controls.Add(br.Vbracket);
            }

            // unplaced
            foreach (Contenders.Contender c in Brackets.ContendersList)
            {
                Visual.VisualContender visualcont = new Visual.VisualContender(c);
                visualcont.Init();
                UnPlacedFpanel.Controls.Add(visualcont.Vcontender);
            }

            // uselesses
            foreach (Contenders.Contender c in Brackets.UselessContenders)
            {
                Visual.VisualContender visualcont = new Visual.VisualContender(c);
                visualcont.Init();
                UnPlacedFpanel.Controls.Add(visualcont.Vcontender);
            }

          

            CreateStatisticClocks();
            CreateMatchWithoutUseless();
            BracketsClock();
        }


        private BusinessClocks.ExecutiveClocks.GoalsClock _MatchClock;
        private BusinessClocks.ExecutiveClocks.GoalsClock _MatchWithoutUselessClock;
        private BusinessClocks.ExecutiveClocks.GoalsClock _BracketsClock;
        private void CreateStatisticClocks()
        {
            float Percent = (float)(Brackets.BracketsList.SelectMany(x => x.ContendersList).Count()) / ((float)GlobalVars.ListOfContenders.Count);
            if (_MatchClock == null)
            {
                _MatchClock = new GoalsClock(90, 90, Percent);
                _MatchClock.OuterCircleWeight = 10;
                _MatchClock.InnerCircleWeight = 5;
                _MatchClock.InnerCircleColor = GlobalVars.Sys_Yellow;
                _MatchClock.OuterCircleColor = Color.FromArgb(78, 78, 78);

                _MatchClock.ClockBackGroundColor = splitContainer1.Panel1.BackColor;
                _MatchClock.Create(false);
                _MatchClock.Clock.Anchor = AnchorStyles.Right | AnchorStyles.Top;

                _MatchClock.Clock.Location = new Point(0,0);
                 lblPercent.Controls.Add(_MatchClock.Clock);
            }
            else
            {
                _MatchClock.PercentOfGoals = Percent;
                _MatchClock.RefreshClock(true);
            }
        }

        private void CreateMatchWithoutUseless()        {            float Percent = (float)(Brackets.BracketsList.SelectMany(x => x.ContendersList).Count()) / ((float)GlobalVars.ListOfContenders.Count - Brackets.UselessContenders.Count);            if (_MatchWithoutUselessClock == null)            {                _MatchWithoutUselessClock = new GoalsClock(90, 90, Percent);                _MatchWithoutUselessClock.OuterCircleWeight = 10;                _MatchWithoutUselessClock.InnerCircleWeight = 5;                _MatchWithoutUselessClock.InnerCircleColor = GlobalVars.Sys_Yellow;                _MatchWithoutUselessClock.OuterCircleColor = Color.FromArgb(78, 78, 78);                _MatchWithoutUselessClock.ClockBackGroundColor = splitContainer1.Panel1.BackColor;                _MatchWithoutUselessClock.Create(false);                _MatchWithoutUselessClock.Clock.Anchor = AnchorStyles.Right | AnchorStyles.Top;                _MatchWithoutUselessClock.Clock.Location = new Point(0, 0);
                lblPercentwithoutUseless.Controls.Add(_MatchWithoutUselessClock.Clock);            }            else            {                _MatchWithoutUselessClock.PercentOfGoals = Percent;                _MatchWithoutUselessClock.RefreshClock(true);            }        }


        private void BracketsClock()        {            float Percent = (float)(Brackets.BracketsList.Where(x=>x.NumberOfContenders == MartialArts.GeneralBracket.NumberOfContenders).Count()) / ((float)Brackets.BracketsList.Count);            if (_BracketsClock == null)            {                _BracketsClock = new GoalsClock(90, 90, Percent);                _BracketsClock.OuterCircleWeight = 10;                _BracketsClock.InnerCircleWeight = 5;                _BracketsClock.InnerCircleColor = GlobalVars.Sys_Yellow;                _BracketsClock.OuterCircleColor = Color.FromArgb(78, 78, 78);                _BracketsClock.ClockBackGroundColor = splitContainer1.Panel1.BackColor;                _BracketsClock.Create(false);                _BracketsClock.Clock.Anchor = AnchorStyles.Right | AnchorStyles.Top;                _BracketsClock.Clock.Location = new Point(0, 0);
                lblBracketsClock.Controls.Add(_BracketsClock.Clock);            }            else            {                _BracketsClock.PercentOfGoals = Percent;                _BracketsClock.RefreshClock(true);            }        }
        private void label7_Click(object sender, EventArgs e)
        {

        }
    }





    public static class ExtensionMethods
    {
        public static void DoubleBuffered(this DataGridView dgv, bool setting)
        {
            Type dgvType = dgv.GetType();
            PropertyInfo pi = dgvType.GetProperty("DoubleBuffered",
                BindingFlags.Instance | BindingFlags.NonPublic);
            pi.SetValue(dgv, setting, null);
        }
    }
}
