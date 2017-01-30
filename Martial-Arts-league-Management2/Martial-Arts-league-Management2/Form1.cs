﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Martial_Arts_league_Management2
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        public OpenFileDialog fd = null;
        private void button1_Click(object sender, EventArgs e)
        {
            fd = new OpenFileDialog();
            // show only excel files
            fd.Filter = "Excel Files|*.xls;*.xlsx;*.xlsm";

            if (fd.ShowDialog() == DialogResult.OK)
            {
                txtPath.Text = fd.FileName;
              
            }

        }

        private void btnLoadFile_Click(object sender, EventArgs e)
        {
            if (txtPath.Text != string.Empty)
            {
               LoadFile();
            }
        }

        private void LoadFile()
        {
            // excel instance
            ExcelOperations Eo = new ExcelOperations(txtPath.Text);
            Eo.GetContenders();
        }
    }
}
