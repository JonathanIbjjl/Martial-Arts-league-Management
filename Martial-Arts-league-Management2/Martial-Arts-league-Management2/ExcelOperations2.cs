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
        List<Contenders.Contender> ContendersList;
        private bool SetContender()
        {
           
            ContendersList = new List<Contenders.Contender>();
            // for out parameter
            bool isok = true; 

            // itirate and add contenders to list
            for (int i = 2; i <= LastRow; i++)
            {
                Contenders.Contender con = new Contenders.Contender();

                con.FirstName = GetPureStringField(i,ContenderObj.HeadersDictionary["FirstName"],"שם פרטי",out isok);
                con.LastName = GetPureStringField(i, ContenderObj.HeadersDictionary["LastName"], "שם משפחה", out isok);
                con.ID = GetMixedString(i, ContenderObj.HeadersDictionary["ID"], "תעודת זהות", out isok);
                con.Email = GetPureStringField(i, ContenderObj.HeadersDictionary["Email"], "אימייל", out isok);
                con.PhoneNumber = GetMixedString(i, ContenderObj.HeadersDictionary["PhoneNumber"], "מספר טלפון", out isok);
                con.DateOfBirth = GetFieldCanBeNull(i, ContenderObj.HeadersDictionary["DateOfBirth"], "תאריך לידה", out isok);
                con.AgeCategory = GetAgeCategory(i, ContenderObj.HeadersDictionary["AgeCategory"], "קטגורית גיל", out isok);
                ContendersList.Add(con);

                // final check to see if there is no defect in that contender
                if (isok == false)
                    return false;
            }


            foreach (Contenders.Contender f in ContendersList)
                Debug.WriteLine(f.FirstName + " " + f.LastName  + " " + f.ID
                    + " " + f.Email + " " + f.PhoneNumber  + " " + f.DateOfBirth
                    + " " + f.AgeCategory);

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

            Helpers.DefaultMessegeBox(" השדה " + HebparameterName + " " + " בשורה " + row + " " + "מכיל קטגוריית גיל לא חוקית" + Environment.NewLine + "התוכנית תפסיק את פעולתה", "נתון חסר", System.Windows.Forms.MessageBoxIcon.Warning);
            return 0;
        }

    }
}