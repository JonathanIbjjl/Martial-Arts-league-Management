using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace MartialArts
{
    public partial class AddContender : Form
    {
        // the new contender object to use outside the class if it is valid it will not be null and dialiogResult will be OK
        public Contenders.Contender NewContender { get; set; }
        protected bool EditContMode { get; set; } = false;
        private bool IsChild;
        public List<string> AcademiesListNames;
        public AddContender(bool ischild,List<string> Academies)
        {
            InitializeComponent();
            this.IsChild = ischild;
            this.AcademiesListNames = Academies;
        }

        public AddContender(bool ischild, List<string> Academies,Contenders.Contender contenderToEdit)
        {
            InitializeComponent();
            this.IsChild = ischild;
            this.AcademiesListNames = Academies;
            this.EditContMode = true;
            NewContender = contenderToEdit;
        }

        private void AddContender_Load(object sender, EventArgs e)
        {
            ColorControls();
            LoadComboBoxes();
            GeneralLook();
            if (EditContMode == true)
            {
                LoadContenderToEdit();
                btnSave.Image = null;
                btnSave.Text = "שמור שינויים";
                this.Text = "עריכת מתחרה";
            }
        }

        private void LoadContenderToEdit()
        {
            try
            {
                txtFirstName.Text = NewContender.FirstName;
                txtLastName.Text = NewContender.LastName;
                txtID.Text = NewContender.ID;
                txtEmail.Text = NewContender.Email;
                txtPhone.Text = NewContender.PhoneNumber;
                ComboAge.SelectedItem = NewContender.GetAgeValue;
                radMale.Checked = NewContender.IsMale;
                txtWeight.Text = NewContender.Weight.ToString();
                comboWeight.SelectedItem = NewContender.GetWeightValue;
                comboBelt.SelectedItem = NewContender.HebrewBeltColor;
                ComboAcademy.Text = NewContender.AcademyName;
                txtCoachName.Text = NewContender.CoachName;
                txtCoachPhone.Text = NewContender.CoachPhone;
                chWeightFac.Checked = NewContender.IsAllowedWeightGradeAbove;
                chkAgeFac.Checked = NewContender.IsAllowedAgeGradeAbove;
                chkBeltFac.Checked = NewContender.IsAllowedBeltGradeAbove;
                chkSexFac.Checked = NewContender.IsAllowedVersusMan;
            }

            catch
            {
                // for safty only
                Helpers.ShowGenericPromtForm("המתחרה שברצונך לערוך מכיל נתונים לא חוקיים");
                this.Close();
            }
        }

        private void GeneralLook()
        {
            dtpDateBirth.Format = DateTimePickerFormat.Custom;
            dtpDateBirth.CustomFormat = "dd/MM/yyyy";
        }

        private void LoadComboBoxes()
        {
            // belts
            var beltList = new List<string>() { "לבנה", "אפורה", "צהובה", "כתומה", "ירוקה", "כחולה", "סגולה", "חומה", "שחורה" };
            comboBelt.DataSource = beltList;
            comboBelt.AutoCompleteMode = AutoCompleteMode.Suggest;
            comboBelt.AutoCompleteSource = AutoCompleteSource.ListItems;

            // weight
            var weightList = new List<string>();
            if (this.IsChild == true)
                weightList = Contenders.ContndersGeneral.ChildWeightCat.Keys.ToList();
            else
                weightList = Contenders.ContndersGeneral.AdultWeightCat.Keys.ToList();
            comboWeight.DataSource = weightList;
            comboWeight.AutoCompleteMode = AutoCompleteMode.Suggest;
            comboWeight.AutoCompleteSource = AutoCompleteSource.ListItems;

            // age
            var ageList = new List<string>();
            ageList = Contenders.ContndersGeneral.GetAgeValues(IsChild);
            ComboAge.DataSource = ageList;
            ComboAge.AutoCompleteMode = AutoCompleteMode.Suggest;
            ComboAge.AutoCompleteSource = AutoCompleteSource.ListItems;

            // academies
            ComboAcademy.DataSource = AcademiesListNames;
            ComboAcademy.AutoCompleteMode = AutoCompleteMode.Suggest;
            ComboAcademy.AutoCompleteSource = AutoCompleteSource.ListItems;
        }

        private void ColorControls()
        {
            // role back error signs
            lblMsg.ForeColor = Color.Red;
            lblMsg.BackColor = this.BackColor;
            lblMsg.Text = string.Empty;

            foreach (Control c in this.Controls)
            {
                ColorControl(c);
                foreach (Control cSun in c.Controls)
                {
                    ColorControl(cSun);
                }
            }

            // exceptions
            lblkilo.ForeColor = Color.FromArgb(150, 150, 150);
            lblMsg.ForeColor = Color.Red;
        }

        private void ColorControl(Control c)
        {
            if (c is TextBox)
            {
                c.BackColor = MartialArts.GlobalVars.Sys_Yellow;
                c.ForeColor = MartialArts.GlobalVars.Sys_DarkerGray;
            }
            else if (c is ComboBox)
            {
                c.BackColor = MartialArts.GlobalVars.Sys_Yellow;
                c.ForeColor = MartialArts.GlobalVars.Sys_DarkerGray;
                c.RightToLeft = RightToLeft.Yes;
            }

            else if (c is Label)
            {
                c.ForeColor = MartialArts.GlobalVars.Sys_White;
            }

            else if (c is RadioButton)
            {
                c.ForeColor = MartialArts.GlobalVars.Sys_Yellow;
            }

            else if (c is CheckBox)
            {
                c.ForeColor = MartialArts.GlobalVars.Sys_Yellow;
            }


        }

        private void button1_Click(object sender, EventArgs e)
        {
            // role back control error signs
            ColorControls();
            Contenders.Contender cont = new Contenders.Contender(GlobalVars.ChoosenWeightCategory);
            if (CreateAndValidateContender(ref cont) == false)
            {
                return;
            }
            else
            {
               
                this.DialogResult = DialogResult.OK;
                if (EditContMode != true)
                {
                    NewContender = cont;
                    Helpers.ShowGenericPromtForm("המתחרה החדש נוצר בהצלחה");
                }
                else
                {
                    cont.SystemID = NewContender.SystemID; // take the old system id from the list
                    NewContender = cont;
                    Helpers.ShowGenericPromtForm("המתחרה נערך בהצלחה");
                }

                this.Close();
            }

        }

        private bool CreateAndValidateContender(ref Contenders.Contender contender)
        {

            // validate the controls first
            if (ValidateControls() == false)
                return false;

            bool isok;

            // first name
            contender.FirstName = GetPureStringField(txtFirstName.Text.Trim(), "שם פרטי", out isok);
            if (isok == false)
            {
                txtFirstName.BackColor = GlobalVars.Sys_Red;
                lblMsg.BackColor = Color.FromArgb(20, 20, 20);
                contender = null;
                return false;
            }

            // last name
            contender.LastName = GetPureStringField(txtLastName.Text.Trim(), "שם משפחה", out isok);
            if (isok == false)
            {
                txtLastName.BackColor = GlobalVars.Sys_Red;
                lblMsg.BackColor = Color.FromArgb(20, 20, 20);
                contender = null;
                return false;
            }

            // ID
            contender.ID = GetMixedString(txtID.Text.Trim(), "תעודת זהות", out isok);
            if (isok == false)
            {
                txtID.BackColor = GlobalVars.Sys_Red;
                lblMsg.BackColor = Color.FromArgb(20, 20, 20);
                contender = null;
                return false;
            }

            // email
            contender.Email = GetPureStringField(txtEmail.Text.Trim(), "אימייל", out isok);
            if (isok == false)
            {
                txtEmail.BackColor = GlobalVars.Sys_Red;
                lblMsg.BackColor = Color.FromArgb(20, 20, 20);
                contender = null;
                return false;
            }

            // phone number
            contender.PhoneNumber = GetMixedString(txtPhone.Text.Trim(), "טלפון", out isok);
            if (isok == false)
            {
                txtPhone.BackColor = GlobalVars.Sys_Red;
                lblMsg.BackColor = Color.FromArgb(20, 20, 20);
                contender = null;
                return false;
            }

            // date of birth
            contender.DateOfBirth = dtpDateBirth.Value.Date.ToString();

            // age category
            contender.AgeCategory = GetAgeCategory(ComboAge.SelectedItem.ToString().Trim(), "קטגורית גיל", out isok);
            if (isok == false)
            {
                ComboAge.BackColor = GlobalVars.Sys_Red;
                lblMsg.BackColor = Color.FromArgb(20, 20, 20);
                contender = null;
                return false;
            }

            // gender (validated beofore)
            contender.IsMale = radMale.Checked;

            // exact weight
            contender.Weight = GetWeight(txtWeight.Text, "משקל", out isok);
            if (isok == false)
            {
                txtWeight.BackColor = GlobalVars.Sys_Red;
                lblMsg.BackColor = Color.FromArgb(20, 20, 20);
                contender = null;
                return false;
            }

            // child or adult
            contender.IsChild = IsChildCat(contender.AgeCategory);

            // weight category
            contender.WeightCategory = GetWeightCategory("קטגורית משקל", contender.IsChild, out isok);
            if (isok == false)
            {
                comboWeight.BackColor = GlobalVars.Sys_Red;
                lblMsg.BackColor = Color.FromArgb(20, 20, 20);
                contender = null;
                return false;
            }

            // belt category
            contender.Belt = GetBelt( "חגורה", out isok);
            if (isok == false)
            {
                comboBelt.BackColor = GlobalVars.Sys_Red;
                lblMsg.BackColor = Color.FromArgb(20, 20, 20);
                contender = null;
                return false;
            }

            // academy name
            contender.AcademyName = ComboAcademy.Text.ToString().Trim();

            // coach name
            contender.CoachName = txtCoachName.Text.ToString().Trim();

            // coach phone
            contender.CoachPhone = GetMixedString(txtCoachPhone.Text.Trim(), "טלפון מאמן", out isok);
            if (isok == false)
            {
                txtCoachPhone.BackColor = GlobalVars.Sys_Red;
                lblMsg.BackColor = Color.FromArgb(20, 20, 20);
                contender = null;
                return false;
            }

            // factors
            contender.IsAllowedWeightGradeAbove = chWeightFac.Checked;
            contender.IsAllowedAgeGradeAbove = chkAgeFac.Checked;
            contender.IsAllowedBeltGradeAbove = chkBeltFac.Checked;
            contender.IsAllowedVersusMan = chkSexFac.Checked;

            // for new contenders only
            if (EditContMode == false)
                contender.CreatedAfterBracketBuilder = true;

            return true;
        }

        #region "Validations"
        private string GetPureStringField(object value, string HebparameterName, out bool isok)
        {

            var s = value;

            if (Helpers.IsString(s) == false)
            {
               
                lblMsg.Text = (" השדה " + HebparameterName + " " + "חייב להיות בשפה עברית או לועזית" );
                isok = false;                                     
                return string.Empty;                              
            }                                                     
                                                                  
            if ((string)s == "")                                  
            {                                                     
                                                                  
                lblMsg.Text = (" השדה " + HebparameterName + " " + "לא יכול להיות ריק" );
                isok = false;                                     
                return string.Empty;                              
            }                                                     
                                                                  
            if (((string)s).Trim().Length <= 1)                   
            {                                                     
                lblMsg.Text = (" השדה " + HebparameterName + " " + "חייב להכיל יותר מאות אחת");
                isok = false;
                return string.Empty;
            }

            isok = true;
            return ((string)s).Trim();
        }

        private string GetMixedString(object value, string HebparameterName, out bool isok)
        {
            var s = value;
            string result = System.Convert.ToString(s);

            if (result.Trim().Length <= 6)
            {
              
               lblMsg.Text=(" השדה " + HebparameterName + " " + "חייב להכיל יותר משישה תווים" + Environment.NewLine);
                isok = false;
                return result;
            }

            isok = true;
            return result.Trim();

        }

        private int GetAgeCategory(object value, string HebparameterName, out bool isok)
        {
            isok = false;
            var s = value;
            string result = System.Convert.ToString(s);

            foreach (KeyValuePair<string, int> k in Contenders.ContndersGeneral.AgeGrades)
            {
                if (result.Trim().Contains(k.Key))
                {
                    isok = true;
                    return Contenders.ContndersGeneral.AgeGrades[k.Key];
                }
            }
            isok = false;
          lblMsg.Text = (" השדה " + HebparameterName + " " + "מכיל קטגוריית גיל לא חוקית" + Environment.NewLine);
            return 0;
        }

        private double GetWeight(object value, string HebparameterName, out bool isok)
        {

            var s = value;
            string result = System.Convert.ToString(s);

            if (result.IsNumeric() == false)
            {
               lblMsg.Text = (" השדה " + HebparameterName + " "+ "חייב להכיל מספר כלשהו שמייצג משקל" + Environment.NewLine);
                isok = false;
                return 0;
            }

            double intResult = Helpers.extractNumberFromString(result);

            if (intResult > 0)
            {
                isok = true;
                return intResult;
            }
            else
            {
                isok = false;
                lblMsg.Text = (" השדה " + HebparameterName + " " +  "חייב להכיל מספר כלשהו שמייצג משקל" + Environment.NewLine);
                return 0;
            }
        }

        public static bool IsChildCat(int AgeGrade)
        {
            return AgeGrade < 450;
        }

        private int GetWeightCategory( string HebparameterName, bool IsChild, out bool isok)
        {
            isok = false;
            var s = comboWeight.SelectedItem.ToString().Trim();
            string result = System.Convert.ToString(s);

            if (IsChild) // check against child dictionary
            {
                foreach (KeyValuePair<string, int> k in Contenders.ContndersGeneral.ChildWeightCat)
                {
                    if (result.Trim().Contains(k.Key))
                    {
                        isok = true;
                        return Contenders.ContndersGeneral.ChildWeightCat[k.Key];
                    }
                }

            }

            else // check against adult dictionary
            {
                foreach (KeyValuePair<string, int> k in Contenders.ContndersGeneral.AdultWeightCat)
                {
                    if (result.Trim().Contains(k.Key))
                    {
                        isok = true;
                        return Contenders.ContndersGeneral.AdultWeightCat[k.Key];
                    }
                }
            }
            isok = false;
           lblMsg.Text = (" השדה " + HebparameterName + " " + "מכיל קטגוריית משקל לא חוקית" + Environment.NewLine);
            return 0;
        }

        private int GetBelt(string HebparameterName, out bool isok)
        {
            isok = true;
            var s = comboBelt.SelectedItem.ToString().Trim();
            string result = System.Convert.ToString(s);

            switch (result.Trim())
            {
                case "לבנה":
                    return (int)Contenders.ContndersGeneral.BeltsEnum.white;

                case "אפורה":
                    return (int)Contenders.ContndersGeneral.BeltsEnum.gray;

                case "צהובה":
                    return (int)Contenders.ContndersGeneral.BeltsEnum.yellow;

                case "כתומה":
                    return (int)Contenders.ContndersGeneral.BeltsEnum.orange;

                case "ירוקה":
                    return (int)Contenders.ContndersGeneral.BeltsEnum.green;

                case "כחולה":
                    return (int)Contenders.ContndersGeneral.BeltsEnum.blue;

                case "סגולה":
                    return (int)Contenders.ContndersGeneral.BeltsEnum.purpule;

                case "חומה":
                    return (int)Contenders.ContndersGeneral.BeltsEnum.brown;

                case "שחורה":
                    return (int)Contenders.ContndersGeneral.BeltsEnum.black;

                default:
                    isok = false;
                    lblMsg.Text = (" השדה " + HebparameterName + " " + "חייב להכיל דרגת חגורה חוקית" + Environment.NewLine);
                    return 0;
            }
        }

        // validate the controls
        private bool ValidateControls()
        {
            // check that combo boxes have selected items and radio buttons
            if (comboBelt.SelectedIndex == -1)
            {
                lblMsg.Text = "יש לבחור צבע חגורה מתוך רשימת הגלילה";
                comboBelt.BackColor = GlobalVars.Sys_Red;
                lblMsg.BackColor = Color.FromArgb(20, 20, 20);
                return false;
            }
            else if (ComboAge.SelectedIndex == -1)
            {
                lblMsg.Text = "יש לבחור קטגוריית גיל מתוך רשימת הגלילה";
                ComboAge.BackColor = GlobalVars.Sys_Red;
                lblMsg.BackColor = Color.FromArgb(20, 20, 20);
                return false;
            }
            else if (comboWeight.SelectedIndex == -1)
            {
                lblMsg.Text = "יש לבחור קטגוריית משקל מתוך רשימת הגלילה";
                comboWeight.BackColor = GlobalVars.Sys_Red;
                lblMsg.BackColor = Color.FromArgb(20, 20, 20);
                return false;
            }
            else if (ComboAcademy.Text.Length < 3)
            {
                lblMsg.Text = "שם אקדמיה חייב להכיל לפחות 3 תווים";
                ComboAcademy.BackColor = GlobalVars.Sys_Red;
                lblMsg.BackColor = Color.FromArgb(20, 20, 20);
                return false;
            }
            else if (radFemale.Checked == false && radMale.Checked == false)
            {
                lblMsg.Text = "יש לבחור מגדר";
                lblMsg.BackColor = Color.FromArgb(20, 20, 20);
                return false;
            }

            else if (txtCoachName.ToString().Length <2)
            {
                lblMsg.Text = "שם מאמן חייב להכיל לפחות 2 תווים";
                lblMsg.BackColor = Color.FromArgb(20, 20, 20);
                return false;
            }


            return true;
        }

        #endregion

        private void radMale_CheckedChanged(object sender, EventArgs e)
        {
            if (radMale.Checked == true)
            {
                chkSexFac.Checked = false;
                chkSexFac.Enabled = false;
            }
            else
            {
                chkSexFac.Enabled = true;
            }
        }
    }
}
