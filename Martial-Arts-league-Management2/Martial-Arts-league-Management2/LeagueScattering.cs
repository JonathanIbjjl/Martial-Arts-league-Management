using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Martial_Arts_league_Management2
{
    /// <summary>
    /// class that handles Diffusion indices of the league
    /// </summary>
    class LeagueScattering
    {
        private ICollection<Contenders.Contender> _Contenders;
        public ICollection<Contenders.Contender> Contenders
        {
            get
            {
                return _Contenders;
            }
        }

        public Dictionary<int, int> RankOfFrequencies;
        public Dictionary<string, int> RankOfAcademys;

        public LeagueScattering(List<Contenders.Contender> contenders)
        {
            this._Contenders = contenders;
            // create rank of frequencies dictionary
            SetRankOfFrequencies();
            // create rank of academies dictionary
            SetRankOfAcademies();
        }


        private void SetRankOfFrequencies()
        {
            // temp dictionary (will be sorted for the global dictionary)
          Dictionary<int,int>  TempRankOfFrequencies = new Dictionary<int, int>();
            // extract distinct grades
            int[] DistinctGrades = Contenders.Select(x => x.Grade).Distinct().ToArray();
            // EXTRACT DATA: key: distinct grade value: number of insidents in all league
            foreach (int g in DistinctGrades)
            {
                TempRankOfFrequencies.Add(g, Contenders.Where(x => x.Grade == g).Count());
            }
            // sort the dictionary by value(rank)
           RankOfFrequencies = new Dictionary<int, int>();
           var ordered = TempRankOfFrequencies.OrderBy(x => x.Value);
           RankOfFrequencies = ordered.ToDictionary(t => t.Key, t => t.Value);
        }

        private void SetRankOfAcademies()
        {
            // temp dictionary (will be sorted for the global dictionary)
            Dictionary<string, int> TempRankOfAcademies = new Dictionary<string, int>();
            // extract distinct grades
            string[] DistinctAcademies = Contenders.Select(x => x.AcademyName).Distinct().ToArray();
            // EXTRACT DATA: key: distinct grade value: number of insidents in all league
            foreach (string n in DistinctAcademies)
            {
                TempRankOfAcademies.Add(n, Contenders.Where(x => x.AcademyName == n).Count());
            }

            // sort the dictionary by value(rank)
            RankOfAcademys = new Dictionary<string, int>();
            var ordered = TempRankOfAcademies.OrderBy(x => x.Value);
            RankOfAcademys = ordered.ToDictionary(t => t.Key, t => t.Value);
        }


        /// <summary>
        /// average grade (belt, weight,age) of all contenders
        /// </summary>
        /// <returns></returns>
        public double GetAverageGrade()
        {
            return Contenders.Average(x => x.Grade);
        }

        /// <summary>
        /// standard division of all grades in league
        /// </summary>
        /// <returns></returns>
        public double GetStdDivision()

        {
            double average = GetAverageGrade();
            double sumOfSquaresOfDifferences = Contenders.Select(val => (val.Grade - average) * (val.Grade - average)).Sum();
            double sd = Math.Sqrt(sumOfSquaresOfDifferences / Contenders.Count);
            return sd;
        }
    }
}
