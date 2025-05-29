using System;

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

                // Получаем и проверяем строки
                int rows;
                while (true)
                {
                    Console.Write("Введите количество строк: ");
                    if (int.TryParse(Console.ReadLine(), out rows) && rows > 0)
                        break;
                    Console.WriteLine("Ошибка: количество строк должно быть положительным целым числом!");
                }

                // Получаем и проверяем столбцы
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
                Console.WriteLine("0. Вернуться в главное меню");
                Console.Write("Выберите пункт: ");

                string input = Console.ReadLine();

                switch (input)
                {
                    case "0":
                        return;
                    default:
                        Console.WriteLine("Неверный ввод!");
                        break;
                }
            }
        }
    }
}
