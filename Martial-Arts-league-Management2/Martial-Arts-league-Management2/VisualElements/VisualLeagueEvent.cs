using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Visual
{
    class VisualLeagueEvent : VisualElements
    {
        private static List<VisualBracket> _VisualBracketsList;
        public static List<VisualBracket> VisualBracketsList
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

        private static List<VisualContender> _VisualUnplacedBracketsList;
        public static List<VisualContender> VisualUnplacedBracketsList
        {
            get
            {
                if (_VisualUnplacedBracketsList == null)
                {
                    _VisualUnplacedBracketsList = new List<VisualContender>();
                    return _VisualUnplacedBracketsList;
                }
                else
                {
                    return _VisualUnplacedBracketsList;
                }

            }
            set { _VisualUnplacedBracketsList = value; }
        }

        public static List<VisualContender> AllVisualContenders;
        public static bool IsMerged = false;

        public static MartialArts.Form1 FormObj;
        // used for copy contenders, the property will save the system id of the copied contender
        public static int ClipBoardValue { get; set; }

        public VisualLeagueEvent(MartialArts.Form1 formobj)
        {
            IsMerged = false;
        }

        public VisualLeagueEvent()
        {
            IsMerged = false;
        }

        public static void AddVisualBracket(VisualBracket vbracket)
        {
            VisualBracketsList.Add(vbracket);
        }

        public static void AddUnplacedContender(VisualContender cont)
        {
            VisualUnplacedBracketsList.Add(cont);
        }


        public static void RemoveVisualBracket()
        {

        }


        /// <summary>
        /// add contender to visual bracket or to unplaced list
        /// </summary>
        /// <param name="sysid"></param>
        /// <param name="b">if null contender is moving to uplaced list</param>
        public static void AddContender(int sysid, VisualBracket b = null)
        {
            var VisualContender = AllVisualContenders.AsEnumerable().Where(x => x.Contender.SystemID == sysid).Single();
            // first remove from current bracket if contender is not from unplaced panel
            RemoveContender(sysid);
            // TODO: DISCOVER BUG: there is a very rare unknown bug, probably somthing with the drag and drop,
            // the bug creates contender that exist twice, in the GUI the user dont see it, but in the lists he exist twice
            // very rare bug and its hard to dicover it
            // the if steatment should solve it as a quick fix
            if (VisualBracketsList.SelectMany(x => x.Bracket.ContendersList).Any(x => x.SystemID == sysid) == true)
            {
                return;
            }

            if (b == null)
            {
                // add to unplaced list
                VisualUnplacedBracketsList.Add(VisualContender);
            }
            else
            {
                // moving to a bracket
                b.VisualCont.Add(VisualContender);
                b.Bracket.ContendersList.Add(VisualContender.Contender);
                // refresh the new bracket
                if (b.Refresh() != null)
                {
                    // the user moved the last contender from the bracket, bracket will be removed
                    VisualBracketsList.Remove(b.Refresh());
                }
            }

            // update GUI Clocks
            FormObj.UpdateClocks();
        }
    

        public static void RemoveContender(int sysid)
        {

            // first check if contender came from unplaced contemders, if so remove
            if (VisualUnplacedBracketsList.Any(x => x.SystemID == sysid) == true)
            {
                var cont = VisualUnplacedBracketsList.Where(x => x.SystemID == sysid).Select(c => c).Single();
                // remove
                VisualUnplacedBracketsList.Remove(cont);
                return;
            }

            // the contender exist in one of the visual brackets
            bool isRemoved = false;
            // extract Current Bracket 
            foreach (VisualBracket vb in VisualBracketsList)
            {
                foreach (VisualContender c in vb.VisualCont)
                {
                    if (c.Contender.SystemID == sysid)
                    {
                        vb.VisualCont.Remove(c);
                        isRemoved = true;
                        break;
                    }
                }

                if (isRemoved == true)
                    break;
            }

             isRemoved = false;

            // extract Current Bracket 
            foreach (VisualBracket vb in VisualBracketsList)
            {
                foreach (Contenders.Contender c in vb.Bracket.ContendersList)
                {
                    if (c.SystemID == sysid)
                    {
                        vb.Bracket.ContendersList.Remove(c);
                        isRemoved = true;
                        // refresh bracket
                        if (vb.Refresh() != null)
                        {
                            // the user moved the last contender from the bracket, bracket will be removed
                            VisualBracketsList.Remove(vb.Refresh());
                        }
                        break;
                    }
                }

                if (isRemoved == true)
                    break;
            }
        }

        internal static void CreateNewBracket(int ContID)
        {
            

            // first check unplacedList to remove
            if (VisualUnplacedBracketsList.Any(x => x.SystemID == ContID) == true)
            {
                var cont = VisualUnplacedBracketsList.Where(x => x.SystemID == ContID).Select(c => c).Single();
                // remove from unplaced area
                VisualUnplacedBracketsList.Remove(cont);
            }
            else
            {
                // remove from Bracket area
                RemoveContender(ContID);
            }

            // the old visual contender will be disposd in order to remove from GUI
            var NewBracketVisualContToDispose = AllVisualContenders.Where(x => x.SystemID == ContID).Select(c => c).Single();
            // new visual contender will be created for the new bracket
            var NewBracketCont = AllVisualContenders.Where(x => x.SystemID == ContID).Select(c => c.Contender).Single();

            // remove from AllvisualContenders list (will be created again later)
            AllVisualContenders.Remove(NewBracketVisualContToDispose);
            // dispose old visual contender and he will dissapear from his old place in GUI
            NewBracketVisualContToDispose.Vcontender.Dispose();
            NewBracketVisualContToDispose = null;

            // create a new bracket
            MartialArts.Bracket newBracket = new MartialArts.Bracket(NewBracketCont.AgeCategory, NewBracketCont.Belt, NewBracketCont.WeightCategory);
            newBracket.AddSingleContender(NewBracketCont);
            // create a new visual bracket
            VisualBracket newVisualBracket = new VisualBracket(newBracket);
            newVisualBracket.Init();
            // add to bracket list
            VisualBracketsList.Add(newVisualBracket);
            // add to panel
            FormObj.BracktsFPanel.Controls.Add(newVisualBracket.Vbracket);
            // add the"new" visual contender to AllContenderList (he was removed) in order to find him in searches
            AllVisualContenders.Add(newVisualBracket.VisualCont[0]);
            // scroll to end of the panel to show the user the new bracket, first wait for weak graphic cards
            System.Threading.Thread.Sleep(50);
            FormObj.BracktsFPanel.VerticalScroll.Value = FormObj.BracktsFPanel.VerticalScroll.Maximum;
            // refresh clocks
            FormObj.UpdateClocks();
        }

        public static void MergeListsForSearch()
        {

            if (AllVisualContenders == null)
                AllVisualContenders = new List<VisualContender>();

            IsMerged = true;
            AllVisualContenders = VisualBracketsList.SelectMany(x => x.VisualCont).Select(y => y).ToList();
            // add the unplaced
            foreach (VisualContender c in VisualUnplacedBracketsList)
            {
                AllVisualContenders.Add(c);
            }
        }

        public static bool IsShadowd;
        /// <summary>
        /// returns the Y location of the visual contender
        /// </summary>
        /// <returns></returns>
        public static int Search(string searchString,out int NumOfResults)
        {
            if (IsMerged == false)
                throw new Exception("MergeListsForSearch() method must be initilized in order to perform a search");

            NumOfResults = 0;
            if (IsShadowd == true)
            {
                // the user made a second search immideatly so first every controls must be visible
                CancelAllShadowsOfSearch();
            }
            var fix = searchString.Trim();
            bool found = false;
     
                foreach (VisualContender c in AllVisualContenders)
                {
                    if (c.MakeShadow(fix) == true)
                    {
                        found = true;
                        IsShadowd = true;
                        NumOfResults++;
                    }
                }
            

            // if nothing was found make all controls visible
            if (found == false)
            {
                CancelAllShadowsOfSearch();
            }

            return 0;
        }

        public static void CancelAllShadowsOfSearch()
        {
          
                foreach (VisualContender c in AllVisualContenders)
                {
                    c.CancelShadow();
                }
            

            IsShadowd = false;
        }

        public static bool IsSutibleForBracket(VisualContender v, VisualBracket b)
                 {

                   bool IsSuitable = false;

                    if (Math.Floor(v.Contender.Grade) == Math.Floor(b.Bracket.AverageGrade) && MartialArts.BracketsBuilder.ReturnBinaryGender(b.Bracket) == v.Contender.IsMale)
                    {
                        IsSuitable = true;
                    }
                    else if (Math.Floor(v.Contender.Score_WeightFactor) == Math.Floor(b.Bracket.AverageGrade) && MartialArts.BracketsBuilder.ReturnBinaryGender(b.Bracket) == v.Contender.IsMale)
                    {
                        IsSuitable = true;
                    }

                    else if (Math.Floor(v.Contender.Score_BeltFactor) == Math.Floor(b.Bracket.AverageGrade) && MartialArts.BracketsBuilder.ReturnBinaryGender(b.Bracket) == v.Contender.IsMale)
                    {
                      IsSuitable = true;
                    }
                    else if (Math.Floor(v.Contender.Score_AgeFactor) == Math.Floor(b.Bracket.AverageGrade) && v.Contender.IsChild == true && MartialArts.BracketsBuilder.ReturnBinaryGender(b.Bracket) == v.Contender.IsMale)
                    {
                      IsSuitable = true;
                    }
                    else if (Math.Floor(v.Contender.Score_Weight_Belt_Factor) == Math.Floor(b.Bracket.AverageGrade) && MartialArts.BracketsBuilder.ReturnBinaryGender(b.Bracket) == v.Contender.IsMale)
                    {
                      IsSuitable = true;
                    }
                    else if (Math.Floor(v.Contender.Score_Weight_Age_Factor) == Math.Floor(b.Bracket.AverageGrade) && v.Contender.IsChild == true && MartialArts.BracketsBuilder.ReturnBinaryGender(b.Bracket) == v.Contender.IsMale)
                    {
                      IsSuitable = true;
                    }
                    else if (Math.Floor(v.Contender.Score_AllFactors) == Math.Floor(b.Bracket.AverageGrade) && v.Contender.IsChild == true && MartialArts.BracketsBuilder.ReturnBinaryGender(b.Bracket) == v.Contender.IsMale)
                    {
                       IsSuitable = true;
                    }

            return IsSuitable;
        }


            

        public static void ClearClass()
        {

            if (VisualBracketsList != null)
            {
                VisualBracketsList.Clear();
                VisualBracketsList = null;
            }

            if (VisualUnplacedBracketsList != null)
            {
                VisualUnplacedBracketsList.Clear();
                VisualUnplacedBracketsList = null;
            }

            if (AllVisualContenders != null)
            {
                AllVisualContenders.Clear();
                AllVisualContenders = null;
            }
         }


    }
}
