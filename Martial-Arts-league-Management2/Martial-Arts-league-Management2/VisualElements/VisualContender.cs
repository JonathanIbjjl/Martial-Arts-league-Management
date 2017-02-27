using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
namespace Visual
{
    class VisualElements
    {
        protected struct BeltColors
        {
            public Color DarkColor;
            public Color MediumColor;
            public Color LightColor;
        }
        protected string FactorSign
        {
            get
            {
                return "✔";
            }
        }
        protected string NoFactorSign
        {
            get
            {
                return "";
            }
        }
        protected BeltColors GetBeltShades(Contenders.ContndersGeneral.BeltsEnum belt)
        {
            var Cshapes = new BeltColors();

            switch (belt)
            {
                case Contenders.ContndersGeneral.BeltsEnum.white:
                    Cshapes.DarkColor = Color.FromArgb(222, 222, 222);
                    Cshapes.MediumColor = Color.FromArgb(240, 240, 240);
                    Cshapes.LightColor = Color.FromArgb(255, 255, 255);
                    break;
                case Contenders.ContndersGeneral.BeltsEnum.yellow:
                    Cshapes.DarkColor = Color.FromArgb(204, 204, 0);
                    Cshapes.MediumColor = Color.FromArgb(255, 255, 0);
                    Cshapes.LightColor = Color.FromArgb(255, 255, 102);
                    break;
                case Contenders.ContndersGeneral.BeltsEnum.purpule:
                    Cshapes.DarkColor = Color.FromArgb(51, 0, 51);
                    Cshapes.MediumColor = Color.FromArgb(102, 0, 102);
                    Cshapes.LightColor = Color.FromArgb(245, 0, 245);
                    break;
                case Contenders.ContndersGeneral.BeltsEnum.orange:
                    Cshapes.DarkColor = Color.FromArgb(204, 102, 0);
                    Cshapes.MediumColor = Color.FromArgb(255, 128, 0);
                    Cshapes.LightColor = Color.FromArgb(255, 178, 102);
                    break;
                case Contenders.ContndersGeneral.BeltsEnum.green:
                    Cshapes.DarkColor = Color.FromArgb(0, 100, 0);
                    Cshapes.MediumColor = Color.FromArgb(0, 204, 0);
                    Cshapes.LightColor = Color.FromArgb(102, 255, 102);
                    break;
                case Contenders.ContndersGeneral.BeltsEnum.gray:
                    Cshapes.DarkColor = Color.FromArgb(64, 64, 64);
                    Cshapes.MediumColor = Color.FromArgb(128, 128, 128);
                    Cshapes.LightColor = Color.FromArgb(192, 192, 192);
                    break;
                case Contenders.ContndersGeneral.BeltsEnum.brown:
                    Cshapes.DarkColor = Color.FromArgb(139, 69, 19);
                    Cshapes.MediumColor = Color.FromArgb(210, 105, 30);
                    Cshapes.LightColor = Color.FromArgb(244, 164, 96);
                    break;
                case Contenders.ContndersGeneral.BeltsEnum.blue:
                    Cshapes.DarkColor = Color.FromArgb(25, 25, 112);
                    Cshapes.MediumColor = Color.FromArgb(65, 105, 225);
                    Cshapes.LightColor = Color.FromArgb(135, 206, 250);
                    break;
                case Contenders.ContndersGeneral.BeltsEnum.black:
                    Cshapes.DarkColor = Color.FromArgb(0, 0, 0);
                    Cshapes.MediumColor = Color.FromArgb(28, 28, 28);
                    Cshapes.LightColor = Color.FromArgb(48, 48, 48);
                    break;
                default:
                    Cshapes.DarkColor = Color.FromArgb(255, 255, 255);
                    Cshapes.MediumColor = Color.FromArgb(250, 235, 215);
                    Cshapes.LightColor = Color.FromArgb(255, 222, 173);
                    break;
            }

            return Cshapes;
        }
    }

   partial class VisualContender : VisualElements,IDisposable
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
        protected FlowLayoutPanel _ContMainPanel;
        public FlowLayoutPanel Vcontender
        {
            get
            {
                if (_ContMainPanel == null)
                {
                    _ContMainPanel = new FlowLayoutPanel();
                    _ContMainPanel.Size = ContMainPanel_Size;
                    _ContMainPanel.BackColor = BeltShapes.MediumColor;
                    _ContMainPanel.RightToLeft = RightToLeft.Yes;
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

        // Cont Belt Factor Shape
        protected Label _BeltFactorShape;
        public Label BeltFactorShape
        {
            get
            {
                if (_BeltFactorShape == null)
                {
                    _BeltFactorShape = GetLabel();
                    _BeltFactorShape.Font = new Font("Microsoft Sans Serif", 9, FontStyle.Regular);
                    _BeltFactorShape.Size = BeltFactorShape_Size;
                    // Factor Color
                    if (Contender.IsAllowedBeltGradeAbove == true && (Contenders.ContndersGeneral.BeltsEnum)Contender.Belt != Contenders.ContndersGeneral.BeltsEnum.black)
                    {
                        var ColorAbove = new BeltColors();
                        ColorAbove = GetBeltShades((Contenders.ContndersGeneral.BeltsEnum)Contender.Belt+1000);
                        _BeltFactorShape.BackColor = ColorAbove.DarkColor;
                        _BeltFactorShape.Text = FactorSign;
                    }
                    else
                    {
                        _BeltFactorShape.Text = NoFactorSign;
                        _BeltFactorShape.BackColor = BeltShapes.DarkColor;
                    }
                    return _BeltFactorShape;
                }

                else
                {
                    return _BeltFactorShape;
                }
            }
            set
            {
                _BeltFactorShape = value;
            }
        }
        public static Size BeltFactorShape_Size { get; set; } = new Size(25, 31);

        // Cont Weight Shape
        protected Label _ExactWeightShape;
        public Label ExactWeightShape
        {
            get
            {
                if (_ExactWeightShape == null)
                {
                    _ExactWeightShape = GetLabel();
                    _ExactWeightShape.Size = ExactWeightShape_Size;
                    _ExactWeightShape.BackColor = BeltShapes.LightColor;
                    _ExactWeightShape.Text = Contender.Weight.ToString() + " " + "ק" + "\"" + "ג";
                    return _ExactWeightShape;
                }

                else
                {
                    return _ExactWeightShape;
                }
            }
            set
            {
                _ExactWeightShape = value;
            }
        }
        public Size ExactWeightShape_Size { get; private set; } = new Size(50,31);

        // Cont Weight Category panel
        protected FlowLayoutPanel _WeightCatPanel;
             public FlowLayoutPanel WeightCatPanel
        {
            get
            {
                if (_WeightCatPanel == null)
                {
                    _WeightCatPanel = GetFpanel();
                    _WeightCatPanel.Size = new Size(100,31);
                    AddControlsToWeightCatPanel(ref _WeightCatPanel);
                    return _WeightCatPanel;
                }

                else
                {
                    return _WeightCatPanel;
                }
            }

            set { _WeightCatPanel = value; }
        }

        // Cont age Category Panel
        protected FlowLayoutPanel _AgeCatPanel;
        public FlowLayoutPanel AgeCatPanel
        {
            get
            {
                if (_AgeCatPanel == null)
                {
                    _AgeCatPanel = GetFpanel();
                    _AgeCatPanel.Size = new Size(90, 31);
                    AddControlsToAgeCatPanel(ref _AgeCatPanel);
                    return _AgeCatPanel;
                }

                else
                {
                    return _AgeCatPanel;
                }
            }

            set { _AgeCatPanel = value; }
        }

        // Cont Personal Data Panel
        protected FlowLayoutPanel _PersonalDataPanel;
        public FlowLayoutPanel PersonalDataPanel
        {
            get
            {
                if (_PersonalDataPanel == null)
                {
                    _PersonalDataPanel = GetFpanel();
                    _PersonalDataPanel.Size = new Size(177, 31);
                    _PersonalDataPanel.Padding = new Padding(2, 2, 2, 2);
                    AddControlsToPersonalDataPanel(ref _PersonalDataPanel);
                    return _PersonalDataPanel;
                }

                else
                {
                    return _PersonalDataPanel;
                }
            }

            set { _PersonalDataPanel = value; }
        }

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

                    _BtnShowContData.Size = new Size(26,31);
                    _BtnShowContData.Font = new Font("ARIAL", 16, FontStyle.Bold);
                    _BtnShowContData.Text = "⁞";
                    _BtnShowContData.Cursor = Cursors.Hand;
                    if(Contender.IsUseless== false)
                        _BtnShowContData.ForeColor = MartialArts.GlobalVars.Sys_DarkerGray;
                    else
                        _BtnShowContData.BackColor = Color.Red;

                    _BtnShowContData.Click += new EventHandler(ShowContData_Click);
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



        public void Init()
        {
            Vcontender.Controls.Add(BeltFactorShape);
            Vcontender.Controls.Add(ExactWeightShape);
            Vcontender.Controls.Add(WeightCatPanel);
            Vcontender.Controls.Add(AgeCatPanel);
            Vcontender.Controls.Add(PersonalDataPanel);
            Vcontender.Controls.Add(BtnShowContData);

        }

        public Label GetLabel()
        {
            Label lbl = new Label();
            lbl.Margin = new Padding(3, 3, 3, 3);
            lbl.RightToLeft = RightToLeft.Yes;
            lbl.TextAlign = ContentAlignment.MiddleCenter;
            lbl.ForeColor = ForeColor;
            return lbl;
        }

        public FlowLayoutPanel GetFpanel()
        {
            FlowLayoutPanel fp = new FlowLayoutPanel();
            fp.BackColor = BeltShapes.DarkColor;
            fp.Margin = new Padding(3, 3, 3, 3);
            fp.RightToLeft = RightToLeft.Yes;

            return fp;
        }

        private void AddControlsToWeightCatPanel(ref FlowLayoutPanel fp)
        {
            // factor label
            Label Factor = GetLabel();
            Factor.Size = new Size(15,26);
            Factor.Font = new Font("Microsoft Sans Serif", 9, FontStyle.Regular);
            Factor.BackColor = BeltShapes.MediumColor;
            if (Contender.IsAllowedWeightGradeAbove == true)
                Factor.Text = FactorSign;
            else
                Factor.Text = NoFactorSign;

            fp.Controls.Add(Factor);

            // Weight Cat Label
            Label WeightCat = GetLabel();
            WeightCat.Size = new Size(72, 26);
            WeightCat.BackColor = BeltShapes.LightColor;
            WeightCat.Text = Contender.GetWeightValue + " " + "ק" + "\"" + "ג";

            fp.Controls.Add(WeightCat);
        }

        private void AddControlsToAgeCatPanel(ref FlowLayoutPanel fp)
        {
            // factor label
            Label Factor = GetLabel();
            Factor.Size = new Size(15, 26);
            Factor.BackColor = BeltShapes.MediumColor;
            Factor.Font = new Font("Microsoft Sans Serif", 9, FontStyle.Regular);
            if (Contender.IsAllowedAgeGradeAbove == true && Contender.IsChild == true)
                Factor.Text = FactorSign;
            else
                Factor.Text = NoFactorSign;

            fp.Controls.Add(Factor);

            // age Cat Label
            Label AgeCat = GetLabel();
            AgeCat.Size = new Size(62, 26);
            AgeCat.BackColor = BeltShapes.LightColor;
            AgeCat.Text = Contender.GetAgeValue;

            fp.Controls.Add(AgeCat);
        }

        private void AddControlsToPersonalDataPanel(ref FlowLayoutPanel fp)
        {
            // Gender Factor Label
            Label Factor = GetLabel();
            Factor.Size = new Size(18, 12);
            Factor.Font = new Font("Microsoft Sans Serif", 8, FontStyle.Regular);
            Factor.BackColor = BeltShapes.MediumColor;
            Factor.Margin = new Padding(1, 1, 1, 1);
            if (Contender.IsMale == false && Contender.IsAllowedVersusMan == true)
                Factor.Text = FactorSign;
            else
                Factor.Text = NoFactorSign;

            fp.Controls.Add(Factor);

            // Private and Family name
            Label name = GetLabel();
            name.Size = new Size(151, 12);
            name.Text = Contender.FirstName + " " + Contender.LastName;
            name.Margin = new Padding(1, 1, 1, 1);


            if (Contender.IsMale == true)
                name.BackColor = BeltShapes.LightColor;
            else
                name.BackColor = Color.FromArgb(255, 105, 180);

            fp.Controls.Add(name);

            // Academy
            Label academy = GetLabel();
            academy.Margin = new Padding(1, 1, 1, 1);
            academy.Size = new Size(171, 12);
            academy.Text = Contender.AcademyName;
            academy.BackColor = BeltShapes.LightColor;
            fp.Controls.Add(academy);

        }


        public void MakeShadow()
        {
            if (_ContMainPanel == null)
                return;
            if (Contender.FirstName.Contains("פיני") == false)
                for (int anim = Vcontender.Height; anim >= 10; anim--)
                {
                    Vcontender.Size = new Size(Vcontender.Width, anim);
                    Application.DoEvents();
                    System.Threading.Thread.Sleep(10);
                }
            

        }

        public void Dispose()
        {
            
        }
    }

}