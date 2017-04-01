using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using MartialArts;
namespace  Visual
{
    //
    [Serializable]
    class VisualBracket: VisualElements,ICloneable,IDisposable
    {
       public MartialArts.Bracket Bracket;
       private  FlowLayoutPanel _Vbracket;
       private List<VisualContender> _VisualCont;
       public List<VisualContender> VisualCont
        {
            get
            {
                if (_VisualCont == null)
                {
                    _VisualCont = new List<VisualContender>();
                    return _VisualCont;
                }
                else
                {
                    return _VisualCont;
                }

            }
            set { _VisualCont = value; }
        }
       private Label _Header;
       public Label Header
        {
            get
            {
                if (_Header == null)
                {
                    _Header = new Label();
                    //_Header.DoubleBuffered_Label(true); TODO: DELETE
                    // will be used in drag and drop
                    _Header.Name = "BracketHeader " + Bracket.BracketNumber.ToString();
                    _Header.DragOver += new DragEventHandler(Vbracket_DragOver);
                    _Header.DragDrop += new DragEventHandler(Vbracket_DragDrop);
                    _Header.AllowDrop = true;
                    _Header.AutoSize = false;
                    _Header.Size = new Size(VisualContender.ContMainPanel_Size.Width, 20);
                    _Header.BackColor = MartialArts.GlobalVars.Sys_LighterGray;
                    _Header.ForeColor = MartialArts.GlobalVars.Sys_Yellow;
                    _Header.Text = Bracket.ToString();
                    _Header.Margin = new Padding(3, 3, 3, 3);
                    _Header.Font = new Font("ARIAL", 12, FontStyle.Bold | FontStyle.Underline);
                    _Header.TextAlign = ContentAlignment.MiddleCenter;

                    ContextMenuStrip cm = new ContextMenuStrip();
                    cm.Show();
                    cm.BackColor = MartialArts.GlobalVars.Sys_Yellow;
                    cm.ForeColor = MartialArts.GlobalVars.Sys_DarkerGray;
                    cm.Items.Add("הדבק מתחרה");
                    cm.ItemClicked += new ToolStripItemClickedEventHandler(contexMenuuu_ItemClicked);
                    _Header.ContextMenuStrip = cm;
                    // hide button 
                    btnHideBracket.Location = new Point(_Header.Width- btnHideBracket.Width - 3, 1);
                    _Header.Controls.Add(btnHideBracket);
                    // change description button
                    btnChangeDescription.Location = new Point(6, 1);
                    _Header.Controls.Add(btnChangeDescription);
                    return _Header;
                }
                else
                {
                    return _Header;
                }
            }
            set { _Header = value; }
        }
       public FlowLayoutPanel Vbracket
        {
            get
            {
                if (_Vbracket == null)
                {
                    _Vbracket = new FlowLayoutPanel();
                    // will be used in drag and drop
                    _Vbracket.Name = "Bracket " + Bracket.BracketNumber.ToString();
                    _Vbracket.AllowDrop = true;
                
                    _Vbracket.Size = new Size(VisualContender.ContMainPanel_Size.Width + 4, ((VisualContender.ContMainPanel_Size.Height + 6) * Bracket.ContendersList.Count ) +26); // ontMainPanel_Size.Height + 6 is the margin beetween contenders,last digit:  Header And Margin
                    _Vbracket.BackColor = Color.Black;
                    _Vbracket.Margin = new Padding(6, 6, 6, 6);
                    return _Vbracket;
                }
                else
                {
                    return _Vbracket;
                }
            }

            set { _Vbracket = value; }
        }

        #region "Hide Bracket"
        private Label _btnHideBracket;
        public Label btnHideBracket
        {
            get
            {
                if (_btnHideBracket == null)
                {
                    _btnHideBracket = new Label();
                    _btnHideBracket.Size = new Size(20,18);
                    _btnHideBracket.Font = new Font("ARIAL", 10, FontStyle.Bold);
                    _btnHideBracket.TextAlign = ContentAlignment.MiddleCenter;
                    _btnHideBracket.BackColor = GlobalVars.Sys_Red;
                    _btnHideBracket.BorderStyle = BorderStyle.FixedSingle;
                    _btnHideBracket.ForeColor = Color.White;
                    _btnHideBracket.FlatStyle = FlatStyle.Flat;
                    _btnHideBracket.Cursor = Cursors.Hand;
                    _btnHideBracket.Text = "-";
                    _btnHideBracket.Click += new EventHandler(btnHide_Click);
                    return _btnHideBracket;
                }
                else
                {
                    return _btnHideBracket;
                }
            }
        }

        private void btnHide_Click(object sender, EventArgs e)
        {
            if (((Label)sender).Text == "-")
            {
                // hide
                ((Label)sender).Text = "+";
                Vbracket.Size = new Size(Vbracket.Width, Header.Size.Height + 6);
                // change to unactive colors
                _Header.BackColor = Color.FromArgb(40, 40, 40);
                _Header.ForeColor = Color.FromArgb(70, 70, 70);
                // disable drag events
                _Header.DragOver -= new DragEventHandler(Vbracket_DragOver);
                _Header.DragDrop -= new DragEventHandler(Vbracket_DragDrop);
            }
            else
            {
                // unhide
                ((Label)sender).Text = "-";
                Vbracket.Size = new Size(Vbracket.Width, ((VisualContender.ContMainPanel_Size.Height + 6) * Bracket.ContendersList.Count) + 26);
                // returen to active colors
                _Header.BackColor = MartialArts.GlobalVars.Sys_LighterGray;
                _Header.ForeColor = MartialArts.GlobalVars.Sys_Yellow;
                // enable drag events
                _Header.DragOver += new DragEventHandler(Vbracket_DragOver);
                _Header.DragDrop += new DragEventHandler(Vbracket_DragDrop);
            }
        }

        // method to call from outside the class
        public void Hide()
        {
            // hide
            btnHideBracket.Text = "+";
            Vbracket.Size = new Size(Vbracket.Width, Header.Size.Height + 6);
            // change to unactive colors
            _Header.BackColor = Color.FromArgb(40, 40, 40);
            _Header.ForeColor = Color.FromArgb(70, 70, 70);
            // disable drag events
            _Header.DragOver -= new DragEventHandler(Vbracket_DragOver);
            _Header.DragDrop -= new DragEventHandler(Vbracket_DragDrop);
        }

        public void Expand()
        {
            btnHideBracket.Text = "-";
            Vbracket.Size = new Size(Vbracket.Width, ((VisualContender.ContMainPanel_Size.Height + 6) * Bracket.ContendersList.Count) + 26);
            // returen to active colors
            _Header.BackColor = MartialArts.GlobalVars.Sys_LighterGray;
            _Header.ForeColor = MartialArts.GlobalVars.Sys_Yellow;
            // enable drag events
            _Header.DragOver += new DragEventHandler(Vbracket_DragOver);
            _Header.DragDrop += new DragEventHandler(Vbracket_DragDrop);
        }

        #endregion

        #region "Change Bracket Description"
        private Label _btnChangeDescription;
        public Label btnChangeDescription
        {
            get
            {
                if (_btnChangeDescription == null)
                {
                   _btnChangeDescription = new Label();
                   _btnChangeDescription.Size = new Size(22, 17);
                   _btnChangeDescription.Font = new Font("ARIAL", 10, FontStyle.Regular);
                   _btnChangeDescription.TextAlign = ContentAlignment.MiddleCenter;
                   _btnChangeDescription.BackColor = GlobalVars.Sys_DarkerGray;
                   _btnChangeDescription.BorderStyle = BorderStyle.FixedSingle;
                   _btnChangeDescription.ForeColor = GlobalVars.Sys_Yellow;
                   _btnChangeDescription.FlatStyle = FlatStyle.Flat;
                   _btnChangeDescription.Cursor = Cursors.Hand;
                   _btnChangeDescription.Text = "≡";
                    _btnChangeDescription.Click += new EventHandler(btnDescription_Click);
                    return _btnChangeDescription;
                }
                else
                {
                    return _btnChangeDescription;
                }
            }
        }

        private void btnDescription_Click(object sender, EventArgs e)
        {
            Martial_Arts_league_Management2.ChangeVisualBracketDesc desc = new Martial_Arts_league_Management2.ChangeVisualBracketDesc(Bracket.IsChild,Bracket.AgeGrade,Bracket.WeightGrade,Bracket.BeltGrade);
            if (desc.ShowDialog() == DialogResult.OK)
            {
                Bracket.RefreshBracketInfo(desc.Age,desc.Belt,desc.Weight);
                Header.Text = Bracket.ToString();
            }
        }

        #endregion




        public VisualBracket(MartialArts.Bracket bracket)
        {
            this.Bracket = bracket;
        }

        public void Init()
        {

            Vbracket.Controls.Add(Header);

            foreach (Contenders.Contender c in Bracket.ContendersList)
            {
                VisualContender visualcont = new VisualContender(c);
                visualcont.Init();
                // add to list
                this.VisualCont.Add(visualcont);
                // add to panel
                Vbracket.Controls.Add(visualcont.Vcontender);
            }
        }

        #region "Drag And Drop"

        private void Vbracket_DragOver(object sender, System.Windows.Forms.DragEventArgs e)
        {
            e.Effect = System.Windows.Forms.DragDropEffects.All;
        }


        private void Vbracket_DragDrop(object sender, System.Windows.Forms.DragEventArgs e)
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
            // check if contender is allready belongs to this bracket
            if (Bracket.ContendersList.Any(x => x.SystemID == ContID))
                return;

            // Extract cont
            VisualContender visualcont = VisualLeagueEvent.AllVisualContenders.Where(x => x.Contender.SystemID == ContID).Select(x => x).Single();
            // check if the contender score is suitable for bracket of not promt the user
            if (CheckContTransffer(visualcont) == false)
                return;
            // Its the header ( Vbracket FlowLayoutPanel child, hence we use Parent.Controls.Add)
            ((System.Windows.Forms.Control)sender).Parent.Controls.Add(Parent);


            VisualLeagueEvent.AddContender(ContID,this);

        }

        private bool CheckContTransffer(VisualContender vis)
        {
            double finalgrade;
            if (VisualLeagueEvent.IsSutibleForBracket(vis, this,out finalgrade) == false)
            {

                using (Martial_Arts_league_Management2.PromtForm promt= new Martial_Arts_league_Management2.PromtForm("המתחרה שברצונך להעביר לבית זה אינו מתאים לציון הממוצע של הבית, אנא אשר על מנת להעבירו", false))
                {
                    if (promt.ShowDialog() == DialogResult.OK)
                    {
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

        public VisualBracket Refresh()
        {
            if (Bracket.ContendersList.Count == 0)
            {
                // the user killed the bracket
                Vbracket.Dispose();
                return this;
            }
            else
            {
                // if VisualLeague Event Changed Visual ContenderLIst from out side the resize will take effect 
                Vbracket.Size = new Size(VisualContender.ContMainPanel_Size.Width + 4, ((VisualContender.ContMainPanel_Size.Height + 6) * VisualCont.Count) + 26); // ontMainPanel_Size.Height + 6 is the margin beetween contenders,last digit:  Header And Margin

                // refresh header (moty asked to cancel the new brackets info i want to preserve the original bracket header)
            //   Bracket.RefreshBracketInfo();
               // Header.Text = Bracket.ToString();
                return null;
            }
        }


        void contexMenuuu_ItemClicked(object sender, System.Windows.Forms.ToolStripItemClickedEventArgs e)
        {
            System.Windows.Forms.ToolStripItem item = e.ClickedItem;

            if (item.Text == "הדבק מתחרה")
            {

                if (VisualLeagueEvent.ClipBoardValue < 1000)
                {
                    // no contender was copied do nothing
                }
                else
                {
                    PasteVisualContender();
                    // delete the last contnder that was copied
                    VisualLeagueEvent.ClipBoardValue = 0;
                }
            }
        }

        private void PasteVisualContender()
        {
            // Extract Contender ID
            int ContID = VisualLeagueEvent.ClipBoardValue;
            // check if contender is allready belongs to this bracket
            if (Bracket.ContendersList.Any(x => x.SystemID == ContID))
                return;

            // Extract cont
            VisualContender visualcont = VisualLeagueEvent.AllVisualContenders.Where(x => x.Contender.SystemID == ContID).Select(x => x).Single();
            // check if the contender score is suitable for bracket of not promt the user
            if (CheckContTransffer(visualcont) == false)
                return;

            // add to fpanel
            Vbracket.Controls.Add(visualcont.Vcontender);
            VisualLeagueEvent.AddContender(ContID, this);
        }

        public object Clone()
        {
            return this.MemberwiseClone();
        }

        public void Dispose()
        {
            if (_Header != null)
            {
                foreach (Control c in _Header.Controls)
                {
                    c.Dispose();
                }

                _Header.Dispose();
            }

            if (_Vbracket != null)
            {
                foreach (Control c in _Vbracket.Controls)
                {
                    c.Dispose();
                }

                _Vbracket.Dispose();
            }
        }

        #endregion

    }
}
