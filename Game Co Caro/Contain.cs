using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Game_Co_Caro
{
    class Contain
    {
        public int cooldown_time = 100;
        public int cooldown_step = 10000;
        public int cooldown_interval = 100;
        public string nameplayer;
        public int edgeChess;
        public int row = Program.row;
        public int col = Program.col;
        public int borderH, borderV;
        private int w, h;

        public Contain(Panel p)
        {
            col = col - 1;
            w = p.Width;
            h = p.Height;
            int tempW, tempH;

            tempW = w / (col + 2);
            tempH = h / (row + 2);

            edgeChess = (tempW + tempH) / 2;

            borderH = (w - ((edgeChess+1)*col))/ 2;
            borderV = (h - ((edgeChess+1)*row))/ 2;

            
            Console.WriteLine(w + " " + h);
        }

    }
}
