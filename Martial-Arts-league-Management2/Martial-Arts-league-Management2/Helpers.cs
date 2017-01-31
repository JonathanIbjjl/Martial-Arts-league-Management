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



        /// <summary>
        /// array is for validating the excel input sheet, its for search only with Containes() method
        /// </summary>
          public string[] headersForContaines = {"אימייל","שם פרטי","משפחה",
                    "טלפון נייד","תאריך לידה","פחות שנת הלידה"
                    ,"מגדר","קטגורית משקל.( לפי קטגוריות משקל IBJJF)","דרגת חגורה לפי IBJJF בלבד"
                    ,"במידה ולא ימצא מתחרה בקטגורית משקל",
                    "במידה ולא ימצא מתחרה מאותה דרגת חגורה","שם אקדמיה","שם מאמן"
                    ,"מספר טלפון נייד של המאמן","במידה ולא ימצא מתחרה מאותה קטגורית גיל","תעודת זהות"
                ,"אם האקדמיה אינה מופיעה ברשימה","משקל אמיתי כולל חליפה"
            };



        /// <summary>
        /// array is for promting the user what header is missing in the input this array and headersForContaines indexes must be equal and 
        /// matched by the containes string and the real (original) expected header name
        /// </summary>
        string[] originalHeaders = { "כתובת אימייל",
                "שם פרטי. ", "שם משפחה.", "טלפון נייד.",
                "תאריך לידה.", "קטגורית גיל (2017 פחות שנת הלידה)",
                "מגדר.", "קטגורית משקל.( לפי קטגוריות משקל IBJJF)",
                "דרגת חגורה לפי IBJJF בלבד.",
                "במידה ולא ימצא מתחרה בקטגורית משקל -מעונין\\ת להתחרות בקטגורית משקל אחת גבוהה יותר.",
                "במידה ולא ימצא מתחרה מאותה דרגת חגורה- מעונין\\ת להתחרות בדרגת חגורה אחת גבוהה יותר.", "שם אקדמיה.",
                "שם מאמן\\ת", "מספר טלפון נייד של המאמן.", "במידה ולא ימצא מתחרה מאותה קטגורית גיל - מעונין\\ת להתחרות בקטגורית גיל אחת גבוהה יותר."
               ,"תעודת זהות","אם האקדמיה אינה מופיעה ברשימה - יש לרשום אותה"
                ,"משקל אמיתי כולל חליפה ( שקילה לפי חוקי IBJJF)","אם בחרת נקבה האם ניתן לשבץ בקטגוריה מעורבת?"
        };
        
       
    }
}
