using System;
namespace MuseumApp
{
    internal class Checked
    {
        // Проверка на int
        public static int ReadInt(string prompt)
        {
            Console.Write(prompt);
            while (true)
            {
                string input = Console.ReadLine();
                if (int.TryParse(input, out int x))
                {
                    return x;
                }
                Console.Write("Ошибка. Введите ещё раз: ");
            }
        }

        // Проверка на положительное целое число (для ID и возраста)
        public static int ReadPositiveInt(string prompt)
        {
            Console.Write(prompt);
            while (true)
            {
                string input = Console.ReadLine();
                if (int.TryParse(input, out int x) && x > 0)
                {
                    return x;
                }
                Console.Write("Ошибка. Введите положительное число: ");
            }
        }

        // Проверка на непустую строку (для названий, эпох, имен, городов)
        public static string ReadString(string prompt)
        {
            Console.Write(prompt);
            while (true)
            {
                string input = Console.ReadLine();
                if (!string.IsNullOrWhiteSpace(input))
                {
                    return input.Trim();
                }
                Console.Write("Ошибка. Строка не может быть пустой: ");
            }
        }

        // Проверка на дату (для билетов)
        public static DateTime ReadDate(string prompt)
        {
            Console.Write(prompt);
            while (true)
            {
                string input = Console.ReadLine();
                if (DateTime.TryParse(input, out DateTime date))
                {
                    return date;
                }
                Console.Write("Ошибка. Введите дату в формате дд.мм.гггг: ");
            }
        }

        // Проверка на положительную стоимость (для билетов)
        public static decimal ReadPositiveDecimal(string prompt)
        {
            Console.Write(prompt);
            while (true)
            {
                string input = Console.ReadLine();
                if (decimal.TryParse(input, out decimal price) && price >= 0)
                {
                    return price;
                }
                Console.Write("Ошибка. Введите корректную сумму: ");
            }
        }
    }
}
