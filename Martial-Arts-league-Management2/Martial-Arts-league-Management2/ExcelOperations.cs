using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Office.Interop.Excel;
using System.Runtime.InteropServices;

namespace Martial_Arts_league_Management2
{
    class ExcelOperations
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
                    return ExApp;
                }
            }
            set
            {
                ExApp = value;
            }
        }

        protected Workbook XlWb = null;
        protected Worksheet ExWs = null;

        protected string Path = string.Empty;

        public ExcelOperations(string path)
        {
            this.Path = path;
            Open();
        }


        protected void Open()
        {
            try
            {
                XlWb = ExApp.Workbooks.Open(Path);
            }

            catch (Exception ex)
            {
                Close();
                throw ex;
            }
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
    }
}
