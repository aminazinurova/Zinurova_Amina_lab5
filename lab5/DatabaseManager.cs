using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using ExcelDataReader;
using ClosedXML.Excel;
namespace MuseumApp
{
    internal class DatabaseManager
    {
        public List<Exhibit> Exhibits { get; } = new List<Exhibit>();
        public List<Visitor> Visitors { get; } = new List<Visitor>();
        public List<Ticket> Tickets { get; } = new List<Ticket>();
        private string _filePath;

        public DatabaseManager(string filePath)
        {
            _filePath = filePath;
            System.Text.Encoding.RegisterProvider(System.Text.CodePagesEncodingProvider.Instance);
        }
        public void LoadData()
        {
            if (!File.Exists(_filePath)) throw new FileNotFoundException("Файл не найден!");

            using (var stream = File.Open(_filePath, FileMode.Open, FileAccess.Read))
            using (var reader = ExcelReaderFactory.CreateReader(stream))
            {
                var dataSet = reader.AsDataSet(new ExcelDataSetConfiguration()
                {
                    ConfigureDataTable = (_) => new ExcelDataTableConfiguration() { UseHeaderRow = true }
                });

                Exhibits.Clear();
                foreach (System.Data.DataRow row in dataSet.Tables["Экспонаты"].Rows)
                {
                    Exhibits.Add(new Exhibit(Convert.ToInt32(row[0]), row[1].ToString(), row[2].ToString()));
                }
                Visitors.Clear();
                foreach (System.Data.DataRow row in dataSet.Tables["Посетители"].Rows)
                {
                    Visitors.Add(new Visitor(
                        Convert.ToInt32(row[0]),
                        row[1].ToString(),
                        Convert.ToInt32(row[2]),
                        row[3].ToString()
                    ));
                }
                Tickets.Clear();
                foreach (System.Data.DataRow row in dataSet.Tables["Билеты"].Rows)
                {
                    string priceStr = row[4].ToString().Replace("р.", "").Replace(" ", "").Trim();
                    Tickets.Add(new Ticket(
                        Convert.ToInt32(row[0]),
                        Convert.ToInt32(row[1]),
                        Convert.ToInt32(row[2]),
                        DateTime.Parse(row[3].ToString(), new CultureInfo("ru-RU")),
                        Convert.ToDecimal(priceStr)
                    ));
                }
            }
        }
        public void PrintAll()
        {
            Console.WriteLine("\nЭКСПОНАТЫ");
            Exhibits.ForEach(Console.WriteLine);

            Console.WriteLine("\nПОСЕТИТЕЛИ (первые 20)");
            Visitors.Take(20).ToList().ForEach(Console.WriteLine);

            Console.WriteLine("\nБИЛЕТЫ (первые 20)");
            Tickets.Take(20).ToList().ForEach(Console.WriteLine);
        }
        public void DeleteExhibit(int id)
        {
            var ex = Exhibits.FirstOrDefault(e => e.Id == id);
            if (ex != null)
            {
                Exhibits.Remove(ex);
                Tickets.RemoveAll(t => t.ExhibitId == id);
                Console.WriteLine("Экспонат и связанные билеты удалены");
            }
            else
            {
                Console.WriteLine("Экспонат с таким ID не найден");
            }
        }
        public void AddExhibit(Exhibit newExhibit)
        {
            if (Exhibits.Any(e => e.Id == newExhibit.Id))
            {
                Console.WriteLine("Ошибка.Экспонат с таким ID уже существует");
                return;
            }
            Exhibits.Add(newExhibit);
            Console.WriteLine("Экспонат успешно добавлен");
        }
        public void AddVisitor(Visitor newVisitor)
        {
            if (Visitors.Any(v => v.Id == newVisitor.Id))
            {
                Console.WriteLine("Ошибка.Посетитель с таким ID уже существует");
                return;
            }
            Visitors.Add(newVisitor);
            Console.WriteLine("Посетитель успешно добавлен");
        }
        public void AddTicket(Ticket newTicket)
        {
            if (Tickets.Any(t => t.Id == newTicket.Id))
            {
                Console.WriteLine("Ошибка.Билет с таким ID уже существует");
                return;
            }
            if (!Exhibits.Any(e => e.Id == newTicket.ExhibitId))
            {
                Console.WriteLine("Ошибка.Экспоната с таким ID не существует");
                return;
            }
            if (!Visitors.Any(v => v.Id == newTicket.VisitorId))
            {
                Console.WriteLine("Ошибка.Посетителя с таким ID не существует");
                return;
            }
            Tickets.Add(newTicket);
            Console.WriteLine("Билет успешно добавлен");
        }
        public void SaveData()
        {
            using (var workbook = new XLWorkbook())
            {
                 var wsEx = workbook.Worksheets.Add("Экспонаты");
                wsEx.Cell(1, 1).Value = "ID экспоната";
                wsEx.Cell(1, 2).Value = "Название";
                wsEx.Cell(1, 3).Value = "Эпоха";
                for (int i = 0; i < Exhibits.Count; i++)
                {
                    wsEx.Cell(i + 2, 1).Value = Exhibits[i].Id;
                    wsEx.Cell(i + 2, 2).Value = Exhibits[i].Name;
                    wsEx.Cell(i + 2, 3).Value = Exhibits[i].Era;
                }
                var wsVis = workbook.Worksheets.Add("Посетители");
                wsVis.Cell(1, 1).Value = "ID посетителя";
                wsVis.Cell(1, 2).Value = "Имя";
                wsVis.Cell(1, 3).Value = "Возраст";
                wsVis.Cell(1, 4).Value = "Город проживания"; 
                for (int i = 0; i < Visitors.Count; i++)
                {
                    wsVis.Cell(i + 2, 1).Value = Visitors[i].Id;
                    wsVis.Cell(i + 2, 2).Value = Visitors[i].Name;
                    wsVis.Cell(i + 2, 3).Value = Visitors[i].Age;
                    wsVis.Cell(i + 2, 4).Value = Visitors[i].City;
                }
                var wsTic = workbook.Worksheets.Add("Билеты");
                wsTic.Cell(1, 1).Value = "ID билета";
                wsTic.Cell(1, 2).Value = "ID экспоната";
                wsTic.Cell(1, 3).Value = "ID посетителя";
                wsTic.Cell(1, 4).Value = "Дата";
                wsTic.Cell(1, 5).Value = "Стоимость";
                for (int i = 0; i < Tickets.Count; i++)
                {
                    wsTic.Cell(i + 2, 1).Value = Tickets[i].Id;
                    wsTic.Cell(i + 2, 2).Value = Tickets[i].ExhibitId;
                    wsTic.Cell(i + 2, 3).Value = Tickets[i].VisitorId;
                    wsTic.Cell(i + 2, 4).Value = Tickets[i].Date;
                    wsTic.Cell(i + 2, 4).Style.DateFormat.Format = "dd.MM.yyyy";
                    wsTic.Cell(i + 2, 5).Value = Tickets[i].Price;
                }
                string newFilePath = _filePath.Replace(".xlsx", "_result.xlsx");
                workbook.SaveAs(newFilePath);
                Console.WriteLine($"Данные сохранены в НОВЫЙ файл: {newFilePath}");
            }
        }
        public int Query1_OneTable(string city)
        {
            var query = from v in Visitors
                        where v.City.ToLower() == city.ToLower()
                        select v;
            return query.Count();
        }
        public List<string> Query2_TwoTables()
        {
            var query = from t in Tickets
                        join e in Exhibits on t.ExhibitId equals e.Id
                        select $"Экспонат: {e.Name} | Цена билета: {t.Price} руб.";
            return query.Take(10).ToList();
        }
        public decimal Query3_ThreeTables_Sum(string era)
        {
            var query = from t in Tickets
                        join e in Exhibits on t.ExhibitId equals e.Id
                        join v in Visitors on t.VisitorId equals v.Id
                        where e.Era.ToLower() == era.ToLower() && t.Date.Year == 2023 && t.Date.Month == 6
                        select t.Price;
            return query.Sum();
        }
        public List<string> Query4_ThreeTables_List(string era)
        {
            var query = from t in Tickets
                        join e in Exhibits on t.ExhibitId equals e.Id
                        join v in Visitors on t.VisitorId equals v.Id
                        where e.Era.ToLower() == era.ToLower()
                        select $"Посетитель: {v.Name} (Город: {v.City}) купил билет на {e.Name}";
            return query.Distinct().Take(10).ToList();
        }
    }
}