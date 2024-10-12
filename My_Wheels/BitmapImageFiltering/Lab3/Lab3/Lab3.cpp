#include "imgui.h"
#include "imgui_impl_glfw.h"
#include "imgui_impl_opengl3.h"

#include <iostream>
#include <GLFW/glfw3.h>
#include <GLFW/glfw3native.h>
#include <cmath>
#include <fstream>
#include <cstring>
#include <string>
#include <Windows.h>
#include <shobjidl.h>
//інформація про файл
std::wstring sFilePath;//путь до файлу
char* sSelectedFile;//назва файлу
int w = 0, h = 0;      //ширина/висота зображення
int BytesPerPixel = 0; //кількість байтів на один піксель
int fileSize = 14;     //розмір всього файлу
int data_index = 14;   //зсув, з якого починаються дані
bool is_bmp = true;
//гістограми кольорів:
float* colorTable; //= new float[256];
float* colorTableR;// = new float[256];
float* colorTableG;// = new float[256];
float* colorTableB;// = new float[256];
//для масштабу гістограм
int maxNumOfColorsInColorTable = 0;
int maxNumOfColorsInColorTableR = 0;
int maxNumOfColorsInColorTableG = 0;
int maxNumOfColorsInColorTableB = 0;
bool is_with_badding = false;//чи є зсув
int padding = 0;//кількість бітів для зсуву
//для точкових перетворень
bool* is_show_transformation;//масив, який відповідає, чи показувати і-ту точкову трансформацію
bool* is_show_filter;// масив, який відповідає, чи показувати і-ту фільтрацію
std::string GetCurrentDirectory()
{
    char buffer[MAX_PATH];
    GetModuleFileNameA(NULL, buffer, MAX_PATH);
    std::string::size_type pos = std::string(buffer).find_last_of("\\/");

    return std::string(buffer).substr(0, pos);
}
unsigned char* read_bmp_file(std::wstring path)
{
    std::ifstream f(path, std::ios::binary | std::ios::in);
    char c;
    int num;
    int index = 0;
    int buffer[4];
    int buffer2[2];
    int numOfBitsPerPixel = 0;
    fileSize = 14;
    data_index = 14;
    BytesPerPixel = 0;
    w = 0;
    h = 0;
    unsigned char* arr = new unsigned char[1];
    maxNumOfColorsInColorTable = 0;
    maxNumOfColorsInColorTableR = 0;
    maxNumOfColorsInColorTableG = 0;
    maxNumOfColorsInColorTableB = 0;
    colorTable = new float[255];
    colorTableR = new float[255];
    colorTableG = new float[255];
    colorTableB = new float[255];
    for (int i = 0; i < 256; i++)colorTable[i] = 0;
    for (int i = 0; i < 256; i++)colorTableR[i] = 0;
    for (int i = 0; i < 256; i++)colorTableG[i] = 0;
    for (int i = 0; i < 256; i++)colorTableB[i] = 0;

    int tempColor = 0;
    is_with_badding = false;
    //bool is_color_table = false;

    int MaXnUmBeRoFtHeMaLl = -10000;
    int MiNnUmBeRoFtHeMaLl = 10000;

    while (f.get(c))
    {
        num = 0;
        num = (int)c < 0 ? (256 + (int)c) : (int)c;
        //безпосередні данні:
        if (index >= data_index)
        {
            //arr[index] = c;
            if (num > MaXnUmBeRoFtHeMaLl)MaXnUmBeRoFtHeMaLl = num;
            if (num < MiNnUmBeRoFtHeMaLl)MiNnUmBeRoFtHeMaLl = num;
            arr[index] = c;
            if (BytesPerPixel == 1)
            {
                colorTable[num]++;
                if (maxNumOfColorsInColorTable < colorTable[num])
                    maxNumOfColorsInColorTable = colorTable[num];
            }
            else if (BytesPerPixel == 3)
            {
                if ((index - data_index) % 3 == 0)
                {
                    colorTableB[num]++;
                    if (maxNumOfColorsInColorTableB < colorTableB[num])
                        maxNumOfColorsInColorTableB = colorTableB[num];
                }
                else if ((index - data_index) % 3 == 1)
                {
                    colorTableG[num]++;
                    if (maxNumOfColorsInColorTableG < colorTableG[num])
                        maxNumOfColorsInColorTableG = colorTableG[num];
                }
                else if ((index - data_index) % 3 == 2)
                {
                    colorTableR[num]++;
                    if (maxNumOfColorsInColorTableR < colorTableR[num])
                        maxNumOfColorsInColorTableR = colorTableR[num];
                }
            }
            if (w * h * BytesPerPixel == fileSize - data_index)
                is_with_badding = false;
            else
                is_with_badding = true;
        }
        else
        {
            //читаємо голову файлу:
            //чи є той файл bmp-шником:
            if (index == 0 && num != 66) { is_bmp = false; break; }
            if (index == 1 && num != 77) { is_bmp = false; break; }
            //скільки місця займає файл (розмір):
            if (index >= 2 && index < 6)
                buffer[index - 2] = num;
            if (index == 5)
            {
                int sz = 0;
                for (int i = 3; i >= 0; --i)
                    sz += ((buffer[i] & 0b00001111) + (buffer[i] >> 4) * 16) * pow(16, i * 2);
                std::cout << "file size = " << sz << "\n";
                fileSize = sz;
                arr = new unsigned char[sz];
                arr[0] = 0x42;
                arr[1] = 0x4D;
                for (int i = 0; i < 4; ++i)
                    arr[i + 2] = buffer[i];
            }
            //те, з якої точки у файлі починаються дані:
            if (index >= 10 && index < 14)
                buffer[index - 10] = num;
            if (index == 13)
            {
                int wd = 0;
                for (int i = 3; i >= 0; --i)
                    wd += ((buffer[i] & 0b00001111) + (buffer[i] >> 4) * 16) * pow(16, i * 2);
                std::cout << "where data starts = " << wd << "\n";
                data_index = wd;
            }
            //інфа про файл:
            //ширина:
            if (index >= 18 && index < 22)
                buffer[index - 18] = num;
            if (index == 21)
            {
                w = 0;
                for (int i = 3; i >= 0; --i)
                    w += ((buffer[i] & 0b00001111) + (buffer[i] >> 4) * 16) * pow(16, i * 2);
                std::cout << "width = " << w << "\n";
            }
            //висота:
            if (index >= 22 && index < 26)
                buffer[index - 22] = num;
            if (index == 25)
            {
                h = 0;
                for (int i = 3; i >= 0; --i)
                    h += ((buffer[i] & 0b00001111) + (buffer[i] >> 4) * 16) * pow(16, i * 2);
                std::cout << "height = " << h << "\n";
            }
            //глибина кольору:
            if (index >= 28 && index < 30)
                buffer2[index - 28] = num;
            if (index == 29)
            {
                numOfBitsPerPixel = 0;
                for (int i = 1; i >= 0; --i)
                    numOfBitsPerPixel += ((buffer2[i] & 0b00001111) + (buffer2[i] >> 4) * 16) * pow(16, i * 2);
                std::cout << "Number of bits per pixel = " << numOfBitsPerPixel << "\n";
                BytesPerPixel = (numOfBitsPerPixel >= 8) ? numOfBitsPerPixel / 8 : numOfBitsPerPixel;
            }
            //кількість кольорів:
            if (index >= 46 && index < 50)
                buffer[index - 46] = num;
            if (index == 49)
            {
                int sz = 0;
                for (int i = 3; i >= 0; --i)
                    sz += ((buffer[i] & 0b00001111) + (buffer[i] >> 4) * 16) * pow(16, i * 2);
                std::cout << "Number of colors = " << sz << "\n";
            }

            //паралельно записуємо файл:
            if (index >= 6)
                arr[index] = c;
        }
        ++index;
    }
    if (!is_bmp)
        return 0;
    padding = abs((w * h * BytesPerPixel - (fileSize - data_index)) / h);//кількість відступів у байтах
    if (is_with_badding)
        std::cout << "With padding (padding = " << padding << ")\n";
    else
        std::cout << "Without padding\n";
    f.close();
    return arr;
}
bool openFile()
{
    //створюємо об'єкт для зчитування
    HRESULT f_SysHr = CoInitializeEx(NULL, COINIT_APARTMENTTHREADED | COINIT_DISABLE_OLE1DDE);
    if (FAILED(f_SysHr))
        return false;
    //створюємо об'єкт для файлового діалогу
    IFileOpenDialog* f_FileSystem;
    f_SysHr = CoCreateInstance(CLSID_FileOpenDialog, NULL, CLSCTX_ALL, IID_IFileOpenDialog, reinterpret_cast<void**>(&f_FileSystem));
    if (FAILED(f_SysHr)) 
    {
        CoUninitialize();
        return false;
    }
    //відкриваємо вікно для вибору файла
    f_SysHr = f_FileSystem->Show(NULL);
    if (FAILED(f_SysHr)) 
    {
        f_FileSystem->Release();
        CoUninitialize();
        return false;
    }
    //зчитуємо повне ім'я файлу
    IShellItem* f_Files;
    f_SysHr = f_FileSystem->GetResult(&f_Files);
    if (FAILED(f_SysHr)) 
    {
        f_FileSystem->Release();
        CoUninitialize();
        return false;
    }

    //зберігаємо ім'я файлу
    PWSTR f_Path;
    f_SysHr = f_Files->GetDisplayName(SIGDN_FILESYSPATH, &f_Path);
    if (FAILED(f_SysHr)) {
        f_Files->Release();
        f_FileSystem->Release();
        CoUninitialize();
        return false;
    }
    std::wstring path(f_Path);
    std::wstring ws(path);
    sFilePath = ws;
    
    std::string c(path.begin(), path.end());
    //std::string c = path;
    //std::wcout << path;
    //for (unsigned char ch : path)
    //    c += ch;
    //sFilePath = c;
    
    const size_t slash = c.find_last_of("/\\");
    std::wstring temp = sFilePath.substr(slash + 1);
    std::string fIlEnaAmE = "";
    for (unsigned char ch : temp)
        fIlEnaAmE += ch;
    sSelectedFile = new char[fIlEnaAmE.length()+1];
    for (int i = 0; i < fIlEnaAmE.length(); i++)
        sSelectedFile[i] = fIlEnaAmE[i];
    sSelectedFile[fIlEnaAmE.length()] = '\0';
    //очищуємо пам'ять та всьо
    CoTaskMemFree(f_Path);
    f_Files->Release();
    f_FileSystem->Release();
    CoUninitialize();
    return true;
}
unsigned char linearFunction(unsigned char ch, unsigned char fmax, unsigned char fmin, unsigned char gmax = 255, unsigned char gmin = 0)
{
    float a = ((float)(ch - fmin) * ((float)(gmax - gmin) / (fmax - fmin)) + gmin);
    if (a > 255)
        return (unsigned char)(255);
    else if (a < 0)
        return (unsigned char)(0);
    return (unsigned char)(a);
}
unsigned char var14_transform(unsigned char ch, unsigned char a, unsigned char b)
{
    if (ch > a)
        return (unsigned char)(255);
    return (unsigned char)((float)(ch) * (float)(b) / (float)(a));
}
void drawPicture(unsigned char *arr, float divider, int fmax = 255, int fmin = 0, int  gmax = 255, int  gmin = 0,int a_param = 128, int  b_param = 128, bool is_show_second_transformation = false)
{
    float shiftKoef = 0;
    glClearColor(0, 0, 0, 0);
    glBegin(GL_POINTS);
    if (BytesPerPixel == 1)
    {
        //pure picture
        //for (int j = 0; j < h; j++)
        //    for (int i = 0; i < w; i++)
        //    {
        //        glColor3d(((double)(unsigned int)arr[j * w + i + data_index+j*padding] / divider),
        //                  ((double)(unsigned int)arr[j * w + i + data_index+j*padding] / divider),
        //                  ((double)(unsigned int)arr[j * w + i + data_index+j*padding] / divider));
        //        glVertex2d((double)(2 * i - w) / w, (double)(2 * j - h) / h);
        //    }
        if (!is_show_second_transformation)
            for (int j = 0; j < h; j++)
                for (int i = 0; i < w; i++)
                {
                    glColor3d(shiftKoef + ((double)((unsigned int)linearFunction(arr[j * w + i + data_index + j * padding], fmax, fmin, gmax, gmin)) / divider),
                        shiftKoef + ((double)((unsigned int)linearFunction(arr[j * w + i + data_index + j * padding], fmax, fmin, gmax, gmin)) / divider),
                        shiftKoef + ((double)((unsigned int)linearFunction(arr[j * w + i + data_index + j * padding], fmax, fmin, gmax, gmin)) / divider));
                    //glColor3d(((float)(unsigned int)arr[j * w + i + data_index] / divider), 0, 0);
                    glVertex2d((double)(2 * i - w) / w, (double)(2 * j - h) / h);
                }
        else
            for (int j = 0; j < h; j++)
                for (int i = 0; i < w; i++)
                {
                    glColor3d(shiftKoef + ((double)((unsigned int)var14_transform(linearFunction(arr[j * w + i + data_index + j * padding], fmax, fmin, gmax, gmin), a_param, b_param)) / divider),
                        shiftKoef + ((double)((unsigned int)var14_transform(linearFunction(arr[j * w + i + data_index + j * padding], fmax, fmin, gmax, gmin), a_param, b_param)) / divider),
                        shiftKoef + ((double)((unsigned int)var14_transform(linearFunction(arr[j * w + i + data_index + j * padding], fmax, fmin, gmax, gmin), a_param, b_param)) / divider));
                    //glColor3d(((float)(unsigned int)arr[j * w + i + data_index] / divider), 0, 0);
                    glVertex2d((double)(2 * i - w) / w, (double)(2 * j - h) / h);
                }
    }
    else if (BytesPerPixel == 3)
    {
        //pure picture
        //for (int j = 0; j < h; j++)
        //    for (int i = 0; i < w; i++)
        //    {
        //        glColor3d(((double)((unsigned int)arr[j * w*3 + i * 3 + 2 + data_index+j*padding]) / divider),
        //                  ((double)((unsigned int)arr[j * w*3 + i * 3 + 1 + data_index+j*padding]) / divider),
        //                  ((double)((unsigned int)arr[j * w*3 + i * 3 + 0 + data_index+j*padding]) / divider));
        //        glVertex2d((double)(2 * i - w) / w, (double)(2 * j - h) / h);
        //    }
        if (!is_show_second_transformation)
            for (int j = 0; j < h; j++)
                for (int i = 0; i < w; i++)
                {
                    glColor3d(shiftKoef + ((double)((unsigned int)linearFunction(arr[j * w * 3 + i * 3 + 2 + data_index + j * padding], fmax, fmin, gmax, gmin)) / divider),
                        shiftKoef + ((double)((unsigned int)linearFunction(arr[j * w * 3 + i * 3 + 1 + data_index + j * padding], fmax, fmin, gmax, gmin)) / divider),
                        shiftKoef + ((double)((unsigned int)linearFunction(arr[j * w * 3 + i * 3 + 0 + data_index + j * padding], fmax, fmin, gmax, gmin)) / divider));
                    glVertex2d((double)(2 * i - w) / w, (double)(2 * j - h) / h);
                }
        else
            for (int j = 0; j < h; j++)
                for (int i = 0; i < w; i++)
                {
                    glColor3d(shiftKoef + ((double)((unsigned int)var14_transform(linearFunction(arr[j * w * 3 + i * 3 + 2 + data_index + j * padding], fmax, fmin, gmax, gmin), a_param, b_param)) / divider),
                              shiftKoef + ((double)((unsigned int)var14_transform(linearFunction(arr[j * w * 3 + i * 3 + 1 + data_index + j * padding], fmax, fmin, gmax, gmin), a_param, b_param)) / divider),
                              shiftKoef + ((double)((unsigned int)var14_transform(linearFunction(arr[j * w * 3 + i * 3 + 0 + data_index + j * padding], fmax, fmin, gmax, gmin), a_param, b_param)) / divider));
                    glVertex2d((double)(2 * i - w) / w, (double)(2 * j - h) / h);
                }
    }
    glEnd();
}
unsigned char stabilise(float val)
{
    if (val > 255) return 255;
    else if (val < 0) return 0;
    else return (unsigned char)val;
}
unsigned char* sobel(unsigned char* a, float maskX[3][3], float maskY[3][3]) 
{
    unsigned char* answer = new unsigned char[fileSize];
    //const int maskX[][3] = { {-1,0,1},
    //                         {-2,0,2},
    //                         {-1,0,1} };
    //const int maskY[][3] = { {1,2,1},
    //                         {0,0,0},
    //                         {-1,-2,-1} };
    for (int i = 0; i < data_index; i++)
        answer[i] = a[i];
    float dx, dy;
    if (BytesPerPixel == 1)
    {
        for (int j = 0; j < h; j++)
            for (int i = 0; i < w; i++)
            {
                if (j == 0 || i == 0 || j == h - 1 || i == w - 1)
                {
                    answer[j * w + i + data_index + j * padding] = 0;
                }
                else
                {
                    dx = (float)(maskX[0][0] * a[(j - 1) * w + (i - 1) + data_index + (j - 1) * padding] + maskX[0][1] * a[(j - 1) * w + i + data_index + (j - 1) * padding] + maskX[0][2] * a[(j - 1) * w + (i + 1) + data_index + (j - 1) * padding]
                                +maskX[1][0] * a[j * w       + (i - 1) + data_index + j * padding]       + maskX[1][1] * a[j * w       + i + data_index + j * padding]       + maskX[1][2] * a[j * w       + (i + 1) + data_index + j * padding]
                                +maskX[2][0] * a[(j + 1) * w + (i - 1) + data_index + (j + 1) * padding] + maskX[2][1] * a[(j + 1) * w + i + data_index + (j + 1) * padding] + maskX[2][2] * a[(j + 1) * w + (i + 1) + data_index + (j + 1) * padding]);
                    dy = (float)(maskY[0][0] * a[(j - 1) * w + (i - 1) + data_index + (j - 1) * padding] + maskY[0][1] * a[(j - 1) * w + i + data_index + (j - 1) * padding] + maskY[0][2] * a[(j - 1) * w + (i + 1) + data_index + (j - 1) * padding]
                               + maskY[1][0] * a[j * w       + (i - 1) + data_index + j * padding]       + maskY[1][1] * a[j * w       + i + data_index + j * padding]       + maskY[1][2] * a[j * w       + (i + 1) + data_index + j * padding]
                               + maskY[2][0] * a[(j + 1) * w + (i - 1) + data_index + (j + 1) * padding] + maskY[2][1] * a[(j + 1) * w + i + data_index + (j + 1) * padding] + maskY[2][2] * a[(j + 1) * w + (i + 1) + data_index + (j + 1) * padding]);
                    answer[j * w + i + data_index + j * padding] = stabilise( sqrt(dx * dx + dy * dy));
                }
            }
        
    }
    else if (BytesPerPixel == 3)
    {
        for (int j = 0; j < h; j++)
            for (int i = 0; i < w; i++)
            {
                if (j == 0 || i == 0 || j == h - 1 || i == w - 1)
                {
                    answer[j * w * 3 + i * 3 + 2 + data_index + j * padding] = 0;
                    answer[j * w * 3 + i * 3 + 1 + data_index + j * padding] = 0;
                    answer[j * w * 3 + i * 3 + 0 + data_index + j * padding] = 0;
                }
                else
                {
                    dx =(float)(maskX[0][0]*a[(j-1) * w*3 + (i-1)*3 +2+ data_index + (j-1) * padding] + maskX[0][1]*a[(j-1) * w*3 + i*3 +2+ data_index + (j-1) * padding] + maskX[0][2]*a[(j-1) * w*3 + (i+1)*3 +2+ data_index + (j-1) * padding]
                               +maskX[1][0]*a[j * w*3     + (i-1)*3 +2+ data_index + j * padding]     + maskX[1][1]*a[j * w*3     + i*3 +2+ data_index + j * padding]     + maskX[1][2]*a[j * w*3     + (i+1)*3 +2+ data_index + j * padding]
                               +maskX[2][0]*a[(j+1) * w*3 + (i-1)*3 +2+ data_index + (j+1) * padding] + maskX[2][1]*a[(j+1) * w*3 + i*3 +2+ data_index + (j+1) * padding] + maskX[2][2]*a[(j+1) * w*3 + (i+1)*3 +2+ data_index + (j+1) * padding]);
                    dy =(float)(maskY[0][0]*a[(j-1) * w*3 + (i-1)*3 +2+ data_index + (j-1) * padding] + maskY[0][1]*a[(j-1) * w*3 + i*3 +2+ data_index + (j-1) * padding] + maskY[0][2]*a[(j-1) * w*3 + (i+1)*3 +2+ data_index + (j-1) * padding]
                               +maskY[1][0]*a[j * w*3     + (i-1)*3 +2+ data_index + j * padding]     + maskY[1][1]*a[j * w*3     + i*3 +2+ data_index + j * padding]     + maskY[1][2]*a[j * w*3     + (i+1)*3 +2+ data_index + j * padding]
                               +maskY[2][0]*a[(j+1) * w*3 + (i-1)*3 +2+ data_index + (j+1) * padding] + maskY[2][1]*a[(j+1) * w*3 + i*3 +2+ data_index + (j+1) * padding] + maskY[2][2]*a[(j+1) * w*3 + (i+1)*3 +2+ data_index + (j+1) * padding]);
                    answer[j * w * 3 + i * 3 + 2 + data_index + j * padding] = stabilise(sqrt(dx * dx + dy * dy));
                    dx =(float)(maskX[0][0]*a[(j-1) * w*3 + (i-1)*3 +1+ data_index + (j-1) * padding] + maskX[0][1]*a[(j-1) * w*3 + i*3 +1+ data_index + (j-1) * padding] + maskX[0][2]*a[(j-1) * w*3 + (i+1)*3 +1+ data_index + (j-1) * padding]
                               +maskX[1][0]*a[j * w*3     + (i-1)*3 +1+ data_index + j * padding]     + maskX[1][1]*a[j * w*3     + i*3 +1+ data_index + j * padding]     + maskX[1][2]*a[j * w*3     + (i+1)*3 +1+ data_index + j * padding]
                               +maskX[2][0]*a[(j+1) * w*3 + (i-1)*3 +1+ data_index + (j+1) * padding] + maskX[2][1]*a[(j+1) * w*3 + i*3 +1+ data_index + (j+1) * padding] + maskX[2][2]*a[(j+1) * w*3 + (i+1)*3 +1+ data_index + (j+1) * padding]);
                    dy =(float)(maskY[0][0]*a[(j-1) * w*3 + (i-1)*3 +1+ data_index + (j-1) * padding] + maskY[0][1]*a[(j-1) * w*3 + i*3 +1+ data_index + (j-1) * padding] + maskY[0][2]*a[(j-1) * w*3 + (i+1)*3 +1+ data_index + (j-1) * padding]
                               +maskY[1][0]*a[j * w*3     + (i-1)*3 +1+ data_index + j * padding]     + maskY[1][1]*a[j * w*3     + i*3 +1+ data_index + j * padding]     + maskY[1][2]*a[j * w*3     + (i+1)*3 +1+ data_index + j * padding]
                               +maskY[2][0]*a[(j+1) * w*3 + (i-1)*3 +1+ data_index + (j+1) * padding] + maskY[2][1]*a[(j+1) * w*3 + i*3 +1+ data_index + (j+1) * padding] + maskY[2][2]*a[(j+1) * w*3 + (i+1)*3 +1+ data_index + (j+1) * padding]);
                    answer[j * w * 3 + i * 3 + 1 + data_index + j * padding] = stabilise(sqrt(dx * dx + dy * dy));
                    dx =(float)(maskX[0][0]*a[(j-1) * w*3 + (i-1)*3 +0+ data_index + (j-1) * padding] + maskX[0][1]*a[(j-1) * w*3 + i*3 +0+ data_index + (j-1) * padding] + maskX[0][2]*a[(j-1) * w*3 + (i+1)*3 +0+ data_index + (j-1) * padding]
                               +maskX[1][0]*a[j * w*3     + (i-1)*3 +0+ data_index + j * padding]     + maskX[1][1]*a[j * w*3     + i*3 +0+ data_index + j * padding]     + maskX[1][2]*a[j * w*3     + (i+1)*3 +0+ data_index + j * padding]
                               +maskX[2][0]*a[(j+1) * w*3 + (i-1)*3 +0+ data_index + (j+1) * padding] + maskX[2][1]*a[(j+1) * w*3 + i*3 +0+ data_index + (j+1) * padding] + maskX[2][2]*a[(j+1) * w*3 + (i+1)*3 +0+ data_index + (j+1) * padding]);
                    dy =(float)(maskY[0][0]*a[(j-1) * w*3 + (i-1)*3 +0+ data_index + (j-1) * padding] + maskY[0][1]*a[(j-1) * w*3 + i*3 +0+ data_index + (j-1) * padding] + maskY[0][2]*a[(j-1) * w*3 + (i+1)*3 +0+ data_index + (j-1) * padding]
                               +maskY[1][0]*a[j * w*3     + (i-1)*3 +0+ data_index + j * padding]     + maskY[1][1]*a[j * w*3     + i*3 +0+ data_index + j * padding]     + maskY[1][2]*a[j * w*3     + (i+1)*3 +0+ data_index + j * padding]
                               +maskY[2][0]*a[(j+1) * w*3 + (i-1)*3 +0+ data_index + (j+1) * padding] + maskY[2][1]*a[(j+1) * w*3 + i*3 +0+ data_index + (j+1) * padding] + maskY[2][2]*a[(j+1) * w*3 + (i+1)*3 +0+ data_index + (j+1) * padding]);
                    answer[j * w * 3 + i * 3 + 0 + data_index + j * padding] = stabilise(sqrt(dx * dx + dy * dy));
                }
            }
    }
    return answer;
}
unsigned char* previtt(unsigned char* a) 
{
    unsigned char* answer = new unsigned char[fileSize];
    const int maskX[][3] = { {-1,0,1},
                             {-1,0,1},
                             {-1,0,1} };
    const int maskY[][3] = { {1,1,1},
                             {0,0,0},
                             {-1,-1,-1} };
    float dx, dy;
    for (int i = 0; i < data_index; i++)
        answer[i] = a[i];
    if (BytesPerPixel == 1)
    {
        for (int j = 0; j < h; j++)
            for (int i = 0; i < w; i++)
            {
                if (j == 0 || i == 0 || j == h - 1 || i == w - 1)
                {
                    answer[j * w + i + data_index + j * padding] = 0;
                }
                else
                {
                    dx = (float)(maskX[0][0] * a[(j - 1) * w + (i - 1) + data_index + (j - 1) * padding] + maskX[0][1] * a[(j - 1) * w + i + data_index + (j - 1) * padding] + maskX[0][2] * a[(j - 1) * w + (i + 1) + data_index + (j - 1) * padding]
                        + maskX[1][0] * a[j * w + (i - 1) + data_index + j * padding] + maskX[1][1] * a[j * w + i + data_index + j * padding] + maskX[1][2] * a[j * w + (i + 1) + data_index + j * padding]
                        + maskX[2][0] * a[(j + 1) * w + (i - 1) + data_index + (j + 1) * padding] + maskX[2][1] * a[(j + 1) * w + i + data_index + (j + 1) * padding] + maskX[2][2] * a[(j + 1) * w + (i + 1) + data_index + (j + 1) * padding]);
                    dy = (float)(maskY[0][0] * a[(j - 1) * w + (i - 1) + data_index + (j - 1) * padding] + maskY[0][1] * a[(j - 1) * w + i + data_index + (j - 1) * padding] + maskY[0][2] * a[(j - 1) * w + (i + 1) + data_index + (j - 1) * padding]
                        + maskY[1][0] * a[j * w + (i - 1) + data_index + j * padding] + maskY[1][1] * a[j * w + i + data_index + j * padding] + maskY[1][2] * a[j * w + (i + 1) + data_index + j * padding]
                        + maskY[2][0] * a[(j + 1) * w + (i - 1) + data_index + (j + 1) * padding] + maskY[2][1] * a[(j + 1) * w + i + data_index + (j + 1) * padding] + maskY[2][2] * a[(j + 1) * w + (i + 1) + data_index + (j + 1) * padding]);
                    answer[j * w + i + data_index + j * padding] = stabilise(sqrt(dx * dx + dy * dy));
                }
            }

    }
    else if (BytesPerPixel == 3)
    {
        for (int j = 0; j < h; j++)
            for (int i = 0; i < w; i++)
            {
                if (j == 0 || i == 0 || j == h - 1 || i == w - 1)
                {
                    answer[j * w * 3 + i * 3 + 2 + data_index + j * padding] = 0;
                    answer[j * w * 3 + i * 3 + 1 + data_index + j * padding] = 0;
                    answer[j * w * 3 + i * 3 + 0 + data_index + j * padding] = 0;
                }
                else
                {
                    dx = (float)(maskX[0][0] * a[(j - 1) * w * 3 + (i - 1) * 3 + 2 + data_index + (j - 1) * padding] + maskX[0][1] * a[(j - 1) * w * 3 + i * 3 + 2 + data_index + (j - 1) * padding] + maskX[0][2] * a[(j - 1) * w * 3 + (i + 1) * 3 + 2 + data_index + (j - 1) * padding]
                        + maskX[1][0] * a[j * w * 3 + (i - 1) * 3 + 2 + data_index + j * padding] + maskX[1][1] * a[j * w * 3 + i * 3 + 2 + data_index + j * padding] + maskX[1][2] * a[j * w * 3 + (i + 1) * 3 + 2 + data_index + j * padding]
                        + maskX[2][0] * a[(j + 1) * w * 3 + (i - 1) * 3 + 2 + data_index + (j + 1) * padding] + maskX[2][1] * a[(j + 1) * w * 3 + i * 3 + 2 + data_index + (j + 1) * padding] + maskX[2][2] * a[(j + 1) * w * 3 + (i + 1) * 3 + 2 + data_index + (j + 1) * padding]);
                    dy = (float)(maskY[0][0] * a[(j - 1) * w * 3 + (i - 1) * 3 + 2 + data_index + (j - 1) * padding] + maskY[0][1] * a[(j - 1) * w * 3 + i * 3 + 2 + data_index + (j - 1) * padding] + maskY[0][2] * a[(j - 1) * w * 3 + (i + 1) * 3 + 2 + data_index + (j - 1) * padding]
                        + maskY[1][0] * a[j * w * 3 + (i - 1) * 3 + 2 + data_index + j * padding] + maskY[1][1] * a[j * w * 3 + i * 3 + 2 + data_index + j * padding] + maskY[1][2] * a[j * w * 3 + (i + 1) * 3 + 2 + data_index + j * padding]
                        + maskY[2][0] * a[(j + 1) * w * 3 + (i - 1) * 3 + 2 + data_index + (j + 1) * padding] + maskY[2][1] * a[(j + 1) * w * 3 + i * 3 + 2 + data_index + (j + 1) * padding] + maskY[2][2] * a[(j + 1) * w * 3 + (i + 1) * 3 + 2 + data_index + (j + 1) * padding]);
                    answer[j * w * 3 + i * 3 + 2 + data_index + j * padding] = stabilise(sqrt(dx * dx + dy * dy));
                    dx = (float)(maskX[0][0] * a[(j - 1) * w * 3 + (i - 1) * 3 + 1 + data_index + (j - 1) * padding] + maskX[0][1] * a[(j - 1) * w * 3 + i * 3 + 1 + data_index + (j - 1) * padding] + maskX[0][2] * a[(j - 1) * w * 3 + (i + 1) * 3 + 1 + data_index + (j - 1) * padding]
                        + maskX[1][0] * a[j * w * 3 + (i - 1) * 3 + 1 + data_index + j * padding] + maskX[1][1] * a[j * w * 3 + i * 3 + 1 + data_index + j * padding] + maskX[1][2] * a[j * w * 3 + (i + 1) * 3 + 1 + data_index + j * padding]
                        + maskX[2][0] * a[(j + 1) * w * 3 + (i - 1) * 3 + 1 + data_index + (j + 1) * padding] + maskX[2][1] * a[(j + 1) * w * 3 + i * 3 + 1 + data_index + (j + 1) * padding] + maskX[2][2] * a[(j + 1) * w * 3 + (i + 1) * 3 + 1 + data_index + (j + 1) * padding]);
                    dy = (float)(maskY[0][0] * a[(j - 1) * w * 3 + (i - 1) * 3 + 1 + data_index + (j - 1) * padding] + maskY[0][1] * a[(j - 1) * w * 3 + i * 3 + 1 + data_index + (j - 1) * padding] + maskY[0][2] * a[(j - 1) * w * 3 + (i + 1) * 3 + 1 + data_index + (j - 1) * padding]
                        + maskY[1][0] * a[j * w * 3 + (i - 1) * 3 + 1 + data_index + j * padding] + maskY[1][1] * a[j * w * 3 + i * 3 + 1 + data_index + j * padding] + maskY[1][2] * a[j * w * 3 + (i + 1) * 3 + 1 + data_index + j * padding]
                        + maskY[2][0] * a[(j + 1) * w * 3 + (i - 1) * 3 + 1 + data_index + (j + 1) * padding] + maskY[2][1] * a[(j + 1) * w * 3 + i * 3 + 1 + data_index + (j + 1) * padding] + maskY[2][2] * a[(j + 1) * w * 3 + (i + 1) * 3 + 1 + data_index + (j + 1) * padding]);
                    answer[j * w * 3 + i * 3 + 1 + data_index + j * padding] = stabilise(sqrt(dx * dx + dy * dy));
                    dx = (float)(maskX[0][0] * a[(j - 1) * w * 3 + (i - 1) * 3 + 0 + data_index + (j - 1) * padding] + maskX[0][1] * a[(j - 1) * w * 3 + i * 3 + 0 + data_index + (j - 1) * padding] + maskX[0][2] * a[(j - 1) * w * 3 + (i + 1) * 3 + 0 + data_index + (j - 1) * padding]
                        + maskX[1][0] * a[j * w * 3 + (i - 1) * 3 + 0 + data_index + j * padding] + maskX[1][1] * a[j * w * 3 + i * 3 + 0 + data_index + j * padding] + maskX[1][2] * a[j * w * 3 + (i + 1) * 3 + 0 + data_index + j * padding]
                        + maskX[2][0] * a[(j + 1) * w * 3 + (i - 1) * 3 + 0 + data_index + (j + 1) * padding] + maskX[2][1] * a[(j + 1) * w * 3 + i * 3 + 0 + data_index + (j + 1) * padding] + maskX[2][2] * a[(j + 1) * w * 3 + (i + 1) * 3 + 0 + data_index + (j + 1) * padding]);
                    dy = (float)(maskY[0][0] * a[(j - 1) * w * 3 + (i - 1) * 3 + 0 + data_index + (j - 1) * padding] + maskY[0][1] * a[(j - 1) * w * 3 + i * 3 + 0 + data_index + (j - 1) * padding] + maskY[0][2] * a[(j - 1) * w * 3 + (i + 1) * 3 + 0 + data_index + (j - 1) * padding]
                        + maskY[1][0] * a[j * w * 3 + (i - 1) * 3 + 0 + data_index + j * padding] + maskY[1][1] * a[j * w * 3 + i * 3 + 0 + data_index + j * padding] + maskY[1][2] * a[j * w * 3 + (i + 1) * 3 + 0 + data_index + j * padding]
                        + maskY[2][0] * a[(j + 1) * w * 3 + (i - 1) * 3 + 0 + data_index + (j + 1) * padding] + maskY[2][1] * a[(j + 1) * w * 3 + i * 3 + 0 + data_index + (j + 1) * padding] + maskY[2][2] * a[(j + 1) * w * 3 + (i + 1) * 3 + 0 + data_index + (j + 1) * padding]);
                    answer[j * w * 3 + i * 3 + 0 + data_index + j * padding] = stabilise(sqrt(dx * dx + dy * dy));
                }
            }
    }
    return answer;
}
//unsigned char* contrast(unsigned char* a, float koef) 
//{
//    unsigned char* answer = new unsigned char[fileSize];
//    const int mask[][3] = { { 1,1,1},
//                             {1,1,1},
//                             {1,1,1} };
//    float dx, dy;
//    if (BytesPerPixel == 1)
//    {
//        for (int j = 0; j < h; j++)
//            for (int i = 0; i < w; i++)
//            {
//                if (j == 0 || i == 0 || j == h - 1 || i == w - 1)
//                {
//                    answer[j * w + i + data_index + j * padding] = 0;
//                }
//                else
//                {
//                    dx = (float)(mask[0][0] * a[(j - 1) * w + (i - 1) + data_index + (j - 1) * padding] + mask[0][1] * a[(j - 1) * w + i + data_index + (j - 1) * padding] + mask[0][2] * a[(j - 1) * w + (i + 1) + data_index + (j - 1) * padding]
//                                +mask[1][0] * a[j * w       + (i - 1) + data_index + j * padding]       + mask[1][1] * a[j * w       + i + data_index + j * padding]       + mask[1][2] * a[j * w       + (i + 1) + data_index + j * padding]
//                                +mask[2][0] * a[(j + 1) * w + (i - 1) + data_index + (j + 1) * padding] + mask[2][1] * a[(j + 1) * w + i + data_index + (j + 1) * padding] + mask[2][2] * a[(j + 1) * w + (i + 1) + data_index + (j + 1) * padding]);
//                    dx = dx / 9.0f;
//                    //dy = (float)(mask[0][0] * a[(j - 1) * w + (i - 1) + data_index + (j - 1) * padding] + mask[0][1] * a[(j - 1) * w + i + data_index + (j - 1) * padding] + mask[0][2] * a[(j - 1) * w + (i + 1) + data_index + (j - 1) * padding]
//                    //           + mask[1][0] * a[j * w       + (i - 1) + data_index + j * padding]       + mask[1][1] * a[j * w       + i + data_index + j * padding]       + mask[1][2] * a[j * w       + (i + 1) + data_index + j * padding]
//                    //           + mask[2][0] * a[(j + 1) * w + (i - 1) + data_index + (j + 1) * padding] + mask[2][1] * a[(j + 1) * w + i + data_index + (j + 1) * padding] + mask[2][2] * a[(j + 1) * w + (i + 1) + data_index + (j + 1) * padding]);
//                    float val = (1 + koef) * answer[j * w + i + data_index + j * padding] - koef * (dx);
//                    answer[j * w + i + data_index + j * padding] = stabilise((1 + koef) * answer[j * w + i + data_index + j * padding] - koef * (dx));
//                }
//            }
//        
//    }
//    else if (BytesPerPixel == 3)
//    {
//        for (int j = 0; j < h; j++)
//            for (int i = 0; i < w; i++)
//            {
//                if (j == 0 || i == 0 || j == h - 1 || i == w - 1)
//                {
//                    answer[j * w * 3 + i * 3 + 2 + data_index + j * padding] = 0;
//                    answer[j * w * 3 + i * 3 + 1 + data_index + j * padding] = 0;
//                    answer[j * w * 3 + i * 3 + 0 + data_index + j * padding] = 0;
//                }
//                else
//                {
//                    dx =(float)(mask[0][0]*a[(j-1) * w*3 + (i-1)*3 +2+ data_index + (j-1) * padding] + mask[0][1]*a[(j-1) * w*3 + i*3 +2+ data_index + (j-1) * padding] + mask[0][2]*a[(j-1) * w*3 + (i+1)*3 +2+ data_index + (j-1) * padding]
//                               +mask[1][0]*a[j * w*3     + (i-1)*3 +2+ data_index + j * padding]     + mask[1][1]*a[j * w*3     + i*3 +2+ data_index + j * padding]     + mask[1][2]*a[j * w*3     + (i+1)*3 +2+ data_index + j * padding]
//                               +mask[2][0]*a[(j+1) * w*3 + (i-1)*3 +2+ data_index + (j+1) * padding] + mask[2][1]*a[(j+1) * w*3 + i*3 +2+ data_index + (j+1) * padding] + mask[2][2]*a[(j+1) * w*3 + (i+1)*3 +2+ data_index + (j+1) * padding]);
//                    dx = dx / 9.0f;
//                    answer[j * w * 3 + i * 3 + 2 + data_index + j * padding] = stabilise((1 + koef) * answer[j * w * 3 + i * 3 + 2 + data_index + j * padding] - koef * (dx));
//                    dx =(float)(mask[0][0]*a[(j-1) * w*3 + (i-1)*3 +1+ data_index + (j-1) * padding] + mask[0][1]*a[(j-1) * w*3 + i*3 +1+ data_index + (j-1) * padding] + mask[0][2]*a[(j-1) * w*3 + (i+1)*3 +1+ data_index + (j-1) * padding]
//                               +mask[1][0]*a[j * w*3     + (i-1)*3 +1+ data_index + j * padding]     + mask[1][1]*a[j * w*3     + i*3 +1+ data_index + j * padding]     + mask[1][2]*a[j * w*3     + (i+1)*3 +1+ data_index + j * padding]
//                               +mask[2][0]*a[(j+1) * w*3 + (i-1)*3 +1+ data_index + (j+1) * padding] + mask[2][1]*a[(j+1) * w*3 + i*3 +1+ data_index + (j+1) * padding] + mask[2][2]*a[(j+1) * w*3 + (i+1)*3 +1+ data_index + (j+1) * padding]);
//                    dx = dx / 9.0f;
//                    answer[j * w * 3 + i * 3 + 1 + data_index + j * padding] = stabilise((1 + koef) * answer[j * w * 3 + i * 3 + 1 + data_index + j * padding] - koef * (dx));
//                    dx =(float)(mask[0][0]*a[(j-1) * w*3 + (i-1)*3 +0+ data_index + (j-1) * padding] + mask[0][1]*a[(j-1) * w*3 + i*3 +0+ data_index + (j-1) * padding] + mask[0][2]*a[(j-1) * w*3 + (i+1)*3 +0+ data_index + (j-1) * padding]
//                               +mask[1][0]*a[j * w*3     + (i-1)*3 +0+ data_index + j * padding]     + mask[1][1]*a[j * w*3     + i*3 +0+ data_index + j * padding]     + mask[1][2]*a[j * w*3     + (i+1)*3 +0+ data_index + j * padding]
//                               +mask[2][0]*a[(j+1) * w*3 + (i-1)*3 +0+ data_index + (j+1) * padding] + mask[2][1]*a[(j+1) * w*3 + i*3 +0+ data_index + (j+1) * padding] + mask[2][2]*a[(j+1) * w*3 + (i+1)*3 +0+ data_index + (j+1) * padding]);
//                    dx = dx / 9.0f;
//                    
//                    answer[j * w * 3 + i * 3 + 0 + data_index + j * padding] = stabilise((1 + koef) * answer[j * w * 3 + i * 3 + 0 + data_index + j * padding] - koef * (dx));
//                }
//            }
//    }
//    return answer;
//}

int main()
{
    unsigned char* arr=new unsigned char[1];//повний файл, побайтово
    //unsigned char* arr_do_not_change;//повна копія оригінального файлу, побайтово, записувати тільки при зчитуванні файлу
    std::string outFile = GetCurrentDirectory() + "\\" + "output.bmp";

    if (!glfwInit())
        return -1;
    GLFWwindow* controlWindow = glfwCreateWindow(800, 900, "Control", NULL, NULL);
    GLFWwindow* showWindow = NULL;// glfwCreateWindow(500, 500, "", NULL, NULL);
    glfwMakeContextCurrent(controlWindow);
    IMGUI_CHECKVERSION();
    ImGui::CreateContext();
    ImGuiIO& io = ImGui::GetIO(); (void)io;
    ImGui::StyleColorsDark();
    ImGui_ImplGlfw_InitForOpenGL(controlWindow, true);
    ImGui_ImplOpenGL3_Init("#version 330");
    bool is_show = false, is_to_draw_picture = false;
    int kkk = 1;
    float divider = 256;
    int fmax = 255, fmin = 0, gmax = 255, gmin = 0;
    int a_param = 128, b_param = 128;
    bool is_show_second_transformation = false;
    //фільтри
    bool is_applied_sobel = false;
    bool is_applied_previtt = false;
    //bool is_applied_contrast = false;
    //float alpha = 0.5;
    float maskX[3][3] = { {-1,0,1},
                         {-2,0,2},
                         {-1,0,1} };
    float maskY[3][3] = { {1,2,1},
                         {0,0,0},
                         {-1,-2,-1} };
    while (!glfwWindowShouldClose(controlWindow))
    {
        glfwMakeContextCurrent(controlWindow);
        glClear(GL_COLOR_BUFFER_BIT);
        glClearColor(0, 0, 0, 0);
        ImGui_ImplOpenGL3_NewFrame();
        ImGui_ImplGlfw_NewFrame();
        ImGui::NewFrame();
        ImGui::Begin("File");
        ImGui::SetWindowFontScale(2);
        if (ImGui::Button("Open BMP-file"))
        {//завантажити новий файл:
            if (openFile())
            {
                arr = read_bmp_file(sFilePath);
                if (is_bmp)
                {
                    int superNumber = 256;//abs(MiNnUmBeRoFtHeMaLl) + abs(MaXnUmBeRoFtHeMaLl);
                    kkk = w > h ? w : h;
                    int wdth = w / kkk, hght = h / kkk;
                    if (w > 1500 || h > 1300)
                    {

                        kkk = kkk > 1000 ? 1 + (kkk / 1000) : 1;
                        wdth = w / kkk;
                        hght = h / kkk;
                    }
                    else
                    {
                        kkk = 1;
                        wdth = w;
                        hght = h;
                    }
                    divider = 256;
                    glfwDestroyWindow(showWindow);
                    showWindow = glfwCreateWindow(wdth, hght, sSelectedFile, NULL, NULL);
                    is_show = true;
                    is_to_draw_picture = true;
                }
                else
                {
                    std::cout << "!!!!!!!!!Wrong file type!!!!!!!!!\n";
                }
            }
        }
        ImGui::SameLine();
        if (ImGui::Button("Apply changes"))
        {
            if (showWindow != NULL)
            {
                is_to_draw_picture = true;
                if (BytesPerPixel == 1)
                {
                    if (is_applied_sobel || is_applied_previtt)
                    {
                        if (is_applied_sobel && is_applied_previtt) arr = sobel(previtt(arr), maskX, maskY);
                        else if (is_applied_sobel)arr = sobel(arr, maskX, maskY);
                        else if (is_applied_previtt)arr = previtt(arr);
                    }
                    if (is_show_second_transformation)
                    {
                        for (int j = 0; j < h; j++)
                            for (int i = 0; i < w; i++)
                            {
                                arr[j * w + i + data_index + j * padding] = var14_transform( linearFunction( arr[j * w + i + data_index + j * padding], fmax, fmin, gmax, gmin), a_param, b_param);
                            }
                    }
                    else
                    {
                        for (int j = 0; j < h; j++)
                            for (int i = 0; i < w; i++)
                            {
                                arr[j * w + i + data_index + j * padding] = linearFunction(arr[j * w + i + data_index + j * padding], fmax, fmin, gmax, gmin);
                            }
                    }
                    //перерахунок гістограми:
                    maxNumOfColorsInColorTable = 0;
                    for (int i = 0; i < 255; i++)colorTable[i] = 0;
                    for (int i = data_index; i < fileSize; i++)
                    {
                        int num = (int)arr[i] < 0 ? (256 + (int)arr[i]) : (int)arr[i];
                        colorTable[num]++;
                        if (maxNumOfColorsInColorTable < colorTable[num])
                            maxNumOfColorsInColorTable = colorTable[num];
                    }
                }
                else if (BytesPerPixel == 3)
                {
                    if (is_applied_sobel || is_applied_previtt)
                    {
                        if (is_applied_sobel && is_applied_previtt) arr = sobel(previtt(arr), maskX, maskY);
                        else if (is_applied_sobel)arr = sobel(arr, maskX, maskY);
                        else if (is_applied_previtt)arr = previtt(arr);
                    }
                    if (is_show_second_transformation)
                    {
                        for (int j = 0; j < h; j++)
                            for (int i = 0; i < w; i++)
                            {
                                arr[j * w * 3 + i * 3 + 2 + data_index + j * padding] = var14_transform(linearFunction(arr[j * w * 3 + i * 3 + 2 + data_index + j * padding], fmax, fmin, gmax, gmin), a_param, b_param);
                                arr[j * w * 3 + i * 3 + 1 + data_index + j * padding] = var14_transform(linearFunction(arr[j * w * 3 + i * 3 + 1 + data_index + j * padding], fmax, fmin, gmax, gmin), a_param, b_param);
                                arr[j * w * 3 + i * 3 + 0 + data_index + j * padding] = var14_transform(linearFunction(arr[j * w * 3 + i * 3 + 0 + data_index + j * padding], fmax, fmin, gmax, gmin), a_param, b_param);
                            }
                    }
                    else
                    {
                        for (int j = 0; j < h; j++)
                            for (int i = 0; i < w; i++)
                            {
                                arr[j * w * 3 + i * 3 + 2 + data_index + j * padding] = linearFunction(arr[j * w * 3 + i * 3 + 2 + data_index + j * padding], fmax, fmin, gmax, gmin);
                                arr[j * w * 3 + i * 3 + 1 + data_index + j * padding] = linearFunction(arr[j * w * 3 + i * 3 + 1 + data_index + j * padding], fmax, fmin, gmax, gmin);
                                arr[j * w * 3 + i * 3 + 0 + data_index + j * padding] = linearFunction(arr[j * w * 3 + i * 3 + 0 + data_index + j * padding], fmax, fmin, gmax, gmin);
                            }
                    }
                    maxNumOfColorsInColorTable = 0;
                    for (int i = 0; i < 255; i++)
                    {
                        colorTableR[i] = 0;
                        colorTableG[i] = 0;
                        colorTableB[i] = 0;
                    }
                    for (int i = data_index; i < fileSize; i++)
                    {
                        int num = (int)arr[i] < 0 ? (256 + (int)arr[i]) : (int)arr[i];
                        if ((i - data_index) % 3 == 0)
                        {
                            colorTableB[num]++;
                            if (maxNumOfColorsInColorTable < colorTableB[num])
                                maxNumOfColorsInColorTable = colorTableB[num];
                        }
                        else if ((i - data_index) % 3 == 1)
                        {
                            colorTableG[num]++;
                            if (maxNumOfColorsInColorTable < colorTableG[num])
                                maxNumOfColorsInColorTable = colorTableG[num];
                        }
                        else if ((i - data_index) % 3 == 2)
                        {
                            colorTableR[num]++;
                            if (maxNumOfColorsInColorTable < colorTableR[num])
                                maxNumOfColorsInColorTable = colorTableR[num];
                        }
                    }
                }
                is_show_second_transformation = false;
                
                maskX[0][0] = -1; maskX[0][1] = 0; maskX[0][2] = 1;
                maskX[1][0] = -2; maskX[1][1] = 0; maskX[1][2] = 2;
                maskX[2][0] = -1; maskX[2][1] = 0; maskX[2][2] = 1;

                maskY[0][0] = 1; maskY[0][1] = 2; maskY[0][2] = 1;
                maskY[1][0] = 0; maskY[1][1] = 0; maskY[1][2] = 0;
                maskY[2][0] = -1; maskY[2][1] = -2; maskY[2][2] = -1;
                
                is_applied_sobel = false;
                is_applied_previtt = false;

                fmax = 255;
                gmax = 255;
                fmin = 0;
                gmin = 0;
            }
        }
        ImGui::SameLine();
        if (ImGui::Button("Save as output.bmp"))
        {
            if (showWindow != NULL)
            {
                std::ofstream p(outFile, std::ios::binary | std::ios::out);
                p.write((char*)arr, fileSize);
                p.close();
            }
        }
        ImGui::BeginTabBar("Tabs");
        if (ImGui::BeginTabItem("File info"))
        {
            if (showWindow != NULL)
            {
                ImGui::Text("File size = %d",fileSize);
                ImGui::Text("Where data stars = %d", data_index);
                ImGui::Text("Width = %d", w);
                ImGui::Text("Height = %d", h);
                ImGui::Text("Bits per pixel = %d", BytesPerPixel * 8);
                if (is_with_badding)
                    ImGui::Text("With padding");
                else
                    ImGui::Text("Without padding");
            }
            else
            {
                ImGui::Text("Load file");
            }
            ImGui::EndTabItem();
        }
        if (ImGui::BeginTabItem("Point filters"))
        {
            if (ImGui::TreeNode("Linear corection"))
            {
                bool ttt1 = ImGui::SliderInt("fmax", &fmax, 0, 255);
                bool ttt2 = ImGui::SliderInt("fmin", &fmin, 0, 255);
                bool ttt3 = ImGui::SliderInt("gmax", &gmax, 0, 255);
                bool ttt4 = ImGui::SliderInt("gmin", &gmin, 0, 255);
                if (ttt1 || ttt2 || ttt3 || ttt4)
                {
                    if (fmax < fmin)
                        fmax = fmin;
                    is_to_draw_picture = true;
                }
                ImGui::TreePop();
                ImGui::Spacing();
            }
            if (ImGui::TreeNode("Var 14 transformation"))
            {
                bool ttt1 = ImGui::SliderInt("a", &a_param, 0, 255);
                bool ttt2 = ImGui::SliderInt("b", &b_param, 0, 255);
                bool ttt3 = ImGui::Checkbox("Show pre transform", &is_show_second_transformation);
                if (ttt1 || ttt2 || ttt3)
                {
                    //var14_transform
                    is_to_draw_picture = true;
                }
                ImGui::TreePop();
                ImGui::Spacing();
            }
            if (ImGui::TreeNode("Graphics"))
            {
                float lines[256];
                for (int n = 0; n < 256; n++)
                    lines[n] = linearFunction(n, fmax, fmin, gmax, gmin);
                ImGui::PlotLines("linearFunction", lines, 256, 0,(const char*)0, 0, 256, ImVec2(256, 200));
                for (int n = 0; n < 256; n++)
                    lines[n] = var14_transform(n,a_param,b_param);
                ImGui::PlotLines("var14_transform", lines, 256, 0, (const char*)0, 0, 256, ImVec2(256, 200));
                ImGui::TreePop();
                ImGui::Spacing();
            }
            ImGui::EndTabItem();
        }
        if (ImGui::BeginTabItem("Filters"))
        {
            //if (ImGui::Checkbox("Contrast filter", &is_applied_contrast))is_to_draw_picture = true;
            //if (is_applied_contrast)
            //{
            //    ImGui::SameLine();
            //    if (ImGui::SliderFloat("alpha", &alpha, -2, 2))is_to_draw_picture = true;
            //}
            if (ImGui::Checkbox("Sobel filter", &is_applied_sobel))is_to_draw_picture = true;
            if (is_applied_sobel)
            {
                ImGui::Text("X mask:");
                if (ImGui::BeginTable("table1", 3))
                {
                    for (int row = 0; row < 3; row++)
                    {
                        ImGui::TableNextRow();
                        for (int column = 0; column < 3; column++)
                        {
                            ImGui::TableSetColumnIndex(column);
                            if(ImGui::DragFloat(("-a[" + std::to_string(row) + "," + std::to_string(column) + "]").c_str(), &maskX[row][column], 0.01f, 0.f, 0.f, "%.2f"))is_to_draw_picture = true;
                        }
                    }
                    ImGui::EndTable();
                }
                ImGui::Text("Y mask:");
                if (ImGui::BeginTable("table1", 3))
                {
                    for (int row = 0; row < 3; row++)
                    {
                        ImGui::TableNextRow();
                        for (int column = 0; column < 3; column++)
                        {
                            ImGui::TableSetColumnIndex(column);
                            if(ImGui::DragFloat(("-a[" + std::to_string(row) + "," + std::to_string(column) + "]").c_str(),&maskY[row][column], 0.01f, 0.f, 0.f, "%.2f"))is_to_draw_picture = true;
                        }
                    }
                    ImGui::EndTable();
                }
            }
            if (ImGui::Checkbox("Previtt filter", &is_applied_previtt))is_to_draw_picture = true;
            //ImGui::Text("Here are some filters");
            ImGui::EndTabItem();
        }
        if (ImGui::BeginTabItem("Histogram"))
        {
            if (showWindow != NULL)
            {
                if (BytesPerPixel == 1)
                {
                    ImGui::PlotHistogram(" ", colorTable, 256, 0, (const char*)0, 0, maxNumOfColorsInColorTable, ImVec2(512, 210), 4);
                }
                else if (BytesPerPixel == 3)
                {
                    ImGui::PlotHistogram("Red", colorTableR, 256, 0, (const char*)0, 0, maxNumOfColorsInColorTableR, ImVec2(512, 210), 4);
                    ImGui::PlotHistogram("Green", colorTableG, 256, 0, (const char*)0, 0, maxNumOfColorsInColorTableG, ImVec2(512, 210), 4);
                    ImGui::PlotHistogram("Blue", colorTableB, 256, 0, (const char*)0, 0, maxNumOfColorsInColorTableB, ImVec2(512, 210), 4);
                }
            }
            ImGui::EndTabItem();
        }
        ImGui::EndTabBar();
        ImGui::End();
        ImGui::Render();
        ImGui_ImplOpenGL3_RenderDrawData(ImGui::GetDrawData());
        glfwSwapBuffers(controlWindow);
        if (showWindow !=NULL && glfwWindowShouldClose(showWindow))
        {
            //glfwSetWindowShouldClose(showWindow, GL_TRUE);
            glfwDestroyWindow(showWindow);
            showWindow = NULL;
            //is_show = false;
            delete[] arr;
            //delete[] arr_do_not_change;
        }
        else if(showWindow != NULL)
        {
            if (is_to_draw_picture)
            {
                glfwMakeContextCurrent(showWindow);
                glClear(GL_COLOR_BUFFER_BIT);
                if (is_applied_sobel || is_applied_previtt)
                {
                    if (is_applied_sobel && is_applied_previtt)drawPicture((sobel(previtt(arr), maskX, maskY)), divider, fmax, fmin, gmax, gmin, a_param, b_param, is_show_second_transformation);
                    //if (is_applied_contrast)drawPicture(contrast(arr,alpha), divider, fmax, fmin, gmax, gmin, a_param, b_param, is_show_second_transformation);
                    else if (is_applied_sobel)drawPicture(sobel(arr,maskX,maskY), divider, fmax, fmin, gmax, gmin, a_param, b_param, is_show_second_transformation);
                    else if (is_applied_previtt)drawPicture(previtt(arr), divider, fmax, fmin, gmax, gmin, a_param, b_param, is_show_second_transformation);
                    
                    //drawPicture(sobel(previtt(contrast(arr))), divider, fmax, fmin, gmax, gmin, a_param, b_param, is_show_second_transformation);
                }
                else
                    drawPicture(arr, divider, fmax, fmin, gmax, gmin, a_param, b_param, is_show_second_transformation);

                glfwSwapBuffers(showWindow);
            }
        }
        glfwPollEvents();
        is_to_draw_picture = false;
    }
    ImGui_ImplOpenGL3_Shutdown();
    ImGui_ImplGlfw_Shutdown();
    ImGui::DestroyContext();
}