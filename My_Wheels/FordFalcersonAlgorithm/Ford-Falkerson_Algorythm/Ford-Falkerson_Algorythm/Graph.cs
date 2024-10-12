using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.Drawing;

namespace Ford_Falkerson_Algorythm
{
    class Edge
    {
        public int id_in,id_out;
        public float weight, thread;
        //float x1, x2, y1, y2;
        public Edge(int _in, int _out, float _weight)
        {
            id_in = _in;
            id_out = _out;
            weight = _weight;//максимальный поток
            thread = 0;//текущий поток
            //x1 = _in.x;
            //x2 = _out.x;
            //y1 = _in.y;
            //y2 = _out.y;
        }
        public Edge(Edge ed)
        {
            id_in = ed.id_in;
            id_out = ed.id_out;
            weight = ed.weight;
            thread = ed.thread;
        }
        //public void Draw(Graphics g)
        //{
        //    g.DrawLine(new Pen(Color.Black), x1, y1, x2, y2);
        //}
    }
    class Vertex
    {
        public static int R 
        {
            get { return r; } 
        }
        private static int r = 15;
        public float x { get; set; }
        public float y { get; set; }
        //public static int id=-1;
        //int curr_id;
        public Vertex(float _x, float _y)
        {
            //id++;
            //curr_id = id;
            x = _x;
            y = _y;
        }
        public Vertex(Vertex V)
        {
            x = V.x;
            y = V.y;
        }
        public void Draw(Graphics g, string text)
        {
            //g.DrawEllipse(new Pen(Color.Black), x - r, y - r, r*2, r*2);
            g.FillEllipse(new SolidBrush(Color.LightPink), x - r, y - r, r*2, r*2);
            g.DrawString(text, new Font("Arial", 12), new SolidBrush(Color.Black), x-r/2, y-r/2);
        }
        public void Draw(Graphics g, string text,bool is_activ)
        {
            //g.DrawEllipse(new Pen(Color.Black), x - r, y - r, r*2, r*2);
            if (is_activ)
            {
                g.FillEllipse(new SolidBrush(Color.LightGreen), x - r, y - r, r * 2, r * 2);
                g.DrawString(text, new Font("Arial", 12), new SolidBrush(Color.Black), x - r / 2, y - r / 2);
            }
            else
            {
                g.FillEllipse(new SolidBrush(Color.LightPink), x - r, y - r, r * 2, r * 2);
                g.DrawString(text, new Font("Arial", 12), new SolidBrush(Color.Black), x - r / 2, y - r / 2);
            }
        }
        public bool is_inside(float _x, float _y)
        {
            return Math.Sqrt((x-_x)*(x-_x)+(y-_y)*(y-_y))<r;
        }
    }
    class Graph
    {
        public List<Vertex> v;
        public List<Edge> u;
        public int N { get { return v.Count(); } }
        //float[,] Mmax;//матрица для максимумов
        //float[,] Mcur;//матрица для текущей заполнености
        public Graph(Vertex[] vertices, Edge[] edges)
        {
            v = new List<Vertex>();
            u = new List<Edge>();
            foreach (Vertex x in vertices) v.Add(x);
            foreach (Edge x in edges) u.Add(x);
        }
        public Graph(Graph gr)
        {
            v = new List<Vertex>();
            u = new List<Edge>();
            foreach (Vertex x in gr.v) v.Add(new Vertex(x));
            foreach (Edge x in gr.u) u.Add(new Edge(x));
        }
        //public bool Is_enabled()
        //{//проверка, выполнены ли все условия (есть ли входной и выходной элемент)
        //
        //    return false;
        //}
        public void AddVertex(Vertex vert)
        {
            v.Add(vert);
        }
        //public void RemoveVertex(Vertex vert)
        //{
        //    v.Remove(vert);
        //}
        public void RemoveVertex(int _id)
        {
            for(int i=0;i< u.Count(); i++)
            {
                if(u[i].id_in==_id||u[i].id_out==_id)
                {
                    u.RemoveAt(i);
                    i--;
                }
            }
            for(int i=0;i< u.Count(); i++)
            {
                if (u[i].id_in > _id)
                {
                    u[i].id_in--;
                }
                if(u[i].id_out > _id)
                {
                    u[i].id_out--;
                }
            }
            v.RemoveAt(_id);
        }
        public void AddEdge(int id1,int id2,float MaxThread)
        {
            //проверить, нет ли уже такой дуги
            Edge t = new Edge(id1, id2, MaxThread);
            bool flag = true;
            foreach(Edge eee in u)
                if (eee.id_in == t.id_in && eee.id_out == t.id_out)
                {
                    flag = false;
                    break;
                }
            if (flag)
                u.Add(t);
        }
        //public void AddEdge(Edge edge)
        //{
        //    u.Add(edge);
        //}
        //public void RemoveEdge(Edge edge)
        //{
        //    u.Remove(edge);
        //}
        //public void RemoveEdge(int _id)
        //{
        //    u.RemoveAt(_id);
        //}

        //ТУДУ:
        //+удаление вершин так, чтобы удалялись и дуги
        //+редактирование весов дуг
        //удаление дуг (только дуг, без вершин)
        //+стрелочки на дугах
        //?возможность сделать дуги не прямыми, а именно дугами (шоб красиво было)
        public void Show(Graphics g)
        {
            foreach (Edge x in u)
            {
                g.DrawLine(new Pen(Color.Black,1), v[x.id_in].x, v[x.id_in].y, v[x.id_out].x, v[x.id_out].y);
                float dy=0, dx = 0,lngthKoef=0;
                if ((v[x.id_in].x > v[x.id_out].x && v[x.id_in].y > v[x.id_out].y) ||
                        (v[x.id_out].x > v[x.id_in].x && v[x.id_out].y > v[x.id_in].y))//если цифры будут заграждаться дугой
                {
                    dx = Math.Abs(v[x.id_in].x - v[x.id_out].x);
                    dy = Math.Abs(v[x.id_in].y - v[x.id_out].y);
                    lngthKoef = 50.0f/(float)Math.Sqrt(dx * dx + dy * dy);
                }
                g.DrawString(x.weight.ToString() + ", " + x.thread.ToString(),
                    new Font("Arial", 12),
                    new SolidBrush(Color.Black),
                    (v[x.id_in].x + v[x.id_out].x) / 2, (v[x.id_in].y + v[x.id_out].y - dx * (lngthKoef)) / 2);
                //стрелочки:
                drawArrow(new PointF(v[x.id_in].x, v[x.id_in].y),
                    new PointF(v[x.id_out].x, v[x.id_out].y),Vertex.R, g,Color.Black);
                
            }
            int i = 0,n=v.Count();
            foreach (Vertex x in v)
            {
                string text = i == 0 ? "s" : (i == n - 1 ? "t" : i.ToString());
                x.Draw(g, text);
                i++;
            }
        }
        public void Show(Graphics g,bool []activeted_v,bool []activated_u)//v=vertex, u=edges
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
                    g.DrawString(x.weight.ToString() + ", " + x.thread.ToString(),
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
                            (x.weight.ToString() + ", " + x.thread.ToString()).Length * 9, 20);
                    g.DrawString(x.weight.ToString() + ", " + x.thread.ToString(),
                        new Font("Arial", 12, FontStyle.Bold),
                        new SolidBrush(col),
                        (v[x.id_in].x + v[x.id_out].x) / 2, (v[x.id_in].y + v[x.id_out].y - dx * (lngthKoef)) / 2);
                    //стрелочки:
                    drawArrow(new PointF(v[x.id_in].x, v[x.id_in].y),
                        new PointF(v[x.id_out].x, v[x.id_out].y), Vertex.R, g, col);
                }
                i++;
            }
            int n = v.Count();
            i = 0;
            foreach (Vertex x in v)
            {
                string text = i == 0 ? "s" : (i == n - 1 ? "t" : i.ToString());
                x.Draw(g, text,activeted_v[i]);
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
            float normX = B.Y,normY=-B.X;
            float someShit = (float)Math.Sqrt(normX * normX + normY * normY);
            normX = ((float)normX / (float)someShit);
            normY = ((float)normY / (float)someShit);
            float mult = 4.0f;//ширина стрелочки
            PointF N = new PointF((A.X + (float)B.X * 0.01f * k), (A.Y + (float)B.Y * 0.01f * k));
            PointF P = new PointF((A.X + (float)B.X * 0.01f * (k-5)), (A.Y + (float)B.Y * 0.01f * (k-5)));
            PointF K = new PointF(((P.X) + normX*mult), ((P.Y) + normY* mult));
            PointF L = new PointF(((P.X) - normX * mult),((P.Y) - normY * mult));
            g.FillPolygon(new SolidBrush(col),new PointF[] {N,K,L});
        }
        public int checkSelection(float _x, float _y)
        {
            for(int i=0;i<v.Count();i++)
            {
                if(v[i].is_inside(_x,_y))
                {
                    return i;
                }
            }
            return -1;
        }
        public void MoveVertex(int _id, float _x, float _y)
        {
            if(_id>=0)
            {
                v[_id].x = _x;
                v[_id].y = _y;
            }
        }
    }
    static class So
    {//тут будет генерироваться решение (текст, перемещение и перекраска графа, каждый шаг алгоритма)
        public static List<string> step;//шаги алгоритма
        public static List<Graph> stepGraph;//состояние графа
        private static int n;
        private static int N { set { n = value; } }
        public static Graph graph;
        public static List<bool[]> a_v;//список использованых вершин для пошагового мода
        public static List<bool[]> a_u;//список использованых дуг для пошагового мода
        private static Graph graph_to_change;
        private static float[,] M0;//начальная матрица инцидентности
        private static float[,] M1;//тож самое, но эта матрица будет меняться на каждом шагу
        //показываем текущую матрицу:
        public static string ShowMatrix0()
        {
            string answer="";
            for (int i = 0; i < n; i++)
            {
                for (int j = 0; j < n; j++)
                    answer += M0[i,j].ToString()+" \t";
                answer += "\n";
            }
            return answer;
        }
        public static string ShowMatrix1()
        {
            string answer = "";
            for (int i = 0; i < n; i++)
            {
                for (int j = 0; j < n; j++)
                    answer += M1[i, j].ToString() + " \t";
                answer += "\n";
            }
            return answer;
        }
        //передаем граф:
        public static void init(Graph graph_in)
        {
            n = graph_in.N;
            M0 = new float[n, n];
            M1 = new float[n, n];
            a_v = new List<bool[]>();
            a_u = new List<bool[]>();
            step = new List<string>();
            stepGraph = new List<Graph>();
            for (int i = 0; i < n; i++)
                for (int j = 0; j < n; j++)
                {
                    M0[i, j] = 0;
                    M1[i, j] = 0;
                }
            foreach(Edge x in graph_in.u)
            {
                M1[x.id_in, x.id_out] = x.weight;
                M1[x.id_out, x.id_in] = -x.weight;
                M0[x.id_in, x.id_out] = 1;
                M0[x.id_out, x.id_in] = -1;
            }
            graph = new Graph(graph_in);
            graph_to_change = graph_in;
        }
        public static void Solve()
        {//решаем
            float answer = 0,try_answer=0;//ответ и проверочный ответ
            List<float> epsilons = new List<float>();//список эпсилонов
            stepGraph.Add(new Graph(graph));
            a_v.Add(new bool[graph.v.Count()]);
            a_u.Add(new bool[graph.u.Count()]);
            int counter = 1;
            //цикл1:
            //1. находим путь
            //1.1 если путь из s в t не получилось обнаружить, то надо определить, на какие кластеры разбилась (s,t)-сеть
            //    это можно реализовать обходом всех вершин вширину/вглубину: если за один обход не удалось обойти все вершины, то пройденые вершины - первый кластер, а оставшиеся - второй
            //2. находим минимум из всех пропускных способностей = находим эпсилон
            //3. в матрице М1 вычитаем* эпсилон их из весов дуг, которые были по пути
            //4. в самих дугах (которые в списке) - добавляем эпсилон к "заполненности"
            //5. записываем этот шаг в step
            //* - если шаг по дуге - вычитаем, если против дуги - добавляем
            while (true)
            {
                //bool[] act_u = new bool[n];
                //bool[] act_v = new bool[n];
                string answer_txt = counter+") S(S+,∞)\n";
                //1:
                List<Edge> path = FindNewPath();//обработать ошибки: две+ обратные дуги;
                                                //обработка, если поледняя дуга - обратная
                                                //обработка, если первая дуга - обратная
                bool []pathDirection;//массив в котором true-положительное направление false-отрицательное направление
                if (path.Count() == 0)
                {
                    //1.1:
                    //!!!!!!!!!!!!!
                    break;
                }
                //определяем направления дуг:
                pathDirection = new bool[path.Count()];
                for(int i=0;i<path.Count();i++)
                {
                    if(i==0)
                    {//обработка первой дуги
                        if (path[i].id_in != 0)
                            pathDirection[i] = false;
                        else
                            pathDirection[i] = true;
                    }
                    else if(i==path.Count()-1)
                    {//обработка последней дуги
                        if (path[i].id_out != n - 1)
                            pathDirection[i] = false;
                        else
                            pathDirection[i] = true;
                    }
                    else
                    {//обработка остальных дуг
                        if(pathDirection[i-1]==false)
                        {
                            if (path[i - 1].id_in == path[i].id_in)
                                pathDirection[i] = true;
                            else
                                pathDirection[i] = false;
                        }
                        else
                        {
                            if (path[i - 1].id_out == path[i].id_in)
                                pathDirection[i] = true;
                            else
                                pathDirection[i] = false;
                        }
                    }
                }
                //2:
                float min = float.MaxValue;
                for (int i = 0; i < path.Count(); i++)
                {
                    string way="+";
                    //if (path[i].weight > 0)
                    //if ((i == 0 || i == path.Count() - 1) ||
                    //    !(path[i].id_in == path[i + 1].id_in && path[i].id_out == path[i - 1].id_out))
                    if(pathDirection[i])
                    {
                        if (min > path[i].weight - path[i].thread)
                            min = path[i].weight - path[i].thread;
                        answer_txt += (path[i].id_out == n - 1 ? "t" : path[i].id_out.ToString()) +
                        "(" + (path[i].id_in == 0 ? "S" : (path[i].id_in.ToString()))
                        + way + ", " + min.ToString() + ")\n";
                    }
                    else//если обратная дуга, то макс. поток не меняем
                    {
                        way = "-";
                        if (path[i].thread > 0)
                        //if (min > path[i].weight + path[i].thread)
                        {
                            if (path[i].thread < min)
                                min = path[i].thread;
                        }
                        answer_txt += (path[i].id_in == n - 1 ? "t" : path[i].id_in.ToString()) +
                            "(" + (path[i].id_out == 0 ? "S" : (path[i].id_out.ToString()))
                            + way + ", " + min.ToString() + ")\n";
                    }
                }
                float epsilon = min;
                //if(epsilon==0)
                //{
                //    answer_txt += "Epsilon_" + counter + " = " + epsilon + "\n";
                //    step.Add(answer_txt);
                //    epsilons.Add(epsilon);
                //    break;
                //}
                //3:
                for (int i = 0; i < path.Count(); i++)
                {
                    M1[path[i].id_in, path[i].id_out] -= epsilon;
                    M1[path[i].id_out, path[i].id_in] += epsilon;

                }
                //4:
                for (int i = 0; i < path.Count(); i++)
                    for (int j = 0; j < graph.u.Count(); j++)
                    {
                        if (graph.u[j].id_in == path[i].id_in && graph.u[j].id_out == path[i].id_out)
                        {
                            //if (((i == 0 || i == path.Count() - 1) || !(path[i].id_in == path[i + 1].id_in && path[i].id_out == path[i - 1].id_out)))
                            if(pathDirection[i])
                            {//если прямая дуга -> +
                                graph.u[j].thread += epsilon;
                                graph_to_change.u[j].thread += epsilon;
                            }
                            else
                            {//если обратная -> -
                                graph.u[j].thread -= epsilon;
                                graph_to_change.u[j].thread -= epsilon;
                            }
                        }
                        //else if (graph.u[j].id_in == path[i].id_out &&
                        //         graph.u[j].id_out == path[i].id_in)
                        //    graph.u[j].thread -= epsilon;
                    }
                //5:
                answer_txt += "Epsilon_" + counter + " = " + epsilon + "\n";
                step.Add(answer_txt);
                stepGraph.Add(new Graph(graph));
                epsilons.Add(epsilon);
                counter++;
                if (counter > 100)
                    break;
            }
            //цикл2:
            //делаем проверку дуг: если вершины дуги (по М0) принадлежат и первому и второму кластеру, то добавляем ее пропускную споособность в результат
            //пожже

            //цикл3:
            //проверка результата - суммируем все эпсилонты, которые получились в ходе первого цикла
            foreach (float x in epsilons)
                try_answer += x;
            step.Add("Ответ: максимальный поток = "+try_answer);
            //если рез-ты второго и третьего цикла совпадают, то все хорошо.
            //иначе вывести большими буквами, что что-то не так                                                                                                                                                                                                                                                                                                                                 //вывести, что я далбаеб и все сделано хуево
        }
        private static List<Edge> FindNewPath()
        {
            List<int> path = FindNewPath(new List<int>(),new List<int> { 0});
            if(path.Count()==0||path[path.Count()-1]!=n-1)
            {
                return new List<Edge>();
            }
            bool[] act_u = new bool[graph.u.Count()];
            bool[] act_v = new bool[graph.v.Count()];
            for (int i = 0; i < path.Count(); i++)
                act_v[path[i]] = true;
            List<Edge> answer = new List<Edge>();
            for(int i=0;i<path.Count()-1;i++)
            {
                //Edge edg = graph.u.Find(ed => ed.id_in == path[i] && ed.id_out == path[i + 1]);
                answer.Add(graph.u.Find(ed => ed.id_in == path[i] && ed.id_out == path[i + 1]));
                if(answer[i]==null)
                {
                    answer.RemoveAt(i);
                    answer.Add(graph.u.Find(ed => ed.id_in == path[i+1] && ed.id_out == path[i]));
                    //if (answer[i] == null)
                    //{
                    //    //если программа зашла сюда - то где-то неправильно найден путь
                    //}
                }
                act_u[graph.u.IndexOf(answer[answer.Count()-1])]= true;
            }
            a_v.Add(act_v);
            a_u.Add(act_u);
            return answer;
        }

        private static List<int> FindNewPath(List<int> a, List<int> path)//список пройденых вершин
        {
            List<int> t = new List<int>();//для положительных дуг
            List<int> t1 = new List<int>();//для отрицательных дуг
            int i = path[path.Count() - 1];
            if (i == n - 1)
                return path;
            for (int j=0;j<n;j++)//ищем положительные ребра
            {
                //if((!a.Contains(j))&&(M1[i,j]!=0||M0[i,j]==-1))
                if((!a.Contains(j))&&(M1[i,j]>0))
                {
                    t.Add(j);
                }
            }
            for (int j = 0; j < n; j++)//ищем отрицательные ребра
            {
                if ((!a.Contains(j)) && (M1[i, j] < 0 || M0[i, j] == -1))//поиск ребра
                {
                    //если ребро не заполнено в обратную сторону:
                    Edge edg = graph.u.Find(ed => (ed.id_in == j && ed.id_out == i));
                    if (edg != null)
                        if (Math.Abs(edg.thread) > 0)
                            t1.Add(j);
                }
            }
            a.Add(i);
            if(t.Count()+t1.Count()==0)
            {
                path.RemoveAt(path.Count() - 1);
                return path;
            }
            
            t.Sort(delegate(int x,int y)
            {
                if (x > y) return -1;
                else return 1;
            });
            t1.Sort(delegate (int x, int y)
            {
                if (x > y) return -1;
                else return 1;
            });
            foreach (int m in t1)
                t.Add(m);
            List<int> savedPath = path;
            for (int k = 0; k < t.Count(); k++)
            {
                List <int>ans = savedPath;
                //if(!a.Contains(t[k]))
                ans.Add(t[k]);
                FindNewPath(a, ans);
                if (path.Count!=0&&path[path.Count() - 1] == n - 1)
                    break;
            }
            if (path[path.Count() - 1] != n - 1)
            {
                path.RemoveAt(path.Count() - 1);
                return path;
            }
            return path;
        }
        private static List<Vertex> is_one_claster()
        {//делаем поиск в ширину/глубину по М1 и если за обход прошлись не по всем вершинам, то вернуть набор вершин первого кластера

            //иначе - возвращаем пустое множество
            return null;//!!!!!!!!!!!
        }
    }
}
