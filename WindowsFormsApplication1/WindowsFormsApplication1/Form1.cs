using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace WindowsFormsApplication1
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            //    Random Rand = new Random();
            //    long[] l = new long[500000000];
            //    for (long i = 0; i < 500000000; i++)
            //    {

            //        l[i] = Rand.Next(1, 6);
            //    }


            //    for (int c = 1; c <= 6; c++)
            //    {
            //        MessageBox.Show(c.ToString() + ": " + l.Where(x => x == c).Count());
            //    }


            //    MessageBox.Show("DONE");
            //}

            //private void label1_Click(object sender, EventArgs e)
            //{

            //}

            richTextBox1.Text = "<html><body><h1> adfgdfag</h1></body></html>";
        }

        private void richTextBox1_TextChanged(object sender, EventArgs e)
        {

        }
    }
}
