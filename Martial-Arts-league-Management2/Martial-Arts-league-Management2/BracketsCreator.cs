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
        protected MartialArts.LeagueScattering ScatteringObj;
        protected List<Contenders.ContenderLeague> ContendersLeagueList = new List<Contenders.ContenderLeague>();
        protected List<MartialArts.Bracket> BracketsList = new List<MartialArts.Bracket>();
        public BracketsCreator(List<Contenders.Contender> AllContendersList)
        {
            // create statistics of the all league
            ScatteringObj = new MartialArts.LeagueScattering(AllContendersList);
            // create contnders list with more data derived from scattering class
            foreach (Contenders.Contender c in AllContendersList)
            {
                Contenders.ContenderLeague cont = new Contenders.ContenderLeague(c,ScatteringObj);
                ContendersLeagueList.Add(cont);
            }
        
            test();
            UpgradeContenders();




            RefreshData();
        

            test();
        }


        void test()
        {

               Debug.WriteLine("*****************************************************");
            foreach (KeyValuePair<double, int> itm in ScatteringObj.RankOfFrequencies)
            {
                Debug.WriteLine("{0}---{1}", itm.Key, itm.Value);
            }
                   
             
            
            Debug.WriteLine("------------------------------------------------------------------------------------------------------------");
            Debug.WriteLine("שם"+ "," + "משפחה"
+ "," + "גיל" + "," +"משקל"
+ "," + "חגורה" + "," + "ציון" + "," + "שכיחות");

            foreach (Contenders.ContenderLeague f in ContendersLeagueList)
        {
            Debug.WriteLine(f.Contender.FirstName + "," + f.Contender.LastName 
+ "," + f.Contender.AgeCategory + ","+  f.Contender.WeightCategory.ToString()
+ "," + f.Contender.Belt + "," + f.FinalGrade + "," + f.FrequencyOfGrade);
        }
        }

        /// <summary>
        /// after each chnges in the ContendersLeagueList object must refresh data in order to change the statistcs object of all contenders
        /// </summary>
        private void RefreshData()
        {
            MartialArts.ScattteringWithContenderLeague RefrshedStatistics = new MartialArts.ScattteringWithContenderLeague(ContendersLeagueList);
            // change the statistics object in each contender objects
            foreach (ContenderLeague n in ContendersLeagueList)
            {
                n.LoadNewScatteringStatistics(RefrshedStatistics);
            }
        }
        /// <summary>
        /// 1. perfect brackets will not be changed
        /// 2. step one iterate from the lower frequancies to the higher frequencies to check if its possible to close more perfect brackets
        /// </summary>
        public void UpgradeContenders()
        {
         
            foreach (KeyValuePair<double,int> itm in ScatteringObj.RankOfFrequencies)
            {
                if (itm.Value >= MartialArts.GeneralBracket.NumberOfContenders)
                {                    
                    LargeAmmountOfContenders(itm.Value,itm.Key);
                }
                else
                {
                    SmallAmmountOfContenders(itm.Value, itm.Key);
                }
            }

          
        }

        private void SmallAmmountOfContenders(int value, double key)
        {
         
            UpgradeContenders(MartialArts.GeneralBracket.NumberOfContenders);
        }

        private void LargeAmmountOfContenders(int value, double key)
        {
   

            // exact number of contenders to only 1 bracket
            if (IsPerfectBracket(value) && value == MartialArts.GeneralBracket.NumberOfContenders)
            {

            }
            // exact number of contenders to more than 1 bracket
            else if (IsPerfectBracket(value) && value > MartialArts.GeneralBracket.NumberOfContenders)
            {

            }
            // more contenders from 1 bracket but not the exact number to close all brackets
            else
            {
                // create academy variance and close as much brackets as possible
                var t = ContendersLeagueList.Select(x => x).ToList();
                CreateAcademyVariance(ref t);
            }
        }

        /// <summary>
        /// recursive method to upgrade Incomplete brackets
        /// the rule is to give preference to the higher frequencies hence recursive is in descending order
        /// </summary>
        /// <param name="numberOfContendersInBracket"></param>
        public void UpgradeContenders(int numberOfContendersInBracket)
        {
            int n = MartialArts.GeneralBracket.NumberOfContenders;
            int y = numberOfContendersInBracket;
            if (y < 1)
                return;

            int Missing = n - y; // missing contenders if n=4 and the frequency is 2 hence 2 missing
            var l = ContendersLeagueList.AsEnumerable().Where(x => x.FrequencyOfGrade == y).Select(x => x).ToList();
            // seperate all frequencies by their grade, for example frequencies = {2,2,2} and their grades in correlation is {50,60,150} so try to find matches to each group of the 3 grpups
            var distinctGrades = l.AsEnumerable().Select(x => x.FinalGrade).Distinct().ToArray();
            // insert all the contenders with the same grade in different index in the list
            List<List<ContenderLeague>> seperated = new List<List<ContenderLeague>>();
            foreach (var g in distinctGrades)
            {
                seperated.Add(l.AsEnumerable().Where(x => x.FinalGrade == g).Select(x => x).ToList());
            }

            // iterate on each group and try to upgrade other contenders to that group
            foreach (List<ContenderLeague> group in seperated)
            {
                TryUpgradeToThatGroup(group,Missing);
            }


            UpgradeContenders(y - 1);
        }

        private void TryUpgradeToThatGroup(List<ContenderLeague> group,int MissingContenders)
        {
            double GroupGrade = group[0].FinalGrade;
            int GroupFrequency = group.Count;
            int missing = MissingContenders;
            bool finisedSearch = false;
            while (MissingContenders != 0 && finisedSearch == false)
            {
                // iterate via all contenders that are lower ranked and have a lower frequency than the examined group
                var LowerGradeAndLowerFrequencyThanGroup = ContendersLeagueList.AsEnumerable().Where(x => x.FinalGrade < GroupGrade && x.FrequencyOfGrade <= GroupFrequency).Select(x => x).ToList();
                foreach (ContenderLeague clHigh in group)
                {
                    foreach (ContenderLeague cllow in LowerGradeAndLowerFrequencyThanGroup)
                    {
                        if (AdultsMatchChecking(clHigh, cllow,true) == true)
                        {
                            var temp = ContendersLeagueList.Select(x => x.Contender).ToList();
                            ScatteringObj.RefreshFrequencies(temp); // convert contenderList to IContender list and pass to ScatteringObj.RefreshFrequencies
                            missing -= 1;
                            if (missing == 0)
                                break;
                        }
                    }
                    if (missing == 0)
                        break;
                }
                // stop the while loop
                finisedSearch = true;
            }
        }

        public bool IsPerfectBracket(int frequencyValue)
        {
            var n = MartialArts.GeneralBracket.NumberOfContenders;
            if (frequencyValue % n == 0)
                return true;
            else
                return false;
        }

        /// <summary>
        /// check id match is possible beetween higher and lower graded contenders. the priorities is weight than belt and then both of them. 
        /// </summary>
        /// <param name="frequencyValue"></param>
        /// <param name="stepUp"></param>
        /// <param name="stepDown"></param>
        public void MissingContenders(int frequencyValue, out int stepUp, out int stepDown)
        {
            var n = MartialArts.GeneralBracket.NumberOfContenders;
            stepDown = frequencyValue % n; // number of contenders to substruct in order to step down i.e from 7 to 4 its 3 [n=4]
            stepUp = n - stepDown; // number of contenders to add in order to step up i.e from 7 to 8 it 1 [n=4]
        }

        private bool AdultsMatchChecking(ContenderLeague HigherGraded,ContenderLeague LowerGraded, bool CheckAndUpgradeContender = false)
        {
            // check if weight is only 1 rank of category above
            if (HigherGraded.Contender.WeightCategory - LowerGraded.Contender.WeightCategory == 1)
            {
                 // check if lower contender is allowed to have one rank of weight above and if the grades are equal its a match
                if (LowerGraded.Contender.WeightFactor + LowerGraded.Contender.Grade == HigherGraded.FinalGrade)
                {
                    if (CheckAndUpgradeContender == true) // upgrade contender if needed
                        LowerGraded.Factor += 1;
                    // match succided
                    return true;
                }
            }

            // check if belt is only 1 rank of category above
            if (HigherGraded.Contender.Belt - LowerGraded.Contender.Belt == 1000)
            {
                // check if lower contender is allowed to have one rank of belt above and if the grades are equal its a match
                if (LowerGraded.Contender.BeltFactor + LowerGraded.Contender.Grade == HigherGraded.FinalGrade)
                {
                    if (CheckAndUpgradeContender == true) // upgrade contender if needed
                        LowerGraded.Factor += 1000;
                    // match succided
                    return true;
                }
            }

            // check if the 2 condition above can make a match
            if (HigherGraded.Contender.Belt - LowerGraded.Contender.Belt == 1000 && HigherGraded.Contender.WeightCategory - LowerGraded.Contender.WeightCategory == 1)
            {
                if (LowerGraded.Contender.BeltFactor + LowerGraded.Contender.WeightFactor + LowerGraded.Contender.Grade == HigherGraded.FinalGrade)
                {
                    if (CheckAndUpgradeContender == true) // upgrade contender if needed
                        LowerGraded.Factor += 1001;
                    // match succided
                    return true;
                }
            }

            // no match
            return false;

        }

        protected void CreateAcademyVariance(ref List<Contenders.ContenderLeague> group)
        {
           
        }

    }
}
