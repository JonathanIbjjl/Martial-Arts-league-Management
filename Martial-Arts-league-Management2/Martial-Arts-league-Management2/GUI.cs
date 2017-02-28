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
  
        public void CreateVisualBrackets()
        {
            Visual.VisualLeagueEvent.FormObj = this;
            try
            {
                Cursor.Hide();

                // sort by Bracket Size
                Brackets.BracketsList = Brackets.BracketsList.AsEnumerable().OrderByDescending(x => x.NumberOfContenders).ToList();

                foreach (MartialArts.Bracket b in Brackets.BracketsList)
                {

                    Visual.VisualBracket br = new Visual.VisualBracket(b);
                    br.Init();
                    Visual.VisualLeagueEvent.AddVisualBracket(br);
                    // add to GUI
                    BracktsFPanel.Controls.Add(br.Vbracket);
                }

                // add uselesess and unplaced contenders

                // unplaced
                foreach (Contenders.Contender c in Brackets.ContendersList)
                {
                    Visual.VisualContender visualcont = new Visual.VisualContender(c);
                    visualcont.Init();
                    Visual.VisualLeagueEvent.AddUnplacedContender(visualcont);
                    UnPlacedFpanel.Controls.Add(visualcont.Vcontender);
                }

                // Uselesses
                foreach (Contenders.Contender c in Brackets.UselessContenders)
                {
                    Visual.VisualContender visualcont = new Visual.VisualContender(c);
                    visualcont.Init();
                    Visual.VisualLeagueEvent.AddUnplacedContender(visualcont);
                    UnPlacedFpanel.Controls.Add(visualcont.Vcontender);
                }

                // must merge all contenders in LeagueEvent instance
                Visual.VisualLeagueEvent.MergeListsForSearch();
                UpdateClocks();

                System.Threading.Thread.Sleep(1000);
                tabControl1.SelectedTab = tabPage2;
            }
            catch (Exception ex)
            {

            }
            finally
            {
                Cursor.Show();
                MoveCursor();
            }
          
        }

        public void UpdateClocks(bool FirstLoadWithoutPercent = false)
        {
            UpdateStatisticClocks(FirstLoadWithoutPercent);
            UpdateNetoClock(FirstLoadWithoutPercent);
            UpdateBracketsClock(FirstLoadWithoutPercent);
        }

        private void UpdateStatisticClocks(bool FirstLoadWithoutPercent = false)
        {
            float Percent = 0;
            if (FirstLoadWithoutPercent == false)
            {
                int ContendersWhithBracket = Visual.VisualLeagueEvent.VisualBracketsList.SelectMany(x => x.Bracket.ContendersList).Count();
                int AllConts = ContendersWhithBracket + Visual.VisualLeagueEvent.VisualUnplacedBracketsList.Select(x => x).Count();
                Percent = (float)ContendersWhithBracket / (float)AllConts;

                // statistics lables
                lblPlacedContsCount.Text = ContendersWhithBracket.ToString().PadLeft(3, '0');
                lblAllContsCount.Text = AllConts.ToString().PadLeft(3, '0');

                int allUnplacedConts = Visual.VisualLeagueEvent.VisualUnplacedBracketsList.Select(x => x).Count();
                int Useless = Visual.VisualLeagueEvent.VisualUnplacedBracketsList.Where(x => x.Contender.IsUseless == true).Count();

                lblIsUselessContsCount.Text = Useless.ToString().PadLeft(3, '0');

                if (Useless > allUnplacedConts)
                    lblAllUnplacedContsCount.Text = (Useless - allUnplacedConts).ToString().PadLeft(3, '0');
                else
                    lblAllUnplacedContsCount.Text = (allUnplacedConts - Useless).ToString().PadLeft(3, '0');
            }

            if (_MatchClock == null)
            {
                _MatchClock = new GoalsClock(70, 70, Percent);
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

           
        }

        private void UpdateNetoClock(bool FirstLoadWithoutPercent = false)
        {

            float Percent = 0;
            if (FirstLoadWithoutPercent == false)
            {
                int ContendersWhithBracket = Visual.VisualLeagueEvent.VisualBracketsList.SelectMany(x => x.Bracket.ContendersList).Count();
                int AllContsMinusUseless = (ContendersWhithBracket + Visual.VisualLeagueEvent.VisualUnplacedBracketsList.Select(x => x).Count())
                    - Visual.VisualLeagueEvent.VisualUnplacedBracketsList.Where(x => x.Contender.IsUseless == true).Count();

                Percent = (float)ContendersWhithBracket / (float)AllContsMinusUseless;
            }
         
            if (_MatchWithoutUselessClock == null)
            {
                _MatchWithoutUselessClock = new GoalsClock(70, 70, Percent);
                _MatchWithoutUselessClock.OuterCircleWeight = 10;
                _MatchWithoutUselessClock.InnerCircleWeight = 5;
                _MatchWithoutUselessClock.InnerCircleColor = GlobalVars.Sys_Yellow;
                _MatchWithoutUselessClock.OuterCircleColor = Color.FromArgb(78, 78, 78);

                _MatchWithoutUselessClock.ClockBackGroundColor = splitContainer1.Panel1.BackColor;
                _MatchWithoutUselessClock.Create(false);
                _MatchWithoutUselessClock.Clock.Anchor = AnchorStyles.Right | AnchorStyles.Top;

                _MatchWithoutUselessClock.Clock.Location = new Point(0, 0);
                lblPercentwithoutUseless.Controls.Add(_MatchWithoutUselessClock.Clock);
            }
            else
            {
                _MatchWithoutUselessClock.PercentOfGoals = Percent;
                _MatchWithoutUselessClock.RefreshClock(true);
            }
        }

        private void UpdateBracketsClock(bool FirstLoadWithoutPercent = false)
        {
            float Percent = 0;
            if (FirstLoadWithoutPercent == false)
            {
                int NumberOf_N_Brackets = Visual.VisualLeagueEvent.VisualBracketsList.Where(x => x.Bracket.NumberOfContenders >= GeneralBracket.NumberOfContenders).Count();
                int NumberOfBrackets = Visual.VisualLeagueEvent.VisualBracketsList.Select(x => x.Bracket).Count();
                Percent = (float)(NumberOf_N_Brackets) / (float)NumberOfBrackets;
            }

            if (_BracketsClock == null)
            {
                _BracketsClock = new GoalsClock(70, 70, Percent);
                _BracketsClock.OuterCircleWeight = 10;
                _BracketsClock.InnerCircleWeight = 5;
                _BracketsClock.InnerCircleColor = GlobalVars.Sys_Yellow;
                _BracketsClock.OuterCircleColor = Color.FromArgb(78, 78, 78);

                _BracketsClock.ClockBackGroundColor = splitContainer1.Panel1.BackColor;
                _BracketsClock.Create(false);
                _BracketsClock.Clock.Anchor = AnchorStyles.Right | AnchorStyles.Top;

                _BracketsClock.Clock.Location = new Point(0, 0);
                lblBracketsClock.Controls.Add(_BracketsClock.Clock);
            }
            else
            {
                _BracketsClock.PercentOfGoals = Percent;
                _BracketsClock.RefreshClock(true);
            }
        }

   


    }
}
