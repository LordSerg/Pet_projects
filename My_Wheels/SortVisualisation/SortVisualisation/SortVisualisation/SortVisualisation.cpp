#include <iostream>
#include <chrono>
#include <thread>
#include<GLFW/glfw3.h>
int maxValue = 512;
int length = 512;
GLFWwindow* window;
void ShowArray(int* arr,int indexSwap1,int indexSwap2)
{
	glClearColor(0, 0, 0, 1);
	glClear(GL_COLOR_BUFFER_BIT);
	glBegin(GL_QUADS);
	for (int i = 0; i < length; i++)
	{
		double x = ((double)(i * 2) / length) - 1;
		double x1 = ((double)((i+1) * 2) / length) - 1;//без щелей
		//double x1 = ((double)(i * 2 + 1) / length) - 1;//с щелями
		double y = -1;
		double y1 = ((double)(arr[i] * 2) / (maxValue + 5)) - 1;
		glColor3d(1-(double)arr[i]/maxValue, (double)arr[i] / maxValue, 1-(double)arr[i] / maxValue);
		if (i == indexSwap1 || i == indexSwap2)
			glColor3d(1, 1, 1);
		glVertex2d(x, y);
		glVertex2d(x, y1);
		glVertex2d(x1, y1);
		glVertex2d(x1, y);
	}
	glEnd();
	glfwSwapBuffers(window);
	glfwPollEvents();
	//std::this_thread::sleep_for(std::chrono::milliseconds(10));
}
void swap(int& a, int& b)
{
	int t = a;
	a = b;
	b = t;
}
void InsertionSort(int* arr)
{
	int key,j;
	for (int i = 1; i < length; i++)
	{
		key = arr[i];
		j = i - 1;
		while (j >= 0 && arr[j] > key)
		{
			arr[j + 1] = arr[j];
			ShowArray(arr, j, j+1);
			j--;
		}
		arr[j + 1] = key;
		ShowArray(arr, i, j+1);
	}
}
void SelectionSort(int* arr)
{
	for (int i = 0; i < length - 1; i++)
		for (int j = i + 1; j < length; j++)
		{
			if (arr[i] > arr[j])
				swap(arr[i],arr[j]);
			ShowArray(arr, i, j);
		}
}
void ExchangeSort(int* arr)//bubble
{
	bool flag;
	for (int i = 1; i < length-1; i++)
	{
		flag = false;
		for (int j = 0; j < length - i; j++)
		{
			if (arr[j] > arr[j + 1])
			{
				swap(arr[j],arr[j+1]);
				flag = true;
			}
			ShowArray(arr, j, j+1);
		}
		if (!flag)
			break;
	}
}
void ShakerSort(int* arr)
{
	int left = 0;
	int right = length;
	bool flag = true;//умова Айверсона
	while (left < right && flag)
	{
		flag = false;
		for (int i = left; i < right - 1; i++)
		{
			if (arr[i] > arr[i + 1])
			{
				swap(arr[i], arr[i+1]);
				flag = true;
			}
			ShowArray(arr, i, i+1);
		}
		right--;
		if (left == right || !flag)
			break;
		flag = false;
		for (int i = right; i>left; i--)
		{
			if (arr[i] < arr[i - 1])
			{
				swap(arr[i], arr[i-1]);
				flag = true;
			}
			ShowArray(arr, i, i-1);
		}
		left++;
		if (left == right || !flag)
			break;
	}
}
void ShellSort(int *arr) //сортировка методом Шелла
{
	for (int interval = length / 2; interval > 0; interval /= 2)
		for (int i = interval; i < length; i++)
		{
			int temp = arr[i];
			int j;
			for (j = i; j >= interval && arr[j - interval] > temp; j -= interval)
			{
				arr[j] = arr[j - interval];
				ShowArray(arr, j, j - interval);
			}
			arr[j] = temp;
			ShowArray(arr, j, i);
		}
}
int Partition(int* arr, int low, int high)
{//разбитие Ломуто
	int pivot = arr[high];
	int j = low;
	for (int i = low; i <= high - 1; i++)
	{
		if (arr[i] <= pivot)
		{
			swap(arr[i],arr[j]);
			j++;
		}
		ShowArray(arr, i, j);
	}
	swap(arr[j],arr[high]);
	ShowArray(arr, high, j);
	return j;
}
void QuickSort(int*arr,int low,int high)
{
	if (low < high)
	{
		int p = Partition(arr, low, high);
		QuickSort(arr,low,p-1);
		QuickSort(arr,p+1,high);
	}
}
void MERGE(int* arr, int low, int mid, int high)
{
	int* b;
	b = new int[length];
	for (int i = low; i <= high; i++)
		b[i] = arr[i];
	int l = low;
	int r = mid + 1;
	int i = low;
	while (l<=mid&&r<=high)
	{
		if (b[l] <= b[r])
		{
			arr[i] = b[l];
			ShowArray(arr, i, l);
			l++;
		}
		else
		{
			arr[i] = b[r];
			ShowArray(arr, i, r);
			r++;
		}
		i++;
	}
	while (l <= mid)
	{
		arr[i] = b[l];
		ShowArray(arr, i, l);
		l++;
		i++;
	}
	while (r <= high)
	{
		arr[i] = b[r];
		ShowArray(arr, i, r);
		r++;
		i++;
	}
}
void MeregeSort(int* arr, int low, int high)
{
	if (low < high)
	{
		int mid = floor((low + high) / 2);
		MeregeSort(arr,low,mid);
		MeregeSort(arr,mid+1,high);
		MERGE(arr,low,mid,high);
	}
}
int main()
{
	if (!glfwInit())
		return -1;
	int w = 700, h = 700;
	window = glfwCreateWindow(w,h,"SortVisualisation",NULL/*glfwGetPrimaryMonitor()*/,NULL);
	glfwMakeContextCurrent(window);
	int a[8192];
	for (int i = 0; i < length; i++)
		a[i] = 1 + i;// rand() % maxValue;
	//glScaled(0.8,1,1);
	/*while (!glfwWindowShouldClose(window))
	{
		glClearColor(0,0,0,1);
		ShowArray(a, 4, 5);
		glfwSwapBuffers(window);
		glfwPollEvents();
	}*/
	int order;
	while (true)
	{
		std::cout << "\n0 - Mix values";
		std::cout << "\n1 - ExchangeSort";
		std::cout << "\n2 - SelectionSort";
		std::cout << "\n3 - ShellSort";
		std::cout << "\n4 - InsertionSort";
		std::cout << "\n5 - ShakerSort";
		std::cout << "\n6 - QuickSort";
		std::cout << "\n7 - MeregeSort";
		std::cout << "\n8 - Exit";
		std::cout << "\nyour choise: ";
		std::cin >> order;
		if (order == 0)
			for (int i = 0; i < length; i++)
			{
				int k = rand() % length;//ai=10; ak=20
				a[i] += a[k];		//ai=30; ak=20
				a[k] = a[i] - a[k]; //ai=30; ak=10
				a[i] -= a[k];		//ai=20; ak=10
			}
		else if (order == 1)ExchangeSort(a);
		else if (order == 2)SelectionSort(a);
		else if (order == 3)ShellSort(a);
		else if (order == 4)InsertionSort(a);
		else if (order == 5)ShakerSort(a);
		else if (order == 6)QuickSort(a, 0, length - 1);
		else if (order == 7)MeregeSort(a, 0, length - 1);
		else if (order == 8)break;
		ShowArray(a,-1,-1);
	}
}