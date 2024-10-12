using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;
namespace GraphLibrary
{
    public class Vertex
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
            g.FillEllipse(new SolidBrush(Color.LightPink), x - r, y - r, r * 2, r * 2);
            g.DrawString(text, new Font("Arial", 12), new SolidBrush(Color.Black), x - r / 2, y - r / 2);
        }
        public void Draw(Graphics g, string text, bool is_activ)
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
            return Math.Sqrt((x - _x) * (x - _x) + (y - _y) * (y - _y)) < r;
        }
    }
}
