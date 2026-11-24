using System;
namespace MuseumApp
{
    internal class Program
    {
        static void Main(string[] args)
        {
            string filePath = "LR5-var3.xlsx";
            DatabaseManager db = new DatabaseManager(filePath);

            try
            {
                db.LoadData();
                Console.WriteLine("База данных успешно загружена!");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Ошибка загрузки: {ex.Message}");
                return;
            }
            while (true)
            {
                Console.WriteLine("\nМЕНЮ");
                Console.WriteLine("1. Просмотр базы");
                Console.WriteLine("2. Добавить запись");
                Console.WriteLine("3. Удалить экспонат по ID");
                Console.WriteLine("4. Выполнить 4 LINQ-запроса");
                Console.WriteLine("5. Сохранить в Excel");
                Console.WriteLine("0. Выход");
                Console.Write("Выбор: ");
                string choice = Console.ReadLine();
                switch (choice)
                {
                    case "1": db.PrintAll(); break;
                    case "2": AddUI(db); break;
                    case "3": DeleteUI(db); break;
                    case "4": RunQueries(db); break;
                    case "5": db.SaveData(); break;
                    case "0": return;
                    default: Console.WriteLine("Неверный ввод"); break;
                }
            }
        }
        static int ReadInt(string prompt)
        {
            while (true)
            {
                Console.Write(prompt);
                if (int.TryParse(Console.ReadLine(), out int res)) return res;
                Console.WriteLine("Ошибка: нужно ввести целое число");
            }
        }
        static void AddUI(DatabaseManager db)
        {
            Console.WriteLine("\nЧТО ДОБАВИТЬ?");
            Console.WriteLine("1.Экспонат");
            Console.WriteLine("2.Посетителя");
            Console.WriteLine("3.Билет");
            Console.Write("Выбор: ");
            string choice = Console.ReadLine();
            switch (choice)
            {
                case "1":
                    Console.WriteLine("\nДОБАВЛЕНИЕ ЭКСПОНАТА");
                    int id = ReadInt("ID: ");
                    Console.Write("Название: "); string name = Console.ReadLine();
                    Console.Write("Эпоха: "); string era = Console.ReadLine();
                    db.AddExhibit(new Exhibit(id, name, era));
                    break;
                case "2":
                    Console.WriteLine("\nДОБАВЛЕНИЕ ПОСЕТИТЕЛЯ");
                    int vId = ReadInt("ID: ");
                    Console.Write("Имя: "); string vName = Console.ReadLine();
                    int age = ReadInt("Возраст: ");
                    Console.Write("Город: "); string city = Console.ReadLine();
                    db.AddVisitor(new Visitor(vId, vName, age, city));
                    break;
                case "3":
                    Console.WriteLine("\nДОБАВЛЕНИЕ БИЛЕТА");
                    int tId = ReadInt("ID билета: ");
                    int exId = ReadInt("ID экспоната: ");
                    int visId = ReadInt("ID посетителя: ");

                    DateTime date;
                    while (true)
                    {
                        Console.Write("Дата (дд.мм.гггг): ");
                        if (DateTime.TryParse(Console.ReadLine(), out date)) break;
                        Console.WriteLine("Ошибка: введите дату в формате дд.мм.гггг.");
                    }
                    decimal price;
                    while (true)
                    {
                        Console.Write("Стоимость: ");
                        if (decimal.TryParse(Console.ReadLine(), out price) && price >= 0) break;
                        Console.WriteLine("Ошибка: введите корректную сумму");
                    }

                    db.AddTicket(new Ticket(tId, exId, visId, date, price));
                    break;
                default:
                    Console.WriteLine("Неверный ввод");
                    break;
            }
        }
        static void DeleteUI(DatabaseManager db)
        {
            Console.WriteLine("\nУДАЛЕНИЕ ЭКСПОНАТА");
            int id = ReadInt("Введите ID для удаления: ");
            db.DeleteExhibit(id);
        }
        static void RunQueries(DatabaseManager db)
        {
            Console.Write("\nВведите город для поиска посетителей: ");
            string city = Console.ReadLine();
            int count = db.Query1_OneTable(city);
            Console.WriteLine($"\nЗАПРОС 1 (одно значение)");
            Console.WriteLine($"Количество посетителей из города {city}: {count}");

            Console.WriteLine("\nЗАПРОС 2 (перечень, 2 таблицы)");
            Console.WriteLine("Экспонаты и цены билетов (первые 10):");
            foreach (var s in db.Query2_TwoTables()) Console.WriteLine(s);
            Console.Write("\nВведите эпоху для подсчета выручки и поиска: ");
            string era = Console.ReadLine();

            Console.WriteLine($"\nЗАПРОС 3 (одно значение, 3 таблицы)");
            Console.WriteLine($"Выручка ({era}, июнь 2023): {db.Query3_ThreeTables_Sum(era):F2} руб.");

            Console.WriteLine($"\nЗАПРОС 4 (перечень, 3 таблицы)");
            Console.WriteLine($"Кто покупал билеты на {era} (первые 10):");
            foreach (var s in db.Query4_ThreeTables_List(era)) Console.WriteLine(s);
        }
    }
};
        
    
