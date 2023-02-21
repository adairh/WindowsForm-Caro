using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net.NetworkInformation;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Windows.Media;

namespace Game_Co_Caro
{
    public partial class FormBanCo : Form
    {
        private Label[,] Map;
        private static int columns, rows;
        
        private int player;
        private bool gameover;
        private bool vsComputer;
        private int[,] vtMap;
        private Stack<Chess> chesses;
        private Chess chess;

        private Contain contain;

        private Random r;





        MusicForm mf;
        bool mfShow = false;

        public bool isComputer
        {
            get
            {
                return vsComputer;
            }
            set
            {
                vsComputer = value;
            }
        }

        public int Player
        {
            get
            {
                return player;
            }
            set
            {
                player = value;
            }
        }


        public int Hard
        {
            get
            {
                return level;
            }
            set
            {
                level = value;
            }
        }
        public FormBanCo(bool isComputer)
        {
            r = new Random();
            columns = Program.col;
            rows = Program.row;

            mf = new MusicForm();
            //mf.Show();
            //mf.Hide();

            vsComputer = isComputer;
            gameover = false;
            player = 1;          
            Map = new Label[rows, columns];
            vtMap = new int[rows, columns];
            chesses = new Stack<Chess>();
            
            InitializeComponent();
            
            contain = new Contain(pnTableChess);
            panel1.BackColor = System.Drawing.Color.FromArgb(0, 0, 0, 0);
            btnLAN.BackColor = System.Drawing.Color.FromArgb(0, 0, 0, 0);
            menuStrip1.BackColor = System.Drawing.Color.FromArgb(0, 0, 0, 0);
            menuStrip2.BackColor = System.Drawing.Color.FromArgb(0, 0, 0, 0);
            pnTableChess.BackColor = System.Drawing.Color.FromArgb(0, 0, 0, 0);
            this.BackColor = System.Drawing.Color.White;
            //this.TransparencyKey = Color.Black;
            //this.BackgroundImage = Properties.Resources.galaxy;
            //socket = new SocketManager();


            

            BuildTable(Map);
            //PictureBox picture = new PictureBox
            //{
            //    Size = new Size(this.Width, this.Height),
            //    Location = new Point(0, 0),
            //    Image = Properties.Resources.galaxy,
            //    SizeMode = PictureBoxSizeMode.AutoSize
            //};
            //this.Controls.Add(picture);
        }

        private void player1VsPlayer2(object sender, EventArgs e)
        {
            //vsComputer = false;
            //gameover = false;
            //psbCooldownTime.Value = 0;
            //tmCooldown.Stop();
            //pnTableChess.Controls.Clear();

            //txtNamePlayer.Text = "";
            //ptbPayer.Image = null;
            //menuStrip1.Parent = pnTableChess;
            //player = 1;
            //Map = new Label[rows + 2, columns + 2];
            //vtMap = new int[rows + 2, columns + 2];
            //chesses = new Stack<Chess>();
            //Form1_Load(false);
            //Loading(pnTableChess);


            //var thread2 = new Thread(new ThreadStart(ActiveLoad));
            //var thread = new Thread(new ThreadStart(ActiveBuild));

            //thread2.Start();
            //thread.Start();



            //pnTableChess.Controls.Remove(picture);
            reload(false);
        }

        private void PlayVsComputer1(object sender, EventArgs e)
        {
            level = 1;
            reload(true);
        }
        private void PlayVsComputer2(object sender, EventArgs e)
        {
            level = 2;
            reload(true);
        }
        private void PlayVsComputer3(object sender, EventArgs e)
        {
            level = 3;
            reload(true);
        }
        private void PlayVsComputer5(object sender, EventArgs e)
        {
            level = 5;
            reload(true);
        }


        bool done = false;
        public int level = 1;

        private void reload(bool computer)
        {

            this.Hide();
            vsComputer = computer;
            gameover = false;
            psbCooldownTime.Value = 0;
            tmCooldown.Stop();
            pnTableChess.Controls.Clear();

            ptbPayer.Image = Properties.Resources.onnnn;
            txtNamePlayer.Text = "Player";
            //menuStrip1.Parent = pnTableChess;
            player = 1;
            Map = new Label[rows, columns];
            vtMap = new int[rows, columns];
            chesses = new Stack<Chess>();
            BuildTable(Map);
            this.Show();
        }

        private void BuildTable(Label[,] Mapp)
        {

            for (int i = 0; i < rows; i++)
                for (int j = 0; j < columns; j++)
                {
                    //Console.WriteLine(i + " - " + j);
                    Label l = new Label();

                    l.Parent = pnTableChess;
                    l.Size = new Size(contain.edgeChess - 1, contain.edgeChess - 1);
                    l.BackColor = System.Drawing.Color.Snow;

                    l.MouseLeave += Form1_MouseLeave;
                    l.MouseEnter += Form1_MouseEnter;
                    l.Click += Form1_Click;
                    l.Top = (i) * contain.edgeChess + contain.borderV;
                    l.Left = (j) * contain.edgeChess + contain.borderH;
                    l.BorderStyle = BorderStyle.Fixed3D;

                    //l.Text = i + " : " + j;
                    Mapp[i, j] = l;
                }

            done = true;
        }

        private void Form1_Click(object sender, EventArgs e)
        {
            if (gameover)
                return;
            Label lb = (Label)sender;
            int x = (lb.Top-contain.borderV) / contain.edgeChess, y = (lb.Left - contain.borderH) / contain.edgeChess;
            if (vtMap[x, y] != 0)
                return;

            Console.WriteLine("X: " + x + "Y: " + y);
            if (vsComputer)
            {
                player = 1;
                psbCooldownTime.Value = 0;
                tmCooldown.Start();
                lb.Image = Properties.Resources.o;             
                vtMap[x, y] = 1;
                Check(x, y);
                //Console.WriteLine(level);
                switch (level)
                {
                    case 1:
                        randomlyPlaceChess();
                        break;
                    case 2:
                        CptFindChessATK();
                        break;
                    case 3:
                        CptFindChessDEF();
                        break;
                    case 4:
                        CptFind();
                        break;
                    case 5:
                        CptFindChess();
                        break;

                }
            }
            else
            {
                if (player == 1)
                {
                    psbCooldownTime.Value = 0;
                    tmCooldown.Start();
                    lb.Image = Properties.Resources.o;                  
                    vtMap[x, y] = 1;
                    Check(x, y);

                    player = 2;
                    ptbPayer.Image = Properties.Resources.x_copy;
                    txtNamePlayer.Text = "Player 2";
                }
                else
                {
                    psbCooldownTime.Value = 0;
                    lb.Image = Properties.Resources.x;                   
                    vtMap[x, y] = 2;
                    Check(x, y);

                    player = 1;
                    ptbPayer.Image = Properties.Resources.onnnn;
                    txtNamePlayer.Text = "Player 1";
                }
            }
            chess = new Chess(lb, x, y);
            chesses.Push(chess);
            //SoundPlayer simpleSound = new SoundPlayer((string)Properties.Resources.DING);
            //simpleSound.Play();
        }

        private void Form1_MouseEnter(object sender, EventArgs e)
        {
            if (gameover)
                return;
            Label lb = (Label)sender;
            lb.BackColor = System.Drawing.Color.AliceBlue;
        }

        private void Form1_MouseLeave(object sender, EventArgs e)
        {
            if (gameover)
                return;
            Label lb = (Label)sender;
            lb.BackColor = System.Drawing.Color.Snow;
        }

        private void tmCooldown_Tick(object sender, EventArgs e)
        {
            psbCooldownTime.PerformStep();
            if (psbCooldownTime.Value >= psbCooldownTime.Maximum)
            {
                Gameover();
            }
        }
      
        private void menuUndo_Click(object sender, EventArgs e)
        {
            if (!vsComputer)
            {
                Chess template = new Chess();
                template = chesses.Pop();
                template.lb.Image = null;
                vtMap[template.X, template.Y] = 0;
                psbCooldownTime.Value = 0;
                ChangePlayer();
            }
            else
            {

                Chess template = new Chess();
                template = chesses.Pop();
                template.lb.Image = null;
                vtMap[template.X, template.Y] = 0;

                template = chesses.Pop();
                template.lb.Image = null;
                vtMap[template.X, template.Y] = 0;

                psbCooldownTime.Value = 0;
                player = 1;
            }
        }

        private void menuQuit_Click_1(object sender, EventArgs e)
        {
            DialogResult dialog;
            dialog = MessageBox.Show("Bạn có chắc muốn thoát không?", "Thông báo", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            if (dialog == DialogResult.Yes)
            {
                this.Dispose();
                this.Close();
            }
        }

        private void Gameover()
        {
            tmCooldown.Stop();
            gameover = true;
            backgroundgameover();
        }
        private void backgroundgameover()
        {
            for (int i = 0; i < rows; i++)
                for (int j = 0; j < columns; j++)
                {
                    Map[i, j].BackColor = System.Drawing.Color.Gray;
                }
        }

        private void ChangePlayer()
        {
            if (player == 1)
            {
                player = 2;
                txtNamePlayer.Text = "Player 2";
                ptbPayer.Image = Properties.Resources.x_copy;
            }
            else
            {
                player = 1;
                txtNamePlayer.Text = "Player 1";
                ptbPayer.Image = Properties.Resources.onnnn;
            }

        }

        private void Check(int x, int y)
        {
            try
            {
                int i = x - 1, j = y;
                int column = 1, row = 1, mdiagonal = 1, ediagonal = 1;
                //Console.WriteLine("X:" + x + " Y:" + y + " I:" + i + " J:" + j);
                if (i < 0 || j < 0) return;
                while (i >= 0 && vtMap[x, y] ==
                    vtMap[i, j])
                {
                    column++;
                    i--;
                }
                i = x + 1;
                while (i <= rows && vtMap[x, y] == vtMap[i, j])
                {
                    column++;
                    i++;
                }
                i = x; j = y - 1;
                if (i < 0 || j < 0) return;
                while (j >= 0 && vtMap[x, y] == vtMap[i, j])
                {
                    row++;
                    j--;
                }
                j = y + 1;
                while (j <= columns && vtMap[x, y] == vtMap[i, j])
                {
                    row++;
                    j++;
                }
                i = x - 1; j = y - 1;
                if (i < 0 || j < 0) return;
                while (i >= 0 && j >= 0 && vtMap[x, y] == vtMap[i, j])
                {
                    mdiagonal++;
                    i--;
                    j--;
                }
                i = x + 1; j = y + 1;
                if (i >= Program.row || j >= Program.col) return;
                while (i <= rows && j <= columns && vtMap[x, y] == vtMap[i, j])
                {
                    mdiagonal++;
                    i++;
                    j++;
                }
                i = x - 1; j = y + 1;
                if (i < 0 || j >= Program.col) return;
                while (i >= 0 && j <= columns && vtMap[x, y] == vtMap[i, j])
                {
                    ediagonal++;
                    i--;
                    j++;
                }
                i = x + 1; j = y - 1;
                if (i >= Program.row || j < 0) return;
                while (i <= rows && j >= 0 && vtMap[x, y] == vtMap[i, j])
                {
                    ediagonal++;
                    i++;
                    j--;
                }
                if (row >= 5 || column >= 5 || mdiagonal >= 5 || ediagonal >= 5)
                {
                    Gameover();
                    if (vsComputer)
                    {
                        if (player == 1)
                            MessageBox.Show("You win!!");
                        else
                            MessageBox.Show("You lost!!");
                    }
                    else
                    {
                        if (player == 1)
                            MessageBox.Show("Player 1 Win");
                        else
                            MessageBox.Show("Player 2 Win");
                    }
                }
            } catch (IndexOutOfRangeException e)
            {
                return;
            }

        }

        private void PutCircleChess(int x, int y)
        {
            if (vsComputer)
                player = 0;
            psbCooldownTime.Value = 0;
            Map[x, y].Image = Properties.Resources.o;

            vtMap[x, y] = 1;
            Check(x, y);

            chess = new Chess(Map[x, y], x, y);
            chesses.Push(chess);
        }

        private void PutChess(int x, int y)
        {
            if (vsComputer)
                player = 0;
            psbCooldownTime.Value = 0;
            Map[x, y].Image = Properties.Resources.x;

            vtMap[x, y] = 2;
            Check(x, y);

            chess = new Chess(Map[x, y], x, y);
            chesses.Push(chess);
        }




        #region AI

        private int[] Attack = new int[7] { 0, 9, 54, 162, 1458, 13112, 118008 };
        private int[] Defense = new int[7] { 0, 3, 27, 99, 729, 6561, 59049 };
        private readonly object tmCoolDown;


        private void randomlyPlaceChess()
        {
            bool got = false;
            int i, j;
            while (!got)
            {
                i = r.Next() % Program.row;
                j = r.Next() % Program.col;

                if (vtMap[i, j] == 0)
                {
                    got = true;
                    PutChess(i, j);
                }
            }
        } 

        private void CptFind()
        {
            int i = r.Next() % 2;
            if (i == 0)
            {
                CptFindChessATK();
            } else
            {
                CptFindChessDEF();
            }
        }
        
        private void CptFindChess()
        {
            if (gameover) return;
            long max = 0;
            int imax = 1, jmax = 1;
            for (int i = 0; i < rows; i++)
            {
                for (int j = 0; j < columns; j++)
                    if (vtMap[i, j] == 0)
                    {
                        long temp = Caculate(i, j);
                        if (temp > max)
                        {
                            max = temp;
                            imax = i; jmax = j;
                        }
                    }
            }
            PutChess(imax, jmax);
        }

        private void CptFindChessDEF()
        {
            if (gameover) return;
            long max = 0;
            int imax = 1, jmax = 1;
            for (int i = 0; i < rows; i++)
            {
                for (int j = 0; j < columns; j++)
                    if (vtMap[i, j] == 0)
                    {
                        long temp = CaculateDEF(i, j);
                        if (temp > max)
                        {
                            max = temp;
                            imax = i; jmax = j;
                        }
                    }
            }
            PutChess(imax, jmax);
        }

        private void CptFindChessATK()
        {
            if (gameover) return;
            long max = 0;
            int imax = 1, jmax = 1;
            for (int i = 0; i < rows; i++)
            {
                for (int j = 0; j < columns; j++)
                    if (vtMap[i, j] == 0)
                    {
                        long temp = CaculateATK(i, j);
                        if (temp > max)
                        {
                            max = temp;
                            imax = i; jmax = j;
                        }
                    }
            }
            PutChess(imax, jmax);
        }

        private long Caculate(int x, int y)
        {
            return EnemyChesses(x, y) + ComputerChesses(x, y);
        }

        private long CaculateATK(int x, int y)
        {
            return ComputerChesses(x, y);
        }


        private long CaculateDEF(int x, int y)
        {
            return EnemyChesses(x, y);
        }


        private long ComputerChesses(int x, int y)
        {
            try
            {
                int i = x - 1, j = y;
                int column = 0, row = 0, mdiagonal = 0, ediagonal = 0;
                int sc_ = 0, sc = 0, sr_ = 0, sr = 0, sm_ = 0, sm = 0, se_ = 0, se = 0;
                while (vtMap[i, j] == 2 && i >= 0)
                {
                    column++;
                    i--;
                }
                if (vtMap[i, j] == 0) sc_ = 1;
                i = x + 1;
                while (vtMap[i, j] == 2 && i <= rows)
                {
                    column++;
                    i++;
                }
                if (vtMap[i, j] == 0) sc = 1;
                i = x; j = y - 1;
                while (vtMap[i, j] == 2 && j >= 0)
                {
                    row++;
                    j--;
                }
                if (vtMap[i, j] == 0) sr_ = 1;
                j = y + 1;
                while (vtMap[i, j] == 2 && j <= columns)
                {
                    row++;
                    j++;
                }
                if (vtMap[i, j] == 0) sr = 1;
                i = x - 1; j = y - 1;
                while (vtMap[i, j] == 2 && i >= 0 && j >= 0)
                {
                    mdiagonal++;
                    i--;
                    j--;
                }
                if (vtMap[i, j] == 0) sm_ = 1;
                i = x + 1; j = y + 1;
                while (vtMap[i, j] == 2 && i <= rows && j <= columns)
                {
                    mdiagonal++;
                    i++;
                    j++;
                }
                if (vtMap[i, j] == 0) sm = 1;
                i = x - 1; j = y + 1;
                while (vtMap[i, j] == 2 && i >= 0 && j <= columns)
                {
                    ediagonal++;
                    i--;
                    j++;
                }
                if (vtMap[i, j] == 0) se_ = 1;
                i = x + 1; j = y - 1;
                while (vtMap[i, j] == 2 && i <= rows && j >= 0)
                {
                    ediagonal++;
                    i++;
                    j--;
                }
                if (vtMap[i, j] == 0) se = 1;

                if (column == 4) column = 5;
                if (row == 4) row = 5;
                if (mdiagonal == 4) mdiagonal = 5;
                if (ediagonal == 4) ediagonal = 5;

                if (column == 3 && sc == 1 && sc_ == 1) column = 4;
                if (row == 3 && sr == 1 && sr_ == 1) row = 4;
                if (mdiagonal == 3 && sm == 1 && sm_ == 1) mdiagonal = 4;
                if (ediagonal == 3 && se == 1 && se_ == 1) ediagonal = 4;

                if (column == 2 && row == 2 && sc == 1 && sc_ == 1 && sr == 1 && sr_ == 1) column = 3;
                if (column == 2 && mdiagonal == 2 && sc == 1 && sc_ == 1 && sm == 1 && sm_ == 1) column = 3;
                if (column == 2 && ediagonal == 2 && sc == 1 && sc_ == 1 && se == 1 && se_ == 1) column = 3;
                if (row == 2 && mdiagonal == 2 && sm == 1 && sm_ == 1 && sr == 1 && sr_ == 1) column = 3;
                if (row == 2 && ediagonal == 2 && se == 1 && se_ == 1 && sr == 1 && sr_ == 1) column = 3;
                if (ediagonal == 2 && mdiagonal == 2 && sm == 1 && sm_ == 1 && se == 1 && se_ == 1) column = 3;

                long Sum = Attack[row] + Attack[column] + Attack[mdiagonal] + Attack[ediagonal];

                return Sum;
            } catch (IndexOutOfRangeException e)
            {
                return 0;
            }
        }

        private long EnemyChesses(int x, int y)
        {
            try
            {
                int i = x - 1, j = y;
                int sc_ = 0, sc = 0, sr_ = 0, sr = 0, sm_ = 0, sm = 0, se_ = 0, se = 0;
                int column = 0, row = 0, mdiagonal = 0, ediagonal = 0;
                while (i >= 0 && vtMap[i, j] == 1)
                {
                    column++;
                    i--;
                }
                if (vtMap[i, j] == 0) sc_ = 1;
                i = x + 1;
                while (i <= rows && vtMap[i, j] == 1)
                {
                    column++;
                    i++;
                }
                if (vtMap[i, j] == 0) sc = 1;
                i = x; j = y - 1;
                while (j >= 0 && vtMap[i, j] == 1)
                {
                    row++;
                    j--;
                }
                if (vtMap[i, j] == 0) sr_ = 1;
                j = y + 1;
                while (j <= columns && vtMap[i, j] == 1)
                {
                    row++;
                    j++;
                }
                if (vtMap[i, j] == 0) sr = 1;
                i = x - 1; j = y - 1;
                while (i >= 0 && j >= 0 && vtMap[i, j] == 1)
                {
                    mdiagonal++;
                    i--;
                    j--;
                }
                if (vtMap[i, j] == 0) sm_ = 1;
                i = x + 1; j = y + 1;
                while (i <= rows && j <= columns && vtMap[i, j] == 1)
                {
                    mdiagonal++;
                    i++;
                    j++;
                }
                if (vtMap[i, j] == 0) sm = 1;
                i = x - 1; j = y + 1;
                while (i >= 0 && j <= columns && vtMap[i, j] == 1)
                {
                    ediagonal++;
                    i--;
                    j++;
                }
                if (vtMap[i, j] == 0) se_ = 1;
                i = x + 1; j = y - 1;
                while (i <= rows && j >= 0 && vtMap[i, j] == 1)
                {
                    ediagonal++;
                    i++;
                    j--;
                }
                if (vtMap[i, j] == 0) se = 1;

                if (column == 4) column = 5;
                if (row == 4) row = 5;
                if (mdiagonal == 4) mdiagonal = 5;
                if (ediagonal == 4) ediagonal = 5;

                if (column == 3 && sc == 1 && sc_ == 1) column = 4;
                if (row == 3 && sr == 1 && sr_ == 1) row = 4;
                if (mdiagonal == 3 && sm == 1 && sm_ == 1) mdiagonal = 4;
                if (ediagonal == 3 && se == 1 && se_ == 1) ediagonal = 4;

                if (column == 2 && row == 2 && sc == 1 && sc_ == 1 && sr == 1 && sr_ == 1) column = 3;
                if (column == 2 && mdiagonal == 2 && sc == 1 && sc_ == 1 && sm == 1 && sm_ == 1) column = 3;
                if (column == 2 && ediagonal == 2 && sc == 1 && sc_ == 1 && se == 1 && se_ == 1) column = 3;
                if (row == 2 && mdiagonal == 2 && sm == 1 && sm_ == 1 && sr == 1 && sr_ == 1) column = 3;
                if (row == 2 && ediagonal == 2 && se == 1 && se_ == 1 && sr == 1 && sr_ == 1) column = 3;
                if (ediagonal == 2 && mdiagonal == 2 && sm == 1 && sm_ == 1 && se == 1 && se_ == 1) column = 3;
                long Sum = Defense[row] + Defense[column] + Defense[mdiagonal] + Defense[ediagonal];

                return Sum;
            } catch (IndexOutOfRangeException e)
            {
                return 0;
            }
        }

        private void toolStripMenuItem18_Click(object sender, EventArgs e)
        {
            Save.save(vtMap, this);
        }

        private bool MFShow
        {
            get { return mfShow; }
            set
            {
                mfShow = value;
                if (!mfShow)
                {
                    mf.show = false;
                    mf.Hide();
                } else
                {
                    mf.show = true;
                    mf.Show();
                }
            }
        }

        private void musicButton_Click(object sender, EventArgs e)
        {
            //Console.WriteLine(mfShow);
            MFShow = !MFShow;
            //Console.WriteLine(mfShow);
        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {

        }

        private void toolStripMenuItem19_Click(object sender, EventArgs e)
        {
            reload(vsComputer);
            int[,] m = Save.load(this);
            for (int i = 0; i < rows; i++)
                for (int j = 0; j < columns; j++) { 
                    //Map[j, i] = new Label();
                    //Console.WriteLine(vtMap[i, j].ToString());
                    Console.WriteLine("Node " + i + " : " + j);
                    if (m[i, j] == 2)
                    {
                        PutChess(i, j);
                    } else if (m[i, j] == 1)
                    {
                        PutCircleChess(i, j);
                    }
                    //Map[i, j].Image = (vtMap[i, j] == 2 ? Properties.Resources.x : Properties.Resources.o);
                    //chess = new Chess(Map[i + 1, j], i, j);
                    //chesses.Push(chess);
                }

            Console.WriteLine(Player + " time ");

        }



    

        #endregion

    }

    public class Chess
    {
        public Label lb;
        public int X;
        public int Y;
        public Chess()
        {
            lb = new Label();
        }
        public Chess(Label _lb, int x, int y)
        {
            lb = new Label();
            lb = _lb;
            X = x;
            Y = y;
        }

        internal static void OtherPlayerMark(Point point)
        {
            throw new NotImplementedException();
        }
    }

}