using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static System.Net.Mime.MediaTypeNames;
using System.Windows.Forms;
using System.Runtime.InteropServices;

namespace GameOfLife
{
    public class GameEngine
    {
        private bool[,] field;        
        private readonly int rows;
        private readonly int cols;
        private readonly int density;
        public uint CurrentGeneration { get; private set; }

        private Random random = new Random();


        public GameEngine(int rows, int cols, int density)
        {
            this.rows = rows;
            this.cols = cols;
            this.density = density;

            this.field = new bool[cols, rows];

            for (int x = 0; x < cols; x++)
            {
                for (int y = 0; y < rows; y++)
                {
                    this.field[x, y] = false;
                    //this.field[x, y] = random.Next(density + 1) == 0;
                }
            }

            CurrentGeneration = 0;
        }

        public void RandomGeneration()
        {
            for (int x = 0; x < cols; x++)
            {
                for (int y = 0; y < rows; y++)
                {
                    this.field[x, y] = random.Next(density + 1) == 0;
                }
            }

            CurrentGeneration = 0;
        }

        public void NextGeneration()
        {
            bool[,] newField = new bool[cols, rows];

            for (int x = 0; x < cols; x++)
            {
                for (int y = 0; y < rows; y++)
                {
                    int neighbours = NeighboursCount(x, y);
                    bool hasLife = this.field[x, y];

                    if (hasLife && (neighbours == 2 || neighbours == 3))
                        newField[x, y] = true;
                    if (hasLife && (neighbours < 2 || neighbours > 4))
                        newField[x, y] = false;
                    if (!hasLife && neighbours == 3)
                        newField[x, y] = true;
                }
            }
            CurrentGeneration++;
            this.field = newField;
        }

        private int NeighboursCount(int x, int y)
        {
            int count = 0;

            for (int nx = -1; nx <= 1; nx++)
            {
                for (int ny = -1; ny <= 1; ny++)
                {
                    int tx = (x + nx + cols) % cols;
                    int ty = (y + ny + rows) % rows;

                    bool isSelf = tx == x && ty == y;

                    if (!isSelf && this.field[tx, ty])
                        count++;
                }
            }
            return count;
        }

        private bool ValidateCellPosition(int x, int y)
        {
            return x >= 0 && y >= 0 && x < cols && y < rows;
        }

        private void UpdateSell(int x, int y, bool state) 
        {
            if (ValidateCellPosition(x, y))
                this.field[x, y] = state;
        }

        public void AddCell(int x, int y)
        {
            UpdateSell(x, y, state: true);
        }

        public void RemoveCell(int x, int y)
        {
            UpdateSell(x, y, state: false);
        }

        public void ClearAllCells()
        {
            for (int x = 0; x < cols; x++)
            {
                for (int y = 0; y < rows; y++)
                {
                    this.field[x, y] = false;
                }
            }
        }

        public bool[,] GetCurrentField()
        {
            bool[,] currentField = this.field;
            return currentField;
        }
    }
}
