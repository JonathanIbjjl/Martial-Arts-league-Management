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
using System.Media;

namespace MartialArts
{

    public partial class Form1 : Form
    {
        public static bool ExampleListIsPresented = true;
        BracketsBuilder Brackets;
        public Form1()
        {
            InitializeComponent();
            this.BracktsFPanel.MouseWheel += FpPanel_MouseWheel;
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
            UpdateClocks(true);
            LoadMe();
        }

        private void LoadMe()
        {
            toolTip1.OwnerDraw = true;
            toolTip1.BackColor = MartialArts.GlobalVars.Sys_Red;
            toolTip1.ForeColor = Color.White;
            toolTip1.Draw += new DrawToolTipEventHandler(tp_Draw);

            // check files archive directory
            Helpers.checkPath();
            // example table
            DgvDefenitions();
            // example with gracie family
            DgvExample();
        }

        private void tp_Draw(object sender, System.Windows.Forms.DrawToolTipEventArgs e)
        {
            e.DrawBackground();
            e.DrawBorder();
            e.DrawText();
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
            if (GlobalVars.IsLoading == true)
                return;

            /// 
            /// safty checks
            /// 

            if (dgvMain.Rows.Count > 1 && ExampleListIsPresented ==false)
            {
                if (Helpers.PromtYesNowQuestion("שים לב!" + Environment.NewLine + "קיימים נתונים ברשימה, טעינת הקובץ תביא להחלפת הרשימה ומחיקתם אנא אשר?") == false)
                    return;
            }

            if (GlobalVars.ListOfContenders.Count > 1)
            {
                Martial_Arts_league_Management2.PromtForm promt = new Martial_Arts_league_Management2.PromtForm("קיימת רשימת מתחרים בזיכרון המערכת, האם ברצונך להחליף אותם?");

                if (promt.ShowDialog() == DialogResult.No)
                    return;
                else
                {
                    GlobalVars.ListOfContenders.Clear();
                    dgvMain.DataSource = null;
                }

                promt.Dispose();
            }

            ///
            ///
            ///
            System.Threading.Thread waitThread = new System.Threading.Thread(LoadWaitClock);
            waitThread.Start();

            System.Threading.Thread load = new System.Threading.Thread(LoadFileThread);
            load.Start();

        }


        private void LoadFileThread()
        {
            GlobalVars.IsLoading = true;

            if (txtPath.Text != string.Empty)
            {

                if (LoadFile() == false)
                {
                    this.Invoke(new Action(wClock.Dispose));
                    GlobalVars.IsLoading = false;
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

            GlobalVars.IsLoading = false;
        }


        private bool LoadFile()
        {


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
            if (GlobalVars.IsLoading == true)
                return;

                if (BuiletBracketsAgain() == true)
            {
                // doing it still from main thread
                dgvMain.AllowUserToAddRows = false; 

                System.Threading.Thread waitThread = new System.Threading.Thread(LoadWaitClock);
                waitThread.Start();

                System.Threading.Thread load = new System.Threading.Thread(BuiledBrackets);
                load.Start();
            }

        }

        private void BuiledBrackets()
        {

            if (GlobalVars.IsLoading == true)
            {
                this.Invoke(new Action(wClock.Dispose));
                return;
            }
            if (GlobalVars.ListOfContenders.Count < 2 && dgvMain.Rows.Count<2)
            {
                Helpers.DefaultMessegeBox("לא קיימים משתתפים לבניית בתים" + Environment.NewLine + "אנא טען קובץ או ייצר רשימה", "חסרים נתונים", MessageBoxIcon.Warning);
                GlobalVars.IsLoading = false;
                this.Invoke(new Action(wClock.Dispose));
            }
            else
            {
                // if MartialArts.GlobalVars.ListOfContenders is empty at this stage the data is not coming from excel, it comes from DgvMain
                // in that case data will load here (in excel datasheet data loaded before via btnLoad button)
                if (MartialArts.GlobalVars.ListOfContenders.Count <= 0)
                {
                    using (CreateContendersFromDgv createConts = new CreateContendersFromDgv(ref dgvMain))
                    {
                        if (createConts.Init() == false)
                        {
                            // something went wrong
                            GlobalVars.IsLoading = false;
                            this.Invoke(new Action(wClock.Dispose));
                            return;
                        }
                    }
                }

                GlobalVars.IsLoading = true;
                Brackets = new BracketsBuilder(MartialArts.GlobalVars.ListOfContenders, false);
                Brackets.Init();               
                this.Invoke(new Action(wClock.Dispose));
                this.Invoke(new Action(CreateVisualBrackets));
                GlobalVars.IsLoading = false;
            }
        }


        private BusinessClocks.ExecutiveClocks.WaitClock wClock;
        private void LoadWaitClock()
        {
            FilesPanel.Invoke(new Action(AddClock));
        }

        private void AddClock()
        {

      

            wClock = new WaitClock(86, 86, "המתן");
            wClock.ClockBackGroundColor = FilesPanel.BackColor;
            wClock.LoadFont("ARIAL", 9, FontStyle.Bold);
            wClock.OuterCircleWeight = 10;
            wClock.InnerCircleWeight = 5;
            wClock.OuterCircleColor = Color.FromArgb(227, 154, 44);
            wClock.setArrayColors(new Color[] { Color.FromArgb(28, 28, 28), Color.Maroon });

            wClock.Create(true);
            wClock.Clock.Location = new Point(0, 0);
            lblwaitClock.Controls.Add(wClock.Clock);
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
            dgvMain.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dgvMain.ColumnHeadersDefaultCellStyle.BackColor = Color.FromArgb(28, 28, 28);
            dgvMain.ColumnHeadersDefaultCellStyle.ForeColor = Color.FromArgb(227, 154, 44);
            this.dgvMain.RowHeadersWidth = 70;
            dgvMain.AutoResizeColumns(DataGridViewAutoSizeColumnsMode.Fill);
            dgvMain.ReadOnly = true;

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

                dgvMain.Rows[i].Cells["IsAllowedAgeGradeAbove"].Value = (GlobalVars.ListOfContenders[i].IsAllowedAgeGradeAbove == true) ? "כן" : "לא";
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

            ExampleListIsPresented = false;


        }

        private void dgvMain_DoubleClick(object sender, EventArgs e)
        {

          //  Emails.OpenEmail(dgvMain.Rows[dgvMain.CurrentRow.Index].Cells[5].Value.ToString());
        }

        private void dgvMain_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

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
            if (Environment.UserName == "john")
            {
                DgvDefenitions();
            }
        }

        private void MoveCursor()
        {
            // Set the Current cursor, move the cursor's Position,
            // and set its clipping rectangle to the form. 

            this.Cursor = new Cursor(Cursor.Current.Handle);
            Cursor.Position = new Point(0, 0);
            Cursor.Clip = new Rectangle(this.Location, this.Size);
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
                        ClearExistingBrackets();
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

        private void ClearExistingBrackets()
        {
            if (Brackets != null)
            {
                Brackets = null;
            }

            

            foreach (Control c in BracktsFPanel.Controls)
            {
                c.Dispose();

                foreach (Control child in c.Controls)
                {
                    child.Dispose();

                    foreach (Control grandChild in child.Controls)
                    {
                        grandChild.Dispose();
                    }
                }
            }

            foreach (Control c in UnPlacedFpanel.Controls)
            {
                c.Dispose();

                foreach (Control child in c.Controls)
                {
                    child.Dispose();

                    foreach (Control grandChild in child.Controls)
                    {
                        grandChild.Dispose();
                    }
                }
            }


            BracktsFPanel.Controls.Clear();
            UnPlacedFpanel.Controls.Clear();

            UnPlacedFpanel.Dispose();
            UnPlacedFpanel = new FlowLayoutPanel();
            UnPlacedFpanel.BackColor = Color.Black;
            UnPlacedFpanel.Dock = DockStyle.Fill;
            UnPlacedFpanel.AutoScroll = true;
            UnPlacedFpanel.Location = new Point(0, 0);
            splitContainer1.Panel1.Controls.Add(UnPlacedFpanel);

            BracktsFPanel.Dispose();
            BracktsFPanel = new FlowLayoutPanel();
            BracktsFPanel.BackColor = Color.Black;
            BracktsFPanel.Dock = DockStyle.Fill;
            BracktsFPanel.AutoScroll = true;
            BracktsFPanel.Location = new Point(0, 0);
            splitContainer1.Panel2.Controls.Add(BracktsFPanel);

            Visual.VisualLeagueEvent.ClearClass();

            for (int i = 0; i < GlobalVars.ListOfContenders.Count; i++)
            {
                GlobalVars.ListOfContenders[i].IsPlaced = false;
                GlobalVars.ListOfContenders[i].IsUseless = false;
            }

            GlobalVars.IsLoading = false;
        }


        private void label7_Click(object sender, EventArgs e)
        {

        }

        private void btnSearch_Click(object sender, EventArgs e)
        {

        }

        private void splitContainer1_SplitterMoved(object sender, SplitterEventArgs e)
        {

        }

        private void btnSearch_Click_1(object sender, EventArgs e)
        {
            if (btnSearch.Text != "אפס חיפוש (Esc)")
            {
                MakeASearch();
            }
            else
            {
                RollBackBtnSearch();
            }
        }

        private void RollBackBtnSearch()
        {
            btnSearch.Text = "חפש";
            btnSearch.BackColor = GlobalVars.Sys_Yellow;
            btnSearch.ForeColor = Color.FromArgb(28, 28, 28);
            Visual.VisualLeagueEvent.CancelAllShadowsOfSearch();
            lblSearchMsg.Text = "";
        }

        private void txtSearch_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                MakeASearch();
            }
        }

        private void MakeASearch()
        {
            if (Visual.VisualLeagueEvent.AllVisualContenders != null)
            {
                lblSearchMsg.Text = "";
                int numberOfResults;
                Visual.VisualLeagueEvent.Search(txtSearch.Text, out numberOfResults);
                txtSearch.Text = "";
                if (numberOfResults == 0)
                {
                    lblSearchMsg.Text = "אין תוצאות";
                }
                else
                {
                    lblSearchMsg.Text = numberOfResults.ToString() + " " + "תוצאות";
                    btnSearch.Text = "אפס חיפוש (Esc)";
                    btnSearch.BackColor = Color.FromArgb(10, 10, 10);
                    btnSearch.ForeColor = Color.White;
                }
            }
        }

        private void tabControl1_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Escape)
            {
                RollBackBtnSearch();
            }
        }

        #region "Drag And Drop"
        private void UnPlacedFpanel_DragDrop(object sender, DragEventArgs e)
        {
            System.Windows.Forms.Control c = e.Data.GetData(e.Data.GetFormats()[0]) as System.Windows.Forms.Control;


            // find parent control (Vcont FlowLayout Panel), max parent is "grandfather"
            System.Windows.Forms.Control Parent;
            if (c.Name.Contains("Cont"))
                Parent = c;
            else if (c.Parent.Name.Contains("Cont"))
                Parent = c.Parent;
            else
                Parent = c.Parent.Parent;

            // Extract Contender ID
            int ContID = (int)MartialArts.Helpers.extractNumberFromString(Parent.Name);
            // check if contender is allready belongs to UnplacedPanel
            if (Visual.VisualLeagueEvent.VisualUnplacedBracketsList.Any(x => x.SystemID == ContID))
                return;

            ((System.Windows.Forms.Control)sender).Controls.Add(Parent);

            Visual.VisualLeagueEvent.AddContender(ContID);
        }

        private void UnPlacedFpanel_DragOver(object sender, DragEventArgs e)
        {
            e.Effect = System.Windows.Forms.DragDropEffects.All;
        }

        private void BracktsFPanel_MouseClick(object sender, MouseEventArgs e)
        {

        }

        #endregion

        private void btnUNDO_Click(object sender, EventArgs e)
        {
            SystemSounds.Beep.Play();
            foreach (Visual.VisualBracket vb in Visual.VisualLeagueEvent.VisualBracketsList)
            {
                foreach (Visual.VisualContender v in vb.VisualCont)
                {
                    Debug.WriteLine(v.Contender.FirstName + " " + v.Contender.LastName);
                }
                Debug.WriteLine("^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^");
            }

            Debug.WriteLine("&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&");
            foreach (Visual.VisualContender v in Visual.VisualLeagueEvent.VisualUnplacedBracketsList)
            
            {
                Debug.WriteLine(v.Contender.FirstName + " " + v.Contender.LastName);
            }
        }

        private void btnBW_Click(object sender, EventArgs e)
        {
            UnderConstruction();
        }

        private void btnFW_Click(object sender, EventArgs e)
        {
            UnderConstruction();
        }

        public static void UnderConstruction()
        {
            Helpers.ShowGenericPromtForm("UNDER CONSTRUCTION");
        }

        private void ייצארשימתבתיםלאקסלToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (GlobalVars.IsLoading == true)
                return;

            if (Visual.VisualLeagueEvent.VisualElementsAndEventExist == false)
            {
                Helpers.ShowGenericPromtForm("לא קיימים בתים, יש ליצור בתים באמצעות לחצן יצירת בתים לאחר טעינת נתונים");
                return;
            }

            // promt the user to permit
            using (var promt = new Martial_Arts_league_Management2.PromtForm("האם לייצא את רשימת הבתים המסכמת לקובץ אקסל?"))
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

            System.Threading.Thread load = new System.Threading.Thread(ExportBracketsToExcel);
            load.Start();
        }

        private void ExportBracketsToExcel()
        {
            ExportBrackets export = new ExportBrackets("", Visual.VisualLeagueEvent.GetUndoStruct());
            export.init();
            this.Invoke(new Action(wClock.Dispose));
        }

        private void FpPanel_MouseWheel(object sender, MouseEventArgs e)
        {
       
        }

        private void קרדיטיםToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            Martial_Arts_league_Management2.Credits credits = new Martial_Arts_league_Management2.Credits();
            credits.ShowDialog();
        }

        private void ArchiveFiles_Click(object sender, EventArgs e)
        {
            // open archive folder
            Helpers.OpenArchiveFolder();
        }

     

        private void btnNewList_Click(object sender, EventArgs e)
        {

        }

        private void MenuItemCreateNewList_Click(object sender, EventArgs e)
        {
            if (dgvMain.Rows.Count > 1 && ExampleListIsPresented == false)
            {
                if (Helpers.PromtYesNowQuestion("שים לב!" + Environment.NewLine + "קיימים נתונים ברשימה, האם למחוק אותם ולייצר רשימה חדשה?") == false)
                   
                    return;
            }

            MartialArts.GlobalVars.ListOfContenders.Clear();
            MartialArts.GlobalVars.ListOfContenders = null;
            ClearExistingBrackets();
            DgvDefenitions();
        }

        private void הצגרשימהלדוגמאToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (dgvMain.Rows.Count > 1 && ExampleListIsPresented == false)
            {
                if (Helpers.PromtYesNowQuestion("שים לב!" + Environment.NewLine + "קיימים נתונים ברשימה, האם למחוק אותם ולהציג רשימה לדוגמא?") == false)

                return;
            }

            MartialArts.GlobalVars.ListOfContenders.Clear();
            MartialArts.GlobalVars.ListOfContenders = null;
            ClearExistingBrackets();
            DgvDefenitions();
            DgvExample();
        }

        private void btnNewList_Click_1(object sender, EventArgs e)
        {
            UnPlacedFpanel.Dispose();
            UnPlacedFpanel = new FlowLayoutPanel();
            UnPlacedFpanel.BackColor = Color.Black;
            UnPlacedFpanel.Dock = DockStyle.Fill;
            UnPlacedFpanel.AutoScroll = true;
            UnPlacedFpanel.Location = new Point(0, 0);
            splitContainer1.Panel1.Controls.Add(UnPlacedFpanel);

            BracktsFPanel.Dispose();
            BracktsFPanel = new FlowLayoutPanel();
            BracktsFPanel.BackColor = Color.Black;
            BracktsFPanel.Dock = DockStyle.Fill;
            BracktsFPanel.AutoScroll = true;
            BracktsFPanel.Location = new Point(0, 0);
            splitContainer1.Panel2.Controls.Add(BracktsFPanel);
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

        // TODO: DELETE
        //public static void DoubleBuffered_FlPanel(this FlowLayoutPanel fp, bool setting)
        //{
        //    Type dgvType = fp.GetType();
        //    PropertyInfo pi = dgvType.GetProperty("DoubleBuffered",
        //        BindingFlags.Instance | BindingFlags.NonPublic);
        //    pi.SetValue(fp, setting, null);
        //}

        //public static void DoubleBuffered_Label(this Label lbl, bool setting)
        //{
        //    Type dgvType = lbl.GetType();
        //    PropertyInfo pi = dgvType.GetProperty("DoubleBuffered",
        //        BindingFlags.Instance | BindingFlags.NonPublic);
        //    pi.SetValue(lbl, setting, null);
        //}

        //public static void DoubleBuffered_SplitContainer(this SplitContainer sp, bool setting)
        //{
        //    Type dgvType = sp.GetType();
        //    PropertyInfo pi = dgvType.GetProperty("DoubleBuffered",
        //        BindingFlags.Instance | BindingFlags.NonPublic);
        //    pi.SetValue(sp, setting, null);
        //}

        //public static void DoubleBuffered_Panel(this Panel panel, bool setting)
        //{
        //    Type dgvType = panel.GetType();
        //    PropertyInfo pi = dgvType.GetProperty("DoubleBuffered",
        //        BindingFlags.Instance | BindingFlags.NonPublic);
        //    pi.SetValue(panel, setting, null);
        //}



        public static bool IsNumeric(this string s)
        {
            float output;
            return float.TryParse(s, out output);
        }

        // Deep clone
        public static IList<T> Clone<T>(this IList<T> listToClone) where T : ICloneable
        {
            return listToClone.Select(item => (T)item.Clone()).ToList();
        }
    }
}