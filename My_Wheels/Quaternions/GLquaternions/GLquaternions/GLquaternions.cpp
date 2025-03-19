#include <iostream>
#include "GLFW/glfw3.h"
struct Quaternion
{
public: double re, i, j, k;
	  Quaternion()
	  {
		  re = i = j = k = 0;
	  }
	  Quaternion(double Re, double I, double J, double K)
	  {
		  re = Re;
		  i = I;
		  j = J;
		  k = K;
	  }
};
struct d4D
{
public:
	double x, y, z, w;
	d4D()
	{
		x = y = z = w = 0;
	}
	d4D(double X, double Y, double Z, double W)
	{
		x = X;
		y = Y;
		z = Z;
		w = W;
	}
};
struct Color
{
public:
	double r, g, b;
	Color() { r = g = b = 0; }
	Color(double R, double G, double B)
	{
		r = R;
		g = G;
		b = B;
	}
};
int num_of_points_to_show = 0;
const int size = 400;
const long array_sizes = 300 * 300 * 300;
d4D all[array_sizes];
//d4D a[array_sizes/100];
Color colors[array_sizes];
//Color c[array_sizes/100];
/*void reculc(double c1, double c2)
{
	num_of_points_to_show = 0;
	for (int index = 0; index < size * size * size; index++)
	{
		if (colors[index].r + colors[index].g + colors[index].b > c1
			&& colors[index].r + colors[index].g + colors[index].b < c2)
		//if(colors[index].r>c1 && colors[index].r < c2
		//|| colors[index].g>c1 && colors[index].g < c2
		//|| colors[index].b>c1 && colors[index].b < c2)
		{
			a[num_of_points_to_show] = all[index];
			c[num_of_points_to_show] = colors[index];//RGB(200, 200, 200);
			num_of_points_to_show++;
		}
	}
}*/
int main()
{
    if (!glfwInit())
        return -1;
	std::cout << "Calculating...";
    //делаем точки для отображения:
	int n = 50, radius = 30, index = 0;
	double zoom = 0.8;
	double koef = 2;
	double some_const1 = 1.5,some_const2=1.7;//ограничители
	Quaternion q1, q2, qc;
	qc = Quaternion(-0.49245, 0.5389555, -0.31145005, 0.054079213);
	int l;
	int alpha;
	for (int i = 0; i < size; i++)
		for (int j = 0; j < size; j++)
			for (int k = 0; k < size; k++)
				//for (int l = 0; l < size; l++)
			{
				l = size / 2;
				q1 = Quaternion((i - size / 2) / (0.5 * zoom * size),
					(j - size / 2) / (0.5 * zoom * size),
					(k - size / 2) / (0.5 * zoom * size),
					(l - size / 2) / (0.5 * zoom * size));
				for (alpha = 0; alpha < n; alpha++)
				{
					q2 = q1;
					q1 = Quaternion(
						q2.re * q2.re - q2.i * q2.i - q2.j * q2.j - q2.k * q2.k + qc.re,
						2 * q2.re * q2.i + qc.i,
						2 * q2.re * q2.j + qc.j,
						2 * q2.re * q2.k + qc.k);
					if (q1.re * q1.re + q1.i * q1.i + q1.j * q1.j + q1.k * q1.k > radius)
						break;
				}

				all[index] = d4D((double)(i - size / 2) * koef/size, (double)(j - size / 2) * koef / size, (double)(k - size / 2) * koef / size, (double)(l - size / 2) * koef / size);
				colors[index] = Color((double)((alpha * 51) % 255) / 255, (double)((alpha * 74) % 255) / 255, (double)((alpha * 60) % 255) / 255);
				if ((double)((alpha * 51) % 255) / 255+ (double)((alpha * 74) % 255) / 255+ (double)((alpha * 60) % 255) / 255 > some_const1
					&& (double)((alpha * 52) % 255) / 255 + (double)((alpha * 74) % 255) / 255 + (double)((alpha * 63) % 255) / 255 < some_const2)
					index++;
			}
	//reculc(some_const1, some_const2);
	for (int i = 0; i<index ; i++)
	{
		if (colors[i].r + colors[i] .g+ colors[i] .b> some_const1
			&& colors[i].r + colors[i].g + colors[i].b < some_const2)
		{
			all[num_of_points_to_show] = all[i];
			colors[num_of_points_to_show] = colors[i];//RGB(200, 200, 200);
			num_of_points_to_show++;
		}
	}
	std::cout << "\na,w,s,d,q,e - are for rotating";
	//делаем окно:
    GLFWwindow* window;
    int width = 1024, height = 1024;	
    window = glfwCreateWindow(width, height, "", /*glfwGetPrimaryMonitor()*/NULL, NULL);
    glfwMakeContextCurrent(window);
    glEnable(GL_BLEND);
    glBlendFunc(GL_SRC_ALPHA, GL_ONE);
    //отображаем+поворачиваем
	int stateR, stateL, stateU, stateD,stateRot1,stateRot2;
	int interval = 10;
    while (!glfwWindowShouldClose(window))
    {
		//glfwSetWindowTitle(window, (const char*)((int)(some_const1 * 100)));
        glClear(GL_COLOR_BUFFER_BIT);
        glClearColor(0, 0, 0, 1);
		glBegin(GL_POINTS);
		for (index = 0; index < num_of_points_to_show; index++)
		{
			//field4D::set_pixel(a[index], hdc, c[index], size * koef);
			//field4D::set_pixel(a[num_of_points_to_show-index], hdc, c[num_of_points_to_show-index], size * koef);
		
			glColor4d(colors[index].r, colors[index].g, colors[index].b,0.4);
			glVertex3d(all[index].x, all[index].y, all[index].z/*, a[index].w*/);
		}
		glEnd();

		stateR = glfwGetKey(window, GLFW_KEY_RIGHT);
		stateL = glfwGetKey(window, GLFW_KEY_LEFT);
		stateU = glfwGetKey(window, GLFW_KEY_UP);
		stateD = glfwGetKey(window, GLFW_KEY_DOWN);
		stateRot1 = glfwGetKey(window, GLFW_KEY_Q);
		stateRot2 = glfwGetKey(window, GLFW_KEY_E);
		if (stateR || stateL || stateU || stateD || stateRot1 || stateRot2)
			glRotated(0.4, stateR - stateL, stateU - stateD, stateRot1 - stateRot2);

		stateR = glfwGetKey(window, GLFW_KEY_D);
		stateL = glfwGetKey(window, GLFW_KEY_A);
		stateU = glfwGetKey(window, GLFW_KEY_W);
		stateD = glfwGetKey(window, GLFW_KEY_S);
		if (stateR || stateL || stateU || stateD)
			glRotated(0.4, stateR - stateL, stateU - stateD, stateRot1 - stateRot2);
        glfwSwapBuffers(window);
        glfwPollEvents();
    }
}