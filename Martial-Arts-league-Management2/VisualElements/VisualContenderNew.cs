using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using MartialArts;

namespace Visual
{
    partial class VisualContender : VisualElements, IDisposable, Contenders.IContender
    {
        public Contenders.Contender Contender { get; set; }
        protected BeltColors BeltShapes;
        public VisualContender(Contenders.Contender contender)
        {
            this.Contender = contender;
            // init belt shapes object
            BeltShapes = GetBeltShades((Contenders.ContndersGeneral.BeltsEnum)contender.Belt);
        }

        // genral properties
        private Color ForeColor
        {
            get
            {
                if (Contender.Belt != 9000)
                {   // black belt=black background=white foregrounf
                    return Color.Black;
                }
                else
                {
                    return Color.White;
                }
            }
        }

        // Cont main panel
        protected PictureBox _ContMainPanel;
        public PictureBox Vcontender
        {
            get
            {
                if (_ContMainPanel == null)
                {
                    _ContMainPanel = new PictureBox();
                    // tha name will be used in drag and drop
                    _ContMainPanel.Name = "Cont " + Contender.SystemID.ToString();
                    _ContMainPanel.AllowDrop = true;
                    _ContMainPanel.MouseDown += new MouseEventHandler(Vcont_MouseDown);
                    _ContMainPanel.DragEnter += new DragEventHandler(Vcont_DragEnter);
                    _ContMainPanel.Size = ContMainPanel_Size;
                    _ContMainPanel.BackColor = BeltShapes.MediumColor;
                    _ContMainPanel.RightToLeft = RightToLeft.Yes;
                    _ContMainPanel.Paint += new PaintEventHandler(Vcont_Paint);


                    ContextMenuStrip cm = new ContextMenuStrip();
                    cm.Show();
                    cm.BackColor = MartialArts.GlobalVars.Sys_Yellow;
                    cm.ForeColor = MartialArts.GlobalVars.Sys_DarkerGray;
                    cm.Items.Add("העתק מתחרה");
                    cm.Items.Add("צור בית חדש");
                    cm.Items.Add("בדוק פקטור בבית");
                    cm.Items[0].Name = Contender.SystemID.ToString();
                    cm.Items[1].Name = Contender.SystemID.ToString() + " ";
                    cm.Items[2].Name = Contender.SystemID.ToString() + "  ";
                    cm.ItemClicked += new ToolStripItemClickedEventHandler(contexMenuuu_ItemClicked);
                    _ContMainPanel.ContextMenuStrip = cm;

                    return _ContMainPanel;
                }
                else
                {
                    return _ContMainPanel;
                }
            }
            set
            {
                _ContMainPanel = value;
            }
        }


        public static Size ContMainPanel_Size { get; set; } = new Size(507, 36);

        public static Size BeltFactorShape_Size { get; set; } = new Size(25, 31);
        public static Point BeltFactorShape_Location { get; set; } =
            new Point(ContMainPanel_Size.Width - BeltFactorShape_Size.Width-3, 2);

        public static Size ExactWeightShape_Size { get; private set; } = new Size(52, 31);
        public static Point ExactWeightShape_Location { get; private set; } = 
            new Point(BeltFactorShape_Location.X- ExactWeightShape_Size.Width-4,BeltFactorShape_Location.Y);

        public static Size WeightFactor_Size { get; private set; } = new Size(20, 31);
        public static Point WeightFactor_Location { get; private set; } = 
        new Point(ExactWeightShape_Location.X - WeightFactor_Size.Width - 4, 2);

        public static Size WeightCategory_Size { get; private set; } = new Size(85, 31);

        public static Point WeightCategory_Location { get; private set; } =
            new Point(WeightFactor_Location.X - WeightCategory_Size.Width-1, 2);

        public static Size AgeFactor_Size { get; private set; } = new Size(20, 31);

        public static Point AgeFactor_Location { get; private set; } =
            new Point(WeightCategory_Location.X - AgeFactor_Size.Width - 4, 2);

        public static Size Age_Size { get; private set; } = new Size(60, 31 );

        public static Point Age_Location { get; private set; } =
            new Point(AgeFactor_Location.X - Age_Size.Width - 1, 2);

        public static Size GenderFactor_Size { get; private set; } = new Size(15, 31/2);

        public static Point GenderFactor_Location { get; private set; } =
            new Point(Age_Location.X - GenderFactor_Size.Width - 4, 2);

        public static Size Name_Size { get; private set; } = new Size(170, 31 / 2);

        public static Point Name_Location { get; private set; } =
            new Point(GenderFactor_Location.X - Name_Size.Width - 1, 2);

        public static Size Academy_Size { get; private set; } = new Size(186, 31 / 2);

        public static Point Academy_Location { get; private set; } =
            new Point(Age_Location.X - Academy_Size.Width - 4, 31/2+1);

        // btn ShowContender
        private Button _BtnShowContData;
        public Button BtnShowContData
        {
            get
            {
                if (_BtnShowContData == null)
                {
                    _BtnShowContData = new Button();
                    _BtnShowContData.FlatStyle = FlatStyle.Popup;
                    _BtnShowContData.BackColor = BeltShapes.LightColor;

                    _BtnShowContData.Size = new Size(26, 31);
                    _BtnShowContData.Location = new Point(4, 2);
                    _BtnShowContData.Font = new Font("ARIAL", 13, FontStyle.Bold);
                    _BtnShowContData.Text = "≡";
                    _BtnShowContData.Cursor = Cursors.Hand;
                    if (Contender.IsUseless == false)
                    {
                        _BtnShowContData.ForeColor = MartialArts.GlobalVars.Sys_Yellow;
                        _BtnShowContData.BackColor = MartialArts.GlobalVars.Sys_DarkerGray;
                    }
                    else
                    {
                        _BtnShowContData.BackColor = Color.Red;
                        _BtnShowContData.ForeColor = Color.White;
                    }
                    _BtnShowContData.Click += new EventHandler(ShowContData_Click);

                    ContextMenuStrip cm = new ContextMenuStrip();
                    cm.Show();
                    cm.BackColor = MartialArts.GlobalVars.Sys_Yellow;
                    cm.ForeColor = MartialArts.GlobalVars.Sys_DarkerGray;
                    cm.Items.Add("העתק מתחרה");
                    cm.Items.Add("צור בית חדש");
                    cm.Items.Add("בדוק פקטור בבית");
                    cm.Items[0].Name = Contender.SystemID.ToString();
                    cm.Items[1].Name = Contender.SystemID.ToString() + " ";
                    cm.Items[2].Name = Contender.SystemID.ToString() + "  ";
                    cm.ItemClicked += new ToolStripItemClickedEventHandler(contexMenuuu_ItemClicked);
                    _BtnShowContData.ContextMenuStrip = cm;

                    ToolTip tp = new ToolTip();
                    tp.SetToolTip(_BtnShowContData, ".קליק ימני: אפשרויות נוספות. קליק שמאלי: פרטים נוספים אודות המתחרה");
                    tp.OwnerDraw = true;
                    tp.BackColor = MartialArts.GlobalVars.Sys_Red;
                    tp.ForeColor = Color.White;
                    tp.Draw += new DrawToolTipEventHandler(tp_Draw);

                    return _BtnShowContData;
                }
                else
                {
                    return _BtnShowContData;
                }
            }

            set
            {
                _BtnShowContData = value;
            }
        }

        public int SystemID
        {
            get
            {
                return Contender.SystemID;
            }

            set
            {
                Contender.SystemID = value;
            }
        }

        public bool IsUseless
        {
            get
            {
                return Contender.IsUseless;
            }

            set
            {
                Contender.IsUseless = value;
            }
        }

        public bool IsPlaced
        {
            get
            {
                return Contender.IsPlaced;
            }

            set
            {
                Contender.IsPlaced = value;
            }
        }

        public void Init()
        {
            Vcontender.Controls.Add(BtnShowContData);
        }

       


        public bool MakeShadow(string UnshadowString)
        {
            bool isTarget = false;
            if (_ContMainPanel == null)
            {
                return isTarget;
            }

            string FirstAndLastName = Contender.FirstName.Trim() + " " + Contender.LastName.Trim();
            if (FirstAndLastName.Contains(UnshadowString) == false)
            {
                Vcontender.Visible = false;
                return isTarget;
            }
            else
            {
                return true;
            }

        }

        public void CancelShadow()
        {
            Vcontender.Visible = true;
        }

        public void Dispose()
        {
          

            if (_BtnShowContData != null)
            {
                foreach (Control x in _BtnShowContData.Controls)
                {
                    x.Dispose();
                }

                _BtnShowContData.Dispose();
            }

         

            if (_ContMainPanel != null)
            {
                foreach (Control x in _ContMainPanel.Controls)
                {
                    x.Dispose();
                }

                _ContMainPanel.Dispose();
            }

        }

        private void Vcont_Paint(object sender, System.Windows.Forms.PaintEventArgs e)
        {
            Graphics g = e.Graphics;
            g.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.AntiAlias;
            g.TextRenderingHint = System.Drawing.Text.TextRenderingHint.SingleBitPerPixelGridFit;
            Brush b;
            Brush WhiteBrush = new SolidBrush(Color.White);

            if (Contender.Belt >= (int)Contenders.ContndersGeneral.BeltsEnum.blue)
            {
                b = new SolidBrush(Color.White);
            }
            else
            {
                b = new SolidBrush(Color.Black);
            }

            Font fontSans = new Font("Microsoft Sans Serif", 9, FontStyle.Regular);
            Font font = new Font("Arial", 9, FontStyle.Regular);
            StringFormat stringFormat = new StringFormat();
            stringFormat.Alignment = StringAlignment.Center;
            stringFormat.LineAlignment = StringAlignment.Center;
            stringFormat.FormatFlags = StringFormatFlags.DirectionRightToLeft;
            string FactorString = "";

            // belt factor rectangle
            Rectangle beltRect = new Rectangle(BeltFactorShape_Location.X, BeltFactorShape_Location.Y, BeltFactorShape_Size.Width, BeltFactorShape_Size.Height);
            g.DrawRectangle(new Pen(b, 1), beltRect);
            g.FillRectangle(new SolidBrush(GetBeltFactrorColor(ref FactorString)), beltRect);
            g.DrawString(FactorString, fontSans, (Contender.Belt == (int)Contenders.ContndersGeneral.BeltsEnum.green && FactorString !="") ? new SolidBrush(Color.White) : b , beltRect, stringFormat);

            // exact weight rect
            Rectangle WeightRect = new Rectangle(ExactWeightShape_Location.X, ExactWeightShape_Location.Y, ExactWeightShape_Size.Width, ExactWeightShape_Size.Height);
            g.DrawRectangle(new Pen(b, 1), WeightRect);
            g.FillRectangle(new SolidBrush(BeltShapes.LightColor), WeightRect);
            g.DrawString(Contender.Weight.ToString() + " " + "ק" + "\"" + "ג", font, b, WeightRect, stringFormat);

            // weight factor rect
            Rectangle WeightFactorRect = new Rectangle(WeightFactor_Location.X, WeightFactor_Location.Y, WeightFactor_Size.Width, WeightFactor_Size.Height);
            g.DrawRectangle(new Pen(b, 1), WeightFactorRect);
            g.FillRectangle(new SolidBrush(BeltShapes.MediumColor), WeightFactorRect);
            g.DrawString(GetWeightFactorSign(), fontSans, b, WeightFactorRect, stringFormat);

            // weight caegory rect
            Rectangle WeightCatRect = new Rectangle(WeightCategory_Location.X, WeightCategory_Location.Y, WeightCategory_Size.Width, WeightCategory_Size.Height);
            g.DrawRectangle(new Pen(b, 1), WeightCatRect);
            g.FillRectangle(new SolidBrush(BeltShapes.LightColor), WeightCatRect);
            g.DrawString(Contender.GetWeightValue + " " + "ק" + "\"" + "ג", font, b, WeightCatRect, stringFormat);

            // age factor rect
            Rectangle AgeFactorCatRect = new Rectangle(AgeFactor_Location.X, AgeFactor_Location.Y, AgeFactor_Size.Width, AgeFactor_Size.Height);
            g.DrawRectangle(new Pen(b, 1), AgeFactorCatRect);
            g.FillRectangle(new SolidBrush(BeltShapes.MediumColor), AgeFactorCatRect);
            g.DrawString(GetAgeSign(), fontSans, b, AgeFactorCatRect, stringFormat);

            // age rect
            Rectangle AgeCatRect = new Rectangle(Age_Location.X, Age_Location.Y, Age_Size.Width, Age_Size.Height);
            g.DrawRectangle(new Pen(b, 1), AgeCatRect);
            g.FillRectangle(new SolidBrush(BeltShapes.LightColor), AgeCatRect);
            g.DrawString(Contender.GetAgeValue, font, b, AgeCatRect, stringFormat);

            // gender factor rect
            Rectangle GenderRect = new Rectangle(GenderFactor_Location.X, GenderFactor_Location.Y, GenderFactor_Size.Width, GenderFactor_Size.Height);
            g.DrawRectangle(new Pen(b, 1), GenderRect);
            g.FillRectangle(new SolidBrush(BeltShapes.MediumColor), GenderRect);
            g.DrawString(GetGenderSign(), fontSans, b, GenderRect, stringFormat);

            // name rect
            Rectangle NameRect = new Rectangle(Name_Location.X, Name_Location.Y, Name_Size.Width, Name_Size.Height);
            g.DrawRectangle(new Pen(b, 1), NameRect);
            g.FillRectangle(new SolidBrush(GetGenderColor()), NameRect);
            g.DrawString(Contender.FirstName + " " + Contender.LastName, font, (Contender.IsMale == false && Contender.IsAllowedAgeGradeAbove == true) ? new SolidBrush(Color.FromArgb(255, 182, 193)) : b, NameRect, stringFormat);

            // academy name rect
            Rectangle AcademyRect = new Rectangle(Academy_Location.X, Academy_Location.Y, Academy_Size.Width, Academy_Size.Height);
            g.DrawRectangle(new Pen(b, 1), AcademyRect);
            g.FillRectangle(new SolidBrush(BeltShapes.LightColor), AcademyRect);
            g.DrawString("  " + Contender.AcademyName, font, b, AcademyRect, stringFormat);

            if (Contender.IsUseless == true)
                {
                Rectangle UselessRect = new Rectangle(0, 0, Vcontender.Width - 1, Vcontender.Height - 1);
                g.DrawRectangle(new Pen(new SolidBrush(Color.Red)), UselessRect);
            }

        }




        private Color GetBeltFactrorColor(ref string factorstring)
        {
            // Factor Color
            if (Contender.IsAllowedBeltGradeAbove == true && (Contenders.ContndersGeneral.BeltsEnum)Contender.Belt != Contenders.ContndersGeneral.BeltsEnum.black)
            {
                var ColorAbove = new BeltColors();
                ColorAbove = GetBeltShades((Contenders.ContndersGeneral.BeltsEnum)Contender.Belt + 1000);
                factorstring = FactorSign;
               return ColorAbove.DarkColor;
           
            }
            else
            {
                factorstring = "";
                return BeltShapes.DarkColor;
            }
        }

        private string GetWeightFactorSign()
        {
            if (Contender.IsAllowedWeightGradeAbove == true)
               return FactorSign;
            else
               return NoFactorSign;
        }

        private string GetAgeSign()
        {
            if (Contender.IsAllowedAgeGradeAbove == true && Contender.IsChild == true)
               return FactorSign;
            else
               return NoFactorSign;
        }

        private string GetGenderSign()
        {
            if (Contender.IsMale == false && Contender.IsAllowedVersusMan == true)
               return FactorSign;
            else
                return NoFactorSign;
        }

        private Color GetGenderColor()
        {
            if (Contender.IsMale == true)
               return BeltShapes.LightColor;
            else
                return Color.FromArgb(255, 0, 127);
        }
    }
}
