using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MartialArts
{
    class SortingMethods
    {
        public static void SortByAvgGrade(Form1 f,bool Desc)
        {
            if (Desc == true)
            {
                f.BracktsFPanel.Controls.Clear();
                // sort by Bracket grade
                Visual.VisualLeagueEvent.VisualBracketsList = Visual.VisualLeagueEvent.VisualBracketsList.AsEnumerable().OrderByDescending(x => x.Bracket.AverageGrade).ToList();
                foreach (Visual.VisualBracket vb in Visual.VisualLeagueEvent.VisualBracketsList)
                {
                    f.BracktsFPanel.Controls.Add(vb.Vbracket);
                }
            }
            else
            {
                f.BracktsFPanel.Controls.Clear();
                // sort by Bracket grade
                Visual.VisualLeagueEvent.VisualBracketsList = Visual.VisualLeagueEvent.VisualBracketsList.AsEnumerable().OrderBy(x => x.Bracket.AverageGrade).ToList();
                foreach (Visual.VisualBracket vb in Visual.VisualLeagueEvent.VisualBracketsList)
                {
                    f.BracktsFPanel.Controls.Add(vb.Vbracket);
                }
            }
        }
    }
}
