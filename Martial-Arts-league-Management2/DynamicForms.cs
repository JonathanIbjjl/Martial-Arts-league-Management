using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace DynamicForms
{
   abstract class DynamicForms
    {
       protected Form form = null;
       protected FlowLayoutPanel flowPanel = null;
    }



    class ChooseExSheetForm : DynamicForms
    {

        string[] SheetNames = null;

        private string _ChoosenWsName;
        public string ChoosenWsName
        {
            get
            {
                return _ChoosenWsName;
            }
        }

        public ChooseExSheetForm(string[] sheetnames)
        {
            this.SheetNames = sheetnames;
            
        }

        public void showForm()
        {
            CreateForm();
            if (form != null)
            {
                form.ShowDialog();
            }
        }

        private void CreateForm()
        {
            var lblMsgHight = 40;
            form = new Form();
            form.Size = new Size(205, CalculateFormHeight() +lblMsgHight); // last digit is for the messege label
            form.StartPosition = FormStartPosition.CenterScreen;
            form.MaximizeBox = false;
            form.FormBorderStyle = FormBorderStyle.Fixed3D;
            form.BackColor = MartialArts.GlobalVars.Sys_Yellow;
            form.FormBorderStyle = FormBorderStyle.None;

            // messege label
            Label lblmsg = new Label();
            lblmsg.Size = new Size(205, lblMsgHight);
            lblmsg.Location = new Point(5, 0);
            lblmsg.RightToLeft = RightToLeft.Yes;
            lblmsg.TextAlign = ContentAlignment.MiddleCenter;
            lblmsg.Text = "נמצאו גיליונות מרובים בקובץ. אנא בחר גיליון:";
            lblmsg.BackColor = MartialArts.GlobalVars.Sys_Yellow;
            lblmsg.ForeColor = MartialArts.GlobalVars.Sys_DarkerGray;
            form.Controls.Add(lblmsg);

            flowPanel = new FlowLayoutPanel();
            flowPanel.Size = new Size(200, CalculateFormHeight()-(form.Height - CalculateFormHeight())); // decreaced space label
            flowPanel.Location = new Point(0, form.Height - CalculateFormHeight()); // y pos is after the messege label
            form.Controls.Add(flowPanel);

            // add buttons with sheet names
            foreach (string s in SheetNames)
            {
                CustomButton cb = new CustomButton(200, 25, s);
                cb.button.Click += new EventHandler(cb_Click);

                flowPanel.Controls.Add(cb.button);
         
            }
        }

        private void cb_Click(object sender, EventArgs e)
        {
            // set ws name property
            _ChoosenWsName = ((Button)sender).Text;
            // exit form
            form.Close();
        }

      
        private int CalculateFormHeight()
        {
            var result = 25;
            foreach (string s in SheetNames)
            {
                result += 25;
            }
            // add margin of buttons
            result = result + (SheetNames.Length +1) * 8;
            return result+5;
        }
    }

    class CustomButton
    {
        public Button button = null;
        public CustomButton(int width, int height,string txt)
        {
            button = new Button();
            button.Size = new Size(width, height);
            button.BackColor = MartialArts.GlobalVars.Sys_DarkerGray;
            button.ForeColor = MartialArts.GlobalVars.Sys_White;
            button.TextAlign = ContentAlignment.MiddleCenter;
            button.FlatStyle = FlatStyle.Popup;
            button.Cursor = Cursors.Hand;
            button.Text = txt;
        }
            
    }

}
