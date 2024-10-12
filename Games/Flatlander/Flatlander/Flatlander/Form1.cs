using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Flatlander
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void button4_Click(object sender, EventArgs e)
        {//Exit
            Application.Exit();
        }

        private void button3_Click(object sender, EventArgs e)
        {//Author
            Author f = new Author();
            f.Show();
        }

        private void button2_Click(object sender, EventArgs e)
        {//Rules
            Rules f = new Rules();
            f.Show();
        }

        private void button1_Click(object sender, EventArgs e)
        {//Start game
            Game g = new Game();
            this.Hide();
            g.ShowDialog();
            this.Show();
        }

        private void button5_Click(object sender, EventArgs e)
        {
            richTextBox1.Text = "";
            int lvl = 1;
            int[,] a = MapGenerator.Generate(lvl);
            int k = (lvl+1) * 2 + 1;
            for (int i = 0; i < k; i++)
            {
                for (int j = 0; j < k; j++)
                {
                    richTextBox1.Text += a[i, j].ToString() + " ";
                }
                richTextBox1.Text += "\n";
            }
        }
    }
}
