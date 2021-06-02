using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace WindowsFormsApp1
{
    public partial class Form1 : Form
    {
        static int SIZE = 100;
        static int CELL_SIZE = 5;
        static bool[,] grid;
        static int counter = 0; 
        public Form1()
        {
            InitializeComponent();
            grid = new bool[SIZE, SIZE];
        }

        private void button1_Click(object sender, EventArgs e)
        {

            for (int i = 0; i < SIZE; i++)
                for (int j = 0; j < SIZE; j++)
                {
                    grid[i, j] = false;
                }

            Random rd = new Random();
            for(int i = 0; i<SIZE; i++)
                for(int j = 0; j<SIZE; j++)
                {
                    int n = rd.Next(0, 2);
                    grid[i,j] = (n == 0);
                }

            DrawGrid();
            timer1.Enabled = true;

        }

      

        void DrawGrid()
        {
            Bitmap bitmap = new Bitmap(pb.Width, pb.Height);
            for(int x = 0; x < SIZE; x++)
            {
                for(int y = 0; y<SIZE; y++)
                {
                    if(grid[x,y] == false)
                        DrawPixel(bitmap, x, y, Color.White);
                    else
                        DrawPixel(bitmap, x, y, Color.Green);
                }
            }
            pb.Image = bitmap;
        }

        void DrawPixel(Bitmap bmp, int x, int y, Color color)
        {
            for(int xDir = 0; xDir < CELL_SIZE; xDir++)
                for(int yDir = 0; yDir < CELL_SIZE; yDir++)
                {
                    bmp.SetPixel(xDir + (x * CELL_SIZE), yDir + (y * CELL_SIZE), color);
                }
        }
        static bool IsValid(int x, int y) => ((x < 0 || y < 0) || (x >= SIZE || y >= SIZE)) ? false : true;


        //rules 
        /// <summary>
        ///  una celula muerta con exactamente 3 celulas vivas nace 
        ///  una celula viva con 2 o 3 celulas sigue viva, en otro caso dif muere
        /// </summary>

        void StartGame()
        {
            counter++;
            label1.Text = counter + ""; 
            int cellsAlive;
            bool[,] nextGen = new bool[SIZE, SIZE];
            DrawGrid();    
            for (int i = 0; i < grid.GetLength(0); i++)
            {
                for (int j = 0; j < grid.GetLength(1); j++)
                {
                    cellsAlive = CalculateNeigbors(i, j);
                    if (grid[i, j] == true)
                    {
                        if (cellsAlive == 2 || cellsAlive == 3)
                        {
                            nextGen[i, j] = true;
                        }
                        else
                        {
                            nextGen[i, j] = false;
                        }
                    }
                    else if (grid[i, j] == false)
                    {
                        if (cellsAlive == 3)
                        {
                            nextGen[i, j] = true;
                        }
                    }
                }
            }
            //pasamos el resultado de la proxima gen a la actual despues de válidar todo
            grid = (bool[,])nextGen.Clone();
        }
        static int CalculateNeigbors(int row, int cell)
        {
            Vecino[,] vecinos =
            {
                {new Vecino(-1, -1), new Vecino(-1, 0), new Vecino(-1, 1)},
                {new Vecino(0, -1), new Vecino(0, 0), new Vecino(0, 1)},
                {new Vecino(1, -1), new Vecino(1, 0), new Vecino(1, 1)}
            };

            int x = 0, y = 0;
            int cellAlive = 0;
            for (int i = 0; i < vecinos.GetLength(0); i++)
            {
                for (int j = 0; j < vecinos.GetLength(0); j++)
                {
                    //sacando los vecinos de la casilla actual
                    x = row + vecinos[i, j].X;
                    y = cell + vecinos[i, j].Y;
                    if (IsValid(x, y))
                    {
                        //si esta viva la casilla contamos y tambien tiene que ser dif a la casilla actual
                        if (grid[x, y] == true && (x != row || y != cell))
                        {
                            cellAlive++;
                        }
                    }
                }
            }
            return cellAlive;
        }

        public struct Vecino
        {
            private int x;
            private int y;
            public Vecino(int X, int Y)
            {
                x = X;
                y = Y;
            }

            public int X { get => x; set => x = value; }
            public int Y { get => y; set => y = value; }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            EndGame();
            timer1.Enabled = false;
        }

        void EndGame()
        {
            for (int i = 0; i < SIZE; i++)
                for (int j = 0; j < SIZE; j++)
                {
                    grid[i, j] = false;
                }
            DrawGrid();
        }
        
        private void Pause()
        {
            timer1.Enabled = !timer1.Enabled; 
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            StartGame();
        }

        private void button3_Click(object sender, EventArgs e)
        {
            Pause();
        }
    }
}
