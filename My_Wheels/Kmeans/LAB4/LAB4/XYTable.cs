using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Drawing;
using System.Drawing.Drawing2D;

namespace LAB3
{
    public class XYtable : ScrollableControl
    {
        //таблица ввысь:
        // X    | Y     |
        // x1   | y1    |
        // x2   | y2    |
        // x3   | y3    |
        // x4   | y4    |
        TextBox tbInput = new TextBox();
        public List<TextBox> TextBoxes = new List<TextBox>();//текстбоксы, в которых будут содержатся значения ячеек
        public int n;//количество введенных значений
        int sizeW = 100, sizeH = 25;
        public XYtable()
        {
            SetStyle(ControlStyles.AllPaintingInWmPaint, true);
            SetStyle(ControlStyles.OptimizedDoubleBuffer, true);
            SetStyle(ControlStyles.ResizeRedraw, true);
            SetStyle(ControlStyles.SupportsTransparentBackColor, true);
            SetStyle(ControlStyles.UserPaint, true);
            DoubleBuffered = true;
            Size = new Size(500, 200);
            n = 10;
            for (int i = 0; i < n * 2; i += 2)
            {
                TextBoxes.Add(new TextBox());
                TextBoxes[i].BorderStyle = BorderStyle.Fixed3D;
                TextBoxes[i].Size = new Size(sizeW, sizeH);
                TextBoxes[i].Location = new Point((this.Width / 2) - (this.Width / 4) - sizeW / 2, (i / 2) * sizeH + 10);
                TextBoxes[i].TextChanged += TB_TextChanged;
                TextBoxes.Add(new TextBox());
                TextBoxes[i + 1] = new TextBox();
                TextBoxes[i + 1].BorderStyle = BorderStyle.Fixed3D;
                TextBoxes[i + 1].Size = new Size(sizeW, sizeH);
                TextBoxes[i + 1].Location = new Point((this.Width / 2) + (this.Width / 4) - sizeW / 2, (i / 2) * sizeH + 10);
                TextBoxes[i + 1].TextChanged += TB_TextChanged;
                this.Controls.Add(TextBoxes[i]);
                this.Controls.Add(TextBoxes[i + 1]);
            }
            Text = "";
            AutoScroll = true;
            HScroll = false;
            
            //AutoScrollMinSize = new Size(n * sizeH, 200);
        }
        protected override void OnPaint(PaintEventArgs e)
        {
            Graphics g = e.Graphics;
            g.SmoothingMode = SmoothingMode.HighQuality;
            g.Clear(Parent.BackColor);
            //рисуем контур
            g.FillRectangle(new SolidBrush(Color.Gray), 0, 0, Width, Height);
            g.FillRectangle(new SolidBrush(Parent.BackColor), 1, 1, Width - 2, Height - 2);
            //g.DrawString(Text, Font, new SolidBrush(Color.Red), 0, 0);
            //отрисовываем заглавный столбец
            //g.DrawString("X", Font, new SolidBrush(Color.Black), 0, (this.Height / 2) - (this.Height / 4));
            //g.DrawString("Y", Font, new SolidBrush(Color.Black), 0, (this.Height / 2) + (this.Height / 4));

        }

        private void TB_TextChanged(object sender, EventArgs e)
        {
            Text = tbInput.Text;
        }
        public void AddCol()
        {
            n++;
            for (int i = (n - 1) * 2; i < n * 2; i += 2)
            {
                TextBoxes.Add(new TextBox());
                TextBoxes[i].BorderStyle = BorderStyle.Fixed3D;
                TextBoxes[i].Location = new Point((this.Width / 2) - (this.Width / 4) - sizeW / 2, (i / 2) * sizeH + 10 - VerticalScroll.Value);
                TextBoxes[i].Size = new Size(sizeW, sizeH);
                TextBoxes[i].TextChanged += TB_TextChanged;
                TextBoxes.Add(new TextBox());
                TextBoxes[i + 1] = new TextBox();
                TextBoxes[i + 1].BorderStyle = BorderStyle.Fixed3D;
                TextBoxes[i + 1].Location = new Point((this.Width / 2) + (this.Width / 4) - sizeW / 2, (i / 2) * sizeH + 10 - VerticalScroll.Value);
                TextBoxes[i + 1].Size = new Size(sizeW, sizeH);
                TextBoxes[i + 1].TextChanged += TB_TextChanged;
                this.Controls.Add(TextBoxes[i]);
                this.Controls.Add(TextBoxes[i + 1]);
            }
            
            //AutoScrollMinSize = new Size(n * sizeH, 200);
        }
        
        public void DeleteCol()
        {
            if (n > 0)
            {
                this.Controls.Remove(TextBoxes[n * 2 - 1]);
                this.Controls.Remove(TextBoxes[n * 2 - 2]);
                TextBoxes.Remove(TextBoxes[n * 2 - 1]);
                TextBoxes.Remove(TextBoxes[n * 2 - 2]);
                n--;
            }
            //AutoScrollMinSize = new Size(n * sizeH, 200);
        }
        protected override void OnSizeChanged(EventArgs e)
        {
            for (int i = 0; i < n * 2; i += 2)
            {
                TextBoxes[i].Location = new Point((this.Width / 2) - (this.Width / 4) - sizeW / 2, (i / 2) * sizeH + 10 - VerticalScroll.Value);

                TextBoxes[i + 1].Location = new Point((this.Width / 2) + (this.Width / 4) - sizeW / 2, (i / 2) * sizeH + 10 - VerticalScroll.Value);
            }
            this.Refresh();
            //AutoScrollMinSize = new Size(n * sizeH, 200);
        }
        protected override void OnClick(EventArgs e)
        {

        }
        protected override void OnTextChanged(EventArgs e)
        {

        }
        protected override void OnScroll(ScrollEventArgs se)
        {
            this.Refresh();
        }
    }
}
