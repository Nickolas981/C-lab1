using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OTP_Lab1
{
    class Program
    {
        static int[] X, Y;
        static int[][] C;

        static void Main(string[] args)
        {
            Console.WriteLine("Random? (y - yes)");
            char key = Console.ReadKey().KeyChar;
            Console.WriteLine();
            if (key == 'y' || key == 'Y')
            {
                C = GenerateMatrix();
                Console.WriteLine("Matrix: ");
                PrintMatrix(C);
            }
            else
                C = ReadMatrixC();

            Console.WriteLine("Enter the inputs");
            X = ReadMatrixX(C.Length - 1);
            Console.WriteLine("Enter the outputs");
            Y = ReadMatrixX(C.Length - 1);

            Console.WriteLine("Result A: " + WayA());
            Console.WriteLine("Result B: " + WayB());

            Console.ReadKey();
        }

        static int[][] ReadMatrixC()
        {
            int[][] C;
            string[] splitStr;
            int n = ReadNumber();
            C = new int[n][];
            Console.Write("V   ");
            for (int i = 0; i < n; i++)
                Console.Write(i + " ");
            Console.WriteLine();
            for (int i = 0; i < n; i++)
            {
                Console.Write($"V{i}: ");
                splitStr = Console.ReadLine().Split(' ');
                if (splitStr.Length != n)
                {
                    i--;
                    Console.WriteLine($"Incorrect row (length = {splitStr.Length} != N)");
                    continue;
                }
                C[i] = new int[n];
                for (int j = 0; j < n; j++)
                {
                    if (!int.TryParse(splitStr[j], out C[i][j]) || !(C[i][j] == 1 || C[i][j] == 0))
                    {
                        i--;
                        Console.WriteLine($"Incorrect symbol {splitStr[j]}");
                        break;
                    }
                }
            }
            SymmetricalMatrix(C);
            return C;
        }

        static int[] ReadMatrixX(int maxN)
        {
            string[] splitStr;
            int[] X = null;
            while (X == null)
            {
                splitStr = Console.ReadLine().Split(' ');

                if (splitStr.Length > maxN)
                    Console.WriteLine($"Number of vertex must be less than {maxN}, try again ... ");

                X = new int[splitStr.Length];
                for (int i = 0; i < splitStr.Length; i++)
                    if (!Int32.TryParse(splitStr[i], out X[i]) || X[i] > maxN || X[i] < 0)
                    {
                        X = null;
                        Console.WriteLine($"Number must be less than {maxN}, try again ... ");
                        break;
                    }
            }

            return X;
        }

        static int ReadNumber()
        {
            int n = 0;
            while (true)
            {
                Console.Write("N: ");
                if (!Int32.TryParse(Console.ReadLine(), out n) || n <= 0)
                {
                    Console.WriteLine("Incorrect number, try again ... ");
                    continue;
                }
                break;
            }
            return n;
        }

        static int[][] GenerateMatrix()
        {
            int n = ReadNumber();
            Random rand = new Random();
            int[][] C = new int[n][];
            for (int i = 0; i < n; i++)
            {
                C[i] = new int[n];
                for (int j = 0; j < n; j++)
                    C[i][j] = (int)(rand.Next(0, 11) > 5 ? 1 : 0); // 50%
            }
            SymmetricalMatrix(C);
            return C;
        }

        static void SymmetricalMatrix(int[][] C)
        {
            for (int i = 0; i < C.Length; i++)
            {
                for (int j = 0; j < C.Length; j++)
                    if (C[i][j] == 1)
                        C[j][i] = 1;
                C[i][i] = 0;
            }
        }

        static void PrintMatrix(int[][] C)
        {
            Console.Write("V   ");
            for (int i = 0; i < C.Length; i++)
                Console.Write(i + " ");
            Console.WriteLine();
            for (int i = 0; i < C.Length; i++)
            {
                Console.Write($"V{i}: ");
                for (int j = 0; j < C.Length; j++)
                    Console.Write(C[i][j] + " ");
                Console.WriteLine();
            }

        }

        static void PrintWay(List<int> way)
        {
            foreach (int n in way)
                Console.Write(n + "->");
        }

        static int WayA()
        {
            int num = 0;
            bool[][] used = new bool[C.Length][];
            for (int i = 0; i < used.Length; i++)
            {
                used[i] = new bool[used.Length];
                for (int j = 0; j < used.Length; j++)
                    used[i][j] = false;
            }
            List<int> way = new List<int>();
            for (int i = 0; i < X.Length; i++)
            {
                way.Clear();
                way.Add(X[i]);
                if (DFSA(used, way, X[i]))
                    num++;
            }
            return num;
        }

        static bool DFSA(bool[][] used, List<int> way, int curent)
        {
            if (way.Count > 1)
            {
                used[way.Last()][way.ElementAt(way.Count - 2)] = true;
                used[way.ElementAt(way.Count - 2)][way.Last()] = true;
            }
            if (Y.Contains(curent))
                return true;
            for (int i = 0; i < C[curent].Length; i++)
            {
                if (C[curent][i] == 1 && !used[curent][i])
                {
                    way.Add(i);
                    if (DFSA(used, way, i))
                        return true;
                    else
                        way.Remove(i);
                }
            }
            if (way.Count != 0)
            {
                used[way.Last()][way.ElementAt(way.Count - 2)] = false;
                used[way.ElementAt(way.Count - 2)][way.Last()] = false;
            }
            return false;
        }

        static int WayB()
        {
            int num = 0;
            bool[] used = new bool[C.Length];
            for (int i = 0; i < used.Length; i++)
                used[i] = false;
            List<int> way = new List<int>();
            for (int i = 0; i < X.Length; i++)
            {
                way.Clear();
                way.Add(X[i]);
                if (DFSB(used, way, X[i]))
                    num++;
            }
            return num;
        }

        static bool DFSB(bool[] used, List<int> way, int curent)
        {
            used[curent] = true;
            if (Y.Contains(curent))
                return true;
            for (int i = 0; i < C[curent].Length; i++)
            {
                if (C[curent][i] == 1 && !used[i])
                {
                    way.Add(i);
                    if (DFSB(used, way, i))
                        return true;
                    else
                        way.Remove(i);
                }
            }
            used[curent] = false;
            return false;
        }
    }
}

/*
4

0 0 0 1
0 0 0 1
0 0 0 1
0 0 1 0

8

0 0 1 0 1 1 0 0
0 0 1 1 0 1 0 0
1 1 0 1 1 1 0 0
0 1 1 0 1 0 1 0
1 0 1 1 0 1 1 0
1 1 1 0 1 0 0 1
0 0 0 1 1 0 0 1
0 0 0 0 1 1 1 0
*/
