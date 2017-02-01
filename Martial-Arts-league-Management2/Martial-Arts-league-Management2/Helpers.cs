using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Martial_Arts_league_Management2
{
    class Helpers
    {
        /// <summary>
        /// add image from embedded resources
        /// </summary>
        /// <param name="ImageFileName"></param>
        /// <returns>bitmap image</returns>
        public static Bitmap getImage(string ImageFileName)
        {
            System.Reflection.Assembly myAssembly = System.Reflection.Assembly.GetExecutingAssembly();
            using (Stream myStream = myAssembly.GetManifestResourceStream("Martial-Arts-league-Management2." + ImageFileName))
            {
                Bitmap image = new Bitmap(myStream);
                return image;
            }
        }

        public static void DefaultMessegeBox(string txt, string caption,System.Windows.Forms.MessageBoxIcon icon)
        {
            System.Windows.Forms.MessageBox.Show(txt,
            caption, System.Windows.Forms.MessageBoxButtons.OK,icon,
            System.Windows.Forms.MessageBoxDefaultButton.Button1, System.Windows.Forms.MessageBoxOptions.RightAlign);
        }


        private static string[,] _ColsRecognition;
        /// <summary>
        /// static property to get excel columns structure, array[x,0]= properties names array[x,1] = to search contained string,array[x,2] = requested full column name
        /// </summary>
        public static string[,] ColsRecognition
        {
            get
            {
                if (_ColsRecognition == null)
                {
                    _ColsRecognition = GetExcelColumnsRecognition();
                    return _ColsRecognition;
                }

                else
                {
                    return _ColsRecognition;
                }
            }
        }
        private static string[,] GetExcelColumnsRecognition()
        {
            string[,] ExcelColumnsRecognition = new string[19, 3];
            // column with the name of the property
            ExcelColumnsRecognition[0,0] = "FirstName";
            ExcelColumnsRecognition[1,0] = "Lastname";
            ExcelColumnsRecognition[2,0] = "ID";
            ExcelColumnsRecognition[3,0] = "Email";
            ExcelColumnsRecognition[4,0] = "PhoneNumber";
            ExcelColumnsRecognition[5,0] = "DateOfBirth";
            ExcelColumnsRecognition[6,0] = "AgeCategory";
            ExcelColumnsRecognition[7,0] = "IsMale";
            ExcelColumnsRecognition[8,0] = "Weight";
            ExcelColumnsRecognition[9,0] = "WeightCategory";
            ExcelColumnsRecognition[10,0] = "Belt";
            ExcelColumnsRecognition[11,0] = "AcademyName";
            ExcelColumnsRecognition[12,0] = "CoachName";
            ExcelColumnsRecognition[13,0] = "CoachPhone";
            ExcelColumnsRecognition[14,0] = "IsAllowedWeightGradeAbove";
            ExcelColumnsRecognition[15,0] = "IsAllowedAgeGradeAbove";
            ExcelColumnsRecognition[16,0] = "IsAllowedBeltGradeAbove";
            ExcelColumnsRecognition[17, 0] = "IsAllowedVersusMan";
            ExcelColumnsRecognition[18, 0] = "AcademyNameNotInCombo";

            // column is for validating the excel input sheet, its for search only with Containes() method
            ExcelColumnsRecognition[0, 1] = "שם פרטי";
            ExcelColumnsRecognition[1, 1] = "משפחה";
            ExcelColumnsRecognition[2, 1] = "תעודת זהות";
            ExcelColumnsRecognition[3, 1] = "אימייל";
            ExcelColumnsRecognition[4, 1] = "טלפון נייד";
            ExcelColumnsRecognition[5, 1] = "תאריך לידה";
            ExcelColumnsRecognition[6, 1] = "פחות שנת הלידה";
            ExcelColumnsRecognition[7, 1] = "מגדר";
            ExcelColumnsRecognition[8, 1] = "משקל אמיתי כולל חליפה ( שקילה לפי חוקי IBJJF)";
            ExcelColumnsRecognition[9, 1] = "קטגורית משקל.( לפי קטגוריות משקל IBJJF)";
            ExcelColumnsRecognition[10, 1] = "דרגת חגורה לפי IBJJF בלבד";
            ExcelColumnsRecognition[11, 1] = "שם אקדמיה";
            ExcelColumnsRecognition[12, 1] = "שם מאמן";
            ExcelColumnsRecognition[13, 1] = "מספר טלפון נייד של המאמן";
            ExcelColumnsRecognition[14, 1] = "במידה ולא ימצא מתחרה בקטגורית משקל";
            ExcelColumnsRecognition[15, 1] = "במידה ולא ימצא מתחרה מאותה קטגורית גיל";
            ExcelColumnsRecognition[16, 1] = "במידה ולא ימצא מתחרה מאותה דרגת חגורה";
            ExcelColumnsRecognition[17, 1] = "האם ניתן לשבץ בקטגוריה מעורבת";
            ExcelColumnsRecognition[18, 1] = "אם האקדמיה אינה מופיעה ברשימה";

            // column is for promting the user what header is missing in the input this array and headersForContaines indexes must be equal and
            // matched by the containes string and the real (original) expected header name
            ExcelColumnsRecognition[0, 2] = "שם פרטי";
            ExcelColumnsRecognition[1, 2] = "משפחה";
            ExcelColumnsRecognition[2, 2] = "תעודת זהות";
            ExcelColumnsRecognition[3, 2] = "אימייל";
            ExcelColumnsRecognition[4, 2] = "טלפון נייד";
            ExcelColumnsRecognition[5, 2] = "תאריך לידה";
            ExcelColumnsRecognition[6, 2] = "קטגורית גיל"  + "(" + DateTime.Now.Year.ToString() + "פחות שנת הלידה" + ")";
            ExcelColumnsRecognition[7, 2] = "מגדר";
            ExcelColumnsRecognition[8, 2] = "משקל אמיתי כולל חליפה ( שקילה לפי חוקי IBJJF)";
            ExcelColumnsRecognition[9, 2] = "קטגורית משקל.( לפי קטגוריות משקל IBJJF)";
            ExcelColumnsRecognition[10, 2] = "דרגת חגורה לפי IBJJF בלבד";
            ExcelColumnsRecognition[11, 2] = "שם אקדמיה";
            ExcelColumnsRecognition[12, 2] = "שם מאמן";
            ExcelColumnsRecognition[13, 2] = "מספר טלפון נייד של המאמן";
            ExcelColumnsRecognition[14, 2] = "במידה ולא ימצא מתחרה בקטגורית משקל -מעונין\\ת להתחרות בקטגורית משקל אחת גבוהה יותר.";
            ExcelColumnsRecognition[15, 2] = "במידה ולא ימצא מתחרה מאותה קטגורית גיל - מעונין\\ת להתחרות בקטגורית גיל אחת גבוהה יותר.";
            ExcelColumnsRecognition[16, 2] = "במידה ולא ימצא מתחרה מאותה דרגת חגורה- מעונין\\ת להתחרות בדרגת חגורה אחת גבוהה יותר.";
            ExcelColumnsRecognition[17, 2] = "אם בחרת נקבה האם ניתן לשבץ בקטגוריה מעורבת?";
            ExcelColumnsRecognition[18, 2] = "אם האקדמיה אינה מופיעה ברשימה - יש לרשום אותה";

            return ExcelColumnsRecognition;

        }




    }
}
