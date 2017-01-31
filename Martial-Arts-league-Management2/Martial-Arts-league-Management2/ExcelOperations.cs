using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Office.Interop.Excel;
using System.Runtime.InteropServices;
using System.Drawing;

namespace Martial_Arts_league_Management2
{
    class ExcelOperations : IDisposable
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
        protected string Path = string.Empty;
        protected Dictionary<string, int> ColumnsDictionary;
        public Contenders.Contender ContenderObj; 
        public ExcelOperations(string path)
        {
            this.Path = path;          
        }

        public bool GetContenders()
        {
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
                    ExWs = XlWb.Worksheets[WorkSheetName];
                }
                else
                {
                    // the first worksheet is the only one
                    ExWs = XlWb.Worksheets[0];
                }

                //continue analizing excel, check data is legal
                if (IsExcelIsLegal() == false)
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
            ContenderObj = new Contenders.Contender();
            // check column names
            ColumnsDictionary = new Dictionary<string, int>();
            return false; // TODO: delete this
        }

        protected bool Open()
        {
            try
            {
                XlWb = ExApp.Workbooks.Open(Path);
                return true;
            }

            catch (Exception ex)
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
    }

}
