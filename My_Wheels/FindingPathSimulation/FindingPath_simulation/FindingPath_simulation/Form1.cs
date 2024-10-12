using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using GraphLibrary;

namespace FindingPath_simulation
{
    public partial class Form1 : Form
    {
        Graphics g;
        Bitmap bit;
        Graph graph;
        bool edit_mode, ms_down, ms_downX, ms_downY, ms_downZ;
        float X_cur, Y_cur, X_cur_fixed, Y_cur_fixed;
        int circle_r = 20;
        int selected_id;
        public Form1()
        {
            InitializeComponent();
            bit = new Bitmap(pictureBox1.Width,pictureBox1.Height);
            g = Graphics.FromImage(bit);
            comboBox1.Items.Clear();
            comboBox1.Items.Add("4-угольная без диагоналей");
            comboBox1.Items.Add("3-угольная");
            comboBox1.Items.Add("4-угольная");
            comboBox1.Items.Add("6-угольная");
            comboBox1.SelectedIndex = 1;
            comboBox2.Items.Clear();
            comboBox2.Items.Add("Дейкстры");
            //comboBox2.Items.Add("Беллмана — Форда");
            //comboBox2.Items.Add("А*");
            //comboBox2.Items.Add("Флойда — Уоршелла");
            //comboBox2.Items.Add("Алгоритм Джонсона");
            //comboBox2.Items.Add("Алгоритм Ли (волновой алгоритм)");
            //comboBox2.Items.Add("какой-то еще....");
            comboBox2.SelectedIndex = 0;
            timer1.Interval = 1;
            timer1.Enabled = false;
            edit_mode = true;
            graph = new Graph(new Vertex[0], new Edge[0]);
            ms_down = false;
            ms_downX = false;
            ms_downY = false;
            if (pictureBox1.Width > 0 && pictureBox1.Height > 0)
            {
                bit = new Bitmap(pictureBox1.Width, pictureBox1.Height);
                g = Graphics.FromImage(bit);
            }
            checkBox5.Checked = true;
        }

        private void button1_Click(object sender, EventArgs e)
        {//About
            About form = new About();
            form.ShowDialog();
        }

        private void button2_Click(object sender, EventArgs e)
        {//go/stop
            //if(button2.Text=="go")
            //{//запускаем процесс
            //    //...
            //    button2.Text = "stop";
            //}
            //else
            //{//останавливаем процесс
            //    //...
            //    button2.Text = "go";
            //}
        }

        private void checkBox5_CheckedChanged(object sender, EventArgs e)
        {//режим редакции графа вкл/выкл
            edit_mode = checkBox5.Checked;
            groupBox1.Visible = checkBox5.Checked;
            timer1.Enabled = !checkBox5.Checked;
            if (!checkBox5.Checked)
                Walker.init(graph);
            Draw();
        }

        private void checkBox1_CheckedChanged(object sender, EventArgs e)
        {//(не)показовать путь

        }

        private void checkBox2_CheckedChanged(object sender, EventArgs e)
        {//(не)показовать граф

        }

        private void checkBox3_CheckedChanged(object sender, EventArgs e)
        {//учитывать вес дуг

        }

        private void checkBox4_CheckedChanged(object sender, EventArgs e)
        {//учитывать вес вершин

        }

        private void comboBox2_SelectedIndexChanged(object sender, EventArgs e)
        {//алгоритм поиска

        }

        private void button3_Click(object sender, EventArgs e)
        {//сгенерировать новый граф
            graph = new Graph((int)numericUpDown1.Value, (int)numericUpDown2.Value, comboBox1.SelectedIndex);
            Draw();
        }

        private void pictureBox1_SizeChanged(object sender, EventArgs e)
        {
            if (pictureBox1.Width > 0 && pictureBox1.Height > 0)
            {
                bit = new Bitmap(pictureBox1.Width, pictureBox1.Height);
                g = Graphics.FromImage(bit);
            }
            Draw();
        }

        private void pictureBox1_MouseMove(object sender, MouseEventArgs e)
        {
            if (ms_down)
            {//проверяем, не нажато ли на вершину(для её перемещения)
                if (selected_id >= 0)
                {
                    graph.MoveVertex(selected_id, e.X, e.Y);
                    //if (checkBox1.Checked)
                    //{
                    //    foreach (Graph gg in So.stepGraph)
                    //        gg.MoveVertex(selected_id, e.X, e.Y);
                    //}
                }
            }
            X_cur = e.X;
            Y_cur = e.Y;
            if(edit_mode)
                Draw();
        }

        private void pictureBox1_MouseDown(object sender, MouseEventArgs e)
        {
            X_cur_fixed = e.X;
            Y_cur_fixed = e.Y;
            if (e.Button == MouseButtons.Left)
            {
                if (edit_mode)
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
                else
                {
                    ms_down = true;
                    selected_id = graph.checkSelection(e.X, e.Y);
                }
            }
            else if (e.Button == MouseButtons.Right)
            {
                if (e.X > 60 && edit_mode)
                {//тыкаем по графу и проводим дугу
                    selected_id = graph.checkSelection(e.X, e.Y);
                    ms_downY = true;
                }
                if(!edit_mode)
                {
                    selected_id = graph.checkSelection(e.X, e.Y);
                    ms_downZ = true;
                }
            }
        }

        private void pictureBox1_MouseUp(object sender, MouseEventArgs e)
        {
            bool superFlag = false;
            if (ms_downX && edit_mode)
            {
                if (X_cur > 60)
                {
                    //добавляем новую вершину
                    graph.AddVertex(new Vertex(e.X, e.Y));
                    superFlag = true;
                }
            }
            ms_downX = false;
            if (ms_down)
            {
                if (selected_id >= 0)
                {
                    if (X_cur <= 60 && edit_mode)
                    {
                        graph.RemoveVertex(selected_id);
                        //Walker.graph_local = graph;
                        superFlag = true;
                    }
                }
            }
            ms_down = false;
            if (ms_downY && edit_mode)
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
                        graph.EditEdge(t,selected_id,smth);
                        //graph.u.Remove(graph.u.Find(ed => ((ed.id_in == t && ed.id_out == selected_id) || (ed.id_in == selected_id && ed.id_out == t))));
                        //if (smth != 0)
                        //{
                        //    graph.AddEdge(selected_id, t, smth);
                        //}
                    }
                    superFlag = true;
                }
            }
            if(ms_downZ)
            {//хотим пойти сюда
                if(selected_id>=0&& graph.checkSelection(e.X, e.Y)==selected_id)
                {
                    Walker.SetPath(graph.FindPath(Walker.CurID,selected_id,comboBox2.SelectedIndex));
                }
            }
            ms_downX = false;
            ms_downY = false;
            ms_downZ = false;
            selected_id = -1;
            double z = Math.Sqrt((X_cur - X_cur_fixed) * (X_cur - X_cur_fixed) + (Y_cur - Y_cur_fixed) * (Y_cur - Y_cur_fixed));
            if (z > 5)
                contextMenuStrip1.Visible = false;
            //if (checkBox1.Checked)
            //{
            //    //if (graph.u.Count() != So.graph.u.Count() ||
            //    //    graph.v.Count() != So.graph.v.Count())//если изменили структуру графа
            //    if (superFlag == true)
            //    {
            //        id_step = 0;
            //        foreach (Edge ed in graph.u)
            //            ed.thread = 0;
            //        So.init(graph);
            //        richTextBox1.Text = "";
            //        So.Solve();
            //    }
            //    else//иначе - просто меняем расположение вершин...
            //    {
            //        foreach (Graph gg in So.stepGraph)
            //            for (int i = 0; i < graph.v.Count(); i++)
            //            {
            //                gg.v[i].x = graph.v[i].x;
            //                gg.v[i].y = graph.v[i].y;
            //            }
            //    }
            //}
            Draw();
        }

        public void Draw()
        {
            g.Clear(Color.Silver);
            graph.Show(g);
            if (edit_mode)
            {
                g.FillRectangle(new SolidBrush(Color.LightBlue), 0, 0, 60, pictureBox1.Height);
            }
            else
            {
                Walker.Show(g);
            }
            if (ms_downX)
            {
                g.FillEllipse(new SolidBrush(Color.LightPink), X_cur - circle_r, Y_cur - circle_r, circle_r * 2, circle_r * 2);
            }
            if (ms_downY && selected_id >= 0)
            {
                g.DrawLine(new Pen(Color.Black), X_cur, Y_cur, graph.v[selected_id].x, graph.v[selected_id].y);
            }
            pictureBox1.Image = bit;
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            Walker.Go();
            Draw();
        }
    }
}
