using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Office.Interop.Excel;
using System.Runtime.InteropServices;
using System.Drawing;
using System.Diagnostics;
namespace MartialArts
{ 
  partial class ExcelOperations : IDisposable
    {
        protected Application _ExApp = null;
        protected Application ExApp
        {
            get
            {
                if (_ExApp == null)
                {
                    _ExApp = new Application();
                    return _ExApp;
                }

                else
                {
                    return _ExApp;
                }
            }
            set
            {
                _ExApp = value;
            }
        }

        protected Workbook XlWb = null;
        protected Worksheet ExWs = null;
        protected string WorkSheetName = string.Empty; 
        protected int LastRow = 1;
        protected string Path = string.Empty;
        public Contenders.ContndersGeneral ContenderObj; 
        public ExcelOperations(string path)
        {
            this.Path = path;          
        }

        public bool GetContenders()
        {

            // check if excel is open
            if (Helpers.fileIsOpen(Path) == true)
            {
                ShowPromtForm("קובץ האקסל פתוח, אנא סגור אותו וטען את הקובץ שוב" + Environment.NewLine + "התוכנית תפסיק את הפעולתה");
                return false;
            }

            if (Open())
            {
                // check if there is multiple sheets
                if (XlWb.Worksheets.Count > 1)
                {
                    // show the user menu to choose sheet
                    DynamicForms.ChooseExSheetForm choose = new DynamicForms.ChooseExSheetForm(GetExcelWorkSheets());
                    choose.showForm();
                    // choosen worksheet
                    WorkSheetName = choose.ChoosenWsName;
                    ExWs = (Worksheet)XlWb.Sheets[WorkSheetName];
                }
                else
                {
                    // the first worksheet is the only one
                    ExWs = (Worksheet)XlWb.Sheets[1];
                }

                //continue analizing excel, check data is legal
                if (IsExcelIsLegal() == false)
                    return false;
                // set contenders properties
                if (SetContender() == false)
                    return false;

                return true;
            }

            else
            {
                // excel load fail  
                Close();
                return false;
            }
        }


        private bool IsExcelIsLegal()
        {
            // init contender object
            ContenderObj = new Contenders.ContndersGeneral();
            // check column names
            if (CheckExcelColsExist() == false)
            {
                Close();
                return false;
            }

            // check that are not null rows
            if (CheckNullRows() == false)
            {
                Close();
                return false;
            }

            return true;
        }

        private bool CheckNullRows()
        {
            // 1 is or un equal used rows or 1 row
            if (SetLastRow(ContenderObj.HeadersDictionary["FirstName"], ContenderObj.HeadersDictionary["LastName"]) == 1)
            {
                return false;
            }

            // check if all columns have values
            for (int i = 1; i <= LastRow; i++)
            {
                for (int j = 0; j <= ContenderObj.NotAllowedNullColumns.Length - 1; j++)
                {
                    var s = ExWs.Cells[i, ContenderObj.HeadersDictionary[ContenderObj.NotAllowedNullColumns[j]]].Value;
 

                    if ( s == null )
                    {
                        ShowPromtForm("חסרים נתונים בשורה" + " " + i + " " + Environment.NewLine + "עמודת" + " " + FindQuestionByPropertyName(ContenderObj.NotAllowedNullColumns[j])
                             + Environment.NewLine + "יש לתקן את הקובץ, התוכנית תפסיק את פעולתה");
                        return false;
                    }
                }
            }
            return true;
        }

        private int SetLastRow(int firstNameCol,int LastNameCol)
        {

            int lr1 = ExWs.Cells[ExWs.Rows.Count, firstNameCol].End(XlDirection.xlUp).Row;
            int lr2 = ExWs.Cells[ExWs.Rows.Count, LastNameCol].End(XlDirection.xlUp).Row;

            if (lr1 != lr2)
            {
                ShowPromtForm("לא יתכן שעמודות שם משפחה ושם פרטי אינן שוות במספר הנתונים שלהם. התוכנית תפסיק את פעולתה");              
                return 1;
            }
            // set global field of last row
            this.LastRow = lr1;
            return lr1;
        }

        protected bool Open()
        {
            try
            {
                XlWb = ExApp.Workbooks.Open(Path);
                return true;
            }

            catch
            {
                Close();
                return false;
            }
        }

        protected string[] GetExcelWorkSheets()
        {
            string[] result = new string[XlWb.Worksheets.Count];
            byte counter = 0;

            foreach (Worksheet ws in XlWb.Worksheets)
            {
                result[counter] = ws.Name;
                counter++;
            }

            return result;
        }

        protected bool CheckExcelColsExist()
        {
            // iterate via array column names
            for (int i = 0; i < Helpers.ColsRecognition.GetLength(0); i++)
            {
                // default value of dictionary is '0' if it stays zero at the end of the function the column is missing
                ContenderObj.HeadersDictionary.Add(Helpers.ColsRecognition[i, 0], 0);
                // iterate trough the first 100 columns of the worksheet
                for (int j = 1; j < 100; j++)
                {

                    string s = ExWs.Cells[1, j].Value;
                    // check if column contains target string
                    if (s != null && s.Contains(Helpers.ColsRecognition[i, 1]))
                    {
                        // swap 0 value with the column number
                        ContenderObj.HeadersDictionary[Helpers.ColsRecognition[i, 0]] = j;
                        break;
                    }
                }
            }

            // check if dictionary is legal (no zeros)
            foreach (KeyValuePair<string, int> itm in ContenderObj.HeadersDictionary)
            {
                if (itm.Value == 0)
                {
                    // find the missing column
                    string missing = FindQuestionByPropertyName(itm.Key);

                    // the dictionary is not valid promt the user
                    ShowPromtForm(":העמודה הבאה חסרה בקובץ " + " " +
                        Environment.NewLine + missing + Environment.NewLine +
                        "התוכנית תפסיק את פעולתה");
                    return false;
                }
            }

            return true; // columns are valid
        }

        private string FindQuestionByPropertyName(string ContenderPropertyName)
        {
            string missing = "";
            for (int i = 0; i < Helpers.ColsRecognition.GetLength(0); i++)
            {
                if (ContenderPropertyName == Helpers.ColsRecognition[i, 0])
                {
                    missing = Helpers.ColsRecognition[i, 2];
                    break;
                }
            }

            return missing;
        }

        protected void Close()
        {
            try
            {
                if (XlWb != null)
                {
                    XlWb.Close();
                    Marshal.ReleaseComObject(XlWb);
                    XlWb = null;

                    if (ExWs != null)
                    {
                        Marshal.ReleaseComObject(ExWs);
                        ExWs = null;
                    }

                    
                }
                
                if (_ExApp != null)
                {
                    ExApp.Quit();
                    Marshal.ReleaseComObject(_ExApp);
                    ExApp = null;
                }
               
            }

            catch (Exception ex)
            {
                throw ex;
            }
        }

        public void Dispose()
        {
            Close();
        }

        private void ShowPromtForm(string txt)
        {
            using (Martial_Arts_league_Management2.PromtForm promt = new Martial_Arts_league_Management2.PromtForm(txt, false, "התוכנית תפסיק את פעולתה", true))
            {
                promt.ShowDialog();
            }
        }
    }

}
