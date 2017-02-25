using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Diagnostics;
namespace WindowsFormsApplication3
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            Form2 f = new Form2();
            f.Show();
        }

        private void Func()
        {
            double a = 2007.03;
            double b = a - Math.Floor(a);
            double c = Math.Floor(a);
            Debug.WriteLine(b.ToString());
            Debug.WriteLine(a - (double)2007);

        }

    }
}
