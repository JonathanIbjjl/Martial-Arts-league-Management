using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Diagnostics;
namespace Contenders
{
    class BracketsCreator
    {
        protected List<Contenders.ContenderLeague> ContendersLeagueList = new List<ContenderLeague>();
        protected Martial_Arts_league_Management2.LeagueScattering ScatteringObj;

        // UnMatched Contenders
        protected List<ContenderLeague> UnMatched;
        public BracketsCreator(List<Contenders.Contender> AllContendersList)
        {
            // create statistics of the all league
            ScatteringObj = new Martial_Arts_league_Management2.LeagueScattering(AllContendersList);
            // create contnders list with more data derived from scattering class
            foreach (Contenders.Contender contender in AllContendersList)
            {
                Contenders.ContenderLeague cont = new Contenders.ContenderLeague(contender, ScatteringObj);
                ContendersLeagueList.Add(cont);
            }

            UpgradeContenders();
        }
        /// <summary>
        /// 1. perfect brackets will not be changed
        /// 2. step one iterate from the lower frequancies to the higher frequencies to check if its possible to close more perfect brackets
        /// </summary>
        public void UpgradeContenders()
        {

            int StepUp, StepDown;
            foreach (KeyValuePair<double,int> itm in ScatteringObj.RankOfFrequencies)
            {

                
                if (!IsPerfectBracket(itm.Value))
                {
                    MissingContenders(itm.Value, out StepUp,out StepDown);
                    // the goal here is only to try to close perfect brackets or to close small brackets
                    tryToClosePerfectBracket(itm.Key,StepDown);
                }
            }
        }

        private void tryToClosePerfectBracket(double grade, int stepDown)
        {
            var TryUpgradeConts = ContendersLeagueList.AsEnumerable().Where(x => x.Contender.Grade == grade).Select(x => x).ToList();
            var OneRankAboveConts = ContendersLeagueList.AsEnumerable().Where(x => x.Contender.Grade == grade + 1).Select(x => x).ToList();
            // check if all contenders can be upgraded
            bool IsTheSameValue = TryUpgradeConts.Any(c => c.Contender.Factor != 1);


        }



        public bool IsPerfectBracket(int frequencyValue)
        {
            var n = Martial_Arts_league_Management2.GeneralBracket.NumberOfContenders;
            if (frequencyValue % n == 0)
                return true;
            else
                return false;
        }

        public void MissingContenders(int frequencyValue, out int stepUp, out int stepDown)
        {
            var n = Martial_Arts_league_Management2.GeneralBracket.NumberOfContenders;
            stepDown = frequencyValue % n; // number of contenders to substruct in order to step down i.e from 7 to 4 its 3 [n=4]
            stepUp = n - stepDown; // number of contenders to add in order to step up i.e from 7 to 8 it 1 [n=4]
        }

    }
}
