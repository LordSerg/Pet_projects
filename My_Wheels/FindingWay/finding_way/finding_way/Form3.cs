using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace finding_way
{
    public partial class Form3 : Form
    {
        public Form3()
        {
            InitializeComponent();
        }
        double [,]map;
        int w, h;
        Graphics g;
        Bitmap b;
        bool is_start_chosen = false, is_finish_chosen = false;
        int Size = 20;
        Point from, to;
        private void Form3_Load(object sender, EventArgs e)
        {
            w = (int)(pictureBox1.Width / Size);
            h = (int)(pictureBox1.Height / Size);
            map = new double[w, h];
            for (int i = 0; i < w; i++)
                for (int j = 0; j < h; j++)
                    map[i, j] = 0;
            generate(30);
            Show();
        }

        private void Button1_Click(object sender, EventArgs e)
        {//сгенерировать припятствие
            generate(30);
            Show();
        }

        private void Button2_Click(object sender, EventArgs e)
        {//найти путь
            Point[] way = find_way(from,to,true);
            Show2(way);
        }

        private void Button3_Click(object sender, EventArgs e)
        {//показать карту
            Show();
        }

        private void Button4_Click(object sender, EventArgs e)
        {//показать цифры

        }

        private void Button5_Click(object sender, EventArgs e)
        {//показать градиент
            find_way(from,to,false);
            Show1();
        }
        private void PictureBox1_Click(object sender, EventArgs e)
        {
            if (!isRight)
            {
                //выбор начальной и конечной точки
                if (is_start_chosen == false && is_finish_chosen == false)
                {//выбираем начало
                    if (SetStart((int)(Cursor.Position.X - pictureBox1.Location.X) / Size, (int)(Cursor.Position.Y - pictureBox1.Location.Y) / Size) == true)
                    {
                        Show();
                        is_start_chosen = true;
                    }
                }
                else if (is_finish_chosen == false)
                {//выбираем конец
                    if (SetFinish((int)(Cursor.Position.X - pictureBox1.Location.X) / Size, (int)(Cursor.Position.Y - pictureBox1.Location.Y) / Size) == true)
                    {
                        Show();
                        is_start_chosen = false;
                        is_finish_chosen = true;
                    }
                }
                else if (is_start_chosen == false)
                {//выбираем начало
                    if (SetStart((int)(Cursor.Position.X - pictureBox1.Location.X) / Size, (int)(Cursor.Position.Y - pictureBox1.Location.Y) / Size) == true)
                    {
                        Show();
                        is_start_chosen = true;
                        is_finish_chosen = false;
                    }
                }
            }
            else
            {//раасставляем препятствия
                map[(int)(Cursor.Position.X - pictureBox1.Location.X) / Size, (int)(Cursor.Position.Y - pictureBox1.Location.Y) / Size] = -1;
                Show();
            }
        }
        bool SetStart(int x,int y)
        {
            try
            {
                Point a = new Point(x, y);
                if (map[x, y] != -1 && to != a)
                {
                    //map[from.X, from.Y] = 0;
                    from = new Point(x, y);
                    //m[x, y] = -2;
                    return true;
                }
                else
                    return false;
            }
            catch
            {
                return false;
            }
        }
        bool SetFinish(int x, int y)
        {
            try
            {
                Point a = new Point(x, y);
                if (map[x, y] != -1 && from != a)
                {
                    to = new Point(x, y);
                    return true;
                }
                else
                    return false;
            }
            catch
            {
                return false;
            }
        }
        void generate(int p)
        {
            Random r = new Random();
            for (int i = 0; i < w; i++)
                for (int j = 0; j < h; j++)
                    if (r.Next(100) < p)
                        map[i, j] = -1;
            else
                        map[i, j] = 0;
            from = new Point();
            to = new Point();
        }
        void Show2(Point[]way)
        {
            b = new Bitmap(pictureBox1.Width, pictureBox1.Height);
            g = Graphics.FromImage(b);
            for (int i = 0; i < way.Length; i++)
                g.FillRectangle(Brushes.Green, way[i].X * Size, way[i].Y * Size, Size, Size);
            for(int i=0;i<w;i++)
                for(int j=0;j<h;j++)
                {
                    if (map[i, j] == -1)
                        g.FillRectangle(Brushes.Black, i * Size, j * Size, Size, Size);
                    else if (i == from.X && j == from.Y)
                        g.FillRectangle(Brushes.Red, i * Size, j * Size, Size, Size);
                    else if (i == to.X && j == to.Y)
                        g.FillRectangle(Brushes.Blue, i * Size, j * Size, Size, Size);
                }
            pictureBox1.Image = b;
        }
        void Show1()
        {
            b = new Bitmap(pictureBox1.Width, pictureBox1.Height);
            g = Graphics.FromImage(b);
            for (int i = 0; i < w; i++)
                for (int j = 0; j < h; j++)
                {
                    if (map[i, j] == -1)
                        g.FillRectangle(Brushes.Black, i * Size, j * Size, Size, Size);
                    else if (i == from.X && j == from.Y)
                        g.FillRectangle(Brushes.Red, i * Size, j * Size, Size, Size);
                    else if (i == to.X && j == to.Y)
                        g.FillRectangle(Brushes.Blue, i * Size, j * Size, Size, Size);
                    else
                        g.FillRectangle(new SolidBrush(Color.FromArgb(255 - (int)(map[i, j]>254?255: map[i, j]), 255 - (int)(map[i, j] > 254 ? 255 : map[i, j]), 255 - (int)(map[i, j] > 254 ? 255 : map[i, j]))), i * Size, j * Size, Size, Size);
                        //g.FillRectangle(new SolidBrush(Color.Gray), i * Size, j * Size, Size, Size);
                }
            pictureBox1.Image = b;
        }
        bool isRight;
        private void pictureBox1_MouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
                isRight = false;
            else
                isRight = true;
        }

        void Show()
        {
            b = new Bitmap(pictureBox1.Width, pictureBox1.Height);
            g = Graphics.FromImage(b);
            for (int i = 0; i < w; i++)
                for (int j = 0; j < h; j++)
                {
                    if (map[i, j] == -1)
                        g.FillRectangle(Brushes.Black, i * Size, j * Size, Size, Size);
                    else if (i == from.X && j == from.Y)
                        g.FillRectangle(Brushes.Red, i * Size, j * Size, Size, Size);
                    else if (i == to.X && j == to.Y)
                        g.FillRectangle(Brushes.Blue, i * Size, j * Size, Size, Size);
                }
            pictureBox1.Image = b;
            label1.Text = "from:"+from.X+" "+from.Y;
            label2.Text = "to:"+to.X+" "+to.Y;
        }
        Point[] find_way(Point from,Point to, bool k)
        {
            List<Point> answer = new List<Point>();
            answer.Add(to);
            //маркируем карту:
            for (int ii = 0; ii < w; ii++)
                for (int jj = 0; jj < h; jj++)
                    map[ii, jj] = map[ii, jj] == -1 ? -1 : 0;
            int i = from.X, j = from.Y;
            map[i, j] = 200;
            setDist(i, j, 1);
            //ищем мин. путь
            if (k)
            {
                i = to.X;
                j = to.Y;
                bool brk = false;
                double min;
                int deadline = 0;
                while (i >= 0 && i < w && j >= 0 && j < h && !brk)
                {
                    if (i == from.X && j == from.Y)
                    {
                        brk = true;
                        break;
                    }
                    min = map[i, j];
                    if (i + 1 >= 0 && i + 1 < w && j >= 0 && j < h && map[i + 1, j] < min && map[i + 1, j] != -1)
                    {
                        i++;
                        answer.Add(new Point(i, j));
                    }
                    else if (i >= 0 && i < w && j + 1 >= 0 && j + 1 < h && map[i, j + 1] < min && map[i, j + 1] != -1)
                    {
                        j++;
                        answer.Add(new Point(i, j));
                    }
                    else if (i - 1 >= 0 && i - 1 < w && j >= 0 && j < h && map[i - 1, j] < min && map[i - 1, j] != -1)
                    {
                        i--;
                        answer.Add(new Point(i, j));
                    }
                    else if (i >= 0 && i < w && j - 1 >= 0 && j - 1 < h && map[i, j - 1] < min && map[i, j - 1] != -1)
                    {
                        j--;
                        answer.Add(new Point(i, j));
                    }

                    else if (i+1 >= 0 && i+1 < w && j + 1 >= 0 && j + 1 < h && map[i+1, j + 1] < min && map[i+1, j + 1] != -1)
                    {
                        i++;
                        j++;
                        answer.Add(new Point(i, j));
                    }
                    else if (i + 1 >= 0 && i + 1 < w && j-1 >= 0 && j-1 < h && map[i + 1, j-1] < min && map[i + 1, j-1] != -1)
                    {
                        i++;
                        j--;
                        answer.Add(new Point(i, j));
                    }
                    else if (i-1 >= 0 && i-1 < w && j + 1 >= 0 && j + 1 < h && map[i-1, j + 1] < min && map[i-1, j + 1] != -1)
                    {
                        i--;
                        j++;
                        answer.Add(new Point(i, j));
                    }
                    else if (i-1 >= 0 && i-1 < w && j - 1 >= 0 && j - 1 < h && map[i-1, j - 1] < min && map[i-1, j - 1] != -1)
                    {
                        i--;
                        j--;
                        answer.Add(new Point(i, j));
                    }
                    else
                    {
                        /*answer.RemoveAt(answer.Count - 1);
                        map[answer[answer.Count - 1].X, answer[answer.Count - 1].Y] = 120;
                        i = answer[answer.Count - 1].X;
                        j = answer[answer.Count - 1].Y;*/
                        deadline++;
                    }
                    if (deadline > 100)
                        break;
                }
            }
            //
            return answer.ToArray();
        }

        void setDist(int i, int j, double val)
        {
            if (i == to.X && j == to.Y)
            {
                map[i, j] = val;
            }
            else if (map[i, j] != -1 && (map[i, j] == 0 || map[i, j] > val))
            {
                map[i, j] = val;
                if (i + 1 >= 0 && i + 1 < w && j >= 0 && j < h)
                    setDist(i + 1, j, val + 1);
                if (i >= 0 && i < w && j + 1 >= 0 && j + 1 < h)
                    setDist(i, j + 1, val + 1);
                if (i - 1 >= 0 && i - 1 < w && j >= 0 && j < h)
                    setDist(i - 1, j, val + 1);
                if (i >= 0 && i < w && j - 1 >= 0 && j - 1 < h)
                    setDist(i, j - 1, val + 1);

                if (i + 1 >= 0 && i + 1 < w && j + 1 >= 0 && j + 1 < h)
                    setDist(i + 1, j + 1, val + Math.Sqrt(2));
                if (i + 1 >= 0 && i + 1 < w && j - 1 >= 0 && j - 1 < h)
                    setDist(i + 1, j - 1, val + Math.Sqrt(2));
                if (i - 1 >= 0 && i - 1 < w && j + 1 >= 0 && j + 1 < h)
                    setDist(i - 1, j + 1, val + Math.Sqrt(2));
                if (i - 1 >= 0 && i - 1 < w && j - 1 >= 0 && j - 1 < h)
                    setDist(i - 1, j - 1, val + Math.Sqrt(2));
            }
        }
    }
}
