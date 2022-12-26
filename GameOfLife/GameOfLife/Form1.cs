using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.Design;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace GameOfLife
{
    public partial class Form1 : Form
    {
        private Graphics graphics;

        private int resolution;
        private int density;

        private bool[,] field;
        private int rows;
        private int cols;

        private int currentGeneration;

        public Form1()
        {
            InitializeComponent();
        }

        private void startGame()
        {
            if (timer1.Enabled)
                return;

            currentGeneration = 0;
            Text = $"Generation {currentGeneration}";

            btnStart.Enabled = false;
            btnStop.Enabled = true;

            numericResolution.Enabled = false;
            numericDensity.Enabled = false;

            resolution = (int)numericResolution.Value;
            density = (int)numericDensity.Value;

            rows = pictureBox1.Height / resolution;
            cols = pictureBox1.Width / resolution;

            field = new bool[cols, rows];

            Random random = new Random();

            for (int x = 0; x < cols; x++)
            {
                for (int y = 0; y < rows; y++)
                {
                    field[x, y] = random.Next(density + 1) == 0;
                }
            }

            pictureBox1.Image = new Bitmap(pictureBox1.Width, pictureBox1.Height);
            graphics = Graphics.FromImage(pictureBox1.Image);

            timer1.Start();
        }

        private void nextGeneration()
        {
            graphics.Clear(Color.Black);

            bool[,] newField = new bool[cols, rows];

            for (int x = 0; x < cols; x++)
            {
                for (int y = 0; y < rows; y++)
                {
                    int neighbours = neighboursCount(x, y);
                    bool hasLife = field[x, y];

                    if (hasLife && (neighbours == 2 || neighbours == 3))
                        newField[x, y] = true;
                    if (hasLife && (neighbours < 2 || neighbours > 4))
                        newField[x, y] = false;
                    if (!hasLife && neighbours == 3)
                        newField[x, y] = true;

                    if (field[x, y])
                        graphics.FillRectangle(Brushes.Crimson, x * resolution, y * resolution, resolution, resolution);
                }
            }

            field = newField;
            pictureBox1.Refresh();

            Text = $"Generation {++currentGeneration}";
        }

        private int neighboursCount(int x, int y)
        {
            int count = 0;

            for (int nx = -1; nx <= 1; nx++)
            {
                for (int ny = -1; ny <= 1; ny++)
                {
                    int tx = (x + nx + cols) % cols;
                    int ty = (y + ny + rows) % rows;

                    bool isSelf = tx == x && ty == y;

                    if (!isSelf && field[tx, ty] )
                        count++;
                }
            }

            return count;
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            nextGeneration();
        }

        private void btnStart_Click(object sender, EventArgs e)
        {
            startGame();
        }

        private void btnStop_Click(object sender, EventArgs e)
        {
            timer1.Stop();

            numericResolution.Enabled = true;
            numericDensity.Enabled = true;

            btnStart.Enabled = true;
            btnStop.Enabled = false;
        }

        private void pictureBox1_MouseMove(object sender, MouseEventArgs e)
        { 
            if (e.Button == MouseButtons.Left)
            {
                int mx = e.Location.X / resolution;
                int my = e.Location.Y / resolution;

                field[mx, my] = true;
            }

            if (e.Button == MouseButtons.Right)
            {
                int mx = e.Location.X / resolution;
                int my = e.Location.Y / resolution;

                field[mx, my] = false;
            }
        }
    }
}
