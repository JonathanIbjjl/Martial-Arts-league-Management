using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Office.Interop.Excel;
using System.Runtime.InteropServices;
using System.Drawing;
using System.Diagnostics;
namespace Martial_Arts_league_Management2
{
    partial class ExcelOperations
    {
       
        private bool SetContender()
        {
           
            GlobalVars.ListOfContenders = new List<Contenders.Contender>();
            // for out parameter
            bool isok = true;

   
                // itirate and add contenders to list
                for (int i = 2; i <= LastRow; i++)
                {
                    Contenders.Contender con = new Contenders.Contender();

                    con.FirstName = GetPureStringField(i, ContenderObj.HeadersDictionary["FirstName"], "שם פרטי", out isok);
                    con.LastName = GetPureStringField(i, ContenderObj.HeadersDictionary["LastName"], "שם משפחה", out isok);
                    con.ID = GetMixedString(i, ContenderObj.HeadersDictionary["ID"], "תעודת זהות", out isok);
                    con.Email = GetPureStringField(i, ContenderObj.HeadersDictionary["Email"], "אימייל", out isok);
                    con.PhoneNumber = GetMixedString(i, ContenderObj.HeadersDictionary["PhoneNumber"], "מספר טלפון", out isok);
                    con.DateOfBirth = GetFieldCanBeNull(i, ContenderObj.HeadersDictionary["DateOfBirth"], "תאריך לידה", out isok);
                    con.AgeCategory = GetAgeCategory(i, ContenderObj.HeadersDictionary["AgeCategory"], "קטגורית גיל", out isok);
                    con.IsMale = GetGender(i, ContenderObj.HeadersDictionary["IsMale"], "מגדר", out isok);
                    con.Weight = GetWeight(i, ContenderObj.HeadersDictionary["Weight"], "משקל", out isok);
                    con.IsChild = IsChild(con.AgeCategory);
                    con.WeightCategory = GetWeightCategory(i, ContenderObj.HeadersDictionary["WeightCategory"], "קטגורית משקל",con.IsChild, out isok);
                    con.Belt = GetBelt(i, ContenderObj.HeadersDictionary["Belt"], "חגורה", out isok);
                    con.AcademyName = GetFieldCanBeNull(i, ContenderObj.HeadersDictionary["AcademyName"], "אקדמיה", out isok);
                    con.AcademyNameNotInCombo = GetFieldCanBeNull(i, ContenderObj.HeadersDictionary["AcademyNameNotInCombo"], "אקדמיה", out isok);
                    con.CoachName = GetFieldCanBeNull(i, ContenderObj.HeadersDictionary["CoachName"], "מאמן", out isok);
                    con.CoachPhone = GetFieldCanBeNull(i, ContenderObj.HeadersDictionary["CoachPhone"], "טלפון מאמן", out isok);
                    con.IsAllowedWeightGradeAbove = GetBooleanQuestions(i, ContenderObj.HeadersDictionary["IsAllowedWeightGradeAbove"], "אישור דרגת משקל מעל", out isok);
                    con.IsAllowedAgeGradeAbove = GetBooleanQuestions(i, ContenderObj.HeadersDictionary["IsAllowedAgeGradeAbove"], "אישור דרגת גיל מעל", out isok);
                    con.IsAllowedBeltGradeAbove = GetBooleanQuestions(i, ContenderObj.HeadersDictionary["IsAllowedBeltGradeAbove"], "אישור דרגת חגורה מעל", out isok);
                    con.IsAllowedVersusMan = GetBooleanQuestions(i, ContenderObj.HeadersDictionary["IsAllowedVersusMan"], "אישור להתחרות מול בנים", out isok);
               
                    GlobalVars.ListOfContenders.Add(con);

                    // final check to see if there is no defect in that contender
                    if (isok == false)
                        return false;
                }



         
            if (isok == true)
                return true;
            else
                return false;

        }

        private string GetPureStringField(int row,int col,string HebparameterName,out bool isok)
        {

            var s = ExWs.Cells[row, col].value;

            if (Helpers.IsString(s)==false)
            {
                Helpers.DefaultMessegeBox(" השדה " + HebparameterName + " " +" בשורה "  + row + " " + "חייב להיות בשפה עברית או לועזית" + Environment.NewLine + "התוכנית תפסיק את פעולתה", "נתון חסר", System.Windows.Forms.MessageBoxIcon.Warning);
                isok = false;
                return string.Empty;
            }

           if (s == "")
            {
                Helpers.DefaultMessegeBox(" השדה " + HebparameterName + " " + " בשורה " + row + " " + "לא יכול להיות ריק" + Environment.NewLine + "התוכנית תפסיק את פעולתה", "נתון חסר", System.Windows.Forms.MessageBoxIcon.Warning);
                isok = false;
                return string.Empty;
            }

            if (((string)s).Trim().Length <= 1)
            {
                Helpers.DefaultMessegeBox(" השדה " + HebparameterName + " " + " בשורה " + row + " " + "חייב להכיל יותר מאות אחת" + Environment.NewLine + "התוכנית תפסיק את פעולתה", "נתון חסר", System.Windows.Forms.MessageBoxIcon.Warning);
                isok = false;
                return string.Empty;
            }

            isok = true;
            return ((string)s).Trim();
        }

        private string GetMixedString(int row, int col, string HebparameterName, out bool isok)
        {
            var s = ExWs.Cells[row, col].value;
            string result = System.Convert.ToString(s);

            if (result.Trim().Length <= 6)
            {
                Helpers.DefaultMessegeBox(" השדה " + HebparameterName + " " + " בשורה " + row + " " + "חייב להכיל יותר משישה תווים" + Environment.NewLine + "התוכנית תפסיק את פעולתה", "נתון חסר", System.Windows.Forms.MessageBoxIcon.Warning);
                isok = false;
                return result;
            }

            isok = true;
            return result.Trim();

        }

        private string GetFieldCanBeNull(int row, int col, string HebparameterName, out bool isok)
        {
            isok = true;
            var s = ExWs.Cells[row, col].value;
            if (s == null)
                return "";


            string result = System.Convert.ToString(s);
            return result.Trim();
                
        }

        private int GetAgeCategory(int row, int col, string HebparameterName, out bool isok)
        {
            isok = false;
            var s = ExWs.Cells[row, col].value;
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
            Helpers.DefaultMessegeBox(" השדה " + HebparameterName + " " + " בשורה " + row + " " + "מכיל קטגוריית גיל לא חוקית" + Environment.NewLine + "התוכנית תפסיק את פעולתה", "נתון חסר", System.Windows.Forms.MessageBoxIcon.Warning);
            return 0;
        }

        private bool GetGender(int row, int col, string HebparameterName, out bool isok)
        {
            var s = ExWs.Cells[row, col].value;
            string result = System.Convert.ToString(s);

            if (result.Trim() != "זכר" && result.Trim() != "נקבה")
            {
                isok = false;
                Helpers.DefaultMessegeBox(" השדה " + HebparameterName + " " + " בשורה " + row + " " + "חייב להכיל את הערכים זכר או נקבה בלבד" + Environment.NewLine + "התוכנית תפסיק את פעולתה", "נתון חסר", System.Windows.Forms.MessageBoxIcon.Warning);
                return false;
            }

            isok = true;
            return (result.Trim() == "זכר");
        }

        private double GetWeight(int row, int col, string HebparameterName, out bool isok)
        {
           
            var s = ExWs.Cells[row, col].value;
            string result = System.Convert.ToString(s);

            double intResult = Helpers.extractNumberFromString(result);

            if (intResult > 0)
            {
                isok = true;
                return intResult;
            }
            else
            {
                isok = false;
                Helpers.DefaultMessegeBox(" השדה " + HebparameterName + " " + " בשורה " + row + " " + "חייב להכיל מספר כלשהו שמייצג משקל" + Environment.NewLine + "התוכנית תפסיק את פעולתה", "נתון חסר", System.Windows.Forms.MessageBoxIcon.Warning);
                return 0;
            }
        }

        private bool IsChild(int AgeGrade)
        {
            return AgeGrade < 8;
        }

        private int GetWeightCategory(int row, int col, string HebparameterName,bool IsChild, out bool isok)
        {
            isok = false;
            var s = ExWs.Cells[row, col].value;
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
            Helpers.DefaultMessegeBox(" השדה " + HebparameterName + " " + " בשורה " + row + " " + "מכיל קטגוריית משקל לא חוקית" + Environment.NewLine + "התוכנית תפסיק את פעולתה", "נתון חסר", System.Windows.Forms.MessageBoxIcon.Warning);
            return 0;
        }

        private bool GetBooleanQuestions(int row, int col, string HebparameterName, out bool isok)
        {
            isok = true;
            var s = ExWs.Cells[row, col].value;
            if (s == null)
                return false; // default is false, the program will continue even if value is null.


            string result = System.Convert.ToString(s);
            return result.Trim() == "כן";

        }

        private int GetBelt(int row, int col, string HebparameterName, out bool isok)
        {
            isok = true;
            var s = ExWs.Cells[row, col].value;
            string result = System.Convert.ToString(s);
            
            switch  (result.Trim())
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
                    Helpers.DefaultMessegeBox(" השדה " + HebparameterName + " " + " בשורה " + row + " " + "חייב להכיל דרגת חגורה חוקית" + Environment.NewLine + "התוכנית תפסיק את פעולתה", "נתון חסר", System.Windows.Forms.MessageBoxIcon.Warning);
                    return 0;       
            }


        }

    }
}