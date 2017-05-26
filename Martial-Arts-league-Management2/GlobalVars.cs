using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MartialArts
{

  public  class GlobalVars
    {
       
        public  const string VerNum = "Beta 3.0";

        private static MartialArts.ProjectsSavedAsBinaryFiles _CurrentProject;
        public static  MartialArts.ProjectsSavedAsBinaryFiles CurrentProject
        {
            get { return _CurrentProject; }
            set
            {
                _CurrentProject = value;
                
             
            }
        }



        public static bool IsLoading;
        public static int NumOfContendersInLeuge
        {
            get
            {
              return  ListOfContenders.Count();
            }
        }

        private static List<Contenders.Contender> _ListOfContenders;
        public static List<Contenders.Contender> ListOfContenders
        {
            get
            {
                if (_ListOfContenders == null)
                {
                    _ListOfContenders = new List<Contenders.Contender>();
                    return _ListOfContenders;
                }

                else
                {
                    return _ListOfContenders;
                }
            }

            set
            {
                _ListOfContenders = value;
            }
        }

     public enum GenderEnum
        {
            Woman = 0,
            Man=1,
            Mixed = 2
        }

        public static string GEtGenderStringByEnum(GenderEnum g)
        {
            switch (g)
            {
                case GenderEnum.Woman:
                    return "בנות";
                case GenderEnum.Man:
                    return "בנים";
                default:
                    return "מעורב";
            }
        }

        #region "System Colors"
        public static System.Drawing.Color Sys_Yellow
        {
            get
            {
                return System.Drawing.Color.FromArgb(227, 154, 44);
            }
        }

        public static System.Drawing.Color Sys_Red
        {
            get
            {
                return System.Drawing.Color.Maroon;
            }
        }

        public static System.Drawing.Color Sys_DarkerGray
        {
            get
            {
                return System.Drawing.Color.FromArgb(28,28,28);
            }
        }

        public static System.Drawing.Color Sys_LighterGray
        {
            get
            {
                return System.Drawing.Color.FromArgb(48, 48, 48);
            }
        }

        public static System.Drawing.Color Sys_White
        {
            get
            {
                return System.Drawing.Color.FromArgb(250, 250, 250);
            }
        }

        public static System.Drawing.Color Sys_LabelGray
        {
            get
            {
                return System.Drawing.Color.FromArgb(200, 200, 200);
            }
        }
        #endregion
      public static System.Drawing.Font BaseSystemFont
        {
            get
            {
                return new System.Drawing.Font("ARIAL", 9, System.Drawing.FontStyle.Regular);
            }
        }

        /*
         this propery will determine the weight category of new event from system list or excel file
         this property will be passed to the constructor of the Contender instance that created and from their to 
         the base class of contender (ContenderGeneral class) the constructor in ContenderGeneral will set the
         static property WeightCatEnum.

         the options to change this propert is only:
         1. when creating a new system list.
         2. when loading excel.
         3. when loading saved data.
        */
        private static Contenders.WeightCategiries.WeightCatEnum _ChoosenWeightCategory = Contenders.WeightCategiries.WeightCatEnum.IBJJL; // default is basic
        public static Contenders.WeightCategiries.WeightCatEnum ChoosenWeightCategory
        {
            get { return _ChoosenWeightCategory;}
            set
            {
                _ChoosenWeightCategory = value;
                Contenders.ContndersGeneral.WeightCatEnum = value;
                Contenders.ContndersGeneral.SetWeightDictionariesToNull();
            }
        }
    }
}
