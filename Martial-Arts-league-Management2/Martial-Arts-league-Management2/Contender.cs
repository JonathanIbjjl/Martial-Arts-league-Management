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
        public int Weight { get; set; }
        public int WeightCategory { get; set; }
        public int Belt { get; set; }
        public string AcademyName {
            get
            {
                if (AcademyName == string.Empty)
                    return AcademyNameNotInCombo;
                else
                    return AcademyName;
            }
            set
            {
                AcademyName = value;
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

 }

}