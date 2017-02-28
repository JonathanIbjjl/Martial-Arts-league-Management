using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace DragDrop
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
          
        }

        private void flowLayoutPanel2_MouseEnter(object sender, EventArgs e)
        {
            Control c = sender as Control;

            c.DoDragDrop(c, DragDropEffects.Move);
        }

        private void flowLayoutPanel2_DragEnter(object sender, DragEventArgs e)
        {
            e.Effect = DragDropEffects.All;
        }

        private void flowLayoutPanel1_DragOver(object sender, DragEventArgs e)
        {
            e.Effect = DragDropEffects.All;
        }

        private void flowLayoutPanel1_DragDrop(object sender, DragEventArgs e)
        {
            Control c = e.Data.GetData(e.Data.GetFormats()[0]) as Control;
            ((Control)sender).Controls.Add(c.Parent);
        }
    }
}
