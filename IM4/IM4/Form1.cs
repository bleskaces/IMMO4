﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;


namespace IM4
{
    public partial class Form1 : Form
    {
        private int currentGeneration = 0;
        private Graphics graphics;
        private int resolution;
        private bool[,] field;
        private int rows; //строки
        private int cols; //колонки

        public Form1()
        {
            InitializeComponent();
        }


        private void StartGame()
        {
            if (timer1.Enabled)
                return;

            currentGeneration = 0;
            Text = $"Generation {currentGeneration}";

            nudResolution.Enabled = false;
            nudDensity.Enabled = false;
            resolution = (int)nudResolution.Value;
            rows = pictureBox1.Height / resolution;
            cols = pictureBox1.Width / resolution;
            field = new bool[cols, rows];

            Random random = new Random();
            for (int x = 0; x < cols; x++) //генерация клетки
            {
                for (int y = 0; y < rows; y++)
                {
                    field[x, y] = random.Next((int)nudDensity.Value) == 0;
                }
            }

            pictureBox1.Image = new Bitmap(pictureBox1.Width, pictureBox1.Height);
            graphics = Graphics.FromImage(pictureBox1.Image);
            timer1.Start();
        }

        private void NextGeneration()
        {
            graphics.Clear(Color.White);
            var newField = new bool[cols, rows];    
            for (int x = 0; x < cols; x++)
            {
                for (int y = 0; y < rows; y++)
                {
                    var neighboursCount = CountNeighbours(x, y);
                    var hasLife = field[x, y]; // есть ли живая клетка
                    
                    if (!hasLife && neighboursCount == 3) //нет жизни и соседи 3
                    {
                        newField[x, y] = true; 
                    }
                    else if (hasLife && (neighboursCount < 2 || neighboursCount > 3)) //погибает
                    {
                        newField[x,y] = false;    
                    }
                    else
                    {
                        newField[x, y] = field[x, y];   
                    }
                    if (hasLife)
                    {
                        graphics.FillRectangle(Brushes.Yellow, x * resolution, y * resolution, resolution - 1, resolution - 1);
                    }
                }
            }
            field = newField;
            pictureBox1.Refresh();
            Text = $"Generation {++currentGeneration}";
        }
    
             
    
        private int CountNeighbours(int x, int y)
        {
            int count = 0;
            for (int i = -1; i < 2; i++)
            {
                for (int j = -1; j < 2; j++)
                {
                    var col = (x + i + cols) % cols; // чтобы не выйти за границы
                    var row = (y + j + rows) % rows;
                    var isSelfChecking = col == x && row == y; // проверка саму себя
                    var hasLife = field[col, row];
                    if (hasLife && !isSelfChecking) //подсчет живых соседей
                        count++;
                }
            }
            return count;
        }
        private void StopGame()
        {
            if (!timer1.Enabled)
                return;
            timer1.Stop();
            nudResolution.Enabled = true;
            nudDensity.Enabled = true;
        }
        private void timer1_Tick(object sender, EventArgs e)
        {
            NextGeneration();
        }

        private void bStart_Click(object sender, EventArgs e)
        {

            StartGame();
        }

        private void bStop_Click(object sender, EventArgs e)
        {
            StopGame();
        }
    }
}
