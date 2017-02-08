using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Contenders
{
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

                    _AgeGrades.Add("4-5", 1);
                    _AgeGrades.Add("6-7", 2);
                    _AgeGrades.Add("8-9", 3);
                    _AgeGrades.Add("10-11", 4);
                    _AgeGrades.Add("12-13", 5);
                    _AgeGrades.Add("14-15", 6);
                    _AgeGrades.Add("16-17", 7);
                    _AgeGrades.Add("18-30", 8);
                    _AgeGrades.Add("31-35", 9);
                    _AgeGrades.Add("36-40", 10);
                    _AgeGrades.Add("41-45", 11);
                    _AgeGrades.Add("46-50", 12);
                    _AgeGrades.Add("51-55", 13);
                    _AgeGrades.Add("56", 14);
                    return _AgeGrades;
                }
                else
                {
                    return _AgeGrades;
                }
            }
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
            white = 1,
            gray = 2,
            yellow = 3,
            orange = 4,
            green = 5,
            blue = 6,
            purpule = 7,
            brown = 8,
            black = 9
        }
    }


    class Contender
    {
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
        public string AcademyName {
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

        public int Grade
        {
            get
            {
                return AgeCategory + WeightCategory + Belt;
            }
        }


 }
    /// <summary>
    /// this class crosses contender data against the intier entire league descriptive statistics
    /// its adds more statistic data to perform brackets division to each contender compare the the league
    /// </summary>
    class ContenderLeague : Contenders.Contender
    {
        private Martial_Arts_league_Management2.LeagueScattering League;
        private Contender _Contender;
        public Contender Contender
        {
            get
            {
                return _Contender;
            }
        }
        /// <summary>
        /// constructor
        /// </summary>
        /// <param name="contender">contender object</param>
        /// <param name="LeagueToCompare">league of all contenders to compare</param>
        public ContenderLeague(Contender contender,Martial_Arts_league_Management2.LeagueScattering LeagueToCompare)
        {
            this._Contender = contender;
            this.League = LeagueToCompare;
        }

        /// <summary>
        /// number of the contender incidents inside the league (count())
        /// </summary>
        public int FrequencyOfGrade
        {
            get
            {
                return League.Contenders.AsEnumerable().Where(x => x.Grade == Contender.Grade).Count();
            }
        }

    }  
    
}