using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Office.Interop.Excel;
namespace MartialArts
{
    class ExportDgvToExcel : ExcelOperations
    {
        public ExportDgvToExcel(string path) : base(path)
        {
            this.Path = path;
        }

        public bool Export(System.Windows.Forms.DataGridView dgv)
        {
            try
            {
                ExApp = new Application();
                XlWb = ExApp.Workbooks.Add();
                ExWs = XlWb.Worksheets.Add();
                ExWs.Name = "רשימת מתחרים";

                for (int Ecolumns = 0; Ecolumns <= dgv.Columns.Count - 1; Ecolumns++)
                {
                    ExWs.Cells[1, Ecolumns + 1] = dgv.Columns[Ecolumns].HeaderText;
                    ExWs.Cells[1, Ecolumns + 1].Interior.Color = GlobalVars.Sys_LighterGray;
                    ExWs.Cells[1, Ecolumns + 1].Font.Color = GlobalVars.Sys_Yellow;
                    ExWs.Cells[1, Ecolumns + 1].Font.Bold = true;

                    for (int Erows = 0; Erows <= dgv.RowCount - 1; Erows++)
                    {
                        ExWs.Cells[Erows + 2, Ecolumns + 1] = dgv.Rows[Erows].Cells[Ecolumns].Value;
                        ExWs.Range[ExWs.Cells[Erows + 2, Ecolumns + 1], ExWs.Cells[Erows + 2, dgv.Columns.Count - 1]].interior.color = dgv.Rows[Erows].DefaultCellStyle.BackColor;
                        ExWs.Range[ExWs.Cells[Erows + 2, Ecolumns + 1], ExWs.Cells[Erows + 2, dgv.Columns.Count - 1]].font.color = dgv.Rows[Erows].DefaultCellStyle.ForeColor;
                    }

                }

                ExWs.Columns["A:Z"].Autofit();
                base.SaveAs(ExcelFileSubjects.ContendersList);
                ExApp.Visible = true;
                return true;
            }


            
            catch (Exception ex)
            {
                return false;
            }
        }
    }
}
