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
                string choice = Checked.ReadString("Выбор: ");

                switch (choice)
                {
                    case "1": db.PrintAll(); break;
                    case "2": AddUI(db); break;
                    case "3": DeleteUI(db); break;
                    case "4": RunQueries(db); break;
                    case "5": db.SaveData(); break;
                    case "0": return;
                    default: Console.WriteLine("Неверный ввод."); break;
                }
            }
        }

        static void AddUI(DatabaseManager db)
        {
            Console.WriteLine("\nЧТО ДОБАВИТЬ?");
            Console.WriteLine("1. Экспонат");
            Console.WriteLine("2. Посетителя");
            Console.WriteLine("3. Билет");
            string choice = Checked.ReadString("Выбор: ");

            switch (choice)
            {
                case "1":
                    Console.WriteLine("\nДОБАВЛЕНИЕ ЭКСПОНАТА");
                    int id = Checked.ReadPositiveInt("ID: ");
                    string name = Checked.ReadString("Название: ");
                    string era = Checked.ReadString("Эпоха: ");
                    db.AddExhibit(new Exhibit(id, name, era));
                    break;

                case "2":
                    Console.WriteLine("\nДОБАВЛЕНИЕ ПОСЕТИТЕЛЯ");
                    int vId = Checked.ReadPositiveInt("ID: ");
                    string vName = Checked.ReadString("Имя: ");
                    int age = Checked.ReadPositiveInt("Возраст: ");
                    string city = Checked.ReadString("Город: ");
                    db.AddVisitor(new Visitor(vId, vName, age, city));
                    break;

                case "3":
                    Console.WriteLine("\nДОБАВЛЕНИЕ БИЛЕТА");
                    int tId = Checked.ReadPositiveInt("ID билета: ");
                    int exId = Checked.ReadPositiveInt("ID экспоната: ");
                    int visId = Checked.ReadPositiveInt("ID посетителя: ");
                    DateTime date = Checked.ReadDate("Дата (дд.мм.гггг): ");
                    decimal price = Checked.ReadPositiveDecimal("Стоимость: ");
                    db.AddTicket(new Ticket(tId, exId, visId, date, price));
                    break;

                default:
                    Console.WriteLine("Неверный ввод.");
                    break;
            }
        }

        static void DeleteUI(DatabaseManager db)
        {
            Console.WriteLine("\nУДАЛЕНИЕ ЭКСПОНАТА");
            int id = Checked.ReadPositiveInt("Введите ID для удаления: ");
            db.DeleteExhibit(id);
        }

        static void RunQueries(DatabaseManager db)
        {
            string city = Checked.ReadString("\nВведите город для поиска посетителей (например, Obrienberg): ");
            int count = db.Query1_OneTable(city);
            Console.WriteLine($"\nЗАПРОС 1 (одно значение)");
            Console.WriteLine($"Количество посетителей из города {city}: {count}");

            Console.WriteLine("\nЗАПРОС 2 (перечень, 2 таблицы)");
            Console.WriteLine("Экспонаты и цены билетов (первые 10):");
            foreach (var s in db.Query2_TwoTables()) Console.WriteLine(s);

            string era = Checked.ReadString("\nВведите эпоху для подсчета выручки и поиска (например, Ренессанс): ");

            Console.WriteLine($"\nЗАПРОС 3 (одно значение, 3 таблицы)");
            Console.WriteLine($"Выручка ({era}, июнь 2023): {db.Query3_ThreeTables_Sum(era):F2} руб.");

            Console.WriteLine($"\nЗАПРОС 4 (перечень, 3 таблицы)");
            Console.WriteLine($"Кто покупал билеты на {era} (первые 10):");
            foreach (var s in db.Query4_ThreeTables_List(era)) Console.WriteLine(s);
        }
    }
}
