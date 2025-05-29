using System;

namespace MatrixCalculator
{
    public class SquareMatrix
    {
        private int[,] data;
        public int Size { get; private set; }
        private static Random rand = new Random();

        public SquareMatrix(int rows, int columns)
        {
            Size = rows;
            data = new int[rows, columns];
            FillRandom();
        }

        private void FillRandom()
        {
            for (int i = 0; i < Size; i++)
                for (int j = 0; j < Size; j++)
                    data[i, j] = rand.Next(1, 101); // от 1 до 100
        }

        public void Print()
        {
            Console.WriteLine($"\nМатрица {Size}x{Size}:");
            for (int i = 0; i < Size; i++)
            {
                for (int j = 0; j < Size; j++)
                    Console.Write($"{data[i, j],4}");
                Console.WriteLine();
            }
        }
       
    }
}
