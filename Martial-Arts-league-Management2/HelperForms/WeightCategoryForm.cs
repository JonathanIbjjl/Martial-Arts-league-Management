using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Martial_Arts_league_Management2.HelperForms
{
    public partial class WeightCategoryForm : Form
    {
        Dictionary<string, int> WeightsEnumDictionary = new Dictionary<string, int>();
        public string ChoosenWeight;
        private string CurrentCategory = "IBJJL";
        private bool IsChild = true;
        public WeightCategoryForm(string CurrentCategory,bool ischild)
        {
            InitializeComponent();
            LoadWeightDictionary();
            this.IsChild = ischild;
            this.CurrentCategory = CurrentCategory;
        }

        private void LoadWeightDictionary()
        {
            var values = Enum.GetValues(typeof(Contenders.WeightCategiries.WeightCatEnum));
            foreach (Contenders.WeightCategiries.WeightCatEnum s in values)
            {
                WeightsEnumDictionary.Add(s.ToString(), ((int)s));
            }
        }

        private void WeightCategoryForm_Load(object sender, EventArgs e)
        {
            LoadColorsAndDesign();
            LoadPickWeightList();
            LoadCurrentVars();
        }

        private void LoadCurrentVars()
        {
            // for safty check that category exist in enum dictionary
            if (WeightsEnumDictionary.ContainsKey(CurrentCategory))
            {
                lblCurrentCategory.Text = CurrentCategory;
                ListPickWeight.SelectedItem = CurrentCategory;
            }

            if (IsChild)
                radChild.Checked = true;
            else
                radAdult.Checked = true;
        }

        private void LoadColorsAndDesign()
        {
            this.BackColor = MartialArts.GlobalVars.Sys_Yellow;
            ListPickWeight.BackColor = MartialArts.GlobalVars.Sys_DarkerGray;
            ListShowWeight.BackColor = MartialArts.GlobalVars.Sys_DarkerGray;
            ListPickWeight.ForeColor = MartialArts.GlobalVars.Sys_LabelGray;
            ListShowWeight.ForeColor = MartialArts.GlobalVars.Sys_LabelGray;
            btnCancel.BackColor = MartialArts.GlobalVars.Sys_Red;
            btnCancel.ForeColor = MartialArts.GlobalVars.Sys_White;
            btnOK.BackColor = MartialArts.GlobalVars.Sys_Red;
            btnOK.ForeColor = MartialArts.GlobalVars.Sys_White;
            lblCurrentCategory.ForeColor = MartialArts.GlobalVars.Sys_Red;
            // font 
            this.Font = MartialArts.GlobalVars.BaseSystemFont;

        }

        private void LoadPickWeightList()
        {
            foreach (KeyValuePair<string, int> entry in WeightsEnumDictionary)
            {
                ListPickWeight.Items.Add(entry.Key);
            }
        }

        private void btnOK_Click(object sender, EventArgs e)
        {
            if (ListPickWeight.SelectedIndex == -1)
            {
                MartialArts.Helpers.ShowGenericPromtForm("אנא בחר קטגוריית משקל");
                return;
            }
            else
            {
                MartialArts.GlobalVars.ChoosenWeightCategory = SearchAndCatSelectedCategory();
                ChoosenWeight = ListPickWeight.SelectedItem.ToString();
                this.DialogResult = DialogResult.OK;
                this.Close();
            }
        }

        private Contenders.WeightCategiries.WeightCatEnum SearchAndCatSelectedCategory()
        {
            return (Contenders.WeightCategiries.WeightCatEnum)WeightsEnumDictionary[ListPickWeight.SelectedItem.ToString()]; 
        }
        private void ListPickWeight_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (ListPickWeight.SelectedIndex > -1)
            {
                LoadExampleWeight(SearchAndCatSelectedCategory());
            }
        }

        private void LoadExampleWeight(Contenders.WeightCategiries.WeightCatEnum cat)
        {
            ListShowWeight.Items.Clear();
            var d = Contenders.WeightCategiries.GetWeightCategory(cat, radChild.Checked);
            foreach (KeyValuePair<string, int> k in d)
            {
                ListShowWeight.Items.Add(k.Key);
            }          
        }

        private void radChild_CheckedChanged(object sender, EventArgs e)
        {
            if (ListPickWeight.SelectedIndex > -1)
            {
                LoadExampleWeight(SearchAndCatSelectedCategory());
            }
        }

        private void radAdult_CheckedChanged(object sender, EventArgs e)
        {
            if (ListPickWeight.SelectedIndex > -1)
            {
                LoadExampleWeight(SearchAndCatSelectedCategory());
            }
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.Cancel;
            this.Close();
        }

        private void ListShowWeight_SelectedIndexChanged(object sender, EventArgs e)
        {

        }
    }
}
