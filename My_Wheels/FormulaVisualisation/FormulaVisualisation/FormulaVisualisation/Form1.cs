using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using RPN;

namespace FormulaVisualisation
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();

        }
        string input = "";
        Bitmap bit;
        Graphics g;
        private void button1_Click(object sender, EventArgs e)
        {
            input = textBox1.Text;
            RealEquasion output = new RealEquasion(input);
            label1.Text = output.ToString();
            pictureBox1.Image = output.ShowFunction(20);//not working yet,..
        }   
    }
}
