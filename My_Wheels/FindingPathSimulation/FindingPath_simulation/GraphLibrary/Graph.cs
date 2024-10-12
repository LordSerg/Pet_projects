using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;

namespace GraphLibrary
{
    public class Graph
    {
        public List<Vertex> v;
        public List<Edge> u;
        public int N { get { return v.Count(); } }
        private int n;
        float[,] M;//матрица смежности (0 или 1)
        //float[,] M1;//матрица смежности (с весами дуг)
        float[] m;//весы вершин
        public Graph(Vertex[] vertices, Edge[] edges)
        {
            v = new List<Vertex>();
            u = new List<Edge>();
            foreach (Vertex x in vertices) v.Add(x);
            foreach (Edge x in edges) u.Add(x);
            n = N;
        }
        public Graph(Graph gr)
        {
            v = new List<Vertex>();
            u = new List<Edge>();
            foreach (Vertex x in gr.v) v.Add(new Vertex(x));
            foreach (Edge x in gr.u) u.Add(new Edge(x));
            n = N;
        }
        public Graph(int w,int h,int k)//k - для вида сетки(2-,3-,4-,6-угольная)
        {
            v = new List<Vertex>();
            u = new List<Edge>();
            n = w*h;
            M = new float[n, n];
            if (k == 0)
            {//квадратная скетка без диагоналей
                for (int j = 0; j < w; j++)
                {
                    for (int i = 0; i < h; i++)
                    {
                        v.Add(new Vertex(100 + i * 65, 100 + j * 65));
                    }
                }
                for (int i = 0; i < n; i++)
                {
                    if ((i + 1) % h != 0 && i + 1 < n)
                        u.Add(new Edge(i, i + 1, 1));
                    if (i + h < n)
                        u.Add(new Edge(i, i + h, 1));
                    //if (i - h >= 0)
                    //    u.Add(new Edge(i, i - h, 1));
                    //if ((i - h + 1) % w != 0&& i - h + 1>=0)
                    //    u.Add(new Edge(i, i - h + 1, 1));
                    //if ((i - h - 1) % w != w - 1&& i - h - 1>=0)
                    //    u.Add(new Edge(i, i - h - 1, 1));
                    //if ((i + h + 1) % h != 0 && i + h + 1 < n)
                    //    u.Add(new Edge(i, i + h + 1, 1));
                    //if ((i + h - 1) % h != h - 1 && i + h - 1 < n)
                    //    u.Add(new Edge(i, i + h - 1, 1));
                }
            }
            else if (k == 1)
            {//сетка для треугольников
                for (int j = 0; j < w; j++)
                {
                    for (int i = 0; i < h; i++)
                    {
                        if (i % 2 == 0)
                        {
                            if (j % 2 == 0)
                                v.Add(new Vertex(100 + i * 65, 100 + j * 65));
                            else
                                v.Add(new Vertex(100 + i * 65, 80 + j * 65));
                        }
                        else
                        {
                            if (j % 2 == 0)
                                v.Add(new Vertex(100 + i * 65, 80 + j * 65));
                            else
                                v.Add(new Vertex(100 + i * 65, 100 + j * 65));
                        }
                    }
                }
                for (int i = 0; i < n; i++)
                {
                    if ((i + 1) % h != 0 && i + 1 < n)
                        u.Add(new Edge(i, i + 1, 1));
                    if ((i / h) % 2 == 0)
                    {
                        if ((i % h) % 2 == 0)
                        {
                            if (i + h < n)
                                u.Add(new Edge(i, i + h, 1));
                        }
                    }
                    else
                    {
                        if ((i % h) % 2 == 1)
                        {
                            if (i + h < n)
                                u.Add(new Edge(i, i + h, 1));
                        }
                    }
                    //if (i - h >= 0)
                    //    u.Add(new Edge(i, i - h, 1));
                    //if ((i - h + 1) % w != 0&& i - h + 1>=0)
                    //    u.Add(new Edge(i, i - h + 1, 1));
                    //if ((i - h - 1) % w != w - 1&& i - h - 1>=0)
                    //    u.Add(new Edge(i, i - h - 1, 1));
                    //if ((i + h + 1) % h != 0 && i + h + 1 < n)
                    //    u.Add(new Edge(i, i + h + 1, 1));
                    //if ((i + h - 1) % h != h - 1 && i + h - 1 < n)
                    //    u.Add(new Edge(i, i + h - 1, 1));
                }
            }
            else if (k == 2)
            {//квадратная сетка с диагоналями
                for (int j = 0; j < w; j++)
                {
                    for (int i = 0; i < h; i++)
                    {
                        v.Add(new Vertex(100 + i * 65, 100 + j * 65));
                    }
                }
                for (int i = 0; i < n; i++)
                {
                    if ((i + 1) % h != 0 && i + 1 < n)
                        u.Add(new Edge(i, i + 1, 1));
                    if (i + h < n)
                        u.Add(new Edge(i, i + h, 1));
                    //if (i - h >= 0)
                    //    u.Add(new Edge(i, i - h, 1));
                    //if ((i - h + 1) % w != 0&& i - h + 1>=0)
                    //    u.Add(new Edge(i, i - h + 1, 1));
                    //if ((i - h - 1) % w != w - 1&& i - h - 1>=0)
                    //    u.Add(new Edge(i, i - h - 1, 1));
                    if ((i + h + 1) % h != 0 && i + h + 1 < n)
                        u.Add(new Edge(i, i + h + 1, 1.4f));
                    if ((i + h - 1) % h != h - 1 && i + h - 1 < n)
                        u.Add(new Edge(i, i + h - 1, 1.4f));
                }
            }
            else if(k==3)
            {//6-угольная сетка
                for (int j = 0; j < w; j++)
                {
                    for (int i = 0; i < h; i++)
                    {
                        if (j % 2 == 0)
                            v.Add(new Vertex(100 + i * 65, 100 + j * 65));
                        else
                            v.Add(new Vertex(132 + i * 65, 100 + j * 65));
                    }
                }
                for (int i = 0; i < n; i++)
                {
                    if ((i + 1) % h != 0 && i + 1 < n)
                        u.Add(new Edge(i, i + 1, 1));
                    if (i + h < n)
                        u.Add(new Edge(i, i + h, 1));
                    if ((i / h) % 2 == 0)
                    {
                        if ((i + h - 1) % h != h - 1 && i + h - 1 < n)
                            u.Add(new Edge(i, i + h - 1, 1));
                    }
                    else
                    {
                        if ((i + h + 1) % h != 0 && i + h + 1 < n)
                            u.Add(new Edge(i, i + h + 1, 1));
                    }
                }
            }
            foreach (Edge ed in u)
            {
                M[ed.id_in, ed.id_out] = ed.weight;
                M[ed.id_out, ed.id_in] = ed.weight;
            }
        }
        //public Graph(float[,] Matrix,float[] array)
        //{
        //
        //}
        public void AddVertex(Vertex vert)
        {
            v.Add(vert);
            n = v.Count();
        }
        public void RemoveVertex(int _id)
        {
            for (int i = 0; i < u.Count(); i++)
            {
                if (u[i].id_in == _id || u[i].id_out == _id)
                {
                    u.RemoveAt(i);
                    i--;
                }
            }
            for (int i = 0; i < u.Count(); i++)
            {
                if (u[i].id_in > _id)
                {
                    u[i].id_in--;
                }
                if (u[i].id_out > _id)
                {
                    u[i].id_out--;
                }
            }
            v.RemoveAt(_id);
            M = new float[n, n];
            foreach (Edge ed in u)
            {
                M[ed.id_in, ed.id_out] = ed.weight;
                M[ed.id_out, ed.id_in] = ed.weight;
            }
        }
        public void AddEdge(int id1, int id2, float MaxThread)
        {
            //проверить, нет ли уже такой дуги
            Edge t = new Edge(id1, id2, MaxThread);
            bool flag = true;
            foreach (Edge eee in u)
                if (eee.id_in == t.id_in && eee.id_out == t.id_out)
                {
                    flag = false;
                    break;
                }
            if (flag)
                u.Add(t);
            n = v.Count();
            M = new float[n, n];
            foreach (Edge ed in u)
            {
                M[ed.id_in, ed.id_out] = ed.weight;
                M[ed.id_out, ed.id_in] = ed.weight;
            }
        }
        public void EditEdge(int id1, int id2, float newValue)
        {
            u.Remove(u.Find(ed => ((ed.id_in == id1 && ed.id_out == id2) || (ed.id_in == id2 && ed.id_out == id1))));
            M[id1, id2] = 0;
            M[id2, id1] = 0;
            if (newValue != 0)
            {
                AddEdge(id2, id1, newValue);
                M[id1, id2] = newValue;
                M[id2, id1] = newValue;
            }
        }
        public void Show(Graphics g)
        {
            foreach (Edge x in u)
            {
                g.DrawLine(new Pen(Color.Black, 1), v[x.id_in].x, v[x.id_in].y, v[x.id_out].x, v[x.id_out].y);
                //float dy = 0, dx = 0, lngthKoef = 0;
                //if ((v[x.id_in].x > v[x.id_out].x && v[x.id_in].y > v[x.id_out].y) ||
                //        (v[x.id_out].x > v[x.id_in].x && v[x.id_out].y > v[x.id_in].y))//если цифры будут заграждаться дугой
                //{
                //    dx = Math.Abs(v[x.id_in].x - v[x.id_out].x);
                //    dy = Math.Abs(v[x.id_in].y - v[x.id_out].y);
                //    lngthKoef = 50.0f / (float)Math.Sqrt(dx * dx + dy * dy);
                //}
                //g.DrawString(x.weight.ToString() /*+ ", " + x.thread.ToString()*/,
                //    new Font("Arial", 12),
                //    new SolidBrush(Color.Black),
                //    (v[x.id_in].x + v[x.id_out].x) / 2, (v[x.id_in].y + v[x.id_out].y - dx * (lngthKoef)) / 2);
                //стрелочки:
                //drawArrow(new PointF(v[x.id_in].x, v[x.id_in].y),
                //    new PointF(v[x.id_out].x, v[x.id_out].y), Vertex.R, g, Color.Black);

            }
            int i = 0, n = v.Count();
            foreach (Vertex x in v)
            {
                //string text = i == 0 ? "s" : (i == n - 1 ? "t" : i.ToString());
                string text = i.ToString();
                x.Draw(g, text);
                i++;
            }
        }
        public void Show(Graphics g, bool[] activeted_v, bool[] activated_u)//v=vertex, u=edges
        {
            int i = 0;

            foreach (Edge x in u)
            {
                if (!activated_u[i])
                {
                    g.DrawLine(new Pen(Color.Black, 1), v[x.id_in].x, v[x.id_in].y, v[x.id_out].x, v[x.id_out].y);
                    float dy = 0, dx = 0, lngthKoef = 0;
                    if ((v[x.id_in].x > v[x.id_out].x && v[x.id_in].y > v[x.id_out].y) ||
                            (v[x.id_out].x > v[x.id_in].x && v[x.id_out].y > v[x.id_in].y))//если цифры будут заграждаться дугой
                    {
                        dx = Math.Abs(v[x.id_in].x - v[x.id_out].x);
                        dy = Math.Abs(v[x.id_in].y - v[x.id_out].y);
                        lngthKoef = 50.0f / (float)Math.Sqrt(dx * dx + dy * dy);
                    }
                    g.DrawString(x.weight.ToString() /*+ ", " + x.thread.ToString()*/,
                        new Font("Arial", 12),
                        new SolidBrush(Color.Black),
                        (v[x.id_in].x + v[x.id_out].x) / 2, (v[x.id_in].y + v[x.id_out].y - dx * (lngthKoef)) / 2);
                    //стрелочки:
                    drawArrow(new PointF(v[x.id_in].x, v[x.id_in].y),
                        new PointF(v[x.id_out].x, v[x.id_out].y), Vertex.R, g, Color.Black);
                }
                i++;
            }
            i = 0;
            foreach (Edge x in u)
            {
                if (activated_u[i])
                {
                    Color col = Color.Red;
                    g.DrawLine(new Pen(col, 3), v[x.id_in].x, v[x.id_in].y, v[x.id_out].x, v[x.id_out].y);
                    float dy = 0, dx = 0, lngthKoef = 0;
                    if ((v[x.id_in].x > v[x.id_out].x && v[x.id_in].y > v[x.id_out].y) ||
                            (v[x.id_out].x > v[x.id_in].x && v[x.id_out].y > v[x.id_in].y))//если цифры будут заграждаться дугой
                    {
                        dx = Math.Abs(v[x.id_in].x - v[x.id_out].x);
                        dy = Math.Abs(v[x.id_in].y - v[x.id_out].y);
                        lngthKoef = 50.0f / (float)Math.Sqrt(dx * dx + dy * dy);
                    }
                    g.FillRectangle(new SolidBrush(Color.LightGray),
                            (v[x.id_in].x + v[x.id_out].x) / 2, (v[x.id_in].y + v[x.id_out].y - dx * (lngthKoef)) / 2,
                            (x.weight.ToString() /*+ ", " + x.thread.ToString()*/).Length * 9, 20);
                    g.DrawString(x.weight.ToString() /*+ ", " + x.thread.ToString()*/,
                        new Font("Arial", 12, FontStyle.Bold),
                        new SolidBrush(col),
                        (v[x.id_in].x + v[x.id_out].x) / 2, (v[x.id_in].y + v[x.id_out].y - dx * (lngthKoef)) / 2);
                    //стрелочки:
                    drawArrow(new PointF(v[x.id_in].x, v[x.id_in].y),
                        new PointF(v[x.id_out].x, v[x.id_out].y), Vertex.R, g, col);
                }
                i++;
            }
            n = v.Count();
            i = 0;
            foreach (Vertex x in v)
            {
                string text = i == 0 ? "s" : (i == n - 1 ? "t" : i.ToString());
                x.Draw(g, text, activeted_v[i]);
                i++;
            }
        }
        private void drawArrow(PointF A, PointF B, int r, Graphics g, Color col)
        {
            float dx = ((float)A.X - (float)B.X);
            float dy = ((float)A.Y - (float)B.Y);
            float lngth = (float)Math.Sqrt((dx * dx) + (dy * dy));
            B = new PointF(B.X - A.X, B.Y - A.Y);//для удобства коэфициента в параметрическом уравнении
            float k = 100.0f * (lngth - r) / lngth;//по пропорции находим, на каком проценте от длинны прямой будет конец стрелки
            //находим вектор-нормаль прямой и нормализируем ее
            float normX = B.Y, normY = -B.X;
            float someShit = (float)Math.Sqrt(normX * normX + normY * normY);
            normX = ((float)normX / (float)someShit);
            normY = ((float)normY / (float)someShit);
            float mult = 4.0f;//ширина стрелочки
            PointF N = new PointF((A.X + (float)B.X * 0.01f * k), (A.Y + (float)B.Y * 0.01f * k));
            PointF P = new PointF((A.X + (float)B.X * 0.01f * (k - 5)), (A.Y + (float)B.Y * 0.01f * (k - 5)));
            PointF K = new PointF(((P.X) + normX * mult), ((P.Y) + normY * mult));
            PointF L = new PointF(((P.X) - normX * mult), ((P.Y) - normY * mult));
            g.FillPolygon(new SolidBrush(col), new PointF[] { N, K, L });
        }
        public int checkSelection(float _x, float _y)
        {
            for (int i = 0; i < v.Count(); i++)
            {
                if (v[i].is_inside(_x, _y))
                {
                    return i;
                }
            }
            return -1;
        }
        public void MoveVertex(int _id, float _x, float _y)
        {
            if (_id >= 0)
            {
                v[_id].x = _x;
                v[_id].y = _y;
            }
        }
        public List<Vertex> FindPath(int from,int to,int Algorythm_id)
        {
            List<Vertex> answer = new List<Vertex>();
            if (Algorythm_id == 0)
            {
                List<int> path = Findpath_Deikstra(from,to);
                foreach (int i in path)
                    answer.Add(v[i]);
            }
            else if (Algorythm_id == 1)
            {
                answer = Findpath_A_star();
            }
            else if (Algorythm_id == 2)
            {

            }
            return answer;
        }
        private List<int> Findpath_Deikstra(int id_from,int id_to)
        {//предполагается, что граф c положительными дугами и не ориентирован
            List<int> path = new List<int>();
            //float[,] matr = new float[n, n];
            bool[] flag = new bool[n];
            float[] l = new float[n];
            int[] pth = new int[n];//в каждом элементе записываем индекс вершины, из которой был минимальный путь
            int cur_id;
            for (int i = 0; i < n; i++)
            {
                //for (int j = 0; j < n; j++)
                //{
                //    matr[i, j] = M[i, j];
                //}
                flag[i] = false;
                pth[i] = -1;
                l[i] = 65535;
            }
            l[id_from] = 0;
            pth[id_from] = -100;
            //flag[id_from] = true;
            cur_id = id_from;
            List<int> ids_to_visit=new List<int>();
            int imin = -1;
            do
            {
                float min = 65536;
                imin = -1;
                for(int i=0;i<n;i++)
                {
                    if(M[cur_id,i]!=0&&!flag[i])
                    {
                        if(!ids_to_visit.Contains(i))
                        {
                            ids_to_visit.Add(i);
                        }
                        if(imin==-1)
                        {
                            imin = i;
                        }
                        if (l[i] > l[cur_id] + M[cur_id, i])
                        {
                            l[i] = l[cur_id] + M[cur_id, i];
                            pth[i] = cur_id;
                        }
                        if (min >= l[i])
                        {
                            min = l[i];
                            imin = i;
                        }
                    }
                }
                if(imin==-1&& ids_to_visit.Count()>0)
                {
                    imin = ids_to_visit[0];
                    ids_to_visit.RemoveAt(0);
                }
                flag[cur_id] = true;
                cur_id = imin;
            }
            while (!flag[id_to]&&ids_to_visit.Count()>0);
            int k = id_to;
            path.Add(k);
            while(k!=id_from)
            {
                path.Add(pth[k]);
                k = pth[k];
            }
            path.Reverse();
            return path;
        }
        private List<Vertex> Findpath_A_star() { return null; }
        //private List<int> Findpath_A_star() { return null; }
    }
}
