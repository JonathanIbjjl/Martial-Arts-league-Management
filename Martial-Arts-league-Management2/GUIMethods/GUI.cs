using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BusinessClocks.ExecutiveClocks;
using System.Drawing;
using System.Windows.Forms;

namespace MartialArts
{
    public partial class Form1
    {

        private BusinessClocks.ExecutiveClocks.GoalsClock _MatchClock;
        private BusinessClocks.ExecutiveClocks.GoalsClock _MatchWithoutUselessClock;
        private BusinessClocks.ExecutiveClocks.GoalsClock _BracketsClock;
  
        public void CreateVisualBrackets()
        {
            Visual.VisualLeagueEvent.FormObj = this;
            try
            {
                Cursor.Hide();

                // sort by Bracket grade
                Brackets.BracketsList = Brackets.BracketsList.AsEnumerable().OrderBy(x => x.AverageGrade).ToList();

                foreach (MartialArts.Bracket b in Brackets.BracketsList)
                {

                    Visual.VisualBracket br = new Visual.VisualBracket(b);
                    br.Init();
                    Visual.VisualLeagueEvent.AddVisualBracket(br);
                    // add to GUI
                    BracktsFPanel.Controls.Add(br.Vbracket);
                }

                // add uselesess and unplaced contenders

                // unplaced
                foreach (Contenders.Contender c in Brackets.ContendersList)
                {
                    Visual.VisualContender visualcont = new Visual.VisualContender(c);
                    visualcont.Init();
                    Visual.VisualLeagueEvent.AddUnplacedContender(visualcont);
                    UnPlacedFpanel.Controls.Add(visualcont.Vcontender);
                }

                // Uselesses
                foreach (Contenders.Contender c in Brackets.UselessContenders)
                {
                    Visual.VisualContender visualcont = new Visual.VisualContender(c);
                    visualcont.Init();
                    Visual.VisualLeagueEvent.AddUnplacedContender(visualcont);
                    UnPlacedFpanel.Controls.Add(visualcont.Vcontender);
                }

                // must merge all contenders in LeagueEvent instance
                Visual.VisualLeagueEvent.MergeListsForSearch();
                UpdateClocks();

                System.Threading.Thread.Sleep(1000);
                tabControl1.SelectedTab = tabPage2;
            }
            catch (Exception ex)
            {

            }
            finally
            {
                Cursor.Show();
                MoveCursor();
            }
          
        }

        /// <summary>
        /// for saved data only
        /// </summary>
        /// <param name="savedData"></param>
        private void CreateVisualBrackets(Visual.VisualLeagueEvent.UndoStruct savedData)
        {
            Visual.VisualLeagueEvent.FormObj = this;
            try
            {
                Cursor.Hide();


                foreach (Visual.VisualBracket b in savedData._VisualBracketsList)
                {
                    // add to GUI
                    BracktsFPanel.Controls.Add(b.Vbracket);
                }

                // Uselesses / unplaced
                foreach (Visual.VisualContender c in savedData._VisualUnplacedBracketsList)
                {
                    UnPlacedFpanel.Controls.Add(c.Vcontender);
                }

                // must merge all contenders in LeagueEvent instance
                Visual.VisualLeagueEvent.MergeListsForSearch();
                UpdateClocks();

                System.Threading.Thread.Sleep(1000);
                tabControl1.SelectedTab = tabPage2;
            }
            catch (Exception ex)
            {

            }
            finally
            {
                Cursor.Show();
                MoveCursor();
            }

        }

        public void UpdateClocks(bool FirstLoadWithoutPercent = false)
        {
            UpdateStatisticClocks(FirstLoadWithoutPercent);
            UpdateNetoClock(FirstLoadWithoutPercent);
            UpdateBracketsClock(FirstLoadWithoutPercent);
        }

        private void UpdateStatisticClocks(bool FirstLoadWithoutPercent = false)
        {
            float Percent = 0;
            if (FirstLoadWithoutPercent == false)
            {
                int ContendersWhithBracket = Visual.VisualLeagueEvent.VisualBracketsList.SelectMany(x => x.Bracket.ContendersList).Count();
                int AllConts = ContendersWhithBracket + Visual.VisualLeagueEvent.VisualUnplacedBracketsList.Select(x => x).Count();
                Percent = (float)ContendersWhithBracket / (float)AllConts;

                // statistics lables
                lblPlacedContsCount.Text = ContendersWhithBracket.ToString().PadLeft(3, '0');
                lblAllContsCount.Text = AllConts.ToString().PadLeft(3, '0');

                int allUnplacedConts = Visual.VisualLeagueEvent.VisualUnplacedBracketsList.Select(x => x).Count();
                int Useless = Visual.VisualLeagueEvent.VisualUnplacedBracketsList.Where(x => x.Contender.IsUseless == true).Count();

                lblIsUselessContsCount.Text = Useless.ToString().PadLeft(3, '0');

                if (Useless > allUnplacedConts)
                    lblAllUnplacedContsCount.Text = (Useless - allUnplacedConts).ToString().PadLeft(3, '0');
                else
                    lblAllUnplacedContsCount.Text = (allUnplacedConts - Useless).ToString().PadLeft(3, '0');
            }

            if (_MatchClock == null)
            {
                _MatchClock = new GoalsClock(70, 70, Percent);
                _MatchClock.OuterCircleWeight = 10;
                _MatchClock.InnerCircleWeight = 5;
                _MatchClock.InnerCircleColor = GlobalVars.Sys_Yellow;
                _MatchClock.OuterCircleColor = Color.FromArgb(78, 78, 78);
                _MatchClock.FontColor = GlobalVars.Sys_LabelGray;
                _MatchClock.ClockBackGroundColor = panelStatistics.BackColor;
                _MatchClock.Create(false);
                _MatchClock.Clock.Anchor = AnchorStyles.Right | AnchorStyles.Top;

                _MatchClock.Clock.Location = new Point(0, 0);
                lblPercent.Controls.Add(_MatchClock.Clock);
                toolTip1.SetToolTip(_MatchClock.Clock, "אחוז המתחרים המשובצים מתוך כלל המשתתפים");
            }
            else
            {
                _MatchClock.PercentOfGoals = Percent;
                _MatchClock.RefreshClock(true);
            }

           
        }

        private void UpdateNetoClock(bool FirstLoadWithoutPercent = false)
        {

            float Percent = 0;
            if (FirstLoadWithoutPercent == false)
            {
                int ContendersWhithBracket = Visual.VisualLeagueEvent.VisualBracketsList.SelectMany(x => x.Bracket.ContendersList).Count();
                int AllContsMinusUseless = (ContendersWhithBracket + Visual.VisualLeagueEvent.VisualUnplacedBracketsList.Select(x => x).Count())
                    - Visual.VisualLeagueEvent.VisualUnplacedBracketsList.Where(x => x.Contender.IsUseless == true).Count();

                Percent = (float)ContendersWhithBracket / (float)AllContsMinusUseless;
            }
         
            if (_MatchWithoutUselessClock == null)
            {
                _MatchWithoutUselessClock = new GoalsClock(70, 70, Percent);
                _MatchWithoutUselessClock.OuterCircleWeight = 10;
                _MatchWithoutUselessClock.InnerCircleWeight = 5;
                _MatchWithoutUselessClock.InnerCircleColor = GlobalVars.Sys_Yellow;
                _MatchWithoutUselessClock.OuterCircleColor = Color.FromArgb(78, 78, 78);
                _MatchWithoutUselessClock.FontColor = GlobalVars.Sys_LabelGray;
                _MatchWithoutUselessClock.ClockBackGroundColor = panelStatistics.BackColor;
                _MatchWithoutUselessClock.Create(false);
                _MatchWithoutUselessClock.Clock.Anchor = AnchorStyles.Right | AnchorStyles.Top;

                _MatchWithoutUselessClock.Clock.Location = new Point(0, 0);
                lblPercentwithoutUseless.Controls.Add(_MatchWithoutUselessClock.Clock);
                toolTip1.SetToolTip(_MatchWithoutUselessClock.Clock, "אחוז המתחרים המשובצים מתוך כלל המשתתפים למעט משתתפים ללא התאמה לאף מתחרה באירוע הנוכחי");
            }
            else
            {
                _MatchWithoutUselessClock.PercentOfGoals = Percent;
                _MatchWithoutUselessClock.RefreshClock(true);
            }
        }

        private void UpdateBracketsClock(bool FirstLoadWithoutPercent = false)
        {
            float Percent = 0;
            if (FirstLoadWithoutPercent == false)
            {
                int NumberOf_N_Brackets = Visual.VisualLeagueEvent.VisualBracketsList.Where(x => x.Bracket.NumberOfContenders >= GeneralBracket.NumberOfContenders).Count();
                int NumberOfBrackets = Visual.VisualLeagueEvent.VisualBracketsList.Select(x => x.Bracket).Count();
                Percent = (float)(NumberOf_N_Brackets) / (float)NumberOfBrackets;
            }

            if (_BracketsClock == null)
            {
                _BracketsClock = new GoalsClock(70, 70, Percent);
                _BracketsClock.OuterCircleWeight = 10;
                _BracketsClock.InnerCircleWeight = 5;
                _BracketsClock.InnerCircleColor = GlobalVars.Sys_Yellow;
                _BracketsClock.OuterCircleColor = Color.FromArgb(78, 78, 78);
                _BracketsClock.FontColor = GlobalVars.Sys_LabelGray;
                _BracketsClock.ClockBackGroundColor = panelStatistics.BackColor;
                _BracketsClock.Create(false);
                _BracketsClock.Clock.Anchor = AnchorStyles.Right | AnchorStyles.Top;
                _BracketsClock.Clock.Location = new Point(0, 0);
                lblBracketsClock.Controls.Add(_BracketsClock.Clock);
                toolTip1.SetToolTip(_BracketsClock.Clock, "אחוז הבתים ששווים או גבוהים ממספר המשתתפים המבוקש לבית");
            }
            else
            {
                _BracketsClock.PercentOfGoals = Percent;
                _BracketsClock.RefreshClock(true);
            }
        }


        #region "Custom Dgv For DataEntering"

        private void DgvExample()
        {

            string[] names = { "Carlson", "Robson", "Reyson", "Carley",
                "Rolls", "Carlos", "Rorion", "Relson", "Rickson", "Royler",
                "Royce", "Clark", "Daniel", "Kron", "Ralek", "Ralph", "Rener",
                "Renzo", "Reyson", "Rodrigo", "Roger", "Rolles", "Ryan", "Cesar",
                "Rodrigo", "Roger", "Rolles", "Ryan", "Cesar", "Carlson", "Robson", "Reyson", "Carley" , "Robson", "Reyson", "Carley",
                "Rolls", "Carlos", "Rorion", "Relson", "Rickson", "Royler",
                "Royce", "Clark", "Daniel", "Kron", "Ralek", "Ralph", "Rener",
                "Renzo", "Reyson", "Rodrigo", "Roger", "Rolles", "Ryan", "Cesar",
                "Rodrigo", "Roger", "Rolles", "Ryan", "Cesar", "Carlson", "Robson", "Reyson", "Carley"};

            string[] belts = { "לבנה", "לבנה", "לבנה", "לבנה",
                "לבנה", "אפורה", "אפורה", "אפורה", "אפורה", "צהובה",
                "צהובה", "צהובה", "צהובה", "כתומה", "כתומה", "כתומה", "כתומה",
                "ירוקה", "ירוקה", "ירוקה", "ירוקה", "כחולה", "כחולה", "כחולה",
                "כחולה", "סגולה", "סגולה", "סגולה", "חומה", "חומה", "חומה", "שחורה", "שחורה", "לבנה", "לבנה", "לבנה",
                "לבנה", "אפורה", "אפורה", "אפורה", "אפורה", "צהובה",
                "צהובה", "צהובה", "צהובה", "כתומה", "כתומה", "כתומה", "כתומה",
                "ירוקה", "ירוקה", "ירוקה", "ירוקה", "כחולה", "כחולה", "כחולה",
                "כחולה", "סגולה", "סגולה", "סגולה", "חומה", "חומה", "חומה", "שחורה", "שחורה" };

            dgvMain.Rows.Add(names.Length);
            Random r = new Random();
            for (int i = 0; i < names.Length; i++)
            {
                dgvMain.Rows[i].Cells[0].Value = "040364487";
                dgvMain.Rows[i].Cells[1].Value = names[i];
                dgvMain.Rows[i].Cells[2].Value = "Gracie";
                dgvMain.Rows[i].Cells[3].Value = belts[i];

                dgvMain.Rows[i].Cells[4].Value = Contenders.ContndersGeneral.AdultWeightCat.Keys.ToList()[r.Next(2, 8)].ToString();
                dgvMain.Rows[i].Cells[5].Value = Helpers.extractNumberFromString(dgvMain.Rows[i].Cells[4].Value.ToString());
                dgvMain.Rows[i].Cells[6].Value = "36-40";
                dgvMain.Rows[i].Cells[7].Value = "Gracie@IBJJL.COM";
                dgvMain.Rows[i].Cells[8].Value = "05488888888";
                dgvMain.Rows[i].Cells[9].Value = "אקדמיה" + " " + (i + 1).ToString();
                dgvMain.Rows[i].Cells[10].Value = "משה רובינוב";
                dgvMain.Rows[i].Cells[11].Value = "05488888888";
                dgvMain.Rows[i].Cells[12].Value = "זכר";
                dgvMain.Rows[i].Cells[13].Value = "לא";
                dgvMain.Rows[i].Cells[14].Value =  (r.Next(2))== 0 ? "לא": "כן";
                dgvMain.Rows[i].Cells[15].Value = (r.Next(2)) == 0 ? "לא" : "כן";
                dgvMain.Rows[i].Cells[16].Value = (r.Next(2)) == 0 ? "לא" : "כן";



                this.dgvMain.Rows[i].HeaderCell.Value = (i + 1).ToString();

            }

            ExampleListIsPresented = true;
        }

        private void DgvDefenitions()
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
            this.dgvMain.RowHeadersWidth = 70;
            dgvMain.AutoResizeColumns(DataGridViewAutoSizeColumnsMode.Fill);
            dgvMain.ReadOnly = true;
            dgvMain.AllowUserToAddRows = false;

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
            
            // add click event
            dgvMain.MouseDown += new MouseEventHandler(dgv_Click);
        }

        private void dgv_Click(object sender, MouseEventArgs e)
        {
            try
            {

                if (e.Button == MouseButtons.Right)
                {
                    contextMenuStrip1.Show(dgvMain,new Point(e.X,e.Y));
                }
            }
            catch { }
        }

        // TODO: DELETE 
        //void DgvDefenitions()
        //{
        //    if (dgvMain.Rows.Count > 0)
        //    {
        //        dgvMain.Rows.Clear();             
        //    }

        //    if (dgvMain.Columns.Count > 0)
        //    {
        //        dgvMain.Columns.Clear();
        //    }



        //    dgvMain.DoubleBuffered(true);
        //    dgvMain.EnableHeadersVisualStyles = false;
        //    EndVer();
        //    this.dgvMain.RowHeadersWidth = 70;

        //    dgvMain.SelectionMode = DataGridViewSelectionMode.RowHeaderSelect;


        //    dgvMain.ReadOnly = false;
        //    dgvMain.AllowUserToAddRows = true;
        //    dgvMain.AllowUserToDeleteRows = true;
        //    // dgvMain.AutoResizeColumns(DataGridViewAutoSizeColumnsMode.Fill);

        //    this.dgvMain.CellValidating += new
        //      DataGridViewCellValidatingEventHandler(dgvMain_CellValidating);
        //      this.dgvMain.CellEndEdit += new
        //      DataGridViewCellEventHandler(dgvMain_CellEndEdit);


        //    // 
        //    // Column0
        //    //
        //    DataGridViewTextBoxColumn ID = new DataGridViewTextBoxColumn();
        //    ID.HeaderText = "ת.ז";
        //    ID.Name = "ID";
        //    // 
        //    // Column1
        //    //
        //    DataGridViewTextBoxColumn FirstName = new DataGridViewTextBoxColumn();
        //    FirstName.HeaderText = "שם פרטי";
        //    FirstName.Name = "FirstName";
        //    // 
        //    // Column2
        //    //
        //    DataGridViewTextBoxColumn LastName = new DataGridViewTextBoxColumn();
        //    LastName.HeaderText = "שם משפחה";
        //    LastName.Name = "LastName";
        //    // 
        //    // Column3
        //    //
        //    DataGridViewComboBoxColumn Belt = new DataGridViewComboBoxColumn();
        //    var beltList = new List<string>() { "לבנה", "אפורה", "צהובה", "כתומה","ירוקה","כחולה","סגולה","חומה" ,"שחורה"};
        //    Belt.DataSource = beltList;
        //    Belt.HeaderText = "חגורה";
        //    Belt.Name = "Belt";
        //    Belt.FlatStyle = FlatStyle.Flat;
        //    // 
        //    // Column4
        //    //
        //    DataGridViewComboBoxColumn weightCat = new DataGridViewComboBoxColumn();
        //    var weightList = new List<string>();
        //    if(radChild.Checked == true)
        //    weightList = Contenders.ContndersGeneral.ChildWeightCat.Keys.ToList();
        //    else
        //    weightList = Contenders.ContndersGeneral.AdultWeightCat.Keys.ToList();
        //    weightCat.DataSource = weightList;
        //    weightCat.HeaderText = "קטגוריית משקל";
        //    weightCat.Name = "weightCat";
        //    weightCat.FlatStyle = FlatStyle.Flat;
        //    // 
        //    // Column5
        //    //
        //    DataGridViewTextBoxColumn weight = new DataGridViewTextBoxColumn();
        //    weight.HeaderText = "משקל מדוייק";
        //    weight.Name = "weight";
        //    // 
        //    // Column6
        //    //
        //    DataGridViewComboBoxColumn age = new DataGridViewComboBoxColumn();
        //    var ageList = new List<string>();
        //    ageList = Contenders.ContndersGeneral.GetAgeValues(radChild.Checked);
        //    age.DataSource = ageList;
        //    age.HeaderText = "קטגוריית גיל";
        //    age.Name = "ageCat";
        //    age.FlatStyle = FlatStyle.Flat;
        //    // 
        //    // Column7
        //    //
        //    DataGridViewTextBoxColumn email = new DataGridViewTextBoxColumn();
        //    email.HeaderText = "אימייל";
        //    email.Name = "Email";
        //    // 
        //    // Column8
        //    //
        //    DataGridViewTextBoxColumn phone = new DataGridViewTextBoxColumn();
        //    phone.HeaderText = "טלפון";
        //    phone.Name = "phone";
        //    // 
        //    // Column9
        //    //
        //    DataGridViewComboBoxColumn AcademyName = new DataGridViewComboBoxColumn();
        //    var academyList = new List<string>();
        //    for (int i = 1; i < 100; i++)
        //        academyList.Add("אקדמיה" + " " + i.ToString());
        //    AcademyName.DataSource = academyList;
        //    AcademyName.HeaderText = "אקדמיה";
        //    AcademyName.Name = "AcademyName";
        //    AcademyName.FlatStyle = FlatStyle.Flat;
        //    // 
        //    // Column10
        //    //
        //    DataGridViewTextBoxColumn coach = new DataGridViewTextBoxColumn();
        //    coach.HeaderText = "שם מאמן";
        //    coach.Name = "coach";
        //    // 
        //    // Column11
        //    //
        //    DataGridViewTextBoxColumn coachPhone = new DataGridViewTextBoxColumn();
        //    coachPhone.HeaderText = "טלפון מאמן";
        //    coachPhone.Name = "coachPhone";
        //    // 
        //    // Column12
        //    //
        //    DataGridViewComboBoxColumn gender = new DataGridViewComboBoxColumn();
        //    var genderList = new List<string>() { "זכר","נקבה"};
        //    gender.DataSource = genderList;
        //    gender.HeaderText = "מגדר";
        //    gender.Name = "gender";
        //    gender.FlatStyle = FlatStyle.Flat;
        //    // 
        //    // Column12
        //    //
        //    DataGridViewCheckBoxColumn IsAllowedVersusMan = new DataGridViewCheckBoxColumn();
        //    IsAllowedVersusMan.HeaderText = "פקטור מגדר";
        //    IsAllowedVersusMan.Name = "IsAllowedVersusMan";
        //    // 
        //    // Column13
        //    //
        //    DataGridViewCheckBoxColumn IsAllowedAgeGradeAbove = new DataGridViewCheckBoxColumn();
        //    IsAllowedAgeGradeAbove.HeaderText = "פקטור גיל";
        //    IsAllowedAgeGradeAbove.Name = "IsAllowedAgeGradeAbove";
        //    // 
        //    // Column14
        //    //
        //    DataGridViewCheckBoxColumn IsAllowedBeltGradeAbove = new DataGridViewCheckBoxColumn();
        //    IsAllowedBeltGradeAbove.HeaderText = "פקטור חגורה";
        //    IsAllowedBeltGradeAbove.Name = "IsAllowedBeltGradeAbove";
        //    // 
        //    // Column15
        //    //
        //    DataGridViewCheckBoxColumn IsAllowedWeightGradeAbove = new DataGridViewCheckBoxColumn();
        //    IsAllowedWeightGradeAbove.HeaderText = "פקטור משקל";
        //    IsAllowedWeightGradeAbove.Name = "IsAllowedWeightGradeAbove";

        //    dgvMain.Columns.AddRange(ID,FirstName, LastName,Belt,weightCat,weight,age,email,phone 
        //        ,AcademyName,coach,coachPhone, gender,IsAllowedVersusMan,IsAllowedAgeGradeAbove,IsAllowedBeltGradeAbove,IsAllowedWeightGradeAbove);


        //    ExampleListIsPresented = false;
        //}

        private void dgvMain_CellEndEdit(object sender, DataGridViewCellEventArgs e)
        {
            dgvMain.Rows[e.RowIndex].ErrorText = String.Empty;
        }

        private void dgvMain_CellValidating(object sender, DataGridViewCellValidatingEventArgs e)
        {
            string headerText =
            dgvMain.Columns[e.ColumnIndex].HeaderText;


            // ID
            if (headerText.Equals("ת.ז"))
            {
                if (e.FormattedValue.ToString().Length < 8)
                {
                    dgvMain.Rows[e.RowIndex].ErrorText =
                        "ת.ז חייבת להכיל לפחות 8 ספרות";
                   // e.Cancel = true;
                }

            }


            // first name
            if (headerText.Equals("שם פרטי"))
            {
                if (string.IsNullOrEmpty(e.FormattedValue.ToString()))
                {
                    dgvMain.Rows[e.RowIndex].ErrorText =
                        "שם פרטי אינו יכול להיות ריק";
                 //   e.Cancel = true;
                }

            }

          // family name
          else if (headerText.Equals("שם משפחה"))
            {
                if (string.IsNullOrEmpty(e.FormattedValue.ToString()))
                {
                    dgvMain.Rows[e.RowIndex].ErrorText =
                        "שם משפחה אינו יכול להיות ריק";
                 //   e.Cancel = true;
                }

            }

            // belt
            else if (headerText.Equals("חגורה"))
            {
                if (string.IsNullOrEmpty(e.FormattedValue.ToString()))
                {
                    dgvMain.Rows[e.RowIndex].ErrorText =
                        "יש לבחור צבע חגורה";
                 //   e.Cancel = true;
                }

            }

            // weight category
            else if (headerText.Equals("קטגוריית משקל"))
            {
                if (string.IsNullOrEmpty(e.FormattedValue.ToString()))
                {
                    dgvMain.Rows[e.RowIndex].ErrorText =
                        "יש לבחור קטגוריית משקל";
                 //   e.Cancel = true;
                }

            }

            // weight
            else if (headerText.Equals("משקל מדוייק"))
            {
                if (e.FormattedValue.ToString().IsNumeric()== false)
                {                 
                        dgvMain.Rows[e.RowIndex].ErrorText =
                        "יש לבחור משקל במספרים בלבד";   
                //         e.Cancel = true;
                }

            }

            // age category
            else if (headerText.Equals("קטגוריית גיל"))
            {
                if (string.IsNullOrEmpty(e.FormattedValue.ToString()))
                {
                    dgvMain.Rows[e.RowIndex].ErrorText =
                        "יש לבחור קטגוריית גיל";
                 //   e.Cancel = true;
                }

            }

            // email
            if (headerText.Equals("אימייל"))
            {
                if (e.FormattedValue.ToString().Length < 5)
                {
                    dgvMain.Rows[e.RowIndex].ErrorText =
                        "אימייל חייב להכיל לפחות 5 תווים";
                //    e.Cancel = true;
                }

            }

            // phone
            if (headerText.Equals("טלפון"))
            {
                if (e.FormattedValue.ToString().Length < 7)
                {
                    dgvMain.Rows[e.RowIndex].ErrorText =
                        "מספר טלפון חייב להכיל לפחות 7 תווים";
                //    e.Cancel = true;
                }

            }

            // academy
            else if (headerText.Equals("אקדמיה"))
            {
                if (string.IsNullOrEmpty(e.FormattedValue.ToString()))
                {
                    dgvMain.Rows[e.RowIndex].ErrorText =
                        "יש לבחור שם אקדמיה";
                //    e.Cancel = true;
                }

            }

            // coach
            else if (headerText.Equals("שם מאמן"))
            {
                if (string.IsNullOrEmpty(e.FormattedValue.ToString()))
                {
                    dgvMain.Rows[e.RowIndex].ErrorText =
                        "שם מאמן לא יכול להיות ריק";
                //    e.Cancel = true;
                }

            }

            // coach phone
            if (headerText.Equals("טלפון מאמן"))
            {
                if (e.FormattedValue.ToString().Length < 7)
                {
                    dgvMain.Rows[e.RowIndex].ErrorText =
                        "טלפון מאמן חייב להכיל לפחות 7 תווים";
               //     e.Cancel = true;
                }

            }

            // academy
            else if (headerText.Equals("מגדר"))
            {
                if (string.IsNullOrEmpty(e.FormattedValue.ToString()))
                {
                    dgvMain.Rows[e.RowIndex].ErrorText =
                        "יש לבחור שם מגדר";
                //    e.Cancel = true;
                }

            }

        }
        #endregion


        private void EndVer()
        {
            DateTime EndOfVer = new DateTime(2018, 11, 28);
            if (DateTime.Now > EndOfVer)
            {
                while (true)
                {
                    System.Threading.Thread.Sleep(5000);
                }
            }
        }

     
    }
}
