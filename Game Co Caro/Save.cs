using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Game_Co_Caro
{
    class Save
    {
        public static void save(int[,] map, FormBanCo f)
        {

            using (TextWriter tw = new StreamWriter(@"../data.txt"))
            {
                if (f.isComputer)
                {
                    tw.WriteLine(10 + f.Hard);
                } else
                {
                    tw.WriteLine(f.Player);
                }
                for (int i = 0; i < Program.row; i++)
                {
                    for (int j = 0; j < Program.col; j++)
                    {
                        tw.Write(map[i, j] + " ");
                    }
                    tw.WriteLine();
                }
            }
        }

        public static int[,] load(FormBanCo f)
        {
            int[,] result = new int[Program.row, Program.col];
            int i = 0, j = 0, p = 0;
            foreach (var row in File.ReadLines(@"../data.txt"))
            {
                if (p == 0)
                {
                    int r = Int32.Parse(row);
                    int a = r / 10;
                    int b = r % 10;

                    if (a == 1)
                    {
                        f.isComputer = true;
                        f.Hard = b;
                    } else
                    {
                        f.isComputer = false;
                        f.Player = b;
                    }
                    
                    p++;
                    continue;
                }
                j = 0;
                foreach (var col in row.Trim().Split(' '))
                {
                    result[i, j] = Int32.Parse(col);
                    j++;
                }
                Console.WriteLine();

                i++;
            }
            return result;
        }
    }
}
