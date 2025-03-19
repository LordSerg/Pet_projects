using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace LAB4
{
    public partial class Form2 : Form
    {
        public Form2()
        {
            InitializeComponent();
            while (xyTableWithLabels1.n > 2)
                xyTableWithLabels1.DeleteCol();
        }

        public (double, double)[] intervals;
        public int NumOfData;
        public int NumOfCriterias;
        public int NumOfKlusters;
        public bool is_ghauss = false;
        public bool is_make_file = false;
        public bool is_accept_data = true;

        private void button1_Click(object sender, EventArgs e)
        {
            int n = (int)numericUpDown2.Value;
            NumOfData = (int)numericUpDown1.Value;
            NumOfCriterias = (int)numericUpDown2.Value;
            NumOfKlusters = (int)numericUpDown3.Value;
            intervals = new (double, double)[n];
            for (int i = 0; i < n; i++)
            {
                intervals[i] = (double.Parse(xyTableWithLabels1.TextBoxes[i * 2].Text),
                                double.Parse(xyTableWithLabels1.TextBoxes[i * 2 + 1].Text));
            }
            is_accept_data = true;
            is_make_file = checkBox1.Checked;
            this.Close();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            is_accept_data = false;
            this.Close();
        }

        private void radioButton1_CheckedChanged(object sender, EventArgs e)
        {
            is_ghauss = radioButton2.Checked;
        }

        private void radioButton2_CheckedChanged(object sender, EventArgs e)
        {
            is_ghauss = radioButton2.Checked;
        }

        private void numericUpDown2_ValueChanged(object sender, EventArgs e)
        {
            while (xyTableWithLabels1.n > numericUpDown2.Value)
                xyTableWithLabels1.DeleteCol();
            while (xyTableWithLabels1.n < numericUpDown2.Value)
                xyTableWithLabels1.AddCol();
        }
    }
}
