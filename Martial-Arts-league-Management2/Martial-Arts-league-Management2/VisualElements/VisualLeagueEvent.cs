using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Visual
{
    class VisualLeagueEvent: VisualElements
    {
      private  List<VisualBracket> _VisualBracketsList;
      public  List<VisualBracket> VisualBracketsList
        {
            get
            {
                if (_VisualBracketsList == null)
                {
                    _VisualBracketsList = new List<VisualBracket>();
                    return _VisualBracketsList;
                }
                else
                {
                    return _VisualBracketsList;
                }

            }
            set { _VisualBracketsList = value; }
        }

     private   List<Contenders.Contender> _VisualUnplacedBracketsList;
      public  List<Contenders.Contender> VisualUnplacedBracketsList
        {
            get
            {
                if (_VisualUnplacedBracketsList == null)
                {
                    _VisualUnplacedBracketsList = new List<Contenders.Contender>();
                    return _VisualUnplacedBracketsList;
                }
                else
                {
                    return _VisualUnplacedBracketsList;
                }

            }
            set { _VisualUnplacedBracketsList = value; }
        }


        public VisualLeagueEvent()
        {

        }

        public void AddVisualBracket(VisualBracket vbracket)
        {
            VisualBracketsList.Add(vbracket);
        }

        public void AddUnplacedContender(Contenders.Contender cont)
        {
            VisualUnplacedBracketsList.Add(cont);
        }


        public void RemoveVisualBracket()
        {

        }

        public void AddContender()
        {

        }

        public void RemoveContender()
        {

        }

        /// <summary>
        /// returns the Y location of the visual contender
        /// </summary>
        /// <returns></returns>
        public int Search()
        {
            return 0;
        }
    }
}
