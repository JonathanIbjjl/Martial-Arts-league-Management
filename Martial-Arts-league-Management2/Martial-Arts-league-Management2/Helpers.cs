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

        public static Icon getIcon(string ImageFileName)
        {
            System.Reflection.Assembly myAssembly = System.Reflection.Assembly.GetExecutingAssembly();
            using (Stream myStream = myAssembly.GetManifestResourceStream("Martial-Arts-league-Management2." + ImageFileName))
            {
                Icon image = new Icon(myStream);
                return image;
            }
        }

    }
}
