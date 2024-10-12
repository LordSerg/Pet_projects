using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Flatlander
{
    /*
     * there will be 5 levels:
     * 0 - map 3*3 - training to manipulate 
     * 1 - map 5*5
     * 2 - map 7*7
     * 3 - map 9*9
     * 4 - map 11*11 - last level
     * 
     * for each level the point is to find the center(highest) stair and stand there
     * every time levels will be generated automaticly, depending of level and hardness
     * 
     * level generation:
     * 1) create path from [0,0] to [mid,mid] 
     * 2) build steirs for this path - for each next step (level of stair)->(++)
     * 3) if there are nondefined stairs set their height = some_neighbour_level + rand[-2,2]; (...) > [mid,mid] ? (--) : (...)
     * 4) to make gamme more ineresting we going through the path and increasing/lowing som of the steirs 2/3/5 times, depending on hardness level
     * 
     * hardness: 0 -> 0*([level_width^(1/2)]) changes; 10 tries
     *           1 -> 1*([level_width^(1/2)]) changes; 10 tries
     *           2 -> 2*([level_width^(1/2)]) changes; 10 tries
     *           3 -> 3*([level_width^(1/2)]) changes; 10 tries
     */
    public static class MapGenerator
    {
        private static Random rand = new Random();
        public static int[,] Generate(int level=1, int hardness=1)
        {
            int size = (level + 1) * 2 + 1;
            int[,] answer = new int[size,size];
            int i = 0, j = 0,d;
            int mid = level+1;
            int x, y,iter,prevDir;
            bool flag = false;
            int maxLevelStair = size * size-(size-2)*3;
            do
            {
                for (i = 0; i < size; i++)
                    for (j = 0; j < size; j++)
                        answer[i, j] = -1;
                answer[mid, mid] = maxLevelStair;
                x = y = mid;
                iter = 0;
                prevDir = -1;
                i = j = 0;
                flag = false;
                while (answer[0, 0] == -1 && iter < size*size*size)
                {
                    prevDir = RandDirection(ref i, ref j, prevDir);
                    if (rand.Next(100) > 95)
                        d = 1;
                    else if (rand.Next(100) > 90)
                        d = -2;
                    else
                        d = -1;
                    if (x + i >= 0 && x + i < size
                     && y + j >= 0 && y + j < size
                     && answer[x, y] + d >= 0)
                    {
                        if (iter == 0)
                            answer[x + i, y + j] = answer[x, y] - 1;
                        else if (answer[x + i, y + j] != -1)
                            answer[x + i, y + j] = answer[x + i, y + j];
                        else
                            answer[x + i, y + j] = answer[x, y] + d;
                        x += i;
                        y += j;
                    }
                    //if (answer[x, y] == 0)
                    //    break;
                    iter++;
                }
                if (answer[0, 0] != -1)
                {//walkel to [0,0] successfully
                    for (i = 0; i < size; i++)
                        for (j = 0; j < size; j++)
                            if (answer[i, j] == -1)
                                answer[i, j] = rand.Next(2, maxLevelStair-size/2);
                }
                else// if (answer[x, y] == 0)
                {//walkel to [x,y] successfully, but fuck...
                 // for (i = 0; i < size; i++)
                 //     for (j = 0; j < size; j++)
                 //         if (answer[i, j] == -1)
                 //             answer[i, j] = rand.Next(2, 9);
                    flag = true;
                }
            }
            while (/*answer[mid, mid] != maxLevelStair || iter < size*size ||*/ answer[0, 0] >= size || flag == true);
            //
            //return null;
            //int[,] answer = { {0,1,2,3,2 },
            //                  {3,2,5,4,3 },
            //                  {4,5,15,14,4 },
            //                  {7,6,7,13,5 },
            //                  {8,10,11,12,6 }};

            return answer;
        }
        private static int RandDirection(ref int i, ref int j, int forebid = -1)
        {
            int k;
            do
            {
                k = rand.Next(0, 4);
                if (k == 0)
                {
                    i = -1;
                    j = 0;
                }
                else if (k == 1)
                {
                    i = 0;
                    j = -1;
                }
                else if (k == 2)
                {
                    i = 0;
                    j = 1;
                }
                else if (k == 3)
                {
                    i = 1;
                    j = 0;
                } 
            }
            while (k==forebid);
            return 3-k;
        }
    }
}
