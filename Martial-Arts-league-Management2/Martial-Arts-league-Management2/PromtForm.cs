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

        private void LoadMe()
        {
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

        private void FormSize()
        {
    
        }

        private void btnYes_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.OK;
            this.Close();
        }

        private void btnNo_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.No;
            this.Close();
        }

        private void lblQuestion_Click(object sender, EventArgs e)
        {

        }
    }
}
