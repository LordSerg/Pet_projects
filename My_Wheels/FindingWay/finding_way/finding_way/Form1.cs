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
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }
        bool is_start_chosen, is_finish_chosen;
        Map map;
        int Size=20;//размер одной ячейки
        private void Form1_Load(object sender, EventArgs e)
        {
            is_finish_chosen = false;
            is_start_chosen = false;
            map = new Map((int)pictureBox1.Width / Size, (int)pictureBox1.Height / Size);
            map.Generate(30);
            Map.Show(pictureBox1, Size);
        }
        private void Button1_Click(object sender, EventArgs e)
        {//генерация припятствий
            is_finish_chosen = false;
            is_start_chosen = false;
            map = new Map((int)pictureBox1.Width / Size, (int)pictureBox1.Height / Size);
            map.Generate(20);
            Map.Show(pictureBox1, Size);
        }

        private void Button2_Click(object sender, EventArgs e)
        {//поиск пути из одной точки в другую, минуя препятствия
            
            //map.Show1(pictureBox1, Size,map.FindWay());
            map.FindWay();
            Map.Show(pictureBox1, Size);
        }

        private void PictureBox1_Click(object sender, EventArgs e)
        {//выбор начальной и конечной точки
            if(is_start_chosen==false&&is_finish_chosen==false)
            {//выбираем начало
                if (map.SetStart((int)(Cursor.Position.X - pictureBox1.Location.X) / Size, (int)(Cursor.Position.Y - pictureBox1.Location.Y) / Size) == true)
                {
                    Map.Show(pictureBox1, Size);
                    is_start_chosen = true;
                }
            }
            else if(is_finish_chosen==false)
            {//выбираем конец
                if (map.SetFinish((int)(Cursor.Position.X - pictureBox1.Location.X) / Size, (int)(Cursor.Position.Y - pictureBox1.Location.Y) / Size) == true)
                {
                    Map.Show(pictureBox1, Size);
                    is_start_chosen = false;
                    is_finish_chosen = true;
                }
            }
            else if(is_start_chosen==false)
            {//выбираем начало
                if (map.SetStart((int)(Cursor.Position.X - pictureBox1.Location.X) / Size, (int)(Cursor.Position.Y - pictureBox1.Location.Y) / Size) == true)
                {
                    Map.Show(pictureBox1, Size);
                    is_start_chosen = true;
                    is_finish_chosen = false;
                }
            }
        }

        private void Button3_Click(object sender, EventArgs e)
        {
            Map.Show(pictureBox1, Size);
        }

        private void Button4_Click(object sender, EventArgs e)
        {
            map.Show1(pictureBox1, Size, map.FindWay());
        }

        private void Button5_Click(object sender, EventArgs e)
        {
            map.Show2(pictureBox1, Size, map.FindWay());
        }

        class Map
        {
            static int[,] m;
            static int w, h;
            public Point from, to;
            public Map(int width, int height)
            {
                m = new int[width, height];
                w = width;
                h = height;
                //-1 - препятствие;
                //0 - нет припятствий;
                //-2 - начало пути
                //-3 - конец пути
                //-5 - путь
                from = new Point();
                to = new Point();
            }
            public void Generate(int prosent_of_let)//процент припятствий
            {
                Random r = new Random();
                for (int i = 0; i < w; i++)
                    for (int j = 0; j < h; j++)
                        if (r.Next(prosent_of_let - 50, prosent_of_let + 50) > 50)
                            m[i, j] = -1;
                        else
                            m[i, j] = 0;
            }
            public static void Show(PictureBox pictBox,int size)
            {
                Bitmap b = new Bitmap(pictBox.Width, pictBox.Height);
                Graphics g = Graphics.FromImage(b);
                g.Clear(Color.White);
                for (int i = 0; i < w; i++)
                    for (int j = 0; j < h; j++)
                        if (m[i, j] == 0)//ничего
                            g.FillRectangle(Brushes.White, i * size, j * size, size, size);
                        else if (m[i, j] == -1)//преграда
                            g.FillRectangle(Brushes.Black, i * size, j * size, size, size);
                        else if (m[i, j] == -2)//начало
                            g.FillRectangle(Brushes.Blue, i * size, j * size, size, size);
                        else if (m[i, j] == -3)//конец
                            g.FillRectangle(Brushes.LightBlue, i * size, j * size, size, size);
                        else if (m[i, j] == -5)//путь
                            g.FillRectangle(Brushes.Red, i * size, j * size, size, size);
                pictBox.Image = b;
            }
            public void Show1(PictureBox pictBox, int size,double[,] array)
            {
                Bitmap b = new Bitmap(pictBox.Width, pictBox.Height);
                Graphics g = Graphics.FromImage(b);
                Font fnt = new System.Drawing.Font("Arial", (float)10);
                Brush br = new SolidBrush(Color.Red);
                g.Clear(Color.White);
                for (int i = 0; i < w; i++)
                    for (int j = 0; j < h; j++)
                    {
                        //if (array[i, j] > 0)
                        //    br = new SolidBrush(Color.FromArgb(250 - (int)array[i, j], 250 - (int)array[i, j], 250 - (int)array[i, j]));
                        //else
                        //    br = new SolidBrush(Color.FromArgb(0, 0, 0));
                        //g.FillRectangle(br, i * size, j * size, size, size);
                        g.DrawString(array[i,j].ToString(),fnt,br,  i * size, j * size);
                    }
                pictBox.Image = b;
            }
            public void Show2(PictureBox pictBox, int size, double[,] array)
            {
                Bitmap b = new Bitmap(pictBox.Width, pictBox.Height);
                Graphics g = Graphics.FromImage(b);
                Font fnt = new System.Drawing.Font("Arial", (float)10);
                Brush br = new SolidBrush(Color.Red);
                g.Clear(Color.White);
                for (int i = 0; i < w; i++)
                    for (int j = 0; j < h; j++)
                    {
                        if (array[i, j] > 0)
                            br = new SolidBrush(Color.FromArgb(250-(int)array[i, j], 250 - (int)array[i, j], 250 - (int)array[i, j]));
                        else
                            br = new SolidBrush(Color.FromArgb(0, 0, 0));
                        g.FillRectangle(br, i * size, j * size, size, size);
                        //g.DrawString(array[i, j].ToString(), fnt, br, i * size, j * size);
                    }
                pictBox.Image = b;
            }
            public bool SetStart(int x, int y)
            {
                try
                {


                    if (m[x, y] != -1 && m[x, y] != -3)
                    {
                        m[from.X, from.Y] = 0;
                        from = new Point(x, y);
                        m[x, y] = -2;
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
            public bool SetFinish(int x, int y)
            {
                try
                {
                    if (m[x, y] != -1 && m[x, y] != -2)
                    {
                        m[to.X, to.Y] = 0;
                        to = new Point(x, y);
                        m[x, y] = -3;
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
            public double [,] FindWay()
            {//волновой алгоритм (с диагональю)
                if (to != from)
                {
                    Point[] way;
                    double[,] find_way;
                    int length = 1, count = 0, c = 0;
                    bool ok = false;
                    way = new Point[w * h];
                    //for (int i = 0; i < w * h; i++)
                    //    way[i] = new Point(0, 0);
                    find_way = new double[w, h];
                    for (int i = 0; i < w; i++)
                        for (int j = 0; j < h; j++)
                            if (m[i, j] == -1)
                                find_way[i, j] = -1;
                            //else if (i == from.X && j == from.Y)
                            //    find_way[i, j] = 0;
                            else
                                find_way[i, j] = 0;
                    //else if (i == to.X && j == to.Y)
                    //    find_way[i, j] = 0;
                    way[0] = from;
                    //цикл для волн(поиска всех вариантов):
                    while (true)
                    {
                        count = 0;//счетчик новых "помеченых точек"
                        c = 0;
                        for (int u = 0; u < length; u++)
                        {
                            for (int i = -1; i < 2; i++)
                                for (int j = -1; j < 2; j++)
                                    if (i != 0 || j != 0)
                                    {
                                        if (cnv(way[u].X + i, way[u].Y + j, w, h) == true)//если не выходим за рамки карты
                                        {
                                            if (way[u].X + i == to.X && way[u].Y + j == to.Y)//если это искомая точка
                                            {
                                                way[count] = new Point(way[u].X + i, way[u].Y + j);
                                                ok = true;
                                            }
                                            else if (find_way[way[u].X + i, way[u].Y + j] != -1)//если точка - не препятствие
                                            {
                                                if (way[u].X + i != from.X && way[u].Y + j != from.Y)//если это не начальная точка
                                                {
                                                    if (Math.Abs(i) == Math.Abs(j))//помечаем диагональ
                                                    {
                                                        if (find_way[way[u].X, way[u].Y] + 1.4 < find_way[way[u].X + i, way[u].Y + j] && find_way[way[u].X + i, way[u].Y + j] > 0)
                                                        {
                                                            find_way[way[u].X + i, way[u].Y + j] = find_way[way[u].X, way[u].Y] + 1.4;
                                                            way[count] = new Point(way[u].X + i, way[u].Y + j);
                                                            count++;
                                                        }
                                                        else if (find_way[way[u].X + i, way[u].Y + j] == 0)
                                                        {
                                                            find_way[way[u].X + i, way[u].Y + j] = find_way[way[u].X, way[u].Y] + 1.4;
                                                            way[count] = new Point(way[u].X + i, way[u].Y + j);
                                                            count++;
                                                        }
                                                    }
                                                    else//помечаем прямую
                                                    {
                                                        if (find_way[way[u].X, way[u].Y] + 1 < find_way[way[u].X + i, way[u].Y + j] && find_way[way[u].X + i, way[u].Y + j] > 0)
                                                        {
                                                            find_way[way[u].X + i, way[u].Y + j] = find_way[way[u].X, way[u].Y] + 1;
                                                            way[count] = new Point(way[u].X + i, way[u].Y + j);
                                                            count++;
                                                        }
                                                        else if (find_way[way[u].X + i, way[u].Y + j] == 0)
                                                        {
                                                            find_way[way[u].X + i, way[u].Y + j] = find_way[way[u].X, way[u].Y] + 1;
                                                            way[count] = new Point(way[u].X + i, way[u].Y + j);
                                                            count++;
                                                        }
                                                    }
                                                    //way[count] = new Point(way[u].X + i, way[u].Y + j);
                                                    //count++;
                                                }
                                            }
                                            else
                                                c++;
                                        }
                                    }
                        }
                        if (ok == true)
                        {//дошли до точки
                            break;
                        }
                        if (c == 8 * length)//если весь алгоритм зашел в тупик
                            break;
                        length = count;
                    }
                    if (ok)
                    {
                        //очищаем массив точек (для записи пути)
                        for (int i = 0; i < w * h; i++)
                            way[i] = new Point(0, 0);
                        Form2 f = new Form2();
                        f.Text = "ВСЕ ОК";
                        f.ShowDialog();
                        //count = 0;
                        //ищем самый короткий путь, начиная с to к from
                        int q = 0;
                        way[q] = to;
                        //m[way[0].X, way[0].Y] = -5;
                        while (true)
                        {
                            q++;
                            if (q >= w * h)
                                break;

                            way[q] = find_min(way[q - 1], find_way);
                            if (way[q] == from)
                            {
                                break;
                            }
                            else
                                m[way[q].X, way[q].Y] = -5;
                            //if (m[way[q].X, way[q].Y] == -2)
                            //{
                            //    m[way[q].X, way[q].Y] = -5;
                            //    break;
                            //}

                            //if (way[q] == from)
                        }
                    }
                    else
                    {
                        Form2 f = new Form2();
                        f.Text = "ВСЕ НЕ ОК";
                        f.ShowDialog();
                    }
                    return find_way;
                }
                else
                    return null;
            }

            public double [,]Find_way()
            {
                if (from != to)
                {
                    while(true)
                    {
                        for (int i = -1; i < 2; i++)
                            for (int j = -1; j < 2; j++)
                                if (i != 0 && j != 0)
                                {

                                }
                    }
                }
                else
                    return null;
            }
            Point find_min(Point p, double[,] mp)
            {//находим минимальное, ненулевое значение на карте путей
                //и возвращаем координату с этим значчением.
                //0
                Point[] pi = new Point[8];
                double[] z = new double[8];
                int imin=0,g=0;
                for(int i=-1;i<2;i++)
                    for(int j=-1;j<2;j++)
                        if (i != 0 || j != 0)
                        {
                            if (cnv(p.X + i, p.Y + j, w, h) == true)
                            {
                                pi[g] = new Point(p.X + i, p.Y + j);
                                z[g] = mp[p.X + i, p.Y + j];
                                g++;
                            }
                        }
                for (int i = 0; i < g; i++)
                    if (z[i] > 0)
                    {
                        imin = i;
                        break;
                    }
                for (int i = 0; i < 8; i++)
                    if (z[i] < z[imin] && z[i] > 0)
                        imin = i;
                //if(pi[imin].X==from.X&& pi[imin].Y)
                return pi[imin];
            }
            bool cnv(int x,int y,int W,int H)
            {
                if (x >= W)
                    return false;
                if (x < 0)
                    return false;
                if (y >= H)
                    return false;
                if (y < 0)
                    return false;
                return true;
            }

        }
    }
}
