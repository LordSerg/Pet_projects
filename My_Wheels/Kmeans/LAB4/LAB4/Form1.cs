using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO;

namespace LAB4
{
    public partial class Form1 : Form
    {
        int numOfData;//кількість різних даних
        int critNum;//кількість критеріїв
        int klustNum;//кількість кластерів
        List<double[]> data;//дані, з якими ми працюємо
        Bitmap bit;
        int ph, pw;//висота та ширина pictureBox-а
        Graphics g;
        double[] From, To;//границі максимальних та мінімальних значень вибірки
        List<int> klusterId;
        Color textColor = Color.White;
        Color backColor = Color.Black;
        Color axisColor = Color.Orange;
        //Brush invisPointColor = new SolidBrush()
        Brush pointColor = new SolidBrush(Color.Red);
        Random rand = new Random();
        Brush[] klustCol = { new SolidBrush(Color.Red), new SolidBrush(Color.Green), new SolidBrush(Color.Blue), new SolidBrush(Color.FromArgb(100,100,100)),
                            new SolidBrush(Color.Violet), new SolidBrush(Color.Orange), new SolidBrush(Color.DarkMagenta), new SolidBrush( Color.Cyan) , new SolidBrush( Color.White),
                            new SolidBrush( Color.Brown), new SolidBrush( Color.Yellow), new SolidBrush( Color.BurlyWood), new SolidBrush( Color.Pink)};
        int i1, i2;//індекси критеріїв, що відображаються зараз
        double[,] center;//центри кластерів (н-вимірні, тому матрицею, де кожна строка - це вектор)
        //int selectedIndexForKNN=-1;
        int KNN=1;//кількість найближчих
        //bool is_KNN_Solved=false;
        List<d2> h2;
        List<d3> h3;
        List<d4> h4;
        List<d5> h5;
        List<d6> h6;
        double mult = 0.25;
        double RotMult = 0.25;
        List<CheckBox> chB;
        bool is_showCenter = true;
        public Form1()
        {
            InitializeComponent();
            panel2.Visible = false;
            panel2.Dock = DockStyle.None;
            panel4.Visible = true;
            panel4.Dock = DockStyle.Fill;
            ph = pictureBox1.Height;
            pw = pictureBox1.Width;
            bit = new Bitmap(pw, ph);
            g = Graphics.FromImage(bit);
            g.Clear(backColor);
            pictureBox1.Image = bit;
        }

        private void loadToolStripMenuItem_Click(object sender, EventArgs e)
        {//завантажити дані з файлу
            OpenFileDialog fd = new OpenFileDialog();
            fd.InitialDirectory = Environment.CurrentDirectory;
            fd.RestoreDirectory = true;
            if (fd.ShowDialog() == DialogResult.OK)
            {
                button1.Enabled = false;
                button2.Enabled = false;
                button3.Enabled = false;
                button4.Enabled = false;
                button5.Enabled = false;
                string filePath = fd.FileName;
                Stream file = fd.OpenFile();
                StreamReader reader = new StreamReader(file);
                string[] FileData = reader.ReadLine().Split();
                critNum = int.Parse(FileData[1]);
                label1.Text = "Кількість критеріїв = " + critNum;
                klustNum = int.Parse(FileData[2]);
                numericUpDown1.Value = klustNum;
                numOfData = int.Parse(FileData[0]);
                data = new List<double[]>();// double[numOfData, critNum];
                //checkNumOfColumns(numOfData);
                //double x, y;
                double[] tempData;
                for (int i = 0; i < numOfData; i++)
                {
                    FileData = reader.ReadLine().Split();
                    tempData = new double[critNum];
                    for (int j = 0; j < critNum; j++)
                    {
                        tempData[j] = double.Parse(FileData[j]);
                    }
                    data.Add(tempData);
                }
                comboBox1.Items.Clear();
                comboBox2.Items.Clear();
                for (int i = 0; i < critNum; i++)
                {
                    comboBox1.Items.Add("Критерій " + (i + 1));
                    comboBox2.Items.Add("Критерій " + (i + 1));
                }
                FindMaxMinOfData();
                Do_N_dImEnTiOn();
                comboBox1.SelectedIndex = 0;
                comboBox2.SelectedIndex = 1;
                //Draw();
                reader.Close();
                file.Close();
                groupBox1.Enabled = true;
                groupBox2.Enabled = true;
                groupBox3.Enabled = true;
                numericUpDown3.Maximum = numOfData - 1;
            }
        }

        private void generateToolStripMenuItem_Click(object sender, EventArgs e)
        {//згенерувати нові дані
            Form2 f = new Form2();
            f.ShowDialog();
            if (f.is_accept_data)
            {
                numOfData = f.NumOfData;
                critNum = f.NumOfCriterias;
                label1.Text = "Кількість критеріїв = " + critNum;
                klustNum = f.NumOfKlusters;
                numericUpDown1.Value = klustNum;
                data = new List<double[]>();
                bool is_ghauss = f.is_ghauss;
                comboBox1.Items.Clear();
                comboBox2.Items.Clear();
                for (int i = 0; i < critNum; i++)
                {
                    comboBox1.Items.Add("Критерій " + (i + 1));
                    comboBox2.Items.Add("Критерій " + (i + 1));
                }

                /*
                 double[] tempData;
                for (int i = 0; i < numOfData; i++)
                {
                    FileData = reader.ReadLine().Split();
                    tempData = new double[critNum];
                    for (int j = 0; j < critNum; j++)
                    {
                        tempData[j] = double.Parse(FileData[j]);
                    }
                    data.Add(tempData);
                }
                */
                double[] tempData;
                for (int i = 0; i < numOfData; i++)
                {
                    tempData = new double[critNum];
                    for (int j = 0; j < critNum; j++)
                    {
                        double from, to;
                        from = f.intervals[j].Item1;
                        to = f.intervals[j].Item2;
                        if (from > to)
                        {
                            from += to;
                            to = from - to;
                            from -= to;
                        }
                        int t = rand.Next(1, klustNum + 1);// (int)Math.Sqrt(klustNum);//temporary klustNum
                        double[] inters = new double[t + 1];
                        inters[0] = from;
                        inters[t] = to;
                        for (int k = 1; k < t; k++)
                        {
                            inters[k] = from + (to - from) * ((double)k / (double)t) +
                                GaussianDistribution((from - to) / (2 * t), (to - from) / (2 * t));
                        }
                        if (is_ghauss)
                        {//Gaussian distribution:
                            int p = rand.Next(0, t);
                            tempData[j] = GaussianDistribution(inters[p], inters[p + 1]);
                        }
                        else
                        {//Random:
                            tempData[j] = from + (to - from) * (1 - rand.NextDouble());
                        }
                    }
                    data.Add(tempData);
                }
                    groupBox1.Enabled = true;
                    groupBox2.Enabled = true;
                    groupBox3.Enabled = true;
                    numericUpDown3.Maximum = numOfData - 1;
                if (f.is_make_file)
                {//створюємо файл
                    string path = @"E:\_Лабораторки\_Інтелектуальний аналіз даних\лб\LAB3\LAB3\LAB3\bin\Debug\file1.data";
                    FileStream fs = File.Create(path);
                    fs.Close();

                    StreamWriter sw = new StreamWriter(path);
                    sw.WriteLine(numOfData + " " + critNum + " " + klustNum);
                    for (int i = 0; i < numOfData; i++)
                    {
                        string s = "";
                        for (int j = 0; j < critNum; j++)
                        {
                            s += data[i][j] + " ";
                        }
                        sw.WriteLine(s);
                    }
                    sw.Close();
                }
                FindMaxMinOfData();
                Do_N_dImEnTiOn();

                button1.Enabled = false;
                button2.Enabled = false;
                button3.Enabled = false;
                button4.Enabled = false;
                button5.Enabled = false;
                comboBox1.SelectedIndex = 0;
                comboBox2.SelectedIndex = 1;
            }
        }

        private void saveToolStripMenuItem_Click(object sender, EventArgs e)
        {//зберегти поточні дані
            string path = Environment.CurrentDirectory;
            SaveFileDialog sfd = new SaveFileDialog();
            sfd.InitialDirectory = Environment.CurrentDirectory;
            sfd.RestoreDirectory = true;
            if (sfd.ShowDialog() == DialogResult.OK)
            {
                path = sfd.FileName;
                FileStream fs = File.Create(path);
                fs.Close();

                StreamWriter sw = new StreamWriter(path);
                sw.WriteLine(numOfData + " " + critNum + " " + klustNum);
                for (int i = 0; i < numOfData; i++)
                {
                    string s = "";
                    for (int j = 0; j < critNum; j++)
                    {
                        s += data[i][j] + " ";
                    }
                    sw.WriteLine(s);
                }
                sw.Close();
            }
        }

        private void aboutToolStripMenuItem_Click(object sender, EventArgs e)
        {//про програму

        }

        private void button6_Click(object sender, EventArgs e)
        {//обрати початкові центри
            klusterId = new List<int>();
            button1.Enabled = true;
            button2.Enabled = true;
            button3.Enabled = true;
            button4.Enabled = true;
            button5.Enabled = true;
            CheckCheckBox();
            center = new double[klustNum, critNum];
            //обираємо центри як декілька об'єктів вибірки
            for(int i=0;i<klustNum;i++)
            {
                int t = rand.Next(i * numOfData / klustNum, (i + 1) * numOfData / klustNum);
                for(int j=0;j<critNum;j++)
                {
                    center[i, j] = data[t][j];
                }
            }
            //обираємо центри як проміжок між максимумом та мінімумом вибірки
            //for (int i = 0; i < klustNum; i++)
            //{
            //    for (int j = 0; j < critNum; j++)
            //    {
            //        //center[i, j] = From[j] + (To[j] - From[j]) * rand.NextDouble();
            //        center[i, j] = GaussianDistribution(From[j], To[j]);
            //    }
            //}
            for (int i = 0; i < numOfData; i++)
            {
                double min = dist_to_centroid(0, i);
                int imin = 0;
                for (int j = 1; j < klustNum; j++)
                {
                    double d = dist_to_centroid(j, i);
                    if (d < min)
                    {
                        imin = j;
                        min = d;
                    }
                }
                klusterId.Add(imin);
            }
            Draw();
        }

        private void button3_Click(object sender, EventArgs e)
        {//Зробити один крок
            int[] numsOfPointsInKluster = new int[klustNum];
            double[,] sumsOfPointsCoordInKluster = new double[klustNum, critNum];
            for (int i = 0; i < numOfData; i++)
            {
                double min = dist_to_centroid(0, i);
                int imin = 0;
                for (int j = 1; j < klustNum; j++)
                {
                    double d = dist_to_centroid(j, i);
                    if (d < min)
                    {
                        imin = j;
                        min = d;
                    }
                }
                klusterId[i] = imin;
                numsOfPointsInKluster[imin]++;
                for (int j = 0; j < critNum; j++)
                {
                    sumsOfPointsCoordInKluster[imin, j] += data[i][j];
                }
            }
            for (int i = 0; i < klustNum; i++)
            {
                if (numsOfPointsInKluster[i] != 0)
                    for (int j = 0; j < critNum; j++)
                    {
                        center[i, j] = sumsOfPointsCoordInKluster[i, j] / (double)numsOfPointsInKluster[i];
                    }
            }
            Draw();
        }

        private void button5_Click(object sender, EventArgs e)
        {//Розв'язати задачу
            int[] numsOfPointsInKluster = new int[klustNum];
            double[,] sumsOfPointsCoordInKluster = new double[klustNum, critNum];
            double[,] prev = new double[klustNum, critNum];
            int cnt = 0;
            while (cnt < 100)
            {
                numsOfPointsInKluster = new int[klustNum];
                sumsOfPointsCoordInKluster = new double[klustNum, critNum];
                prev = new double[klustNum, critNum];
                for (int i = 0; i < klustNum; i++)
                    for (int j = 0; j < critNum; j++)
                        prev[i, j] = center[i, j];
                for (int i = 0; i < numOfData; i++)
                {
                    double min = dist_to_centroid(0, i);
                    int imin = 0;
                    for (int j = 1; j < klustNum; j++)
                    {
                        double d = dist_to_centroid(j, i);
                        if (d < min)
                        {
                            imin = j;
                            min = d;
                        }
                    }
                    klusterId[i] = imin;
                    numsOfPointsInKluster[imin]++;
                    for (int j = 0; j < critNum; j++)
                    {
                        sumsOfPointsCoordInKluster[imin, j] += data[i][j];
                    }
                }
                for (int i = 0; i < klustNum; i++)
                {
                    if (numsOfPointsInKluster[i] != 0)
                        for (int j = 0; j < critNum; j++)
                        {
                            center[i, j] = sumsOfPointsCoordInKluster[i, j] / (double)numsOfPointsInKluster[i];
                        }
                }
                int t = 0;
                for (int i = 0; i < klustNum; i++)
                    for (int j = 0; j < critNum; j++)
                        if (Math.Abs(prev[i, j] - center[i, j]) > 0.000001)
                            t++;
                if (t == 0)
                    break;
            }
            Draw();
        }

        private void button1_Click(object sender, EventArgs e)
        {//Скинути розв'язок
            button1.Enabled = false;
            button2.Enabled = false;
            button3.Enabled = false;
            button4.Enabled = false;
            button5.Enabled = false;
            Draw();
        }

        private void checkBox14_CheckedChanged(object sender, EventArgs e)
        {//Показувати центри
            is_showCenter = checkBox14.Checked;
            Draw();
        }

        private void button2_Click(object sender, EventArgs e)
        {//Розв'язати для нової точки

            string[] str = Prompt.ShowDialog("Будь ласка, введіть " + critNum + " координати нової точки через пробіл", "Введіть координати нової точки").Split(' ');
            if (str.Length == critNum)
            {
                double[] arr = new double[critNum];
                for (int i = 0; i < critNum; i++)
                    arr[i] = Convert.ToDouble(str[i]);
                AddNewPoint_KNN(arr);
                FindMaxMinOfData();
                Draw();
            }
            else
            {
                MessageBox.Show("Ви ввели щось неправильно!");
            }
        }
        void AddNewPoint_KNN(double[]newElem)
        {//додаємо новий елемент та розв'язуємо задачу KNN
            //додаємо новий елемент 
            data.Add(newElem);
            //знаходимо всі відстані до інших точок вибірки
            List<(double, int)> distansesToNewPoint__index = new List<(double, int)>();
            for (int i = 0; i < numOfData; i++)
            {
                distansesToNewPoint__index.Add((dist_to_point(i, numOfData), i));
            }
            //сортуємо ці відстані за зростанням
            distansesToNewPoint__index.Sort(delegate ((double, int) x, (double, int) y)
            {
                if (x.Item1 == y.Item1) return 0;
                else if (x.Item1 < y.Item1) return -1;
                else return 1;
            });
            //рахуємо перші K відстаней на належність до якогось кластеру
            int[] numOfKlustNeighbours = new int[klustNum];
            for (int i = 0; i < KNN && i < numOfData; i++)
            {
                numOfKlustNeighbours[klusterId[distansesToNewPoint__index[i].Item2]]++;
            }
            //знаходимо, якого кластеру знайшлося більше
            int max = numOfKlustNeighbours[0];
            int imax = 0;
            for (int i = 0; i < klustNum; i++)
            {
                if (max < numOfKlustNeighbours[i])
                {
                    max = numOfKlustNeighbours[i];
                    imax = i;
                }
            }
            //присвоюємо новій точці належність до найбільш популярного кластеру
            klusterId.Add(imax);
            numOfData++;
            //reconvX(data[i][0], 0) * mult
            if (critNum == 2) h2.Add(new d2(reconvX(newElem[0], 0) * mult, reconvX(newElem[1], 1) * mult));
            if (critNum == 3) h3.Add(new d3(reconvX(newElem[0], 0) * mult, reconvX(newElem[1], 1) * mult, reconvX(newElem[2], 2) * mult));
            if (critNum == 4) h4.Add(new d4(reconvX(newElem[0], 0) * mult, reconvX(newElem[1], 1) * mult, reconvX(newElem[2], 2) * mult, reconvX(newElem[3], 3) * mult));
            if (critNum == 5) h5.Add(new d5(reconvX(newElem[0], 0) * mult, reconvX(newElem[1], 1) * mult, reconvX(newElem[2], 2) * mult, reconvX(newElem[3], 3) * mult, reconvX(newElem[4], 4) * mult));
            if (critNum == 6) h6.Add(new d6(reconvX(newElem[0], 0) * mult, reconvX(newElem[1], 1) * mult, reconvX(newElem[2], 2) * mult, reconvX(newElem[3], 3) * mult, reconvX(newElem[4], 4) * mult, reconvX(newElem[5], 5) * mult));
        }
        void AddNewPoint_KNN()
        {//додаємо новий елемент та розв'язуємо задачу KNN
            //додаємо новий елемент 
            double []newElem = new double[critNum];
            for (int i = 0; i < critNum; i++)
            {
                //newElem[i] = From[i] + (To[i] - From[i]) * rand.NextDouble();
                newElem[i] = GaussianDistribution(From[i], To[i]);
            } 
            data.Add(newElem);
            //знаходимо всі відстані до інших точок вибірки
            List<(double,int)> distansesToNewPoint__index = new List<(double,int)>();//
            for (int i = 0; i < numOfData; i++)
            {
                distansesToNewPoint__index.Add((dist_to_point(i, numOfData), i));
            }
            //сортуємо ці відстані за зростанням
            distansesToNewPoint__index.Sort(delegate ((double,int) x, (double, int) y)
            {
                if (x.Item1 ==y.Item1) return 0;
                else if (x.Item1 < y.Item1) return -1;
                else return 1;
            });
            //рахуємо перші K відстаней на належність до якогось кластеру
            int[] numOfKlustNeighbours = new int[klustNum];
            for (int i = 0; i < KNN && i < numOfData; i++)
            {
                numOfKlustNeighbours[klusterId[distansesToNewPoint__index[i].Item2]]++;
            }
            //знаходимо, якого кластеру знайшлося більше
            int max = numOfKlustNeighbours[0];
            int imax = 0;
            for (int i = 0; i < klustNum; i++)
            {
                if (max < numOfKlustNeighbours[i])
                {
                    max = numOfKlustNeighbours[i];
                    imax = i;
                }
            }
            //присвоюємо новій точці належність до найбільш популярного кластеру
            klusterId.Add(imax);
            numOfData++;
            //reconvX(data[i][0], 0) * mult
            if (critNum == 2) h2.Add(new d2(reconvX(newElem[0], 0) * mult, reconvX(newElem[1], 1) * mult)); 
            if (critNum == 3) h3.Add(new d3(reconvX(newElem[0], 0) * mult, reconvX(newElem[1], 1) * mult, reconvX(newElem[2], 2) * mult));
            if (critNum == 4) h4.Add(new d4(reconvX(newElem[0], 0) * mult, reconvX(newElem[1], 1) * mult, reconvX(newElem[2], 2) * mult, reconvX(newElem[3], 3) * mult));
            if (critNum == 5) h5.Add(new d5(reconvX(newElem[0], 0) * mult, reconvX(newElem[1], 1) * mult, reconvX(newElem[2], 2) * mult, reconvX(newElem[3], 3) * mult, reconvX(newElem[4], 4) * mult));
            if (critNum == 6) h6.Add(new d6(reconvX(newElem[0], 0) * mult, reconvX(newElem[1], 1) * mult, reconvX(newElem[2], 2) * mult, reconvX(newElem[3], 3) * mult, reconvX(newElem[4], 4) * mult, reconvX(newElem[5], 5) * mult));
            numericUpDown3.Maximum = numOfData - 1;
        }

        private void button4_Click(object sender, EventArgs e)
        {//Розв'язати для нових випадкових точок
            int numOfNew = (int)numericUpDown2.Value;
            for (int i = 0; i < numOfNew; i++)
            {
                AddNewPoint_KNN();
            }
            FindMaxMinOfData();
            Draw();
        }

        private void numericUpDown2_ValueChanged(object sender, EventArgs e)
        {//точка №
            //selectedIndexForKNN = (int)numericUpDown2.Value;
            //is_KNN_Solved = false;
            Draw();
        }

        private void numericUpDown3_ValueChanged(object sender, EventArgs e)
        {//кількість найближчих сусідів
            KNN = (int)numericUpDown3.Value;
            //is_KNN_Solved = false;
            Draw();
        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {//змінюємо критерій1
            if (comboBox1.SelectedIndex >= 0 && comboBox2.SelectedIndex >= 0)
            {
                i1 = comboBox1.SelectedIndex;
                i2 = comboBox2.SelectedIndex;
                Draw();
            }
        }

        private void comboBox2_SelectedIndexChanged(object sender, EventArgs e)
        {//змінюємо критерій2
            if (comboBox1.SelectedIndex >= 0 && comboBox2.SelectedIndex >= 0)
            {
                i1 = comboBox1.SelectedIndex;
                i2 = comboBox2.SelectedIndex;
                Draw();
            }
        }

        private void button8_Click(object sender, EventArgs e)
        {//скинути нД зображення
            Do_N_dImEnTiOn();
        }

        private void trackBar1_Scroll(object sender, EventArgs e)
        {//змінюємо швидкість обертання

        }

        private void checkBox1_CheckedChanged(object sender, EventArgs e)
        {//змінюємо видимість кластеру

        }

        private void trackBar22_Scroll(object sender, EventArgs e)
        {//змінюємо масштаб відображення
            mult = (double)1 / (double)trackBar22.Value;
            label4.Text = mult.ToString();
            Do_N_dImEnTiOn();
        }

        private void tabControl2_SelectedIndexChanged(object sender, EventArgs e)
        {//змінюємо точку зору
            if (tabControl2.SelectedIndex == 0)
            {
                panel2.Visible = false;
                panel2.Dock = DockStyle.None;
                panel4.Visible = true;
                panel4.Dock = DockStyle.Fill;
                timer1.Enabled = false;
                g = Graphics.FromImage(bit);
                g.Clear(backColor);
                pictureBox1.Image = bit;
                Draw();
            }
            else
            {
                panel2.Visible = true;
                panel2.Dock = DockStyle.Fill;
                panel4.Visible = false;
                panel4.Dock = DockStyle.None;
                Do_N_dImEnTiOn();
            }
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            DrawInND();
        }

        double convX(int x)
        {
            double xs = x;// = x - pw / 2;
            xs *= (double)(To[i1] - From[i1]) / (double)pw;
            xs += From[i1];
            //xs = To * xs/pw + From*(1-xs/pw);//=From+(To-From)*xs/pw
            return xs;
        }
        int reconvX(double x)
        {
            x -= From[i1];
            x /= (double)(To[i1] - From[i1]) / (double)pw;
            return (int)x;
        }
        int reconvX(double x, int diapazonIndex)
        {
            //x -= From[diapazonIndex];
            x /= (double)(To[diapazonIndex] - From[diapazonIndex]) / (double)pw;
            return (int)x;
        }
        double convY(int y)
        {
            double ys = y;// - pw / 2;
            ys = ph - ys;
            //ys *= -1;
            ys *= (double)(To[i2] - From[i2]) / (double)ph;
            ys += From[i2];
            return ys;
        }

        private void numericUpDown1_ValueChanged(object sender, EventArgs e)
        {
            klustNum = (int)numericUpDown1.Value;
            button1.Enabled = false;
            button2.Enabled = false;
            button3.Enabled = false;
            button4.Enabled = false;
            button5.Enabled = false;
            CheckCheckBox();
        }

        private void pictureBox1_SizeChanged(object sender, EventArgs e)
        {
            if (pictureBox1.Width > 0 && pictureBox1.Height > 0)
            {
                ph = pictureBox1.Height;
                pw = pictureBox1.Width;
                bit = new Bitmap(pw, ph);
                g = Graphics.FromImage(bit);
                if (comboBox1.SelectedIndex >= 0 && comboBox2.SelectedIndex >= 0)
                {
                    i1 = comboBox1.SelectedIndex;
                    i2 = comboBox2.SelectedIndex;
                    Draw();
                }
            }
        }

        private void pictureBox2_SizeChanged(object sender, EventArgs e)
        {
            if (pictureBox2.Width > 0 && pictureBox2.Height > 0)
            {
                if (tabControl2.SelectedIndex == 1)
                {
                    ph = pictureBox2.Height;
                    pw = pictureBox2.Width;
                    bit = new Bitmap(pw, ph);
                    g = Graphics.FromImage(bit);
                    g.Clear(backColor);
                    pictureBox2.Image = bit;
                    Draw();
                }
                //if (comboBox1.SelectedIndex >= 0 && comboBox2.SelectedIndex >= 0)
                //{
                //    i1 = comboBox1.SelectedIndex;
                //    i2 = comboBox2.SelectedIndex;
                //    Draw();
                //}
            }
        }

        int reconvY(double y)
        {
            y -= From[i2];
            y /= (double)(To[i2] - From[i2]) / (double)ph;
            //y *= -1;
            y = ph - y;
            return (int)y;
        }
        void FindMaxMinOfData()
        {
            From = new double[critNum];
            To = new double[critNum];
            for (int i = 0; i < critNum; i++)
            {
                From[i] = data[0][i];
                To[i] = data[0][i];
            }
            for (int i = 0; i < numOfData; i++)
            {
                for (int j = 0; j < critNum; j++)
                {
                    if (From[j] > data[i][j]) From[j] = data[i][j];
                    if (To[j] < data[i][j]) To[j] = data[i][j];
                }
            }
            for (int i = 0; i < critNum; i++)
            {
                double dx = Math.Abs(From[i] - To[i]) / 10.0d;
                From[i] -= dx;
                To[i] += dx;
                if (From[i] - To[i] < 0.00001)
                {
                    From[i] -= 0.5;
                    To[i] += 0.5;
                }
            }
        }
        double GaussianDistribution(double from, double to)
        {
            double sigma = 0.18;
            double mu = 0;
            //return (1 / (sigma * Math.Sqrt(2 * Math.PI))) * (Math.Pow(Math.E, (-1.0d / 2) * Math.Pow((x - mu) / sigma, 2)));
            double u1 = 1.0 - rand.NextDouble();
            double u2 = 1.0 - rand.NextDouble();

            double randStdNormal = Math.Sqrt(-2.0 * Math.Log(u1)) * Math.Sin(2.0 * Math.PI * u2);
            double randNormal = mu + sigma * randStdNormal;
            return from + (to - from) * (randNormal + 0.5);
        }
        void CheckCheckBox()
        {
            chB = new List<CheckBox> { checkBox1, checkBox2, checkBox3, checkBox4, checkBox5, checkBox6, checkBox7, checkBox8, checkBox9, checkBox10, checkBox11, checkBox12, checkBox13 };
            foreach (CheckBox ch in chB)
            {
                ch.Visible = false;
            }
            if (button3.Enabled)
            {
                for (int i = 0; i < klustNum; i++)
                {
                    chB[i].Visible = true;
                }
            }

        }
        void Draw()
        {
            if (tabControl2.SelectedIndex == 0)
            {
                DrawIn2D();
            }
            else if (critNum != 0)
            {
                DrawInND();
            }
        }
        void Do_N_dImEnTiOn()
        {
            var l = new List<TrackBar>{trackBar1,trackBar2, trackBar3, trackBar4, trackBar5, trackBar6, trackBar7, trackBar8, trackBar9, trackBar10
            , trackBar11,trackBar12, trackBar13, trackBar14, trackBar15, trackBar16, trackBar17, trackBar18, trackBar19, trackBar20, trackBar21};
            d2.reset();
            d3.reset();
            d4.reset();
            d5.reset();
            d6.reset();
            foreach (TrackBar track in l)
            {
                track.Visible = false;
                track.Value = 0;
            }

            if (critNum == 2)
            {//2d
                trackBar1.Visible = true;
                h2 = new List<d2>();
                for (int i = 0; i < numOfData; i++)
                    h2.Add(new d2(reconvX(data[i][0], 0) * mult, reconvX(data[i][1], 1) * mult));
            }
            else if (critNum == 3)
            {//3d
                trackBar1.Visible = true;
                trackBar2.Visible = true;
                trackBar3.Visible = true;
                h3 = new List<d3>();
                for (int i = 0; i < numOfData; i++)
                    h3.Add(new d3(reconvX(data[i][0], 0) * mult, reconvX(data[i][1], 1) * mult, reconvX(data[i][2], 2) * mult));
            }
            else if (critNum == 4)
            {//4d
                trackBar1.Visible = true;
                trackBar2.Visible = true;
                trackBar3.Visible = true;
                trackBar4.Visible = true;
                trackBar5.Visible = true;
                trackBar6.Visible = true;
                h4 = new List<d4>();
                for (int i = 0; i < numOfData; i++)
                    h4.Add(new d4(reconvX(data[i][0], 0) * mult, reconvX(data[i][1], 1) * mult, reconvX(data[i][2], 2) * mult, reconvX(data[i][3], 3) * mult));
            }
            else if (critNum == 5)
            {//5d
                trackBar1.Visible = true;
                trackBar2.Visible = true;
                trackBar3.Visible = true;
                trackBar4.Visible = true;
                trackBar5.Visible = true;
                trackBar6.Visible = true;
                trackBar7.Visible = true;
                trackBar8.Visible = true;
                trackBar9.Visible = true;
                trackBar10.Visible = true;
                h5 = new List<d5>();
                for (int i = 0; i < numOfData; i++)
                    h5.Add(new d5(reconvX(data[i][0], 0) * mult, reconvX(data[i][1], 1) * mult, reconvX(data[i][2], 2) * mult, reconvX(data[i][3], 3) * mult, reconvX(data[i][4], 4) * mult));
            }
            else if (critNum == 6)
            {//6d
                trackBar1.Visible = true;
                trackBar2.Visible = true;
                trackBar3.Visible = true;
                trackBar4.Visible = true;
                trackBar5.Visible = true;
                trackBar6.Visible = true;
                trackBar7.Visible = true;
                trackBar8.Visible = true;
                trackBar9.Visible = true;
                trackBar10.Visible = true;
                trackBar11.Visible = true;
                trackBar12.Visible = true;
                trackBar13.Visible = true;
                trackBar14.Visible = true;
                trackBar15.Visible = true;
                h6 = new List<d6>();
                for (int i = 0; i < numOfData; i++)
                    h6.Add(new d6(reconvX(data[i][0], 0) * mult, reconvX(data[i][1], 1) * mult, reconvX(data[i][2], 2) * mult, reconvX(data[i][3], 3) * mult, reconvX(data[i][4], 4) * mult, reconvX(data[i][5], 5) * mult));
            }
            timer1.Enabled = true;
        }
        void DrawIn2D()
        {
            //FindMaxMinOfData();
            g.Clear(backColor);
            double x, y;
            //From = -1;// Convert.ToDouble(textBox5.Text);
            //To = 13; //Convert.ToDouble(textBox4.Text);
            //вісі координат
            int k1 = pictureBox1.Width / 100, k2 = pictureBox1.Height / 100;
            for (double t = 0; t < To[i1]; t += (To[i1] - From[i1]) / k1)
            {
                x = ((t - From[i1]) * pictureBox1.Width / (To[i1] - From[i1]));
                g.DrawLine(new Pen(textColor), (int)x, 0, (int)x, pictureBox1.Height);
                g.DrawString(Math.Round(t, 2).ToString(), this.Font, new SolidBrush(textColor), (int)x, pictureBox1.Height - 20);
            }
            for (double t = 0; t > From[i1]; t -= (To[i1] - From[i1]) / k1)
            {
                x = ((t - From[i1]) * pictureBox1.Width / (To[i1] - From[i1]));
                g.DrawLine(new Pen(textColor), (int)x, 0, (int)x, pictureBox1.Height);
                g.DrawString(Math.Round(t, 2).ToString(), this.Font, new SolidBrush(textColor), (int)x, pictureBox1.Height - 20);
            }
            for (double t = 0; t < To[i2]; t += (To[i2] - From[i2]) / k2)
            {
                y = pictureBox1.Height - (t - From[i2]) * pictureBox1.Height / (To[i2] - From[i2]);
                g.DrawLine(new Pen(textColor), 0, (int)y, pictureBox1.Width, (int)y);
                g.DrawString(Math.Round(t, 2).ToString(), this.Font, new SolidBrush(textColor), 0, (int)y);
            }
            for (double t = 0; t > From[i2]; t -= (To[i2] - From[i2]) / k2)
            {
                y = pictureBox1.Height - (t - From[i2]) * pictureBox1.Height / (To[i2] - From[i2]);
                g.DrawLine(new Pen(textColor), 0, (int)y, pictureBox1.Width, (int)y);
                g.DrawString(Math.Round(t, 2).ToString(), this.Font, new SolidBrush(textColor), 0, (int)y);
            }
            if (From[i2] <= 0) g.DrawLine(new Pen(axisColor, 3), 0, reconvY(0), pw, reconvY(0));
            if (From[i1] <= 0) g.DrawLine(new Pen(axisColor, 3), reconvX(0), 0, reconvX(0), ph);

            //точки
            if (!button3.Enabled)
            {
                for (int i = 0; i < numOfData; i++)
                    g.FillEllipse(pointColor, reconvX(data[i][i1]), reconvY(data[i][i2]), 10, 10);
            }
            else
            {
                for (int i = 0; i < numOfData; i++)
                {
                    if (chB[klusterId[i]].Checked)
                        g.FillEllipse(klustCol[klusterId[i]], reconvX(data[i][i1]), reconvY(data[i][i2]), 10, 10);
                }
                if (is_showCenter)
                {
                    int radius1 = 10;
                    int radius2 = 20;
                    Point[] pts = new Point[10];
                    for (int i = 0; i < klustNum; i++)
                    {
                        if (chB[i].Checked)
                        {
                            for (int k = 0; k < 10; k += 2)
                            {
                                pts[k] = new Point(reconvX(center[i, i1]) + (int)(radius1 * Math.Cos(Math.PI * 2 * k / 10)),
                                    reconvY(center[i, i2]) + (int)(radius1 * Math.Sin(Math.PI * 2 * k / 10)));
                                pts[k + 1] = new Point(reconvX(center[i, i1]) + (int)(radius2 * Math.Cos(Math.PI * 2 * (k + 1) / 10)),
                                    reconvY(center[i, i2]) + (int)(radius2 * Math.Sin(Math.PI * 2 * (k + 1) / 10)));
                            }
                            g.FillPolygon(klustCol[i], pts);
                            g.DrawPolygon(new Pen(textColor), pts);
                        }
                    }
                }
            }
            pictureBox1.Image = bit;
        }

        void DrawInND()
        {
            if (critNum == 2)
            {//2d
                //d2.reset();
                g.Clear(backColor);
                d2.rotate(trackBar1.Value * RotMult);
                if (!button3.Enabled)
                {
                    for (int i = 0; i < numOfData; i++)
                        d2.draw_point(h2[i], pictureBox2, g, pointColor, false);
                }
                else
                {
                    for (int i = 0; i < numOfData; i++)
                    {
                        if (chB[klusterId[i]].Checked)
                            d2.draw_point(h2[i], pictureBox2, g, klustCol[klusterId[i]], false);
                    }
                    if (is_showCenter)
                        for (int i = 0; i < klustNum; i++)
                        {
                            if (chB[i].Checked)
                            {
                                d2 cent = new d2(reconvX(center[i, 0], 0) * mult, reconvX(center[i, 1], 1) * mult);
                                cent.Size = 15;
                                d2.draw_point(cent, pictureBox2, g, klustCol[i], true);
                            }
                        }
                }
                d2.draw_basis(pictureBox2, g);
            }
            else if (critNum == 3)
            {//3d
                //d3.reset();
                g.Clear(backColor);
                d3.rotate(trackBar3.Value * RotMult,
                          trackBar2.Value * RotMult, 
                          trackBar1.Value * RotMult);
                if (!button3.Enabled)
                {
                    for (int i = 0; i < numOfData; i++)
                        d3.draw_point(h3[i], pictureBox2, g, pointColor, false);
                }
                else
                {
                    for (int i = 0; i < numOfData; i++)
                    {
                        if (chB[klusterId[i]].Checked)
                            d3.draw_point(h3[i], pictureBox2, g, klustCol[klusterId[i]], false);
                    }
                    if (is_showCenter)
                        for (int i = 0; i < klustNum; i++)
                        {
                            if (chB[i].Checked)
                            {
                                d3 cent = new d3(reconvX(center[i, 0], 0) * mult, reconvX(center[i, 1], 1) * mult, reconvX(center[i, 2], 2) * mult);
                                cent.Size = 15;
                                d3.draw_point(cent, pictureBox2, g, klustCol[i], true);
                            }
                        }
                }
                d3.draw_basis(pictureBox2, g);
            }
            else if (critNum == 4)
            {//4d
                //d4.reset();
                g.Clear(backColor);
                d4.rotate(trackBar6.Value * RotMult,
                          trackBar5.Value * RotMult,
                          trackBar4.Value * RotMult,
                          trackBar3.Value * RotMult,
                          trackBar2.Value * RotMult,
                          trackBar1.Value * RotMult);
                if (!button3.Enabled)
                {
                    for (int i = 0; i < numOfData; i++)
                        d4.draw_point(h4[i], pictureBox2, g, pointColor, false);
                }
                else
                {
                    for (int i = 0; i < numOfData; i++)
                    {
                        if (chB[klusterId[i]].Checked)
                            d4.draw_point(h4[i], pictureBox2, g, klustCol[klusterId[i]], false);
                    }
                    if (is_showCenter)
                        for (int i = 0; i < klustNum; i++)
                        {
                            if (chB[i].Checked)
                            {
                                d4 cent = new d4(reconvX(center[i, 0], 0) * mult, reconvX(center[i, 1], 1) * mult, reconvX(center[i, 2], 2) * mult, reconvX(center[i, 3], 3) * mult);
                                cent.Size = 15;
                                d4.draw_point(cent, pictureBox2, g, klustCol[i], true);
                            }
                        }
                }
                d4.draw_basis(pictureBox2, g);
            }
            else if (critNum == 5)
            {//5d
                //d5.reset();
                g.Clear(backColor);
                d5.rotate(trackBar10.Value * RotMult,
                          trackBar9.Value * RotMult,
                          trackBar8.Value * RotMult,
                          trackBar7.Value * RotMult,
                          trackBar6.Value * RotMult,
                          trackBar5.Value * RotMult,
                          trackBar4.Value * RotMult,
                          trackBar3.Value * RotMult,
                          trackBar2.Value * RotMult,
                          trackBar1.Value * RotMult);
                if (!button3.Enabled)
                {
                    for (int i = 0; i < numOfData; i++)
                        d5.draw_point(h5[i], pictureBox2, g, pointColor, false);
                }
                else
                {
                    for (int i = 0; i < numOfData; i++)
                    {
                        if (chB[klusterId[i]].Checked)
                            d5.draw_point(h5[i], pictureBox2, g, klustCol[klusterId[i]], false);
                    }
                    if (is_showCenter)
                        for (int i = 0; i < klustNum; i++)
                        {
                            if (chB[i].Checked)
                            {
                                d5 cent = new d5(reconvX(center[i, 0], 0) * mult, reconvX(center[i, 1], 1) * mult, reconvX(center[i, 2], 2) * mult, reconvX(center[i, 3], 3) * mult, reconvX(center[i, 4], 4) * mult);
                                cent.Size = 15;
                                d5.draw_point(cent, pictureBox2, g, klustCol[i], true);
                            }
                        }
                }
                d5.draw_basis(pictureBox2, g);
            }
            else if (critNum == 6)
            {
                g.Clear(backColor);
                d6.rotate(trackBar15.Value * RotMult,
                          trackBar14.Value * RotMult,
                          trackBar13.Value * RotMult,
                          trackBar12.Value * RotMult,
                          trackBar11.Value * RotMult,
                          trackBar10.Value * RotMult,
                          trackBar9.Value * RotMult,
                          trackBar8.Value * RotMult,
                          trackBar7.Value * RotMult,
                          trackBar6.Value * RotMult,
                          trackBar5.Value * RotMult,
                          trackBar4.Value * RotMult,
                          trackBar3.Value * RotMult,
                          trackBar2.Value * RotMult,
                          trackBar1.Value * RotMult);
                if (!button3.Enabled)
                {
                    for (int i = 0; i < numOfData; i++)
                        d6.draw_point(h6[i], pictureBox2, g, pointColor, false);
                }
                else
                {
                    for (int i = 0; i < numOfData; i++)
                    {
                        if (chB[klusterId[i]].Checked)
                            d6.draw_point(h6[i], pictureBox2, g, klustCol[klusterId[i]], false);
                    }
                    if (is_showCenter)
                        for (int i = 0; i < klustNum; i++)
                        {
                            if (chB[i].Checked)
                            {
                                d6 cent = new d6(reconvX(center[i, 0], 0) * mult, reconvX(center[i, 1], 1) * mult, reconvX(center[i, 2], 2) * mult, reconvX(center[i, 3], 3) * mult, reconvX(center[i, 4], 4) * mult, reconvX(center[i, 5], 5) * mult);
                                cent.Size = 15;
                                d6.draw_point(cent, pictureBox2, g, klustCol[i], true);
                            }
                        }
                }
                d6.draw_basis(pictureBox2, g);
            }
            pictureBox2.Image = bit;
        }
        double dist_to_centroid(int center_index, int point_index)
        {
            double answer = 0;
            for (int i = 0; i < critNum; i++)
            {
                answer += Math.Pow(center[center_index, i] - data[point_index][i], 2);
            }
            return Math.Sqrt(answer);
            //return answer;
        }
        double dist_to_point(int p_from_index, int p_to_index)
        {
            double answer = 0;
            for (int i = 0; i < critNum; i++)
            {
                answer += Math.Pow(data[p_from_index][i] - data[p_to_index][i], 2);
            }
            return Math.Sqrt(answer);
            //return answer;
        }
    }
}
