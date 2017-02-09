﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Martial_Arts_league_Management2
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
        }
    }
    class Bracket
    {
        public int _BracketNumber { get; set; }
        public double BracketTotalGrade { get; set; }
        public int AgeGrade { get; set; }
        public int BeltGrade { get; set; }
        public int WeightGrade { get; set; }
        public GlobalVars.GenderEnum Gender { get; set; }
    
        private List< Contenders.Contender> _ContendersList;
        public List< Contenders.Contender>ContendersList
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
