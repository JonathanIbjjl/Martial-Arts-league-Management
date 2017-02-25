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
       private Label _Header;
       public Label Header
        {
            get
            {
                if (_Header == null)
                {
                    _Header = new Label();
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
                    return Header;
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
            // add header
            Vbracket.Controls.Add(Header);
            foreach (Contenders.Contender c in Bracket.ContendersList)
            {
                VisualContender visualcont = new VisualContender(c);
                visualcont.Init();
                Vbracket.Controls.Add(visualcont.Vcontender);
            }
        }
    }
}
