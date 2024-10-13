using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Lab1
{
    public partial class Form1 : Form
    {
        Random random;
        public Form1()
        {
            InitializeComponent();
            comboBox1.Items.Add("Шифр Цезаря");
            comboBox1.Items.Add("Перестановочний шифр");
            comboBox2.Items.Add("Шифр Цезаря");
            comboBox2.Items.Add("Перестановочний шифр");
            random = new Random();
        }
        int[] KEY;
        private void Form1_SizeChanged(object sender, EventArgs e)
        {
            panel1.Width = (this.Width - 50) / 2;
            panel1.Location = new Point(10, panel1.Location.Y);
            panel2.Width = (this.Width - 50) / 2;
            panel2.Location = new Point(this.Width / 2, panel1.Location.Y);
            richTextBox1.Height = this.Height / 2 - 120;
            label4.Location = new Point(label4.Location.X, richTextBox1.Location.Y + richTextBox1.Height + 5);
            richTextBox2.Location = new Point(richTextBox2.Location.X, label4.Location.Y + label4.Height);
            richTextBox2.Height = this.Height / 2 - 120;
            richTextBox4.Height = this.Height / 2 - 120;
            label5.Location = new Point(label5.Location.X, richTextBox1.Location.Y + richTextBox1.Height + 5);
            richTextBox3.Location = new Point(richTextBox3.Location.X, label5.Location.Y + label5.Height);
            richTextBox3.Height = this.Height / 2 - 120;
        }

        private void button2_Click(object sender, EventArgs e)
        {//зашифрувати
            if (comboBox1.SelectedIndex == 0)
            {//Шифр Цезаря
                string in_str = richTextBox4.Text;
                richTextBox3.Text = CesarCode(in_str, (int)numericUpDown1.Value);
            }
            else if (comboBox1.SelectedIndex == 1)
            {//Перестановочний шифр
                int keyLength = (int)numericUpDown1.Value;
                int[] key = new int[keyLength];
                for (int i = 0; i < keyLength; i++)
                    key[i] = i + 1;
                for (int i = 0; i < 100; i++)
                {
                    int a = random.Next(0, keyLength);
                    int b = random.Next(0, keyLength);
                    if (a != b)
                    {
                        key[a] += key[b];
                        key[b] = key[a] - key[b];
                        key[a] -= key[b];
                    }
                }
                KEY = new int[keyLength];
                for (int i = 0; i < keyLength; i++)
                    KEY[i] = key[i];
                string in_str = richTextBox4.Text;
                richTextBox3.Text = "The key:\n";
                for (int i = 0; i < keyLength; i++)
                    richTextBox3.Text += key[i].ToString() + " ";
                richTextBox3.Text += "\nResult:\n";
                richTextBox3.Text += PermutationCode(in_str, key);
            }
            //for (int i = 0; i < 200; ++i)
            //    richTextBox3.Text += i.ToString() + ". " +(char)i+ "\n";
        }

        private void button1_Click(object sender, EventArgs e)
        {//розшифрувати
            if (comboBox2.SelectedIndex == 0)
            {//Шифр Цезаря
                string in_str = richTextBox1.Text;
                (string, int) ans = UnCesarCode_frequency(in_str);
                richTextBox2.Text = "Probably, the key is " + ans.Item2 + ";\n";
                richTextBox2.Text += "Result:\n";
                richTextBox2.Text += ans.Item1;
            }
            else if (comboBox2.SelectedIndex == 1)
            {//Перестановочний шифр
                if (KEY.Length > 0)
                {
                    richTextBox2.Text = "For Key:\n";
                    for (int i = 0; i < KEY.Length; i++)
                        richTextBox2.Text += KEY[i].ToString() + " ";
                    richTextBox2.Text += "Result:\n";
                    richTextBox2.Text += UnPermutationCode(richTextBox1.Text, KEY);
                }
            }
        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (comboBox1.SelectedIndex == 0)
            {
                label7.Visible = true;
                numericUpDown1.Visible = true;
                numericUpDown1.Maximum = 25;
            }
            else
            {
                label7.Visible = true;
                numericUpDown1.Visible = true;
                numericUpDown1.Maximum = 1000;
            }
        }
        int FindFirst(int[] arr, int whatWeLookFor)
        {
            for (int i = 0; i < arr.Length; i++)
                if (arr[i] == whatWeLookFor)
                    return i;
            return -1;
        }
        string PermutationCode(string text, int[] key)
        {
            int key_length = key.Length;
            int n = text.Length;
            string s = "abcdefghijklmnopqrstuvwxyz";
            string S = s.ToUpper();
            //text = text.ToLower();
            char[] a = text.ToArray();
            List<char>[] temp = new List<char>[key_length];//масив листів
            for (int i = 0; i < key_length; i++)
                temp[i] = new List<char>();
            int t = 0;
            for (int i = 0; i < n; i++)
                if (s.Contains(a[i]) || S.Contains(a[i]))
                {
                    temp[t % key_length].Add(a[i]);
                    t++;
                }
            for (; (t) % key_length != 0; t++)
                temp[t % key_length].Add(s[random.Next(0, 26)]);
            int[] tween_key = new int[key_length]; //індекси послідовних цифр у ключі
                                                   //      key = 6 3 2 4 1 5
                                                   //tween_key = 4 2 1 3 5 0
            for (int i = 0; i < key_length; i++)
                tween_key[i] = FindFirst(key, i + 1);
            StringBuilder answer = new StringBuilder();
            t = 0;
            for (int i = 0; i < key_length; i++)
                for (int j = 0; j < temp[i].Count; j++)
                {
                    if (text.Length > t)
                    {
                        if (s.Contains(text[t]) || S.Contains(text[t]))
                            answer.Append(temp[tween_key[i]][j]);
                        else
                        {
                            answer.Append(" ");
                            j--;
                        }
                        t++;
                    }
                    else
                        answer.Append(temp[tween_key[i]][j]);
                }
            return answer.ToString();
        }
        string UnPermutationCode(string text, int[] key)
        {
            string noSpaces = text.Replace(" ", "");
            string s = "abcdefghijklmnopqrstuvwxyz";
            string S = s.ToUpper();
            char[,] reText = new char[noSpaces.Length / key.Length, key.Length];
            int key_length = key.Length;
            int[] tween_key = new int[key_length]; //індекси послідовних цифр у ключі
                                                   //      key = 6 3 2 4 1 5
                                                   //tween_key = 4 2 1 3 5 0
            for (int i = 0; i < key_length; i++)
                tween_key[i] = FindFirst(key, i + 1);
            for (int j = 0; j < key_length; j++)
                for (int i = 0; i < noSpaces.Length / key_length; i++)
                    reText[i, tween_key[j]] = noSpaces[i + j * noSpaces.Length / key_length];
            StringBuilder answer = new StringBuilder();
            int t = 0;
            for (int i = 0; i < noSpaces.Length / key_length; i++)
                for (int j = 0; j < key_length; j++)
                    if (text.Length > t)
                    {
                        if (s.Contains(text[t]) || S.Contains(text[t]))
                            answer.Append(reText[i, j]);
                        else
                        {
                            answer.Append(" ");
                            j--;
                        }
                        t++;
                    }
                    else
                        answer.Append(reText[i, j]);
            return answer.ToString();
        }

        int mod(int num, int mod_num)
        {
            while (num < 0) num += mod_num;
            while (num >= mod_num) num -= mod_num;
            return num;
        }
        string CesarCode(string text, int k)
        {
            string s = "abcdefghijklmnopqrstuvwxyz";
            string S = s.ToUpper();
            char[] a = text.ToArray();
            int n = text.Length;
            for (int i = 0; i < n; ++i)
            {
                if (s.Contains(a[i]))
                {
                    a[i] = s[mod(a[i] - 97 + k, s.Length)];//(char)97 = a
                }
                else if (S.Contains(a[i]))
                {
                    a[i] = S[mod(a[i] - 65 + k, S.Length)];//(char)65 = A
                }
            }
            StringBuilder answer = new StringBuilder("");
            for (int i = 0; i < n; ++i)
            {
                answer.Append(a[i].ToString());
            }
            return answer.ToString();
        }
        (string, int) UnCesarCode_frequency(string text)
        {//повертаємо ймовірний розв'язок та ймовірний ключ (таке розшифрування погано працює для маленьких текстів)
            string s = "abcdefghijklmnopqrstuvwxyz";
            string S = s.ToUpper();
            char[] a = text.ToArray();
            int n = text.Length;
            int k = 0;
            StringBuilder ans;
            double minProbabilityDiff = double.MaxValue;
            for (int t = 1; t < 26; t++)//перебираємо кожний ключ
            {
                a = text.ToArray();
                for (int i = 0; i < n; ++i)//шифруємо поточним ключем повідомлення
                {
                    if (s.Contains(a[i]))
                        a[i] = s[mod(a[i] - 97 - t, s.Length)];//(char)97 = a
                    else if (S.Contains(a[i]))
                        a[i] = S[mod(a[i] - 65 - t, S.Length)];//(char)65 = A
                }
                ans = new StringBuilder("");//конвертуємо його у текст
                for (int i = 0; i < n; ++i)
                    ans.Append(a[i].ToString());
                double probDif = checkProbabilityDifference(ans.ToString());//перевіряємо різницю ймовірностей появи літер
                if (probDif < minProbabilityDiff)//знаходимо найменшу з різниць ймовірності
                {
                    minProbabilityDiff = probDif;
                    k = t;
                }
            }
            return (CesarCode(text, -k), k);//виводимо результат: розшифрований текст та його ключ
        }
        double checkProbabilityDifference(string str)
        {//рахуємо кількість символів та порівнюємо їх із статистичним розподілом(для англ. мови)

            /*(char,double)[] distribution = {('e',12.7),
                                            ('t',9.06),
                                            ('a',8.17),
                                            ('o',7.51),
                                            ('i',6.97),
                                            ('n',6.75),
                                            ('s',6.33),
                                            ('h',6.09),
                                            ('r',5.99),
                                            ('d',4.25),
                                            ('l',4.03),
                                            ('c',2.78),
                                            ('u',2.76),
                                            ('m',2.41),
                                            ('w',2.36),
                                            ('f',2.23),
                                            ('g',2.02),
                                            ('y',1.97),
                                            ('p',1.93),
                                            ('b',1.49),
                                            ('v',0.98),
                                            ('k',0.77),
                                            ('x',0.15),
                                            ('j',0.15),
                                            ('q',0.1),
                                            ('z',0.05),
            };*/
            string s = "abcdefghigklmnopqrstuvwxyz";
            string S = s.ToUpper();
            double[] our_distr = new double[26];
            int num_of_letters = 0;
            char[] a = str.ToArray();
            int n = str.Length;
            for (int i = 0; i < n; i++)
            {
                if (s.Contains(a[i]))
                {
                    our_distr[a[i] - 97]++;//(char)97 = a
                    num_of_letters++;
                }
                else if (S.Contains(a[i]))
                {
                    our_distr[a[i] - 65]++;//(char)65 = A
                    num_of_letters++;
                }
            }
            double diff = 0;
            if (num_of_letters > 0)
            {
                double[] distribution = { 8.17, 1.49, 2.78, 4.25, 12.7, 2.23, 2.02, 6.09, 6.97, 0.15, 0.77, 4.03, 2.41,
                                  6.75, 7.51, 1.93, 0.1,  5.99, 6.33, 9.06, 2.76, 0.98, 2.36, 0.15, 1.97, 0.05};//ймовірності попадання літер у алфавітному порядку
                for (int i = 0; i < 26; i++)
                    our_distr[i] /= (double)num_of_letters / 100.0;
                for (int i = 0; i < 26; i++)
                {
                    diff += Math.Abs(distribution[i] - our_distr[i]);
                }
            }
            return diff;
        }
    }
}