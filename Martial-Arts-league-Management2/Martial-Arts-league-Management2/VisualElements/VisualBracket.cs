using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
namespace  Visual
{
    class VisualBracket: VisualElements
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
                     MartialArts.ExtensionMethods.DoubleBuffered_FlPanel(_Vbracket, true);
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
            if (((System.Windows.Forms.Control)sender).Name.Contains("BracketHeader"))
            ((System.Windows.Forms.Control)sender).Parent.Controls.Add(Parent);
            else // its the Vbracket FlowLayoutPanel
                ((System.Windows.Forms.Control)sender).Controls.Add(Parent);

            VisualLeagueEvent.AddContender(ContID,this);

   
        }

        private bool CheckContTransffer(VisualContender vis)
        {
            if (VisualLeagueEvent.IsSutibleForBracket(vis, this) == false)
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

                // refresh header
                Header.Text = Bracket.ToString();
                return null;
            }
        }



        #endregion

    }
}
