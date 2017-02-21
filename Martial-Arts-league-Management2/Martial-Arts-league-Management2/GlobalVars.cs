using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MartialArts
{
    class GlobalVars
    {
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

       
    }
}
