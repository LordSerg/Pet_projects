#include <GLFW/glfw3.h>
#include <iostream>
#include <omp.h>
#include <cmath>
struct p
{
    float x, y, z;
    p() { x = y = z = 0; }
    p(float _x, float _y, float _z)
    {
        x = _x;
        y = _y;
        z = _z;
    }
};
struct p4
{
    float x, y, z, u;
    p4() { x = y = z = u = 0; }
    p4(float _x, float _y, float _z, float _u)
    {
        x = _x;
        y = _y;
        z = _z;
        u = _u;
    }
};
void initValues(int i,//индекс аттрактора
    float &k,//масштаб
    float &kx, float &ky, float &kz,//масштаб камеры
    float &speedMultiplier)//скорость
{
    if (i == 0)
    {//thomas
        k = 0.1;//0.1
        speedMultiplier = 0.05;//0.1;
        kx = ky = kz = 0.2;//0.1
    }
    else if (i == 1)
    {//lorenz
        k = 0.0001;//0.001;
        speedMultiplier = 0.005;
        kx = ky = kz = 0.0175;
    }
    else if (i == 2)
    {//aizava
        k = 0.001;
        speedMultiplier = 0.005;//0.005;
        kx = ky = kz = 0.48;
    }
    else if (i == 3)
    {//chen
        k = 0.05;
        speedMultiplier = 0.005;
        kx = ky = kz = 0.015;
    }
    else if (i == 4)
    {//spott
        k = 0.001;
        speedMultiplier = 0.04;// 0.045;
        kx = ky = kz = 0.3;
    }
    else if(i==5)
    {//some
        k = 0.0001;
        speedMultiplier = 5.5;
        kx = ky = kz = 0.3;
    }
    else if (i == 6)
    {
        k = 0.01;
        speedMultiplier = 0.001;
        kx = ky = kz = 1.0;
    }
}
float mysin(float a)
{
    float a2=a*a;
    float a3=a2*a;
    float a5=a3*a2;
    float a7=a5*a2;
    float a9=a7*a2;
    return a - (a3) / 6 + (a5) / 120 - (a7) / 5040 +(a9) / 362880;
}
//запускать точки в масштабе от (any), скорость ~ 0.1, масштаб камеры ~ 0.1
void some_system(p& a, float speedMultiplier)
{
    float ax = a.x, ay = a.y, az = a.z;
    //a.x += 3 * ax + ax * (ax - 3) * (5 * ay * ay - az * az) / (1 + ay * ay + az * az);
    //a.y += 2 * ay - 14 * az - 5 * (ax - 3) * ay;
    //a.z += 14 * ay + 2 * az + 5 * (ax - 3) * az;

    a.x += 4000 - 7 * ax - 1000 * ay * ay;
    a.y += ay + 2 * az + ax * (ay + 3 * az);
    a.z += -2 * ay + az + ax * (-3 * ay + az);

    //a.x += (3*ax-10*az+20*ay*ay-az*az) * speedMultiplier;//10
    //a.y += (10*az-20*ax*ay) * speedMultiplier;//28
    //a.z += (10*ax-10*ay+20*ax*az) * speedMultiplier;//2.6
}
void thomas(p& a, float speedMultiplier)
{
    float ax = a.x, ay = a.y, az = a.z;
    /*
    a.x += (mysin(ay) - 0.2 * ax) * speedMultiplier;
    a.y += (mysin(az) - 0.2 * ay) * speedMultiplier;
    a.z += (mysin(ax) - 0.2 * az) * speedMultiplier;
    */
    a.x += (std::sin(ay) - 0.2 * ax) * speedMultiplier;
    a.y += (std::sin(az) - 0.2 * ay) * speedMultiplier;
    a.z += (std::sin(ax) - 0.2 * az) * speedMultiplier;
}
void thomas (float& x,float& y,float &z,float speedMultiplier)
{
    float ax = x, ay = y, az = z;
    x += (sin(ay) - 0.2 * ax) * speedMultiplier;
    y += (sin(az) - 0.2 * ay) * speedMultiplier;
    z += (sin(ax) - 0.2 * az) * speedMultiplier;
}
float dx_thomas(float x, float y, float z, float koef=0.2) {return  sin(y)-koef*x;}//koef=0.2
float dy_thomas(float x, float y, float z, float koef=0.2) {return  sin(z)-koef*y;}//koef=0.2
float dz_thomas(float x, float y, float z, float koef=0.2) {return  sin(x)-koef*z;}//koef=0.2
//запускать точки в масштабе от (any), скорость ~ 0.01, масштаб камеры ~ 0.0175
void lorenz(p& a, float speedMultiplier)
{
    float ax = a.x, ay = a.y, az = a.z;
    //a.x += ((-2) * (ay - ax)) * speedMultiplier;//10
    //a.y += (ax * ((30) - az) - ay) * speedMultiplier;//28
    //a.z += (ax * ay - (1.9) * az) * speedMultiplier;//2.6
    a.x += ((10) * (ay - ax)) * speedMultiplier;//10
    a.y += (ax * ((28) - az) - ay) * speedMultiplier;//28
    a.z += (ax * ay - (2.6) * az) * speedMultiplier;//2.6
}
void lorenz(float& x, float& y, float& z, float speedMultiplier)
{
    float ax = x, ay = y, az = z;
    x += (10 * (ay - ax)) * speedMultiplier;
    y += (ax * (28 - az) - ay) * speedMultiplier;
    z += (ax * ay - 2.6 * az) * speedMultiplier;
}
float dx_lorenz(float x, float y, float z, float koef=10) { return koef * (y - x); }//koef=10
float dy_lorenz(float x, float y, float z, float koef=28) { return x * (koef - z) - y; }//koef=28
float dz_lorenz(float x, float y, float z, float koef=2.6) { return x * y - koef * z; }//koef=8/3
//запускать точки в масштабе от (any), скорость ~ 0.01, масштаб камеры ~ 0.4
void aizava(p& a, float speedMultiplier)
{
    float ax = a.x, ay = a.y, az = a.z;
    a.x += ((az * ax - 0.7 * ax) - (3.5 * ay)) * speedMultiplier;
    a.y += ((3.5 * ax) + (az * ay - 0.7 * ay)) * speedMultiplier;
    a.z += (0.6 + (0.95 * az) - (az * az * az / 3) - (ax * ax + ay * ay) * (0.25 * az + 1) + (0.1 * az * ax * ax * ax)) * speedMultiplier;
}
void aizava(float& x, float& y, float& z, float speedMultiplier)
{
    float ax = x, ay = y, az = z;
    x += ((az * ax - 0.7 * ax) - (3.5 * ay)) * speedMultiplier;
    y += ((3.5 * ax) + (az * ay - 0.7 * ay)) * speedMultiplier;
    z += (0.6 + (0.95 * az) - (az * az * az / 3) - (ax * ax + ay * ay) * (0.25 * az + 1) + (0.1 * az * ax * ax * ax)) * speedMultiplier;
}
float dx_aizava(float x, float y, float z, float k1=0.7,float k2=3.5) { return (z * x -k1 * x)-(k2*y); }//k1=0.7,k2=3.5
float dy_aizava(float x, float y, float z, float k1=3.5,float k2=0.7) { return (k1*x)+(z * y -k2 * y); }//k1=3.5,k2=0.7
float dz_aizava(float x, float y, float z, float k1=0.6,float k2=0.95,float k3=0.25,float k4=0.1) { return k1+(k2*z)-(z*z*z/3)-(x*x+y*y)*(k3*z+1)+(k4*z*x*x*x); }//k1=0.6,k2=0.95,k3=0.25,k4=0.1
//запускать точки в масштабе от (any), скорость ~ 0.005, масштаб камеры ~ 0.015
void chen(p& a, float speedMultiplier)
{
    float ax = a.x, ay = a.y, az = a.z;
    a.x += (5 * ax - ay * az) * speedMultiplier;
    a.y += ((-10) * ay + ax * az) * speedMultiplier;
    a.z += ((-0.38) * az + ax * ay / 3) * speedMultiplier;
}
void chen(float& x, float& y, float& z, float speedMultiplier)
{
    float ax = x, ay = y, az = z;
    x += (5 * ax - ay * az) * speedMultiplier;
    y += ((-10) * ay + ax * az) * speedMultiplier;
    z += ((-0.38) * az + ax * ay / 3) * speedMultiplier;
}
float dx_chen(float x, float y, float z, float koef=5) { return koef*x-y*z; }//koef=5
float dy_chen(float x, float y, float z, float koef=-10) { return koef*y+x*z; }//koef=-10
float dz_chen(float x, float y, float z, float koef=-0.38) { return koef*z+x*y/3; }//koef=-0.38
//запускать точки в масштабе от (any), скорость ~ 0.05, масштаб камеры ~ 0.3
void sprott(p& a, float speedMultiplier)
{
    float ax = a.x, ay = a.y, az = a.z;
    a.x += (ay + 2.07 * ax * ay + ax * az) * speedMultiplier;
    a.y += (1 - 1.79 * ax * ax + ay * az) * speedMultiplier;
    a.z += (ax - ax * ax - ay * ay) * speedMultiplier;
}
void sprott(float& x, float& y, float& z, float speedMultiplier)
{
    float ax = x, ay = y, az = z;
    x += (ay + 2.07 * ax * ay + ax * az) * speedMultiplier;
    y += (1 - 1.79 * ax * ax + ay * az) * speedMultiplier;
    z += (ax - ax * ax - ay * ay) * speedMultiplier;
}
float dx_sprott(float x, float y, float z, float koef=2.07) { return y+koef*x*y+x*z; }//koef=
float dy_sprott(float x, float y, float z, float koef=1.79) { return 1-koef*x*x+y*z; }//koef=
float dz_sprott(float x, float y, float z) { return x-x*x-y*y; }//
const int num = 100000;
//float ax[num];
//float ay[num];
//float az[num];


float a10 = 0.1;
float a20 = 0;
float a30 = 0;
float a40 = -0.2;
float a12 = -3;
float a13 = 1;
float a14 = 1;
float a23 = 2;
float a24 = 0;
float a34 = 4;
float a11 = 0.00562;
float a22 = -0.0494;
float a33 = -0.00793;
float a44 = 0.00924;
float k1 = -1.01;
float k2 = -1.01;
float k3 = -1.05;
float k4 = -1.5;
void SuperSystem(p4& a, float speedMultiplier)
{
    float ax = a.x, ay = a.y, az = a.z, au = a.u;
    //a.x += ((-2) * (ay - ax)) * speedMultiplier;//10
    //a.y += (ax * ((30) - az) - ay) * speedMultiplier;//28
    //a.z += (ax * ay - (1.9) * az) * speedMultiplier;//2.6

    a.x += (a10 + a11 * (abs(ax) * ax + k1 * ax) + a12 * (abs(ay) * ay + k2 * ay) + a13 * (abs(az) * az + k3 * az) + a14 * (abs(au) * au + k4 * au))*speedMultiplier;
    a.y += (a20 - a12 * (abs(ax) * ax + k1 * ax) + a22 * (abs(ay) * ay + k2 * ay) + a23 * (abs(az) * az + k3 * az) + a24 * (abs(au) * au + k4 * au))*speedMultiplier;
    a.z += (a30 - a13 * (abs(ax) * ax + k1 * ax) - a23 * (abs(ay) * ay + k2 * ay) + a33 * (abs(az) * az + k3 * az) + a34 * (abs(au) * au + k4 * au))*speedMultiplier;
    a.u += (a40 - a14 * (abs(ax) * ax + k1 * ax) - a24 * (abs(ay) * ay + k2 * ay) - a34 * (abs(az) * az + k3 * az) + a44 * (abs(au) * au + k4 * au))*speedMultiplier;
    //a.x += ((10) * (ay - ax)) * speedMultiplier;//10
    //a.y += (ax * ((28) - az) - ay) * speedMultiplier;//28
    //a.z += (ax * ay - (2.6) * az) * speedMultiplier;//2.6
}


p4 arr4[num];

int main()                                                                    
{
    if (!glfwInit())
        return -1;
    //std::cout << "right/left arrow - is for rotation\nup/down arrow - is for zooming in/out\n";
    //std::system("pause");
    int w = 1024, h = 1024;
    GLFWmonitor* monitor = glfwGetPrimaryMonitor();
    //float xscale=1280, yscale=1024;
    //glfwGetMonitorContentScale(monitor, &xscale, &yscale);
    GLFWwindow* window = glfwCreateWindow(w, h, "", /*monitor*/NULL, NULL);
    glfwMakeContextCurrent(window);
    glEnable(GL_BLEND);
    glBlendFunc(GL_SRC_ALPHA, GL_ONE);
    float tx, ty, tz=0;
    float k;//масштаб
    float kx, ky, kz;//масштаб камеры
    float speedMultiplier;//скорость
    //!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!помен€л аттрактор - помен€й индекс
    initValues(6,k,kx,ky,kz,speedMultiplier);
    //for (int i = 0; i < num; i++)
    //    arr[i] = p((rand() % 200 - 100) * k, (rand() % 200 - 100) * k, (rand() % 200 - 100) * k);
    for (int i = 0; i < num; i++)
          arr4[i] = p4((rand() % 200 - 100) * k, (rand() % 200 - 100) * k, (rand() % 200 - 100) * k, (rand() % 200 - 100) * k);
    float msX, msY;
    //glPointSize(0.005f);
    float a=1, b=3, c=-0.38;
    int q_k, e_k, a_k, d_k, z_k, c_k, plus, minus, lft, rght, up,dn;
    glScaled((float)h/(float)w,1,1);
    while (!glfwWindowShouldClose(window))
    {
        glClear(GL_COLOR_BUFFER_BIT);
        glClearColor(0,0,0,0);
        //glClearColor(0.46, 0.46, 0.46, 1);
        //glClearColor(1,1,1, 1);
        //glColor3d(1, 1, 1);
        //glBegin(GL_POINTS);
        glBegin(GL_POINTS);
//#pragma omp parallel shared(arr)
        {
            int i = 0;
            while (i < num)
            {
                //some_system(arr[i], speedMultiplier);
                //thomas(arr[i], speedMultiplier);
                SuperSystem(arr4[i],speedMultiplier);
                
                //if(abs(arr[i].x)>5|| abs(arr[i].y) > 5 || abs(arr[i].z) > 5)
                //    arr[i] = p((rand() % 200 - 100) * k, (rand() % 200 - 100) * k, (rand() % 200 - 100) * k);
                //glColor3d(1, (i) % 30 == 0, 0);
                //glColor4d((i)%50==0, (i)%30==0, (i)%100==0, 0.9);
                glColor4d(0, 1, (i)%30==0,0.6);
                //glColor4d((i)%20==0, (i)%30==0, 1,0.02);
                //glColor4f(1,1,1,1);
                //glColor3d((i)%1==0, (i)%1==0, (i)%30==0);
                //glColor4d((i)%50==0, (i)%1==1, (i)%1==1, 0.2);
                
                //glColor4d(abs(arr[i-1].x+arr[i].x) / 4, abs(arr[i].y) / 2, abs(arr[i].z) / 2, 1);
                //glColor4d(abs(static_cast<int>(arr[i].x * i)>>i) % 2 == 0, 
                //          abs(static_cast<int>(arr[i].y * i)>>i) % 3 == 0, 
                //          abs(static_cast<int>(arr[i].z * i)>>i) % 2 == 0, 1);
                /*
                float dx = abs(dx_thomas(arr[i].x, arr[i].y, arr[i].z));
                float dy = abs(dy_thomas(arr[i].x, arr[i].y, arr[i].z));
                float dz = abs(dz_thomas(arr[i].x, arr[i].y, arr[i].z));
                float speed = std::sqrt(dx * dx + dy * dy * dz * dz);
                
                glColor4d(dx, dy, dz,0.5);
                */

                //glColor3d((i)%1==0, (i)%1==0, (i)%1==0);

                glVertex3d(arr4[i].x * kx, arr4[i].y * ky, arr4[i].z * kz);
                i++;
            }
        }
        glEnd();
        //glfwGetCursorPos(window, &msX, &msY);
        //msX = 1 * msX / w;
        //msY = 1 - (1 * msY / h);
        int state = glfwGetMouseButton(window, GLFW_MOUSE_BUTTON_LEFT);
        if (state == GLFW_PRESS)
            for (int i = 0; i < num; i++)
                arr4[i] = p4((rand() % 200 - 100) * k, (rand() % 200 - 100) * k, (rand() % 200 - 100) * k, (rand() % 200 - 100) * k);
        //q_k = glfwGetKey(window, GLFW_KEY_Q);
        //e_k = glfwGetKey(window, GLFW_KEY_E);
        //a_k = glfwGetKey(window, GLFW_KEY_A);
        //d_k = glfwGetKey(window, GLFW_KEY_D);
        //z_k = glfwGetKey(window, GLFW_KEY_Z);
        //c_k = glfwGetKey(window, GLFW_KEY_C);
        plus = glfwGetKey(window, GLFW_KEY_Q);
        minus = glfwGetKey(window, GLFW_KEY_E);
        lft = glfwGetKey(window, GLFW_KEY_LEFT);
        rght = glfwGetKey(window, GLFW_KEY_RIGHT);
        up = glfwGetKey(window, GLFW_KEY_UP);
        dn = glfwGetKey(window, GLFW_KEY_DOWN);
        //if (q_k) { a -= 0.001; }//0.0001
        //if (e_k) { a += 0.001; }
        //if (a_k) { b -= 0.001; }
        //if (d_k) { b += 0.001; }
        //if (z_k) { c -= 0.001; }
        //if (c_k) { c += 0.001; }
        if (plus) 
        {
            kx += 0.005;
            ky += 0.005;
            kz += 0.005;
        }
        if (minus) 
        {
            kx -= 0.005; 
            ky -= 0.005; 
            kz -= 0.005; 
        }
        if (lft) glRotated(1, 0, 1, 0);
        if (rght)glRotated(1, 0, -1, 0);
        if (up)  glRotated(1, 1, 0, 0);
        if (dn)  glRotated(1, -1, 0, 0);
        glfwSwapBuffers(window);
        glfwPollEvents();
    }
}