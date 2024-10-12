using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;
namespace GraphLibrary
{
    public class Edge
    {
        public int id_in, id_out;
        public float weight;//, thread;
        //float x1, x2, y1, y2;
        public Edge(int _in, int _out, float _weight)
        {
            id_in = _in;
            id_out = _out;
            weight = _weight;//максимальный поток
            //thread = 0;//текущий поток
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
            //thread = ed.thread;
        }
        //public void Draw(Graphics g)
        //{
        //    g.DrawLine(new Pen(Color.Black), x1, y1, x2, y2);
        //}
    }
}
