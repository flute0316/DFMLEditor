using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace AGFRP
{
    public partial class RandomReadForm : Form
    {
        public int maxRepetition;
        public string indexResult;
        public RandomReadForm()
        {
            InitializeComponent();
            FullReadRadioButton.Checked = true;
        }
        private void SubmitButton_Click(object sender, EventArgs e)
        {
            if (FullReadRadioButton.Checked)
            {
                FullRead();
            }
            else if (RandomlyReadRadioButton.Checked)
            {
                RandomlyRead();
            }

        }

        private void RandomlyRead()
        {
            if (IndexBox.Text == string.Empty)
            {
                MessageBox.Show("Please enter the index of data you want to read", "Erro", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            else if (maxRepetition == -1 || int.Parse(IndexBox.Text) <= maxRepetition && int.Parse(IndexBox.Text) >= 0)
            {
                this.Close();
            }
            else if (int.Parse(IndexBox.Text) < 0)
            {
                MessageBox.Show("The input index cannot be less than 0", "Erro", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            else if (int.Parse(IndexBox.Text) > maxRepetition)
            {
                MessageBox.Show("The input index cannot be greater than the maxRepetition", "Erro", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            indexResult = IndexBox.Text;
        }
        
        private void Cancelbutton_Click(object sender, EventArgs e)
        {
            this.Close();
            indexResult = string.Empty;
        }

        private void FullRead()
        {
            this.Close();
            indexResult = "full read";
        }

        private void IndexBox_TextChanged(object sender, EventArgs e)
        {
            RandomlyReadRadioButton.Checked = true;
        }

        private void FullReadRadioButton_CheckedChanged(object sender, EventArgs e)
        {
            if (FullReadRadioButton.Checked == true)
            {
                IndexBox.Enabled = false;
            }
        }

        private void RandomlyReadRadioButton_CheckedChanged(object sender, EventArgs e)
        {
            if (RandomlyReadRadioButton.Checked == true)
            {
                IndexBox.Enabled = true;
            }
        }
    }
}
