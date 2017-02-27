using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BusinessClocks.ExecutiveClocks;
using System.Drawing;
using System.Windows.Forms;

namespace MartialArts
{
    public partial class Form1
    {

        private BusinessClocks.ExecutiveClocks.GoalsClock _MatchClock;
        private BusinessClocks.ExecutiveClocks.GoalsClock _MatchWithoutUselessClock;
        private BusinessClocks.ExecutiveClocks.GoalsClock _BracketsClock;
        Visual.VisualLeagueEvent LeagueEvent = new Visual.VisualLeagueEvent();
        public void CreateVisualBrackets()
        {
            // if ClearExistingBrackets() was activated
            if (LeagueEvent == null)
                LeagueEvent = new Visual.VisualLeagueEvent();

            tabControl1.SelectedTab = tabPage2;
            // sort by Bracket Size
            Brackets.BracketsList = Brackets.BracketsList.AsEnumerable().OrderByDescending(x => x.NumberOfContenders).ToList();

            foreach (MartialArts.Bracket b in Brackets.BracketsList)
            {

                Visual.VisualBracket br = new Visual.VisualBracket(b);
                br.Init();
                LeagueEvent.AddVisualBracket(br);
                // add to GUI
                BracktsFPanel.Controls.Add(br.Vbracket);
            }

            // add uselesess and unplaced contenders

            // unplaced
            foreach (Contenders.Contender c in Brackets.ContendersList)
            {
                Visual.VisualContender visualcont = new Visual.VisualContender(c);
                visualcont.Init();
                LeagueEvent.AddUnplacedContender(c);
                UnPlacedFpanel.Controls.Add(visualcont.Vcontender);
            }

            // Uselesses
            foreach (Contenders.Contender c in Brackets.UselessContenders)
            {
                Visual.VisualContender visualcont = new Visual.VisualContender(c);
                visualcont.Init();
                LeagueEvent.AddUnplacedContender(c);
                UnPlacedFpanel.Controls.Add(visualcont.Vcontender);
            }

            UpdateClocks();
        }

        private void UpdateClocks()
        {
            UpdateStatisticClocks();
            UpdateNetoClock();
            UpdateBracketsClock();
        }

        private void UpdateStatisticClocks()
        {
            int ContendersWhithBracket = LeagueEvent.VisualBracketsList.SelectMany(x => x.Bracket.ContendersList).Count();
            int AllConts = ContendersWhithBracket + LeagueEvent.VisualUnplacedBracketsList.Select(x => x).Count();
            
            float Percent = (float)ContendersWhithBracket / (float)AllConts;

            if (_MatchClock == null)
            {
                _MatchClock = new GoalsClock(90, 90, Percent);
                _MatchClock.OuterCircleWeight = 10;
                _MatchClock.InnerCircleWeight = 5;
                _MatchClock.InnerCircleColor = GlobalVars.Sys_Yellow;
                _MatchClock.OuterCircleColor = Color.FromArgb(78, 78, 78);

                _MatchClock.ClockBackGroundColor = splitContainer1.Panel1.BackColor;
                _MatchClock.Create(false);
                _MatchClock.Clock.Anchor = AnchorStyles.Right | AnchorStyles.Top;

                _MatchClock.Clock.Location = new Point(0, 0);
                lblPercent.Controls.Add(_MatchClock.Clock);
            }
            else
            {
                _MatchClock.PercentOfGoals = Percent;
                _MatchClock.RefreshClock(true);
            }

            // statistics lables
            lblPlacedContsCount.Text = ContendersWhithBracket.ToString();
            lblAllContsCount.Text = AllConts.ToString();

            int allUnplacedConts = LeagueEvent.VisualUnplacedBracketsList.Select(x => x).Count();
            int Useless = LeagueEvent.VisualUnplacedBracketsList.Where(x => x.IsUseless == true).Count();

            lblIsUselessContsCount.Text = Useless.ToString();

            if (Useless > allUnplacedConts)
                lblAllUnplacedContsCount.Text = (Useless - allUnplacedConts).ToString();
            else
                lblAllUnplacedContsCount.Text = (allUnplacedConts - Useless).ToString();

        }

        private void UpdateNetoClock()
        {


            int ContendersWhithBracket = LeagueEvent.VisualBracketsList.SelectMany(x => x.Bracket.ContendersList).Count();
            int AllContsMinusUseless = (ContendersWhithBracket + LeagueEvent.VisualUnplacedBracketsList.Select(x => x).Count())
                - LeagueEvent.VisualUnplacedBracketsList.Where(x => x.IsUseless == true).Count();

            float Percent = (float)ContendersWhithBracket / (float)AllContsMinusUseless;
                      // float Percent = (float)(Brackets.BracketsList.SelectMany(x => x.ContendersList).Count()) / ((float)GlobalVars.ListOfContenders.Count - Brackets.UselessContenders.Count);            if (_MatchWithoutUselessClock == null)            {                _MatchWithoutUselessClock = new GoalsClock(90, 90, Percent);                _MatchWithoutUselessClock.OuterCircleWeight = 10;                _MatchWithoutUselessClock.InnerCircleWeight = 5;                _MatchWithoutUselessClock.InnerCircleColor = GlobalVars.Sys_Yellow;                _MatchWithoutUselessClock.OuterCircleColor = Color.FromArgb(78, 78, 78);                _MatchWithoutUselessClock.ClockBackGroundColor = splitContainer1.Panel1.BackColor;                _MatchWithoutUselessClock.Create(false);                _MatchWithoutUselessClock.Clock.Anchor = AnchorStyles.Right | AnchorStyles.Top;                _MatchWithoutUselessClock.Clock.Location = new Point(0, 0);
                lblPercentwithoutUseless.Controls.Add(_MatchWithoutUselessClock.Clock);            }            else            {                _MatchWithoutUselessClock.PercentOfGoals = Percent;                _MatchWithoutUselessClock.RefreshClock(true);            }
        }

        private void UpdateBracketsClock()
        {
            int NumberOf_N_Brackets = LeagueEvent.VisualBracketsList.Where(x => x.Bracket.NumberOfContenders == GeneralBracket.NumberOfContenders).Count();
            int NumberOfBrackets = LeagueEvent.VisualBracketsList.Select(x=> x.Bracket).Count();
            float Percent = (float)(NumberOf_N_Brackets) / (float)NumberOfBrackets;                         if (_BracketsClock == null)            {                _BracketsClock = new GoalsClock(90, 90, Percent);                _BracketsClock.OuterCircleWeight = 10;                _BracketsClock.InnerCircleWeight = 5;                _BracketsClock.InnerCircleColor = GlobalVars.Sys_Yellow;                _BracketsClock.OuterCircleColor = Color.FromArgb(78, 78, 78);                _BracketsClock.ClockBackGroundColor = splitContainer1.Panel1.BackColor;                _BracketsClock.Create(false);                _BracketsClock.Clock.Anchor = AnchorStyles.Right | AnchorStyles.Top;                _BracketsClock.Clock.Location = new Point(0, 0);
                lblBracketsClock.Controls.Add(_BracketsClock.Clock);            }            else            {                _BracketsClock.PercentOfGoals = Percent;                _BracketsClock.RefreshClock(true);            }
        }


    }
}
