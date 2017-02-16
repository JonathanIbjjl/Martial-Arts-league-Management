using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
    


        public double BracketTotalGrade { get; set; }
        public int AgeGrade { get; set; }
        public int BeltGrade { get; set; }
        public int WeightGrade { get; set; }
        public GlobalVars.GenderEnum Gender { get; set; }

        public Bracket(int ageGrade,int BeltGrade,int WeightGrade,GlobalVars.GenderEnum gender)
        {
            this.AgeGrade = ageGrade;
            this.WeightGrade = WeightGrade;
            this.BeltGrade = BeltGrade;
            this.Gender = gender;
            _BracketNumber += 1;
            BracketNumber = _BracketNumber;
           
        }
    
        private List< Contenders.ContenderLeague> _ContendersList;
        public List< Contenders.ContenderLeague> ContendersList
        {
            get
            {
                if (_ContendersList == null)
                {
                    this._ContendersList = new List<Contenders.ContenderLeague>();  
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

        // TODO: return the bracket description string
        public override string ToString()
        {
            string belt = Helpers.GetHebBeltName((Contenders.ContndersGeneral.BeltsEnum)BeltGrade);
            string age = Contenders.ContndersGeneral.AgeGrades.Where(x => x.Value == AgeGrade).Select(z => z.Key).SingleOrDefault();
            string weight = Contenders.ContndersGeneral.ChildWeightCat.Where(x => x.Value == WeightGrade).Select(z => z.Key).SingleOrDefault();

            return "חגורה: " + belt + " " + "גילאי: " + age + " " + "קטגוריית משקל: " + weight + " " + "מגדר: " + GlobalVars.GEtGenderStringByEnum(Gender);
        }

    }
}
