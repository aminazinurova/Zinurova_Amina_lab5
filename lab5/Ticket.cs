using System;
namespace MuseumApp
{
    internal class Ticket
    {
        private int _id;
        private int _exhibitId;
        private int _visitorId;
        private DateTime _date;
        private decimal _price;

        public int Id 
        { 
            get 
            { 
                return _id;
            } 
            set 
            { 
                _id = value;
            }
        }
        public int ExhibitId 
        { 
            get 
            {
                return _exhibitId; 
            } 
            set 
            { 
                _exhibitId = value;
            } 
        }
        public int VisitorId 
        { 
            get 
            { 
                return _visitorId; 
            } 
            set 
            { 
                _visitorId = value;
            } 
        }
        public DateTime Date
        { get 
            { 
                return _date; 
            } 
            set 
            { 
                _date = value;
            } 
        }
        public decimal Price 
        { 
            get 
            { 
                return _price;
            } 
            set 
            { 
                _price = value;
            } 
        }
        public Ticket(int id, int exhibitId, int visitorId, DateTime date, decimal price)
        {
            _id = id;
            _exhibitId = exhibitId;
            _visitorId = visitorId;
            _date = date;
            _price = price;
        }

        public override string ToString()
        {
            return $"ID: {_id} | ID экспоната: {_exhibitId} | ID посетителя: {_visitorId} | Дата: {_date:dd.MM.yyyy} | Цена: {_price}";
        }
    }
}