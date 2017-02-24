﻿using System;
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

            s.Append("צירופים אפשריים");
            s.Append(Environment.NewLine);

            if (Contender.PbListArchive.Count <= 0)
            {
                s.Append("אין צירופים אפשרים עבור מתחרה זה!");
            }

            else
            {
                foreach (Contenders.Contender.PotentialBrackets pb in Contender.PbListArchive)
                {

                    s.Append(pb.Score);
                    s.Append(Environment.NewLine);
                }
            }



            string con = s.ToString();
            Martial_Arts_league_Management2.PromtForm promt = new Martial_Arts_league_Management2.PromtForm(con, true, "פרטי מתחרה", true, "סגור");
            promt.Show();

        }
    }
}
