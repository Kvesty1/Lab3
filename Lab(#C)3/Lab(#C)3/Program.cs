using System;
using System.Numerics;

namespace MatrixCalculator
{
    class Program
    {
        static void Main(string[] args)
        {
            while (true)
            {
                Console.Clear();
                Console.WriteLine("=== Матричный калькулятор ===");
                Console.WriteLine("1. Создать квадратные матрицы");
                Console.WriteLine("0. Выход");
                Console.Write("Выберите пункт: ");

                string input = Console.ReadLine();

                switch (input)
                {
                    case "1":
                        CreateMatrices();
                        break;
                    case "0":
                        Console.WriteLine("Выход из программы...");
                        return;
                    default:
                        Console.WriteLine("Неверный ввод. Нажмите любую клавишу...");
                        Console.ReadKey();
                        break;
                }
            }
        }

        private static void CreateMatrices()
        {
            try
            {
                Console.Clear();
                Console.WriteLine("--- Создание двух случайных квадратных матриц ---");

                // Указываем и проверяем строки
                int rows;
                while (true)
                {
                    Console.Write("Введите количество строк: ");
                    if (int.TryParse(Console.ReadLine(), out rows) && rows > 0)
                        break;
                    Console.WriteLine("Ошибка: количество строк должно быть положительным целым числом!");
                }

                // Указываем и проверяем столбцы
                int columns;
                while (true)
                {
                    Console.Write("Введите количество столбцов: ");
                    if (int.TryParse(Console.ReadLine(), out columns) && columns > 0)
                        break;
                    Console.WriteLine("Ошибка: количество столбцов должно быть положительным целым числом!");
                }

                // Дополнительная проверка на квадратность
                if (rows != columns)
                {
                    Console.WriteLine("Ошибка: матрица должна быть квадратной! Автокоррекция: размер изменен на {0}x{0}", Math.Max(rows, columns));
                    rows = columns = Math.Max(rows, columns);
                }

                // Создаем матрицы после всех проверок
                var matrix1 = new SquareMatrix(rows, columns);
                var matrix2 = new SquareMatrix(rows, columns);

                Console.WriteLine("\nМатрицы успешно созданы!");

                // Показываем созданные матрицы
                matrix1.Print();
                matrix2.Print();

                Console.WriteLine("\nНажмите любую клавишу для продолжения...");
                Console.ReadKey();

                // Переходим в меню операций
                MatrixOperationsMenu(matrix1, matrix2);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"\nКритическая ошибка: {ex.Message}");
                Console.WriteLine("Нажмите любую клавишу для возврата в меню...");
                Console.ReadKey();
            }
        }

        static void MatrixOperationsMenu(SquareMatrix matrix1, SquareMatrix matrix2)
        {
            while (true)
            {
                Console.Clear();
                Console.WriteLine("=== Работа с матрицами ===");
                Console.WriteLine("1. Показать обе матрицы");
                Console.WriteLine("2. Сложить матрицы");
                Console.WriteLine("3. Умножить матрицы");
                Console.WriteLine("4. Сравнить матрицы");
                Console.WriteLine("5. Найти определитель матриц");
                Console.WriteLine("6. Найти обратную матрицу");
                Console.WriteLine("7. Клонировать матрицу");
                Console.WriteLine("0. Вернуться в главное меню");
                Console.Write("Выберите пункт: ");

                string input = Console.ReadLine();

                try
                {
                    switch (input)
                    {
                        case "1":
                            matrix1.Print("\n=== Матрица 1 ===");
                            matrix2.Print("\n=== Матрица 2 ===");
                            break;
                        case "2":
                            var sum = matrix1 + matrix2;
                            sum.Print("\n=== Сумма матриц ===");
                            break;
                        case "3":
                            var product = matrix1 * matrix2;
                            product.Print("\n=== Произведение матриц ===");
                            break;
                        case "4":
                            Console.WriteLine($"\nmatrix1 > matrix2: {matrix1 > matrix2}");
                            Console.WriteLine($"matrix1 < matrix2: {matrix1 < matrix2}");
                            Console.WriteLine($"matrix1 == matrix2: {matrix1 == matrix2}");
                            Console.WriteLine($"matrix1 != matrix2: {matrix1 != matrix2}");
                            break;
                        case "5":
                            Console.WriteLine($"\nОпределитель матрицы 1: {matrix1.Determinant():F2}");
                            Console.WriteLine($"Определитель матрицы 2: {matrix2.Determinant():F2}");
                            break;
                        case "6":
                            var inv1 = matrix1.Inverse();
                            inv1.Print("\n=== Обратная матрица 1 ===");

                            var inv2 = matrix2.Inverse();
                            inv2.Print("\n=== Обратная матрица 2 ===");
                            break;
                        case "7":
                            var clone1 = matrix1.Clone();
                            var clone2 = matrix2.Clone();
                            clone1.Print("\n=== Клон матрицы 1 ===");
                            clone2.Print("\n=== Клон матрицы 2 ===");
                            Console.WriteLine("Матрицы успешно клонированы!");
                            break;
                        case "0":
                            return;
                        default:
                            Console.WriteLine("Неверный ввод!");
                            break;
                    }
                }
                catch (MatrixSizeMismatchException ex)
                {
                    Console.WriteLine($"Ошибка размеров: {ex.Message}");
                }
                catch (SingularMatrixException ex)
                {
                    Console.WriteLine($"Ошибка операции: {ex.Message}");
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Непредвиденная ошибка: {ex.Message}");
                }

                Console.WriteLine("\nНажмите любую клавишу для продолжения...");
                Console.ReadKey();
            }
        }
    }
}