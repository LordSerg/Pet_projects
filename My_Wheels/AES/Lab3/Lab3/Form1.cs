using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Lab3
{
    public partial class Form1 : Form
    {
        byte[,] S_box;
        byte[,] invert_S_box;
        byte[,] multMatr;
        byte[,] invert_multMatr;
        byte[][] expandedKeys;
        byte[][] Rcon;
        //кількість стовпчиків для станів
        int Nb = 4;//4 -> 4*4=16 байт = 128 біт
                   //6 -> 4*6=24 байт = 192 біт
                   //8 -> 4*8=32 байт = 256 біт
        //кількість стовпчиків для ключа
        int Nk = 4;//4 -> 32*4=128 біт = 16 байт -> максимальна довжина ключа = 16 символів
                   //6 -> 32*6=192 біт = 24 байт -> максимальна довжина ключа = 24 символів
                   //8 -> 32*8=256 біт = 32 байт -> максимальна довжина ключа = 32 символів
        //кількість раундів
        int Nr = 10;//
        string text_in="", cifered_text="";
        string decifered_text="";
        public Form1()
        {
            InitializeComponent();
            S_box = new byte[,]{  {0x63, 0x7c, 0x77, 0x7b, 0xf2, 0x6b, 0x6f, 0xc5, 0x30, 0x01, 0x67, 0x2b, 0xfe, 0xd7, 0xab, 0x76 },
                                {0xca, 0x82, 0xc9, 0x7d, 0xfa, 0x59, 0x47, 0xf0, 0xad, 0xd4, 0xa2, 0xaf, 0x9c, 0xa4, 0x72, 0xc0 },
                                {0xb7, 0xfd, 0x93, 0x26, 0x36, 0x3f, 0xf7, 0xcc, 0x34, 0xa5, 0xe5, 0xf1, 0x71, 0xd8, 0x31, 0x15 },
                                {0x04, 0xc7, 0x23, 0xc3, 0x18, 0x96, 0x05, 0x9a, 0x07, 0x12, 0x80, 0xe2, 0xeb, 0x27, 0xb2, 0x75 },
                                {0x09, 0x83, 0x2c, 0x1a, 0x1b, 0x6e, 0x5a, 0xa0, 0x52, 0x3b, 0xd6, 0xb3, 0x29, 0xe3, 0x2f, 0x84 },
                                {0x53, 0xd1, 0x00, 0xed, 0x20, 0xfc, 0xb1, 0x5b, 0x6a, 0xcb, 0xbe, 0x39, 0x4a, 0x4c, 0x58, 0xcf },
                                {0xd0, 0xef, 0xaa, 0xfb, 0x43, 0x4d, 0x33, 0x85, 0x45, 0xf9, 0x02, 0x7f, 0x50, 0x3c, 0x9f, 0xa8 },
                                {0x51, 0xa3, 0x40, 0x8f, 0x92, 0x9d, 0x38, 0xf5, 0xbc, 0xb6, 0xda, 0x21, 0x10, 0xff, 0xf3, 0xd2 },
                                {0xcd, 0x0c, 0x13, 0xec, 0x5f, 0x97, 0x44, 0x17, 0xc4, 0xa7, 0x7e, 0x3d, 0x64, 0x5d, 0x19, 0x73 },
                                {0x60, 0x81, 0x4f, 0xdc, 0x22, 0x2a, 0x90, 0x88, 0x46, 0xee, 0xb8, 0x14, 0xde, 0x5e, 0x0b, 0xdb },
                                {0xe0, 0x32, 0x3a, 0x0a, 0x49, 0x06, 0x24, 0x5c, 0xc2, 0xd3, 0xac, 0x62, 0x91, 0x95, 0xe4, 0x79 },
                                {0xe7, 0xc8, 0x37, 0x6d, 0x8d, 0xd5, 0x4e, 0xa9, 0x6c, 0x56, 0xf4, 0xea, 0x65, 0x7a, 0xae, 0x08 },
                                {0xba, 0x78, 0x25, 0x2e, 0x1c, 0xa6, 0xb4, 0xc6, 0xe8, 0xdd, 0x74, 0x1f, 0x4b, 0xbd, 0x8b, 0x8a },
                                {0x70, 0x3e, 0xb5, 0x66, 0x48, 0x03, 0xf6, 0x0e, 0x61, 0x35, 0x57, 0xb9, 0x86, 0xc1, 0x1d, 0x9e },
                                {0xe1, 0xf8, 0x98, 0x11, 0x69, 0xd9, 0x8e, 0x94, 0x9b, 0x1e, 0x87, 0xe9, 0xce, 0x55, 0x28, 0xdf },
                                {0x8c, 0xa1, 0x89, 0x0d, 0xbf, 0xe6, 0x42, 0x68, 0x41, 0x99, 0x2d, 0x0f, 0xb0, 0x54, 0xbb, 0x16 }};
            invert_S_box = new byte[,] { { 0x52, 0x09, 0x6a, 0xd5, 0x30, 0x36, 0xa5, 0x38, 0xbf, 0x40, 0xa3, 0x9e, 0x81, 0xf3, 0xd7, 0xfb },
                                         { 0x7c, 0xe3, 0x39, 0x82, 0x9b, 0x2f, 0xff, 0x87, 0x34, 0x8e, 0x43, 0x44, 0xc4, 0xde, 0xe9, 0xcb },
                                         { 0x54, 0x7b, 0x94, 0x32, 0xa6, 0xc2, 0x23, 0x3d, 0xee, 0x4c, 0x95, 0x0b, 0x42, 0xfa, 0xc3, 0x4e },
                                         { 0x08, 0x2e, 0xa1, 0x66, 0x28, 0xd9, 0x24, 0xb2, 0x76, 0x5b, 0xa2, 0x49, 0x6d, 0x8b, 0xd1, 0x25 },
                                         { 0x72, 0xf8, 0xf6, 0x64, 0x86, 0x68, 0x98, 0x16, 0xd4, 0xa4, 0x5c, 0xcc, 0x5d, 0x65, 0xb6, 0x92 },
                                         { 0x6c, 0x70, 0x48, 0x50, 0xfd, 0xed, 0xb9, 0xda, 0x5e, 0x15, 0x46, 0x57, 0xa7, 0x8d, 0x9d, 0x84 },
                                         { 0x90, 0xd8, 0xab, 0x00, 0x8c, 0xbc, 0xd3, 0x0a, 0xf7, 0xe4, 0x58, 0x05, 0xb8, 0xb3, 0x45, 0x06 },
                                         { 0xd0, 0x2c, 0x1e, 0x8f, 0xca, 0x3f, 0x0f, 0x02, 0xc1, 0xaf, 0xbd, 0x03, 0x01, 0x13, 0x8a, 0x6b },
                                         { 0x3a, 0x91, 0x11, 0x41, 0x4f, 0x67, 0xdc, 0xea, 0x97, 0xf2, 0xcf, 0xce, 0xf0, 0xb4, 0xe6, 0x73 },
                                         { 0x96, 0xac, 0x74, 0x22, 0xe7, 0xad, 0x35, 0x85, 0xe2, 0xf9, 0x37, 0xe8, 0x1c, 0x75, 0xdf, 0x6e },
                                         { 0x47, 0xf1, 0x1a, 0x71, 0x1d, 0x29, 0xc5, 0x89, 0x6f, 0xb7, 0x62, 0x0e, 0xaa, 0x18, 0xbe, 0x1b },
                                         { 0xfc, 0x56, 0x3e, 0x4b, 0xc6, 0xd2, 0x79, 0x20, 0x9a, 0xdb, 0xc0, 0xfe, 0x78, 0xcd, 0x5a, 0xf4 },
                                         { 0x1f, 0xdd, 0xa8, 0x33, 0x88, 0x07, 0xc7, 0x31, 0xb1, 0x12, 0x10, 0x59, 0x27, 0x80, 0xec, 0x5f },
                                         { 0x60, 0x51, 0x7f, 0xa9, 0x19, 0xb5, 0x4a, 0x0d, 0x2d, 0xe5, 0x7a, 0x9f, 0x93, 0xc9, 0x9c, 0xef },
                                         { 0xa0, 0xe0, 0x3b, 0x4d, 0xae, 0x2a, 0xf5, 0xb0, 0xc8, 0xeb, 0xbb, 0x3c, 0x83, 0x53, 0x99, 0x61 },
                                         { 0x17, 0x2b, 0x04, 0x7e, 0xba, 0x77, 0xd6, 0x26, 0xe1, 0x69, 0x14, 0x63, 0x55, 0x21, 0x0c, 0x7d }};
            multMatr = new byte[,] { {0x02, 0x03, 0x01, 0x01 },
                                     {0x01, 0x02, 0x03, 0x01 },
                                     {0x01, 0x01, 0x02, 0x03 },
                                     {0x03, 0x01, 0x01, 0x02 }};
            invert_multMatr = new byte[,] { {0x0e, 0x0b, 0x0d, 0x09 },
                                            {0x09, 0x0e, 0x0b, 0x0d },
                                            {0x0d, 0x09, 0x0e, 0x0b },
                                            {0x0b, 0x0d, 0x09, 0x0e }};
            Rcon = new byte[50][];
            for (int i = 0; i < 50; i++)
            {
                Rcon[i] = new byte[4];
                for (int j = 0; j < 4; j++)
                    Rcon[i][j] = 0;
            }
            Rcon[0][0] = 1;
            for (int i = 1; i < 14; i++)
                Rcon[i][0] = (byte)((Rcon[i - 1][0] << 1) ^ (0x11b & -(Rcon[i - 1][0] >> 7)));
            comboBox1.Items.AddRange(new object[]{"Nb = 4", "Nb = 6", "Nb = 8" });
            comboBox2.Items.AddRange(new object[]{"Nk = 4", "Nk = 6", "Nk = 8" });
            comboBox1.SelectedIndex = 0;
            comboBox2.SelectedIndex = 0;
            toolTip1.SetToolTip(comboBox1,"Кількість стовбців для кожного стану");
            toolTip1.SetToolTip(comboBox2,"Кількість стовбців для ключа");
            //StringBuilder sb = new StringBuilder();
            //byte bt = 0x61;
            //sb.Append((char)bt);
            //richTextBox1.Text = sb.ToString();
        }

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
        {//зашифрувати текст
            richTextBox3.Text = "";
            cifered_text=Encrypt(richTextBox4.Text);
            //richTextBox3.Text += "result:\n";
            //richTextBox1.Text = richTextBox3.Text;
            //string text = richTextBox4.Text;
            //char[]arr=text.ToCharArray();
            //byte[] barr = new byte[arr.Length];
            //for (int i = 0; i < arr.Length; i++)
            //{
            //    barr[i] = (byte)arr[i];
            //}
            //barr[0] = 0xAC;
            //richTextBox3.Text += "\n"+Convert.ToString(barr[0], 16);
            //richTextBox3.Text += "\n"+Convert.ToString(barr[0]&0x01, 16);
            //richTextBox3.Text += "\n"+Convert.ToString(barr[0]&0x10, 16);
            //richTextBox3.Text += "\n"+Convert.ToString(0x01, 2);
            //richTextBox3.Text += "\n"+Convert.ToString(0x10, 2);
            //richTextBox3.Text += "\n"+Convert.ToString(barr[0], 2);
            //richTextBox3.Text += "\n"+Convert.ToString((barr[0]^ ((barr[0] >> 4)<<4)), 2);
            //richTextBox3.Text += "\n"+Convert.ToString((barr[0]>>4), 2);

            //StringBuilder sb = new StringBuilder();
            //for (int i = 0; i < arr.Length; i++)
            //    sb.Append(Convert.ToString(barr[i], 16));
            //richTextBox3.Text = sb.ToString();
            //richTextBox3.Text += "\nafter SubBytes\n";
            //barr = SubBytes(barr);
            //sb = new StringBuilder();
            //for (int i = 0; i < arr.Length; i++)
            //    sb.Append(Convert.ToString(barr[i], 16));
            //richTextBox3.Text += sb.ToString();
        }

        private void button1_Click(object sender, EventArgs e)
        {//розшифрувати текст
            richTextBox2.Text = "";
            decifered_text = Decrypt(richTextBox1.Text);
            //richTextBox2.Text = Convert.ToString(mult(0x09, 0xed), 2);

            //richTextBox3.Text += "\nafter ShiftRows:\n";
            //byte[] arr = {0x87, 0xf2, 0x4d, 0x97, 
            //              0x6e, 0x4c, 0x90, 0xec,
            //              0x46, 0xe7, 0x4a, 0xc3, 
            //              0xa6, 0x8c, 0xd8, 0x95};
            //byte[] arr = {0x47, 0x40, 0xa3, 0x4c,
            //              0x37, 0xd4, 0x70, 0x9f,
            //              0x94, 0xe4, 0x3a, 0x42,
            //              0xed, 0xa5, 0xa6, 0xbc};
            //for (int i = 0; i < 4; i++)
            //{
            //    for (int j = 0; j < 4; j++)
            //        richTextBox3.Text += Convert.ToString(arr[i * 4 + j],16) + " ";
            //    richTextBox3.Text += "\n";
            //}
            //richTextBox3.Text += "\n\n";
            //arr = ReMixColumns(arr);
            //for (int i = 0; i < 4; i++)
            //{
            //    for (int j = 0; j < 4; j++)
            //        richTextBox3.Text += Convert.ToString(arr[i * 4 + j], 16) + " ";
            //    richTextBox3.Text += "\n";
            //}
            //richTextBox3.Text += "\n\n";
            //arr = MixColumns(arr);
            //for (int i = 0; i < 4; i++)
            //{
            //    for (int j = 0; j < 4; j++)
            //        richTextBox3.Text += Convert.ToString(arr[i * 4 + j], 16) + " ";
            //    richTextBox3.Text += "\n";
            //}
        }
        void check()
        {//перевіряємо довжину ключа та кількість раундів
            Nb = 4 + comboBox1.SelectedIndex * 2;
            Nk = 4 + comboBox2.SelectedIndex * 2;
            if (Nk==4)
            {//128bit = 16byte key=> max 16 symbols
                if (textBox1.Text.Length > 16)
                    textBox1.Text = textBox1.Text.Substring(0, 16);
            }
            else if (Nk==6)
            {//192bit = 24byte key=> max 24 symbols
                if (textBox1.Text.Length > 24)
                    textBox1.Text = textBox1.Text.Substring(0, 24);
            }
            else if (Nk==8)
            {//256bit = 32byte key=> max 32 symbols
                if (textBox1.Text.Length > 32)
                    textBox1.Text = textBox1.Text.Substring(0, 32);
            }
            if (Nb == 8 || Nk == 8)
                Nr = 14;
            else if (Nb == 6 || Nk == 6)
                Nr = 12;
            else
                Nr = 10;
        }
        private void textBox1_TextChanged(object sender, EventArgs e)
        {
            check();
        }
        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            check();
        }
        void ExpandKey(byte []key) 
        {//розширюємо ключ
            for (int i = 0; i < Nk; i++)
                for (int j = 0; j < 4; j++)
                    expandedKeys[i][j] = key[i*4 + j];
            for (int i = Nk; i < (Nr + 1)*Nb; i++)
            {
                if (i%Nk==0)
                {
                    byte[] temp = new byte[4];
                    for (int j = 0; j < 4; j++) //зсув
                        temp[j] = expandedKeys[i - 1][(j+1)%4];
                    if (Nk <= 6)
                    {
                        for (int j = 0; j < 4; j++) //реверс кожного ел. через S-box
                            temp[j] = S_box[(temp[j] & 0xf0)>>4, temp[j] & 0x0f];
                        for (int j = 0; j < 4; j++)
                        {
                            byte a1 = Rcon[i / 4 -1][j];
                            byte a2 = temp[j];
                            byte a3 = expandedKeys[i - Nk][j];
                            expandedKeys[i][j] = (byte)(a1 ^ a2 ^ a3);
                            //expandedKeys[i][j] = (byte)(Rcon[i / 4][j] ^ temp[j] ^ expandedKeys[i - Nk][j]);
                        }
                    }
                    else if (Nk > 6)
                    {
                        for (int j = 0; j < 4; j++)
                            expandedKeys[i][j] = (byte)(Rcon[i/4][j] ^ temp[j] ^ expandedKeys[i - Nk][j]);
                        for (int j = 0; j < 4; j++) //реверс кожного ел. через S-box
                            temp[j] = S_box[(temp[j] & 0xf0)>>4, temp[j] & 0x0f];
                    }
                }
                else
                {
                    for (int j = 0; j < 4; j++)
                        expandedKeys[i][j] = (byte)(expandedKeys[i - 1][j] ^ expandedKeys[i - Nk][j]);
                }
            }
        }
        string ShowState(byte[] state)
        {
            string answer="";
            for (int i = 0; i < 4; i++)
            {
                for (int j = 0; j < Nb; j++)
                    answer += Convert.ToString(state[i * Nb + j], 16) + " ";
                answer += "\n";
            }
            return answer;
        }
        string Encrypt(string text)
        {
            byte[][] pieces;
            int n;
            if (text.Length % (4 * Nb) == 0)
                n = text.Length / (4 * Nb);
            else
                n = (text.Length / (4 * Nb)) + 1;
            pieces = new byte[n][];
            string s="";
            for (int i = 0; i < n; i++)
            {
                pieces[i] = new byte[4 * Nb];
                if (4 * Nb <= text.Length)
                {
                    s = text.Substring(0, 4 * Nb);
                    text = text.Remove(0, 4 * Nb);
                }
                else
                    s = text;
                char[] arr = new char[4 * Nb];
                char[] arr_temp = s.ToCharArray();
                for (int j = 0; j < 4 * Nb; j++)
                {
                    if (j < arr_temp.Length)
                        arr[j] = arr_temp[j];
                    else
                        arr[j] = (char)0;
                }
                for (int j = 0; j < 4 * Nb; j++)
                {
                    pieces[i][(j * Nb) % (4 * Nb) + j / 4] = (byte)arr[j];//[];
                }
            }
            string kkey = textBox1.Text;
            char[] r = kkey.ToCharArray();
            byte[] minikey = new byte[4 * Nk];
            for (int t = 0; t < kkey.Length; t++)
                minikey[t] = (byte)r[t];
            for (int t = kkey.Length; t < 4 * Nk; t++)
                minikey[t] = 0;
            //розширюємо ключ
            expandedKeys = new byte[(Nr + 1) * Nb][];
            for (int i = 0; i < (Nr + 1) * Nb; i++)
                expandedKeys[i] = new byte[4];//4-байтне слово
            ExpandKey(minikey);
            if (checkBox1.Checked)
            {
                richTextBox3.Text += "expandedKeys:\n";
                for (int j = 0; j < 4; j++)
                {
                    for (int i = 0; i < Nb * (Nr + 1); i++)
                    {
                        richTextBox3.Text += Convert.ToString(expandedKeys[i][j], 16) + " ";
                    }
                    richTextBox3.Text += "\n";
                }
                richTextBox3.Text += "\nstates befor steps:\n";
                for (int k = 0; k < n; k++)
                    richTextBox3.Text += ShowState(pieces[k]);
            }
            //головний цикл алгоритму
            for (int k = 0; k < n; k++)
                pieces[k]= AddRoundKey(pieces[k],0);
            if (checkBox1.Checked)
            {
                richTextBox3.Text += "\n---Round 1---\nafter AddRoundKey:\n";
                for (int k = 0; k < n; k++)
                    richTextBox3.Text += ShowState(pieces[k]);
            }
            for (int i = 0; i < Nr - 1; i++)
            {
                for (int k = 0; k < n; k++)
                    pieces[k] = SubBytes(pieces[k]);
                if (checkBox1.Checked)
                {
                    richTextBox3.Text += "\nafter SubBytes:\n";
                    for (int k = 0; k < n; k++) richTextBox3.Text += ShowState(pieces[k]);
                }
                for (int k = 0; k < n; k++)
                    pieces[k] = ShiftRows(pieces[k]);
                if (checkBox1.Checked)
                {
                    richTextBox3.Text += "\nafter ShiftRows:\n";
                    for (int k = 0; k < n; k++) richTextBox3.Text += ShowState(pieces[k]);
                }
                for (int k = 0; k < n; k++)
                    pieces[k] = MixColumns(pieces[k]);
                if (checkBox1.Checked)
                {
                    richTextBox3.Text += "\nafter MixColumns:\n";
                    for (int k = 0; k < n; k++) richTextBox3.Text += ShowState(pieces[k]);
                }
                for (int k = 0; k < n; k++)
                    pieces[k] = AddRoundKey(pieces[k],(i+1)*4);
                if (checkBox1.Checked)
                {
                    richTextBox3.Text += "\n---Round " + (i + 2) + "---";
                    richTextBox3.Text += "\nafter AddRoundKey:\n";
                    for (int k = 0; k < n; k++) richTextBox3.Text += ShowState(pieces[k]);
                }
            }
            for (int k = 0; k < n; k++)
                pieces[k] = SubBytes(pieces[k]);
            if (checkBox1.Checked)
            {
                richTextBox3.Text += "\nafter SubBytes:\n";
                for (int k = 0; k < n; k++) richTextBox3.Text += ShowState(pieces[k]);
            }
            for (int k = 0; k < n; k++)
                pieces[k] = ShiftRows(pieces[k]);
            if (checkBox1.Checked)
            {
                richTextBox3.Text += "\nafter ShiftRows:\n";
                for (int k = 0; k < n; k++) richTextBox3.Text += ShowState(pieces[k]);
            }
            for (int k = 0; k < n; k++)
                pieces[k] = AddRoundKey(pieces[k],Nr*4);
            if (checkBox1.Checked)
            {
                richTextBox3.Text += "\nafter AddRoundKey:\n";
                for (int k = 0; k < n; k++) richTextBox3.Text += ShowState(pieces[k]);
            }
            s = "";
            string s_hex = "";
            for (int i = 0; i < n; i++)
            {
                for (int j = 0; j < 4 * Nb; j++)
                {
                    s_hex += Convert.ToString(pieces[i][j], 16);
                    s += (char)pieces[i][j];
                }
            }

            richTextBox3.Text += "result(str):\n";
            richTextBox3.Text += s;
            richTextBox3.Text += "\nresult(hex):\n";
            richTextBox3.Text += s_hex;
            return s;
        }
        string Decrypt(string text)
        {
            byte[][] pieces;
            int n;
            if (text.Length % (4 * Nb) == 0)
                n = text.Length / (4 * Nb);
            else
                n = (text.Length / (4 * Nb)) + 1;
            pieces = new byte[n][];
            string s = "";
            for (int i = 0; i < n; i++)
            {
                pieces[i] = new byte[4 * Nb];
                if (4 * Nb <= text.Length)
                {
                    s = text.Substring(0, 4 * Nb);
                    text = text.Remove(0, 4 * Nb);
                }
                else
                    s = text;
                char[] arr = new char[4 * Nb];
                char[] arr_temp = s.ToCharArray();
                for (int j = 0; j < 4 * Nb; j++)
                {
                    if (j < arr_temp.Length)
                        arr[j] = arr_temp[j];
                    else
                        arr[j] = (char)0;
                }
                for (int j = 0; j < 4 * Nb; j++)
                {
                    pieces[i][j] = (byte)arr[j]; 
                }
            }
            string kkey = textBox1.Text;
            char[] r = kkey.ToCharArray();
            byte[] minikey = new byte[4 * Nk];
            for (int t = 0; t < kkey.Length; t++)
                minikey[t] = (byte)r[t];
            for (int t = kkey.Length; t < 4 * Nk; t++)
                minikey[t] = 0;
            //розширюємо ключ
            expandedKeys = new byte[(Nr + 1) * Nb][];
            for (int i = 0; i < (Nr + 1) * Nb; i++)
                expandedKeys[i] = new byte[4];//4-байтне слово
            ExpandKey(minikey);
            if (checkBox2.Checked)
            {
                richTextBox2.Text += "expandedKeys:\n";
                for (int j = 0; j < 4; j++)
                {
                    for (int i = 0; i < Nb * (Nr + 1); i++)
                    {
                        richTextBox2.Text += Convert.ToString(expandedKeys[i][j], 16) + " ";
                    }
                    richTextBox2.Text += "\n";
                }
                richTextBox2.Text += "\nstates befor steps:\n";
                for (int k = 0; k < n; k++)
                    richTextBox2.Text += ShowState(pieces[k]);
            }

            //головний цикл алгоритму
            for (int k = 0; k < n; k++)
                pieces[k] = AddRoundKey(pieces[k], Nr*4);
            if (checkBox2.Checked)
            {
                richTextBox2.Text += "---\nafter AddRoundKey:\n";
                for (int k = 0; k < n; k++)
                    richTextBox2.Text += ShowState(pieces[k]);
                richTextBox2.Text += "\n---Round "+Nr+"---\n";
            }
            for (int i = Nr-1; i >0 ; i--)
            {
                for (int k = 0; k < n; k++)
                    pieces[k] = ReShiftRows(pieces[k]);
                if (checkBox2.Checked)
                {
                    richTextBox2.Text += "---\nafter ShiftRows:\n";
                    for (int k = 0; k < n; k++)
                        richTextBox2.Text += ShowState(pieces[k]);
                }

                for (int k = 0; k < n; k++)
                    pieces[k] = ReSubBytes(pieces[k]);
                if (checkBox2.Checked)
                {
                    richTextBox2.Text += "---\nafter SubBytes:\n";
                    for (int k = 0; k < n; k++)
                        richTextBox2.Text += ShowState(pieces[k]);
                }

                for (int k = 0; k < n; k++)
                    pieces[k] = AddRoundKey(pieces[k], i*4);
                if (checkBox2.Checked)
                {
                    richTextBox2.Text += "---\nafter AddRoundKey:\n";
                    for (int k = 0; k < n; k++)
                        richTextBox2.Text += ShowState(pieces[k]);
                }

                for (int k = 0; k < n; k++)
                    pieces[k] = ReMixColumns(pieces[k]);
                if (checkBox2.Checked)
                {
                    richTextBox2.Text += "---\nafter MixColumns:\n";
                    for (int k = 0; k < n; k++)
                        richTextBox2.Text += ShowState(pieces[k]);
                    richTextBox2.Text += "\n---Round " + i + "---\n";
                }
            }
            for (int k = 0; k < n; k++)
                pieces[k] = ReShiftRows(pieces[k]);
            if (checkBox2.Checked)
            {
                richTextBox2.Text += "---\nafter ShiftRows:\n";
                for (int k = 0; k < n; k++)
                    richTextBox2.Text += ShowState(pieces[k]);
            }

            for (int k = 0; k < n; k++)
                pieces[k] = ReSubBytes(pieces[k]);
            if (checkBox2.Checked)
            {
                richTextBox2.Text += "---\nafter SubBytes:\n";
                for (int k = 0; k < n; k++)
                    richTextBox2.Text += ShowState(pieces[k]);
            }

            for (int k = 0; k < n; k++)
                pieces[k] = AddRoundKey(pieces[k], 0);
            if (checkBox2.Checked)
            {
                richTextBox2.Text += "---\nafter AddRoundKey:\n";
                for (int k = 0; k < n; k++)
                    richTextBox2.Text += ShowState(pieces[k]);
            }
            s = "";
            string s_hex = "";
            for (int i = 0; i < n; i++)
            {
                for (int j = 0; j < 4 * Nb; j++)
                {
                    s_hex += Convert.ToString(pieces[i][j], 16);
                    s += (char)pieces[i][(j % 4) * Nb + (j / 4)];
                }
            }
            richTextBox2.Text += "result(str):\n";
            richTextBox2.Text += s;
            richTextBox2.Text += "\nresult(hex):\n";
            richTextBox2.Text += s_hex;
            return s;
        }
        string Decrypt(byte[] bytesInput)
        { 
            byte[][] pieces;
            int n;
            if (bytesInput.Length % (4 * Nb) == 0)
                n = bytesInput.Length / (4 * Nb);
            else
                n = (bytesInput.Length / (4 * Nb)) + 1;
            pieces = new byte[n][];
            
            for (int i = 0; i < n; i++)
            {
                pieces[i] = new byte[4 * Nb];
                for (int j = 0; j < 4 * Nb; j++)
                {
                    pieces[i][j] = bytesInput[j+i*4*Nb];
                }
            }
            string kkey = textBox1.Text;
            char[] r = kkey.ToCharArray();
            byte[] minikey = new byte[4 * Nk];
            for (int t = 0; t < kkey.Length; t++)
                minikey[t] = (byte)r[t];
            for (int t = kkey.Length; t < 4 * Nk; t++)
                minikey[t] = 0;
            //розширюємо ключ
            expandedKeys = new byte[(Nr + 1) * Nb][];
            for (int i = 0; i < (Nr + 1) * Nb; i++)
                expandedKeys[i] = new byte[4];//4-байтне слово
            ExpandKey(minikey);
            if (checkBox2.Checked)
            {
                richTextBox2.Text += "expandedKeys:\n";
                for (int j = 0; j < 4; j++)
                {
                    for (int i = 0; i < Nb * (Nr + 1); i++)
                    {
                        richTextBox2.Text += Convert.ToString(expandedKeys[i][j], 16) + " ";
                    }
                    richTextBox2.Text += "\n";
                }
                richTextBox2.Text += "\nstates befor steps:\n";
                for (int k = 0; k < n; k++)
                    richTextBox2.Text += ShowState(pieces[k]);
            }

            //
            //sub
            //10
            //shift
            //sub
            //addroundkey
            //mixcolumns
            //9
            //shift
            //sub
            //addroundkey
            //mixcolumns
            //...
            //1
            //shift
            //sub
            //addroundkey

            //головний цикл алгоритму
            for (int k = 0; k < n; k++)
                pieces[k] = AddRoundKey(pieces[k], Nr * 4);
            if (checkBox2.Checked)
            {
                richTextBox2.Text += "---\nafter AddRoundKey:\n";
                for (int k = 0; k < n; k++)
                    richTextBox2.Text += ShowState(pieces[k]);
                richTextBox2.Text += "\n---Round " + Nr + "---\n";
            }
            for (int i = Nr - 1; i > 0; i--)
            {
                for (int k = 0; k < n; k++)
                    pieces[k] = ReShiftRows(pieces[k]);
                if (checkBox2.Checked)
                {
                    richTextBox2.Text += "---\nafter ShiftRows:\n";
                    for (int k = 0; k < n; k++)
                        richTextBox2.Text += ShowState(pieces[k]);
                }

                for (int k = 0; k < n; k++)
                    pieces[k] = ReSubBytes(pieces[k]);
                if (checkBox2.Checked)
                {
                    richTextBox2.Text += "---\nafter SubBytes:\n";
                    for (int k = 0; k < n; k++)
                        richTextBox2.Text += ShowState(pieces[k]);
                }

                for (int k = 0; k < n; k++)
                    pieces[k] = AddRoundKey(pieces[k], i * 4);
                if (checkBox2.Checked)
                {
                    richTextBox2.Text += "---\nafter AddRoundKey:\n";
                    for (int k = 0; k < n; k++)
                        richTextBox2.Text += ShowState(pieces[k]);
                }

                for (int k = 0; k < n; k++)
                    pieces[k] = ReMixColumns(pieces[k]);
                if (checkBox2.Checked)
                {
                    richTextBox2.Text += "---\nafter MixColumns:\n";
                    for (int k = 0; k < n; k++)
                        richTextBox2.Text += ShowState(pieces[k]);
                    richTextBox2.Text += "\n---Round " + i + "---\n";
                }
            }
            for (int k = 0; k < n; k++)
                pieces[k] = ReShiftRows(pieces[k]);
            if (checkBox2.Checked)
            {
                richTextBox2.Text += "---\nafter ShiftRows:\n";
                for (int k = 0; k < n; k++)
                    richTextBox2.Text += ShowState(pieces[k]);
            }

            for (int k = 0; k < n; k++)
                pieces[k] = ReSubBytes(pieces[k]);
            if (checkBox2.Checked)
            {
                richTextBox2.Text += "---\nafter SubBytes:\n";
                for (int k = 0; k < n; k++)
                    richTextBox2.Text += ShowState(pieces[k]);
            }

            for (int k = 0; k < n; k++)
                pieces[k] = AddRoundKey(pieces[k], 0);
            if (checkBox2.Checked)
            {
                richTextBox2.Text += "---\nafter AddRoundKey:\n";
                for (int k = 0; k < n; k++)
                    richTextBox2.Text += ShowState(pieces[k]);
            }

            string s = "", s_hex = "";
            for (int i = 0; i < n; i++)
            {
                for (int j = 0; j < 4 * Nb; j++)
                {
                    s_hex += Convert.ToString(pieces[i][j], 16);
                    s += (char)pieces[i][(j % 4) * Nb + (j / 4)];
                }
            }
            richTextBox2.Text += "result(str):\n";
            richTextBox2.Text += s;
            richTextBox2.Text += "\nresult(hex):\n";
            richTextBox2.Text += s_hex;
            return s;
        }
        int mod(int w, int v) 
        {
            while (w > v) w -= v;
            while (w < 0) w += v;
            return w;
        }
        byte[] AddRoundKey(byte[] b, int from)
        {
            for (int i = 0; i < Nb; i++)
                for (int j = 0; j < 4; j++)
                {
                    byte a1 = b[i  + j* Nb];
                    byte a2 = expandedKeys[from + i][j];
                    byte a3 = (byte)(a1^a2);
                    b[i  + j* Nb] = a3;
                    //b[i * Nb + j] ^= expandedKeys[from + j][i];
                }
            return b;
        }
        byte[] SubBytes(byte[] b)
        {
            for (int i = 0; i < 4*Nb; i++)
                b[i] = S_box[(b[i] & 0xf0)>>4, b[i] & 0x0f];//0x0f=0b1111
            return b;
        }
        byte[] ReSubBytes(byte[] b)
        {
            for (int i = 0; i < 4 * Nb; i++)
                b[i] = invert_S_box[(b[i] & 0xf0) >> 4, b[i] & 0x0f];//0x0f=0b1111
            return b;
        }
        byte[] ShiftRows(byte[] b)
        {
            byte[] result = new byte[4*Nb];
            for (int i = 0; i < 4; i++)
                for (int j = 0; j < Nb; j++) 
                    result[i*Nb + j] = b[i*Nb + (j+i)%Nb];
            return result;
        }
        byte[] ReShiftRows(byte[] b)
        {
            byte[] result = new byte[4 * Nb];
            for (int i = 0; i < 4; i++)
                for (int j = 0; j < Nb; j++)
                    result[i * Nb + j] = b[i * Nb + mod(j - i,Nb)];
            return result;
        }
        byte[] ReMixColumns(byte[] b)
        {
            byte[] result = new byte[4 * Nb];
            for (int i = 0; i < Nb; i++)
                for (int j = 0; j < 4; j++)
                {
                    byte elem = 0;
                    for (int t = 0; t < 4; t++)
                        elem ^= mult(invert_multMatr[j, t], b[i + Nb * t]);
                    result[i + Nb * j] = elem;
                }
            return result;
        }
        byte[] MixColumns(byte[] b)
        {
            byte[] result = new byte[4*Nb];
            for (int i = 0; i < Nb; i++)
                for (int j = 0; j < 4; j++)
                {
                    byte elem = 0;
                    for (int t = 0; t < 4; t++)
                        elem ^= mult(multMatr[j,t], b[i + Nb * t]);
                    result[i + Nb * j] = elem;
                }
            return result;
        }
        byte mult(byte matrixElem, byte columnElem)
        {
            byte polynoms = matrixElem;
            int temp2 = 0b10000000_00000000;
            int result = 0;
            byte temp = 0b1;
            for (int i = 0; i < 8; i++)
            {
                if ((polynoms & temp) != 0)
                {
                    result ^= columnElem << i;
                    temp2 = 0b10000000_00000000;
                    for (int j = 7; j >= 0; j--)
                    {
                        if ((result & temp2) == temp2)
                        {
                            result ^= temp2;
                            result ^= (0b11011 << j);
                            j = 8;
                            temp2 = 0b10000000_00000000;
                        }
                        else
                            temp2 = temp2 >> 1;
                    }
                }
                temp = (byte)(temp << 1);
            }
            return (byte)result;
        }

        private void button4_Click(object sender, EventArgs e)
        {//Завантажити зашифрований текст
            OpenFileDialog ofd = new OpenFileDialog();
            if (ofd.ShowDialog() == DialogResult.OK)
            {
                Stream file = ofd.OpenFile();
                StreamReader reader = new StreamReader(file);
                string[] s = reader.ReadToEnd().Trim().Split(' ');
                byte[] r = new byte[s.Length];
                for (int i = 0; i < s.Length; i++)
                {
                    r[i] = Convert.ToByte(s[i]);
                }
                cifered_text = string.Empty;
                for (int i = 0; i < s.Length; i++)
                {
                    cifered_text += (char)r[i];
                }
                //cifered_text;
                reader.Close();
                file.Close();
                richTextBox1.Text = cifered_text;
                richTextBox2.Text = "";
                decifered_text = Decrypt(r);
            }
        }

        private void button6_Click(object sender, EventArgs e)
        {//Зберегти звичайний текст
            if (decifered_text.Length > 0)
            {
                SaveFileDialog sfd = new SaveFileDialog();
                if (sfd.ShowDialog() == DialogResult.OK)
                {
                    string path = sfd.FileName;
                    FileStream fs = File.Create(path);
                    fs.Close();
                    StreamWriter sw = new StreamWriter(path);
                    sw.Write(decifered_text);
                    sw.Close();
                }
            }
        }

        private void button3_Click(object sender, EventArgs e)
        {//завантажити звичайний текст
            OpenFileDialog ofd = new OpenFileDialog();
            if (ofd.ShowDialog() == DialogResult.OK)
            {
                string filePath = ofd.FileName;
                Stream file = ofd.OpenFile();
                StreamReader reader = new StreamReader(file);
                text_in = reader.ReadToEnd();
                reader.Close();
                file.Close();
                richTextBox4.Text = text_in;
                richTextBox3.Text = "";
            }
        }

        private void button5_Click(object sender, EventArgs e)
        {//Зберегти зашифрований текст
            if (cifered_text.Length > 0)
            {
                SaveFileDialog sfd = new SaveFileDialog();
                if (sfd.ShowDialog() == DialogResult.OK)
                {
                    string path = sfd.FileName;
                    FileStream fs = File.Create(path);
                    fs.Close();
                    byte[] result = StringToByteArray(cifered_text);
                    StreamWriter sw = new StreamWriter(path);
                    for (int i = 0; i < result.Length; i++)
                    {
                        sw.Write((int)result[i] + " ");
                    }
                    sw.Close();
                }
            }
        }
        public static byte[] StringToByteArray(string str)
        {
            char[] arr = str.ToCharArray();
            byte[] result = new byte[str.Length];
            for (int i = 0; i < str.Length; i++)
                result[i] = (byte)arr[i];
            return result;
        }

    }
}
