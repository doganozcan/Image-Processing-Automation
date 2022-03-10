using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace WindowsFormsApp1._1
{
    public partial class RenkBul : Form
    {
        public RenkBul()
        {
            InitializeComponent();
        }

        public Bitmap resimRenkBul;
        private void pictureBox1_MouseClick(object sender, MouseEventArgs e)
        {
            
            if (resimRenkBul != null)
            {
                Color renk = resimRenkBul.GetPixel(e.X, e.Y);
                panel1.BackColor = renk;
                textBox2.Text = e.X.ToString();
                textBox3.Text = e.Y.ToString();
                textBox1.Text = renk.Name; 
            }
        }

        private void RenkBul_Load(object sender, EventArgs e)
        {
            pictureBox1.Image = resimRenkBul;
        }
    }
}
