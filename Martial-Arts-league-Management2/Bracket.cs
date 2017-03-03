using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
// test git
namespace MartialArts
{

    class GeneralBracket
    {
        // set from GUI
        private static byte _NumberOfContenders = 4;
        public static byte NumberOfContenders
        {
            get
            {
                return _NumberOfContenders;
            }

            private set
            {
                if (value > 0 && value < 250)
                {
                    _NumberOfContenders = value;
                }
            }
        }
/// <summary>
/// set number of contenders for all brackets (user input)
/// </summary>
/// <param name="numberOfContenders"></param>
        public static void setNumberOfContenders(byte numberOfContenders)
        {
            if (numberOfContenders < 1 || numberOfContenders > 250)
                Helpers.DefaultMessegeBox("מספר המתחרים בבית לא יכול להיות קטן מ-1 או גדול מ-250", "מספר מתחרים לא חוקי", System.Windows.Forms.MessageBoxIcon.Information);
            else
                NumberOfContenders = numberOfContenders;
        }
    }

    class Bracket
    {
        private static int _BracketNumber = 0;
        public int BracketNumber { get; private set; }
        public bool IsChild
        {
            get
            {
                if (ContendersList[0].IsChild == true)
                    return true;
                else
                    return false;
            }
        }
        public int AgeGrade { get; set; }
        public int BeltGrade { get; set; }
        public int WeightGrade { get; set; }

        /// <summary>
        ///   will refresh data based on the higher contender
        ///   the aftermath is Tostring() method will be refreshed also
        /// </summary>
        public void RefreshBracketInfo()
        {
            AgeGrade = ContendersList.Max(x => x.AgeCategory);
            BeltGrade = ContendersList.Max(x => x.Belt);
            WeightGrade = ContendersList.Max(x => x.WeightCategory);
        }

        public double AverageGrade
        {
            get
            {
                return ContendersList.Average(x => x.FinalGradeInBracket);
            }
        }

        public GlobalVars.GenderEnum Gender
        {
            get
            {
                // gender
                if (ContendersList.All(x => x.IsMale == true))
                {
                    return GlobalVars.GenderEnum.Man;
                }
                else if (ContendersList.All(x => x.IsMale == false))
                {
                    return GlobalVars.GenderEnum.Woman;
                }
                else
                {
                    return GlobalVars.GenderEnum.Mixed;
                }
            }
           
        }

        public Bracket(int ageGrade,int BeltGrade,int WeightGrade)
        {
            this.AgeGrade = ageGrade;
            this.WeightGrade = WeightGrade;
            this.BeltGrade = BeltGrade;
           
            _BracketNumber += 1;
            BracketNumber = _BracketNumber;
           
        }
    
        private List< Contenders.Contender> _ContendersList;
        public List< Contenders.Contender> ContendersList
        {
            get
            {
                if (_ContendersList == null)
                {
                    this._ContendersList = new List<Contenders.Contender>();  
                    return this._ContendersList;
                }
                else
                {
                    return this._ContendersList;
                }

            }

            set
            {
                if (value != null)
                    _ContendersList = value;
            }
        }

        public int NumberOfContenders
        {
            get
            {
                return ContendersList.Count();
            }
        }

        public void AddContenders(List<Contenders.Contender> bracketConts)
        {
            ContendersList = bracketConts;
        }

        public void AddSingleContender(Contenders.Contender cont)
        {
            ContendersList.Add(cont);
        }

        // TODO: return the bracket description string
        public override string ToString()
        {
            string belt = Helpers.GetHebBeltName((Contenders.ContndersGeneral.BeltsEnum)BeltGrade);
            string age = Contenders.ContndersGeneral.AgeGrades.Where(x => x.Value == AgeGrade).Select(z => z.Key).SingleOrDefault();
            string weight = "";
            if (IsChild == true)
                weight = Contenders.ContndersGeneral.ChildWeightCat.Where(x => x.Value == WeightGrade).Select(z => z.Key).SingleOrDefault();
            else
                weight = Contenders.ContndersGeneral.AdultWeightCat.Where(x => x.Value == WeightGrade).Select(z => z.Key).SingleOrDefault();

            return "חגורה: " + belt + " " + "גילאי: " + age + " " + "קטגוריית משקל: " + weight + " " + "מגדר: " + GlobalVars.GEtGenderStringByEnum(Gender);
        }

    }
}
