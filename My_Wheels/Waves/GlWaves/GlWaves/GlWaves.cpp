#include <iostream>
#include "GLFW/glfw3.h"
#include "glm/glm/glm.hpp"
#include <cmath>
#include <list>
double f(double x, double y, double t)
{
    return cos(x * 0.01) * sin(y * 0.01) * cos(t * 10) * 100;
}
struct P
{
    P() :x(0), y(0), z(0), vr(0), vg(0), vb(0), r(0),g(0),b(0),weight(1) {}
    double x;
    double y;
    double z;
    double vr,vg,vb;
    double r, g, b;
    double weight;
};
const int Nx = 1920/2,Ny=1080/2;//размерность сетки
const double Kr = 100;//сила распространения
const double Kg = 100;//сила распространения
const double Kb = 100;//сила распространения
const double koefr = 0.999;// 0.999999999;//сила трения передачи волн
const double koefg = 0.995;// 0.999999999;//сила трения передачи волн
const double koefb = 0.990;// 0.999999999;//сила трения передачи волн
const double DTr = 0.01;
const double DTg = 0.01;
const double DTb = 0.01;
const int max = 10;//максимальная сила воздействия
P p[Nx][Ny];
P p_buf[Nx][Ny];
double dist(P p0, P p1)
{
    //return sqrt((p0.x - p1.x) * (p0.x - p1.x) + (p0.y - p1.y) * (p0.y - p1.y) + (p0.z - p1.z) * (p0.z - p1.z));
    //return sqrt((p0.x - p1.x) * (p0.x - p1.x) + (p0.y - p1.y) * (p0.y - p1.y));
    return abs(p0.x - p1.x) + abs(p0.y - p1.y) + abs(p0.z - p1.z);
}
struct Point2D 
{
    double x,y;
    Point2D() :x(0), y(0){}
    Point2D(double _x, double _y)
    {
        x = _x;
        y = _y;
    }
};
Point2D Normalize(Point2D pt)
{
    double lngth = sqrt(pt.x * pt.x + pt.y * pt.y);
    return Point2D(pt.x / lngth, pt.y / lngth);
}
Point2D Perpendicular(Point2D pt)
{
    return Point2D(-pt.y, pt.x);
}
Point2D Average(Point2D pt1, Point2D pt2)
{
    return Point2D((pt1.x + pt2.x) / 2, (pt1.y + pt2.y) / 2);
}
Point2D Minus(Point2D p1, Point2D p2)
{
    return Point2D(p1.x - p2.x, p1.y - p2.y);
}
double dist(Point2D a, Point2D b) { return sqrt((a.x - b.x) * (a.x - b.x) + (a.y - b.y) * (a.y - b.y)); }
const double PI = 3.14159265358979;
Point2D* FillKohaSnowflake(int numOfiter=3)
{
    //0 iter ->    3    points
    //1 iter ->   12    points
    //2 iter ->   48    points
    //n iter -> 3*(4^n) points
    double r = Nx > Ny ? Ny / 3 : Nx / 3;
    Point2D pp1 = Point2D((Nx/2)+(cos(0) * r),      (Ny/2)+(sin(0) * r));
    Point2D pp2 = Point2D((Nx/2)+(cos(2*PI/3) * r), (Ny/2)+(sin(2*PI/3) * r));
    Point2D pp3 = Point2D((Nx/2)+(cos(4*PI/3) * r), (Ny/2)+(sin(4*PI/3) * r));
    Point2D* arr = new Point2D[3 * pow(4, numOfiter)];
    std::list<Point2D> l1;
    std::list<Point2D> l2;
    std::list<Point2D> l3;
    l1.push_back(pp1);
    l1.push_back(pp2);
    l2.push_back(pp2);
    l2.push_back(pp3);
    l3.push_back(pp3);
    l3.push_back(pp1);

    Point2D p1, p2, p3, p4;
    double shit = 0.333;
    double iks = dist(pp1, pp2) * shit;
    for (int i = 0; i < numOfiter; i++)
    {
        int num = l1.size() - 1;
        double ks = iks * pow(shit, i);
        for (int j = 0; j < num; j++)
        {
            p1 = Point2D(l1.front().x, l1.front().y);
            l1.pop_front();
            Point2D v = Normalize(Minus(l1.front(), p1));
            p2 = Point2D(p1.x + v.x * ks, p1.y + v.y * ks);
            float some = (float)(dist(p1, l1.front()) - ks);

            p4 = Point2D(p1.x + v.x * some, p1.y + v.y * some);
            
            p3 = Average(p1, l1.front());

            Point2D normpt = Perpendicular(Normalize(Minus(p1, l1.front())));

            p3.x += normpt.x * ks;
            p3.y += normpt.y * ks;
            l1.push_back(p1);
            l1.push_back(p2);
            l1.push_back(p3);
            l1.push_back(p4);
        }
        p1 = Point2D(l1.front().x, l1.front().y);
        l1.pop_front();
        l1.push_back(p1);
    }
    //for l2:
    for (int i = 0; i < numOfiter; i++)
    {
        int num = l2.size()-1;
        double ks = iks * pow(shit, i);
        for (int j = 0; j < num; j++)
        {
            p1 = Point2D(l2.front().x, l2.front().y);
            l2.pop_front();
            Point2D v = Normalize(Minus(l2.front(), p1));
            p2 = Point2D(p1.x + v.x * ks, p1.y + v.y * ks);
            float some = (float)(dist(p1, l2.front()) - ks);

            p4 = Point2D(p1.x + v.x * some, p1.y + v.y * some);

            p3 = Average(p1, l2.front());

            Point2D normpt = Perpendicular(Normalize(Minus(p1, l2.front())));

            p3.x += normpt.x * ks;
            p3.y += normpt.y * ks;
            l2.push_back(p1);
            l2.push_back(p2);
            l2.push_back(p3);
            l2.push_back(p4);
        }
        p1 = Point2D(l2.front().x, l2.front().y);
        l2.pop_front();
        l2.push_back(p1);
    }
    //for l3:
    for (int i = 0; i < numOfiter; i++)
    {
        int num = l3.size() - 1;
        double ks = iks * pow(shit, i);
        for (int j = 0; j < num; j++)
        {
            p1 = Point2D(l3.front().x, l3.front().y);
            l3.pop_front();
            Point2D v = Normalize(Minus(l3.front(), p1));
            p2 = Point2D(p1.x + v.x * ks, p1.y + v.y * ks);
            float some = (float)(dist(p1, l3.front()) - ks);

            p4 = Point2D(p1.x + v.x * some, p1.y + v.y * some);

            p3 = Average(p1, l3.front());

            Point2D normpt = Perpendicular(Normalize(Minus(p1, l3.front())));

            p3.x += normpt.x * ks;
            p3.y += normpt.y * ks;
            l3.push_back(p1);
            l3.push_back(p2);
            l3.push_back(p3);
            l3.push_back(p4);
        }
        p1 = Point2D(l3.front().x, l3.front().y);
        l3.pop_front();
        l3.push_back(p1);
    }
    //to array
    int arrSize = 3 * pow(4, numOfiter);
    for (int i = 0; i < arrSize / 3; i++)
    {
        arr[i] = Point2D(l1.front().x, l1.front().y);
        l1.pop_front();
    }
    //l2.pop_front();//equals to the last point of l1
    for (int i = arrSize / 3; i < 2* arrSize / 3; i++)
    {
        arr[i] = Point2D(l2.front().x, l2.front().y);
        l2.pop_front();
    }
    //l3.pop_front();//equals to the last point of l2
    for (int i = 2* arrSize / 3; i < arrSize; i++)
    {
        arr[i] = Point2D(l3.front().x, l3.front().y);
        l3.pop_front();
    }
    return arr;
}
bool isInKohaSnowflake(int x, int y, Point2D *vertex, int numOfPoints)
{
    int k = 0;//num of intersections
    double x1 = 0, y1 = 0, x2 = 0, y2 = 0;
    for (int j = 0; j < numOfPoints - 1; j++)
    {
        x1 = vertex[j].x;
        y1 = vertex[j].y;
        x2 = vertex[j + 1].x;
        y2 = vertex[j + 1].y;
        if ((((y1 <= y) && (y < y2)) || ((y2 <= y) && (y < y1))) && (((y2 - y1) != 0) && (x > (((x2 - x1) * (y - y1)) / (y2 - y1) + x1))))
            k++;
    }
    x1 = vertex[0].x;
    y1 = vertex[0].y;
    x2 = vertex[numOfPoints - 1].x;
    y2 = vertex[numOfPoints - 1].y;
    //проверка последней прямой объекта:
    if ((((y1 <= y) && (y < y2)) || ((y2 <= y) && (y < y1))) && (((y2 - y1) != 0) && (x > (((x2 - x1) * (y - y1)) / (y2 - y1) + x1))))
        k++;
    if (k % 2 == 1)
        return true;
    return false;
}
int main()
{//волны на поверхности:
    int statR;
    int statL;
    int statU;
    int statD;
    int statE;
    int statQ;
    GLFWwindow* window;
    int numOfiter = 5;
    Point2D* KohaSnowflake= new Point2D[ 3 * pow(4, numOfiter)];
    KohaSnowflake=FillKohaSnowflake(numOfiter);
    //for (int i = 0; i < 3 * pow(4, numOfiter); i++)
    //{
    //    std::cout << "p" << i << "( " << KohaSnowflake[i].x << ", " << KohaSnowflake[i].y << ")" << std::endl;
    //}
    if (!glfwInit())
        return -1;
    int w = 1920, h = 1080;
    window = glfwCreateWindow(w, h, "wave", glfwGetPrimaryMonitor(), NULL);
    glfwMakeContextCurrent(window);
    float zoom = 1.08;
    glEnable(GL_BLEND);
    glBlendFunc(GL_SRC_ALPHA, GL_ONE);
    if (w > h)
        glScalef((float)(h) / (float)(w), 1.0f, 1.0f);
    else if (h > w)
        glScalef(1.0f, (float)(w+1) / (float)(h+1), 1.0f);
    glScalef(1.0 / zoom, 1.0 / zoom, 1.0);
    glMatrixMode(GL_PROJECTION);
    glLoadIdentity();
    glFrustum(-100, 100, -100, 100, 100, 2000);
    //glFrustum(-100, 100, -100, 100, 100, 1000);
    glMatrixMode(GL_MODELVIEW);
    //glTranslatef(-w / 2, -h / 2, -500);
    glTranslatef(-w/2, -h / 2, -500);
    //glRotatef(-30, 1, 0, 0);
    int rad = Nx > Ny ? Ny / 4 : Nx / 4;
    for (int x = 0; x < Nx; ++x)
        for (int y = 0; y < Ny; ++y)
        {
            p[x][y].x = x * w / Nx;
            p[x][y].y = y * h / Ny;

            //if (x == 5&&y>100&&y<N-100)
            //{
            //    p[x][y].vr = 20;
            //    p[x][y].vg = 20;
            //    p[x][y].vb = 20;
            //}

            //if (sqrt((x - N / 2) * (x - N / 2) + (y - N / 2) * (y - N / 2)) < N / 4) //circle 
            //if (sqrt((x - N / 2) * (x - N / 2) + (y - N / 2) * (y - N / 2)) < N / 4 && !(y>N/2))//semicircle
            //if (sqrt((x - N / 2) * (x - N / 2) + (y - N / 2) * (y - N / 2)) < N / 4 && !(abs(x - y / 2) < N / 4 && abs(x + y / 2) > 3 * N / 4))
            
            //if (sqrt((x - Nx / 2) * (x - Nx / 2) + (y - Ny / 2) * (y - Ny / 2)) < rad && !(abs(x - y / 2) < 3 * Nx / 8 && abs(x + y / 2) > 5 * Nx / 8)) //seggment
            //    p[x][y].weight = 5;
            //else
            //    p[x][y].weight = 1;

            if(isInKohaSnowflake(x,y,KohaSnowflake, 3 * pow(4, numOfiter)))
                p[x][y].weight = 5;
            else
                p[x][y].weight = 1;

            //if (y<Ny-(Ny/4) && abs(x-y/2)<3*Nx/8&& abs(x+y/2)>5*Nx/8)
            //    p[x][y].weight = 2;
            //else
            //    p[x][y].weight = 1;
        }
    //const int dx[] = { -1,0,1,0 ,1,-1,-1,1  };
    //const int dy[] = { 0 ,1,0,-1,1,1 ,-1,-1 };
    const int dx[] = { -1,0,1,0     ,1,-1,-1, 1    ,2,2,2,1,0,-1,-2,-2,-2,-2,-2,-1,0,1,2,2};
    const int dy[] = {  0,1,0,-1    ,1, 1,-1,-1    ,0,1,2,2,2,2,2,1,0,-1,-2,-2,-2,-2,-2,-1};
    double temp;
    double msX, msY;
    int nn = sizeof(dx) / sizeof(int);//num of neighbors
    int dd = sqrt(nn + 1) / 2;
    while (!glfwWindowShouldClose(window))
    {
        for (int x = 0; x < Nx; ++x)
            for (int y = 0; y < Ny; ++y)
            {
                p_buf[x][y].x = p[x][y].x;
                p_buf[x][y].y = p[x][y].y;
                p_buf[x][y].vr = p[x][y].vr;
                p_buf[x][y].vg = p[x][y].vg;
                p_buf[x][y].vb = p[x][y].vb;
                p_buf[x][y].z = p[x][y].z;
                p_buf[x][y].r = p[x][y].r;
                p_buf[x][y].g = p[x][y].g;
                p_buf[x][y].b = p[x][y].b;
                p_buf[x][y].weight = p[x][y].weight;
            }
        glClear(GL_COLOR_BUFFER_BIT);
        glClearColor(0, 0, 0, 1);
        glColor4d(1, 1, 1, 0.1);
        //for (int x = 0; x < N; ++x)
        //{
        //    glBegin(GL_LINE_STRIP);
        //    for (int y = 0; y < N; ++y)
        //    {
        //        //temp = 10 * ((p[x][y].vz) / max);
        //        //glColor4f(1 - temp, 0, 0, 0.5);
        //        //glColor4f(0, 0, 1-temp, 0.5);
        //        //glColor4d(1-temp, 1-temp, 1-temp, 0.5);
        //        glColor4d(abs(10 * ((p[x][y].vr) / max)),
        //                  abs(10 * ((p[x][y].vg) / max)),
        //                  abs(10 * ((p[x][y].vb) / max)), 0.2);
        //
        //        glVertex3d(p[x][y].x, p[x][y].y, p[x][y].z);
        //    }
        //    glEnd();
        //}
        //for (int y = 0; y < N; ++y)
        //{
        //    glBegin(GL_LINE_STRIP);
        //    for (int x = 0; x < N; ++x)
        //    {
        //        //temp = 10 * ((p[x][y].vz) / max);
        //        //glColor4f(0, 0, 1-temp, 0.5);
        //        //glColor4d(1-temp, 1-temp, 1-temp, 0.5);
        //        glColor4d(abs(10 * ((p[x][y].vr) / max)),
        //                  abs(10 * ((p[x][y].vg) / max)),
        //                  abs(10 * ((p[x][y].vb) / max)), 0.2);
        //        glVertex3d(p[x][y].x, p[x][y].y, p[x][y].z);
        //    }
        //    glEnd();
        //}
        for (int x = 0; x < Nx; ++x)
        {
            glBegin(GL_POINTS);
            for (int y = 0; y < Ny; ++y)
            {
                //temp = 10 * ((p[x][y].vz) / max);
                //glColor4f(1 - temp, 0, 0, 0.5);
                //glColor4f(0, 0, 1-temp, 0.5);
                //glColor4d(1-temp, 1-temp, 1-temp, 0.5);
                //glColor4d(abs(10 * ((p[x][y].vr) / max)),
                //          abs(10 * ((p[x][y].vg) / max)),
                //          abs(10 * ((p[x][y].vb) / max)), 0.5);
                glColor4d(abs(p_buf[x][y].vr),
                    abs(p_buf[x][y].vg),
                    abs(p_buf[x][y].vb), 1);

                //glVertex3d(p[x][y].x, p[x][y].y, p[x][y].z);
                //glVertex2d(p[x][y].x, p[x][y].y);
                glVertex2d(p_buf[x][y].x, p_buf[x][y].y);
            }
            glEnd();
        }
        int state = glfwGetMouseButton(window, GLFW_MOUSE_BUTTON_LEFT);
        if (state)
        {
            //glfwGetCursorPos(window, &msX, &msY);
            //msX = Nx * msX / w;
            //msY = Ny - (Ny * msY / h);            
            msX = Nx/2;
            msY = Ny/2;
            int r = Nx > Ny ? Ny / 50 : Nx / 50;
            for (int i = -r; i <= r; i++)
                for (int j = -r; j <= r; j++)
                    if (msX + i > 0 && msX + i < Nx && msY + j>0 && msY + j < Ny)
                    {
                        //if (dist(Point2D(i+ msX, j+ msY), Point2D(msX, msY)) < r)
                        {
                            p[(int)(msX + i)][(int)(msY + j)].vr = max - 2 * (abs((double)i / (double)r) + abs((double)j / (double)r));
                            p[(int)(msX + i)][(int)(msY + j)].vg = max - 2 * (abs((double)i / (double)r) + abs((double)j / (double)r));
                            p[(int)(msX + i)][(int)(msY + j)].vb = max - 2 * (abs((double)i / (double)r) + abs((double)j / (double)r));
                        }
                    }
            //if (msX > 0 && msX < N && msY>0 && msY < N)
            //    p[(int)msX][(int)msY].vz = max;
        }
        state = glfwGetMouseButton(window, GLFW_MOUSE_BUTTON_RIGHT);
        if (state)
        {
            //for (int x = 0; x < N; ++x)
            //    for (int y = 0; y < N; ++y)
            //    {
            //        p[x][y].x = x * w / N;
            //        p[x][y].y = y * h / N;
            //        p[x][y].vr = 0;
            //        p[x][y].vg = 0;
            //        p[x][y].vb = 0;
            //        p[x][y].z = 0;
            //    }
            glfwGetCursorPos(window, &msX, &msY);
            msX = Nx * msX / w;
            msY = Ny - (Ny * msY / h);
            int r = Nx > Ny ? Ny / 50 : Nx / 50;
            for (int i = -r; i <= r; i++)
                for (int j = -r; j <= r; j++)
                    if (msX + i > 0 && msX + i < Nx && msY + j>0 && msY + j < Ny)
                    {
                        p[(int)(msX + i)][(int)(msY + j)].vr = -(max - 2 * (abs((double)i / (double)r) + abs((double)j / (double)r)));
                        p[(int)(msX + i)][(int)(msY + j)].vg = -(max - 2 * (abs((double)i / (double)r) + abs((double)j / (double)r)));
                        p[(int)(msX + i)][(int)(msY + j)].vb = -(max - 2 * (abs((double)i / (double)r) + abs((double)j / (double)r)));
                    }
        }
        for (int x = dd; x < Nx - dd; ++x)
            for (int y = dd; y < Ny - dd; ++y)
            {
                P& p0 = p[x][y];
                double dr = 0, dg = 0, db = 0;
                for (int i = 0; i < nn; ++i)
                {
                    P& p1 = p_buf[x + dx[i]][y + dy[i]];
                    //double d = (p0.x - p1.x) * (p0.x - p1.x) + (p0.y - p1.y) * (p0.y - p1.y) + (p0.z - p1.z) * (p0.z - p1.z);
                    //double d = dist(p0,p1);
                    dr += (p1.r) * 0.999995;
                    dg += (p1.g) * 0.999990;
                    db += (p1.b) * 0.999985;
                    //p0.vr += Kr * ((p1.z - p0.z) / d) * DTr;
                    //p0.vg += Kg * ((p1.z - p0.z) / d) * DTg;
                    //p0.vb += Kb * ((p1.z - p0.z) / d) * DTb;
                    //p0.vr *= koefr;
                    //p0.vg *= koefg;
                    //p0.vb *= koefb;
                }
                //dr /=nn;//*=0.25; 
                //dg /=nn;//*=0.25; 
                //db /=nn;//*=0.25;
                //dr-=p0.r;
                //dg-=p0.g;
                //db-=p0.b;
                //p0.vr += dr;
                //p0.vg += dg;
                //p0.vb += db;
                //p0.vr *= 0.998;
                //p0.vg *= 0.998;
                //p0.vb *= 0.998;

                p0.vr = (p0.vr + ((dr / nn) - p0.r));// * 0.99;
                p0.vg = (p0.vg + ((dg / nn) - p0.g));// * 0.99;
                p0.vb = (p0.vb + ((db / nn) - p0.b));// * 0.99;
            }

        for (int x = 1; x < Nx - 1; ++x)
            for (int y = 1; y < Ny - 1; ++y)
            {
                P& p0 = p[x][y];
                p0.r += (p0.vr / p0.weight);
                p0.g += (p0.vg / p0.weight);
                p0.b += (p0.vb / p0.weight);
                p0.z = p0.r + p0.g + p0.b;
                //p0.z += (p0.vr + p0.vg + p0.vb);
                //if (abs(p0.z) < 0.2)
                //{
                //    if (abs(p0.vr) < 0.2)
                //    {
                //        p0.vr = 0;
                //        p0.z = 0;
                //    }
                //    if (abs(p0.vg) < 0.2)
                //    {
                //        p0.vg = 0;
                //        p0.z = 0;
                //    }
                //    if (abs(p0.vb) < 0.2)
                //    {
                //        p0.vb = 0;
                //        p0.z = 0;
                //    }
                //}
            }
        //statR = glfwGetKey(window, GLFW_KEY_D);
        //statL = glfwGetKey(window, GLFW_KEY_A);
        //statU = glfwGetKey(window, GLFW_KEY_W);
        //statD = glfwGetKey(window, GLFW_KEY_S);
        //statE = glfwGetKey(window, GLFW_KEY_E);
        //statQ = glfwGetKey(window, GLFW_KEY_Q);
        //if (statR || statL || statU || statD || statE || statQ)
        //{
        //    if (statR)glRotated(1,1,0,0);
        //    if (statL)glRotated(1,-1,0,0);
        //    if (statU)glRotated(1,0,1,0);
        //    if (statD)glRotated(1,0,-1,0);
        //    if (statE)glRotated(1,0,0,1);
        //    if (statQ)glRotated(1,0,0,-1);
        //}

        glfwSwapBuffers(window);
        glfwPollEvents();
    }
}