using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Diagnostics;
namespace MartialArts
{
    class BracketsBuilder
    {
        protected List<Contenders.Contender> ContendersList = new List<Contenders.Contender>();
        protected List<ScoreAndID> PotentialScores = new List<ScoreAndID>();
        protected List<Contenders.Contender> SortedContendersList;
        public BracketsBuilder(List<Contenders.Contender> cont)
        {
            ContendersList = cont;

            foreach (Contenders.Contender c in ContendersList)
            {
                foreach (double PotentialScore in c)
                {
                    ScoreAndID si = new ScoreAndID();
                    si.Score = PotentialScore;
                    si.SystemID = c.SystemID;
                    PotentialScores.Add(si);
                }
            }

            var SortedContendersList = PotentialScores.AsEnumerable().OrderBy(x => x.Score);

            foreach (var itm in SortedContendersList)
            {
                Debug.WriteLine("{0}, {1}, {2}",itm.Score,itm.SystemID,SortedContendersList.AsEnumerable().Count(x=>x.Score == itm.Score));
            }

            foreach (Contenders.Contender c in ContendersList)
            {
                var contenderPotential = SortedContendersList.AsEnumerable().Where(x => x.SystemID == c.SystemID).Select(p => p).ToList();

                foreach (ScoreAndID p in contenderPotential)
                {
                    if (p.Score > 0)
                    {
                        
                        int freq = SortedContendersList.Where(x => Math.Floor(x.Score) == p.RoundDownScore).Count();
                        if (freq > 1)
                        {
                            decimal OriginalRating;
                            double Statistics = GetBracketStdDivision(p.Score, out OriginalRating);
                            double ProximityToNumOfConts = (double)freq / (double)(MartialArts.GeneralBracket.NumberOfContenders);
                            c.AddPotentialBracket(p.Score, freq,Statistics,OriginalRating,ProximityToNumOfConts);
                        }
                    }
                }
            }

            foreach (Contenders.Contender c in ContendersList)
            {
                foreach (Contenders.Contender.PotentialBrackets p in c.PbList)
                {
                    Debug.WriteLine("{0}, {1}, {2}, {3}, {4}", p.Score, p.Frquency,p.StdDivision,p.OriginalScoresRating.ToString("#.000"), p.proximityToNumOfConts.ToString("#.000"));
                }

                Debug.WriteLine("");
            }
        }


        private double GetBracketStdDivision(double score,out decimal OriginlGradesRating)
        {
            double examinedScore = Math.Floor(score);
            var l = PotentialScores.Where(x => Math.Floor(x.Score) == Math.Floor(examinedScore)).ToList();

            double average = l.Average(x => x.Score);

            double sumOfSquaresOfDifferences = l.Select(val => (val.Score - average) * (val.Score - average)).Sum();
            double sd = Math.Sqrt(sumOfSquaresOfDifferences / l.Count);

           
            OriginlGradesRating = (decimal)(l.Count(x => x.Score == Math.Floor(examinedScore))) / (decimal)(l.Count);

            return sd;
        }



        

        public struct ScoreAndID
        {
            public double Score;
            public int SystemID;
            public double RoundDownScore
            {
                get
                {
                    return Math.Floor(Score);
                }
            }
        }
    }




}
