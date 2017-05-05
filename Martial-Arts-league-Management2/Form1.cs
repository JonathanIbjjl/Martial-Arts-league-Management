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
            dgvColors();
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
                this.Text = "IBJJL | מערכת ניהול ליגה" + " " + GlobalVars.VerNum;
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

                // create GlobalVars.ListOfContenders again (if its excel it will be the second time) for case that user added or edited contenders via AddContenders form
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
                ReportStatiticsWasMade();
                Brackets = new BracketsBuilder(MartialArts.GlobalVars.ListOfContenders, false);
                Brackets.Init();
                this.Invoke(new Action(wClock.Dispose));
                this.Invoke(new Action(CreateVisualBrackets));
                GlobalVars.IsLoading = false;
            }
        }

        // in order to inditify contenders that was created without statistics after brackets where builed via btnBuiledBrackets button)
        private void ReportStatiticsWasMade()
        {
            for (int i = 0; i < MartialArts.GlobalVars.ListOfContenders.Count; i++)
            {
                MartialArts.GlobalVars.ListOfContenders[i].CreatedAfterBracketBuilder = false;
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
            wClock.ClockBackGroundColor = tabPage1.BackColor;
            wClock.LoadFont("ARIAL", 9, FontStyle.Bold);
            wClock.OuterCircleWeight = 10;
            wClock.InnerCircleWeight = 5;
            wClock.OuterCircleColor = Color.FromArgb(70,70,70);
            wClock.setArrayColors(new Color[] {Color.Red });

            wClock.Create(true);
            wClock.Clock.Location = new Point(0, 0);
            lblwaitClock.Controls.Add(wClock.Clock);
        }

        private void dgvColors()
        {
            dgvMain.BackgroundColor = GlobalVars.Sys_LighterGray;
            dgvMain.DefaultCellStyle.BackColor = Color.FromArgb(15,15,15);
            dgvMain.DefaultCellStyle.ForeColor = Color.FromArgb(200, 200, 200);
            dgvMain.Font = new Font("ARIAL", 8, FontStyle.Regular);
            dgvMain.ColumnHeadersDefaultCellStyle.ForeColor = Color.FromArgb(15, 15, 15); 
            dgvMain.ColumnHeadersDefaultCellStyle.BackColor = GlobalVars.Sys_Yellow;
            dgvMain.RowHeadersDefaultCellStyle.BackColor = GlobalVars.Sys_Yellow;
            dgvMain.RowHeadersDefaultCellStyle.ForeColor = Color.FromArgb(15, 15, 15);
            dgvMain.RowsDefaultCellStyle.SelectionBackColor = GlobalVars.Sys_Red;
            dgvMain.RowsDefaultCellStyle.SelectionForeColor = GlobalVars.Sys_White;
            dgvMain.ColumnHeadersDefaultCellStyle.Font = new Font("ARIAL", 9, FontStyle.Bold);
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
            this.dgvMain.RowHeadersWidth = 70;
           // dgvMain.AutoResizeColumns(DataGridViewAutoSizeColumnsMode.Fill);
            dgvMain.ReadOnly = true;
            dgvMain.AllowUserToAddRows = false;

            if (GlobalVars.ListOfContenders.Count < 1)
                return;

            if (dgvMain.Columns.Count <= 3) // only for safty
            {
                dgvMain.Columns.Add("ID", "ת.ז");
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


                dgvMain.Columns["ID"].Width = 70;
                dgvMain.Columns["HebrewBeltColor"].Width = 70;
                dgvMain.Columns["GetWeightValue"].Width = 70;
                dgvMain.Columns["Weight"].Width = 70;
                dgvMain.Columns["GetAgeValue"].Width = 70;

                dgvMain.Columns["IsMale"].Width = 50;
                dgvMain.Columns["IsAllowedVersusMan"].Width = 50;
                dgvMain.Columns["IsAllowedAgeGradeAbove"].Width = 50;
                dgvMain.Columns["IsAllowedBeltGradeAbove"].Width = 50;
                dgvMain.Columns["IsAllowedWeightGradeAbove"].Width = 50;
            }

            // add rows
            dgvMain.Rows.Add(GlobalVars.ListOfContenders.Count);
            for (int i = 0; i < GlobalVars.ListOfContenders.Count; i++)
            {
                dgvMain.Rows[i].Cells["ID"].Value = GlobalVars.ListOfContenders[i].ID;
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
                dgvMain.Rows[i].Cells[3].Style.BackColor = GlobalVars.ListOfContenders[i].GetBeltColorValue;
                if (GlobalVars.ListOfContenders[i].Belt < (int)Contenders.ContndersGeneral.BeltsEnum.blue)
                    dgvMain.Rows[i].Cells[3].Style.ForeColor = Color.Black;

                this.dgvMain.Rows[i].HeaderCell.Value = GlobalVars.ListOfContenders[i].SystemID.ToString();

            }

            dgvMain.Columns["Email"].AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;
            dgvMain.Columns["AcademyName"].AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;


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

            if (Visual.VisualLeagueEvent.AllVisualContenders == null || Visual.VisualLeagueEvent.AllVisualContenders.Count <= 0)
            {
                // objects has not been created yet
                return;
            }

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
                var SearchText = txtSearch.Text.ToString().ToLower().Trim();
                Visual.VisualLeagueEvent.Search(SearchText, out numberOfResults);
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


    

        public  void UnderConstruction()
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
            ExportBrackets export = new ExportBrackets("",GetUndoStruct());
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
            Contenders.Contender.RoleBackSystemIds();
            ClearExistingBrackets();
            DgvDefenitions();

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
            Helpers.ShowGenericPromtForm("IBJJL" + " " + GlobalVars.VerNum);
        }

        private void tpSave_Click(object sender, EventArgs e)
        {
            Save();
        }

        private bool Save()
        {
            if (GlobalVars.IsLoading == true)
                return false;



            // if it is saved project save also the changes made in the saved project
            if (GlobalVars.CurrentProject != null)
            {

                if (Visual.VisualLeagueEvent.AllVisualContenders == null || Visual.VisualLeagueEvent.AllVisualContenders.Count <= 0)
                {
                    // load only the contenders from the list
                    using (CreateContendersFromDgv createConts = new CreateContendersFromDgv(ref dgvMain))
                    {
                        if (createConts.Init() == false)
                        {
                            // something went wrong
                            GlobalVars.IsLoading = false;
                            this.Invoke(new Action(wClock.Dispose));
                            return false;
                        }
                        else
                        {
                            // serilize only the contenders
                            SerializeDataSaveAs save = new SerializeDataSaveAs(new Visual.VisualLeagueEvent.UndoStruct(), GlobalVars.CurrentProject);
                            save.Serialize(GlobalVars.ListOfContenders);
                        }
                    }
                }
                else
                {
                    // handle project path
                    SerializeDataSaveAs se = new SerializeDataSaveAs(GetUndoStruct(), GlobalVars.CurrentProject);
                    if (se.Serialize() == false)
                        return false;
                }


            }
      

                // save data to the "last saved files"
                // check if there are objects
                if (Visual.VisualLeagueEvent.AllVisualContenders == null || Visual.VisualLeagueEvent.AllVisualContenders.Count <= 0)
                {
                    // load only the contenders from the list
                    using (CreateContendersFromDgv createConts = new CreateContendersFromDgv(ref dgvMain))
                    {
                        if (createConts.Init() == false)
                        {
                            // something went wrong
                            GlobalVars.IsLoading = false;
                            this.Invoke(new Action(wClock.Dispose));
                            return false;
                        }
                        else
                        {
                            // serilize only the contenders
                            SerializeData save = new SerializeData();
                            save.Serialize(GlobalVars.ListOfContenders);
                        }
                    }
                }
                else
                {
                    SerializeData save = new SerializeData(GetUndoStruct());
                if (save.Serialize() == false)
                    return false;
                }
            

            GlobalVars.IsLoading = false;
            return true;
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

                // if null that means that only contenders from list was saved and the visual brackets was not created
                if (b.ContendersList == null)
                {
                    // load onlt DGVmain 
                    List<Contenders.Contender> cont;
                    load.DeSerialize(out Isok,out cont);
                    if (Isok)
                    {
                        // load dgvmain
                        GlobalVars.ListOfContenders = cont;
                        LoadDgv();
                    }
                    GlobalVars.ListOfContenders.Clear();
                    GlobalVars.IsLoading = false;
                    return;
                }

                // load saved contenders to list
                GlobalVars.ListOfContenders = b.ContendersFromDgvMain;
                // load contenders to dgv
                LoadDgv();
      
           
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

        private void tpSaveAs_Click(object sender, EventArgs e)
        {
            if (GlobalVars.IsLoading == true)
                return;

          
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

                // create all files paths instance immideatly if the user will use save() method
                MartialArts.ProjectsSavedAsBinaryFiles paths = new ProjectsSavedAsBinaryFiles(promt.ProjectName);
                paths.SetFullDirWithoutCreating();
                GlobalVars.CurrentProject = paths;
                header(promt.ProjectName);
                if (Save() == false)
                {
                 // something went wrong the file was not upload
                    GlobalVars.CurrentProject = null;
                    header();
                }
            }

            promt.Dispose();
        }

        private void tpOpenProject_Click(object sender, EventArgs e)
        {
            LoadProject();
        }

        private void LoadProject()
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
                
                // if null that means that only contenders from list was saved and the visual brackets was not created
                if (b.ContendersList == null)
                {
                    // load onlt DGVmain 
                    List<Contenders.Contender> cont;
                    load.DeSerialize(out Isok, out cont);
                    if (Isok)
                    {
                        // load dgvmain
                        GlobalVars.ListOfContenders = cont;
                        LoadDgv();
                    }

                    GlobalVars.ListOfContenders.Clear();
                    GlobalVars.IsLoading = false;
                    // set current project
                    GlobalVars.CurrentProject = paths;
                    // set header with project name
                    header(choose.ProjectName);
                    return;
                }

                // load saved contenders to list
                GlobalVars.ListOfContenders = b.ContendersFromDgvMain;
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

      

        private void listBox1_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void groupBox2_Enter(object sender, EventArgs e)
        {

        }

        private void btnFW_Click(object sender, EventArgs e)
        {
            
        }

        private void radioAsc_CheckedChanged(object sender, EventArgs e)
        {
            SortCheckedChanged();
        }

        private void radioDesc_CheckedChanged(object sender, EventArgs e)
        {
            SortCheckedChanged();
        }

        private void SortCheckedChanged()
        {
            if (radioAsc.Checked == true)
            {
                btnAsc.Visible = true;
                btnAsc.Enabled = true;
                btnDesc.Visible = false;
                btnDesc.Enabled = false;
            }
            else
            {
                btnAsc.Visible = false;
                btnAsc.Enabled = false;
                btnDesc.Visible = true;
                btnDesc.Enabled = true;
            }
        }

        private void btnAsc_Click(object sender, EventArgs e)
        {

            if (Visual.VisualLeagueEvent.AllVisualContenders == null || Visual.VisualLeagueEvent.AllVisualContenders.Count <= 0)
            {
                // objects has not been created yet
                return;
            }

            sortVisualBrackets(false);
        }

        private void btnDesc_Click(object sender, EventArgs e)
        {
            sortVisualBrackets(true);
        }

        private void sortVisualBrackets(bool Desc)
        {
            if (radSortAvgGrade.Checked == true)
            {
                SortingMethods.SortByAvgGrade(this, Desc);
            }
            else if (radSortWeight.Checked == true)
            {
                SortingMethods.SortByWeight(this, Desc);
            }

            else if (radSortAge.Checked == true)
            {
                SortingMethods.SortByAge(this, Desc);
            }
        }

        private void radSortAge_CheckedChanged(object sender, EventArgs e)
        {
            if (radSortAge.Checked == true)
            {
                radSortAge.ForeColor = Color.White;
            }
            else
            {
                radSortAge.ForeColor = Color.FromArgb(200,200,200);
            }
        }

        private void radSortWeight_CheckedChanged(object sender, EventArgs e)
        {
            if (radSortWeight.Checked == true)
            {
                radSortWeight.ForeColor = Color.White;
            }
            else
            {
                radSortWeight.ForeColor = Color.FromArgb(200, 200, 200);
            }
        }

        private void radSortAvgGrade_CheckedChanged(object sender, EventArgs e)
        {
            if (radSortAvgGrade.Checked == true)
            {
                radSortAvgGrade.ForeColor = Color.White;
            }
            else
            {
                radSortAvgGrade.ForeColor = Color.FromArgb(200, 200, 200);
            }
        }

        private void btnRemoveAllMarks_Click(object sender, EventArgs e)
        {
            
            if (Visual.VisualLeagueEvent.AllVisualContenders == null || Visual.VisualLeagueEvent.AllVisualContenders.Count <= 0)
            {
                // objects has not been created yet
                return;
            }

            foreach (Visual.VisualContender vc in Visual.VisualLeagueEvent.AllVisualContenders)
            {
                vc.IsMarked = false;
            }
        }

        private void BracktsFPanel_MouseEnter(object sender, EventArgs e)
        {
            if (Martial_Arts_league_Management2.PromtForm.FormIsShown == false)
            {
                BracktsFPanel.Focus();
            }
        }

        private void UnPlacedFpanel_MouseEnter(object sender, EventArgs e)
        {
            if (Martial_Arts_league_Management2.PromtForm.FormIsShown == false)
            {
                UnPlacedFpanel.Focus();
            }
        }


        // the GUI is not stuck
        private void btnSave_Click(object sender, EventArgs e)
        {

            if (Visual.VisualLeagueEvent.AllVisualContenders == null || Visual.VisualLeagueEvent.AllVisualContenders.Count <= 0)
            {
             // objects has not been created yet
                return;
            }

            Save();
            System.Threading.Thread t1 = new System.Threading.Thread(SaveAnimationThread);
            t1.Start();
        }

        private void SaveAnimationThread()
        {
            btnSave.Invoke(new Action(AnimateSave));
            System.Threading.Thread.Sleep(1000);
            btnSave.BackColor = btnLoad.BackColor;
        }

        private void AnimateSave()
        {
            btnSave.BackColor = GlobalVars.Sys_Red;
            Application.DoEvents();
        }

        private void btnLoad_Click(object sender, EventArgs e)
        {
            LoadProject();
        }

        private void btnExpandAll_Click(object sender, EventArgs e)
        {

            if (Visual.VisualLeagueEvent.AllVisualContenders == null || Visual.VisualLeagueEvent.AllVisualContenders.Count <= 0)
            {
                // objects has not been created yet
                return;
            }

            foreach (Visual.VisualBracket vb in Visual.VisualLeagueEvent.VisualBracketsList)
            {
                vb.Expand();
            }
        }

        private void btnCollapseAll_Click(object sender, EventArgs e)
        {

            if (Visual.VisualLeagueEvent.AllVisualContenders == null || Visual.VisualLeagueEvent.AllVisualContenders.Count <= 0)
            {
                // objects has not been created yet
                return;
            }

                foreach (Visual.VisualBracket vb in Visual.VisualLeagueEvent.VisualBracketsList)
                {
                    vb.Hide();
                }
        }

        private void pictureBox2_MouseEnter(object sender, EventArgs e)
        {

        }

        private void button1_Click_2(object sender, EventArgs e)
        {
            // extract academies
            if (Visual.VisualLeagueEvent.IsBracketsAndEventCreated == true)
            {
                if (Helpers.PromtYesNowQuestion("הבתים של האירוע כבר נוצרו, במידה ותיצור מתחרה חדש לא יחושבו עבורו מדדים סטטיסטיים ותידרש לשבץ אותו באופן ידני, האם להמשיך?") == false)
                    return;

                var academies = Visual.VisualLeagueEvent.AllVisualContenders.AsEnumerable().Select(x => x.Contender.AcademyName).Distinct().OrderBy(x => x).ToList();
                // set identity system id to be bigger then the max system id
                var max = GetNextAvilableSysIdNumFromDGV();
                Contenders.Contender.IdentityNumber = max;

                using (AddContender a = new AddContender((radAdult.Checked == true ? false : true), academies))
                {
                    if (a.ShowDialog() == DialogResult.OK)
                    {
                        AddNewContenderToDgv(a.NewContender);
                        // add visual contender to UnPlacedFpanel panel
                        // create visual contender
                        Visual.VisualContender newVisContender = new Visual.VisualContender(a.NewContender);
                        newVisContender.Init();
                        // add to VisualLeagueEvent
                        Visual.VisualLeagueEvent.AllVisualContenders.Add(newVisContender);
                        // add to unplaced list
                        Visual.VisualLeagueEvent.AddUnplacedContender(newVisContender);
                        // add to panel
                        UnPlacedFpanel.Controls.Add(newVisContender.Vcontender);
                    }
                }
            }
        }

        private void btnAddContenderToDgv_Click(object sender, EventArgs e)
        {
            var academies = dgvMain.Rows.Cast<DataGridViewRow>()
                           .Where(x => !x.IsNewRow)                 
                           .Where(x => x.Cells["AcademyName"].Value != null) 
                           .Select(x => x.Cells["AcademyName"].Value.ToString())
                           .Distinct()
                           .ToList();

            using (AddContender a = new AddContender((radAdult.Checked == true ? false : true), academies))
            {
                // set identity system id to be bigger then the max system id
                var max = GetNextAvilableSysIdNumFromDGV();
                Contenders.Contender.IdentityNumber = max;

                if (a.ShowDialog() == DialogResult.OK)
                {
                    AddNewContenderToDgv(a.NewContender);
                }
            }
        }

        public int GetNextAvilableSysIdNumFromDGV()
        {
            var nextID = 0;

            if (dgvMain != null & dgvMain.RowCount > 0)
            {
                for (int i = 0; i < dgvMain.RowCount; i++)
                {
                    if (dgvMain.Rows[i].HeaderCell.Value != null && dgvMain.Rows[i].HeaderCell.Value.ToString().IsNumeric())
                    {
                        if (Int32.Parse(dgvMain.Rows[i].HeaderCell.Value.ToString()) > nextID)
                        {
                            nextID = Int32.Parse(dgvMain.Rows[i].HeaderCell.Value.ToString());
                        } 
                    }
                }
            }

            nextID+=1;
            return nextID;
        }

        public void AddNewContenderToDgv(Contenders.Contender contender)
        {
            dgvMain.Rows.Add(1);
            var i = dgvMain.Rows.Count - 1;

            dgvMain.Rows[i].Cells["ID"].Value = contender.ID;
            dgvMain.Rows[i].Cells["FirstName"].Value = contender.FirstName;
            dgvMain.Rows[i].Cells["LastName"].Value = contender.LastName;

            dgvMain.Rows[i].Cells["HebrewBeltColor"].Value = contender.HebrewBeltColor;

            dgvMain.Rows[i].Cells["GetWeightValue"].Value = contender.GetWeightValue;
            dgvMain.Rows[i].Cells["Weight"].Value = contender.Weight;
            dgvMain.Rows[i].Cells["GetAgeValue"].Value = contender.GetAgeValue;
            dgvMain.Rows[i].Cells["Email"].Value = contender.Email;
            dgvMain.Rows[i].Cells["PhoneNumber"].Value = contender.PhoneNumber;
            dgvMain.Rows[i].Cells["AcademyName"].Value = contender.AcademyName;
            dgvMain.Rows[i].Cells["CoachName"].Value = contender.CoachName;
            dgvMain.Rows[i].Cells["CoachPhone"].Value = contender.CoachPhone;

            if (contender.IsMale == true)
                dgvMain.Rows[i].Cells["IsMale"].Value = "זכר";
            else
                dgvMain.Rows[i].Cells["IsMale"].Value = "נקבה";

            if (contender.IsMale == false && contender.IsAllowedVersusMan == true)
                dgvMain.Rows[i].Cells["IsAllowedVersusMan"].Value = "כן";
            else
                dgvMain.Rows[i].Cells["IsAllowedVersusMan"].Value = "לא";

            dgvMain.Rows[i].Cells["IsAllowedAgeGradeAbove"].Value = (contender.IsAllowedAgeGradeAbove == true) ? "כן" : "לא";
            dgvMain.Rows[i].Cells["IsAllowedBeltGradeAbove"].Value = (contender.IsAllowedBeltGradeAbove == true) ? "כן" : "לא";
            dgvMain.Rows[i].Cells["IsAllowedWeightGradeAbove"].Value = (contender.IsAllowedWeightGradeAbove == true) ? "כן" : "לא";

            // color
            dgvMain.Rows[i].Cells[3].Style.BackColor = contender.GetBeltColorValue;
            if (contender.Belt < (int)Contenders.ContndersGeneral.BeltsEnum.blue)
                dgvMain.Rows[i].Cells[3].Style.ForeColor = Color.Black;

            this.dgvMain.Rows[i].HeaderCell.Value = contender.SystemID.ToString();

        }

        public void EditExistingContender(Contenders.Contender contender,int i)
        {


            dgvMain.Rows[i].Cells["ID"].Value = contender.ID;
            dgvMain.Rows[i].Cells["FirstName"].Value = contender.FirstName;
            dgvMain.Rows[i].Cells["LastName"].Value = contender.LastName;

            dgvMain.Rows[i].Cells["HebrewBeltColor"].Value = contender.HebrewBeltColor;

            dgvMain.Rows[i].Cells["GetWeightValue"].Value = contender.GetWeightValue;
            dgvMain.Rows[i].Cells["Weight"].Value = contender.Weight;
            dgvMain.Rows[i].Cells["GetAgeValue"].Value = contender.GetAgeValue;
            dgvMain.Rows[i].Cells["Email"].Value = contender.Email;
            dgvMain.Rows[i].Cells["PhoneNumber"].Value = contender.PhoneNumber;
            dgvMain.Rows[i].Cells["AcademyName"].Value = contender.AcademyName;
            dgvMain.Rows[i].Cells["CoachName"].Value = contender.CoachName;
            dgvMain.Rows[i].Cells["CoachPhone"].Value = contender.CoachPhone;

            if (contender.IsMale == true)
                dgvMain.Rows[i].Cells["IsMale"].Value = "זכר";
            else
                dgvMain.Rows[i].Cells["IsMale"].Value = "נקבה";

            if (contender.IsMale == false && contender.IsAllowedVersusMan == true)
                dgvMain.Rows[i].Cells["IsAllowedVersusMan"].Value = "כן";
            else
                dgvMain.Rows[i].Cells["IsAllowedVersusMan"].Value = "לא";

            dgvMain.Rows[i].Cells["IsAllowedAgeGradeAbove"].Value = (contender.IsAllowedAgeGradeAbove == true) ? "כן" : "לא";
            dgvMain.Rows[i].Cells["IsAllowedBeltGradeAbove"].Value = (contender.IsAllowedBeltGradeAbove == true) ? "כן" : "לא";
            dgvMain.Rows[i].Cells["IsAllowedWeightGradeAbove"].Value = (contender.IsAllowedWeightGradeAbove == true) ? "כן" : "לא";

            // color
            dgvMain.Rows[i].Cells[3].Style.BackColor = contender.GetBeltColorValue;
            if (contender.Belt < (int)Contenders.ContndersGeneral.BeltsEnum.blue)
                dgvMain.Rows[i].Cells[3].Style.ForeColor = Color.Black;

            this.dgvMain.Rows[i].HeaderCell.Value = contender.SystemID.ToString();

        }

        public List<Contenders.Contender> GetContendersFromDgv()
        {
            
            using (CreateContendersFromDgv createConts = new CreateContendersFromDgv(ref dgvMain))
            {
                if (createConts.Init() == false)
                {
                    return GlobalVars.ListOfContenders;
                }
                else
                {
                    return GlobalVars.ListOfContenders;
                }
            }
        }

        public Visual.VisualLeagueEvent.UndoStruct GetUndoStruct()
        {
            var undo = Visual.VisualLeagueEvent.GetUndoStruct();
            // add the data from DGV
            undo.ContendersInsideDgvMain = GetContendersFromDgv();
            return undo;
        }
        private void StripEditCont_Click(object sender, EventArgs e)
        {
            try
            {
                // row index 
                if ((this.dgvMain.CurrentRow == null))
                {
                    //No row selected
                    Helpers.ShowGenericPromtForm("לא נבחר מתחרה ברשימה");
                    return;
                }
                else
                {
                    var academies = dgvMain.Rows.Cast<DataGridViewRow>()
                           .Where(x => !x.IsNewRow)
                           .Where(x => x.Cells["AcademyName"].Value != null)
                           .Select(x => x.Cells["AcademyName"].Value.ToString())
                           .Distinct()
                           .ToList();

                    CreateContendersFromDgv ce = new CreateContendersFromDgv(ref dgvMain);

                    using (AddContender a = new AddContender((radAdult.Checked == true ? false : true), academies,ce.GetContenderFromDGV(dgvMain.CurrentRow.Index)))
                    {
                        if (a.ShowDialog() == DialogResult.OK)
                        {
                            EditExistingContender(a.NewContender,dgvMain.CurrentRow.Index);
                        }
                    }
                }
            }



            catch
            {

            }
        }

        private void StripDeleteContender_Click(object sender, EventArgs e)
        {
            if ((this.dgvMain.CurrentRow == null))
            {
                //No row selected
                Helpers.ShowGenericPromtForm("לא נבחר מתחרה ברשימה");
                return;
            }

            var name = dgvMain.Rows[dgvMain.CurrentRow.Index].Cells["FirstName"].Value.ToString() + " " + dgvMain.Rows[dgvMain.CurrentRow.Index].Cells["LastName"].Value.ToString();
            if (Helpers.PromtYesNowQuestion("האם אתה בטוח שברצונך להסיר את " + name + " " + "מהרשימה?") == true)
            {
                dgvMain.Rows.RemoveAt(dgvMain.CurrentRow.Index);
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