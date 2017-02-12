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
        protected List<Contenders.ContenderLeague> ExceptionalContenders = new List<ContenderLeague>();
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
            Init();
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
+ "," + "חגורה" + "," + "ציון" + "," + "שכיחות" + "," + "מותר משקל" + "," + "מותר חגורה" + "," + "מותר גיל" + "," + "מותר בנים");

            foreach (Contenders.ContenderLeague f in ContendersLeagueList)
        {
            Debug.WriteLine(f.Contender.FirstName + "," + f.Contender.LastName 
+ "," + f.Contender.AgeCategory + ","+  f.Contender.WeightCategory.ToString()
+ "," + f.Contender.Belt + "," + f.FinalGrade + "," + f.FrequencyOfGrade + "," + f.Contender.IsAllowedWeightGradeAbove 
+ "," + f.Contender.IsAllowedBeltGradeAbove + "," + f.Contender.IsAllowedAgeGradeAbove + "," + f.Contender.IsAllowedVersusMan);
        }
        }

        MartialArts.ScattteringWithContenderLeague RefrshedStatistics;
        /// <summary>
        /// after each chnges in the ContendersLeagueList object must refresh data in order to change the statistcs object of all contenders
        /// </summary>
        private void RefreshData()
        {
            RefrshedStatistics = new MartialArts.ScattteringWithContenderLeague(ContendersLeagueList);
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
        public void Init()
        {

            // start upgrade contenders that have frequency less then NumberOfContenders
            foreach (KeyValuePair<double,int> itm in ScatteringObj.RankOfFrequencies)
            {
                if (itm.Value < MartialArts.GeneralBracket.NumberOfContenders)
                {
                    SmallAmmountOfContenders(itm.Value, itm.Key); 
                }

            }
            RefreshData();
            
            // try to upgrade each specific individual that have as frequency of 1
            var Upgrade = ContendersLeagueList.Where(x => x.FrequencyOfGrade == 1).Select(c => c).ToList();
            foreach (ContenderLeague c in Upgrade)
            {
                TryUpgradeLowerCont(c);
              
            }

            RefreshData();
            // after iterating each individual with freq of 1 the contenders that left have no match in that league
            // thay will saved in ExceptionalContenders List and will be removed from ContendersLeagueList
            ExceptionalContenders = ContendersLeagueList.Where(x => x.FrequencyOfGrade == 1).Select(c => c).ToList(); // save exceptional contenders
            // remove exceptional contenders
            foreach (ContenderLeague x in ExceptionalContenders)
            {
                var ItemToRemove = ContendersLeagueList.SingleOrDefault(r => r.Contender.SystemID == x.Contender.SystemID);
                ContendersLeagueList.Remove(ItemToRemove);
            }

            // try to match frequencies of that are more than 1 and less then n i.e: (n< x >1) from lower to higher frequency
            for (int i = 2; i < MartialArts.GeneralBracket.NumberOfContenders; i++)
            {
                foreach (ContenderLeague c in Upgrade)
                {
                    if (c.FrequencyOfGrade == i)
                    {
                        TryUpgradeLowerCont(c);
                    }
                }
            }

            // now after the last loop we have again frequencies of 1, so that loop will take care of all the frequencies of 1 
            // (all of them will be upgraded because we separated the Exception contenders earlier
            Upgrade = ContendersLeagueList.Where(x => x.FrequencyOfGrade == 1).Select(c => c).ToList();
            foreach (ContenderLeague c in Upgrade)
            {
                TryUpgradeLowerCont(c);

            }

            //// try to make a match <b>only</b> if all the contenders (n< x >1) from that frequency and that <b>grade</b> have a match (in that case necessarily we will upgrade the frequencies)
            //for (int i = 2; i < MartialArts.GeneralBracket.NumberOfContenders; i++)
            //{
            //    var group = ContendersLeagueList.Where(x => x.FrequencyOfGrade == i).Select(z => z).ToList();
            //    // extract the grades
            //    var grades = group.Select(x => x.FinalGrade).Distinct().ToArray();
            //    foreach (double c in grades)
            //    {
            //        var ContWithTheSameGrade = group.Where(x => x.FinalGrade == c).Select(z => z).ToList();
            //        if (ContWithTheSameGrade.Count >= 1)
            //        {
            //            TryToUpgradeAllTheGroup(ContWithTheSameGrade);
            //        }
            //    }
            //}

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

            int Missing = (y <= n) ? n - y : MissingContenders(y); // missing contenders if n=4 and the frequency is 2 hence 2 missing, condition 2 handles a case when frequency is 6 (for example) so 2 are missing to 8

            if (Missing == 0)
                UpgradeContenders(y - 1); // no missing contenders proceed recurtion

            var l = ContendersLeagueList.AsEnumerable().Where(x => x.FrequencyOfGrade == y).Select(x => x).ToList();
            // seperate all frequencies by their grade, for example frequencies = {2,2,2} and their grades in correlation is {50,60,150} so try to find matches to each group of the 3 grpups
            var distinctGrades = l.AsEnumerable().Select(x => x.FinalGrade).Distinct().ToArray();
            // insert all the contenders with the same grade in different index in the list of list
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

        /// <summary>
        /// this function will upgrade the contenders only if <b> all</b> the frequency of that grade will be upgraded
        /// </summary>
        private void TryToUpgradeAllTheGroup(List<ContenderLeague> group)
        {
            var p = ContendersLeagueList.AsEnumerable().Where(x => x.FinalGrade <= group[0].FinalGrade + 1000 + 50 + 1 && x.FinalGrade > group[0].FinalGrade && x.FrequencyOfGrade == group[0].FrequencyOfGrade).Select(x => x).ToList();
            List<int> HighContendersSysID = new List<int>();

            foreach (ContenderLeague clHigh in p)
            {
                for (int i = 0; i < group.Count; i++)
                {
                    var result = MatchChecking(clHigh, group[i], false);
                    if (result > 0)
                        HighContendersSysID.Add(result);
                }
            }

            // if all of the group have match - upgrade
            if (HighContendersSysID.Count >= group.Count)
            {
               for(int i = 0; i<group.Count;i++)
                {
                    var high = ContendersLeagueList.Where(x => x.Contender.SystemID == HighContendersSysID[i]).Select(z => z).Single();
                    MatchChecking(high,group[i],true);
                }
            }
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
                        if (MatchChecking(clHigh, cllow,true) > 0)
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

        private void TryUpgradeLowerCont(ContenderLeague Cont)
        {
            
        
            // ia mirror image of TryUpgradeToThatGroup method (opposite)
            var p = ContendersLeagueList.AsEnumerable().Where(x => x.FinalGrade <= Cont.FinalGrade +1000+50+1 && x.FinalGrade > Cont.FinalGrade  && x.FrequencyOfGrade >= Cont.FrequencyOfGrade).Select(x => x).ToList();
         
                    foreach (ContenderLeague clHigh in p)
                    {
                        if (MatchChecking(clHigh, Cont, true) > 0)
                        {
                    var temp = ContendersLeagueList.Select(x => x.Contender).ToList();
                    ScatteringObj.RefreshFrequencies(temp); // convert contenderList to IContender list and pass to ScatteringObj.RefreshFrequencies
                    break;
                    }
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


        public void MissingContenders(int frequencyValue, out int stepUp, out int stepDown)
        {
            var n = MartialArts.GeneralBracket.NumberOfContenders;
            stepDown = frequencyValue % n; // number of contenders to substruct in order to step down i.e from 7 to 4 its 3 [n=4]
            stepUp = n - stepDown; // number of contenders to add in order to step up i.e from 7 to 8 it 1 [n=4]
        }
        public int MissingContenders(int frequencyValue)
        {
            var n = MartialArts.GeneralBracket.NumberOfContenders;
            var stepDown = frequencyValue % n; // number of contenders to substruct in order to step down i.e from 7 to 4 its 3 [n=4]
            var stepUp = n - stepDown; // number of contenders to add in order to step up i.e from 7 to 8 it 1 [n=4]
            return stepUp;
        }
        #region "Find Matches"
        /// <summary>
        /// this function is only to determine what function to use adult or child
        /// </summary>
        /// <param name="HigherGraded"></param>
        /// <param name="LowerGraded"></param>
        /// <param name="CheckAndUpgradeContender"></param>
        /// <returns></returns>
        private int MatchChecking(ContenderLeague HigherGraded, ContenderLeague LowerGraded, bool CheckAndUpgradeContender = false)
        {
            // to prevent double upgrading, if in the dirst time the contender was upgraded so factor must be set to 0 to prevent adding false data
            if (LowerGraded.Factor > 0)
            {
                LowerGraded.Factor = 0;
            }
      
            if (LowerGraded.Contender.IsChild)
            {
                return ChildsMatchChecking(HigherGraded, LowerGraded, CheckAndUpgradeContender);
            }
            else
            {
                return AdultsMatchChecking(HigherGraded, LowerGraded, CheckAndUpgradeContender);
            }
        }

        /// <summary>
        /// check if match is possible beetween higher and lower graded contenders. the priorities is weight than belt and then both of them. 
        /// </summary>
        /// <param name="frequencyValue"></param>
        /// <param name="stepUp"></param>
        /// <param name="stepDown"></param>
        private int AdultsMatchChecking(ContenderLeague HigherGraded,ContenderLeague LowerGraded, bool CheckAndUpgradeContender = false)
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
                    return HigherGraded.Contender.SystemID;
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
                    return HigherGraded.Contender.SystemID;
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
                    return HigherGraded.Contender.SystemID;
                }
            }

            // if LowerGraded is woman and HigherGraded is man try to upgrade the woman to mans bracket
            if (LowerGraded.Contender.IsMale == false && HigherGraded.Contender.IsMale == true)
                return WomanMatchToManChecking(HigherGraded, LowerGraded, CheckAndUpgradeContender);

                // no match
                return 0;

        }

        /// <summary>
        /// check if match is possible beetween higher and lower graded contenders. the priorities is weight,belt,age,weight+belt,weight+age,belt+age,weight+age+belt 
        /// </summary>
        /// <param name="HigherGraded"></param>
        /// <param name="LowerGraded"></param>
        /// <param name="CheckAndUpgradeContender"></param>
        /// <returns></returns>
        private int ChildsMatchChecking(ContenderLeague HigherGraded, ContenderLeague LowerGraded, bool CheckAndUpgradeContender = false)
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
                    return HigherGraded.Contender.SystemID;
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
                    return HigherGraded.Contender.SystemID;
                }
            }

            // check if age is only 1 rank of category above
            if (HigherGraded.Contender.AgeCategory - LowerGraded.Contender.AgeCategory == 50)
            {
                // check if lower contender is allowed to have one rank of age above and if the grades are equal its a match
                if (LowerGraded.Contender.AgeFactor + LowerGraded.Contender.Grade == HigherGraded.FinalGrade)
                {
                    if (CheckAndUpgradeContender == true) // upgrade contender if needed
                        LowerGraded.Factor += 50;
                    // match succided
                    return HigherGraded.Contender.SystemID;
                }
            }

            // check if the weight condition + belt condition can make a match
            if (HigherGraded.Contender.Belt - LowerGraded.Contender.Belt == 1000 && HigherGraded.Contender.WeightCategory - LowerGraded.Contender.WeightCategory == 1)
            {
                if (LowerGraded.Contender.BeltFactor + LowerGraded.Contender.WeightFactor + LowerGraded.Contender.Grade == HigherGraded.FinalGrade)
                {
                    if (CheckAndUpgradeContender == true) // upgrade contender if needed
                        LowerGraded.Factor += 1001;
                    // match succided
                    return HigherGraded.Contender.SystemID;
                }
            }

            // check if the weight condition + age condition can make a match
            if (HigherGraded.Contender.AgeCategory - LowerGraded.Contender.AgeCategory == 50 && HigherGraded.Contender.WeightCategory - LowerGraded.Contender.WeightCategory == 1)
            {
                if (LowerGraded.Contender.AgeFactor + LowerGraded.Contender.WeightFactor + LowerGraded.Contender.Grade == HigherGraded.FinalGrade)
                {
                    if (CheckAndUpgradeContender == true) // upgrade contender if needed
                        LowerGraded.Factor += 51;
                    // match succided
                    return HigherGraded.Contender.SystemID;
                }
            }

            // check if the belt condition + age condition can make a match
            if (HigherGraded.Contender.AgeCategory - LowerGraded.Contender.AgeCategory == 50 && HigherGraded.Contender.Belt - LowerGraded.Contender.Belt == 1000)
            {
                if (LowerGraded.Contender.AgeFactor + LowerGraded.Contender.BeltFactor + LowerGraded.Contender.Grade == HigherGraded.FinalGrade)
                {
                    if (CheckAndUpgradeContender == true) // upgrade contender if needed
                        LowerGraded.Factor += 1050;
                    // match succided
                    return HigherGraded.Contender.SystemID;
                }
            }


            // check if the belt condition + age + weight condition can make a match
            if (HigherGraded.Contender.AgeCategory - LowerGraded.Contender.AgeCategory == 50 && HigherGraded.Contender.Belt - LowerGraded.Contender.Belt == 1000 && HigherGraded.Contender.WeightCategory - LowerGraded.Contender.WeightCategory == 1)
            {
                if (LowerGraded.Contender.AgeFactor + LowerGraded.Contender.BeltFactor + LowerGraded.Contender.WeightFactor+ LowerGraded.Contender.Grade == HigherGraded.FinalGrade)
                {
                    if (CheckAndUpgradeContender == true) // upgrade contender if needed
                        LowerGraded.Factor += 1051;
                    // match succided
                    return HigherGraded.Contender.SystemID;
                }
            }

            // if LowerGraded is woman and HigherGraded is man try to upgrade the woman to mans bracket
            if (LowerGraded.Contender.IsMale == false && HigherGraded.Contender.IsMale == true)
                return WomanMatchToManChecking(HigherGraded, LowerGraded, CheckAndUpgradeContender);

            // no match
            return 0;

        }
        /// <summary>
        /// check if match beetwen woman and man is possible. the check is not include with other booleans (weight, belt etc...)
        /// </summary>
        /// <param name="HigherGraded">the man contender</param>
        /// <param name="LowerGraded">the woman contender</param>
        /// <param name="CheckAndUpgradeContender"></param>
        /// <returns></returns>
        private int WomanMatchToManChecking(ContenderLeague HigherGraded, ContenderLeague LowerGraded, bool CheckAndUpgradeContender = false)
        {
           // match only woman to man
            if (LowerGraded.Contender.IsMale == false && HigherGraded.Contender.IsMale == true)
            {
                if (HigherGraded.Contender.Grade == LowerGraded.Contender.Grade + 0.5)
                {
                    // check if lower contender is allowed to have one rank of weight above and if the grades are equal its a match
                    if (LowerGraded.Contender.IsAllowedVersusMan == true)
                    {
                        if (CheckAndUpgradeContender == true) // upgrade contender if needed
                            LowerGraded.Factor += 0.5;
                        // match succided
                        return HigherGraded.Contender.SystemID;
                    }
                }
                // no match
                return 0;
            }

            else
            {
                return 0;
            }


        }

        #endregion


        #region "AcademyVariance"

        protected void CreateAcademyVariance(ref List<Contenders.ContenderLeague> group)
        {
            // convert list to array
            Contenders.ContenderLeague[] ar = group.ToArray();
            // mix randomaly all the contenders to increace random variance
            SetRandomVariance(ref ar);
        
            for (int i = 1; i < ar.Length; i++)
            {
                // get the last n contender academy names
                var LastBracketContenders = GetLastBracketContenders(i,ref ar);
                // if last n contenders are from the same academy then search for swapping
                if (LastBracketContenders.Contains(ar[i].AcademyName))
                {
                    // search forward until find academy that not existing in the last n indexes
                    for (int j = i + 1; j < ar.Length; j++)
                    {
                        if (!LastBracketContenders.Contains(ar[j].AcademyName))
                        {
                           // contender with new academy name was found swap beetween the 2
                            var temp = ar[j];
                            ar[j] = ar[i];
                            ar[i] = temp;
                            break;
                        }
                    }
                }
            }

            // convert back to list
            group = ar.ToList();
        }

        /// <summary>
        /// method to create Random Variance and mix all the contenders by their academy names
        /// </summary>
        /// <param name="ar"></param>
        private void SetRandomVariance(ref ContenderLeague[] ar)
        {
            Random rand = new Random();

            for (int mix = 1; mix <= 3; mix++)
            {
                for (int i = 0; i < ar.Length; i++)
                {
                    int swap = rand.Next(0, ar.Length - 1);
                    var temp = ar[swap];
                    ar[swap] = ar[i];
                    ar[i] = temp;
                }
            }
        }

        private List<string> GetLastBracketContenders(int i, ref ContenderLeague[] ar)
        {
            List<string> r = new List<string>();
            // find the last n academy name of the contenders. for example if NumberOfContenders is 4 then
            // the method will return the last ar[i-1],ar[i-2],ar[i-3],ar[i-4] academy names
            for (int j = 1; j<=MartialArts.GeneralBracket.NumberOfContenders;j++)
            {
                if (i - j >= 0)
                {
                    r.Add(ar[i - j].AcademyName);
                }
            }
            return r;
        }

        #endregion


       
      
    }
}
