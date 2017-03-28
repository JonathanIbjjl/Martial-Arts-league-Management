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
using System.Deployment.Application;

namespace MartialArts
{

    public partial class Form1 : Form
    {
        public static bool ExampleListIsPresented = true;
        public static bool EditingList = false;

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

            header();

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

        public void header(string ProjectName = "")
        {
            // load version
            Version myVersion;

            if (System.Deployment.Application.ApplicationDeployment.IsNetworkDeployed)
            {
                myVersion = System.Deployment.Application.ApplicationDeployment.CurrentDeployment.CurrentVersion;
                this.Text = "מערכת ניהול ליגה IBJJL | גרסה " + myVersion.ToString();
            }
            else
            {
                this.Text = "IBJJL | מערכת ניהול ליגה";
            }

            if (ProjectName != "")
            {
                this.Text = this.Text + " | " + ProjectName;
            }

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

            if (dgvMain.Rows.Count > 1 && ExampleListIsPresented == false)
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

            // now its new project
            GlobalVars.CurrentProject = null;
            header("");

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

                System.Threading.Thread waitThread = new System.Threading.Thread(LoadWaitClock);
                waitThread.Start();

                System.Threading.Thread load = new System.Threading.Thread(BuiledBrackets);
                load.Start();
            }

            // now its new project
            GlobalVars.CurrentProject = null;
            header("");
        }

        private void BuiledBrackets()
        {

            if (GlobalVars.IsLoading == true)
            {
                this.Invoke(new Action(wClock.Dispose));
                return;
            }
            if (GlobalVars.ListOfContenders.Count < 2 && dgvMain.Rows.Count < 2)
            {
                Helpers.ShowGenericPromtForm("לא קיימים משתתפים לבניית בתים" + Environment.NewLine + "אנא טען קובץ או ייצר רשימה");
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

                if (MartialArts.GlobalVars.ListOfContenders.Count > 2300)
                {
                    // OS cant create more that 9998 user objects. 550 conts are 8653 user objects (safty range)
                    Helpers.ShowGenericPromtForm("לא ניתן לטעון יותר מ2300 מתחרים" + Environment.NewLine + "מאחר ומערכת ההפעלה מאפשרת יצירה של עד 9998 אוביקטיי משתמש");
                    GlobalVars.IsLoading = false;
                    this.Invoke(new Action(wClock.Dispose));
                    return;
                }

                // create graphical brackets
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

            // determine if child or adult and change radiobutton if needed
            if (GlobalVars.ListOfContenders[0].IsChild == true)
                radChild.Checked = true;
            else
                radAdult.Checked = true;

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
            EditingList = false;

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

            if (GlobalVars.IsLoading == true)
                return;

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

        private bool BuiletBracketsAgain(bool PromtTheUser = true)
        {
            if (Brackets != null || Visual.VisualLeagueEvent.AllVisualContenders != null)
            {

                if (PromtTheUser == false)
                {
                    ClearExistingBrackets();
                    return true;
                }

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


            BracktsFPanel.Controls.Clear();
            UnPlacedFpanel.Controls.Clear();

            // all controls disposed via this method
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
            NewProject();
        }

        public void NewProject()
        {
            if (GlobalVars.IsLoading == true)
                return;

            if (dgvMain.Rows.Count > 1 && ExampleListIsPresented == false)
            {
                if (Helpers.PromtYesNowQuestion("שים לב!" + Environment.NewLine + "קיימים נתונים ברשימה, האם למחוק אותם ולייצר רשימה חדשה?") == false)

                    return;
            }

            MartialArts.GlobalVars.ListOfContenders.Clear();
            MartialArts.GlobalVars.ListOfContenders = null;
            ClearExistingBrackets();
            DgvDefenitions();
            dgvMain.Rows[0].Cells[0].Selected = true;
            this.tabControl1.SelectedTab = tabPage1;

            // now its new project
            GlobalVars.CurrentProject = null;
            header("");

            EditingList = true;
        }

        private void הצגרשימהלדוגמאToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (GlobalVars.IsLoading == true)
                return;

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
            EditingList = false;
            this.tabControl1.SelectedTab = tabPage1;

            // now its new project
            GlobalVars.CurrentProject = null;
            header("");
        }

        private void pictureBox2_Click(object sender, EventArgs e)
        {
            Helpers.ShowGenericPromtForm("IBJJL");
        }

        private void tpSave_Click(object sender, EventArgs e)
        {
            if (GlobalVars.IsLoading == true)
                return;

            // special case of saving the system list even if there are no brackets
            if (EditingList == true)
            {
                // there is only a list brackets has not created yet
                if (Visual.VisualLeagueEvent.AllVisualContenders == null || Visual.VisualLeagueEvent.AllVisualContenders.Count <= 0)
                {
                    SaveEditingList();
                }
                //// brackets has been created TODO: save as systemlist and also save the brackets
                //else
                //{
                //    SaveEditingListAndBrackets();
                //}
                //return;
            }


            // check if there are objects
            if (Visual.VisualLeagueEvent.AllVisualContenders == null || Visual.VisualLeagueEvent.AllVisualContenders.Count <= 0)
            {
                Helpers.ShowGenericPromtForm("עדיין לא יצרת אוביקטים של בתים");
                return;
            }

            // if it is saved project save also the changes made in the saved project
            if (GlobalVars.CurrentProject != null)
            {
                // handle project path
                SerializeDataSaveAs se = new SerializeDataSaveAs(Visual.VisualLeagueEvent.GetUndoStruct(), GlobalVars.CurrentProject);
                se.Serialize();
            }
            else
            {
                SerializeData save = new SerializeData(Visual.VisualLeagueEvent.GetUndoStruct());
                save.Serialize();
            }
        }

        private void SaveEditingList()
        {


            using (CreateContendersFromDgv createConts = new CreateContendersFromDgv(ref dgvMain))
            {
                if (createConts.Init() == true) // list is intact for saving
                {
                    // create empty UNDOstruct only with contenders for the list
                    Visual.VisualLeagueEvent.UndoStruct fakeUndoStruct = new Visual.VisualLeagueEvent.UndoStruct();
                    fakeUndoStruct.IsFakeStructForSavingEditingList = true;

                    fakeUndoStruct.AllVisualContenders = new List<Visual.VisualContender>();
                    fakeUndoStruct._VisualBracketsList = new List<Visual.VisualBracket>();
                    fakeUndoStruct._VisualUnplacedBracketsList = new List<Visual.VisualContender>();
                    // close the editing just for saving to prevent the last empt row being serialize
                    dgvMain.AllowUserToAddRows = false;

                    // create visual contenders just for saving
                    foreach (Contenders.Contender c in GlobalVars.ListOfContenders)
                    {
                        Visual.VisualContender visualcont = new Visual.VisualContender(c);
                        visualcont.Init();
                        fakeUndoStruct.AllVisualContenders.Add(visualcont);
                        fakeUndoStruct._VisualUnplacedBracketsList.Add(visualcont);
                    }


                    // handle project path
                    SerializeData save = new SerializeData(fakeUndoStruct);
                    save.Serialize();

                    // retun to edit mode
                    dgvMain.AllowUserToAddRows = true;

                    GlobalVars.ListOfContenders.Clear();
                    GlobalVars.ListOfContenders = null;
                }
            }

        }

        private void SaveEditingListAndBrackets()
        {
            var Undo = Visual.VisualLeagueEvent.GetUndoStruct();

            // declare the source came from system list
            for (int i = 0; i < Undo.AllVisualContenders.Count; i++)
            {
                Undo.AllVisualContenders[i].Contender.SourceIsFromSystemList = true;
            }

            // if it is saved project save also the changes made in the saved project
            if (GlobalVars.CurrentProject != null)
            {
                // handle project path
                SerializeDataSaveAs se = new SerializeDataSaveAs(Visual.VisualLeagueEvent.GetUndoStruct(), GlobalVars.CurrentProject);
                se.Serialize();
            }

            else
            {
                SerializeData se = new SerializeData(Undo);
                se.Serialize();
            }
        }

        private void tpOpen_Click(object sender, EventArgs e)
        {
            if (GlobalVars.IsLoading == true)
                return;

            string lastSaved = "";
            if (MartialArts.BinaryFiles.SavedDataExist(MartialArts.BinaryFiles.SavedAllContsBinaryFilePath, out lastSaved) == false
                || MartialArts.BinaryFiles.SavedDataExist(MartialArts.BinaryFiles.SavedUnplacedVcBinaryFilePath, out lastSaved) == false
                || MartialArts.BinaryFiles.SavedDataExist(MartialArts.BinaryFiles.SavedVbBinaryFilePath, out lastSaved) == false)
            {
                Helpers.ShowGenericPromtForm("לא קיימים נתונים שמורים או שקרתה תקלה בשמירה האחרונה");
                return;
            }


            try
            {
                // deSerialize
                GlobalVars.IsLoading = true;
                SerializeData load = new SerializeData();
                bool Isok;
                BracketsBuilder b = load.DeSerialize(out Isok);

                // load list
                GlobalVars.ListOfContenders.Clear();
                // load saved contenders to list
                GlobalVars.ListOfContenders = b.ContendersList.ToList();
                foreach (Bracket br in b.BracketsList)
                {
                    foreach (Contenders.Contender c in br.ContendersList)
                    {
                        GlobalVars.ListOfContenders.Add(c);
                    }
                }

                // discover if the contenders came from system list (and brackets has not created yet) if so the list must be system list with ability to edit
                if (b.ContendersList.Count > 0 && b.ContendersList[0].SourceIsFromSystemList == true)
                {
                    LoadFromSystemList(ref b);
                }
                // discover if the contenders came from system list (and brackets created) if so the list must be system list with ability to edit
                else if (b.BracketsList.Count > 0 && b.BracketsList[0].ContendersList.Count> 0 && b.BracketsList[0].ContendersList[0].SourceIsFromSystemList == true)
                {
                    LoadFromSystemList(ref b);
                }
                // contenders did not came from system list, they came from excel
                else
                {
                    LoadDgv();
                }

                // load saved brackets and unplaced contenders
                if (Isok == true)
                {
                    // load data
                    if (BuiletBracketsAgain(false) == true)
                    {
                        Brackets = b;
                        CreateVisualBrackets();
                    }
                }
            }
            catch
            {

            }
            finally
            {
                GlobalVars.IsLoading = false;
            }
        }

        private void LoadFromSystemList(ref BracketsBuilder b)
        {
            DgvDefenitions();
            dgvMain.Rows.Add(GlobalVars.ListOfContenders.Count());
            int counter = 0;
            foreach (Contenders.Contender c in GlobalVars.ListOfContenders)
            {
                dgvMain.Rows[counter].Cells["ID"].Value = c.ID;
                dgvMain.Rows[counter].Cells["FirstName"].Value = c.FirstName;
                dgvMain.Rows[counter].Cells["LastName"].Value = c.LastName;
                dgvMain.Rows[counter].Cells["Belt"].Value = c.HebrewBeltColor;
                dgvMain.Rows[counter].Cells["WeightCat"].Value = c.GetWeightValue;
                dgvMain.Rows[counter].Cells["Weight"].Value = c.Weight;
                dgvMain.Rows[counter].Cells["ageCat"].Value = c.GetAgeValue;
                dgvMain.Rows[counter].Cells["Email"].Value = c.Email;
                dgvMain.Rows[counter].Cells["phone"].Value = c.PhoneNumber;
                dgvMain.Rows[counter].Cells["AcademyName"].Value = c.AcademyName;
                dgvMain.Rows[counter].Cells["coach"].Value = c.CoachName;
                dgvMain.Rows[counter].Cells["coachPhone"].Value = c.CoachPhone;
                dgvMain.Rows[counter].Cells["gender"].Value = (c.IsMale == true) ? "זכר" : "נקבה";
                dgvMain.Rows[counter].Cells["IsAllowedVersusMan"].Value = c.IsAllowedVersusMan;
                dgvMain.Rows[counter].Cells["IsAllowedAgeGradeAbove"].Value = c.IsAllowedAgeGradeAbove;
                dgvMain.Rows[counter].Cells["IsAllowedBeltGradeAbove"].Value = c.IsAllowedBeltGradeAbove;
                dgvMain.Rows[counter].Cells["IsAllowedWeightGradeAbove"].Value = c.IsAllowedWeightGradeAbove;
                counter++;
            }

            EditingList = true;
            GlobalVars.ListOfContenders.Clear();
            GlobalVars.ListOfContenders = null;
        }

        private void tpSaveAs_Click(object sender, EventArgs e)
        {
            if (GlobalVars.IsLoading == true)
                return;

            // check if there are objects
            if (Visual.VisualLeagueEvent.AllVisualContenders == null || Visual.VisualLeagueEvent.AllVisualContenders.Count <= 0)
            {
                Helpers.ShowGenericPromtForm("עדיין לא יצרת אוביקטים של בתים");
                return;
            }

            // promt the user to enter the name of the project
            Martial_Arts_league_Management2.GetProjectNameForm promt = new Martial_Arts_league_Management2.GetProjectNameForm();
            if (promt.ShowDialog() != DialogResult.OK)
            {
                return;
            }

            // handle project path
            MartialArts.ProjectsSavedAsBinaryFiles saveAsPath = new ProjectsSavedAsBinaryFiles(promt.ProjectName);
            if (saveAsPath.CreateSubPath() == true)
            {
                SerializeDataSaveAs se = new SerializeDataSaveAs(Visual.VisualLeagueEvent.GetUndoStruct(), saveAsPath);
                se.Serialize();
            }

            promt.Dispose();
        }

        private void tpOpenProject_Click(object sender, EventArgs e)
        {
            if (GlobalVars.IsLoading == true)
                return;

            if (ProjectsSavedAsBinaryFiles.IsProjectDirExist() == false)
            {
                Helpers.ShowGenericPromtForm("לא קיימת תיקיית פרויקטים");
                return;
            }

            // show the user file to choose
            Martial_Arts_league_Management2.ChooseProjectToLoad choose = new Martial_Arts_league_Management2.ChooseProjectToLoad();

            if (choose.ShowDialog() != DialogResult.OK)
            {
                return;
            }

            // create all files paths instance
            MartialArts.ProjectsSavedAsBinaryFiles paths = new ProjectsSavedAsBinaryFiles(choose.ProjectName);
            paths.SetFullDirWithoutCreating();

            try
            {
                // deSerialize
                GlobalVars.IsLoading = true;
                SerializeDataSaveAs load = new SerializeDataSaveAs(new Visual.VisualLeagueEvent.UndoStruct(), paths);
                bool Isok;
                BracketsBuilder b = load.DeSerialize(out Isok);
                // load list
                GlobalVars.ListOfContenders.Clear();
                // load saved contenders to list
                GlobalVars.ListOfContenders = b.ContendersList.ToList();
                foreach (Bracket br in b.BracketsList)
                {
                    foreach (Contenders.Contender c in br.ContendersList)
                    {
                        GlobalVars.ListOfContenders.Add(c);
                    }
                }

                LoadDgv();

                // load saved brackets and unplaced contenders
                if (Isok == true)
                {
                    // load data
                    if (BuiletBracketsAgain() == true)
                    {
                        Brackets = b;

                        CreateVisualBrackets();
                        // set current project
                        GlobalVars.CurrentProject = paths;
                        // set header with project name
                        header(choose.ProjectName);
                    }
                }

            }
            catch
            {

            }
            finally
            {
                GlobalVars.IsLoading = false;
            }
        }

        private void tpNew_Click(object sender, EventArgs e)
        {
            NewProject();
        }

        private void btnEditList_Click(object sender, EventArgs e)
        {
            dgvMain.Rows.Add(5);

            for (int i = 0; i < dgvMain.Rows.Count; i++)
            {
                dgvMain.Rows[i].Cells[0].Value = "44444444444" + i.ToString();
                dgvMain.Rows[i].Cells[1].Value = "44444444444" + i.ToString();
                dgvMain.Rows[i].Cells[2].Value = "44444444444" + i.ToString();
                dgvMain.Rows[i].Cells[3].Value = "כתומה";
                dgvMain.Rows[i].Cells[4].Value = "עד 70";
                dgvMain.Rows[i].Cells[5].Value = 70;
                dgvMain.Rows[i].Cells[6].Value = "36-40";
                dgvMain.Rows[i].Cells[7].Value = "JJJJJJJJ@";
                dgvMain.Rows[i].Cells[8].Value = "0549000406";
                dgvMain.Rows[i].Cells[9].Value = "אקדמיה 1";
                dgvMain.Rows[i].Cells[10].Value = "asdasdasd";
                dgvMain.Rows[i].Cells[11].Value = "054095995955";
                dgvMain.Rows[i].Cells[12].Value = "זכר";

            }
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