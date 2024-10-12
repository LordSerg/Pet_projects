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
    public partial class Game : Form
    {
        int[,] a;
        int[,] temp;
        bool view = false;//який зріз переглядаємо
        int level = 0, hardness = 0;
        int x, y;//координати чоловічка
        int maxHeight;
        int w, h;
        Bitmap bit;
        Graphics g;
        int lvlSize;
        int magicTimes = 5;
        //for timer:
        const int max_iter_num = 20;
        int iter = 0;
        private void pictureBox1_SizeChanged(object sender, EventArgs e)
        {
            if (pictureBox1.Width > 0 && pictureBox1.Height > 0)
            {
                w = pictureBox1.Width;
                h = pictureBox1.Height;
                bit = new Bitmap(w, h);
                g = Graphics.FromImage(bit);
                g.Clear(Color.White);
                pictureBox1.Image = bit;
                Draw();
            }
        }

        public Game()
        {
            InitializeComponent();
            temp = MapGenerator.Generate(level, hardness);
            lvlSize = (level + 1) * 2 + 1;
            a = new int[lvlSize, lvlSize];
            for (int i = 0; i < lvlSize; i++)
                for (int j = 0; j < lvlSize; j++)
                    a[i, j] = temp[i, j];
            maxHeight = -1;
            for (int i = 0; i < lvlSize; i++)
                for (int j = 0; j < lvlSize; j++)
                    if (a[i, j] > maxHeight) maxHeight = a[i, j];
            maxHeight++;
            x = 0;
            y = 0;
            w = pictureBox1.Width;
            h = pictureBox1.Height;
            bit = new Bitmap(w, h);
            g = Graphics.FromImage(bit);
            g.Clear(Color.White);
            pictureBox1.Image = bit;
            timer1.Enabled = false;
            Draw(); 
        }

        private void Game_KeyDown(object sender, KeyEventArgs e)
        {
            if (iter == 0)
            {
                if (e.KeyCode == Keys.Space)
                {
                    timer1.Enabled = true;
                }
                else if (e.KeyCode == Keys.Right)
                {
                    Move(true);
                    Draw();
                }
                else if (e.KeyCode == Keys.Left)
                {
                    Move(false);
                    Draw();
                }
                if (magicTimes > 0)
                {
                    if (e.KeyCode == Keys.Up)
                    {
                        a[x, y]++;
                        magicTimes--;
                        Draw();
                    }
                    else if (e.KeyCode == Keys.Down)
                    {
                        a[x, y]--;
                        magicTimes--;
                        Draw();
                    }
                }
                if (e.KeyCode == Keys.R)
                {
                    a = new int[lvlSize, lvlSize];
                    for (int i = 0; i < lvlSize; i++)
                        for (int j = 0; j < lvlSize; j++)
                            a[i, j] = temp[i, j];
                    magicTimes = 5+level;
                    x = y = 0;
                    timer1.Enabled = true;
                    view = false;
                    Draw();
                }
            }
        }

        void Move(bool is_forward)
        {
            if (!view)
            {
                if (is_forward)
                {
                    if (y < lvlSize - 1 && Math.Abs(a[x, y + 1] - a[x, y]) <= 1) y++;
                }
                else
                {
                    if (y > 0 && Math.Abs(a[x, y - 1] - a[x, y]) <= 1) y--;
                }
            }
            else
            {
                if (is_forward)
                {
                    if (x < lvlSize - 1 && Math.Abs(a[x + 1, y] - a[x, y]) <= 1) x++;
                }
                else
                {
                    if (x > 0 && Math.Abs(a[x - 1, y] - a[x, y]) <= 1) x--;
                }
            }
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            if (iter >= max_iter_num)
            {
                iter = 0;
                view = !view;
                Draw();
                timer1.Enabled = false;
            }
            else
            {
                g.Clear(Color.White);
                if (view)
                {
                    for (int i = 0; i < lvlSize; i++)
                    {
                        /*
                         a[i,y] -> (((float)a[x, i]*(float)iter/(float)max_iter_num) + ((float)a[i, y]*(float)(max_iter_num - iter)/(float)max_iter_num))
                         */
                        if (x == level + 1 && i == level + 1)
                        {
                            //Gray -> Yellow
                            //(128,128,128) -> (255,255,0)
                            Brush bruh = new SolidBrush(Color.FromArgb( (int)(128*(float)(max_iter_num-iter)/(float)max_iter_num + 255*(float)iter/(float)max_iter_num),
                                                                        (int)(128*(float)(max_iter_num-iter)/(float)max_iter_num + 255*(float)iter/(float)max_iter_num),
                                                                        (int)(128*(float)(max_iter_num-iter)/(float)max_iter_num +   0*(float)iter/(float)max_iter_num)));
                            g.FillRectangle(bruh, i * ((float)w / lvlSize), h - ((((float)a[x, i] * (float)iter / (float)max_iter_num) + ((float)a[i, y] * (float)(max_iter_num - iter) / (float)max_iter_num)) * ((float)h / maxHeight)), (float)w / lvlSize, (((float)a[x, i] * (float)iter / (float)max_iter_num) + ((float)a[i, y] * (float)(max_iter_num - iter) / (float)max_iter_num)) * ((float)h / maxHeight));
                        }
                        else if (y == level + 1 && i == level + 1)
                        {
                            //Yellow -> Gray
                            //(255,255,0) -> (128,128,128)
                            Brush bruh = new SolidBrush(Color.FromArgb( (int)(255*(float)(max_iter_num-iter)/(float)max_iter_num + 128*(float)iter/(float)max_iter_num),
                                                                        (int)(255*(float)(max_iter_num-iter)/(float)max_iter_num + 128*(float)iter/(float)max_iter_num),
                                                                        (int)(  0*(float)(max_iter_num-iter)/(float)max_iter_num + 128*(float)iter/(float)max_iter_num)));
                            g.FillRectangle(bruh, i * ((float)w / lvlSize), h - ((((float)a[x, i] * (float)iter / (float)max_iter_num) + ((float)a[i, y] * (float)(max_iter_num - iter) / (float)max_iter_num)) * ((float)h / maxHeight)), (float)w / lvlSize, (((float)a[x, i] * (float)iter / (float)max_iter_num) + ((float)a[i, y] * (float)(max_iter_num - iter) / (float)max_iter_num)) * ((float)h / maxHeight));
                        }
                        else
                        {
                            g.FillRectangle(Brushes.Gray, i * ((float)w / lvlSize), h - ((((float)a[x, i] * (float)iter / (float)max_iter_num) + ((float)a[i, y] * (float)(max_iter_num - iter) / (float)max_iter_num)) * ((float)h / maxHeight)), (float)w / lvlSize, (((float)a[x, i] * (float)iter / (float)max_iter_num) + ((float)a[i, y] * (float)(max_iter_num - iter) / (float)max_iter_num)) * ((float)h / maxHeight));
                        }
                        g.DrawRectangle(Pens.Black, i * ((float)w / lvlSize), h - ((((float)a[x, i] * (float)iter / (float)max_iter_num) + ((float)a[i, y] * (float)(max_iter_num - iter) / (float)max_iter_num)) * ((float)h / maxHeight)), (float)w / lvlSize, (((float)a[x, i] * (float)iter / (float)max_iter_num) + ((float)a[i, y] * (float)(max_iter_num - iter) / (float)max_iter_num)) * ((float)h / maxHeight));
                        g.DrawString((((float)a[x, i] * (float)iter / (float)max_iter_num) + ((float)a[i, y] * (float)(max_iter_num - iter) / (float)max_iter_num)).ToString(), this.Font, Brushes.Black, i * ((float)w / lvlSize) + (float)w / (2 * lvlSize), h - ((((float)a[x, i] * (float)iter / (float)max_iter_num) + ((float)a[i, y] * (float)(max_iter_num - iter) / (float)max_iter_num)) * ((float)h / maxHeight)));
                    }
                }
                else
                {
                    for (int i = 0; i < lvlSize; i++)
                    {
                        /*
                         a[x, i] -> (((float)a[i, y]*(float)iter/(float)max_iter_num) + ((float)a[x, i]*(float)(max_iter_num - iter)/(float)max_iter_num))
                         */
                        if (y == level + 1 && i == level + 1)
                        {
                            //Gray -> Yellow
                            //(128,128,128) -> (255,255,0)
                            Brush bruh = new SolidBrush(Color.FromArgb((int)(128 * (float)(max_iter_num - iter) / (float)max_iter_num + 255 * (float)iter / (float)max_iter_num),
                                                                        (int)(128 * (float)(max_iter_num - iter) / (float)max_iter_num + 255 * (float)iter / (float)max_iter_num),
                                                                        (int)(128 * (float)(max_iter_num - iter) / (float)max_iter_num + 0 * (float)iter / (float)max_iter_num)));
                            g.FillRectangle(bruh, i * ((float)w / lvlSize), h - ((((float)a[i, y] * (float)iter / (float)max_iter_num) + ((float)a[x, i] * (float)(max_iter_num - iter) / (float)max_iter_num)) * ((float)h / maxHeight)), (float)w / lvlSize, (((float)a[i, y] * (float)iter / (float)max_iter_num) + ((float)a[x, i] * (float)(max_iter_num - iter) / (float)max_iter_num)) * ((float)h / maxHeight));
                        }
                        else if (x == level + 1 && i == level + 1)
                        {
                            //Yellow -> Gray
                            //(255,255,0) -> (128,128,128)
                            Brush bruh = new SolidBrush(Color.FromArgb((int)(255 * (float)(max_iter_num - iter) / (float)max_iter_num + 128 * (float)iter / (float)max_iter_num),
                                                                        (int)(255 * (float)(max_iter_num - iter) / (float)max_iter_num + 128 * (float)iter / (float)max_iter_num),
                                                                        (int)(0 * (float)(max_iter_num - iter) / (float)max_iter_num + 128 * (float)iter / (float)max_iter_num)));
                            g.FillRectangle(bruh, i * ((float)w / lvlSize), h - ((((float)a[i, y] * (float)iter / (float)max_iter_num) + ((float)a[x, i] * (float)(max_iter_num - iter) / (float)max_iter_num)) * ((float)h / maxHeight)), (float)w / lvlSize, (((float)a[i, y] * (float)iter / (float)max_iter_num) + ((float)a[x, i] * (float)(max_iter_num - iter) / (float)max_iter_num)) * ((float)h / maxHeight));
                        }
                        else
                        {
                            g.FillRectangle(Brushes.Gray, i * ((float)w / lvlSize), h - ((((float)a[i, y] * (float)iter / (float)max_iter_num) + ((float)a[x, i] * (float)(max_iter_num - iter) / (float)max_iter_num)) * ((float)h / maxHeight)), (float)w / lvlSize, (((float)a[i, y] * (float)iter / (float)max_iter_num) + ((float)a[x, i] * (float)(max_iter_num - iter) / (float)max_iter_num)) * ((float)h / maxHeight));
                        }
                        g.DrawRectangle(Pens.Black, i * ((float)w / lvlSize), h - ((((float)a[i, y] * (float)iter / (float)max_iter_num) + ((float)a[x, i] * (float)(max_iter_num - iter) / (float)max_iter_num)) * ((float)h / maxHeight)), (float)w / lvlSize, (((float)a[i, y] * (float)iter / (float)max_iter_num) + ((float)a[x, i] * (float)(max_iter_num - iter) / (float)max_iter_num)) * ((float)h / maxHeight));
                        g.DrawString((((float)a[i, y] * (float)iter / (float)max_iter_num) + ((float)a[x, i] * (float)(max_iter_num - iter) / (float)max_iter_num)).ToString(), this.Font, Brushes.Black, (float)i * ((float)w / lvlSize) + ((float)w / (2 * lvlSize)), h - ((((float)a[i, y] * (float)iter / (float)max_iter_num) + ((float)a[x, i] * (float)(max_iter_num - iter) / (float)max_iter_num)) * ((float)h / maxHeight)));
                    }
                }

                if (!view)
                {
                    //y -> ((float)y*(float)(max_iter_num - iter)/(float)max_iter_num + (float)x*(float)iter/(float)max_iter_num)
                    g.FillRectangle(Brushes.Red, ((float)y * (float)(max_iter_num - iter) / (float)max_iter_num + (float)x * (float)iter / (float)max_iter_num) * ((float)w / lvlSize) + (float)w / (2 * lvlSize) - (float)w / (8 * lvlSize), h - (a[x, y] * ((float)h / maxHeight)) - ((float)h / (maxHeight)), (float)w / (5 * lvlSize), (float)h / (maxHeight));
                }
                else
                {
                    //x -> ((float)x*(float)(max_iter_num - iter)/(float)max_iter_num + (float)y*(float)iter/(float)max_iter_num)
                    g.FillRectangle(Brushes.Red, ((float)x * (float)(max_iter_num - iter) / (float)max_iter_num + (float)y * (float)iter / (float)max_iter_num) * ((float)w / lvlSize) + (float)w / (2 * lvlSize) - (float)w / (8 * lvlSize), h - (a[x, y] * ((float)h / maxHeight)) - ((float)h / (maxHeight)), (float)w / (5 * lvlSize), (float)h / (maxHeight));
                }
                
                DrawTimesOfMagic();
                pictureBox1.Image = bit;
                iter++;
            }
        }

        void Draw()
        {
            g.Clear(Color.White);
            if (view)
            {
                for (int i = 0; i < lvlSize; i++)
                {
                    if (i == level + 1 && y == level + 1)
                        g.FillRectangle(Brushes.Yellow, i * ((float)w / lvlSize), h - (a[i, y] * ((float)h / maxHeight)), (float)w / lvlSize, a[i, y] * ((float)h / maxHeight));
                    else
                        g.FillRectangle(Brushes.Gray, i * ((float)w / lvlSize), h - (a[i, y] * ((float)h / maxHeight)), (float)w / lvlSize, a[i, y] * ((float)h / maxHeight));
                    g.DrawRectangle(Pens.Black, i * ((float)w / lvlSize), h - (a[i, y] * ((float)h / maxHeight)), (float)w / lvlSize, a[i, y] * ((float)h / maxHeight));
                    g.DrawString(a[i, y].ToString(), this.Font, Brushes.Black, i * ((float)w / lvlSize) + (float)w / (2 * lvlSize), h - (a[i, y] * ((float)h / maxHeight)));
                }
            }
            else
            {
                for (int i = 0; i < lvlSize; i++)
                {
                    if (i == level + 1 && x == level + 1)
                        g.FillRectangle(Brushes.Yellow, i * ((float)w / lvlSize), h - (a[x, i] * ((float)h / maxHeight)), (float)w / lvlSize, a[x, i] * ((float)h / maxHeight));
                    else
                        g.FillRectangle(Brushes.Gray, i * ((float)w / lvlSize), h - (a[x, i] * ((float)h / maxHeight)), (float)w / lvlSize, a[x, i] * ((float)h / maxHeight));
                    g.DrawRectangle(Pens.Black, i * ((float)w / lvlSize), h - (a[x, i] * ((float)h / maxHeight)), (float)w / lvlSize, a[x, i] * ((float)h / maxHeight));
                    g.DrawString(a[x, i].ToString(), this.Font, Brushes.Black, (float)i * ((float)w / lvlSize) + ((float)w / (2 * lvlSize)), h - (a[x, i] * ((float)h / maxHeight)));
                }
            }
            DrawDude();
            DrawTimesOfMagic();
            pictureBox1.Image = bit;
            if (x == y && x == level + 1)
            {
                if (level == 3)
                    DrawWin(true);
                else 
                    DrawWin(false);
            }
        }

        void DrawDude()
        {
            if (!view) g.FillRectangle(Brushes.Red, y * ((float)w / lvlSize)+ (float)w / (2 * lvlSize) - (float)w / (8 * lvlSize), h - (a[x, y] * ((float)h / maxHeight)) - ((float)h / (maxHeight)), (float)w / (5 * lvlSize), (float)h / (maxHeight));
            else g.FillRectangle(Brushes.Red, x * ((float)w / lvlSize) + (float)w / (2 * lvlSize)- (float)w / (8 * lvlSize), h - (a[x, y] * ((float)h / maxHeight)) - ((float)h / (maxHeight)), (float)w / (5 * lvlSize), (float)h / (maxHeight));
        }
        void DrawTimesOfMagic()
        {
            for (int i = 0; i < magicTimes; i++)
            {
                g.FillRectangle(Brushes.Green, 5 + i * 30, 5, 25, 25);
                g.DrawRectangle(Pens.Black, 5 + i * 30, 5, 25, 25);
            }
        }
        void DrawWin(bool is_win_game)
        {
            string str = "цей рівень";
            if(is_win_game)
                str = "цю гру";
            Random r = new Random();
                int aaaaa = r.Next(0, 5);
                if (aaaaa == 0)
                {
                    MessageBox.Show("Ви виграли "+str+"!!!\nОце було круто👍👍👍\n\n" +
    "─────────────────────░██░▇▆▅▄▃▂  \n" +
    "────────────────────░█▓▓█░▇▆▅▄▃▂ \n" +
    "───────────────────░█▓▓▓█░▇▆▅▄▃▂ \n" +
    "──────────────────░█▓▓▓▓▓█░▇▆▅▄▃▂\n" +
    "─────────────────░█▓▓▓▓▓█░▇▆▅▄▃▂ \n" +
    "──────────░░░───░█▓▓▓▓▓▓█░▇▆▅▄▃▂ \n" +
    "─────────░███░──░█▓▓▓▓▓█░▇▆▅▄▃▂  \n" +
    "───────░██░░░██░█▓▓▓▓▓█░▇▆▅▄▃▂   \n" +
    "──────░█░░█░░░░██▓▓▓▓▓█░▇▆▅▄▃▂   \n" +
    "────░██░░█░░░░░░█▓▓▓▓█░▇▆▅▄▃▂    \n" +
    "───░█░░░█░░░░░░░██▓▓▓█░▇▆▅▄▃▂    \n" +
    "──░█░░░░█░░░░░░░░█▓▓▓█░▇▆▅▄▃▂    \n" +
    "──░█░░░░░█░░░░░░░░█▓▓▓█░▇▆▅▄▃▂   \n" +
    "──░█░░█░░░█░░░░░░░░█▓▓█░▇▆▅▄▃▂   \n" +
    "─░█░░░█░░░░██░░░░░░█▓▓█░▇▆▅▄▃▂   \n" +
    "─░█░░░░█░░░░░██░░░█▓▓▓█░▇▆▅▄▃▂   \n" +
    "─░█░█░░░█░░░░░░███▓▓▓▓█░▇▆▅▄▃▂   \n" +
    "░█░░░█░░░██░░░░░█▓▓▓▓▓█░▇▆▅▄▃▂   \n" +
    "░█░░░░█░░░░█████▓▓▓▓▓█░▇▆▅▄▃▂    \n" +
    "░█░░░░░█░░░░░░░█▓▓▓▓▓█░▇▆▅▄▃▂    \n" +
    "░█░█░░░░██░░░░█▓▓▓▓▓█░▇▆▅▄▃▂     \n" +
    "─░█░█░░░░░████▓▓▓▓██░▇▆▅▄▃▂      \n" +
    "─░█░░█░░░░░░░█▓▓██▓█░▇▆▅▄▃▂      \n" +
    "──░█░░██░░░██▓▓█▓▓▓█░▇▆▅▄▃▂      \n" +
    "───░██░░███▓▓██▓█▓▓█░▇▆▅▄▃▂      \n" +
    "────░██▓▓▓███▓▓▓█▓▓▓█░▇▆▅▄▃▂     \n" +
    "──────░█▓▓▓▓▓▓▓▓█▓▓▓█░▇▆▅▄▃▂     \n" +
    "──────░█▓▓▓▓▓▓▓▓▓▓▓▓▓█░▇▆▅▄▃▂    \n");
                }
                else if (aaaaa == 1)
                {
                    MessageBox.Show("Ви виграли " + str + "!!!\nОце було круто👍👍👍\n\n" +
"⠀⠀⢀⣤⣾⣿⣿⣿⣿⣿⣶⣤⡀⢀⣤⣶⣿⣿⣿⣿⣿⣷⣤⡀⠀⠀ \n" +
"⠀⣰⣿⣿⣿⣿⣿⣿⣿⣿⣿⣿⣿⣿⣿⣿⣿⣿⣿⣿⣿⣿⣿⣿⣆⠀ \n" +
"⢠⣿⣿⣿⣿⣿⣿⣿⣿⣿⣿⣿⣿⣿⣿⣿⣿⣿⣿⣿⣿⣿⣿⣿⣿⡄\n" +
"⢸⣿⣿⣿⣿⣿⣿⣿⣿⣿⣿⣿⣿⣿⣿⣿⣿⣿⣿⣿⣿⣿⣿⣿⣿⡇\n" +
"⠀⣿⣿⣿⣿⣿⣿⣿⣿⣿⣿⣿⣿⣿⣿⣿⣿⣿⣿⣿⣿⣿⣿⣿⣿⠀ \n" +
"⠀⠘⣿⣿⣿⣿⣿⣿⣿⣿⣿⣿⣿⣿⣿⣿⣿⣿⣿⣿⣿⣿⣿⣿⠃⠀ \n" +
"⠀⠀⠈⢿⣿⣿⣿⣿⣿⣿⣿⣿⣿⣿⣿⣿⣿⣿⣿⣿⣿⣿⡿⠁⠀⠀ \n" +
"⠀⠀⠀⠀⠙⢿⣿⣿⣿⣿⣿⣿⣿⣿⣿⣿⣿⣿⣿⣿⡿⠋⠀⠀⠀⠀  \n" +
"⠀⠀⠀⠀⠀⠀⠙⢿⣿⣿⣿⣿⣿⣿⣿⣿⣿⣿⡿⠋⠀⠀⠀⠀⠀⠀  \n" +
"⠀⠀⠀⠀⠀⠀⠀⠀⠙⢿⣿⣿⣿⣿⣿⣿⡿⠋⠀⠀⠀⠀⠀⠀⠀⠀   \n" +
"⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠉⠻⣿⣿⠟⠉⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀    \n" +
"⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠈⠁⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀     \n");
                }
                else if (aaaaa == 2)
                {
                    MessageBox.Show("Ви виграли " + str + "!!!\nОце було круто👍👍👍\n\n" +

"★░░░░░░░░░████░░░░░░░░░░░░░░░░░★\n" +
"★░░░░░░░███░██░░░░░░░░░░░░░░░░░★\n" +
"★░░░░░░░██░░░█░░░░░░░░░░░░░░░░░★\n" +
"★░░░░░░░██░░░██░░░░░░░░░░░░░░░░★\n" +
"★░░░░░░░░██░░░███░░░░░░░░░░░░░░★\n" +
"★░░░░░░░░░██░░░░██░░░░░░░░░░░░░★\n" +
"★░░░░░░░░░██░░░░░███░░░░░░░░░░░★\n" +
"★░░░░░░░░░░██░░░░░░██░░░░░░░░░░★\n" +
"★░░░░░███████░░░░░░░██░░░░░░░░░★\n" +
"★░░█████░░░░░░░░░░░░░░███░██░░░★\n" +
"★░██░░░░░████░░░░░░░░░░██████░░★\n" +
"★░██░░████░░███░░░░░░░░░░░░░██░★\n" +
"★░██░░░░░░░░███░░░░░░░░░░░░░██░★\n" +
"★░░██████████░███░░░░░░░░░░░██░★\n" +
"★░░██░░░░░░░░████░░░░░░░░░░░██░★\n" +
"★░░███████████░░██░░░░░░░░░░██░★\n" +
"★░░░░██░░░░░░░████░░░░░██████░░★\n" +
"★░░░░██████████░██░░░░███░██░░░★\n" +
"★░░░░░░░██░░░░░████░███░░░░░░░░★\n" +
"★░░░░░░░█████████████░░░░░░░░░░★\n" +
"★░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░★\n");
                }
                else if (aaaaa == 3)
                {
                    MessageBox.Show("Ви виграли " + str + "!!!\nОце було круто👍👍👍\n\n" +
"⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⢀⣀⣀⣴⣆⣠⣤                \n" +
"⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠈⣻⣿⣯⣘⠹⣧⣤⡀             \n" +
"⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠛⠿⢿⣿⣷⣾⣯⠉               \n" +
"⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⣾⣿⠜⣿⡍                 \n" +
"⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⣸⣿⠁⠀⠘⣿⣆               \n" +
"⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⢠⣿⡟⠃⡄⠀⠘⢿⣆              \n" +
"⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⣾⣿⣁⣋⣈ ⣤⣮⣿⣧⡀           \n" +
"⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⣠⣤⣤⣤⣤⣤⣶⣦⣤⣄⡀           \n" +
"⠀⠀⠀⠀⠀⠀⠀⢀⣴⣿⡿⠛⠉⠙⠛⠛⠛⠛⠻⢿⣿⣷⣤⡀      \n" +
"⠀⠀⠀⠀⠀⠀⠀⣼⣿⠋⠀⠀⠀⠀⠀⠀⠀⢀⣀⣀⠈⢻⣿⣿⡄      \n" +
"⠀⠀⠀⠀⠀⠀⣸⣿⡏⠀⠀⠀⣠⣶⣾⣿⣿⣿⠿⠿⠿⢿⣿⣿⣿⣄    \n" +
"⠀⠀⠀⠀⠀⠀⣿⣿⠁⠀⠀⢰⣿⣿⣯⠁⠀⠀⠀⠀⠀⠀⠀⠈⠙⢿⣷⡄  \n" +
"⠀⣀⣤⣴⣶⣶⣿⡟⠀⠀⠀⢸⣿⣿⣿⣆⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⣿⣷  \n" +
"⢰⣿⡟⠋⠉⣹⣿⡇⠀⠀⠀⠘⣿⣿⣿⣿⣷⣦⣤⣤⣤⣶⣶⣶⣶⣿⣿⣿\n" +
"⢸⣿⡇⠀⠀⣿⣿⡇⠀⠀⠀⠀⠹⣿⣿⣿⣿⣿⣿⣿⣿⣿⣿⣿⣿⣿⡿⠃ \n" +
"⣸⣿⡇⠀⠀⣿⣿⡇⠀⠀⠀⠀⠀⠉⠻⠿⣿⣿⣿⣿⡿⠿⠿⠛⢻⣿⡇  \n" +
"⣿⣿⠁⠀⠀⣿⣿⡇⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⢸⣿⣧    \n" +
"⣿⣿⠀⠀⠀⣿⣿⡇⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⢸⣿⣿    \n" +
"⣿⣿⠀⠀⠀⣿⣿⡇⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⢸⣿⣿    \n" +
"⢿⣿⡆⠀⠀⣿⣿⡇⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⢸⣿⡇    \n" +
"⠸⣿⣧⡀⠀⣿⣿⡇⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⣿⣿⠃    \n" +
"⠀⠛⢿⣿⣿⣿⣿⣇⠀⠀⠀⠀⠀⣰⣿⣿⣷⣶⣶⣶⣶⠶⢠⣿⣿    \n" +
"⠀⠀⠀⠀⠀⠀⣿⣿⠀⠀⠀⠀⠀⣿⣿⡇⠀⣽⣿⡏⠁⠀⠀⢸⣿⡇     \n" +
"⠀⠀⠀⠀⠀⠀⣿⣿⠀⠀⠀⠀⠀⣿⣿⡇⠀⢹⣿⡆⠀⠀⠀⣸⣿⠇     \n" +
"⠀⠀⠀⠀⠀⠀⢿⣿⣦⣄⣀⣠⣴⣿⣿⠁⠀⠈⠻⣿⣿⣿⣿⡿⠏     \n" +
"⠀⠀⠀⠀⠀⠀⠈⠛⠻⠿⠿⠿⠿⠋⠁⠀⠀⠀               \n");
                }
                else if (aaaaa == 4)
                {
                    MessageBox.Show("Ви виграли " + str + "!!!\nОце було круто👍👍👍\n\n" +

"─────────────▒███░───░████████▒       \n"+
"──────────█████▒░█████░▒▒▒▒▒▒█████    \n"+
"─────────██▒▒▒▒██████████████▒▒▒██░   \n"+
"────────██▒▒▒▒███▒██▒██▒▒█████▒░▒██   \n"+
"────────█░▒▒▒██▒████████████▒█▒▒▒█░   \n"+
"────────█▒▒▒▒██▒▒▒░▓▓▒░▓▒▒████▒▒██    \n"+
"────────█▒▒▒▒██▒▒▒▒▒▒▒▒▒▒▒█▒█░▒████\n"+
"────███████████▒▒▒▒▒▒▒▒██████▒██▓▒██\n"+
"────██▒▒▒▒▒▒█████▒▒▒▒▒▒▒▒█████▒▒▒▒▒█\n"+
"──────██▒▒▒▒▒▒▒▓██████▒▒▒▒▒██▒▒▒▒▒▒██\n"+
"───█████▒▒▒▒▒▒▒▒▒▒████▒▒▒██▒▒▒▒▒▒██\n"+
"───██▒▒▒███▒▒▒▒▒▒▒▒▒▒▓█████▒▒▒▒▒██\n"+
"───███▒▒▒▒███▒▒▒▒▒▒▒▒▒▒▒███▓▒▒███     \n"+
"─────█████▒▒████▒▒▒▒▒▒▒▒▒▒█████       \n"+
"────────████▒▒██████▒▒▒▒█████         \n"+
"───────────███▒▒██████████            \n"+
"─────────────████▓──█▓█               \n"+
"───────────────────████               \n"+
"───────────────────█░█─────█████████  \n"+
"───────────────────█▓█───█████████████\n"+
"─░█████████───────████──██▓███▒▓████  \n"+
"█████████████─────█░███████░██████    \n"+
"──████░▒███▒██────█▓██████████        \n"+
"────█████▓▒█████─████                 \n"+
"────────██████████▓█                  \n"+
"─────────────────█▓█────████▒█▓▒█     \n"+
"────────────────█▓██──█████████████   \n"+
"────────────────█▓█──██▒████░█████    \n"+
"───────────────██████████▒██████      \n"+
"───────────────█▓███████████          \n"+
"──────────────████                    \n"+
"──────────────█▒█                     \n"+
"──────────────███                     \n");
                }
            //Application.Exit();
            if (is_win_game)
                this.Close();
            else
            {
                level++;
                temp = MapGenerator.Generate(level, hardness);
                lvlSize = (level + 1) * 2 + 1;
                a = new int[lvlSize, lvlSize];
                for (int i = 0; i < lvlSize; i++)
                    for (int j = 0; j < lvlSize; j++)
                        a[i, j] = temp[i, j];
                maxHeight = -1;
                for (int i = 0; i < lvlSize; i++)
                    for (int j = 0; j < lvlSize; j++)
                        if (a[i, j] > maxHeight) maxHeight = a[i, j];
                maxHeight++;
                magicTimes = 5 + level+((level+1)/2);
                x = y = 0;
                timer1.Enabled = true;
                view = false;
                Draw();
            }
        }
    }
}
