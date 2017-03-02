using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace MartialArts
{
    class CreateContendersFromDgv: IDisposable
    {
        DataGridView dgvMain;
        public CreateContendersFromDgv(ref DataGridView Dgv)
        {
            this.dgvMain = Dgv;
        }

        public bool Init()
        {

            if (dgvMain.Rows.Count < 2)
            {
                Helpers.ShowGenericPromtForm("חייבות להיות לפחות 2 שורות ברשימה");
                return false;
            }
            // check if datagridview has errors
            if (HasErrorText() == true)
            {
                Helpers.ShowGenericPromtForm("השדות ברשימה אינם תקינים, מעבר עם העכבר על הסימון האדום בצד ימין יציג את הנתון החסר");
                return false;
            }

           
            GlobalVars.ListOfContenders = new List<Contenders.Contender>();

            // itirate and add contenders to list
            for (int i = 0; i < dgvMain.Rows.Count; i++)
            {
                Contenders.Contender con = new Contenders.Contender();

                con.FirstName = dgvMain.Rows[i].Cells[1].Value.ToString();
                con.LastName = dgvMain.Rows[i].Cells[2].Value.ToString();
                con.ID = dgvMain.Rows[i].Cells[0].Value.ToString();
                con.Email = dgvMain.Rows[i].Cells[7].Value.ToString();
                con.PhoneNumber = dgvMain.Rows[i].Cells[8].Value.ToString();
                con.DateOfBirth = "28/10/1980";
                con.AgeCategory = Contenders.ContndersGeneral.AgeGrades[dgvMain.Rows[i].Cells[6].Value.ToString()];
                con.IsMale = (dgvMain.Rows[i].Cells[12].Value.ToString() == "זכר") ? true : false;
                double r = 0;
                Double.TryParse(dgvMain.Rows[i].Cells[5].Value.ToString(), out r);
                con.Weight = r;
                con.IsChild = MartialArts.ExcelOperations.IsChild(con.AgeCategory);
                if (con.IsChild)
                    con.WeightCategory = Contenders.ContndersGeneral.ChildWeightCat[dgvMain.Rows[i].Cells[4].Value.ToString()];
                else
                    con.WeightCategory = Contenders.ContndersGeneral.AdultWeightCat[dgvMain.Rows[i].Cells[4].Value.ToString()];

                con.Belt = Contenders.ContndersGeneral.GetBelt(dgvMain.Rows[i].Cells[3].Value.ToString());
                con.AcademyName = dgvMain.Rows[i].Cells[9].Value.ToString();
                con.CoachName = dgvMain.Rows[i].Cells[10].Value.ToString();
                con.CoachPhone = dgvMain.Rows[i].Cells[11].Value.ToString();
                con.IsAllowedWeightGradeAbove = Convert.ToBoolean(dgvMain.Rows[i].Cells[16].Value);

                con.IsAllowedAgeGradeAbove = Convert.ToBoolean(dgvMain.Rows[i].Cells[14].Value);
                con.IsAllowedBeltGradeAbove = Convert.ToBoolean(dgvMain.Rows[i].Cells[15].Value);
                con.IsAllowedVersusMan = Convert.ToBoolean(dgvMain.Rows[i].Cells[13].Value);
                GlobalVars.ListOfContenders.Add(con);
            }

            return true;
        }


        public bool HasErrorText()
        {
            bool hasErrorText = false;
            //replace this.dataGridView1 with the name of your datagridview control
            foreach (DataGridViewRow row in this.dgvMain.Rows)
            {

                if (row.ErrorText.Length > 0)
                {
                    hasErrorText = true;
                    break;
                }

            }

            return hasErrorText;
        }

        public void Dispose()
        {
          
        }
    }
}
