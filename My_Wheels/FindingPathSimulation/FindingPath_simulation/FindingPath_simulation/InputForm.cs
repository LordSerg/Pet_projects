using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace FindingPath_simulation
{
    public partial class InputForm : Form
    {
        public InputForm()
        {
            InitializeComponent();
            textBox1.Focus();
        }
        public float Val;
        private void button1_Click(object sender, EventArgs e)
        {
            setValue();
        }

        private void InputForm_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                setValue();
            }
        }

        private void setValue()
        {
            string text = textBox1.Text;
            text = text.Replace(",", ".");
            float.TryParse(text, out Val);
            this.Close();
        }
    }
}
