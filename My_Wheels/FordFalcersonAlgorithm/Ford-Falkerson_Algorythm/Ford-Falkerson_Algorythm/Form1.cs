using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Ford_Falkerson_Algorythm
{
    public partial class Form1 : Form
    {
        Graphics g;
        Bitmap bit;
        bool ms_down,ms_downX, ms_downY;
        int selected_id;
        Graph graph;
        int circle_r=20;//круг для перевода из меню в рабочее положение
        float X_cur, Y_cur,X_cur_fixed, Y_cur_fixed;
        public Form1()
        {
            InitializeComponent();
            button4.Visible= checkBox1.Checked;
            button5.Visible= checkBox1.Checked;
            selected_id = -1;
            graph = new Graph(new Vertex[0], new Edge[0]);
            ms_down = false;
            ms_downX = false;
            ms_downY = false;
            if (pictureBox1.Width > 0 && pictureBox1.Height > 0)
            {
                bit = new Bitmap(pictureBox1.Width, pictureBox1.Height);
                g = Graphics.FromImage(bit);
            }
            Draw();
        }

        private void splitter1_MouseMove(object sender, MouseEventArgs e)
        {

        }

        private void splitter1_MouseUp(object sender, MouseEventArgs e)
        {

        }
        
        private void pictureBox1_MouseMove(object sender, MouseEventArgs e)
        {
            if(ms_down)
            {//проверяем, не нажато ли на вершину(для её перемещения)
                if(selected_id>=0)
                {
                    graph.MoveVertex(selected_id,e.X,e.Y);
                    if (checkBox1.Checked)
                    {
                        foreach (Graph gg in So.stepGraph)
                            gg.MoveVertex(selected_id, e.X, e.Y);
                    }
                }
            }
            X_cur = e.X;
            Y_cur = e.Y;
            Draw();
        }

        private void pictureBox1_MouseUp(object sender, MouseEventArgs e)
        {
            bool superFlag = false;
            if (ms_downX)
            {
                if (X_cur > 60)
                {
                    //добавляем новую вершину
                    graph.AddVertex(new Vertex(e.X, e.Y));
                    superFlag = true;
                }
            }
            ms_downX = false;
            if(ms_down)
            {
                if(selected_id>=0)
                {
                    if(X_cur<=60)
                    {
                        graph.RemoveVertex(selected_id);
                        superFlag = true;
                    }
                }
            }
            ms_down = false;
            if (ms_downY)
            {
                int t = graph.checkSelection(X_cur, Y_cur);
                if (t >= 0 && selected_id >= 0 && t != selected_id)//если проводим дугу вообще
                {
                    if (graph.u.Find(ed => ((ed.id_in == t && ed.id_out == selected_id) || (ed.id_in == selected_id && ed.id_out == t))) == null)
                    {//проводим новую дугу между вершинами
                        InputForm in_f = new InputForm();
                        in_f.Text = "Дуга (" + (selected_id == 0 ? "S" : selected_id.ToString()) +
                            "," + (t == graph.N - 1 ? "t" : t.ToString()) + ")";
                        in_f.ShowDialog();
                        float smth = in_f.Val;
                        if (smth != 0)
                            graph.AddEdge(selected_id, t, smth);
                    }
                    else//если поверх дуги рисуем еще одну = если редактируем дугу
                    {
                        InputForm in_f = new InputForm();
                        in_f.Text = "Редакция дуги (" + (selected_id == 0 ? "S" : selected_id.ToString()) +
                            "," + (t == graph.N - 1 ? "t" : t.ToString()) + ")";
                        in_f.ShowDialog();
                        float smth = in_f.Val;
                        graph.u.Remove(graph.u.Find(ed => ((ed.id_in == t && ed.id_out == selected_id) || (ed.id_in == selected_id && ed.id_out == t))));
                        if (smth != 0)
                        {
                            graph.AddEdge(selected_id, t, smth);
                        }
                    }
                    superFlag = true;
                }
            }
            ms_downX = false;
            ms_downY = false;
            selected_id = -1;
            double z = Math.Sqrt((X_cur - X_cur_fixed) * (X_cur - X_cur_fixed) + (Y_cur - Y_cur_fixed) * (Y_cur - Y_cur_fixed));
            if(z>5)
                contextMenuStrip1.Visible = false;
            if (checkBox1.Checked)
            {
                //if (graph.u.Count() != So.graph.u.Count() ||
                //    graph.v.Count() != So.graph.v.Count())//если изменили структуру графа
                if(superFlag==true)
                {
                    id_step = 0;
                    foreach (Edge ed in graph.u)
                        ed.thread = 0;
                    So.init(graph);
                    richTextBox1.Text = "";
                    So.Solve();
                }
                else//иначе - просто меняем расположение вершин...
                {
                    foreach (Graph gg in So.stepGraph)
                        for (int i = 0; i < graph.v.Count(); i++)
                        {
                            gg.v[i].x = graph.v[i].x;
                            gg.v[i].y = graph.v[i].y;
                        }
                }
            }
            Draw();
        }

        private void pictureBox1_MouseDown(object sender, MouseEventArgs e)
        {
            X_cur_fixed = e.X;
            Y_cur_fixed = e.Y;
            if (e.Button == MouseButtons.Left)
            {
                if (e.X > 60)
                {//тыкаем по графу
                    ms_down = true;
                    selected_id = graph.checkSelection(e.X, e.Y);
                }
                if (e.X < 60)
                {//тыкаем по меню
                    ms_downX = true;
                }
            }
            else if(e.Button==MouseButtons.Right)
            {
                if (e.X > 60)
                {//тыкаем по графу и проводим дугу
                    selected_id = graph.checkSelection(e.X, e.Y);
                    ms_downY = true;
                }
            }
         
        }

        private void pictureBox1_MouseLeave(object sender, EventArgs e)
        {
           // ms_enter = false;
        }

        private void pictureBox1_MouseEnter(object sender, EventArgs e)
        {
            //if (ms_down)
            //{
            //    ms_enter = true;
            //    label1.Text = "2";
            //}
        }

        private void button1_Click(object sender, EventArgs e)
        {
            About form = new About();
            form.ShowDialog();
        }

        private void button3_Click(object sender, EventArgs e)
        {//опции:
            //врядли:
            //радиус вешины default = 15;
            //цвет вешины;
            //да:
            //стереть всё
            panel3.Visible = !panel3.Visible;

        }

        private void panel3_MouseMove(object sender, MouseEventArgs e)
        {

        }

        private void panel3_MouseUp(object sender, MouseEventArgs e)
        {

        }

        private void button2_Click(object sender, EventArgs e)
        {
            foreach (Edge ed in graph.u)
                ed.thread = 0;
            So.init(graph);
            //richTextBox1.Text = So.ShowMatrix1();
            So.Solve();
            int ln = So.step.Count();
            richTextBox1.Text = "";
            for (int i = 0; i < ln; i++)
                richTextBox1.Text += So.step[i]+"\n";
            Draw();
            //richTextBox1.Text += So.ShowMatrix1();
            //g.DrawCurve(new Pen(Color.Black),new Point[] {new Point(100,100),
            //                                              new Point(200,100),
            //                                              new Point(200,200)});
        }

        private void button4_Click(object sender, EventArgs e)
        {
            if (id_step < So.step.Count())
            {
                richTextBox1.Text = "";
                for (int i = 0; i <= id_step; i++)
                    richTextBox1.Text += So.step[i] + "\n";
                id_step++;
                //if(id_step== So.step.Count()-1)
                //    richTextBox1.Text += So.step[id_step] + "\n";
            }
            Draw();
            if (richTextBox1.TextLength > 0) 
                richTextBox1.Select(richTextBox1.TextLength - 1, richTextBox1.TextLength);
            richTextBox1.ScrollToCaret();
        }

        private void button5_Click(object sender, EventArgs e)
        {
            if (id_step > 0)
            {
                richTextBox1.Text = "";
                id_step--;
                for (int i = 0; i < id_step; i++)
                    richTextBox1.Text += So.step[i] + "\n";
            }
            Draw();
            if (richTextBox1.TextLength > 0)
                richTextBox1.Select(richTextBox1.TextLength - 1, richTextBox1.TextLength);
            richTextBox1.ScrollToCaret();
        }

        private void button6_Click(object sender, EventArgs e)
        {//удалить граф
            graph = new Graph(new Vertex[0], new Edge[0]);
            id_step = 0;
            foreach (Edge ed in graph.u)
                ed.thread = 0;
            So.init(graph);
            So.Solve();
            richTextBox1.Text = "";
            Draw();
        }

        private void button7_Click(object sender, EventArgs e)
        {//удалить все вершины
            graph.u = new List<Edge>();
            id_step = 0;
            foreach (Edge ed in graph.u)
                ed.thread = 0;
            So.init(graph);
            So.Solve();
            richTextBox1.Text = "";
            Draw();
        }

        int id_step;
        private void checkBox1_CheckedChanged(object sender, EventArgs e)
        {
            button4.Visible= checkBox1.Checked;
            button5.Visible= checkBox1.Checked;
            id_step = 0;
            richTextBox1.Text = "";
            foreach (Edge ed in graph.u)
                ed.thread = 0;
            if (checkBox1.Checked)
            {
                So.init(graph);
                So.Solve();
            }
            Draw();
        }

        private void panel3_MouseDown(object sender, MouseEventArgs e)
        {

        }

        private void pictureBox1_SizeChanged(object sender, EventArgs e)
        {
            if(pictureBox1.Width>0&& pictureBox1.Height>0)
                bit = new Bitmap(pictureBox1.Width, pictureBox1.Height);
            g = Graphics.FromImage(bit);
            Draw();
        }

        public void Draw()
        {
            g.Clear(Color.White);
            g.DrawLine(new Pen(Color.Black), 60, 0, 60, pictureBox1.Height);
            g.FillRectangle(new SolidBrush(Color.LightBlue), 0, 0, 59, pictureBox1.Height);
            //g.DrawEllipse(new Pen(Color.Black), 10, 10, 40, 40);
            if (checkBox1.Checked)
            {
                if (id_step < So.stepGraph.Count())
                    So.stepGraph[id_step].Show(g, So.a_v[id_step], So.a_u[id_step]);
                else if (So.stepGraph.Count() > 0)
                    So.stepGraph[So.stepGraph.Count() - 1].Show(g);
                
                //    So.graph.Show(g);    
            }
            else
                graph.Show(g);
                //So.stepGraph[So.stepGraph.Count()-1].Show(g);
            if(ms_downX)
            {
                g.FillEllipse(new SolidBrush(Color.LightPink), X_cur - circle_r, Y_cur - circle_r, circle_r * 2, circle_r * 2);
            }
            if (ms_downY&&selected_id>=0)
            {
                g.DrawLine(new Pen(Color.Black), X_cur, Y_cur, graph.v[selected_id].x, graph.v[selected_id].y);
            }
            //g.DrawCurve(new Pen(Color.Black), new Point[] {new Point(100,100),
            //                                              new Point(200,100),
            //                                              new Point(200,200)});//no!
            pictureBox1.Image = bit;
        }
    }
}
