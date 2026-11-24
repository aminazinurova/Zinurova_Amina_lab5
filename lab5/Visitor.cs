using System;

namespace MuseumApp
{
    internal class Visitor
    {
        private int _id;
        private string _name;
        private int _age;
        private string _city;

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
        public string Name 
        { 
            get 
            { 
                return _name; 
            } 
            set 
            { 
                _name = value; 
            } 
        }
        public int Age 
        { get 
            { 
                return _age; 
            }
            set 
            { _age = value; 
            } 
        }
        public string City 
        { 
            get 
            { 
                return _city; 
            } 
            set 
            { 
                _city = value;
            } 
        }

        public Visitor(int id, string name, int age, string city)
        {
            _id = id;
            _name = name;
            _age = age;
            _city = city;
        }

        public override string ToString()
        {
            return $"ID: {_id} | Имя: {_name} | Возраст: {_age} | Город: {_city}";
        }
    }
}