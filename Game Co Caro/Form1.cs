using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Game_Co_Caro
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            gifBox.Location = new Point(0, 0);
            gifBox.Image = Properties.Resources.nyan_cat;

            gifBox.SizeMode = PictureBoxSizeMode.Zoom;
            Console.WriteLine("nyan");
        }
    }
}
