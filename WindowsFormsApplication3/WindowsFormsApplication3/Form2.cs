using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace WindowsFormsApplication3
{
    public partial class Form2 : Form
    {
        public Form2()
        {
            InitializeComponent();
        }

        private void Form2_Load(object sender, EventArgs e)
        {
            // Basic SplitContainer properties.
            // This is a horizontal splitter whose top and bottom panels are ListView controls. The top panel is fixed.
            splitContainer1.Dock = System.Windows.Forms.DockStyle.Fill;
            // The top panel remains the same size when the form is resized.
            splitContainer1.FixedPanel = System.Windows.Forms.FixedPanel.Panel1;
            splitContainer1.Location = new System.Drawing.Point(0, 0);
            splitContainer1.Name = "splitContainer2";
            // Create the horizontal splitter.
            splitContainer1.Orientation = System.Windows.Forms.Orientation.Horizontal;
            splitContainer1.Size = new System.Drawing.Size(207, 273);
            splitContainer1.SplitterDistance = 125;
            splitContainer1.SplitterWidth = 6;
            // splitContainer2 is the third control in the tab order.
            splitContainer1.TabIndex = 2;
            splitContainer1.Text = "splitContainer2";
        }
    }
}
