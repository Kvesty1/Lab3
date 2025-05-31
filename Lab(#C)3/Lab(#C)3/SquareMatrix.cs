using System;
using System.Text;

namespace MatrixCalculator
{
    public class SquareMatrix
    {
        private int[,] data;
        public int Size { get; private set; }
        private static Random rand = new Random();

        public SquareMatrix(int rows, int columns)
        {
            // С точки зрения безопасности, должна быть проверка данных, которые присылаются из другого файла.
            // Даже если проверка изначально велась перед отправкой. Иначе возможна бесконечная рекурсия.
            if (rows <= 0)
                throw new ArgumentException("Количество строк должно быть положительным числом", nameof(rows));
            if (columns <= 0)
                throw new ArgumentException("Количество столбцов должно быть положительным числом", nameof(columns));
            if (rows != columns)
                throw new ArgumentException($"Матрица должна быть квадратной. Получен размер: {rows}x{columns}");
            //

            Size = rows;
            data = new int[rows, columns];
            FillRandom();
        }

        // Конструктор для внутреннего использования
        private SquareMatrix(int size)
        {
            Size = size;
            data = new int[size, size];
        }

        private void FillRandom()
        {
            for (int i = 0; i < Size; i++)
                for (int j = 0; j < Size; j++)
                    data[i, j] = rand.Next(1, 101); // от 1 до 100
        }

        public void Print(string title = "")
        {
            if (!string.IsNullOrEmpty(title))
                Console.WriteLine(title);
            else
                Console.WriteLine($"\nМатрица {Size}x{Size}:");

            for (int i = 0; i < Size; i++)
            {
                for (int j = 0; j < Size; j++)
                    Console.Write($"{data[i, j],4}\t");
                Console.WriteLine();
            }
        }

        // Перегрузка оператора +
        public static SquareMatrix operator +(SquareMatrix a, SquareMatrix b)
        {
            if (a.Size != b.Size)
                throw new MatrixSizeMismatchException("Матрицы должны быть одного размера для сложения");

            SquareMatrix result = new SquareMatrix(a.Size);
            for (int i = 0; i < a.Size; i++)
                for (int j = 0; j < a.Size; j++)
                    result.data[i, j] = a.data[i, j] + b.data[i, j];
            return result;
        }

        // Перегрузка оператора *
        public static SquareMatrix operator *(SquareMatrix a, SquareMatrix b)
        {
            if (a.Size != b.Size)
                throw new MatrixSizeMismatchException("Матрицы должны быть одного размера для умножения");

            SquareMatrix result = new SquareMatrix(a.Size);
            for (int i = 0; i < a.Size; i++)
                for (int j = 0; j < a.Size; j++)
                    for (int k = 0; k < a.Size; k++)
                        result.data[i, j] += a.data[i, k] * b.data[k, j];
            return result;
        }

        // Перегрузка операторов сравнения (по детерминанту)
        public static bool operator >(SquareMatrix a, SquareMatrix b) => a.Determinant() > b.Determinant();
        public static bool operator <(SquareMatrix a, SquareMatrix b) => a.Determinant() < b.Determinant();
        public static bool operator >=(SquareMatrix a, SquareMatrix b) => a.Determinant() >= b.Determinant();
        public static bool operator <=(SquareMatrix a, SquareMatrix b) => a.Determinant() <= b.Determinant();

        // Перегрузка операторов равенства
        public static bool operator ==(SquareMatrix a, SquareMatrix b)
        {
            if (ReferenceEquals(a, b)) return true;
            if (a is null || b is null) return false;
            if (a.Size != b.Size) return false;

            for (int i = 0; i < a.Size; i++)
                for (int j = 0; j < a.Size; j++)
                    if (a.data[i, j] != b.data[i, j])
                        return false;
            return true;
        }

        public static bool operator !=(SquareMatrix a, SquareMatrix b) => !(a == b);

        // Перегрузка приведения к double (возвращает детерминант)
        public static explicit operator double(SquareMatrix m) => m.Determinant();

        // Перегрузка true/false (невырожденная матрица)
        public static bool operator true(SquareMatrix m) => Math.Abs(m.Determinant()) > 0.0001;
        public static bool operator false(SquareMatrix m) => Math.Abs(m.Determinant()) < 0.0001;

        // Вычисление определителя
        public double Determinant()
        {
            if (Size == 1) return data[0, 0];
            if (Size == 2) return data[0, 0] * data[1, 1] - data[0, 1] * data[1, 0];

            double det = 0;
            for (int j = 0; j < Size; j++)
            {
                det += (j % 2 == 0 ? 1 : -1) * data[0, j] * GetMinor(0, j).Determinant();
            }
            return det;
        }

        // Получение минора
        private SquareMatrix GetMinor(int row, int col)
        {
            SquareMatrix minor = new SquareMatrix(Size - 1);
            for (int i = 0, mi = 0; i < Size; i++)
            {
                if (i == row) continue;
                for (int j = 0, mj = 0; j < Size; j++)
                {
                    if (j == col) continue;
                    minor.data[mi, mj] = data[i, j];
                    mj++;
                }
                mi++;
            }
            return minor;
        }

        // Обратная матрица
        public SquareMatrix Inverse()
        {
            double det = Determinant();
            if (Math.Abs(det) < 0.0001)
                throw new SingularMatrixException("Матрица вырождена, обратной матрицы не существует");

            SquareMatrix adjugate = new SquareMatrix(Size);
            for (int i = 0; i < Size; i++)
            {
                for (int j = 0; j < Size; j++)
                {
                    double sign = (i + j) % 2 == 0 ? 1 : -1;
                    double cofactor = sign * GetMinor(i, j).Determinant();
                    adjugate.data[j, i] = (int)Math.Round(cofactor / det * 100); // Масштабируем для целочисленного представления
                }
            }
            return adjugate;
        }

        // Метод ToString()
        public override string ToString()
        {
            StringBuilder sb = new StringBuilder();
            sb.AppendLine($"Матрица {Size}x{Size}:");
            for (int i = 0; i < Size; i++)
            {
                for (int j = 0; j < Size; j++)
                    sb.Append($"{data[i, j],4}");
                sb.AppendLine();
            }
            return sb.ToString();
        }

        // Метод CompareTo()
        public int CompareTo(SquareMatrix other) => Size.CompareTo(other?.Size ?? -1);

        // Метод Equals()
        public override bool Equals(object obj) => this == (obj as SquareMatrix);

        // Метод GetHashCode()
        public override int GetHashCode()
        {
            int hash = 17;
            for (int i = 0; i < Size; i++)
                for (int j = 0; j < Size; j++)
                    hash = hash * 23 + data[i, j].GetHashCode();
            return hash;
        }

        // Реализация паттерна "Прототип"
        public SquareMatrix Clone()
        {
            SquareMatrix clone = new SquareMatrix(Size);
            for (int i = 0; i < Size; i++)
                for (int j = 0; j < Size; j++)
                    clone.data[i, j] = data[i, j];
            return clone;
        }
    }

    // Пользовательские исключения
    public class MatrixSizeMismatchException : Exception
    {
        public MatrixSizeMismatchException(string message) : base(message) { }
    }

    public class SingularMatrixException : Exception
    {
        public SingularMatrixException(string message) : base(message) { }
    }
}