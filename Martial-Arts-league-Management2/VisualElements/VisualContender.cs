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
    [Serializable]
    class VisualElements
    {
        [Serializable]
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
                    Cshapes.LightColor = Color.FromArgb(255, 255, 51);
                    break;
                case Contenders.ContndersGeneral.BeltsEnum.purpule:
                    Cshapes.DarkColor = Color.FromArgb(51, 0, 51);
                    Cshapes.MediumColor = Color.FromArgb(102, 0, 102);
                    Cshapes.LightColor = Color.FromArgb(112, 48, 160);
                    break;
                case Contenders.ContndersGeneral.BeltsEnum.orange:
                    Cshapes.DarkColor = Color.FromArgb(153, 76, 0);
                    Cshapes.MediumColor = Color.FromArgb(204, 102, 0);
                    Cshapes.LightColor = Color.FromArgb(255, 128, 0);
                    break;
                case Contenders.ContndersGeneral.BeltsEnum.green:
                    Cshapes.DarkColor = Color.FromArgb(0, 102, 0);
                    Cshapes.MediumColor = Color.FromArgb(0, 204, 0);
                    Cshapes.LightColor = Color.FromArgb(0, 255, 0);
                    break;
                case Contenders.ContndersGeneral.BeltsEnum.gray:
                    Cshapes.DarkColor = Color.FromArgb(64, 64, 64);
                    Cshapes.MediumColor = Color.FromArgb(96, 96, 96);
                    Cshapes.LightColor = Color.FromArgb(160, 160, 160);
                    break;
                case Contenders.ContndersGeneral.BeltsEnum.brown:
                    Cshapes.DarkColor = Color.FromArgb(51, 25, 0);
                    Cshapes.MediumColor = Color.FromArgb(102, 51, 0);
                    Cshapes.LightColor = Color.FromArgb(153, 76, 0);
                    break;
                case Contenders.ContndersGeneral.BeltsEnum.blue:
                    Cshapes.DarkColor = Color.FromArgb(0, 0, 153);
                    Cshapes.MediumColor = Color.FromArgb(0, 0, 204);
                    Cshapes.LightColor = Color.FromArgb(0, 128, 255);
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

    partial class VisualContender : VisualElements,IDisposable,Contenders.IContender
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
                    //_ContMainPanel.DoubleBuffered_FlPanel(true); TODO: DELETE
                    // tha name will be used in drag and drop
                    _ContMainPanel.Name = "Cont " + Contender.SystemID.ToString();
                    _ContMainPanel.AllowDrop = true;
                    _ContMainPanel.MouseDown += new MouseEventHandler(Vcont_MouseDown);
                    _ContMainPanel.DragEnter += new DragEventHandler(Vcont_DragEnter);
                    _ContMainPanel.Size = ContMainPanel_Size;
                    _ContMainPanel.BackColor = BeltShapes.MediumColor;
                 //   _ContMainPanel.Margin = new Padding(6, 6, 6, 6);
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
                    //_AgeCatPanel.DoubleBuffered_FlPanel(true); TODO: DELETE
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
                    //_PersonalDataPanel.DoubleBuffered_FlPanel(true); TODO: DELETE
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
            lbl.Dock = DockStyle.None;
            //lbl.DoubleBuffered_Label(true); TODO: DELETE
            lbl.Margin = new Padding(3, 3, 3, 3);
            lbl.RightToLeft = RightToLeft.Yes;
            lbl.TextAlign = ContentAlignment.MiddleCenter;
            lbl.ForeColor = ForeColor;
            lbl.AllowDrop = true;
            lbl.DragEnter += new DragEventHandler(Vcont_DragEnter);
            lbl.MouseDown += new MouseEventHandler(Vcont_MouseDown);
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
                name.BackColor = Color.FromArgb(255,0,127);
 
            fp.Controls.Add(name);

            // Academy
            Label academy = GetLabel();
            academy.Margin = new Padding(1, 1, 1, 1);
            academy.Size = new Size(171, 12);
            academy.Text = Contender.AcademyName;
            academy.BackColor = BeltShapes.LightColor;
            fp.Controls.Add(academy);

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
            if (_WeightCatPanel != null)
            {
                foreach (Control x in _WeightCatPanel.Controls)
                {
                    x.Dispose();
                }

                _WeightCatPanel.Dispose();
            }


            if (_BeltFactorShape != null)
            {

                foreach (Control x in _BeltFactorShape.Controls)
                {
                    x.Dispose();
                }

                _BeltFactorShape.Dispose();
            }

            if (_PersonalDataPanel != null)
            {
                foreach (Control x in _PersonalDataPanel.Controls)
                {
                    x.Dispose();
                }

                _PersonalDataPanel.Dispose();
            }

            if (_BtnShowContData != null)
            {
                foreach (Control x in _BtnShowContData.Controls)
                {
                    x.Dispose();
                }

                _BtnShowContData.Dispose();
            }

            if (_AgeCatPanel != null)
            {
                foreach (Control x in _AgeCatPanel.Controls)
                {
                    x.Dispose();
                }

                _AgeCatPanel.Dispose();
            }

            if (_WeightCatPanel != null)
            {
                foreach (Control x in _WeightCatPanel.Controls)
                {
                    x.Dispose();
                }

                _WeightCatPanel.Dispose();
            }

            if (_ExactWeightShape != null)
            {
                foreach (Control x in _ExactWeightShape.Controls)
                {
                    x.Dispose();
                }

                _ExactWeightShape.Dispose();
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
    }

}