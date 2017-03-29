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
    public partial class PromtForm : Form
    {
        public String text;
        public bool LongText;
        public bool OnlyYesButton;
        public double FormSizeMultiplication = 1;
        public PromtForm(string txt,bool LongText = false,string Header = "הודעה ממערכת IBJJL",bool OnlyYesButton=false,string btnYesText = "אישור",string btnNoText = "ביטול")
        {
            InitializeComponent();

            this.text = txt;
            this.lblHeader.Text = Header;
            this.LongText = LongText;
            this.OnlyYesButton = OnlyYesButton;
            this.btnYes.Text = btnYesText;
            this.btnNo.Text = btnNoText;
        }

        public PromtForm(Size FormSize,string txt, bool LongText = false, string Header = "הודעה ממערכת IBJJL", bool OnlyYesButton = false, string btnYesText = "אישור", string btnNoText = "ביטול")
        {
            InitializeComponent();
            this.Size = FormSize;
            this.text = txt;
            this.lblHeader.Text = Header;
            this.LongText = LongText;
            this.OnlyYesButton = OnlyYesButton;
            this.btnYes.Text = btnYesText;
            this.btnNo.Text = btnNoText;
        }

        public void MakeBtnsChanges(string btnYesText = "אישור", string btnNoText = "ביטול")
        {
            this.btnYes.Text = btnYesText;
            this.btnNo.Text = btnNoText;
        }

        private void PromtForm_Load(object sender, EventArgs e)
        {
            lblQuestion.Text = this.text;
            LoadMe();
        }

        protected virtual void LoadMe()
        {
            this.lblQuestion.DoubleClick += new EventHandler(ShowOnMsgBox);
            if (OnlyYesButton == true)
            {
                btnYes.Size = new Size(lblQuestion.Width, btnYes.Height);
                btnNo.Visible = false;
                btnNo.Enabled = false;
            }

            if (LongText == true)
            {
                TextBox r = new TextBox();
                r.Font = new Font("Consolas", 8, FontStyle.Regular);
                r.Anchor = AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right | AnchorStyles.Top;
                r.Multiline = true;
                r.Size = lblQuestion.Size;
                r.Location = lblQuestion.Location;
                r.ReadOnly = true;
                r.ScrollBars = ScrollBars.Both;
                
  
                lblQuestion.Dispose();
                
                r.RightToLeft = RightToLeft.Yes;
                r.Text = text;
                r.BackColor = MartialArts.GlobalVars.Sys_Yellow;
                r.ForeColor = MartialArts.GlobalVars.Sys_DarkerGray;
                this.Controls.Add(r);
            }

        }

        private void ShowOnMsgBox(Object sender, EventArgs e)
        {
            MartialArts.Helpers.DefaultMessegeBox(this.lblQuestion.Text, "IBJJL", MessageBoxIcon.Information);
        }

        private void FormSize()
        {
    
        }

        protected virtual void btnYes_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.OK;
            this.Close();
        }

        private void btnNo_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.No;
            this.Close();
        }

    }

    class GetProjectNameForm : PromtForm
    {
        public string ProjectName { get; set; }
        public TextBox t;
        public GetProjectNameForm() : base("",false,"בחירת שם לפרויקט")
        { 

        }

        protected override void LoadMe()
        {
            this.Size = new Size(this.Width, (int)(this.Height / 1.2));
            t = new TextBox();
            t.Size = new Size(lblQuestion.Width, 30);
            t.Location = new Point(lblQuestion.Location.X, 130);
            lblQuestion.Size = new Size (lblQuestion.Width,50);
            lblQuestion.Location = new Point(lblQuestion.Location.X, t.Location.Y - lblQuestion.Height - 5);
            lblQuestion.Text = "אנא בחר שם לפרויקט:";
            this.Controls.Add(t);
         
        }

        protected override void btnYes_Click(object sender, EventArgs e)
        {

            // check length
            if (t.Text.ToString().Length > 60)
            {
                MartialArts.Helpers.DefaultMessegeBox("שם פרויקט לא יכול להכיל יותר משישים תווים", "שם פרויקט לא חוקי", MessageBoxIcon.Stop);
                return;
            }

            // check for iilegal chars for directory name
            if (FilePathHasInvalidChars(t.Text.Trim().ToString()) == true)
            {
                MartialArts.Helpers.DefaultMessegeBox("שם הפרויקט מכיל תווים לא חוקיים", "שם פרויקט לא חוקי", MessageBoxIcon.Stop);
                return;
            }

            ProjectName = t.Text.Trim();

            this.DialogResult = DialogResult.OK;
            this.Close();
        }

        public static bool FilePathHasInvalidChars(string path)
        {
            return (!string.IsNullOrEmpty(path) && path.IndexOfAny(System.IO.Path.GetInvalidPathChars()) >= 0);
        }
    }

    class ChooseProjectToLoad : PromtForm
    {

        private string _selectedMenuItem;
        private ContextMenuStrip collectionRoundMenuStrip;
        public string ProjectName { get; set; }
        ListBox l = new ListBox();
        public ChooseProjectToLoad() : base("", false, "בחר פרויקט לטעינה")
        {

        }

        protected override void LoadMe()
        {
            l.Size = lblQuestion.Size;
            l.Location = lblQuestion.Location;
            l.BackColor = MartialArts.GlobalVars.Sys_DarkerGray;
            l.ForeColor = MartialArts.GlobalVars.Sys_Yellow;
            SetProjectNames();
            lblQuestion.Dispose();
            this.Controls.Add(l);

            var toolStripMenuItem1 = new ToolStripMenuItem { Text = "מחק פרויקט" };
            toolStripMenuItem1.Click += toolStripMenuItem1_Click;

            collectionRoundMenuStrip = new ContextMenuStrip();
            collectionRoundMenuStrip.Items.AddRange(new ToolStripItem[] { toolStripMenuItem1});
            l.MouseDown +=new  MouseEventHandler(listBoxCollectionRounds_MouseDown);

        }

        private void toolStripMenuItem1_Click(object sender, EventArgs e)
        {

            if (l.SelectedIndex == -1)
                return;

            _selectedMenuItem = (l.SelectedItem.ToString());
            // delete project (promt is in the instance)
            MartialArts.ProjectsSavedAsBinaryFiles del = new MartialArts.ProjectsSavedAsBinaryFiles(_selectedMenuItem);
            del.SetFullDirWithoutCreating();
            del.DeleteProject();

            // refresh listbox
            SetProjectNames();

        }

        private void DeleteProject()
        {

        }

        private void listBoxCollectionRounds_MouseDown(object sender, MouseEventArgs e)
        {
            if (l.SelectedIndex == -1)
                return;

            if (e.Button != MouseButtons.Right) return;
            var index = l.IndexFromPoint(e.Location);
            if (index != ListBox.NoMatches)
            {
                _selectedMenuItem = l.Items[index].ToString();
                collectionRoundMenuStrip.Show(Cursor.Position);
                collectionRoundMenuStrip.Visible = true;
            }
            else
            {
                collectionRoundMenuStrip.Visible = false;
            }
        }

        private void SetProjectNames()
        {
            l.Items.Clear();
            List<string> projects = MartialArts.ProjectsSavedAsBinaryFiles.GetProjectNames();
            if (projects.Count == 0)
            {
                MartialArts.Helpers.DefaultMessegeBox("לא קיימים פרויקטים", "אין פרויקטים שמורים", MessageBoxIcon.Stop);
                return;
            }

            foreach (string name in projects)
            {
                l.Items.Add(name);
            }
        }

        protected override void btnYes_Click(object sender, EventArgs e)
        {
            if (l.SelectedIndex == -1)
                return;

            ProjectName = l.SelectedItem.ToString();
            this.DialogResult = DialogResult.OK;
            this.Close();


        }
    }

    class ChangeVisualBracketDesc : PromtForm
    {

        public ComboBox ComboWeight = new ComboBox();
        public ComboBox ComboAge = new ComboBox();
        public ComboBox ComboBelt = new ComboBox();

        public Label lblAge = new Label();
        public Label  lblWeight = new Label();
        public Label lblBelt = new Label();

        public bool IsChild;
        public int Age;
        public int Weight;
        public int Belt;


        public ChangeVisualBracketDesc(bool isChild,int age,int weight,int belt) : base("", false, "עריכת תיאור בית")
        {
            this.IsChild = isChild;
            this.Weight = weight;
            this.Belt = belt;
            this.Age = age;
        }

        protected override void LoadMe()
        {
            lblQuestion.Dispose();
            LblDefenitions(lblAge, "גיל:");
            LblDefenitions(lblBelt,"חגורה:");
            LblDefenitions(lblWeight, "משקל:");

            ComboDefenitions(new ComboBox[] { ComboWeight, ComboAge, ComboBelt });

            lblAge.Location = new Point(270, 85);
            lblBelt.Location = new Point(270, 130);
            lblWeight.Location = new Point(270, 175);

            ComboAge.Location = new Point(lblAge.Location.X -  ComboAge.Width-5, 85-3);
            ComboBelt.Location = new Point(lblAge.Location.X -  ComboAge.Width - 5, 130-3);
            ComboWeight.Location = new Point(lblAge.Location.X -  ComboAge.Width - 5, 175-3);

            LoadCombos();

        }

        private void ComboDefenitions(ComboBox[] com)
        {
            for(int i = 0; i< com.Length;i++)
            {
                com[i].Size = new Size(150, 30);
                com[i].BackColor = MartialArts.GlobalVars.Sys_Red;
                com[i].ForeColor = MartialArts.GlobalVars.Sys_White;
                com[i].FlatStyle = FlatStyle.Flat;
                com[i].RightToLeft = RightToLeft.Yes;
                this.Controls.Add(com[i]);
            }
        }

        private void LblDefenitions(Label lbl,string text)
        {
            lbl.AutoSize = true;
            lbl.Text = text;
            lbl.RightToLeft = RightToLeft.Yes;
            lbl.TextAlign = ContentAlignment.MiddleRight;
            lbl.Font = new Font("ARIAL", 9, FontStyle.Bold);
            this.Controls.Add(lbl);
        }

        private void LoadCombos()
        {
            // age
                foreach(String a in Contenders.ContndersGeneral.GetAgeValues(IsChild))
                {
                    ComboAge.Items.Add(a);
                }
            // weight
            if (IsChild)
            {
                foreach (KeyValuePair<string,int> a in Contenders.ContndersGeneral.ChildWeightCat)
                {
                    ComboWeight.Items.Add(a.Key);
                }
            }
            else
            {
                foreach (KeyValuePair<string, int> a in Contenders.ContndersGeneral.AdultWeightCat)
                {
                    ComboWeight.Items.Add(a.Key);
                }
            }

            // belt
            var beltList = new List<string>() { "לבנה", "אפורה", "צהובה", "כתומה", "ירוקה", "כחולה", "סגולה", "חומה", "שחורה" };
            foreach (string a in beltList)
            {
                ComboBelt.Items.Add(a);
            }



        }

    }
}
