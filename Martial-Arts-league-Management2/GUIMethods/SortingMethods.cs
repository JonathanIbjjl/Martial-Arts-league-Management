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
                // sort unplaced panel
                if (Visual.VisualLeagueEvent.VisualUnplacedBracketsList.Count <= 0)
                    return;
                f.UnPlacedFpanel.Controls.Clear();
                Visual.VisualLeagueEvent.VisualUnplacedBracketsList = Visual.VisualLeagueEvent.VisualUnplacedBracketsList.AsEnumerable().OrderByDescending(x => x.Contender.Grade).ToList();
                foreach (Visual.VisualContender vc in Visual.VisualLeagueEvent.VisualUnplacedBracketsList)
                {
                    f.UnPlacedFpanel.Controls.Add(vc.Vcontender);
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
                // sort unplaced panel
                if (Visual.VisualLeagueEvent.VisualUnplacedBracketsList.Count <= 0)
                    return;
                f.UnPlacedFpanel.Controls.Clear();
                Visual.VisualLeagueEvent.VisualUnplacedBracketsList = Visual.VisualLeagueEvent.VisualUnplacedBracketsList.AsEnumerable().OrderBy(x => x.Contender.Grade).ToList();
                foreach (Visual.VisualContender vc in Visual.VisualLeagueEvent.VisualUnplacedBracketsList)
                {
                    f.UnPlacedFpanel.Controls.Add(vc.Vcontender);
                }
            }
        }

        public static void SortByWeight(Form1 f, bool Desc)
        {
            if (Desc == true)
            {
                f.BracktsFPanel.Controls.Clear();
                // sort by Bracket age
                Visual.VisualLeagueEvent.VisualBracketsList = Visual.VisualLeagueEvent.VisualBracketsList.AsEnumerable().OrderByDescending(x => x.Bracket.WeightGrade).ToList();
                foreach (Visual.VisualBracket vb in Visual.VisualLeagueEvent.VisualBracketsList)
                {
                    f.BracktsFPanel.Controls.Add(vb.Vbracket);
                }
                // sort unplaced panel
                if (Visual.VisualLeagueEvent.VisualUnplacedBracketsList.Count <= 0)
                    return;
                f.UnPlacedFpanel.Controls.Clear();
                Visual.VisualLeagueEvent.VisualUnplacedBracketsList = Visual.VisualLeagueEvent.VisualUnplacedBracketsList.AsEnumerable().OrderByDescending(x => x.Contender.Weight).ToList();
                foreach (Visual.VisualContender vc in Visual.VisualLeagueEvent.VisualUnplacedBracketsList)
                {
                    f.UnPlacedFpanel.Controls.Add(vc.Vcontender);
                }
            }
            else
            {
                f.BracktsFPanel.Controls.Clear();
                // sort by Bracket age
                Visual.VisualLeagueEvent.VisualBracketsList = Visual.VisualLeagueEvent.VisualBracketsList.AsEnumerable().OrderBy(x => x.Bracket.WeightGrade).ToList();
                foreach (Visual.VisualBracket vb in Visual.VisualLeagueEvent.VisualBracketsList)
                {
                    f.BracktsFPanel.Controls.Add(vb.Vbracket);
                }
                // sort unplaced panel
                if (Visual.VisualLeagueEvent.VisualUnplacedBracketsList.Count <= 0)
                    return;
                f.UnPlacedFpanel.Controls.Clear();
                Visual.VisualLeagueEvent.VisualUnplacedBracketsList = Visual.VisualLeagueEvent.VisualUnplacedBracketsList.AsEnumerable().OrderBy(x => x.Contender.Weight).ToList();
                foreach (Visual.VisualContender vc in Visual.VisualLeagueEvent.VisualUnplacedBracketsList)
                {
                    f.UnPlacedFpanel.Controls.Add(vc.Vcontender);
                }
            }
        }

        public static void SortByAge(Form1 f, bool Desc)
        {
            if (Desc == true)
            {
                f.BracktsFPanel.Controls.Clear();
                // sort by Bracket grade
                Visual.VisualLeagueEvent.VisualBracketsList = Visual.VisualLeagueEvent.VisualBracketsList.AsEnumerable().OrderByDescending(x => x.Bracket.AgeGrade).ToList();
                foreach (Visual.VisualBracket vb in Visual.VisualLeagueEvent.VisualBracketsList)
                {
                    f.BracktsFPanel.Controls.Add(vb.Vbracket);
                }
                // sort unplaced panel
                if (Visual.VisualLeagueEvent.VisualUnplacedBracketsList.Count <= 0)
                    return;
                f.UnPlacedFpanel.Controls.Clear();
                Visual.VisualLeagueEvent.VisualUnplacedBracketsList = Visual.VisualLeagueEvent.VisualUnplacedBracketsList.AsEnumerable().OrderByDescending(x => x.Contender.AgeCategory).ToList();
                foreach (Visual.VisualContender vc in Visual.VisualLeagueEvent.VisualUnplacedBracketsList)
                {
                    f.UnPlacedFpanel.Controls.Add(vc.Vcontender);
                }
            }
            else
            {
                f.BracktsFPanel.Controls.Clear();
                // sort by Bracket grade
                Visual.VisualLeagueEvent.VisualBracketsList = Visual.VisualLeagueEvent.VisualBracketsList.AsEnumerable().OrderBy(x => x.Bracket.AgeGrade).ToList();
                foreach (Visual.VisualBracket vb in Visual.VisualLeagueEvent.VisualBracketsList)
                {
                    f.BracktsFPanel.Controls.Add(vb.Vbracket);
                }
                // sort unplaced panel
                if (Visual.VisualLeagueEvent.VisualUnplacedBracketsList.Count <= 0)
                    return;
                f.UnPlacedFpanel.Controls.Clear();
                Visual.VisualLeagueEvent.VisualUnplacedBracketsList = Visual.VisualLeagueEvent.VisualUnplacedBracketsList.AsEnumerable().OrderBy(x => x.Contender.AgeCategory).ToList();
                foreach (Visual.VisualContender vc in Visual.VisualLeagueEvent.VisualUnplacedBracketsList)
                {
                    f.UnPlacedFpanel.Controls.Add(vc.Vcontender);
                }
            }
        }
    }
}
