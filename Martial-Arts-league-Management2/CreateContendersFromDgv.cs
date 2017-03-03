using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
/// <summary>
/// g
/// </summary>
namespace MartialArts
{
    class CreateContendersFromDgv : IDisposable
    {
        DataGridView dgvMain;
        public CreateContendersFromDgv(ref DataGridView Dgv)
        {
            this.dgvMain = Dgv;
        }

        public bool Init2()
        {

            dgvMain.Invoke(new Action(DontAllowUserAddRows));

            if (dgvMain.Rows.Count < 2)
            {
                Helpers.ShowGenericPromtForm("חייבות להיות לפחות 2 שורות ברשימה");
                dgvMain.Invoke(new Action(AllowUserAddRows));
                return false;
            }
            // check if datagridview has errors
            if (HasErrorText() == true)
            {
                Helpers.ShowGenericPromtForm("השדות ברשימה אינם תקינים, מעבר עם העכבר על הסימון האדום בצד ימין יציג את הנתון החסר");
                dgvMain.Invoke(new Action(AllowUserAddRows));
                return false;
            }

            // double check (in special cases can be null cell without error text)
            if (IsThereNullCell() == true)
            {
                Helpers.ShowGenericPromtForm("השדות ברשימה אינם תקינים, מעבר עם העכבר על הסימון האדום בצד ימין יציג את הנתון החסר");
                dgvMain.Invoke(new Action(AllowUserAddRows));
                return false;
            }

            GlobalVars.ListOfContenders = new List<Contenders.Contender>();

            bool isok = true;
            // itirate and add contenders to list
            for (int i = 0; i < dgvMain.Rows.Count; i++)
            {
                Contenders.Contender con = new Contenders.Contender();

                con.FirstName = dgvMain.Rows[i].Cells[1].Value.ToString();
                con.LastName = dgvMain.Rows[i].Cells[2].Value.ToString();
                con.ID = dgvMain.Rows[i].Cells[0].Value.ToString();
                con.Email = dgvMain.Rows[i].Cells[7].Value.ToString();
                con.PhoneNumber = dgvMain.Rows[i].Cells[8].Value.ToString();
                con.DateOfBirth = "28/10/1980";
                con.AgeCategory = Contenders.ContndersGeneral.AgeGrades[dgvMain.Rows[i].Cells[6].Value.ToString()];
                con.IsMale = (dgvMain.Rows[i].Cells[12].Value.ToString() == "זכר") ? true : false;
                double r = 0;
                Double.TryParse(dgvMain.Rows[i].Cells[5].Value.ToString(), out r);
                con.Weight = r;
                con.IsChild = MartialArts.ExcelOperations.IsChild(con.AgeCategory);
                if (con.IsChild)
                    con.WeightCategory = Contenders.ContndersGeneral.ChildWeightCat[dgvMain.Rows[i].Cells[4].Value.ToString()];
                else
                    con.WeightCategory = Contenders.ContndersGeneral.AdultWeightCat[dgvMain.Rows[i].Cells[4].Value.ToString()];

                con.Belt = Contenders.ContndersGeneral.GetBelt(dgvMain.Rows[i].Cells[3].Value.ToString());
                con.AcademyName = dgvMain.Rows[i].Cells[9].Value.ToString();
                con.CoachName = dgvMain.Rows[i].Cells[10].Value.ToString();
                con.CoachPhone = dgvMain.Rows[i].Cells[11].Value.ToString();
                con.IsAllowedWeightGradeAbove = Convert.ToBoolean(dgvMain.Rows[i].Cells[16].Value);
                con.IsAllowedAgeGradeAbove = Convert.ToBoolean(dgvMain.Rows[i].Cells[14].Value);
                con.IsAllowedBeltGradeAbove = Convert.ToBoolean(dgvMain.Rows[i].Cells[15].Value);
                con.IsAllowedVersusMan = Convert.ToBoolean(dgvMain.Rows[i].Cells[13].Value);
                GlobalVars.ListOfContenders.Add(con);
            }

            dgvMain.Invoke(new Action(AllowUserAddRows));
            return true;
        }

        private void AllowUserAddRows()
        {
            dgvMain.AllowUserToAddRows = true;
        }

        private void DontAllowUserAddRows()
        {
            dgvMain.AllowUserToAddRows = false;
        }

        public bool HasErrorText()
        {
            bool hasErrorText = false;
            //replace this.dataGridView1 with the name of your datagridview control
            foreach (DataGridViewRow row in this.dgvMain.Rows)
            {

                if (row.ErrorText.Length > 0)
                {
                    hasErrorText = true;
                    break;
                }

            }

            return hasErrorText;
        }

        private bool IsThereNullCell()
        {
            // its enough to check if rows are not null because it has passed the data validations of dgvMain_CellValidating() event
            for (int i = 0; i < dgvMain.Columns.Count; i++)
            {


                for (int j = 0; j < dgvMain.Rows.Count; j++)
                {
                    if (!dgvMain.Columns[i].CellType.ToString().Contains("DataGridViewComboBoxCell"))
                    {
                        if (string.IsNullOrEmpty(dgvMain.Rows[j].Cells[i].ToString()))
                        {
                            return true;
                        }
                    }
                }
            }

            return false;
        }


        List<string> Defects = new List<string>();
        public bool Init()
        {

            dgvMain.Invoke(new Action(DontAllowUserAddRows));

            if (dgvMain.Rows.Count < 2)
            {
                Helpers.ShowGenericPromtForm("חייבות להיות לפחות 2 שורות ברשימה");
                dgvMain.Invoke(new Action(AllowUserAddRows));
                return false;
            }

            //// check if datagridview has errors
            //if (HasErrorText() == true)
            //{
            //    Helpers.ShowGenericPromtForm("השדות ברשימה אינם תקינים, מעבר עם העכבר על הסימון האדום בצד ימין יציג את הנתון החסר");
            //    dgvMain.Invoke(new Action(AllowUserAddRows));
            //    return false;
            //}

            //// double check (in special cases can be null cell without error text)
            //if (IsThereNullCell() == true)
            //{
            //    Helpers.ShowGenericPromtForm("השדות ברשימה אינם תקינים, מעבר עם העכבר על הסימון האדום בצד ימין יציג את הנתון החסר");
            //    dgvMain.Invoke(new Action(AllowUserAddRows));
            //    return false;
            //}

            GlobalVars.ListOfContenders = new List<Contenders.Contender>();

            bool isok = true;
            // itirate and add contenders to list
            for (int i = 0; i < dgvMain.Rows.Count; i++)
            {
                Contenders.Contender con = new Contenders.Contender();

                con.FirstName = GetPureStringField(dgvMain.Rows[i].Cells[1].Value, "שם פרטי", out isok,i);
                con.LastName = GetPureStringField(dgvMain.Rows[i].Cells[2].Value, "שם משפחה", out isok,i);
                con.ID = GetMixedString(dgvMain.Rows[i].Cells[0].Value, "תעודת זהות", out isok,i);
                con.Email = GetPureStringField(dgvMain.Rows[i].Cells[7].Value, "אימייל", out isok,i);
                con.PhoneNumber = GetMixedString(dgvMain.Rows[i].Cells[8].Value, "מספר טלפון", out isok,i);
                con.DateOfBirth = "28/10/1980";
                con.AgeCategory = GetAgeCategory(dgvMain.Rows[i].Cells[6].Value, "קטגורית גיל", out isok,i);
                con.IsMale = GetGender(dgvMain.Rows[i].Cells[12].Value, "מגדר", out isok,i);
                con.Weight = GetWeight(dgvMain.Rows[i].Cells[5].Value, "משקל", out isok,i);
                con.IsChild = MartialArts.ExcelOperations.IsChild(con.AgeCategory);
                con.WeightCategory = GetWeightCategory(dgvMain.Rows[i].Cells[4].Value, "קטגורית משקל", con.IsChild, out isok,i);
                con.Belt = GetBelt(dgvMain.Rows[i].Cells[3].Value, "חגורה", out isok,i);
                con.AcademyName = GetFieldCanBeNull(dgvMain.Rows[i].Cells[9].Value, "אקדמיה", out isok);
                con.CoachName = GetFieldCanBeNull(dgvMain.Rows[i].Cells[10].Value, "מאמן", out isok);
                con.CoachPhone = GetFieldCanBeNull(dgvMain.Rows[i].Cells[11].Value, "טלפון מאמן", out isok);
                con.IsAllowedWeightGradeAbove = Convert.ToBoolean(dgvMain.Rows[i].Cells[16].Value);
                con.IsAllowedAgeGradeAbove = Convert.ToBoolean(dgvMain.Rows[i].Cells[14].Value);
                con.IsAllowedBeltGradeAbove = Convert.ToBoolean(dgvMain.Rows[i].Cells[15].Value);
                con.IsAllowedVersusMan = Convert.ToBoolean(dgvMain.Rows[i].Cells[13].Value);
                GlobalVars.ListOfContenders.Add(con);
            }

         

            if (Defects.Count <= 0)
            {

                return true;
            }
            else
            {
                dgvMain.Invoke(new Action(AllowUserAddRows)); // in order to make changes
                GlobalVars.ListOfContenders.Clear();
                GlobalVars.ListOfContenders = null;
                string ExDefects = "";
                foreach (string s in Defects)
                {
                    ExDefects += s + Environment.NewLine;
                }

                Martial_Arts_league_Management2.PromtForm p = new Martial_Arts_league_Management2.PromtForm(ExDefects, true, "התוכנית תפסיק את פעולתה:" + " " + "נדרשים תיקונים לקובץ נתונים", true);
                p.ShowDialog();
                return false;
            }
        }


        private string GetPureStringField(object value, string HebparameterName, out bool isok,int row)
        {

            var s = value;

            if (Helpers.IsString(s) == false)
            {
                // Helpers.DefaultMessegeBox(" השדה " + HebparameterName + " " +" בשורה "  + row + " " + "חייב להיות בשפה עברית או לועזית" + Environment.NewLine + "התוכנית תפסיק את פעולתה", "נתון חסר", System.Windows.Forms.MessageBoxIcon.Warning);
                Defects.Add(" השדה " + HebparameterName + " " + " בשורה " + row + " " + "חייב להיות בשפה עברית או לועזית" + Environment.NewLine);
                isok = false;
                return string.Empty;
            }

            if (s == "")
            {
                // Helpers.DefaultMessegeBox(" השדה " + HebparameterName + " " + " בשורה " + row + " " + "לא יכול להיות ריק" + Environment.NewLine + "התוכנית תפסיק את פעולתה", "נתון חסר", System.Windows.Forms.MessageBoxIcon.Warning);
                Defects.Add(" השדה " + HebparameterName + " " + " בשורה " + row + " " + "לא יכול להיות ריק" + Environment.NewLine);
                isok = false;
                return string.Empty;
            }

            if (((string)s).Trim().Length <= 1)
            {
                //  Helpers.DefaultMessegeBox(" השדה " + HebparameterName + " " + " בשורה " + row + " " + "חייב להכיל יותר מאות אחת" + Environment.NewLine + "התוכנית תפסיק את פעולתה", "נתון חסר", System.Windows.Forms.MessageBoxIcon.Warning);
                Defects.Add(" השדה " + HebparameterName + " " + " בשורה " + row + " " + "חייב להכיל יותר מאות אחת" + Environment.NewLine);
                isok = false;
                return string.Empty;
            }

            isok = true;
            return ((string)s).Trim();
        }

        private string GetMixedString(object value, string HebparameterName, out bool isok,int row)
        {
            var s = value;
            string result = System.Convert.ToString(s);

            if (result.Trim().Length <= 6)
            {
                // Helpers.DefaultMessegeBox(" השדה " + HebparameterName + " " + " בשורה " + row + " " + "חייב להכיל יותר משישה תווים" + Environment.NewLine + "התוכנית תפסיק את פעולתה", "נתון חסר", System.Windows.Forms.MessageBoxIcon.Warning);
                Defects.Add(" השדה " + HebparameterName + " " + " בשורה " + row + " " + "חייב להכיל יותר משישה תווים" + Environment.NewLine);
                isok = false;
                return result;
            }

            isok = true;
            return result.Trim();

        }
        private int GetAgeCategory(object value, string HebparameterName, out bool isok,int row)
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
            Defects.Add(" השדה " + HebparameterName + " " + " בשורה " + row + " " + "מכיל קטגוריית גיל לא חוקית" + Environment.NewLine);
            return 0;
        }

        private bool GetGender(object value, string HebparameterName, out bool isok,int row)
        {
            var s = value;
            string result = System.Convert.ToString(s);

            if (result.Trim() != "זכר" && result.Trim() != "נקבה")
            {
                isok = false;
                Defects.Add(" השדה " + HebparameterName + " " + " בשורה " + row + " " + "חייב להכיל את הערכים זכר או נקבה בלבד" + Environment.NewLine);
                return false;
            }

            isok = true;
            return (result.Trim() == "זכר");
        }

        private double GetWeight(object value, string HebparameterName, out bool isok,int row)
        {

            var s = value;
            string result = System.Convert.ToString(s);

            if (result.IsNumeric() == false)
            {
                Defects.Add(" השדה " + HebparameterName + " " + " בשורה " + row + " " + "חייב להכיל מספר כלשהו שמייצג משקל" + Environment.NewLine);
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
                Defects.Add(" השדה " + HebparameterName + " " + " בשורה " + row + " " + "חייב להכיל מספר כלשהו שמייצג משקל" + Environment.NewLine);
                return 0;
            }
        }

        public static bool IsChild(int AgeGrade)
        {
            return AgeGrade < 450;
        }


        private int GetWeightCategory(object value, string HebparameterName, bool IsChild, out bool isok,int row)
        {
            isok = false;
            var s = value;
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
            Defects.Add(" השדה " + HebparameterName + " " + " בשורה " + row + " " + "מכיל קטגוריית משקל לא חוקית" + Environment.NewLine);
            return 0;
        }

        private int GetBelt(object value, string HebparameterName, out bool isok,int row)
        {
            isok = true;
            var s = value;
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
                    Defects.Add(" השדה " + HebparameterName + " " + " בשורה " + row + " " + "חייב להכיל דרגת חגורה חוקית" + Environment.NewLine);
                    return 0;
            }

        }

        private string GetFieldCanBeNull(object value, string HebparameterName, out bool isok)
        {
            isok = true;
            var s = value;
            if (s == null)
                return "";


            string result = System.Convert.ToString(s);
            return result.Trim();

        }

        public void Dispose()
        {

        }
    }
}
