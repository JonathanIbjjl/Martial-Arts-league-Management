
using MartialArts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Visual
{
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

            if (Contender.PbListArchive.Count <= 0)
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
            Martial_Arts_league_Management2.PromtForm promt = new Martial_Arts_league_Management2.PromtForm(new System.Drawing.Size(600,600),con, true, "פרטי מתחרה", true, "סגור");
            promt.Show();

        }


        #region "Drag And Drop"
        private void Vcont_MouseEnter(object sender, EventArgs e)

        {
            System.Windows.Forms.Control c = sender as System.Windows.Forms.Control;

            c.DoDragDrop(c, System.Windows.Forms.DragDropEffects.Move);
        }

        private void Vcont_DragEnter(object sender, System.Windows.Forms.DragEventArgs e)
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
                    VisualLeagueEvent.CreateNewBracket(int.Parse(e.ClickedItem.Name.ToString().Trim()));
                }
            }
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
