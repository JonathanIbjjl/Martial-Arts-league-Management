using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace WindowsFormsApplication4
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

        class Test
        {
            private static List<string> t = new List<string>();

            private static List<Test> ba = new List<Test>();

            public void Init()
            {
                t.Add("AAA");
                t.Add("BBB");
                ba.Add(this);
                t.Add("CCC");
                t.Add("DDD");
                ba.Add(this);
                t.Add("EEE");
                t.Add("VVV");
                ba.Add(this);



            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            Test t = new Test();
            t.Init();
        }
    }
}
