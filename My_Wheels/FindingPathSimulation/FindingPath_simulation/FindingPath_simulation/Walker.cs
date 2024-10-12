using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;
using GraphLibrary;

namespace FindingPath_simulation
{
    static class Walker
    {
        //?:
        //5. делать ли интерпритацию графа как "плитку"?
        private static List<Vertex>path;
        private static float x, y,dx,dy,x_to,y_to;
        private static int cur_path_id;
        private static float divider=20;//по сути это скорость, но наоборот
        public static Graph graph_local;
        public static int CurID = 0;
        public static void init(Graph g)
        {
            if(g.v.Count()>0)
            x = g.v[0].x;
            y = g.v[0].y;
            x_to = g.v[0].x;
            y_to = g.v[0].y;
            graph_local = g;
            CurID = 0;
        }
        public static void SetPath(List<Vertex> newPath)
        {
            path = new List<Vertex>();
            foreach (Vertex v in newPath)
                path.Add(v);
            x = path[0].x;
            y = path[0].y;
            x_to = path[path.Count()-1].x;
            y_to = path[path.Count()-1].y;
            cur_path_id = 0;
        }
        public static void Go()
        {
            if (path!=null && path.Count() > 0)
            {
                if (cur_path_id== divider-1)
                {
                    cur_path_id = 0;
                    x = path[1].x;
                    y = path[1].y;
                    CurID = graph_local.v.FindIndex(vert=>(vert.x==x&&vert.y==y));
                    path.RemoveAt(0);
                    if (path.Count() > 1)
                    {
                        dx = (path[1].x - path[0].x) / divider;
                        dy = (path[1].y - path[0].y) / divider;
                    }
                    else if (path.Count() > 0)
                    {
                        path.RemoveAt(0);
                        //dx = (x - path[0].x) / divider;
                        //dy = (y - path[0].y) / divider;
                    }
                    else
                        return;
                }
                else if (path.Count() == 1)
                {
                    path.RemoveAt(0);
                }
                else if (cur_path_id==0)
                {
                    dx = (path[1].x-path[0].x) / divider;
                    dy = (path[1].y-path[0].y) / divider;
                }
                Move();
                //Show(g);
            }
        }
        private static void Move()
        {
            x += dx;
            y += dy;
            cur_path_id++;
        }
        public static void Show(Graphics g)
        {
            g.FillEllipse(new SolidBrush(Color.Red), x - 10, y - 10, 20, 20);
            g.DrawEllipse(new Pen(Color.Red), x_to - 10, y_to - 10, 20, 20);
        }
    }
}
