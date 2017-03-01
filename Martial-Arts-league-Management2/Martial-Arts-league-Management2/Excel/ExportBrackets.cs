﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Office.Interop.Excel;
using System.Drawing;

namespace MartialArts
{
    class ExportBrackets: ExcelOperations
    {
        Visual.VisualLeagueEvent.UndoStruct LeagueEventData;
        public ExportBrackets(string path,Visual.VisualLeagueEvent.UndoStruct LeagueEventData) : base(path)
        {
            this.LeagueEventData = LeagueEventData;
        }


        public void init()
        {
            try
            {
                ExApp = new Application();
                XlWb = ExApp.Workbooks.Add();

                BracketsSheet();
                //  UnplacedSheet();
                //  SummarySheet();

                ExApp.Visible = true;
            }

            catch (Exception ex)
            {
                Helpers.ShowGenericPromtForm("קרתה תקלה בייצוא לאקסל:" + Environment.NewLine + ex.Message.ToString());
            }
        }

        private void SummarySheet()
        {
            throw new NotImplementedException();
        }

        private void UnplacedSheet()
        {
            throw new NotImplementedException();
        }

        private void BracketsSheet()
        {
            ExWs = XlWb.Worksheets.Add();
            ExWs.Name = "בתים";
            ExWs.Select();

            int counter = 5;

            foreach (Visual.VisualBracket vb in LeagueEventData._VisualBracketsList)
            {
                // bracket description
                BracketHeader(counter,vb.Bracket.ToString());

                counter+=2;
                foreach (Visual.VisualContender c in vb.VisualCont)
                {
                    ExWs.Cells[counter, 2] = c.Contender.ID;
                    ExWs.Cells[counter, 3] = c.Contender.FirstName;
                    ExWs.Cells[counter, 4] = c.Contender.LastName;
                    ExWs.Cells[counter, 5] = c.Contender.HebrewBeltColor;
                    ExWs.Cells[counter, 6] = c.Contender.GetWeightValue;
                    ExWs.Cells[counter, 7] = " " + c.Contender.GetAgeValue + " " ;
                    ExWs.Cells[counter, 8] = c.Contender.AcademyName;
                    ExWs.Cells[counter, 9] = Contenders.ContndersGeneral.GetFactorExplanation(c,vb);

                    RowsDesign(counter);


                    counter++;
                }

                counter +=1;
            }

            // sheet header
            sheetHeader(ExWs.Range["b2:i3"], "IBJJL רשימת בתים מסכמת");
            // signature
            sheetSignature(ExWs.Range["b1:i1"]);

            ExWs.Columns["A:M"].autofit();
        }

        void BracketHeader(int counter,string header)
        {
            // bracket description
            ExWs.Cells[counter, 2] = header;
            // merge 
            ExWs.Range[ExWs.Cells[counter, 2], ExWs.Cells[counter, 9]].Merge();
            // back color
            ExWs.Range[ExWs.Cells[counter, 2], ExWs.Cells[counter, 9]].interior.color = GlobalVars.Sys_Red;
            // foreground
            ExWs.Range[ExWs.Cells[counter, 2], ExWs.Cells[counter, 9]].font.color = Color.White;
            // font
            ExWs.Range[ExWs.Cells[counter, 2], ExWs.Cells[counter, 9]].font.bold = true;
            // aligment
            ExWs.Range[ExWs.Cells[counter, 2], ExWs.Cells[counter, 9]].HorizontalAlignment = XlHAlign.xlHAlignCenter;
            ExWs.Range[ExWs.Cells[counter, 2], ExWs.Cells[counter, 9]].VerticalAlignment = XlVAlign.xlVAlignCenter;
            // borders
            ExWs.Range[ExWs.Cells[counter, 2], ExWs.Cells[counter, 9]].borders.weight = 3;
            ExWs.Range[ExWs.Cells[counter, 2], ExWs.Cells[counter, 9]].borders.color = Color.FromArgb(10, 10, 10);

            // parameter headers
            ExWs.Cells[counter + 1, 2] = "ת.ז";
            ExWs.Cells[counter + 1, 3] = "שם פרטי";
            ExWs.Cells[counter + 1, 4] = "שם משפחה";
            ExWs.Cells[counter + 1, 5] = "חגורה";
            ExWs.Cells[counter + 1, 6] = "קטגוריית משקל";
            ExWs.Cells[counter + 1, 7] = "קטגוריית גיל";
            ExWs.Cells[counter + 1, 8] = "אקדמיה";
            ExWs.Cells[counter + 1, 9] = "פקטור";

            // back color
            ExWs.Range[ExWs.Cells[counter+1, 2], ExWs.Cells[counter+1, 9]].interior.color = GlobalVars.Sys_DarkerGray;
            // foreground
            ExWs.Range[ExWs.Cells[counter+1, 2], ExWs.Cells[counter+1, 9]].font.color = Color.White;
            // font                                                
            ExWs.Range[ExWs.Cells[counter+1, 2], ExWs.Cells[counter+1, 9]].font.bold = true;
            // aligment                                            
            ExWs.Range[ExWs.Cells[counter+1, 2], ExWs.Cells[counter+1, 9]].HorizontalAlignment = XlHAlign.xlHAlignCenter;
            ExWs.Range[ExWs.Cells[counter + 1, 2], ExWs.Cells[counter + 1, 9]].VerticalAlignment = XlVAlign.xlVAlignCenter;
            // borders                                             
            ExWs.Range[ExWs.Cells[counter+1, 2], ExWs.Cells[counter+1, 9]].borders.weight = 3;

        }

        void RowsDesign(int counter)
        {
            // back color
            ExWs.Range[ExWs.Cells[counter, 2], ExWs.Cells[counter, 9]].interior.color = GlobalVars.Sys_DarkerGray;
            // foreground
            ExWs.Range[ExWs.Cells[counter, 2], ExWs.Cells[counter, 9]].font.color = GlobalVars.Sys_Yellow;
            // aligment
            ExWs.Range[ExWs.Cells[counter, 2], ExWs.Cells[counter, 9]].HorizontalAlignment = XlHAlign.xlHAlignCenter;
            ExWs.Range[ExWs.Cells[counter, 2], ExWs.Cells[counter, 9]].VerticalAlignment = XlVAlign.xlVAlignCenter;
            // borders
            ExWs.Range[ExWs.Cells[counter, 2], ExWs.Cells[counter, 9]].borders.weight = 3;
            ExWs.Range[ExWs.Cells[counter, 2], ExWs.Cells[counter, 9]].borders.color = Color.FromArgb(10, 10, 10);
        }

        void sheetHeader(Range rng, string text)
        {
            rng.Merge();
            rng.HorizontalAlignment = XlHAlign.xlHAlignCenter;
            rng.VerticalAlignment = XlVAlign.xlVAlignCenter;
            rng.Value = text;
            rng.Borders.Weight = 4;
            rng.Borders.Color = GlobalVars.Sys_Red;
            rng.Interior.Color = GlobalVars.Sys_Yellow;
            rng.Font.Color = GlobalVars.Sys_DarkerGray;
            rng.Font.Bold = true;
            rng.Font.Size = 18;

        }

        void sheetSignature(Range rng)
        {
            rng.Merge();
            rng.HorizontalAlignment = XlHAlign.xlHAlignCenter;
            rng.VerticalAlignment = XlVAlign.xlVAlignCenter;
            rng.Font.Color = Color.FromArgb(150,150,150);
            rng.Font.Italic = true;
            rng.Value = "הופק בתאריך " + DateTime.Now.ToString() + " על ידי " + "מערכת ניהול ליגה IBJJL";
            rng.Font.Size = 8;

        }
    }
}
