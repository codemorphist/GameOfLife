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
        private GameEngine gameEngine;
        private Graphics graphics;
        private bool isGraphics = false;
        private bool isStopped = false;
        private int resolution = 2;

        private bool[,] field;

        public Form1()
        {
            InitializeComponent();
        }

        private void StartGame()
        {
            btnStart.Enabled = false;
            btnStop.Enabled = true;
            btnClear.Enabled = true;

            resolution = (int)numericResolution.Value;

            numericResolution.Enabled = false;
            numericDensity.Enabled = false;

            isGraphics = true;
            isStopped = false;

            btnStop.Text = "PAUSE";

            gameEngine = new GameEngine
            (
                rows: pictureBox1.Height / resolution,
                cols: pictureBox1.Width / resolution,
                density: (int)numericDensity.Value
            );

            Text = $"Generation {gameEngine.CurrentGeneration}";

            pictureBox1.Image = new Bitmap(pictureBox1.Width, pictureBox1.Height);
            graphics = Graphics.FromImage(pictureBox1.Image);
            graphics.Clear(Color.Black);

            timer1.Start();
        }

        private void PauseGame()
        {
            numericResolution.Enabled = true;
            numericDensity.Enabled = true;

            if (!isStopped)
            {
                isStopped = true;
                btnStop.Text = "CONTINUE";
                timer1.Stop();
            }
            else
            {
                numericResolution.Enabled = false;
                numericDensity.Enabled = false;

                isStopped = false;
                btnStop.Text = "PAUSE";
                timer1.Start();
            }

            btnStart.Text = "RESET";
            btnStart.Enabled = true;
        }


        private void RandomGeneration()
        {

        }

        private void NextGeneration()
        {
            graphics.Clear(Color.Black);

            field = gameEngine.GetCurrentField();

            for (int x = 0; x < field.GetLength(0); x++)
            {
                for (int y = 0; y < field.GetLength(1); y++)
                {
                    if (field[x, y])
                        graphics.FillRectangle(Brushes.Crimson, x * resolution, y * resolution, resolution - 1, resolution - 1);
                } 
            }

            pictureBox1.Refresh();
            Text = $"Generation {gameEngine.CurrentGeneration}";
            gameEngine.NextGeneration();
        }

        private void UpdateGame()
        {
            graphics.Clear(Color.Black);

            field = gameEngine.GetCurrentField();

            for (int x = 0; x < field.GetLength(0); x++)
            {
                for (int y = 0; y < field.GetLength(1); y++)
                {
                    if (field[x, y])
                        graphics.FillRectangle(Brushes.Crimson, x * resolution, y * resolution, resolution - 1, resolution - 1);
                }
            }

            pictureBox1.Refresh();
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            NextGeneration();

        }

        private void btnStart_Click(object sender, EventArgs e)
        {
            StartGame();
        }

        private void btnStop_Click(object sender, EventArgs e)
        {
            PauseGame();
        }

        private void pictureBox1_MouseMove(object sender, MouseEventArgs e)
        {
            if (isGraphics)
            {
                int mx = e.Location.X / resolution;
                int my = e.Location.Y / resolution;

                if (e.Button == MouseButtons.Left)
                    gameEngine.AddCell(mx, my);


                if (e.Button == MouseButtons.Right)
                    gameEngine.RemoveCell(mx, my);

                UpdateGame();
            }
        }

        private void btnClear_Click(object sender, EventArgs e)
        {
            if (isGraphics)
            {
                gameEngine.ClearAllCells();
                UpdateGame();
            }
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            StartGame();
            PauseGame();
        }

        private void trackBar1_MouseMove(object sender, MouseEventArgs e)
        {
            timer1.Interval = (int)trackBar1.Value;
        }

        private void btnRandom_Click(object sender, EventArgs e)
        {
            StartGame();
            gameEngine.RandomGeneration();
        }
    }
}
