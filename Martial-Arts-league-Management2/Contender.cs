using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MartialArts;

namespace Contenders
{
   
    interface IContender
    {
        int SystemID { get; set; }
        bool IsUseless { get; set; }
        bool IsPlaced { get; set; }
    }


    [Serializable]
    class ContndersGeneral
    {
        // hash table for excel column names (keys) and column number (value)
        private Dictionary<string, int> _HeadersDictionary;
        public Dictionary<string, int> HeadersDictionary
        {
            get
            {
                if (_HeadersDictionary == null)
                {
                    _HeadersDictionary = new Dictionary<string, int>();
                    return _HeadersDictionary;
                }

                else
                {
                    return _HeadersDictionary;
                }
            }
            set
            {
                _HeadersDictionary = value;
            }
        }

        // must not be null on excel sheet
        public string[] NotAllowedNullColumns = { "FirstName", "LastName", "ID", "AgeCategory",
            "IsMale", "WeightCategory", "Weight", "Belt", "AcademyName","IsAllowedWeightGradeAbove","IsAllowedBeltGradeAbove" ,"IsAllowedVersusMan","Email","PhoneNumber"};

        private static Dictionary<string, int> _AgeGrades;
        public static Dictionary<string, int> AgeGrades
        {
            get
            {
                if (_AgeGrades == null)
                {
                    _AgeGrades = new Dictionary<string, int>();

                    _AgeGrades.Add("4-5", 100);
                    _AgeGrades.Add("6-7", 150);
                    _AgeGrades.Add("8-9", 200);
                    _AgeGrades.Add("10-11", 250);
                    _AgeGrades.Add("12-13", 300);
                    _AgeGrades.Add("14-15", 350);
                    _AgeGrades.Add("16-17", 400);
                    _AgeGrades.Add("18-30", 450);
                    _AgeGrades.Add("31-35", 500);
                    _AgeGrades.Add("36-40", 550);
                    _AgeGrades.Add("41-45", 600);
                    _AgeGrades.Add("46-50", 650);
                    _AgeGrades.Add("51-55", 700);
                    _AgeGrades.Add("56", 750);
                    return _AgeGrades;
                }
                else
                {
                    return _AgeGrades;
                }
            }


        }

        public static List<string> GetAgeValues(bool childAges)
        {
            List<string> l = new List<string>();

            if (childAges == true)
            {
                l.Add("4-5");
                l.Add("6-7");
                l.Add("8-9");
                l.Add("10-11");
                l.Add("12-13");
                l.Add("14-15");
                l.Add("16-17");
            }
            else
            {
                l.Add("18-30");
                l.Add("31-35");
                l.Add("36-40");
                l.Add("41-45");
                l.Add("46-50");
                l.Add("51-55");
                l.Add("56");
            }

            return l;
        }

        private static Dictionary<string, int> _ChildWeightCat;
        public static Dictionary<string, int> ChildWeightCat
        {
            get
            {
                if (_ChildWeightCat == null)
                {
                    _ChildWeightCat = new Dictionary<string, int>();

                    ChildWeightCat.Add("עד 21", 1);
                    ChildWeightCat.Add("מעל 21", 2);
                    ChildWeightCat.Add("עד 24", 3);
                    ChildWeightCat.Add("עד 27", 4);
                    ChildWeightCat.Add("עד 30", 5);
                    ChildWeightCat.Add("מעל 30", 6);
                    ChildWeightCat.Add("עד 34", 7);
                    ChildWeightCat.Add("עד 38", 8);
                    ChildWeightCat.Add("מעל 38", 9);
                    ChildWeightCat.Add("עד 42", 10);
                    ChildWeightCat.Add("עד 46", 11);
                    ChildWeightCat.Add("עד 50", 12);
                    ChildWeightCat.Add("מעל 50", 13);
                    ChildWeightCat.Add("עד 55", 14);
                    ChildWeightCat.Add("מעל 55", 15);
                    ChildWeightCat.Add("עד 60", 16);
                    ChildWeightCat.Add("עד 65", 17);
                    ChildWeightCat.Add("מעל 65", 18);
                    ChildWeightCat.Add("עד 70", 19);
                    ChildWeightCat.Add("עד 75", 20);
                    ChildWeightCat.Add("עד 80", 21);
                    ChildWeightCat.Add("מעל 80", 22);

                    return _ChildWeightCat;
                }
                else
                {
                    return _ChildWeightCat;
                }
            }
        }

        private static Dictionary<string, int> _AdultWeightCat;
        public static Dictionary<string, int> AdultWeightCat
        {
            get
            {
                if (_AdultWeightCat == null)
                {
                    _AdultWeightCat = new Dictionary<string, int>();

                    AdultWeightCat.Add("עד 53.5", 14);
                    AdultWeightCat.Add("עד 58.5", 15);
                    AdultWeightCat.Add("עד 60", 16);
                    AdultWeightCat.Add("עד 64", 17);
                    AdultWeightCat.Add("עד 69", 18);
                    AdultWeightCat.Add("עד 70", 19);
                    AdultWeightCat.Add("עד 74", 20);
                    AdultWeightCat.Add("מעל 74", 21);
                    AdultWeightCat.Add("עד 76", 22);
                    AdultWeightCat.Add("עד 82", 23);
                    AdultWeightCat.Add("עד 88", 24);
                    AdultWeightCat.Add("עד 94", 25);
                    AdultWeightCat.Add("עד 100", 26);
                    AdultWeightCat.Add("מעל 100", 27);

                    return _AdultWeightCat;
                }
                else
                {
                    return _AdultWeightCat;
                }
            }
        }

        public enum BeltsEnum
        {
            white = 1000,
            gray = 2000,
            yellow = 3000,
            orange = 4000,
            green = 5000,
            blue = 6000,
            purpule = 7000,
            brown = 8000,
            black = 9000
        }

        public virtual int FrequencyOfGrade { get; }
        public virtual double FinalGrade { get; }


        public static int GetBelt(string color)
        {

            string result = color.Trim();
            switch (result.Trim())
            {
                case "לבנה":
                    return (int)Contenders.ContndersGeneral.BeltsEnum.white;

                case "אפורה":
                    return (int)Contenders.ContndersGeneral.BeltsEnum.gray;

                case "צהובה":
                    return (int)Contenders.ContndersGeneral.BeltsEnum.yellow;

                case "כתומה":
                    return (int)Contenders.ContndersGeneral.BeltsEnum.orange;

                case "ירוקה":
                    return (int)Contenders.ContndersGeneral.BeltsEnum.green;

                case "כחולה":
                    return (int)Contenders.ContndersGeneral.BeltsEnum.blue;

                case "סגולה":
                    return (int)Contenders.ContndersGeneral.BeltsEnum.purpule;

                case "חומה":
                    return (int)Contenders.ContndersGeneral.BeltsEnum.brown;

                case "שחורה":
                    return (int)Contenders.ContndersGeneral.BeltsEnum.black;

                default:
                    return 0;
            }

        }

        /// <summary>
        /// by exploring the numbers after decimal points, function will return a string with the explanation of the factor
        /// </summary>
        /// <param name="grade"></param>
        /// <returns></returns>
        public static string GetFactorExplanation(double grade)
        {
            if (grade <= 0) // only for safty, not suppose tha hapen
                return "";
            decimal FixGrade = (Decimal)grade;
            decimal Remaining = FixGrade - Math.Floor(FixGrade);
            string reason = "";

            if (Remaining == 0.01M)
               reason = "עם פקטור של משקל";
            else if (Remaining == 0.02M)
                reason = "עם פקטור של חגורה";
            else if (Remaining == 0.03M)
                reason = "עם פקטור של גיל";
            else if (Remaining == 0.04M)
                reason = "עם פקטור של משקל וחגורה";
            else if (Remaining == 0.05M)
                reason = "עם פקטור של משקל וגיל";
            else if (Remaining == 0.06M)
                reason = "עם פקטור של חגורה וגיל";
            else if (Remaining == 0.07M)
                reason = "עם פקטור של חגורה משקל וגיל";
            else if (Remaining == 0.08M)
                reason = "עם פקטור של אישור להתחרות מול בנים";
            else
                reason = "";

            return reason;
        }

        public static string GetFactorExplanation(Visual.VisualContender cont,Visual.VisualBracket vb)
        {
            double finalgrade;
            Visual.VisualLeagueEvent.IsSutibleForBracket(cont, vb,out finalgrade);

            return GetFactorExplanation(finalgrade);
        }
    }

    [Serializable]
    class Contender : ContndersGeneral, IContender, System.Collections.IEnumerable
    {
        private static int IdentityNumber = 999;
        public Contender()
        {
            // create uniq identity
            IdentityNumber += 1;
            SystemID = IdentityNumber;

            IsUseless = false;
            IsPlaced = false;
        }

        public bool SourceIsFromSystemList { get; set; } = false;
        public int SystemID { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string ID { get; set; }
        public string Email { get; set; }
        public string PhoneNumber { get; set; }
        public string DateOfBirth { get; set; } // its not DateTime Type because format is not validated inside google docs
        public int AgeCategory { get; set; }
        public bool IsMale { get; set; }
        public double Weight { get; set; }
        public int WeightCategory { get; set; }
        public int Belt { get; set; }
        private string _AcademyName;



        public string AcademyName
        {
            get
            {
                if (_AcademyName == "האקדמיה אינה מופיע ברשימה" || _AcademyName == "האקדמיה אינה מופיעה ברשימה" || _AcademyName.Contains("אינה מופיע"))
                    return AcademyNameNotInCombo;
                else
                    return _AcademyName;
            }
            set
            {
                _AcademyName = value;
            }
        }
        public string AcademyNameNotInCombo { get; set; }
        public string CoachName { get; set; }
        public string CoachPhone { get; set; }
        public bool IsAllowedWeightGradeAbove { get; set; }
        public bool IsAllowedAgeGradeAbove { get; set; }
        public bool IsAllowedBeltGradeAbove { get; set; }
        public bool IsAllowedVersusMan { get; set; }
        public bool IsChild { get; set; }

        public double WeightFactor
        {
            get
            {
                if (IsAllowedWeightGradeAbove == true)
                {
                    return 1;
                }
                else
                {
                    return 0;
                }
            }
        }
        public double AgeFactor
        {
            get
            {
                if (IsAllowedAgeGradeAbove == true)
                {
                    return 50;
                }
                else
                {
                    return 0;
                }
            }
        }
        public double BeltFactor
        {
            get
            {
                if (IsAllowedAgeGradeAbove == true)
                {
                    return 1000;
                }
                else
                {
                    return 0;
                }
            }
        }
        public double Grade
        {
            get
            {
                return (AgeCategory + WeightCategory + Belt);
            }
        }
        public double Score_WeightFactor
        {
            get
            {
                if (IsAllowedWeightGradeAbove == true)
                    return Grade + 1 + 0.01;
                else
                    return 0;
            }
        }
        public double Score_BeltFactor
        {
            get
            {
                if (IsAllowedBeltGradeAbove == true)
                    return Grade + 1000 + 0.02;
                else
                    return 0;
            }
        }
        public double Score_AgeFactor
        {
            get
            {
                if (IsAllowedAgeGradeAbove == true && IsChild == true)
                    return Grade + 50 + 0.03;
                else
                    return 0;
            }
        }
        public double Score_Weight_Belt_Factor
        {
            get
            {
                if (IsAllowedWeightGradeAbove == true && IsAllowedBeltGradeAbove == true)
                    return Grade + 1 + 1000 + 0.04;
                else
                    return 0;
            }
        }
        public double Score_Weight_Age_Factor
        {
            get
            {
                if (IsAllowedWeightGradeAbove == true && IsAllowedAgeGradeAbove == true && IsChild == true)
                    return Grade + 1 + 50 + 0.05;
                else
                    return 0;
            }
        }
        public double Score_Belt_Age_Factor
        {
            get
            {
                if (IsAllowedBeltGradeAbove == true && IsAllowedAgeGradeAbove == true && IsChild == true)
                    return Grade + 1000 + 50 + 0.06;
                else
                    return 0;
            }
        }
        public double Score_AllFactors
        {
            get
            {
                if (IsAllowedBeltGradeAbove == true && IsAllowedAgeGradeAbove == true && IsAllowedWeightGradeAbove == true && IsChild == true)
                    return Grade + 1 + 50 + 1000 + 0.07;
                else
                    return 0;
            }
        }
        public double Score_WomanToMan
        {
            get
            {
                if (IsMale == false && IsAllowedVersusMan == true)
                    return Grade + 0.5 + 0.08;
                else
                    return 0;
            }
        }
        private List<PotentialBrackets> _PbList;
        public List<PotentialBrackets> PbList
        {
            get
            {
                if (_PbList == null)
                {
                    _PbList = new List<PotentialBrackets>();
                    return _PbList;
                }

                else
                {
                    return _PbList;
                }
            }

            private set
            {
                _PbList = value;
            }
        }

        private List<PotentialBrackets> _PbListArchive;
        public List<PotentialBrackets> PbListArchive
        {
            get
            {
                if (_PbListArchive == null)
                {
                    _PbListArchive = new List<PotentialBrackets>();
                    return _PbListArchive;
                }

                else
                {
                    return _PbListArchive;
                }
            }

            private set
            {
                _PbListArchive = value;
            }
        }
        public bool IsUseless { get; set; }
        public bool IsPlaced { get; set; }
        public double FinalGradeInBracket { get; set; }

        public string HebrewBeltColor
        {
            get
            {
                return Helpers.GetHebBeltName((Contenders.ContndersGeneral.BeltsEnum)Belt);
            }
        }
        public System.Drawing.Color GetBeltColorValue
        {
            get
            {
                return Helpers.GetBeltColor(Belt);
            }
        }
        public string GetAgeValue
        {
            get
            {
                string age = "0-0";
                foreach (KeyValuePair<string, int> p in AgeGrades)
                {
                    if (p.Value == AgeCategory)
                        age = p.Key;
                }
                return age;
            }
        }
        public string GetWeightValue
        {
            get
            {
                string weight = "0-0";
                if (IsChild)
                {
                    foreach (KeyValuePair<string, int> p in ChildWeightCat)
                    {
                        if (p.Value == WeightCategory)
                            weight = p.Key;
                    }
                }
                else
                {
                    foreach (KeyValuePair<string, int> p in AdultWeightCat)
                    {
                        if (p.Value == WeightCategory)
                            weight = p.Key;
                    }
                }
                return weight;
            }
        }

        public IEnumerator GetEnumerator()
        {
            yield return Grade;
            yield return Score_WeightFactor;
            yield return Score_BeltFactor;
            yield return Score_AgeFactor;
            yield return Score_Weight_Belt_Factor;
            yield return Score_Weight_Age_Factor;
            yield return Score_Belt_Age_Factor;
            yield return Score_AllFactors;
            //  yield return Score_WomanToMan;
        }

        public void ClearPbList()
        {
            PbList.Clear();
        }



        public PotentialBrackets GetMostRecommendedBracket()
        {
            // most ideal for bracket by by priority: first priority is original score second place is proximity to n (frequency) and third place is std division beetween contenders 
            PbList = PbList.OrderByDescending(i => i.OriginalScoreRank).ThenBy(n => n.ProximityRank).ThenBy(n => n.StdRank).ToList();
            return PbList[0];

        }

        public int MostRecomendedBracketFrequency
        {
            get
            {
                if (PbList.Count > 0)
                    return GetMostRecommendedBracket().Frquency;
                else
                    return 0;
            }

        }


        public int GetMaxNumberOfContsBracketNum()
        {
            if (PbList.Count > 1)
                return PbList.Max(x => x.Frquency);
            else
                return 0;
        }

        public void CreateRanks()
        {
            for (int i = 0; i < PbList.Count; i++)
            {
                PbList[i] = GetStdRank(PbList[i]);
                PbList[i] = GetProximityRank(PbList[i]);
                PbList[i] = GetOriginalScoreRate(PbList[i]);
            }

            // order from higher to lower
            PbList.OrderByDescending(i => i.GeneralRate);
        }

        public PotentialBrackets GetStdRank(PotentialBrackets p)
        {
            // rank std division
            int result = PbList.Count;
            foreach (PotentialBrackets p1 in PbList)
            {
                if (p.StdDivision > p1.StdDivision)
                    result -= 1;
            }

            p.StdRank = result;
            return p;
        }

        public PotentialBrackets GetProximityRank(PotentialBrackets p)
        {
            // rank std division
            int result = PbList.Count;
            foreach (PotentialBrackets p1 in PbList)
            {
                if (p.proximityToNumOfConts < p1.proximityToNumOfConts)
                    result -= 1;
            }

            p.ProximityRank = result;
            return p;
        }

        public PotentialBrackets GetOriginalScoreRate(PotentialBrackets p)
        {
            // rank std division
            int result = PbList.Count;
            foreach (PotentialBrackets p1 in PbList)
            {
                if (p.OriginalScoresRating < p1.OriginalScoresRating)
                    result -= 1;
            }
            p.OriginalScoreRank = result;
            return p;
        }


        public byte PotentialBracetNum = 1;
        public void AddPotentialBracket(double score, int frequency, double stdDivision, decimal originalscoresrating,
            double proximitytonumofconts, List<int> participantsIDs, Dictionary<int, double> idandscore, GlobalVars.GenderEnum gender)
        {
            // make fixes if its a woman (is must not have any factor if its mixed house)
            PbList.Add(new PotentialBrackets
            {
                Score = score,
                Frquency = frequency,
                StdDivision = stdDivision,
                OriginalScoresRating = originalscoresrating,
                proximityToNumOfConts = proximitytonumofconts,
                ParticipantsIDs = participantsIDs,
                BracketID = PotentialBracetNum,
                IdAndScore = idandscore,
                Gender = gender
            });
            PotentialBracetNum += 1;
        }

        public void AddPotentialBracketToArchive(double score, int frequency, double stdDivision, decimal originalscoresrating,
            double proximitytonumofconts, List<int> participantsIDs, Dictionary<int, double> idandscore, GlobalVars.GenderEnum gender)
        {
            // make fixes if its a woman (is must not have any factor if its mixed house)
            PbListArchive.Add(new PotentialBrackets
            {
                Score = score,
                Frquency = frequency,
                StdDivision = stdDivision,
                OriginalScoresRating = originalscoresrating,
                proximityToNumOfConts = proximitytonumofconts,
                ParticipantsIDs = participantsIDs,
                BracketID = PotentialBracetNum,
                IdAndScore = idandscore,
                Gender = gender
            });
            PotentialBracetNum += 1;
        }

        [Serializable]
        public struct PotentialBrackets
        {
            public byte BracketID;
            public double Score;
            public int Frquency;
            public GlobalVars.GenderEnum Gender;
            public double StdDivision;
            public decimal OriginalScoresRating;
            public double proximityToNumOfConts;
            public List<int> ParticipantsIDs;
            public Dictionary<int, double> IdAndScore;
            // calculated vars
            public int StdRank;
            public int ProximityRank;
            public int OriginalScoreRank;
            public int GeneralRate
            {
                get
                {
                    return StdRank + ProximityRank + OriginalScoreRank;
                }
            }

        }

    }



    /// <summary>
    /// this class crosses contender data against the intier entire league descriptive statistics
    /// its adds more statistic data to perform brackets division to each contender compare the the league
    /// </summary>
    class ContenderLeague : ContndersGeneral, IContender
    {
        private MartialArts.LeagueScattering League;

        // 1.0 the first initilization the class will use League field after manipulating contenders
        // LoadNewScatteringStatistics() will be activated, after that activation the class will
        // use LoadNewScatteringStatistics when calling to FrequencyOfGrade property
        private bool UseLeagueNewStatistics = false;
        private MartialArts.ScattteringWithContenderLeague NewScatteringStatistics;

        private Contender _Contender;
        public Contender Contender
        {
            get
            {
                return _Contender;
            }
        }
        // upgrade in grade variables
        private double _Factor = 0;
        public double Factor
        {
            get { return _Factor; }
            set { _Factor = value; }
        }
        public override double FinalGrade
        {
            get
            {
                return Grade + Factor;

            }
        }

        /// <summary>
        /// constructor
        /// </summary>
        /// <param name="contender">contender object</param>
        /// <param name="LeagueToCompare">league of all contenders to compare</param>
        public ContenderLeague(Contender contender, MartialArts.LeagueScattering LeagueToCompare)
        {
            this._Contender = contender;
            this.League = LeagueToCompare;

        }

        /// <summary>
        /// number of the contender incidents inside the league (count())
        /// </summary>
        public override int FrequencyOfGrade
        {
            get
            {
                // read comment 1.0
                if (UseLeagueNewStatistics == false)
                    return League.Contenders.AsEnumerable().Where(x => x.Grade == Contender.Grade).Count();
                else
                    return NewScatteringStatistics.ContendersLeague.AsEnumerable().Where(x => x.FinalGrade == FinalGrade).Count();
            }
        }

        public void LoadNewScatteringStatistics(ScattteringWithContenderLeague s)
        {
            NewScatteringStatistics = s;
            UseLeagueNewStatistics = true;
        }

        public double Grade
        {
            get
            {
                return Contender.Grade;
            }
        }

        public string AcademyName
        {
            get
            {
                return Contender.AcademyName;
            }
        }

        public int SystemID
        {
            get
            {
                throw new NotImplementedException();
            }

            set
            {
                throw new NotImplementedException();
            }
        }

        public bool IsUseless
        {
            get
            {
                throw new NotImplementedException();
            }

            set
            {
                throw new NotImplementedException();
            }
        }

        public bool IsPlaced
        {
            get
            {
                throw new NotImplementedException();
            }

            set
            {
                throw new NotImplementedException();
            }
        }

        public override string ToString()
        {
            return FinalGrade.ToString();
        }
    }

}