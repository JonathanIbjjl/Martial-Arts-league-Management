
using MartialArts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Visual
{
    [Serializable]
    partial class VisualContender
    {
        // event for Data button
        private void ShowContData_Click(object sender, EventArgs e)
        {
            string line = "___________________________________";

            // builed string
            StringBuilder s = new StringBuilder();
            s.Append("פרטים אישיים נוספים");
            s.Append(Environment.NewLine);
            s.Append(line);
            s.Append(Environment.NewLine);
            s.Append("מתחרה: " + Contender.FirstName + " " + Contender.LastName);
            s.Append(Environment.NewLine);
            s.Append("אימייל: " + Contender.Email);
            s.Append(Environment.NewLine);
            s.Append("טלפון: " + Contender.PhoneNumber);
            s.Append(Environment.NewLine);
            s.Append("תאריך לידה: " + Contender.DateOfBirth);
            s.Append(Environment.NewLine);
            s.Append("שם מאמן: " + Contender.CoachName);
            s.Append(Environment.NewLine);
            s.Append("טלפון מאמן: " + Contender.CoachPhone);
            s.Append(Environment.NewLine);
            s.Append(line);
            s.Append(Environment.NewLine);
            s.Append(Environment.NewLine);

            s.Append("##################################"
+ Environment.NewLine + "######### צירופים אפשריים ########"
+ Environment.NewLine + "##################################");

            s.Append(Environment.NewLine);
            s.Append(Environment.NewLine);

            if (Contender.CreatedAfterBracketBuilder == true)
            {
                s.Append("מתחרה זה נוצר לאחר בניית הבתים ולכן לא קיימת עבורו סטטיסטיקה, במידה ותיצור את הבתים מחדש תחושב עבורו סטטיסטיקה ואז יתאפשר להציג עבורו מתחרים אפשריים");
            }
            else if (Contender.PbListArchive.Count <= 0)
            {
                s.Append("אין צירופים אפשרים עבור מתחרה זה!");
            }

            else
            {
                foreach (Contenders.Contender.PotentialBrackets pb in Contender.PbListArchive)
                {

                    foreach (KeyValuePair<int, double> scoreAndId in pb.IdAndScore)
                    {
                        var name = MartialArts.GlobalVars.ListOfContenders.Where(x => x.SystemID == scoreAndId.Key).Select(z => z.SystemID + " " + z.FirstName + " " + z.LastName).FirstOrDefault();
                        s.Append(name + " " + Contenders.ContndersGeneral.GetFactorExplanation(scoreAndId.Value));
                        s.Append(Environment.NewLine);
                    }
                    s.Append(line);
                    s.Append(Environment.NewLine);
                    s.Append(Environment.NewLine);
                }
            }



            string con = s.ToString();
            Martial_Arts_league_Management2.PromtForm promt = new Martial_Arts_league_Management2.PromtForm(new System.Drawing.Size(600, 600), con, true, "פרטי מתחרה", true, "סגור");
            promt.Show();

        }


        #region "Drag And Drop"



        private void Vcont_MouseDown(object sender, System.Windows.Forms.MouseEventArgs e)

        {
            if (e.Button == System.Windows.Forms.MouseButtons.Left)
            {
                System.Windows.Forms.Control c = sender as System.Windows.Forms.Control;
                c.Invoke(new Action<System.Windows.Forms.Control>(DoEffect), c);
            }
        }

        private void DoEffect(System.Windows.Forms.Control c)
        {
            c.DoDragDrop(c, System.Windows.Forms.DragDropEffects.Copy);
        }

        private void Vcont_DragEnter(object sender, System.Windows.Forms.DragEventArgs e)
        {
            System.Windows.Forms.Control c = sender as System.Windows.Forms.Control;
            c.Invoke(new Action<System.Windows.Forms.DragEventArgs>(DoEffect), e);
        }

        private void DoEffect(System.Windows.Forms.DragEventArgs e)
        {
            e.Effect = System.Windows.Forms.DragDropEffects.All;
        }

        void contexMenuuu_ItemClicked(object sender, System.Windows.Forms.ToolStripItemClickedEventArgs e)
        {
            System.Windows.Forms.ToolStripItem item = e.ClickedItem;

            if (item.Text == "העתק מתחרה")
            {
                System.Windows.Forms.ContextMenuStrip c = sender as System.Windows.Forms.ContextMenuStrip;
                if (e.ClickedItem.Name.IsNumeric() == true)
                {
                    // save the contender id that will be pasted
                    VisualLeagueEvent.ClipBoardValue = int.Parse(e.ClickedItem.Name);
                }
            }

            else if (item.Text == "צור בית חדש")
            {
                if (e.ClickedItem.Name.ToString().Trim().IsNumeric() == true)
                {

                    // promt the user to permit
                    using (var promt = new Martial_Arts_league_Management2.PromtForm("המתחרה יימחק מהבית הנוכחי ויפתח בית חדש עם כל המתחרים המסומנים " + Environment.NewLine + "אנא אשר על מנת להמשיך"))
                    {
                        var result = promt.ShowDialog();
                        if (result == System.Windows.Forms.DialogResult.No)
                        {
                            return;
                        }
                    }

                 var b = VisualLeagueEvent.CreateNewBracket(int.Parse(e.ClickedItem.Name.ToString().Trim()));
                    // add marked contenders
                    foreach (Visual.VisualContender c in VisualLeagueEvent.AllVisualContenders)
                    {
                        if (c.IsMarked)
                        {
                            c.IsMarked = false;
                            b.Vbracket.Controls.Add(c.Vcontender);
                            VisualLeagueEvent.AddContender(c.SystemID, b);
                        }
                    }
                }
            }

            else if (item.Text == "בדוק פקטור בבית")
            {
                if (e.ClickedItem.Name.ToString().Trim().IsNumeric() == true)
                {

                    FactorCheck(int.Parse(e.ClickedItem.Name));

                }
            }

            else if (item.Text == "סמן")
            {
                if (e.ClickedItem.Name.ToString().Trim().IsNumeric() == true)
                {

                    VisualLeagueEvent.Mark(int.Parse(e.ClickedItem.Name.ToString().Trim()),true);

                }
            }

            else if (item.Text == "הסר סימון")
            {
                if (e.ClickedItem.Name.ToString().Trim().IsNumeric() == true)
                {

                    VisualLeagueEvent.Mark(int.Parse(e.ClickedItem.Name.ToString().Trim()),false);

                }
            }

        }


        private void FactorCheck(int contID)
        {
            // extract contender
            var cont = VisualLeagueEvent.AllVisualContenders.Where(x => x.SystemID == contID).Select(c => c).Single();
            // extract visual bracket number
            var vbNum = VisualLeagueEvent.GetVisualBracketNumtByVisualContender(contID);
            if (vbNum == -1)
            {
                // the contender is uplaced in no bracket
                Helpers.ShowGenericPromtForm(cont.Contender.FirstName + " " + cont.Contender.LastName + " " + "עדיין לא שובץ בשום בית" + " לכן לא ניתן להציג פקטור");
                return;
            }

            // extract visual bracket
            var vb = VisualLeagueEvent.VisualBracketsList.Where(x => x.Bracket.BracketNumber == vbNum).Select(b => b).Single();
            // get the factor string
            string factor = Contenders.ContndersGeneral.GetFactorExplanation(cont, vb);
           
                if (factor == "")
                factor = "לא הופעל אף פקטור עבור " + cont.Contender.FirstName + " " + cont.Contender.LastName;

            // show the factor
            Helpers.ShowGenericPromtForm(factor);
        }

        #endregion

        private void tp_Draw(object sender, System.Windows.Forms.DrawToolTipEventArgs e)
        {
            e.DrawBackground();
            e.DrawBorder();
            e.DrawText();
        }


    }

}
