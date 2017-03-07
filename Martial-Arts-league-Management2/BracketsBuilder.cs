using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Diagnostics;
using Contenders;

namespace MartialArts
{
    class BracketsBuilder : PotentialBracketsGeneralStatistics
    {
        private bool OnlyWoman { get; set; }
        public List<Contenders.Contender> ContendersList = new List<Contenders.Contender>();
        protected List<ScoreAndID> PotentialScores = new List<ScoreAndID>();
        public List<Contenders.Contender> UselessContenders = new List<Contenders.Contender>();
        public List<MartialArts.Bracket> BracketsList = new List<MartialArts.Bracket>();

        protected double AllPotentialBracketsAverage { get; set; }
        protected double AllPotentialBracketsStdDivision;

        public BracketsBuilder(List<Contenders.Contender> cont, bool OnlyWoman)
        {
            this.OnlyWoman = OnlyWoman;
            ContendersList = cont.ToList();
        }

        public BracketsBuilder()
        {
            // only for saved data!
        }

        private void CreateScoreAndID()
        {
            if (ContendersList.Count <= 1)
                return;

            // create the potential scores of all contenders
            List<ScoreAndID> l = new List<ScoreAndID>();

            if (OnlyWoman == false)
            {

                foreach (Contenders.Contender c in ContendersList)
                {
                    if (c.IsMale == true)
                    {
                        foreach (double PotentialScore in c)
                        {
                            ScoreAndID si = new ScoreAndID();
                            si.Score = PotentialScore;
                            si.SystemID = c.SystemID;
                            si.IsMale = c.IsMale;
                            l.Add(si);
                        }
                    }

                    else
                    {
                        // woman factor is canceled vs man
                        ScoreAndID si = new ScoreAndID();
                        si.Score = c.Grade;
                        si.SystemID = c.SystemID;
                        si.IsMale = c.IsMale;
                        l.Add(si);
                    }


                }

            }

            else
            {
                // only woman, factors will be initilized because its only woman
                foreach (Contenders.Contender c in ContendersList)
                {
                    foreach (double PotentialScore in c)
                    {
                        ScoreAndID si = new ScoreAndID();
                        si.Score = PotentialScore;
                        si.SystemID = c.SystemID;
                        si.IsMale = c.IsMale;
                        l.Add(si);
                    }
                }
            }

            // sort by score and save to global list
            PotentialScores = l.AsEnumerable().OrderBy(x => x.Score).ToList();
        }


        private void test()
        {

          BracketsList =   BracketsList.OrderBy(x => x.NumberOfContenders).ToList();
            foreach (MartialArts.Bracket b in BracketsList)
            {
                Debug.WriteLine(b.ToString());
                Debug.WriteLine("{0}, {1}, {2}, {3}, {4}, {5}, {6}, {7}, {8}, {9}, {10}", "שם", "שם משפחה", "אקדמיה", "חגורה", "משקל", "גיל", "פקטור משקל", "פקטור חגורה", "פקטור גיל", "פקטור בנים", "SystemID");

                foreach (Contender c in b.ContendersList)
                {


                   string belt = Helpers.GetHebBeltName((Contenders.ContndersGeneral.BeltsEnum)c.Belt);
                    string age = Contenders.ContndersGeneral.AgeGrades.Where(x => x.Value == c.AgeCategory).Select(z => z.Key).SingleOrDefault();
                    string weight = Contenders.ContndersGeneral.ChildWeightCat.Where(x => x.Value == c.WeightCategory).Select(z => z.Key).SingleOrDefault();

                    Debug.WriteLine("{0}, {1}, {2}, {3}, {4}, {5}, {6}, {7}, {8}, {9}, {10}", c.FirstName, c.LastName, c.AcademyName, belt, weight, age, c.IsAllowedWeightGradeAbove, c.IsAllowedBeltGradeAbove, c.IsAllowedAgeGradeAbove, c.IsAllowedVersusMan, c.SystemID);

                }

                Debug.WriteLine("");
                Debug.WriteLine("");
            }

            Debug.WriteLine("");
            Debug.WriteLine("");
            Debug.WriteLine("מתחרים שהמערכת בחרה לוותר עליהם");
            Debug.WriteLine("{0}, {1}, {2}, {3}, {4}, {5}, {6}, {7}, {8}, {9}, {10}", "שם", "שם משפחה", "אקדמיה", "חגורה", "משקל", "גיל", "פקטור משקל", "פקטור חגורה", "פקטור גיל", "פקטור בנים", "SystemID");
            foreach (Contender c in ContendersList)
            {


                string belt = Helpers.GetHebBeltName((Contenders.ContndersGeneral.BeltsEnum)c.Belt);
                string age = Contenders.ContndersGeneral.AgeGrades.Where(x => x.Value == c.AgeCategory).Select(z => z.Key).SingleOrDefault();
                string weight = Contenders.ContndersGeneral.ChildWeightCat.Where(x => x.Value == c.WeightCategory).Select(z => z.Key).SingleOrDefault();

                Debug.WriteLine("{0}, {1}, {2}, {3}, {4}, {5}, {6}, {7}, {8}, {9}, {10}", c.FirstName, c.LastName, c.AcademyName, belt, weight, age, c.IsAllowedWeightGradeAbove, c.IsAllowedBeltGradeAbove, c.IsAllowedAgeGradeAbove, c.IsAllowedVersusMan, c.SystemID);

                Debug.WriteLine("");
                foreach (Contender.PotentialBrackets p in c.PbListArchive)
                {
                    foreach (KeyValuePair<int, double> k in p.IdAndScore)
                    {
                        Debug.WriteLine("{0}, {1}", k.Key, k.Value);
                    }
                    Debug.WriteLine("");
                }
                Debug.WriteLine("");
                Debug.WriteLine("");

            }

            Debug.WriteLine("");
            Debug.WriteLine("");
            Debug.WriteLine("מתחרים ללא שום התאמה");
            Debug.WriteLine("{0}, {1}, {2}, {3}, {4}, {5}, {6}, {7}, {8}, {9}, {10}", "שם", "שם משפחה", "אקדמיה", "חגורה", "משקל", "גיל", "פקטור משקל", "פקטור חגורה", "פקטור גיל","פקטור בנים","SystemID");

            foreach (Contender c in UselessContenders)
            {


                string belt = Helpers.GetHebBeltName((Contenders.ContndersGeneral.BeltsEnum)c.Belt);
                string age = Contenders.ContndersGeneral.AgeGrades.Where(x => x.Value == c.AgeCategory).Select(z => z.Key).SingleOrDefault();
                string weight = Contenders.ContndersGeneral.ChildWeightCat.Where(x => x.Value == c.WeightCategory).Select(z => z.Key).SingleOrDefault();

                Debug.WriteLine("{0}, {1}, {2}, {3}, {4}, {5}, {6}, {7}, {8}, {9}, {10}", c.FirstName, c.LastName, c.AcademyName, belt, weight, age, c.IsAllowedWeightGradeAbove, c.IsAllowedBeltGradeAbove, c.IsAllowedAgeGradeAbove, c.IsAllowedVersusMan,c.SystemID);

               
            }


        }




        public void Init()
        {
            ArrangePotentialBracketsAgain(true); // data to add to archive

            // handle only woman explictly, woman bracket even 2 contenders is better than mixed houses, if woman are left after that method they have no factor agains man
            if (OnlyWoman == false) // when only woman is true ther is no need for that method
            {
                HandleWoman();
                // sort statistics again 
                ArrangePotentialBracketsAgain();
            }
            else
            {
                // load the potential brackets and statistics of each contender
                ArrangePotentialBracketsAgain();
            }

            // remove useless contenders only on mans instance
            if(OnlyWoman ==false)
            RemoveUselesses();

            // sort statistics again 
            ArrangePotentialBracketsAgain();
            // reduce perfect brackets only
            ReducePerfectBrackets();
            // handle Contenders That are Less then N + the match the most ideal for all contnders by max general score
            HandleLessThanNumOfConts(MartialArts.GeneralBracket.NumberOfContenders - 1);

            // handle Contenders That are Less then N + the match is the most ideal for all contnders by max frequency
            HandleBrackets();
            test();
            HandleRemeining();
     
        }

        private void HandleRemeining()
        {
            for (int i = 0; i < ContendersList.Count; i++)
            {
                foreach (Bracket b in BracketsList)
                {
                    if (Math.Floor(ContendersList[i].Grade) == Math.Floor(b.AverageGrade) && ReturnBinaryGender(b) == ContendersList[i].IsMale)
                    {
                        AddReminingToBracket( ContendersList[i].SystemID, ContendersList[i].Grade, b.BracketNumber);
                        break;
                    }
                    else if (Math.Floor(ContendersList[i].Score_WeightFactor) == Math.Floor(b.AverageGrade) && ReturnBinaryGender(b) == ContendersList[i].IsMale)
                    {
                        AddReminingToBracket(ContendersList[i].SystemID, ContendersList[i].Score_WeightFactor, b.BracketNumber);
                        break;
                    }

                    else if (Math.Floor(ContendersList[i].Score_BeltFactor) == Math.Floor(b.AverageGrade) && ReturnBinaryGender(b) == ContendersList[i].IsMale)
                    {
                        AddReminingToBracket(ContendersList[i].SystemID, ContendersList[i].Score_BeltFactor, b.BracketNumber);
                        break;
                    }
                    else if (Math.Floor(ContendersList[i].Score_AgeFactor) == Math.Floor(b.AverageGrade) && ContendersList[i].IsChild == true && ReturnBinaryGender(b) == ContendersList[i].IsMale)
                    {
                        AddReminingToBracket(ContendersList[i].SystemID, ContendersList[i].Score_AgeFactor, b.BracketNumber);
                        break;
                    }
                    else if (Math.Floor(ContendersList[i].Score_Weight_Belt_Factor) == Math.Floor(b.AverageGrade) && ReturnBinaryGender(b) == ContendersList[i].IsMale)
                    {
                        AddReminingToBracket(ContendersList[i].SystemID, ContendersList[i].Score_Weight_Belt_Factor, b.BracketNumber);
                        break;
                    }
                    else if (Math.Floor(ContendersList[i].Score_Weight_Age_Factor) == Math.Floor(b.AverageGrade) && ContendersList[i].IsChild == true && ReturnBinaryGender(b) == ContendersList[i].IsMale)
                    {
                        AddReminingToBracket(ContendersList[i].SystemID, ContendersList[i].Score_Weight_Age_Factor, b.BracketNumber);
                        break;
                    }
                    else if (Math.Floor(ContendersList[i].Score_AllFactors) == Math.Floor(b.AverageGrade) && ContendersList[i].IsChild == true && ReturnBinaryGender(b) == ContendersList[i].IsMale)
                    {
                        AddReminingToBracket(ContendersList[i].SystemID, ContendersList[i].Score_AllFactors, b.BracketNumber);
                        break;
                    }

                }
            }
        }



        public static bool ReturnBinaryGender(Bracket b)
        {
            if (b.Gender == GlobalVars.GenderEnum.Man)
                return true;
            else 
                return false;
        }

        private void AddReminingToBracket(int systemID,double finalGradeForBracket,int BracketID)
        {
            var Cont = ContendersList.Where(x => x.SystemID == systemID).Select(x => x).First();
            var Bracket = BracketsList.Where(x => x.BracketNumber == BracketID).Select(z => z).Single();

            Bracket.ContendersList.Add(Cont);
            Cont.FinalGradeInBracket = finalGradeForBracket;
            Cont.IsPlaced = true;
            RemoveItemFromList(Cont.SystemID, false);
        }

   
        private void HandleWoman()
        {
            var WomanList = ContendersList.Where(x => x.IsMale == false).Select(w => w).ToList();
            if (WomanList.Count <= 1)
                return;

            BracketsBuilder WomanInstace = new BracketsBuilder(WomanList, true);
            WomanInstace.Init();
            // remove from that instance the womans from woman instance and add their brackets for that instance
            // woman that would not remove will stay with the boys without factors
            for (int i = 0; i < WomanInstace.BracketsList.Count; i++)
            {
                // add to this bracketsList
                this.BracketsList.Add(WomanInstace.BracketsList[i]);
                // remove from this instance
                for (int j = 0; j < WomanInstace.BracketsList[i].ContendersList.Count; j++)
                {
                    var cont = ContendersList.Where(x => x.SystemID == WomanInstace.BracketsList[i].ContendersList[j].SystemID).First();
                    cont.IsPlaced = true;
                    cont.FinalGradeInBracket = WomanInstace.BracketsList[i].ContendersList[j].FinalGradeInBracket;

                    RemoveItemFromList(WomanInstace.BracketsList[i].ContendersList[j].SystemID, false);

                }
            }
        }

        private void HandleLessThanNumOfConts(int NumOfContsToHandle)
        {
            // start handle from the higher conts (n-1) to the lower (n-(x=2))
            if (NumOfContsToHandle < 2)
                return;

            for (int i = 0; i < ContendersList.Count; i++)
            {
                if (ContendersList[i].GetMaxNumberOfContsBracketNum() == NumOfContsToHandle && ContendersList[i].IsPlaced == false)
                {
                    if (ContendersList[i].GetMostRecommendedBracket().Frquency == NumOfContsToHandle)
                    {
                        if (IsPerfectForEverybody(ContendersList[i].SystemID) == true)
                        {
                            // add to bracket
                            BuiledBracket(ReturnContendersByListOfIds(ContendersList[i].GetMostRecommendedBracket().ParticipantsIDs), ContendersList[i].GetMostRecommendedBracket().IdAndScore);
                            ArrangePotentialBracketsAgain();
                        }
                    }
                }
            }

            HandleLessThanNumOfConts(NumOfContsToHandle - 1);
        }

        private double GetProximty(int Freq)
        {
            // because we use order by in Contender.GetMostRecommendedBracket() method wee need that N will be the highest number
            // and after it will be N+1 , N+2 ....N+10000 but if the recommended bracket is less then N so it Must be N-1, N-2...N-10000
            // so for example: N=4: (Bracket has 7 Conts)--> 0.9997 ,(Bracket has 6 Conts)--> 0.9998 ,(Bracket has 5 Conts)--> 0.9999
            // ,(Bracket has 4 Conts)--> 1 ,(Bracket has 3 Conts)--> -0.0001 ,(Bracket has 7 Conts)--> -0.0002 ,(Bracket has 7 Conts)--> -0.0003

            int N = MartialArts.GeneralBracket.NumberOfContenders;
            if (Freq == N)
            {
                return 1;
            }
            else if (Freq < N)
            {
                int diff = N - Freq;
                double result = 0;
                for (int i = 0; i < diff; i++)
                {
                    result += (-0.0001);
                }
                return result;
            }
            else
            {
                int diff = Freq - N;
                double result = 1;
                for (int i = 0; i < diff; i++)
                {
                    result += (-0.0001);
                }
                return result;
            }

        }

        private bool IsPerfectForEverybody(int ID)
        {
            // this is the most rated bracket of this contender, now check if for other contenders its also the best bracket
            Contenders.Contender.PotentialBrackets p = ContendersList.Where(x => x.SystemID == ID).Select(c => c).Single().GetMostRecommendedBracket();
            bool IsPerfectForEverybody = true;
            for (int j = 0; j < p.ParticipantsIDs.Count; j++)
            {

                var p1 = ContendersList.Where(x => x.SystemID == p.ParticipantsIDs[j]).Select(c => c).Single();
                if (p1.IsPlaced == false && p1.GetMostRecommendedBracket().GeneralRate == p.GeneralRate)
                {
                    IsPerfectForEverybody = true;
                }
                else
                {
                    IsPerfectForEverybody = false;
                    break;
                }
            }
            return IsPerfectForEverybody;
        }

        private void ReducePerfectBrackets()
        {
            foreach (Contender c in ContendersList.ToList())
            {
                if (c.IsPlaced == false)
                {
                    foreach (Contender.PotentialBrackets p in c.PbList.ToList())
                    {
                        if (p.Frquency == MartialArts.GeneralBracket.NumberOfContenders && p.StdDivision == 0 && p.OriginalScoresRating == 1)
                        {
                            // its a perfect  bracket with perfect number of contenders - create a bracket
                            BuiledBracket(ReturnContendersByListOfIds(p.ParticipantsIDs), p.IdAndScore);
                            ArrangePotentialBracketsAgain();
                        }
                    }
                }
            }
        }


        private void HandleSmallBrackets()
        {
 
            if (ContendersList.Count == 0)
                return;

            while (ContendersList.Count > 0 && ContendersList.AsEnumerable().Select(x => x.MostRecomendedBracketFrequency).DefaultIfEmpty().Max() > 0 
                && ContendersList.AsEnumerable().Select(x => x.MostRecomendedBracketFrequency).DefaultIfEmpty().Max() < MartialArts.GeneralBracket.NumberOfContenders)
            {

                // extract contender with Max Recomended Bracket
                int MaxRecBracket = ContendersList.AsEnumerable().Select(x => x.MostRecomendedBracketFrequency).DefaultIfEmpty().Max();
                Contender ContMaxRec = ContendersList.
                Where(x => x.MostRecomendedBracketFrequency == MaxRecBracket).Select(x => x).First();

                // for safty check if not placed
                if (ContMaxRec.IsPlaced == false)
                {
                    // extract all the contenders of that recomnded bracket
                    var Conts = ReturnContendersByListOfIds(ContMaxRec.GetMostRecommendedBracket().ParticipantsIDs);
                    // for safty check that no body is placed
                    if (Conts.All(x => x.IsPlaced == false))
                    {
                        // builed bracket and arage potential brackets again
                        BuiledBracket(Conts, ContMaxRec.GetMostRecommendedBracket().IdAndScore);
                        if (ContendersList.Count == 0)
                            return;
                        ArrangePotentialBracketsAgain();

                    }
                }
            }

            if (ContendersList.AsEnumerable().Select(x => x.MostRecomendedBracketFrequency).DefaultIfEmpty().Max() >= MartialArts.GeneralBracket.NumberOfContenders)
            {
                HandleBrackets();
            }
        }


        private void HandleBrackets()
        {
           
            if (ContendersList.Count <= 1)
                return;

            while (ContendersList.AsEnumerable().Select(x => x.MostRecomendedBracketFrequency).DefaultIfEmpty().Max() >= MartialArts.GeneralBracket.NumberOfContenders && ContendersList.Count > 0 )
            {
                // extract contender with Max Recomended Bracket
                int MaxRecBracket = ContendersList.AsEnumerable().Select(x => x.MostRecomendedBracketFrequency).DefaultIfEmpty().Max();
                Contender ContMaxRec = ContendersList.
                Where(x => x.MostRecomendedBracketFrequency == MaxRecBracket).Select(x => x).First();

                // for safty check if not placed
                if (ContMaxRec.IsPlaced == false)
                {
                    // extract all the contenders of that recomnded bracket
                    var Conts = ReturnContendersByListOfIds(ContMaxRec.GetMostRecommendedBracket().ParticipantsIDs);
                    // for safty check that no body is placed
                    if (Conts.All(x => x.IsPlaced == false))
                    {

                        if (Conts.Count > MartialArts.GeneralBracket.NumberOfContenders)
                        {
                            SplitBigBracket(Conts, ContMaxRec.GetMostRecommendedBracket());
                        }
                        else
                        {
                            // builed bracket and arage potential brackets again
                            BuiledBracket(Conts, ContMaxRec.GetMostRecommendedBracket().IdAndScore);
                            ArrangePotentialBracketsAgain();
                        }                       

                        if (ContendersList.Count == 1)
                            return;
                    }
                }
            }

            if (ContendersList.AsEnumerable().Select(x => x.MostRecomendedBracketFrequency).DefaultIfEmpty().Max() < MartialArts.GeneralBracket.NumberOfContenders)
            {
                HandleSmallBrackets();
            }
        }



        /// <summary>
        /// filter the most fitted N contenders for a bracket of N 
        /// the rest of the contenders that is less rated contenders will stay in ContenderList and will be back in the next iterations
        /// </summary>
        /// <param name="Conts"></param>
        /// <param name="RecomendedBracket"></param>
        /// <returns></returns>
        private void SplitBigBracket(List<Contender> Conts, Contender.PotentialBrackets RecomendedBracket)
        {

            // create variance of academies
            BracketsCreator.CreateAcademyVariance(ref Conts);


            // for variance score
            int Variance = 1;
            // for calculating abs z score of weight
            double WeightAverage;
            double WeightStdDiv;
            WeightStdDiv = GetStdDivision(Conts.Select(x => x.Weight).ToArray(), out WeightAverage);
            // create a struct with fields to calculate the most fitted contenders for bracket of N
            List<DecisionStruct> SortedConts = new List<DecisionStruct>();

            for (int i = 0; i < Conts.Count; i++)
            {
                var decision = new DecisionStruct();
                decision.SystemID = Conts[i].SystemID;
                decision.Abs_Weight_Z_Score = Get_Abs_Weight_Z_Score(WeightAverage, WeightStdDiv, Conts[i].Weight);
                decision.CurrentBracketScore = RecomendedBracket.IdAndScore[Conts[i].SystemID]; // score for that bracket
                string AcademyName = Conts[i].AcademyName;
                // score each academy by the number of frequency, the more the academy is more rare the score will be higher (in order to sort by orderby with the conditions)
                // for example: bracket of N=8 and the academis are: A,A,B,C,D,E,F,G so the score of A Will be 2/8 the the rest will be 1/8 - lower grade for rare academy
                //  Conts.OrderByDescending(i => i.OriginalScoreRank).ThenBy(n => n.ProximityRank).ThenBy(n => n.StdRank);
                decision.AcademyVarianceScore = Variance += 1;
                SortedConts.Add(decision);
            }
            // the conditions to rate the most rated contenders
            SortedConts = SortedConts.OrderBy(s => s.CurrentBracketScore).ThenBy(n => n.AcademyVarianceScore).ThenBy(w => w.Abs_Weight_Z_Score).ToList();

            // dictionary to hold the coosen contenders from the first N indexes of SortedConts List 
            Dictionary<int, double> ChoosenContsIdAndFinalScore = new Dictionary<int, double>();
            List<Contender> ChoosenConts = new List<Contender>();

            for (int c = 0; c < MartialArts.GeneralBracket.NumberOfContenders; c++)
            {
                ChoosenContsIdAndFinalScore.Add(SortedConts[c].SystemID, SortedConts[c].CurrentBracketScore);
                ChoosenConts.Add(ContendersList.Where(x => x.SystemID == SortedConts[c].SystemID).Select(z => z).First());
            }

            // builed bracket and arage potential brackets again
            BuiledBracket(ChoosenConts, ChoosenContsIdAndFinalScore);
            ArrangePotentialBracketsAgain();

        }

        private void CreateBracketsRating()
        {
            AllPotentialBracketsAverage = GetAverage(ContendersList, out AllPotentialBracketsStdDivision);
        }

        private void RemoveUselesses()
        {
            // the reason for the syntax ContendersList.ToList(): http://stackoverflow.com/questions/604831/collection-was-modified-enumeration-operation-may-not-execute
            foreach (Contender c in ContendersList.ToList())
            {

                if (c.PbList.Count == 0)
                {
                    RemoveItemFromList(c.SystemID, true);

                }

            }
        }
        /// <summary>
        /// each contender has a list of potential brackets struct with statistic info about all the potential brackets
        /// this method will load this property inside each contender obj
        /// </summary>
        private void LoadContsPotentialBrackets(bool UpdateArchive = false)
        {
            if (ContendersList.Count <= 1)
                return;

            foreach (Contenders.Contender c in ContendersList)
            {
                c.ClearPbList();
                // extract all potential scores for that contender
                var contenderPotential = PotentialScores.AsEnumerable().Where(x => x.SystemID == c.SystemID).Select(p => p).ToList();
                // iterate trough all potential scores
                foreach (ScoreAndID p in contenderPotential)
                {
                    // almost everyone has few scores of 0 (grade with false condition value) dont add them
                    if (p.Score > 0)
                    {

                        int freq = PotentialScores.Where(x => Math.Floor(x.Score) == p.RoundDownScore).Count();
                        // fequency of 1 is useless, parcitipants that have only frequencies of 1 dont have any contenders and they will be removed
                        if (freq > 1)
                        {
                            // list of ID`s of that bracket combination
                            List<int> bracketIds;
                            // Dictionary to hold also ID`s parallel to scores
                            Dictionary<int, double> IdAndScore = new Dictionary<int, double>();
                            // statstic measure for that option: shows the rate of original (most idial) of all contenders in that combination 
                            decimal OriginalRating;
                            // gender var
                            GlobalVars.GenderEnum gender;
                            // std division of the combination (idial is std division of 0)
                            double Statistics = GetBracketStatistic(p.Score, out OriginalRating, out bracketIds, ref IdAndScore, out gender);
                            // the rate of proximity to number of contenders. if number of contenders is 4 and there are 4 contenders in that combination ProximityToNumOfConts will be 1
                            double ProximityToNumOfConts = GetProximty(freq);

                            c.AddPotentialBracket(p.Score, freq, Statistics, OriginalRating, ProximityToNumOfConts, bracketIds, IdAndScore, gender);
                            c.CreateRanks();

                            // add only for the first time, to use in GUI for recomdations
                            if (UpdateArchive == true)
                                c.AddPotentialBracketToArchive(p.Score, freq, Statistics, OriginalRating, ProximityToNumOfConts, bracketIds, IdAndScore, gender);
                        }
                    }
                }
            }
        }




        public void RemoveItemFromList<T>(ref List<T> list, int sysId, bool SetToUselessCont) where T : Contenders.IContender
        {
            var ItemToRemove = list.SingleOrDefault(r => r.SystemID == sysId);
            // add to useless contenders list
            UselessContenders.Add(ContendersList.Where(x => x.SystemID == sysId).Select(x => x).Single());

            // for case of removing participants that have no contenders
            if (SetToUselessCont == true)
                ContendersList.Where(x => x.SystemID == ItemToRemove.SystemID).Single().IsUseless = true;
            else
                ContendersList.Where(x => x.SystemID == ItemToRemove.SystemID).Single().IsPlaced = true;

            // remove
            list.Remove(ItemToRemove);

            // remove from statistics all the combinations of that contender
            var statistcs = PotentialScores.Where(r => r.SystemID == sysId).Select(x => x).ToList();
            foreach (var x in statistcs)
            {
                PotentialScores.Remove(x);
            }

        }

        private void RemoveItemFromList(int sysId, bool SetToUselessCont)
        {
            var ItemToRemove = ContendersList.SingleOrDefault(r => r.SystemID == sysId);

            //if (ItemToRemove == null)
            //    return;



            // for case of removing participants that have no contenders
            if (SetToUselessCont == true)
            {
                ContendersList.Where(x => x.SystemID == ItemToRemove.SystemID).Single().IsUseless = true;
                // add to useless contenders list
                UselessContenders.Add(ContendersList.Where(x => x.SystemID == sysId).Select(x => x).Single());
                // remove
                ContendersList.Remove(ItemToRemove);
            }
            else
            {
                ContendersList.Where(x => x.SystemID == ItemToRemove.SystemID).Single().IsPlaced = true;
                // remove
                ContendersList.Remove(ItemToRemove);
            }



            ArrangePotentialBracketsAgain();

        }

        private double GetBracketStatistic(double score, out decimal OriginlGradesRating, out List<int> PotentialContsIDs, ref Dictionary<int, double> IdAndScore, out GlobalVars.GenderEnum Gender)
        {
            double examinedScore = Math.Floor(score);
            var l = PotentialScores.Where(x => Math.Floor(x.Score) == Math.Floor(examinedScore)).ToList();
            // all conts ids of this combination
            PotentialContsIDs = l.Select(x => x.SystemID).ToList();
            foreach (ScoreAndID s in l)
            {
                IdAndScore.Add(s.SystemID, s.Score);
            }

            // gender
            if (l.All(x => x.IsMale == true))
            {
                Gender = GlobalVars.GenderEnum.Man;
            }
            else if (l.All(x => x.IsMale == false))
            {
                Gender = GlobalVars.GenderEnum.Woman;
            }
            else
            {
                Gender = GlobalVars.GenderEnum.Mixed;
            }

            // calculate std division
            double average = l.Average(x => x.Score);
            double sumOfSquaresOfDifferences = l.Select(val => (val.Score - average) * (val.Score - average)).Sum();
            double sd = Math.Sqrt(sumOfSquaresOfDifferences / l.Count);


            OriginlGradesRating = (decimal)(l.Count(x => x.Score == Math.Floor(examinedScore))) / (decimal)(l.Count);

            return sd;
        }

        private double GetStdDivision(double[] Numbers, out double average)
        {
            // calculate average
            average = Numbers.Average();
            var av = average;
            double sumOfSquaresOfDifferences = Numbers.Select(val => (val - av) * (val - av)).Sum();
            double sd = Math.Sqrt(sumOfSquaresOfDifferences / Numbers.Length);
            return sd;
        }
        private double Get_Abs_Weight_Z_Score(double average, double stdDivision, double weight)
        {
            if (average == 0 || stdDivision == 0 || weight == 0 || weight < 10 || average < 10)
            {
                // if one of the parametes does not make sense return a random reasnible number
                Random r = new Random();
                return r.NextDouble() * 1.5;

            }
            // return the Z score of the weight of the contnder with absulute value in order to rank from averge down and query with OREDER BY
            return Math.Abs((weight - average) / stdDivision);
        }

        private void ArrangePotentialBracketsAgain(bool UpdateArchive = false)
        {
            CreateScoreAndID();
            LoadContsPotentialBrackets(UpdateArchive);
        }
        private void SortPotentialScoresAgain()
        {
            var l = PotentialScores.AsEnumerable().OrderBy(x => x.Score).ToList();
            PotentialScores = l;
        }

        private void BuiledBracket(List<Contenders.Contender> br, Dictionary<int, double> IdAndScore)
        {
            // bracket data
            int HighestAge = br.Max(x => x.AgeCategory);
            int HighestBelt = br.Max(x => x.Belt);
            int HighestWeight = br.Max(x => x.WeightCategory);

            GlobalVars.GenderEnum bracketGender;
            if (br.All(x => x.IsMale == true))
            {
                bracketGender = GlobalVars.GenderEnum.Man;
            }
            else if (br.All(x => x.IsMale == false))
            {
                bracketGender = GlobalVars.GenderEnum.Woman;
            }
            else
            {
                bracketGender = GlobalVars.GenderEnum.Mixed;
            }

            MartialArts.Bracket b = new Bracket(HighestAge, HighestBelt, HighestWeight);
            // add contenders
            b.AddContenders(br);
            // add to brackets list
            BracketsList.Add(b);
            // reduce the contenders from the contendersList and statistics
            foreach (Contenders.Contender c in br)
            {
                // set that contender have bracket
                c.IsPlaced = true;
                // final grade
                c.FinalGradeInBracket = IdAndScore[c.SystemID];
                RemoveItemFromList(c.SystemID, false);
            }
        }

        public List<Contender> ReturnContendersByListOfIds(List<int> ids)
        {
            List<Contender> result = new List<Contender>();
            foreach (int i in ids)
            {
                Contender c = ContendersList.Where(x => x.SystemID == i).Select(d => d).SingleOrDefault();
                if (c != null)
                    result.Add(c);
            }

            return result;
        }

        public struct ScoreAndID : Contenders.IContender
        {
            public double Score;
            public int SystemID;
            int IContender.SystemID
            {
                get
                {
                    return SystemID;
                }

                set
                {
                    SystemID = value;
                }
            }
            public double RoundDownScore
            {
                get
                {
                    return Math.Floor(Score);
                }
            }
            public bool IsUseless
            {
                get
                {
                    return IsUseless;
                }

                set
                {
                    IsUseless = value;
                }
            }

            public bool IsPlaced
            {
                get
                {
                    return IsPlaced;
                }

                set
                {
                    IsPlaced = value;
                }
            }

            public bool IsMale;
        }

        public struct DecisionStruct
        {
            public double CurrentBracketScore;
            public double AcademyVarianceScore;
            public double Abs_Weight_Z_Score;
            public int SystemID;
        }
    }

    class PotentialBracketsGeneralStatistics
    {
        public static double GetAverage(List<Contender> list, out double stdDivision)
        {
            List<int> result = new List<int>();
            foreach (Contender c in list)
            {
                foreach (Contender.PotentialBrackets p in c.PbList)
                {
                    result.Add(p.GeneralRate);
                }
            }

            // calculte std division
            double average = result.Average(x => x);
            double sumOfSquaresOfDifferences = 0;
            foreach (int i in result)
            {
                sumOfSquaresOfDifferences += (i - average) * (i - average);

            }

            stdDivision = Math.Sqrt(sumOfSquaresOfDifferences / result.Count);
            return average;
        }

    }
}
